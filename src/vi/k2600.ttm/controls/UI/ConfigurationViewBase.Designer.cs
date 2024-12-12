using System;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls
{
    public partial class ConfigurationViewBase
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            _groupBox = new System.Windows.Forms.GroupBox();
            _restoreDefaultsButton = new System.Windows.Forms.Button();
            _restoreDefaultsButton.Click += new EventHandler(RestoreDefaultsButton_Click);
            _applyNewConfigurationButton = new System.Windows.Forms.Button();
            _applyNewConfigurationButton.Click += new EventHandler(ApplyNewConfigurationButton_Click);
            _applyConfigurationButton = new System.Windows.Forms.Button();
            _applyConfigurationButton.Click += new EventHandler(ApplyConfigurationButton_Click);
            _thermalTransientGroupBox = new System.Windows.Forms.GroupBox();
            _thermalVoltageLimitLabel = new System.Windows.Forms.Label();
            _thermalVoltageLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _thermalVoltageLowLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _thermalVoltageLowLimitNumericLabel = new System.Windows.Forms.Label();
            _thermalVoltageHighLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _thermalVoltageHighLimitNumericLabel = new System.Windows.Forms.Label();
            _thermalVoltageNumeric = new System.Windows.Forms.NumericUpDown();
            _thermalVoltageNumericLabel = new System.Windows.Forms.Label();
            _thermalCurrentNumeric = new System.Windows.Forms.NumericUpDown();
            _thermalCurrentNumericLabel = new System.Windows.Forms.Label();
            _coldResistanceConfigGroupBox = new System.Windows.Forms.GroupBox();
            _checkContactsCheckBox = new System.Windows.Forms.CheckBox();
            _postTransientDelayNumeric = new System.Windows.Forms.NumericUpDown();
            _postTransientDelayNumericLabel = new System.Windows.Forms.Label();
            _coldResistanceLowLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _resistanceLowLimitNumericLabel = new System.Windows.Forms.Label();
            _coldResistanceHighLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _resistanceHighLimitNumericLabel = new System.Windows.Forms.Label();
            _coldVoltageNumeric = new System.Windows.Forms.NumericUpDown();
            _coldVoltageNumericLabel = new System.Windows.Forms.Label();
            _coldCurrentNumeric = new System.Windows.Forms.NumericUpDown();
            _coldCurrentNumericLabel = new System.Windows.Forms.Label();
            _groupBox.SuspendLayout();
            _thermalTransientGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_thermalVoltageLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_thermalVoltageLowLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_thermalVoltageHighLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_thermalVoltageNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_thermalCurrentNumeric).BeginInit();
            _coldResistanceConfigGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_postTransientDelayNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_coldResistanceLowLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_coldResistanceHighLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_coldVoltageNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_coldCurrentNumeric).BeginInit();
            SuspendLayout();
            // 
            // _groupBox
            // 
            _groupBox.Controls.Add(_restoreDefaultsButton);
            _groupBox.Controls.Add(_applyNewConfigurationButton);
            _groupBox.Controls.Add(_applyConfigurationButton);
            _groupBox.Controls.Add(_thermalTransientGroupBox);
            _groupBox.Controls.Add(_coldResistanceConfigGroupBox);
            _groupBox.Location = new System.Drawing.Point(0, 0);
            _groupBox.Name = "_GroupBox";
            _groupBox.Size = new System.Drawing.Size(544, 333);
            _groupBox.TabIndex = 2;
            _groupBox.TabStop = false;
            _groupBox.Text = "TTM CONFIGURATION";
            // 
            // _restoreDefaultsButton
            // 
            _restoreDefaultsButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _restoreDefaultsButton.Location = new System.Drawing.Point(364, 294);
            _restoreDefaultsButton.Name = "_RestoreDefaultsButton";
            _restoreDefaultsButton.Size = new System.Drawing.Size(153, 30);
            _restoreDefaultsButton.TabIndex = 4;
            _restoreDefaultsButton.Text = "RESTORE DEFAULTS";
            _restoreDefaultsButton.UseVisualStyleBackColor = true;
            // 
            // _applyNewConfigurationButton
            // 
            _applyNewConfigurationButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _applyNewConfigurationButton.Location = new System.Drawing.Point(196, 294);
            _applyNewConfigurationButton.Name = "_applyNewConfigurationButton";
            _applyNewConfigurationButton.Size = new System.Drawing.Size(153, 30);
            _applyNewConfigurationButton.TabIndex = 3;
            _applyNewConfigurationButton.Text = "APPLY CHANGES";
            _applyNewConfigurationButton.UseVisualStyleBackColor = true;
            // 
            // _applyConfigurationButton
            // 
            _applyConfigurationButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _applyConfigurationButton.Location = new System.Drawing.Point(28, 294);
            _applyConfigurationButton.Name = "_applyConfigurationButton";
            _applyConfigurationButton.Size = new System.Drawing.Size(153, 30);
            _applyConfigurationButton.TabIndex = 2;
            _applyConfigurationButton.Text = "APPLY ALL";
            _applyConfigurationButton.UseVisualStyleBackColor = true;
            // 
            // _thermalTransientGroupBox
            // 
            _thermalTransientGroupBox.Controls.Add(_thermalVoltageLimitLabel);
            _thermalTransientGroupBox.Controls.Add(_thermalVoltageLimitNumeric);
            _thermalTransientGroupBox.Controls.Add(_thermalVoltageLowLimitNumeric);
            _thermalTransientGroupBox.Controls.Add(_thermalVoltageLowLimitNumericLabel);
            _thermalTransientGroupBox.Controls.Add(_thermalVoltageHighLimitNumeric);
            _thermalTransientGroupBox.Controls.Add(_thermalVoltageHighLimitNumericLabel);
            _thermalTransientGroupBox.Controls.Add(_thermalVoltageNumeric);
            _thermalTransientGroupBox.Controls.Add(_thermalVoltageNumericLabel);
            _thermalTransientGroupBox.Controls.Add(_thermalCurrentNumeric);
            _thermalTransientGroupBox.Controls.Add(_thermalCurrentNumericLabel);
            _thermalTransientGroupBox.Location = new System.Drawing.Point(277, 23);
            _thermalTransientGroupBox.Name = "_ThermalTransientGroupBox";
            _thermalTransientGroupBox.Size = new System.Drawing.Size(258, 259);
            _thermalTransientGroupBox.TabIndex = 0;
            _thermalTransientGroupBox.TabStop = false;
            _thermalTransientGroupBox.Text = "THERMAL TRANSIENT";
            // 
            // _thermalVoltageLimitLabel
            // 
            _thermalVoltageLimitLabel.AutoSize = true;
            _thermalVoltageLimitLabel.Location = new System.Drawing.Point(45, 113);
            _thermalVoltageLimitLabel.Name = "_ThermalVoltageLimitLabel";
            _thermalVoltageLimitLabel.Size = new System.Drawing.Size(121, 17);
            _thermalVoltageLimitLabel.TabIndex = 4;
            _thermalVoltageLimitLabel.Text = "VOLTAGE LIMIT &[V]:";
            _thermalVoltageLimitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _thermalVoltageLimitNumeric
            // 
            _thermalVoltageLimitNumeric.DecimalPlaces = 3;
            _thermalVoltageLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _thermalVoltageLimitNumeric.Location = new System.Drawing.Point(168, 109);
            _thermalVoltageLimitNumeric.Maximum = new decimal(new int[] { 9999, 0, 0, 196608 });
            _thermalVoltageLimitNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            _thermalVoltageLimitNumeric.Name = "_ThermalVoltageLimitNumeric";
            _thermalVoltageLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _thermalVoltageLimitNumeric.TabIndex = 5;
            _thermalVoltageLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _thermalVoltageLimitNumeric.Value = new decimal(new int[] { 99, 0, 0, 131072 });
            // 
            // _thermalVoltageLowLimitNumeric
            // 
            _thermalVoltageLowLimitNumeric.DecimalPlaces = 3;
            _thermalVoltageLowLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _thermalVoltageLowLimitNumeric.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            _thermalVoltageLowLimitNumeric.Location = new System.Drawing.Point(168, 189);
            _thermalVoltageLowLimitNumeric.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            _thermalVoltageLowLimitNumeric.Name = "_ThermalVoltageLowLimitNumeric";
            _thermalVoltageLowLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _thermalVoltageLowLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _thermalVoltageLowLimitNumeric.TabIndex = 9;
            _thermalVoltageLowLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _thermalVoltageLowLimitNumeric.Value = new decimal(new int[] { 1, 0, 0, 196608 });
            // 
            // _thermalVoltageLowLimitNumericLabel
            // 
            _thermalVoltageLowLimitNumericLabel.AutoSize = true;
            _thermalVoltageLowLimitNumericLabel.Location = new System.Drawing.Point(72, 193);
            _thermalVoltageLowLimitNumericLabel.Name = "_ThermalVoltageLowLimitNumericLabel";
            _thermalVoltageLowLimitNumericLabel.Size = new System.Drawing.Size(94, 17);
            _thermalVoltageLowLimitNumericLabel.TabIndex = 8;
            _thermalVoltageLowLimitNumericLabel.Text = "LOW LIMI&T [V]:";
            _thermalVoltageLowLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _thermalVoltageHighLimitNumeric
            // 
            _thermalVoltageHighLimitNumeric.DecimalPlaces = 3;
            _thermalVoltageHighLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _thermalVoltageHighLimitNumeric.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            _thermalVoltageHighLimitNumeric.Location = new System.Drawing.Point(168, 149);
            _thermalVoltageHighLimitNumeric.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            _thermalVoltageHighLimitNumeric.Name = "_ThermalVoltageHighLimitNumeric";
            _thermalVoltageHighLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _thermalVoltageHighLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _thermalVoltageHighLimitNumeric.TabIndex = 7;
            _thermalVoltageHighLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _thermalVoltageHighLimitNumeric.Value = new decimal(new int[] { 1, 0, 0, 196608 });
            // 
            // _thermalVoltageHighLimitNumericLabel
            // 
            _thermalVoltageHighLimitNumericLabel.AutoSize = true;
            _thermalVoltageHighLimitNumericLabel.Location = new System.Drawing.Point(70, 153);
            _thermalVoltageHighLimitNumericLabel.Name = "_ThermalVoltageHighLimitNumericLabel";
            _thermalVoltageHighLimitNumericLabel.Size = new System.Drawing.Size(96, 17);
            _thermalVoltageHighLimitNumericLabel.TabIndex = 6;
            _thermalVoltageHighLimitNumericLabel.Text = "HI&GH LIMIT [V]:";
            _thermalVoltageHighLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _thermalVoltageNumeric
            // 
            _thermalVoltageNumeric.DecimalPlaces = 3;
            _thermalVoltageNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _thermalVoltageNumeric.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            _thermalVoltageNumeric.Location = new System.Drawing.Point(168, 69);
            _thermalVoltageNumeric.Maximum = new decimal(new int[] { 99, 0, 0, 131072 });
            _thermalVoltageNumeric.Name = "_ThermalVoltageNumeric";
            _thermalVoltageNumeric.Size = new System.Drawing.Size(75, 25);
            _thermalVoltageNumeric.TabIndex = 3;
            _thermalVoltageNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _thermalVoltageNumeric.Value = new decimal(new int[] { 1, 0, 0, 196608 });
            // 
            // _thermalVoltageNumericLabel
            // 
            _thermalVoltageNumericLabel.AutoSize = true;
            _thermalVoltageNumericLabel.Location = new System.Drawing.Point(25, 73);
            _thermalVoltageNumericLabel.Name = "_ThermalVoltageNumericLabel";
            _thermalVoltageNumericLabel.Size = new System.Drawing.Size(141, 17);
            _thermalVoltageNumericLabel.TabIndex = 2;
            _thermalVoltageNumericLabel.Text = "VOLTAGE C&HANGE [V]:";
            _thermalVoltageNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _thermalCurrentNumeric
            // 
            _thermalCurrentNumeric.DecimalPlaces = 3;
            _thermalCurrentNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _thermalCurrentNumeric.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            _thermalCurrentNumeric.Location = new System.Drawing.Point(168, 29);
            _thermalCurrentNumeric.Maximum = new decimal(new int[] { 1514, 0, 0, 196608 });
            _thermalCurrentNumeric.Name = "_ThermalCurrentNumeric";
            _thermalCurrentNumeric.Size = new System.Drawing.Size(75, 25);
            _thermalCurrentNumeric.TabIndex = 1;
            _thermalCurrentNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _thermalCurrentNumeric.Value = new decimal(new int[] { 1, 0, 0, 196608 });
            // 
            // _thermalCurrentNumericLabel
            // 
            _thermalCurrentNumericLabel.AutoSize = true;
            _thermalCurrentNumericLabel.Location = new System.Drawing.Point(40, 33);
            _thermalCurrentNumericLabel.Name = "_ThermalCurrentNumericLabel";
            _thermalCurrentNumericLabel.Size = new System.Drawing.Size(126, 17);
            _thermalCurrentNumericLabel.TabIndex = 0;
            _thermalCurrentNumericLabel.Text = "CURR&ENT LEVEL [A]:";
            _thermalCurrentNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _coldResistanceConfigGroupBox
            // 
            _coldResistanceConfigGroupBox.Controls.Add(_checkContactsCheckBox);
            _coldResistanceConfigGroupBox.Controls.Add(_postTransientDelayNumeric);
            _coldResistanceConfigGroupBox.Controls.Add(_postTransientDelayNumericLabel);
            _coldResistanceConfigGroupBox.Controls.Add(_coldResistanceLowLimitNumeric);
            _coldResistanceConfigGroupBox.Controls.Add(_resistanceLowLimitNumericLabel);
            _coldResistanceConfigGroupBox.Controls.Add(_coldResistanceHighLimitNumeric);
            _coldResistanceConfigGroupBox.Controls.Add(_resistanceHighLimitNumericLabel);
            _coldResistanceConfigGroupBox.Controls.Add(_coldVoltageNumeric);
            _coldResistanceConfigGroupBox.Controls.Add(_coldVoltageNumericLabel);
            _coldResistanceConfigGroupBox.Controls.Add(_coldCurrentNumeric);
            _coldResistanceConfigGroupBox.Controls.Add(_coldCurrentNumericLabel);
            _coldResistanceConfigGroupBox.Location = new System.Drawing.Point(9, 23);
            _coldResistanceConfigGroupBox.Name = "_ColdResistanceConfigGroupBox";
            _coldResistanceConfigGroupBox.Size = new System.Drawing.Size(258, 259);
            _coldResistanceConfigGroupBox.TabIndex = 0;
            _coldResistanceConfigGroupBox.TabStop = false;
            _coldResistanceConfigGroupBox.Text = "COLD RESISTANCE";
            // 
            // _checkContactsCheckBox
            // 
            _checkContactsCheckBox.AutoSize = true;
            _checkContactsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            _checkContactsCheckBox.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _checkContactsCheckBox.Location = new System.Drawing.Point(36, 233);
            _checkContactsCheckBox.Name = "_CheckContactsCheckBox";
            _checkContactsCheckBox.Size = new System.Drawing.Size(139, 21);
            _checkContactsCheckBox.TabIndex = 5;
            _checkContactsCheckBox.Text = "CHECK CONTACTS:";
            _checkContactsCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            _checkContactsCheckBox.UseVisualStyleBackColor = true;
            // 
            // _postTransientDelayNumeric
            // 
            _postTransientDelayNumeric.DecimalPlaces = 2;
            _postTransientDelayNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _postTransientDelayNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            _postTransientDelayNumeric.Location = new System.Drawing.Point(162, 189);
            _postTransientDelayNumeric.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
            _postTransientDelayNumeric.Name = "_PostTransientDelayNumeric";
            _postTransientDelayNumeric.Size = new System.Drawing.Size(75, 25);
            _postTransientDelayNumeric.TabIndex = 9;
            _postTransientDelayNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _postTransientDelayNumeric.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            // 
            // _postTransientDelayNumericLabel
            // 
            _postTransientDelayNumericLabel.AutoSize = true;
            _postTransientDelayNumericLabel.Location = new System.Drawing.Point(28, 193);
            _postTransientDelayNumericLabel.Name = "_PostTransientDelayNumericLabel";
            _postTransientDelayNumericLabel.Size = new System.Drawing.Size(132, 17);
            _postTransientDelayNumericLabel.TabIndex = 8;
            _postTransientDelayNumericLabel.Text = "POST TTM &DELAY [S]:";
            _postTransientDelayNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _coldResistanceLowLimitNumeric
            // 
            _coldResistanceLowLimitNumeric.DecimalPlaces = 3;
            _coldResistanceLowLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _coldResistanceLowLimitNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            _coldResistanceLowLimitNumeric.Location = new System.Drawing.Point(162, 149);
            _coldResistanceLowLimitNumeric.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            _coldResistanceLowLimitNumeric.Name = "_ColdResistanceLowLimitNumeric";
            _coldResistanceLowLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _coldResistanceLowLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _coldResistanceLowLimitNumeric.TabIndex = 7;
            _coldResistanceLowLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _coldResistanceLowLimitNumeric.Value = new decimal(new int[] { 1, 0, 0, 196608 });
            // 
            // _resistanceLowLimitNumericLabel
            // 
            _resistanceLowLimitNumericLabel.AutoSize = true;
            _resistanceLowLimitNumericLabel.Location = new System.Drawing.Point(64, 153);
            _resistanceLowLimitNumericLabel.Name = "_ResistanceLowLimitNumericLabel";
            _resistanceLowLimitNumericLabel.Size = new System.Drawing.Size(96, 17);
            _resistanceLowLimitNumericLabel.TabIndex = 6;
            _resistanceLowLimitNumericLabel.Text = "LO&W LIMIT [Ω]:";
            _resistanceLowLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _coldResistanceHighLimitNumeric
            // 
            _coldResistanceHighLimitNumeric.DecimalPlaces = 3;
            _coldResistanceHighLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _coldResistanceHighLimitNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            _coldResistanceHighLimitNumeric.Location = new System.Drawing.Point(162, 109);
            _coldResistanceHighLimitNumeric.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            _coldResistanceHighLimitNumeric.Name = "_ColdResistanceHighLimitNumeric";
            _coldResistanceHighLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _coldResistanceHighLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _coldResistanceHighLimitNumeric.TabIndex = 5;
            _coldResistanceHighLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _coldResistanceHighLimitNumeric.Value = new decimal(new int[] { 1, 0, 0, 196608 });
            // 
            // _resistanceHighLimitNumericLabel
            // 
            _resistanceHighLimitNumericLabel.AutoSize = true;
            _resistanceHighLimitNumericLabel.Location = new System.Drawing.Point(62, 113);
            _resistanceHighLimitNumericLabel.Name = "_ResistanceHighLimitNumericLabel";
            _resistanceHighLimitNumericLabel.Size = new System.Drawing.Size(98, 17);
            _resistanceHighLimitNumericLabel.TabIndex = 4;
            _resistanceHighLimitNumericLabel.Text = "&HIGH LIMIT [Ω]:";
            _resistanceHighLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _coldVoltageNumeric
            // 
            _coldVoltageNumeric.DecimalPlaces = 2;
            _coldVoltageNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _coldVoltageNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            _coldVoltageNumeric.Location = new System.Drawing.Point(162, 69);
            _coldVoltageNumeric.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            _coldVoltageNumeric.Name = "_ColdVoltageNumeric";
            _coldVoltageNumeric.Size = new System.Drawing.Size(75, 25);
            _coldVoltageNumeric.TabIndex = 3;
            _coldVoltageNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _coldVoltageNumeric.Value = new decimal(new int[] { 1, 0, 0, 131072 });
            // 
            // _coldVoltageNumericLabel
            // 
            _coldVoltageNumericLabel.AutoSize = true;
            _coldVoltageNumericLabel.Location = new System.Drawing.Point(39, 73);
            _coldVoltageNumericLabel.Name = "_ColdVoltageNumericLabel";
            _coldVoltageNumericLabel.Size = new System.Drawing.Size(121, 17);
            _coldVoltageNumericLabel.TabIndex = 2;
            _coldVoltageNumericLabel.Text = "&VOLTAGE LIMIT [V]:";
            _coldVoltageNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _coldCurrentNumeric
            // 
            _coldCurrentNumeric.DecimalPlaces = 4;
            _coldCurrentNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _coldCurrentNumeric.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            _coldCurrentNumeric.Location = new System.Drawing.Point(162, 29);
            _coldCurrentNumeric.Maximum = new decimal(new int[] { 100, 0, 0, 262144 });
            _coldCurrentNumeric.Name = "_ColdCurrentNumeric";
            _coldCurrentNumeric.Size = new System.Drawing.Size(75, 25);
            _coldCurrentNumeric.TabIndex = 1;
            _coldCurrentNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _coldCurrentNumeric.Value = new decimal(new int[] { 1, 0, 0, 262144 });
            // 
            // _coldCurrentNumericLabel
            // 
            _coldCurrentNumericLabel.AutoSize = true;
            _coldCurrentNumericLabel.Location = new System.Drawing.Point(34, 33);
            _coldCurrentNumericLabel.Name = "_ColdCurrentNumericLabel";
            _coldCurrentNumericLabel.Size = new System.Drawing.Size(126, 17);
            _coldCurrentNumericLabel.TabIndex = 0;
            _coldCurrentNumericLabel.Text = "C&URRENT LEVEL [A]:";
            _coldCurrentNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ConfigurationPanel
            // 
            BackColor = System.Drawing.Color.Transparent;
            Controls.Add(_groupBox);
            Name = "ConfigurationPanel";
            Size = new System.Drawing.Size(544, 333);
            _groupBox.ResumeLayout(false);
            _thermalTransientGroupBox.ResumeLayout(false);
            _thermalTransientGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_thermalVoltageLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_thermalVoltageLowLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_thermalVoltageHighLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_thermalVoltageNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_thermalCurrentNumeric).EndInit();
            _coldResistanceConfigGroupBox.ResumeLayout(false);
            _coldResistanceConfigGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_postTransientDelayNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_coldResistanceLowLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_coldResistanceHighLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_coldVoltageNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_coldCurrentNumeric).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox _groupBox;
        private System.Windows.Forms.Button _restoreDefaultsButton;
        private System.Windows.Forms.Button _applyNewConfigurationButton;
        private System.Windows.Forms.Button _applyConfigurationButton;
        private System.Windows.Forms.GroupBox _thermalTransientGroupBox;
        private System.Windows.Forms.Label _thermalVoltageLimitLabel;
        private System.Windows.Forms.NumericUpDown _thermalVoltageLimitNumeric;
        private System.Windows.Forms.NumericUpDown _thermalVoltageLowLimitNumeric;
        private System.Windows.Forms.Label _thermalVoltageLowLimitNumericLabel;
        private System.Windows.Forms.NumericUpDown _thermalVoltageHighLimitNumeric;
        private System.Windows.Forms.Label _thermalVoltageHighLimitNumericLabel;
        private System.Windows.Forms.NumericUpDown _thermalVoltageNumeric;
        private System.Windows.Forms.Label _thermalVoltageNumericLabel;
        private System.Windows.Forms.NumericUpDown _thermalCurrentNumeric;
        private System.Windows.Forms.Label _thermalCurrentNumericLabel;
        private System.Windows.Forms.GroupBox _coldResistanceConfigGroupBox;
        private System.Windows.Forms.CheckBox _checkContactsCheckBox;
        private System.Windows.Forms.NumericUpDown _postTransientDelayNumeric;
        private System.Windows.Forms.Label _postTransientDelayNumericLabel;
        private System.Windows.Forms.NumericUpDown _coldResistanceLowLimitNumeric;
        private System.Windows.Forms.Label _resistanceLowLimitNumericLabel;
        private System.Windows.Forms.NumericUpDown _coldResistanceHighLimitNumeric;
        private System.Windows.Forms.Label _resistanceHighLimitNumericLabel;
        private System.Windows.Forms.NumericUpDown _coldVoltageNumeric;
        private System.Windows.Forms.Label _coldVoltageNumericLabel;
        private System.Windows.Forms.NumericUpDown _coldCurrentNumeric;
        private System.Windows.Forms.Label _coldCurrentNumericLabel;
    }
}
