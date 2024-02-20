using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAQFunctionGenerator
{
    internal class FunctionGenerator
    {
        public static double MAX_AD_RATE = 833000.0; // from daq specs
        public static int MAX_SAMPLE_COUNT = 8191; // from daq specs

        private static NationalInstruments.DAQmx.Task AOTask =
            new NationalInstruments.DAQmx.Task();
        private static AnalogSingleChannelWriter writer =
            new AnalogSingleChannelWriter(AOTask.Stream);

        public string Device { get; set; }
        public double Amplitude { get; set; }
        public double DCOffset { get; set; }
        private double dutyCycle;
        public double DutyCycle
        {
            get { return dutyCycle; } 
            set { dutyCycle = CalcActualDutyCycle(value); }
        }
        public WaveShape WaveShape { get; set; }
        public int Frequency { get; set; }
        public bool On { get; set; }

        // Read-only waveform setup values set by GenerateWaveform
        private int SampleCount;
        public double Wavelength { get; private set; }
        public double ActualFrequency { get; private set; }
        public double[] WaveData { get; private set; }
        private double MinimumVoltage;
        private double MaximumVoltage;


        public FunctionGenerator() {
            // Set default settings
            this.Device = string.Empty;
            this.Amplitude = 1.0;
            this.DCOffset = 0.0;
            this.DutyCycle = 50.0;
            this.WaveShape = WaveShape.Sine;
            this.Frequency = 100;
            this.On = false;
        }

        /* Begin outputting signal to given channel
         */
        public void Start(string channel)
        {
            this.On = true;
            try
            {
                AOTask.AOChannels.CreateVoltageChannel(channel, string.Empty,
                    this.MinimumVoltage, this.MaximumVoltage, AOVoltageUnits.Volts);
            }
            catch (DaqException ex) 
            {
                if (ex.Error != -200489)
                {
                    /* -200489: "Specified channel cannot be added to the task,
                     *  because a channel with the same name is already in the task."
                     * This error occurs when the output is started for a second time
                     *  on the same channel, which is acceptable.
                     */
                    this.Stop();
                    MessageBox.Show(ex.Message);
                }
            }

            try
            {
                /* Configure task
                 *  This must be done at this point rather than immediately after
                 *  creating the task because the device must be known
                 *  and channels must be present.
                 */
                AOTask.Timing.SampleTimingType = SampleTimingType.SampleClock;
                AOTask.AOChannels.All.UseOnlyOnBoardMemory = true;
                AOTask.Timing.ConfigureSampleClock(string.Empty,
                    this.Frequency * this.SampleCount,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples,
                    this.SampleCount);
                AOTask.Timing.SamplesPerChannel = this.SampleCount;

                // Reconfigure channel
                AOTask.AOChannels.All.Minimum = this.MinimumVoltage;
                AOTask.AOChannels.All.Maximum = this.MaximumVoltage;

                // Finally, output waveform to DAQ
                writer.WriteMultiSample(false, this.WaveData);
                AOTask.Start();
            }
            catch (DaqException ex)
            {
                this.Stop();
                MessageBox.Show(ex.Message);
            }
        }

        public void Stop()
        {
            this.On = false;
            AOTask.Stop();
            AnalogSingleChannelWriter zeroWriter = new AnalogSingleChannelWriter(AOTask.Stream);
            AOTask.AOChannels.All.UseOnlyOnBoardMemory = false;
            AOTask.AOChannels.All.Minimum = -1.0;
            AOTask.AOChannels.All.Maximum = 1.0;
            zeroWriter.WriteMultiSample(false, new double[] { 0, 0, 0 });
            AOTask.Start();
            AOTask.Stop();
        }

        public void GenerateWaveform()
        {
            // Calculate samples/buffer, and clip if needed
            this.SampleCount = (int)MAX_AD_RATE / this.Frequency;
            if (this.SampleCount > MAX_SAMPLE_COUNT) this.SampleCount = MAX_SAMPLE_COUNT;
            // Calculate other notable waveform properties
            this.Wavelength = 1.0 / this.Frequency;
            this.ActualFrequency = MAX_AD_RATE / this.SampleCount;

            // Duty cycle may need updating
            this.DutyCycle = this.DutyCycle;

            /* Determine an appropriate min and max voltage, clipped to +/- 10.0V.
             * This is used for the channel configuration when outputting.
             * TTL needs a special case of min and max voltage,
             *  overridden later.
             */
            this.MinimumVoltage = -this.Amplitude + this.DCOffset < -10.0 ?
                -10.0 : -this.Amplitude + this.DCOffset;
            this.MaximumVoltage = this.Amplitude + this.DCOffset > 10.0 ?
                10.0 : this.Amplitude + this.DCOffset;

            this.WaveData = new double[this.SampleCount];
            double y;
            Func<int, double> waveFunction;

            switch (this.WaveShape)
            {
                case WaveShape.Sine:
                    waveFunction = SineFunction;
                    break;
                case WaveShape.Square:
                    waveFunction = SquareFunction;
                    break;
                case WaveShape.Triangle:
                    waveFunction = TriangleFunction;
                    break;
                case WaveShape.Sawtooth:
                    waveFunction = SawtoothFunction;
                    break;
                default: // TTL
                    waveFunction = TTLFunction;
                    // TTL's special case of min and max voltage
                    this.MinimumVoltage = 0.0;
                    this.MaximumVoltage = 5.0;
                    break;
            }

            for (int i = 0; i < this.SampleCount; i++)
            {
                y = waveFunction(i);

                // Clip all functions to +/- 10V
                if (y > 10.0) y = 10.0;
                else if (y < -10.0) y = -10.0;
                this.WaveData[i] = y;
            }
        }

        /* Calculates the voltage of a single point of a sine wave
         *  at a given buffer array index.
         */
        private double SineFunction(int i)
        {
            return this.DCOffset + this.Amplitude * Math.Sin(
                    2.0 * Math.PI * i / this.SampleCount);
        }

        // Calculates single point of square function
        private double SquareFunction(int i)
        {
            if (i < this.SampleCount * (double)this.DutyCycle / 100)
                return this.Amplitude + this.DCOffset;
            else return -this.Amplitude + this.DCOffset;
        }

        // Calculates single point of triangle function
        private double TriangleFunction(int i)
        {
            double d = (double)this.DutyCycle / 100;
            double lambda = this.SampleCount;
            double A = this.Amplitude;

            if (i < lambda*d)
                return 2 * A * i / d / lambda - A + this.DCOffset;
            else
                return A * (2 / (1 - d) * (1 - i / lambda) - 1) + this.DCOffset;
        }

        // Calculates single point of sawtooth function
        private double SawtoothFunction(int i)
        {
            return 2 * this.Amplitude / this.SampleCount * i - this.Amplitude + this.DCOffset;
        }

        // Calculates single point of TTL function
        private double TTLFunction(int i)
        {
            if (i < this.SampleCount * (double)this.DutyCycle / 100)
                return 5.0;
            else return 0.0;
        }

        /* Determines the closest duty cycle to a precision appropriate
         *  to the number of data points, to a maximum precision of 0.1%
         */
        public double CalcActualDutyCycle(double dutyCycle)
        {
            double increment = 100.0 / this.SampleCount;

            if (increment <= 0.1)
            {
                return dutyCycle;
            }
            else
            {
                int nearestIncrement = (int)Math.Round(dutyCycle / increment);

                /* Clamp the increment such that the duty cycle change point
                 *  can't be at the endpoints (first or last sample)
                 */
                if (nearestIncrement < 1 && this.WaveShape == WaveShape.Triangle)
                    nearestIncrement = 1;
                else if (nearestIncrement >= this.SampleCount)
                    nearestIncrement = this.SampleCount - 1;

                return increment * nearestIncrement;
            }
        }
    }

    internal enum WaveShape
    {
        Sine,
        Square,
        Triangle,
        Sawtooth,
        TTL
    }
}
