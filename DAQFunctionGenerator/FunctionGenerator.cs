﻿using NationalInstruments.DAQmx;
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

        /* Begin outputting signal to given channel
         */
        public void Start(string channel)
        {
            this.On = true;
            try
            {
                AOTask.AOChannels.CreateVoltageChannel(channel, string.Empty,
                    this.MinimumVoltage, this.MaximumVoltage, AOVoltageUnits.Volts);

                /* Configure task
                 *  This must be done at this point rather than immediately after
                 *  creating the task because the device must be known
                 *  and channels must be present.
                 */
                AOTask.Timing.SampleTimingType = SampleTimingType.SampleClock;
                AOTask.Timing.SampleQuantityMode = SampleQuantityMode.ContinuousSamples;
                AOTask.AOChannels.All.UseOnlyOnBoardMemory = true;

                writer.WriteMultiSample(false, this.WaveData);
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
                    AOTask.Stop();
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void Stop()
        {
            this.On = false;
            AOTask.Stop();
        }

        public void GenerateWaveform()
        {
            this.SampleCount = (int)MAX_AD_RATE / this.Frequency;
            if (this.SampleCount > MAX_SAMPLE_COUNT) this.SampleCount = MAX_SAMPLE_COUNT;
            this.Wavelength = 1.0 / this.Frequency;
            this.ActualFrequency = MAX_AD_RATE / this.SampleCount;
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

        private double SineFunction(int i)
        {
            return this.DCOffset + this.Amplitude * Math.Sin(
                    2.0 * Math.PI * i / this.SampleCount);
        }

        private double SquareFunction(int i)
        {
            if (i < this.SampleCount * (double)this.DutyCycle / 100)
                return this.Amplitude + this.DCOffset;
            else return -this.Amplitude + this.DCOffset;
        }

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

        private double SawtoothFunction(int i)
        {
            return 2 * this.Amplitude / this.SampleCount * i - this.Amplitude + this.DCOffset;
        }

        private double TTLFunction(int i)
        {
            if (i < this.SampleCount * (double)this.DutyCycle / 100)
                return 5.0;
            else return 0.0;
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
