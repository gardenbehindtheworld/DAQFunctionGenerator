using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NationalInstruments;
using NationalInstruments.DAQmx;
using NationalInstruments.Restricted;

namespace DAQFunctionGenerator
{

    public partial class Frm1 : Form
    {
        /* Initialize settings object. This allows default values to be stored
         * and all user-input settings to be stored.
         * All output controls (and task and writer) are within the FunctionGenerator.
         */
        private FunctionGenerator funcGen = new FunctionGenerator();

        public Frm1()
        {
            InitializeComponent();
        }

        private void Frm1_Load(object sender, EventArgs e)
        {
            BtnStartStopToggle(false);

            // Chart setup
            chWaveform.Titles.Add("Waveform");
            chWaveform.Series.Add("Waveform");
            chWaveform.Series["Waveform"].ChartType =
                System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chWaveform.Series["Waveform"].BorderWidth = 3;
            chWaveform.Series["Waveform"].Color = Color.BlueViolet;
            chWaveform.Legends.Clear();
            chWaveform.ChartAreas[0].AxisX.Title = "Time (s)";
            chWaveform.ChartAreas[0].AxisY.Title = "Voltage (V)";

            // Amplitude updown box settings
            updAmplitude.Value = (decimal)funcGen.Amplitude;
            updAmplitude.DecimalPlaces = 2;
            updAmplitude.Minimum = 0;
            updAmplitude.Maximum = 10;
            updAmplitude.Increment = 0.01M;

            // DC Offset updown box settings
            updDCOffset.Value = (decimal)funcGen.DCOffset;
            updDCOffset.DecimalPlaces = 2;
            updDCOffset.Minimum = -10;
            updDCOffset.Maximum = 10;
            updDCOffset.Increment = 0.01M;

            // Duty Cycle updown box settings
            updDutyCycle.Value = 50.0M;
            updDutyCycle.DecimalPlaces = 1;
            updDutyCycle.Minimum = 1;
            updDutyCycle.Maximum = 99;
            updDutyCycle.Increment = 0.1M;

            // Waveform combo box settings
            cboWaveform.DropDownStyle = ComboBoxStyle.DropDownList;
            cboWaveform.Items.AddRange(new object[] {
                WaveShape.Sine,
                WaveShape.Square,
                WaveShape.Triangle,
                WaveShape.Sawtooth,
                WaveShape.TTL
            });
            cboWaveform.SelectedIndex = 0;

            // Frequency updown box settings
            updFrequency.Value = funcGen.Frequency;
            updFrequency.DecimalPlaces = 0;
            updFrequency.Minimum = 1;
            updFrequency.Maximum = 100000;
            updFrequency.Increment = 1;

            // Device combo box settings
            cboChannel.DropDownStyle = ComboBoxStyle.DropDownList;
            cboChannel.Items.AddRange(
                DaqSystem.Local.GetPhysicalChannels(
                    PhysicalChannelTypes.AO, PhysicalChannelAccess.External));
            if (cboChannel.Items.Count > 0)
            {
                cboChannel.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("There are no analog output channels available.");
                SetUserInputPermissions(false);
            }
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            switch (funcGen.On)
            {
                case true:
                    BtnStartStopToggle(false);
                    funcGen.Stop();
                    break;
                case false:
                    cboChannel.Enabled = false;
                    BtnStartStopToggle(true);
                    funcGen.Start(cboChannel.SelectedItem.ToString());
                    break;
            }
        }

        private void InputChanged()
        {
            // Generate waveform
            funcGen.GenerateWaveform();

            // Put waveform on plot
            chWaveform.Series["Waveform"].Points.Clear();
            for (int i = 0; i < funcGen.WaveData.Length; i++)
            {
                chWaveform.Series["Waveform"].Points.Add(funcGen.WaveData[i]);
            }

            // Update visible outputs
            lblActualFrequencyNum.Text = $"{funcGen.ActualFrequency:0.0000} Hz";

            // Update waveform output if necessary
            if (funcGen.On == true)
            {
                funcGen.Stop();
                funcGen.Start(cboChannel.SelectedItem.ToString());
            }
            Debug.WriteLine(funcGen.DutyCycle);
        }

        private void BtnStartStopToggle(bool startStop)
        {
            switch (startStop)
            {
                case true:
                    btnStartStop.Text = "Running";
                    btnStartStop.BackColor = Color.LightGreen;
                    break;
                case false:
                    btnStartStop.Text = "Stopped";
                    btnStartStop.BackColor = Color.Red;
                    break;
            }
        }

        private void SetUserInputPermissions(bool enableDisable)
        {
            cboChannel.Enabled = enableDisable;
            cboChannel.Refresh();
            updAmplitude.Enabled = enableDisable;
            updDCOffset.Enabled = enableDisable;
            updDutyCycle.Enabled = enableDisable;
            cboWaveform.Enabled = enableDisable;
            cboWaveform.Refresh();
            updFrequency.Enabled = enableDisable;
            btnStartStop.Enabled = enableDisable;
        }

        private void CboChannel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CboWaveform_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWaveform.SelectedItem is WaveShape)
            {
                funcGen.WaveShape = (WaveShape)cboWaveform.SelectedItem;
                switch (funcGen.WaveShape)
                {
                    case WaveShape.Sine:
                    case WaveShape.Sawtooth:
                        updAmplitude.Enabled = true;
                        updDutyCycle.Enabled = false;
                        updDCOffset.Enabled = true;
                        break;
                    case WaveShape.TTL:
                        updAmplitude.Enabled = false;
                        updDutyCycle.Enabled = true;
                        updDCOffset.Enabled = false;
                        break;
                    default:
                        updAmplitude.Enabled = true;
                        updDutyCycle.Enabled = true;
                        updDCOffset.Enabled = true;
                        break;
                }
            }

            InputChanged();
        }

        private void UpdFrequency_ValueChanged(object sender, EventArgs e)
        {
            funcGen.Frequency = (int)updFrequency.Value;
            InputChanged();
        }

        private void UpdDutyCycle_ValueChanged(object sender, EventArgs e)
        {

            funcGen.DutyCycle = (int)updDutyCycle.Value;
            InputChanged();
        }

        private void UpdAmplitude_ValueChanged(object sender, EventArgs e)
        {
            funcGen.Amplitude = (double)updAmplitude.Value;
            InputChanged();
        }

        private void UpdDCOffset_ValueChanged(object sender, EventArgs e)
        {
            funcGen.DCOffset = (double)updDCOffset.Value;
            InputChanged();
        }
    }
}
