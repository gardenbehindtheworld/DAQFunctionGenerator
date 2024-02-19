using NationalInstruments.DAQmx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
        private static AnalogMultiChannelWriter writer =
            new AnalogMultiChannelWriter(AOTask.Stream);

        public string Device { get; set; }
        public double Amplitude { get; set; }
        public double DCOffset { get; set; }
        public int DutyCycle { get; set; }
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
            this.DutyCycle = 50;
            this.WaveShape = WaveShape.Sine;
            this.Frequency = 100;
            this.On = false;
        }

        /* Begin outputting signal to given channel.
         * If channels have not been added yet, use parameter channelArray to add
         *  all available channels.
         */
        public void Start(string[] channelArray, int channelIndex)
        {
            this.On = true;
            foreach (string channel in channelArray)
            {
                try
                {
                    // Add new channel if necessary
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

                // Reconfigure existing channels
                AOTask.AOChannels.All.Minimum = this.MinimumVoltage;
                AOTask.AOChannels.All.Maximum = this.MaximumVoltage;

                // Create a multi-channel array to the size of the current output channels
                double[,] multiChannelData = new double[channelArray.Length, this.WaveData.Length];
                multiChannelData.SetValue(WaveData, channelIndex);

                // Finally, output waveform to DAQ
                writer.WriteMultiSample(false, multiChannelData);
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
            double r = d * lambda;
            double A = this.Amplitude;
            double t1 = r / 2;
            double t3 = lambda - t1;

            if (i < t1)
            {
                return 2 * A / r * i + this.DCOffset;
            }
            else if (i < t3)
            {
                return - 2 * A / (lambda - r) * i + A / (1 - d) + this.DCOffset;
            }
            else
            {
                return 2 * A / r * i - 2 * A / d + this.DCOffset;
            }
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

        /* Public method that allows channels to be added to the Task
         *  from the Form where channels are displayed.
         */
        public void AddChannels(string[] channelArray)
        {
            foreach (string channel in channelArray)
            {
                AOTask.AOChannels.CreateVoltageChannel(channel, string.Empty,
                    this.MinimumVoltage, this.MaximumVoltage, AOVoltageUnits.Volts);
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
