using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAQFunctionGenerator
{
    internal class FunctionGeneratorSettings
    {
        public static double MAX_AD_RATE = 833000.0;

        public string Device { get; set; }
        public double Amplitude { get; set; }
        public double DCOffset { get; set; }
        public int DutyCycle { get; set; }
        public WaveShape WaveShape { get; set; }
        public int Frequency { get; set; }

        // Read-only waveform setup values set by GenerateWaveform
        private int SampleCount;
        public double Wavelength { get; private set; }
        public double ActualFrequency { get; private set; }
        public double[] WaveData { get; private set; }

        public FunctionGeneratorSettings() {
            // Set default settings
            this.Device = string.Empty;
            this.Amplitude = 1.0;
            this.DCOffset = 0.0;
            this.DutyCycle = 50;
            this.WaveShape = WaveShape.Sine;
            this.Frequency = 100;
        }

        public void GenerateWaveform()
        {
            this.SampleCount = (int)MAX_AD_RATE / this.Frequency;
            this.Wavelength = 1.0 / this.Frequency;
            this.ActualFrequency = MAX_AD_RATE / this.SampleCount;

            this.WaveData = new double[this.SampleCount];
            double y;

            for (int i = 0; i < this.SampleCount; i++)
            {
                switch (this.WaveShape)
                {
                    case WaveShape.Sine:
                        y = SineFunction(i);
                        break;
                    case WaveShape.Square:
                        y = SquareFunction(i);
                        break;
                    default:
                        y = 0.0;
                        break;
                }

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
            if (i < this.SampleCount / 2) return -this.Amplitude + this.DCOffset;
            else return this.Amplitude + this.DCOffset;
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
