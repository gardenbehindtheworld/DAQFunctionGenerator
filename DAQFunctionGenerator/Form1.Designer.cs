namespace DAQFunctionGenerator
{
    partial class Frm1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.cboChannel = new System.Windows.Forms.ComboBox();
            this.cboWaveform = new System.Windows.Forms.ComboBox();
            this.updAmplitude = new System.Windows.Forms.NumericUpDown();
            this.updDCOffset = new System.Windows.Forms.NumericUpDown();
            this.updDutyCycle = new System.Windows.Forms.NumericUpDown();
            this.updFrequency = new System.Windows.Forms.NumericUpDown();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblDevice = new System.Windows.Forms.Label();
            this.lblAmplitude = new System.Windows.Forms.Label();
            this.lblDCOffset = new System.Windows.Forms.Label();
            this.lblDutyCycle = new System.Windows.Forms.Label();
            this.lblWaveform = new System.Windows.Forms.Label();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.lblActualFrequency = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblAmplitudeUnit = new System.Windows.Forms.Label();
            this.lblDCOffsetUnit = new System.Windows.Forms.Label();
            this.lblDutyCycleUnit = new System.Windows.Forms.Label();
            this.lblFrequencyUnit = new System.Windows.Forms.Label();
            this.chWaveform = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.updAmplitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updDCOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updDutyCycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chWaveform)).BeginInit();
            this.SuspendLayout();
            // 
            // cboChannel
            // 
            this.cboChannel.FormattingEnabled = true;
            this.cboChannel.Location = new System.Drawing.Point(98, 40);
            this.cboChannel.Name = "cboChannel";
            this.cboChannel.Size = new System.Drawing.Size(96, 21);
            this.cboChannel.TabIndex = 0;
            this.cboChannel.SelectedIndexChanged += new System.EventHandler(this.CboChannel_SelectedIndexChanged);
            // 
            // cboWaveform
            // 
            this.cboWaveform.FormattingEnabled = true;
            this.cboWaveform.Location = new System.Drawing.Point(98, 168);
            this.cboWaveform.Name = "cboWaveform";
            this.cboWaveform.Size = new System.Drawing.Size(96, 21);
            this.cboWaveform.TabIndex = 1;
            this.cboWaveform.SelectedIndexChanged += new System.EventHandler(this.CboWaveform_SelectedIndexChanged);
            // 
            // updAmplitude
            // 
            this.updAmplitude.Location = new System.Drawing.Point(98, 72);
            this.updAmplitude.Name = "updAmplitude";
            this.updAmplitude.Size = new System.Drawing.Size(73, 20);
            this.updAmplitude.TabIndex = 2;
            // 
            // updDCOffset
            // 
            this.updDCOffset.Location = new System.Drawing.Point(98, 104);
            this.updDCOffset.Name = "updDCOffset";
            this.updDCOffset.Size = new System.Drawing.Size(73, 20);
            this.updDCOffset.TabIndex = 3;
            // 
            // updDutyCycle
            // 
            this.updDutyCycle.Location = new System.Drawing.Point(98, 136);
            this.updDutyCycle.Name = "updDutyCycle";
            this.updDutyCycle.Size = new System.Drawing.Size(73, 20);
            this.updDutyCycle.TabIndex = 4;
            // 
            // updFrequency
            // 
            this.updFrequency.Location = new System.Drawing.Point(98, 200);
            this.updFrequency.Name = "updFrequency";
            this.updFrequency.Size = new System.Drawing.Size(73, 20);
            this.updFrequency.TabIndex = 5;
            // 
            // btnStartStop
            // 
            this.btnStartStop.BackColor = System.Drawing.SystemColors.Window;
            this.btnStartStop.Location = new System.Drawing.Point(31, 252);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(163, 32);
            this.btnStartStop.TabIndex = 6;
            this.btnStartStop.Text = "button1";
            this.btnStartStop.UseVisualStyleBackColor = false;
            // 
            // lblDevice
            // 
            this.lblDevice.Location = new System.Drawing.Point(28, 43);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(64, 20);
            this.lblDevice.TabIndex = 7;
            this.lblDevice.Text = "Channel";
            this.lblDevice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblAmplitude
            // 
            this.lblAmplitude.Location = new System.Drawing.Point(28, 74);
            this.lblAmplitude.Name = "lblAmplitude";
            this.lblAmplitude.Size = new System.Drawing.Size(64, 20);
            this.lblAmplitude.TabIndex = 8;
            this.lblAmplitude.Text = "Amplitude";
            this.lblAmplitude.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDCOffset
            // 
            this.lblDCOffset.Location = new System.Drawing.Point(28, 106);
            this.lblDCOffset.Name = "lblDCOffset";
            this.lblDCOffset.Size = new System.Drawing.Size(64, 20);
            this.lblDCOffset.TabIndex = 9;
            this.lblDCOffset.Text = "DC Offset";
            this.lblDCOffset.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDutyCycle
            // 
            this.lblDutyCycle.Location = new System.Drawing.Point(28, 138);
            this.lblDutyCycle.Name = "lblDutyCycle";
            this.lblDutyCycle.Size = new System.Drawing.Size(64, 20);
            this.lblDutyCycle.TabIndex = 10;
            this.lblDutyCycle.Text = "Duty Cycle";
            this.lblDutyCycle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblWaveform
            // 
            this.lblWaveform.Location = new System.Drawing.Point(28, 171);
            this.lblWaveform.Name = "lblWaveform";
            this.lblWaveform.Size = new System.Drawing.Size(64, 20);
            this.lblWaveform.TabIndex = 11;
            this.lblWaveform.Text = "Waveform";
            this.lblWaveform.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblFrequency
            // 
            this.lblFrequency.Location = new System.Drawing.Point(28, 202);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(64, 20);
            this.lblFrequency.TabIndex = 12;
            this.lblFrequency.Text = "Frequency";
            this.lblFrequency.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblActualFrequency
            // 
            this.lblActualFrequency.AutoSize = true;
            this.lblActualFrequency.Location = new System.Drawing.Point(28, 312);
            this.lblActualFrequency.Name = "lblActualFrequency";
            this.lblActualFrequency.Size = new System.Drawing.Size(93, 13);
            this.lblActualFrequency.TabIndex = 13;
            this.lblActualFrequency.Text = "Actual Frequency:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(159, 312);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "label8";
            // 
            // lblAmplitudeUnit
            // 
            this.lblAmplitudeUnit.Location = new System.Drawing.Point(177, 74);
            this.lblAmplitudeUnit.Name = "lblAmplitudeUnit";
            this.lblAmplitudeUnit.Size = new System.Drawing.Size(17, 23);
            this.lblAmplitudeUnit.TabIndex = 16;
            this.lblAmplitudeUnit.Text = "V";
            // 
            // lblDCOffsetUnit
            // 
            this.lblDCOffsetUnit.Location = new System.Drawing.Point(177, 106);
            this.lblDCOffsetUnit.Name = "lblDCOffsetUnit";
            this.lblDCOffsetUnit.Size = new System.Drawing.Size(17, 23);
            this.lblDCOffsetUnit.TabIndex = 17;
            this.lblDCOffsetUnit.Text = "V";
            // 
            // lblDutyCycleUnit
            // 
            this.lblDutyCycleUnit.Location = new System.Drawing.Point(177, 138);
            this.lblDutyCycleUnit.Name = "lblDutyCycleUnit";
            this.lblDutyCycleUnit.Size = new System.Drawing.Size(17, 23);
            this.lblDutyCycleUnit.TabIndex = 18;
            this.lblDutyCycleUnit.Text = "%";
            // 
            // lblFrequencyUnit
            // 
            this.lblFrequencyUnit.Location = new System.Drawing.Point(177, 202);
            this.lblFrequencyUnit.Name = "lblFrequencyUnit";
            this.lblFrequencyUnit.Size = new System.Drawing.Size(30, 23);
            this.lblFrequencyUnit.TabIndex = 19;
            this.lblFrequencyUnit.Text = "Hz";
            // 
            // chWaveform
            // 
            this.chWaveform.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chWaveform.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chWaveform.Legends.Add(legend1);
            this.chWaveform.Location = new System.Drawing.Point(229, 12);
            this.chWaveform.Name = "chWaveform";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chWaveform.Series.Add(series1);
            this.chWaveform.Size = new System.Drawing.Size(467, 323);
            this.chWaveform.TabIndex = 20;
            this.chWaveform.Text = "chart1";
            // 
            // Frm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 347);
            this.Controls.Add(this.chWaveform);
            this.Controls.Add(this.lblFrequencyUnit);
            this.Controls.Add(this.lblDutyCycleUnit);
            this.Controls.Add(this.lblDCOffsetUnit);
            this.Controls.Add(this.lblAmplitudeUnit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblActualFrequency);
            this.Controls.Add(this.lblFrequency);
            this.Controls.Add(this.lblWaveform);
            this.Controls.Add(this.lblDutyCycle);
            this.Controls.Add(this.lblDCOffset);
            this.Controls.Add(this.lblAmplitude);
            this.Controls.Add(this.lblDevice);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.updFrequency);
            this.Controls.Add(this.updDutyCycle);
            this.Controls.Add(this.updDCOffset);
            this.Controls.Add(this.updAmplitude);
            this.Controls.Add(this.cboWaveform);
            this.Controls.Add(this.cboChannel);
            this.Name = "Frm1";
            this.Text = "Function Generator";
            this.Load += new System.EventHandler(this.Frm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.updAmplitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updDCOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updDutyCycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chWaveform)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboChannel;
        private System.Windows.Forms.ComboBox cboWaveform;
        private System.Windows.Forms.NumericUpDown updAmplitude;
        private System.Windows.Forms.NumericUpDown updDCOffset;
        private System.Windows.Forms.NumericUpDown updDutyCycle;
        private System.Windows.Forms.NumericUpDown updFrequency;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.Label lblAmplitude;
        private System.Windows.Forms.Label lblDCOffset;
        private System.Windows.Forms.Label lblDutyCycle;
        private System.Windows.Forms.Label lblWaveform;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Label lblActualFrequency;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblAmplitudeUnit;
        private System.Windows.Forms.Label lblDCOffsetUnit;
        private System.Windows.Forms.Label lblDutyCycleUnit;
        private System.Windows.Forms.Label lblFrequencyUnit;
        private System.Windows.Forms.DataVisualization.Charting.Chart chWaveform;
    }
}

