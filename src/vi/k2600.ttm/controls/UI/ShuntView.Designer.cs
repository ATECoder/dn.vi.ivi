using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls
{
    public partial class ShuntView
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
            _shuntLayout = new System.Windows.Forms.TableLayoutPanel();
            _shuntDisplayLayout = new System.Windows.Forms.TableLayoutPanel();
            _shuntResistanceTextBox = new System.Windows.Forms.TextBox();
            _shuntResistanceTextBoxLabel = new System.Windows.Forms.Label();
            _shuntGroupBoxLayout = new System.Windows.Forms.TableLayoutPanel();
            _measureShuntResistanceButton = new System.Windows.Forms.Button();
            _shuntConfigureGroupBoxLayout = new System.Windows.Forms.TableLayoutPanel();
            _shuntResistanceConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            _restoreShuntResistanceDefaultsButton = new System.Windows.Forms.Button();
            _applyNewShuntResistanceConfigurationButton = new System.Windows.Forms.Button();
            _applyShuntResistanceConfigurationButton = new System.Windows.Forms.Button();
            _shuntResistanceCurrentRangeNumeric = new System.Windows.Forms.NumericUpDown();
            _shuntResistanceCurrentRangeNumericLabel = new System.Windows.Forms.Label();
            _shuntResistanceLowLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _shuntResistanceLowLimitNumericLabel = new System.Windows.Forms.Label();
            _shuntResistanceHighLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _shuntResistanceHighLimitNumericLabel = new System.Windows.Forms.Label();
            _shuntResistanceVoltageLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _shuntResistanceVoltageLimitNumericLabel = new System.Windows.Forms.Label();
            _shuntResistanceCurrentLevelNumeric = new System.Windows.Forms.NumericUpDown();
            _shuntResistanceCurrentLevelNumericLabel = new System.Windows.Forms.Label();
            _shuntLayout.SuspendLayout();
            _shuntDisplayLayout.SuspendLayout();
            _shuntGroupBoxLayout.SuspendLayout();
            _shuntConfigureGroupBoxLayout.SuspendLayout();
            _shuntResistanceConfigurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceCurrentRangeNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceLowLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceHighLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceVoltageLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceCurrentLevelNumeric).BeginInit();
            SuspendLayout();
            // 
            // _shuntLayout
            // 
            _shuntLayout.ColumnCount = 3;
            _shuntLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _shuntLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntLayout.Controls.Add(_shuntDisplayLayout, 1, 1);
            _shuntLayout.Controls.Add(_shuntGroupBoxLayout, 1, 3);
            _shuntLayout.Controls.Add(_shuntConfigureGroupBoxLayout, 1, 2);
            _shuntLayout.Location = new System.Drawing.Point(3, 9);
            _shuntLayout.Name = "_ShuntLayout";
            _shuntLayout.RowCount = 5;
            _shuntLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntLayout.Size = new System.Drawing.Size(466, 492);
            _shuntLayout.TabIndex = 1;
            // 
            // _shuntDisplayLayout
            // 
            _shuntDisplayLayout.BackColor = System.Drawing.Color.Black;
            _shuntDisplayLayout.ColumnCount = 3;
            _shuntDisplayLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65.0f));
            _shuntDisplayLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _shuntDisplayLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65.0f));
            _shuntDisplayLayout.Controls.Add(_shuntResistanceTextBox, 1, 2);
            _shuntDisplayLayout.Controls.Add(_shuntResistanceTextBoxLabel, 1, 1);
            _shuntDisplayLayout.Dock = System.Windows.Forms.DockStyle.Left;
            _shuntDisplayLayout.Location = new System.Drawing.Point(64, 11);
            _shuntDisplayLayout.Name = "_ShuntDisplayLayout";
            _shuntDisplayLayout.RowCount = 4;
            _shuntDisplayLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0f));
            _shuntDisplayLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntDisplayLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntDisplayLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0f));
            _shuntDisplayLayout.Size = new System.Drawing.Size(337, 75);
            _shuntDisplayLayout.TabIndex = 0;
            // 
            // _shuntResistanceTextBox
            // 
            _shuntResistanceTextBox.BackColor = System.Drawing.Color.Black;
            _shuntResistanceTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            _shuntResistanceTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _shuntResistanceTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _shuntResistanceTextBox.Location = new System.Drawing.Point(68, 25);
            _shuntResistanceTextBox.Name = "_ShuntResistanceTextBox";
            _shuntResistanceTextBox.ReadOnly = true;
            _shuntResistanceTextBox.Size = new System.Drawing.Size(201, 39);
            _shuntResistanceTextBox.TabIndex = 7;
            _shuntResistanceTextBox.Text = "0.000";
            _shuntResistanceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _shuntResistanceTextBoxLabel
            // 
            _shuntResistanceTextBoxLabel.AutoSize = true;
            _shuntResistanceTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _shuntResistanceTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _shuntResistanceTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _shuntResistanceTextBoxLabel.Location = new System.Drawing.Point(68, 5);
            _shuntResistanceTextBoxLabel.Name = "_ShuntResistanceTextBoxLabel";
            _shuntResistanceTextBoxLabel.Size = new System.Drawing.Size(201, 17);
            _shuntResistanceTextBoxLabel.TabIndex = 6;
            _shuntResistanceTextBoxLabel.Text = "SHUNT RESISTANCE [Ω]";
            _shuntResistanceTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // _shuntGroupBoxLayout
            // 
            _shuntGroupBoxLayout.ColumnCount = 3;
            _shuntGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _shuntGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntGroupBoxLayout.Controls.Add(_measureShuntResistanceButton, 1, 1);
            _shuntGroupBoxLayout.Location = new System.Drawing.Point(64, 417);
            _shuntGroupBoxLayout.Name = "_ShuntGroupBoxLayout";
            _shuntGroupBoxLayout.RowCount = 3;
            _shuntGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0f));
            _shuntGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0f));
            _shuntGroupBoxLayout.Size = new System.Drawing.Size(337, 63);
            _shuntGroupBoxLayout.TabIndex = 2;
            // 
            // _measureShuntResistanceButton
            // 
            _measureShuntResistanceButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _measureShuntResistanceButton.Location = new System.Drawing.Point(53, 13);
            _measureShuntResistanceButton.Name = "_MeasureShuntResistanceButton";
            _measureShuntResistanceButton.Size = new System.Drawing.Size(230, 36);
            _measureShuntResistanceButton.TabIndex = 3;
            _measureShuntResistanceButton.Text = "READ SHUNT RESISTANCE";
            _measureShuntResistanceButton.UseVisualStyleBackColor = true;
            // 
            // _shuntConfigureGroupBoxLayout
            // 
            _shuntConfigureGroupBoxLayout.ColumnCount = 3;
            _shuntConfigureGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntConfigureGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _shuntConfigureGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntConfigureGroupBoxLayout.Controls.Add(_shuntResistanceConfigurationGroupBox, 1, 1);
            _shuntConfigureGroupBoxLayout.Location = new System.Drawing.Point(64, 92);
            _shuntConfigureGroupBoxLayout.Name = "_ShuntConfigureGroupBoxLayout";
            _shuntConfigureGroupBoxLayout.RowCount = 3;
            _shuntConfigureGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntConfigureGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntConfigureGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntConfigureGroupBoxLayout.Size = new System.Drawing.Size(337, 319);
            _shuntConfigureGroupBoxLayout.TabIndex = 3;
            // 
            // _shuntResistanceConfigurationGroupBox
            // 
            _shuntResistanceConfigurationGroupBox.Controls.Add(_restoreShuntResistanceDefaultsButton);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_applyNewShuntResistanceConfigurationButton);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_applyShuntResistanceConfigurationButton);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceCurrentRangeNumeric);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceCurrentRangeNumericLabel);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceLowLimitNumeric);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceLowLimitNumericLabel);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceHighLimitNumeric);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceHighLimitNumericLabel);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceVoltageLimitNumeric);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceVoltageLimitNumericLabel);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceCurrentLevelNumeric);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceCurrentLevelNumericLabel);
            _shuntResistanceConfigurationGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            _shuntResistanceConfigurationGroupBox.Location = new System.Drawing.Point(39, 12);
            _shuntResistanceConfigurationGroupBox.Name = "_ShuntResistanceConfigurationGroupBox";
            _shuntResistanceConfigurationGroupBox.Size = new System.Drawing.Size(258, 294);
            _shuntResistanceConfigurationGroupBox.TabIndex = 2;
            _shuntResistanceConfigurationGroupBox.TabStop = false;
            _shuntResistanceConfigurationGroupBox.Text = "SHUNT RESISTANCE CONFIG.";
            // 
            // _restoreShuntResistanceDefaultsButton
            // 
            _restoreShuntResistanceDefaultsButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _restoreShuntResistanceDefaultsButton.Location = new System.Drawing.Point(14, 252);
            _restoreShuntResistanceDefaultsButton.Name = "_RestoreShuntResistanceDefaultsButton";
            _restoreShuntResistanceDefaultsButton.Size = new System.Drawing.Size(234, 30);
            _restoreShuntResistanceDefaultsButton.TabIndex = 12;
            _restoreShuntResistanceDefaultsButton.Text = "RESTORE DEFAULTS";
            _restoreShuntResistanceDefaultsButton.UseVisualStyleBackColor = true;
            // 
            // _applyNewShuntResistanceConfigurationButton
            // 
            _applyNewShuntResistanceConfigurationButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _applyNewShuntResistanceConfigurationButton.Location = new System.Drawing.Point(133, 210);
            _applyNewShuntResistanceConfigurationButton.Name = "_applyNewShuntResistanceConfigurationButton";
            _applyNewShuntResistanceConfigurationButton.Size = new System.Drawing.Size(115, 30);
            _applyNewShuntResistanceConfigurationButton.TabIndex = 11;
            _applyNewShuntResistanceConfigurationButton.Text = "APPLY CHANGES";
            _applyNewShuntResistanceConfigurationButton.UseVisualStyleBackColor = true;
            // 
            // _applyShuntResistanceConfigurationButton
            // 
            _applyShuntResistanceConfigurationButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _applyShuntResistanceConfigurationButton.Location = new System.Drawing.Point(11, 209);
            _applyShuntResistanceConfigurationButton.Name = "_applyShuntResistanceConfigurationButton";
            _applyShuntResistanceConfigurationButton.Size = new System.Drawing.Size(115, 30);
            _applyShuntResistanceConfigurationButton.TabIndex = 10;
            _applyShuntResistanceConfigurationButton.Text = "APPLY ALL";
            _applyShuntResistanceConfigurationButton.UseVisualStyleBackColor = true;
            // 
            // _shuntResistanceCurrentRangeNumeric
            // 
            _shuntResistanceCurrentRangeNumeric.DecimalPlaces = 3;
            _shuntResistanceCurrentRangeNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _shuntResistanceCurrentRangeNumeric.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            _shuntResistanceCurrentRangeNumeric.Location = new System.Drawing.Point(161, 62);
            _shuntResistanceCurrentRangeNumeric.Maximum = new decimal(new int[] { 1, 0, 0, 65536 });
            _shuntResistanceCurrentRangeNumeric.Name = "_ShuntResistanceCurrentRangeNumeric";
            _shuntResistanceCurrentRangeNumeric.Size = new System.Drawing.Size(75, 25);
            _shuntResistanceCurrentRangeNumeric.TabIndex = 3;
            _shuntResistanceCurrentRangeNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _shuntResistanceCurrentRangeNumeric.Value = new decimal(new int[] { 1, 0, 0, 131072 });
            // 
            // _shuntResistanceCurrentRangeNumericLabel
            // 
            _shuntResistanceCurrentRangeNumericLabel.AutoSize = true;
            _shuntResistanceCurrentRangeNumericLabel.Location = new System.Drawing.Point(25, 66);
            _shuntResistanceCurrentRangeNumericLabel.Name = "_ShuntResistanceCurrentRangeNumericLabel";
            _shuntResistanceCurrentRangeNumericLabel.Size = new System.Drawing.Size(134, 17);
            _shuntResistanceCurrentRangeNumericLabel.TabIndex = 2;
            _shuntResistanceCurrentRangeNumericLabel.Text = "CURRENT RANGE [A]:";
            _shuntResistanceCurrentRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _shuntResistanceLowLimitNumeric
            // 
            _shuntResistanceLowLimitNumeric.DecimalPlaces = 1;
            _shuntResistanceLowLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _shuntResistanceLowLimitNumeric.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            _shuntResistanceLowLimitNumeric.Location = new System.Drawing.Point(161, 172);
            _shuntResistanceLowLimitNumeric.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            _shuntResistanceLowLimitNumeric.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            _shuntResistanceLowLimitNumeric.Name = "_ShuntResistanceLowLimitNumeric";
            _shuntResistanceLowLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _shuntResistanceLowLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _shuntResistanceLowLimitNumeric.TabIndex = 9;
            _shuntResistanceLowLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _shuntResistanceLowLimitNumeric.Value = new decimal(new int[] { 1150, 0, 0, 0 });
            // 
            // _shuntResistanceLowLimitNumericLabel
            // 
            _shuntResistanceLowLimitNumericLabel.AutoSize = true;
            _shuntResistanceLowLimitNumericLabel.Location = new System.Drawing.Point(63, 176);
            _shuntResistanceLowLimitNumericLabel.Name = "_ShuntResistanceLowLimitNumericLabel";
            _shuntResistanceLowLimitNumericLabel.Size = new System.Drawing.Size(96, 17);
            _shuntResistanceLowLimitNumericLabel.TabIndex = 8;
            _shuntResistanceLowLimitNumericLabel.Text = "LO&W LIMIT [Ω]:";
            _shuntResistanceLowLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _shuntResistanceHighLimitNumeric
            // 
            _shuntResistanceHighLimitNumeric.DecimalPlaces = 1;
            _shuntResistanceHighLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _shuntResistanceHighLimitNumeric.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            _shuntResistanceHighLimitNumeric.Location = new System.Drawing.Point(161, 135);
            _shuntResistanceHighLimitNumeric.Maximum = new decimal(new int[] { 3000, 0, 0, 0 });
            _shuntResistanceHighLimitNumeric.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            _shuntResistanceHighLimitNumeric.Name = "_ShuntResistanceHighLimitNumeric";
            _shuntResistanceHighLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _shuntResistanceHighLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _shuntResistanceHighLimitNumeric.TabIndex = 7;
            _shuntResistanceHighLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _shuntResistanceHighLimitNumeric.Value = new decimal(new int[] { 1250, 0, 0, 0 });
            // 
            // _shuntResistanceHighLimitNumericLabel
            // 
            _shuntResistanceHighLimitNumericLabel.AutoSize = true;
            _shuntResistanceHighLimitNumericLabel.Location = new System.Drawing.Point(61, 139);
            _shuntResistanceHighLimitNumericLabel.Name = "_ShuntResistanceHighLimitNumericLabel";
            _shuntResistanceHighLimitNumericLabel.Size = new System.Drawing.Size(98, 17);
            _shuntResistanceHighLimitNumericLabel.TabIndex = 6;
            _shuntResistanceHighLimitNumericLabel.Text = "&HIGH LIMIT [Ω]:";
            _shuntResistanceHighLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _shuntResistanceVoltageLimitNumeric
            // 
            _shuntResistanceVoltageLimitNumeric.DecimalPlaces = 2;
            _shuntResistanceVoltageLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _shuntResistanceVoltageLimitNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            _shuntResistanceVoltageLimitNumeric.Location = new System.Drawing.Point(161, 98);
            _shuntResistanceVoltageLimitNumeric.Maximum = new decimal(new int[] { 40, 0, 0, 0 });
            _shuntResistanceVoltageLimitNumeric.Name = "_ShuntResistanceVoltageLimitNumeric";
            _shuntResistanceVoltageLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _shuntResistanceVoltageLimitNumeric.TabIndex = 5;
            _shuntResistanceVoltageLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _shuntResistanceVoltageLimitNumeric.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // _shuntResistanceVoltageLimitNumericLabel
            // 
            _shuntResistanceVoltageLimitNumericLabel.AutoSize = true;
            _shuntResistanceVoltageLimitNumericLabel.Location = new System.Drawing.Point(38, 102);
            _shuntResistanceVoltageLimitNumericLabel.Name = "_ShuntResistanceVoltageLimitNumericLabel";
            _shuntResistanceVoltageLimitNumericLabel.Size = new System.Drawing.Size(119, 17);
            _shuntResistanceVoltageLimitNumericLabel.TabIndex = 4;
            _shuntResistanceVoltageLimitNumericLabel.Text = "&VOLTAGE LIMIT [V]:";
            _shuntResistanceVoltageLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _shuntResistanceCurrentLevelNumeric
            // 
            _shuntResistanceCurrentLevelNumeric.DecimalPlaces = 4;
            _shuntResistanceCurrentLevelNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _shuntResistanceCurrentLevelNumeric.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            _shuntResistanceCurrentLevelNumeric.Location = new System.Drawing.Point(161, 27);
            _shuntResistanceCurrentLevelNumeric.Maximum = new decimal(new int[] { 100, 0, 0, 262144 });
            _shuntResistanceCurrentLevelNumeric.Name = "_ShuntResistanceCurrentLevelNumeric";
            _shuntResistanceCurrentLevelNumeric.Size = new System.Drawing.Size(75, 25);
            _shuntResistanceCurrentLevelNumeric.TabIndex = 1;
            _shuntResistanceCurrentLevelNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            _shuntResistanceCurrentLevelNumeric.Value = new decimal(new int[] { 1, 0, 0, 196608 });
            // 
            // _shuntResistanceCurrentLevelNumericLabel
            // 
            _shuntResistanceCurrentLevelNumericLabel.AutoSize = true;
            _shuntResistanceCurrentLevelNumericLabel.Location = new System.Drawing.Point(33, 31);
            _shuntResistanceCurrentLevelNumericLabel.Name = "_ShuntResistanceCurrentLevelNumericLabel";
            _shuntResistanceCurrentLevelNumericLabel.Size = new System.Drawing.Size(126, 17);
            _shuntResistanceCurrentLevelNumericLabel.TabIndex = 0;
            _shuntResistanceCurrentLevelNumericLabel.Text = "C&URRENT LEVEL [A]:";
            _shuntResistanceCurrentLevelNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ShuntView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_shuntLayout);
            Name = "ShuntView";
            Size = new System.Drawing.Size(472, 510);
            _shuntLayout.ResumeLayout(false);
            _shuntDisplayLayout.ResumeLayout(false);
            _shuntDisplayLayout.PerformLayout();
            _shuntGroupBoxLayout.ResumeLayout(false);
            _shuntConfigureGroupBoxLayout.ResumeLayout(false);
            _shuntResistanceConfigurationGroupBox.ResumeLayout(false);
            _shuntResistanceConfigurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceCurrentRangeNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceLowLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceHighLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceVoltageLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceCurrentLevelNumeric).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel _shuntLayout;
        private System.Windows.Forms.TableLayoutPanel _shuntDisplayLayout;
        private System.Windows.Forms.TextBox _shuntResistanceTextBox;
        private System.Windows.Forms.Label _shuntResistanceTextBoxLabel;
        private System.Windows.Forms.TableLayoutPanel _shuntGroupBoxLayout;
        private System.Windows.Forms.Button _measureShuntResistanceButton;
        private System.Windows.Forms.TableLayoutPanel _shuntConfigureGroupBoxLayout;
        private System.Windows.Forms.GroupBox _shuntResistanceConfigurationGroupBox;
        private System.Windows.Forms.Button _restoreShuntResistanceDefaultsButton;
        private System.Windows.Forms.Button _applyNewShuntResistanceConfigurationButton;
        private System.Windows.Forms.Button _applyShuntResistanceConfigurationButton;
        private System.Windows.Forms.NumericUpDown _shuntResistanceCurrentRangeNumeric;
        private System.Windows.Forms.Label _shuntResistanceCurrentRangeNumericLabel;
        private System.Windows.Forms.NumericUpDown _shuntResistanceLowLimitNumeric;
        private System.Windows.Forms.Label _shuntResistanceLowLimitNumericLabel;
        private System.Windows.Forms.NumericUpDown _shuntResistanceHighLimitNumeric;
        private System.Windows.Forms.Label _shuntResistanceHighLimitNumericLabel;
        private System.Windows.Forms.NumericUpDown _shuntResistanceVoltageLimitNumeric;
        private System.Windows.Forms.Label _shuntResistanceVoltageLimitNumericLabel;
        private System.Windows.Forms.NumericUpDown _shuntResistanceCurrentLevelNumeric;
        private System.Windows.Forms.Label _shuntResistanceCurrentLevelNumericLabel;
    }
}
