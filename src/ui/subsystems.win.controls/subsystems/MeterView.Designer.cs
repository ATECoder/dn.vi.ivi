using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class MeterView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MeterView));
            _readingToolStrip = new ToolStrip();
            _readSplitButton = new ToolStripLabel();
            _readingElementTypesComboBoxLabel = new ToolStripLabel();
            _readingElementTypesComboBox = new ToolStripComboBox();
            _readingElementTypesComboBox.SelectedIndexChanged += new EventHandler(ReadingElementTypesComboBox_SelectedIndexChanged);
            _measureValueButton = new ToolStripButton();
            _measureValueButton.Click += new EventHandler(MeasureValueButton_Click);
            _functionConfigurationToolStrip = new ToolStrip();
            _functionLabel = new ToolStripLabel();
            _apertureNumericLabel = new ToolStripLabel();
            _apertureNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _senseRangeNumericLabel = new ToolStripLabel();
            _senseRangeNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _autoRangeToggleButton = new ToolStripButton();
            _autoRangeToggleButton.CheckStateChanged += new EventHandler(AutoRangeToggleButton_CheckStateChanged);
            _autoZeroToggleButton = new ToolStripButton();
            _autoZeroToggleButton.CheckStateChanged += new EventHandler(AutoZeroToggleButton_CheckStateChanged);
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _applyFunctionModeMenuItem = new ToolStripMenuItem();
            _applyFunctionModeMenuItem.Click += new EventHandler(ApplyFunctionModeButton_Click);
            _readFunctionModeMenuItem = new ToolStripMenuItem();
            _readFunctionModeMenuItem.Click += new EventHandler(ReadFunctionModeMenuItem_Click);
            _measureOptionsMenuItem = new ToolStripMenuItem();
            _measureImmediateMenuItem = new ToolStripMenuItem();
            _measureImmediateMenuItem.Click += new EventHandler(MeasureImmediateMenuItem_Click);
            _fetchOnMeasurementEventMenuItem = new ToolStripMenuItem();
            _autoInitiateMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem.Click += new EventHandler(ApplySettingsToolStripMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsToolStripMenuItem_Click);
            _senseFunctionComboBox = new ToolStripComboBox();
            _senseFunctionComboBox.SelectedIndexChanged += new EventHandler(SenseFunctionComboBox_SelectedIndexChanged);
            _applyFunctionModeButton = new ToolStripButton();
            _applyFunctionModeButton.Click += new EventHandler(ApplyFunctionModeButton_Click);
            _filterToolStrip = new ToolStrip();
            _filterLabel = new ToolStripLabel();
            _filterEnabledToggleButton = new ToolStripButton();
            _filterEnabledToggleButton.CheckStateChanged += new EventHandler(FilterEnabledToggleButton_CheckStateChanged);
            _filterCountNumericLabel = new ToolStripLabel();
            _filterCountNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _filterWindowNumericLabel = new ToolStripLabel();
            _filterWindowNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _windowTypeToggleButton = new ToolStripButton();
            _windowTypeToggleButton.CheckStateChanged += new EventHandler(WindowTypeToggleButton_CheckStateChanged);
            _infoToolStrip = new ToolStrip();
            _infoLabel = new ToolStripLabel();
            _autoDelayToggleButton = new ToolStripButton();
            _autoDelayToggleButton.CheckStateChanged += new EventHandler(AutoDelayToggleButton_CheckStateChanged);
            _openDetectorToggleButton = new ToolStripButton();
            _openDetectorToggleButton.CheckStateChanged += new EventHandler(OpenDetectorToggleButton_CheckStateChanged);
            _terminalStateReadButton = new ToolStripButton();
            _terminalStateReadButton.CheckStateChanged += new EventHandler(TerminalStateReadButton_CheckStateChanged);
            _terminalStateReadButton.Click += new EventHandler(TerminalStateReadButton_Click);
            _resolutionDigitsNumericLabel = new ToolStripLabel();
            _resolutionDigitsNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _toolStripPanel = new ToolStripPanel();
            _readingToolStrip.SuspendLayout();
            _functionConfigurationToolStrip.SuspendLayout();
            _subsystemToolStrip.SuspendLayout();
            _filterToolStrip.SuspendLayout();
            _infoToolStrip.SuspendLayout();
            _toolStripPanel.SuspendLayout();
            SuspendLayout();
            //
            // _readingToolStrip
            //
            _readingToolStrip.BackColor = System.Drawing.Color.Transparent;
            _readingToolStrip.Dock = DockStyle.None;
            _readingToolStrip.GripMargin = new Padding(0);
            _readingToolStrip.Items.AddRange(new ToolStripItem[] { _readSplitButton, _readingElementTypesComboBoxLabel, _readingElementTypesComboBox, _measureValueButton });
            _readingToolStrip.Location = new System.Drawing.Point(0, 25);
            _readingToolStrip.Name = "_ReadingToolStrip";
            _readingToolStrip.Size = new System.Drawing.Size(413, 25);
            _readingToolStrip.Stretch = true;
            _readingToolStrip.TabIndex = 1;
            _readingToolStrip.Text = "Meter Tool Strip";
            //
            // _readSplitButton
            //
            _readSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _readSplitButton.DoubleClickEnabled = true;
            _readSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _readSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _readSplitButton.Margin = new Padding(6, 1, 0, 2);
            _readSplitButton.Name = "_ReadSplitButton";
            _readSplitButton.Size = new System.Drawing.Size(34, 22);
            _readSplitButton.Text = "Read";
            //
            // _readingElementTypesComboBoxLabel
            //
            _readingElementTypesComboBoxLabel.Name = "_ReadingElementTypesComboBoxLabel";
            _readingElementTypesComboBoxLabel.Size = new System.Drawing.Size(34, 22);
            _readingElementTypesComboBoxLabel.Text = "Type:";
            //
            // _readingElementTypesComboBox
            //
            _readingElementTypesComboBox.Name = "_ReadingElementTypesComboBox";
            _readingElementTypesComboBox.Size = new System.Drawing.Size(195, 25);
            _readingElementTypesComboBox.ToolTipText = "Select reading type to display";
            //
            // _measureValueButton
            //
            _measureValueButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _measureValueButton.Image = (System.Drawing.Image)resources.GetObject("_MeasureValueButton.Image");
            _measureValueButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _measureValueButton.Name = "_MeasureValueButton";
            _measureValueButton.Size = new System.Drawing.Size(56, 22);
            _measureValueButton.Text = "Measure";
            _measureValueButton.ToolTipText = "Initiate a measurement and fetch value based on the fetch options";
            //
            // _functionConfigurationToolStrip
            //
            _functionConfigurationToolStrip.BackColor = System.Drawing.Color.Transparent;
            _functionConfigurationToolStrip.Dock = DockStyle.None;
            _functionConfigurationToolStrip.GripMargin = new Padding(0);
            _functionConfigurationToolStrip.Items.AddRange(new ToolStripItem[] { _functionLabel, _apertureNumericLabel, _apertureNumeric, _senseRangeNumericLabel, _senseRangeNumeric, _autoRangeToggleButton, _autoZeroToggleButton });
            _functionConfigurationToolStrip.Location = new System.Drawing.Point(0, 50);
            _functionConfigurationToolStrip.Name = "_FunctionConfigurationToolStrip";
            _functionConfigurationToolStrip.Size = new System.Drawing.Size(413, 28);
            _functionConfigurationToolStrip.Stretch = true;
            _functionConfigurationToolStrip.TabIndex = 2;
            //
            // _functionLabel
            //
            _functionLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _functionLabel.Margin = new Padding(6, 1, 0, 2);
            _functionLabel.Name = "_FunctionLabel";
            _functionLabel.Size = new System.Drawing.Size(28, 25);
            _functionLabel.Text = "Volt";
            _functionLabel.ToolTipText = "Selected function mode";
            //
            // _apertureNumericLabel
            //
            _apertureNumericLabel.Name = "_apertureNumericLabel";
            _apertureNumericLabel.Size = new System.Drawing.Size(56, 25);
            _apertureNumericLabel.Text = "Aperture:";
            //
            // _apertureNumeric
            //
            _apertureNumeric.Name = "_apertureNumeric";
            _apertureNumeric.Size = new System.Drawing.Size(41, 25);
            _apertureNumeric.Text = "3";
            _apertureNumeric.ToolTipText = "Measurement aperture in number of power line cycles (NPLC)";
            _apertureNumeric.Value = new decimal(new int[] { 3, 0, 0, 0 });
            //
            // _senseRangeNumericLabel
            //
            _senseRangeNumericLabel.Margin = new Padding(3, 1, 0, 2);
            _senseRangeNumericLabel.Name = "_SenseRangeNumericLabel";
            _senseRangeNumericLabel.Size = new System.Drawing.Size(43, 25);
            _senseRangeNumericLabel.Text = "Range:";
            //
            // _senseRangeNumeric
            //
            _senseRangeNumeric.Name = "_SenseRangeNumeric";
            _senseRangeNumeric.Size = new System.Drawing.Size(41, 25);
            _senseRangeNumeric.Text = "0";
            _senseRangeNumeric.ToolTipText = "Sense range";
            _senseRangeNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            //
            // _autoRangeToggleButton
            //
            _autoRangeToggleButton.Checked = true;
            _autoRangeToggleButton.CheckOnClick = true;
            _autoRangeToggleButton.CheckState = CheckState.Indeterminate;
            _autoRangeToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _autoRangeToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _autoRangeToggleButton.Name = "_autoRangeToggleButton";
            _autoRangeToggleButton.Size = new System.Drawing.Size(45, 25);
            _autoRangeToggleButton.Text = "Auto ?";
            _autoRangeToggleButton.ToolTipText = "Toggle auto range";
            //
            // _autoZeroToggleButton
            //
            _autoZeroToggleButton.Checked = true;
            _autoZeroToggleButton.CheckOnClick = true;
            _autoZeroToggleButton.CheckState = CheckState.Indeterminate;
            _autoZeroToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _autoZeroToggleButton.Image = (System.Drawing.Image)resources.GetObject("_autoZeroToggleButton.Image");
            _autoZeroToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _autoZeroToggleButton.Name = "_autoZeroToggleButton";
            _autoZeroToggleButton.Size = new System.Drawing.Size(43, 25);
            _autoZeroToggleButton.Text = "Zero ?";
            _autoZeroToggleButton.ToolTipText = "Toggle auto zero";
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.Dock = DockStyle.None;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _senseFunctionComboBox, _applyFunctionModeButton });
            _subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(413, 25);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 0;
            //
            // _subsystemSplitButton
            //
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applyFunctionModeMenuItem, _readFunctionModeMenuItem, _measureOptionsMenuItem, _applySettingsMenuItem, _readSettingsMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(58, 22);
            _subsystemSplitButton.Text = "Meter";
            _subsystemSplitButton.ToolTipText = "Selects function actions";
            //
            // _applyFunctionModeMenuItem
            //
            _applyFunctionModeMenuItem.Name = "_applyFunctionModeMenuItem";
            _applyFunctionModeMenuItem.Size = new System.Drawing.Size(185, 22);
            _applyFunctionModeMenuItem.Text = "Apply Function Mode";
            _applyFunctionModeMenuItem.ToolTipText = "Applies the selected function mode";
            //
            // _readFunctionModeMenuItem
            //
            _readFunctionModeMenuItem.Name = "_ReadFunctionModeMenuItem";
            _readFunctionModeMenuItem.Size = new System.Drawing.Size(185, 22);
            _readFunctionModeMenuItem.Text = "Read Function Mode";
            _readFunctionModeMenuItem.ToolTipText = "reads the instrument function mode";
            //
            // _measureOptionsMenuItem
            //
            _measureOptionsMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _measureImmediateMenuItem, _fetchOnMeasurementEventMenuItem, _autoInitiateMenuItem });
            _measureOptionsMenuItem.Enabled = false;
            _measureOptionsMenuItem.Name = "_MeasureOptionsMenuItem";
            _measureOptionsMenuItem.Size = new System.Drawing.Size(185, 22);
            _measureOptionsMenuItem.Text = "Measure Options";
            _measureOptionsMenuItem.ToolTipText = "Selects measurement options";
            //
            // _measureImmediateMenuItem
            //
            _measureImmediateMenuItem.Name = "_MeasureImmediateMenuItem";
            _measureImmediateMenuItem.Size = new System.Drawing.Size(225, 22);
            _measureImmediateMenuItem.Text = "Immediate";
            _measureImmediateMenuItem.ToolTipText = "Fetches a measurement irrespective of the 'Fetch of Measurement Event' setting";
            //
            // _fetchOnMeasurementEventMenuItem
            //
            _fetchOnMeasurementEventMenuItem.CheckOnClick = true;
            _fetchOnMeasurementEventMenuItem.Name = "_FetchOnMeasurementEventMenuItem";
            _fetchOnMeasurementEventMenuItem.Size = new System.Drawing.Size(225, 22);
            _fetchOnMeasurementEventMenuItem.Text = "Fetch on Measurement Event";
            _fetchOnMeasurementEventMenuItem.ToolTipText = "Fetch on measurement event; Pressing Measure, sets the measurement event handling" + " and initiates a measurement, which is fetched upon the event.";
            //
            // _autoInitiateMenuItem
            //
            _autoInitiateMenuItem.CheckOnClick = true;
            _autoInitiateMenuItem.Name = "_autoInitiateMenuItem";
            _autoInitiateMenuItem.Size = new System.Drawing.Size(225, 22);
            _autoInitiateMenuItem.Text = "Auto Initiate";
            _autoInitiateMenuItem.ToolTipText = "When checked, trigger plan is restarted following each data fetch";
            //
            // _applySettingsMenuItem
            //
            _applySettingsMenuItem.Name = "_applySettingsMenuItem";
            _applySettingsMenuItem.Size = new System.Drawing.Size(185, 22);
            _applySettingsMenuItem.Text = "Apply Settings";
            _applySettingsMenuItem.ToolTipText = "Applies settings";
            //
            // _readSettingsMenuItem
            //
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(185, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            _readSettingsMenuItem.ToolTipText = "Reads settings";
            //
            // _senseFunctionComboBox
            //
            _senseFunctionComboBox.Name = "_SenseFunctionComboBox";
            _senseFunctionComboBox.Size = new System.Drawing.Size(200, 25);
            _senseFunctionComboBox.Text = "Voltage";
            _senseFunctionComboBox.ToolTipText = "Selects the function";
            //
            // _applyFunctionModeButton
            //
            _applyFunctionModeButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _applyFunctionModeButton.Image = (System.Drawing.Image)resources.GetObject("_applyFunctionModeButton.Image");
            _applyFunctionModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _applyFunctionModeButton.Name = "_applyFunctionModeButton";
            _applyFunctionModeButton.Size = new System.Drawing.Size(27, 22);
            _applyFunctionModeButton.Text = "Set";
            _applyFunctionModeButton.ToolTipText = "Set the selected function";
            //
            // _filterToolStrip
            //
            _filterToolStrip.BackColor = System.Drawing.Color.Transparent;
            _filterToolStrip.Dock = DockStyle.None;
            _filterToolStrip.GripMargin = new Padding(0);
            _filterToolStrip.Items.AddRange(new ToolStripItem[] { _filterLabel, _filterEnabledToggleButton, _filterCountNumericLabel, _filterCountNumeric, _filterWindowNumericLabel, _filterWindowNumeric, _windowTypeToggleButton });
            _filterToolStrip.Location = new System.Drawing.Point(0, 78);
            _filterToolStrip.Name = "_FilterToolStrip";
            _filterToolStrip.Size = new System.Drawing.Size(413, 28);
            _filterToolStrip.Stretch = true;
            _filterToolStrip.TabIndex = 3;
            //
            // _filterLabel
            //
            _filterLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _filterLabel.Margin = new Padding(6, 1, 0, 2);
            _filterLabel.Name = "_FilterLabel";
            _filterLabel.Size = new System.Drawing.Size(33, 25);
            _filterLabel.Text = "Filter";
            _filterLabel.ToolTipText = "Averaging Window";
            //
            // _filterEnabledToggleButton
            //
            _filterEnabledToggleButton.Checked = true;
            _filterEnabledToggleButton.CheckOnClick = true;
            _filterEnabledToggleButton.CheckState = CheckState.Indeterminate;
            _filterEnabledToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _filterEnabledToggleButton.Image = (System.Drawing.Image)resources.GetObject("_FilterEnabledToggleButton.Image");
            _filterEnabledToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _filterEnabledToggleButton.Name = "_FilterEnabledToggleButton";
            _filterEnabledToggleButton.Size = new System.Drawing.Size(35, 25);
            _filterEnabledToggleButton.Text = "On ?";
            _filterEnabledToggleButton.ToolTipText = "Toggle filter enabled";
            //
            // _filterCountNumericLabel
            //
            _filterCountNumericLabel.Name = "_FilterCountNumericLabel";
            _filterCountNumericLabel.Size = new System.Drawing.Size(43, 25);
            _filterCountNumericLabel.Text = "Count:";
            //
            // _filterCountNumeric
            //
            _filterCountNumeric.AutoSize = false;
            _filterCountNumeric.Name = "_FilterCountNumeric";
            _filterCountNumeric.Size = new System.Drawing.Size(50, 25);
            _filterCountNumeric.Text = "0";
            _filterCountNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            //
            // _filterWindowNumericLabel
            //
            _filterWindowNumericLabel.Margin = new Padding(3, 1, 0, 2);
            _filterWindowNumericLabel.Name = "_FilterWindowNumericLabel";
            _filterWindowNumericLabel.Size = new System.Drawing.Size(75, 25);
            _filterWindowNumericLabel.Text = "Window [%]:";
            //
            // _filterWindowNumeric
            //
            _filterWindowNumeric.AutoSize = false;
            _filterWindowNumeric.Name = "_FilterWindowNumeric";
            _filterWindowNumeric.Size = new System.Drawing.Size(50, 25);
            _filterWindowNumeric.Text = "10";
            _filterWindowNumeric.ToolTipText = "Noise tolerance window in % of range";
            _filterWindowNumeric.Value = new decimal(new int[] { 9999, 0, 0, 196608 });
            //
            // _windowTypeToggleButton
            //
            _windowTypeToggleButton.Checked = true;
            _windowTypeToggleButton.CheckOnClick = true;
            _windowTypeToggleButton.CheckState = CheckState.Indeterminate;
            _windowTypeToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _windowTypeToggleButton.Image = (System.Drawing.Image)resources.GetObject("_WindowTypeToggleButton.Image");
            _windowTypeToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _windowTypeToggleButton.Name = "_WindowTypeToggleButton";
            _windowTypeToggleButton.Size = new System.Drawing.Size(60, 25);
            _windowTypeToggleButton.Text = "Moving ?";
            _windowTypeToggleButton.ToolTipText = "Toggle window type";
            //
            // _infoToolStrip
            //
            _infoToolStrip.BackColor = System.Drawing.Color.Transparent;
            _infoToolStrip.Dock = DockStyle.None;
            _infoToolStrip.GripMargin = new Padding(0);
            _infoToolStrip.Items.AddRange(new ToolStripItem[] { _infoLabel, _autoDelayToggleButton, _openDetectorToggleButton, _terminalStateReadButton, _resolutionDigitsNumericLabel, _resolutionDigitsNumeric });
            _infoToolStrip.Location = new System.Drawing.Point(0, 106);
            _infoToolStrip.Name = "_InfoToolStrip";
            _infoToolStrip.Size = new System.Drawing.Size(413, 28);
            _infoToolStrip.Stretch = true;
            _infoToolStrip.TabIndex = 4;
            //
            // _infoLabel
            //
            _infoLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _infoLabel.Margin = new Padding(6, 1, 0, 2);
            _infoLabel.Name = "_InfoLabel";
            _infoLabel.Size = new System.Drawing.Size(27, 25);
            _infoLabel.Text = "Info";
            //
            // _autoDelayToggleButton
            //
            _autoDelayToggleButton.Checked = true;
            _autoDelayToggleButton.CheckOnClick = true;
            _autoDelayToggleButton.CheckState = CheckState.Indeterminate;
            _autoDelayToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _autoDelayToggleButton.Image = (System.Drawing.Image)resources.GetObject("_autoDelayToggleButton.Image");
            _autoDelayToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _autoDelayToggleButton.Name = "_autoDelayToggleButton";
            _autoDelayToggleButton.Size = new System.Drawing.Size(51, 25);
            _autoDelayToggleButton.Text = "Delay: ?";
            //
            // _openDetectorToggleButton
            //
            _openDetectorToggleButton.Checked = true;
            _openDetectorToggleButton.CheckOnClick = true;
            _openDetectorToggleButton.CheckState = CheckState.Indeterminate;
            _openDetectorToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _openDetectorToggleButton.Image = (System.Drawing.Image)resources.GetObject("_OpenDetectorToggleButton.Image");
            _openDetectorToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _openDetectorToggleButton.Name = "_OpenDetectorToggleButton";
            _openDetectorToggleButton.Size = new System.Drawing.Size(96, 25);
            _openDetectorToggleButton.Text = "Open Detector ?";
            _openDetectorToggleButton.ToolTipText = "Toggle enabling open detector";
            //
            // _terminalStateReadButton
            //
            _terminalStateReadButton.Checked = true;
            _terminalStateReadButton.CheckState = CheckState.Indeterminate;
            _terminalStateReadButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _terminalStateReadButton.Image = (System.Drawing.Image)resources.GetObject("_TerminalStateReadButton.Image");
            _terminalStateReadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _terminalStateReadButton.Name = "_TerminalStateReadButton";
            _terminalStateReadButton.Size = new System.Drawing.Size(98, 25);
            _terminalStateReadButton.Text = "Terminals: Rear ?";
            _terminalStateReadButton.ToolTipText = "Read and display terminal state";
            //
            // _resolutionDigitsNumericLabel
            //
            _resolutionDigitsNumericLabel.Name = "_ResolutionDigitsNumericLabel";
            _resolutionDigitsNumericLabel.Size = new System.Drawing.Size(40, 25);
            _resolutionDigitsNumericLabel.Text = "Digits:";
            //
            // _resolutionDigitsNumeric
            //
            _resolutionDigitsNumeric.AutoSize = false;
            _resolutionDigitsNumeric.Name = "_ResolutionDigitsNumeric";
            _resolutionDigitsNumeric.Size = new System.Drawing.Size(31, 25);
            _resolutionDigitsNumeric.Text = "0";
            _resolutionDigitsNumeric.ToolTipText = "Sets the measurement resolution";
            _resolutionDigitsNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            //
            // _toolStripPanel
            //
            _toolStripPanel.BackColor = System.Drawing.Color.Transparent;
            _toolStripPanel.Controls.Add(_subsystemToolStrip);
            _toolStripPanel.Controls.Add(_readingToolStrip);
            _toolStripPanel.Controls.Add(_functionConfigurationToolStrip);
            _toolStripPanel.Controls.Add(_filterToolStrip);
            _toolStripPanel.Controls.Add(_infoToolStrip);
            _toolStripPanel.Dock = DockStyle.Top;
            _toolStripPanel.Location = new System.Drawing.Point(1, 1);
            _toolStripPanel.Name = "_ToolStripPanel";
            _toolStripPanel.Orientation = Orientation.Horizontal;
            _toolStripPanel.RowMargin = new Padding(3, 0, 0, 0);
            _toolStripPanel.Size = new System.Drawing.Size(413, 134);
            //
            // MeterView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_toolStripPanel);
            Name = "MeterView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(415, 132);
            _readingToolStrip.ResumeLayout(false);
            _readingToolStrip.PerformLayout();
            _functionConfigurationToolStrip.ResumeLayout(false);
            _functionConfigurationToolStrip.PerformLayout();
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            _filterToolStrip.ResumeLayout(false);
            _filterToolStrip.PerformLayout();
            _infoToolStrip.ResumeLayout(false);
            _infoToolStrip.PerformLayout();
            _toolStripPanel.ResumeLayout(false);
            _toolStripPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStrip _subsystemToolStrip;
        private ToolStripComboBox _senseFunctionComboBox;
        private ToolStrip _functionConfigurationToolStrip;
        private ToolStripLabel _functionLabel;
        private ToolStripLabel _apertureNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _apertureNumeric;
        private ToolStripLabel _senseRangeNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _senseRangeNumeric;
        private ToolStripButton _autoZeroToggleButton;
        private ToolStrip _filterToolStrip;
        private ToolStripLabel _filterLabel;
        private ToolStripLabel _filterCountNumericLabel;
        private ToolStripLabel _filterWindowNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _filterWindowNumeric;
        private ToolStripButton _autoRangeToggleButton;
        private cc.isr.WinControls.ToolStripNumericUpDown _filterCountNumeric;
        private ToolStripButton _applyFunctionModeButton;
        private ToolStripButton _filterEnabledToggleButton;
        private ToolStripButton _windowTypeToggleButton;
        private ToolStrip _infoToolStrip;
        private ToolStripLabel _infoLabel;
        private ToolStripButton _autoDelayToggleButton;
        private ToolStripButton _openDetectorToggleButton;
        private ToolStripButton _terminalStateReadButton;
        private ToolStripLabel _resolutionDigitsNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _resolutionDigitsNumeric;
        private ToolStrip _readingToolStrip;
        private ToolStripComboBox _readingElementTypesComboBox;
        private ToolStripLabel _readingElementTypesComboBoxLabel;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _readFunctionModeMenuItem;
        private ToolStripMenuItem _applyFunctionModeMenuItem;
        private ToolStripButton _measureValueButton;
        private ToolStripMenuItem _measureOptionsMenuItem;
        private ToolStripMenuItem _measureImmediateMenuItem;
        private ToolStripMenuItem _fetchOnMeasurementEventMenuItem;
        private ToolStripMenuItem _autoInitiateMenuItem;
        private ToolStripMenuItem _applySettingsMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private ToolStripLabel _readSplitButton;
        private ToolStripPanel _toolStripPanel;
    }
}
