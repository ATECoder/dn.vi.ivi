using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class LimitView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LimitView));
            this._limitToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this._subsystemToolStrip = new System.Windows.Forms.ToolStrip();
            this._subsystemSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this._applySettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._readSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._performLimitStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._readLimitTestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._limitEnabledToggleButton = new System.Windows.Forms.ToolStripButton();
            this._autoClearToggleButton = new System.Windows.Forms.ToolStripButton();
            this._limitFailedButton = new System.Windows.Forms.ToolStripButton();
            this._upperLimitToolStrip = new System.Windows.Forms.ToolStrip();
            this._upperLimitLabel = new System.Windows.Forms.ToolStripLabel();
            this._upperLimitDecimalsNumericLabel = new System.Windows.Forms.ToolStripLabel();
            this._upperLimitDecimalsNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._upperLimitNumericLabel = new System.Windows.Forms.ToolStripLabel();
            this._upperLimitNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._upperLimitBitPatternNumericButton = new System.Windows.Forms.ToolStripButton();
            this._upperLimitBitPatternNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._lowerLimitToolStrip = new System.Windows.Forms.ToolStrip();
            this._lowerLimitLabel = new System.Windows.Forms.ToolStripLabel();
            this._lowerLimitDecimalsNumericLabel = new System.Windows.Forms.ToolStripLabel();
            this._lowerLimitDecimalsNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._lowerLimitNumericLabel = new System.Windows.Forms.ToolStripLabel();
            this._lowerLimitNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._lowerLimitBitPatternNumericButton = new System.Windows.Forms.ToolStripButton();
            this._lowerLimitBitPatternNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._limitToolStripPanel.SuspendLayout();
            this._subsystemToolStrip.SuspendLayout();
            this._upperLimitToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._upperLimitDecimalsNumeric.NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._upperLimitNumeric.NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._upperLimitBitPatternNumeric.NumericUpDown)).BeginInit();
            this._lowerLimitToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._lowerLimitDecimalsNumeric.NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._lowerLimitNumeric.NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._lowerLimitBitPatternNumeric.NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // _limitToolStripPanel
            // 
            this._limitToolStripPanel.BackColor = System.Drawing.Color.Transparent;
            this._limitToolStripPanel.Controls.Add(this._subsystemToolStrip);
            this._limitToolStripPanel.Controls.Add(this._upperLimitToolStrip);
            this._limitToolStripPanel.Controls.Add(this._lowerLimitToolStrip);
            this._limitToolStripPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._limitToolStripPanel.Location = new System.Drawing.Point(1, 1);
            this._limitToolStripPanel.Name = "_limitToolStripPanel";
            this._limitToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this._limitToolStripPanel.RowMargin = new System.Windows.Forms.Padding(0);
            this._limitToolStripPanel.Size = new System.Drawing.Size(443, 75);
            // 
            // _subsystemToolStrip
            // 
            this._subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            this._subsystemToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._subsystemToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this._subsystemToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._subsystemSplitButton,
            this._limitEnabledToggleButton,
            this._autoClearToggleButton,
            this._limitFailedButton});
            this._subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            this._subsystemToolStrip.Name = "_subsystemToolStrip";
            this._subsystemToolStrip.Size = new System.Drawing.Size(443, 25);
            this._subsystemToolStrip.Stretch = true;
            this._subsystemToolStrip.TabIndex = 3;
            // 
            // _subsystemSplitButton
            // 
            this._subsystemSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._subsystemSplitButton.DoubleClickEnabled = true;
            this._subsystemSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._applySettingsMenuItem,
            this._readSettingsMenuItem,
            this._performLimitStripMenuItem,
            this._readLimitTestMenuItem});
            this._subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._subsystemSplitButton.Name = "_subsystemSplitButton";
            this._subsystemSplitButton.Size = new System.Drawing.Size(55, 22);
            this._subsystemSplitButton.Text = "Limit1";
            this._subsystemSplitButton.ToolTipText = "Double-click to read limit state";
            // 
            // _applySettingsMenuItem
            // 
            this._applySettingsMenuItem.Name = "_applySettingsMenuItem";
            this._applySettingsMenuItem.Size = new System.Drawing.Size(184, 22);
            this._applySettingsMenuItem.Text = "Apply Settings";
            this._applySettingsMenuItem.ToolTipText = "Applies settings onto the instrument";
            this._applySettingsMenuItem.Click += new System.EventHandler(this.ApplySettingsMenuItem_Click);
            // 
            // _readSettingsMenuItem
            // 
            this._readSettingsMenuItem.Name = "_readSettingsMenuItem";
            this._readSettingsMenuItem.Size = new System.Drawing.Size(184, 22);
            this._readSettingsMenuItem.Text = "Read Settings";
            this._readSettingsMenuItem.ToolTipText = "Reads settings from the instrument";
            this._readSettingsMenuItem.Click += new System.EventHandler(this.ReadSettingsMenuItem_Click);
            // 
            // _performLimitStripMenuItem
            // 
            this._performLimitStripMenuItem.Name = "_performLimitStripMenuItem";
            this._performLimitStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this._performLimitStripMenuItem.Text = "Perform Limit Test";
            this._performLimitStripMenuItem.ToolTipText = "Perform limit test";
            this._performLimitStripMenuItem.Click += new System.EventHandler(this.PerformLimitStripMenuItem_Click);
            // 
            // _readLimitTestMenuItem
            // 
            this._readLimitTestMenuItem.Name = "_readLimitTestMenuItem";
            this._readLimitTestMenuItem.Size = new System.Drawing.Size(184, 22);
            this._readLimitTestMenuItem.Text = "Read Limit Test State";
            this._readLimitTestMenuItem.ToolTipText = "Read limit test result";
            this._readLimitTestMenuItem.Click += new System.EventHandler(this.ReadLimitTestMenuItem_Click);
            // 
            // _limitEnabledToggleButton
            // 
            this._limitEnabledToggleButton.Checked = true;
            this._limitEnabledToggleButton.CheckOnClick = true;
            this._limitEnabledToggleButton.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this._limitEnabledToggleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._limitEnabledToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._limitEnabledToggleButton.Name = "_limitEnabledToggleButton";
            this._limitEnabledToggleButton.Size = new System.Drawing.Size(56, 22);
            this._limitEnabledToggleButton.Text = "Disabled";
            this._limitEnabledToggleButton.ToolTipText = "toggle to enable or disable this limit";
            this._limitEnabledToggleButton.CheckStateChanged += new System.EventHandler(this.LimitEnabledToggleButton_CheckStateChanged);
            // 
            // _autoClearToggleButton
            // 
            this._autoClearToggleButton.Checked = true;
            this._autoClearToggleButton.CheckOnClick = true;
            this._autoClearToggleButton.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this._autoClearToggleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._autoClearToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._autoClearToggleButton.Name = "_autoClearToggleButton";
            this._autoClearToggleButton.Size = new System.Drawing.Size(90, 22);
            this._autoClearToggleButton.Text = "Auto Clear: Off";
            this._autoClearToggleButton.ToolTipText = "Toggle to turn auto clear on or off";
            this._autoClearToggleButton.CheckStateChanged += new System.EventHandler(this.AutoClearButton_CheckStateChanged);
            // 
            // _limitFailedButton
            // 
            this._limitFailedButton.Checked = true;
            this._limitFailedButton.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this._limitFailedButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._limitFailedButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._limitFailedButton.Name = "_limitFailedButton";
            this._limitFailedButton.Size = new System.Drawing.Size(47, 22);
            this._limitFailedButton.Text = "Failed?";
            this._limitFailedButton.ToolTipText = "displays fail state";
            this._limitFailedButton.CheckStateChanged += new System.EventHandler(this.LimitFailedToggleButton_CheckStateChanged);
            // 
            // _upperLimitToolStrip
            // 
            this._upperLimitToolStrip.BackColor = System.Drawing.Color.Transparent;
            this._upperLimitToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._upperLimitToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this._upperLimitToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._upperLimitLabel,
            this._upperLimitDecimalsNumericLabel,
            this._upperLimitDecimalsNumeric,
            this._upperLimitNumericLabel,
            this._upperLimitNumeric,
            this._upperLimitBitPatternNumericButton,
            this._upperLimitBitPatternNumeric});
            this._upperLimitToolStrip.Location = new System.Drawing.Point(0, 25);
            this._upperLimitToolStrip.Name = "_upperLimitToolStrip";
            this._upperLimitToolStrip.Size = new System.Drawing.Size(340, 25);
            this._upperLimitToolStrip.TabIndex = 2;
            // 
            // _upperLimitLabel
            // 
            this._upperLimitLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._upperLimitLabel.Margin = new System.Windows.Forms.Padding(6, 1, 0, 2);
            this._upperLimitLabel.Name = "_upperLimitLabel";
            this._upperLimitLabel.Size = new System.Drawing.Size(39, 22);
            this._upperLimitLabel.Text = "Upper";
            // 
            // _upperLimitDecimalsNumericLabel
            // 
            this._upperLimitDecimalsNumericLabel.Name = "_upperLimitDecimalsNumericLabel";
            this._upperLimitDecimalsNumericLabel.Size = new System.Drawing.Size(58, 22);
            this._upperLimitDecimalsNumericLabel.Text = "Decimals:";
            // 
            // _upperLimitDecimalsNumeric
            // 
            this._upperLimitDecimalsNumeric.Name = "_upperLimitDecimalsNumeric";
            // 
            // _upperLimitDecimalsNumeric
            // 
            this._upperLimitDecimalsNumeric.NumericUpDown.Location = new System.Drawing.Point(108, 1);
            this._upperLimitDecimalsNumeric.NumericUpDown.Name = "_upperLimitDecimalsNumeric";
            this._upperLimitDecimalsNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._upperLimitDecimalsNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._upperLimitDecimalsNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._upperLimitDecimalsNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._upperLimitDecimalsNumeric.NumericUpDown.Size = new System.Drawing.Size(41, 22);
            this._upperLimitDecimalsNumeric.NumericUpDown.TabIndex = 0;
            this._upperLimitDecimalsNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._upperLimitDecimalsNumeric.NumericUpDown.ValueChanged += new System.EventHandler(this.UpperLimitDecimalsNumeric_ValueChanged);
            this._upperLimitDecimalsNumeric.Size = new System.Drawing.Size(41, 22);
            this._upperLimitDecimalsNumeric.Text = "0";
            this._upperLimitDecimalsNumeric.ToolTipText = "Number of decimal places for setting the limits";
            this._upperLimitDecimalsNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _upperLimitNumericLabel
            // 
            this._upperLimitNumericLabel.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._upperLimitNumericLabel.Name = "_upperLimitNumericLabel";
            this._upperLimitNumericLabel.Size = new System.Drawing.Size(38, 22);
            this._upperLimitNumericLabel.Text = "Value:";
            // 
            // _upperLimitNumeric
            // 
            this._upperLimitNumeric.Name = "_upperLimitNumeric";
            // 
            // _upperLimitNumeric
            // 
            this._upperLimitNumeric.NumericUpDown.Location = new System.Drawing.Point(190, 1);
            this._upperLimitNumeric.NumericUpDown.Name = "_upperLimitNumeric";
            this._upperLimitNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._upperLimitNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._upperLimitNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._upperLimitNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._upperLimitNumeric.NumericUpDown.Size = new System.Drawing.Size(41, 22);
            this._upperLimitNumeric.NumericUpDown.TabIndex = 1;
            this._upperLimitNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._upperLimitNumeric.Size = new System.Drawing.Size(41, 22);
            this._upperLimitNumeric.Text = "0";
            this._upperLimitNumeric.ToolTipText = "Upper limit";
            this._upperLimitNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _upperLimitBitPatternNumericButton
            // 
            this._upperLimitBitPatternNumericButton.CheckOnClick = true;
            this._upperLimitBitPatternNumericButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._upperLimitBitPatternNumericButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._upperLimitBitPatternNumericButton.Name = "_upperLimitBitPatternNumericButton";
            this._upperLimitBitPatternNumericButton.Size = new System.Drawing.Size(65, 22);
            this._upperLimitBitPatternNumericButton.Text = "Source: 0x";
            this._upperLimitBitPatternNumericButton.ToolTipText = "Toggle to switch between decimal and hex display";
            this._upperLimitBitPatternNumericButton.CheckStateChanged += new System.EventHandler(this.UpperLimitBitPatternNumericButton_CheckStateChanged);
            // 
            // _upperLimitBitPatternNumeric
            // 
            this._upperLimitBitPatternNumeric.Name = "_upperLimitBitPatternNumeric";
            // 
            // _upperLimitBitPatternNumeric
            // 
            this._upperLimitBitPatternNumeric.NumericUpDown.Location = new System.Drawing.Point(296, 1);
            this._upperLimitBitPatternNumeric.NumericUpDown.Name = "_upperLimitBitPatternNumeric";
            this._upperLimitBitPatternNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._upperLimitBitPatternNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._upperLimitBitPatternNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._upperLimitBitPatternNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._upperLimitBitPatternNumeric.NumericUpDown.Size = new System.Drawing.Size(41, 22);
            this._upperLimitBitPatternNumeric.NumericUpDown.TabIndex = 2;
            this._upperLimitBitPatternNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._upperLimitBitPatternNumeric.Size = new System.Drawing.Size(41, 22);
            this._upperLimitBitPatternNumeric.Text = "0";
            this._upperLimitBitPatternNumeric.ToolTipText = "Upper limit bit pattern";
            this._upperLimitBitPatternNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _lowerLimitToolStrip
            // 
            this._lowerLimitToolStrip.BackColor = System.Drawing.Color.Transparent;
            this._lowerLimitToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._lowerLimitToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this._lowerLimitToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._lowerLimitLabel,
            this._lowerLimitDecimalsNumericLabel,
            this._lowerLimitDecimalsNumeric,
            this._lowerLimitNumericLabel,
            this._lowerLimitNumeric,
            this._lowerLimitBitPatternNumericButton,
            this._lowerLimitBitPatternNumeric});
            this._lowerLimitToolStrip.Location = new System.Drawing.Point(0, 50);
            this._lowerLimitToolStrip.Name = "_lowerLimitToolStrip";
            this._lowerLimitToolStrip.Size = new System.Drawing.Size(372, 25);
            this._lowerLimitToolStrip.TabIndex = 1;
            // 
            // _lowerLimitLabel
            // 
            this._lowerLimitLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lowerLimitLabel.Margin = new System.Windows.Forms.Padding(6, 1, 2, 2);
            this._lowerLimitLabel.Name = "_lowerLimitLabel";
            this._lowerLimitLabel.Size = new System.Drawing.Size(38, 22);
            this._lowerLimitLabel.Text = "Lower";
            // 
            // _lowerLimitDecimalsNumericLabel
            // 
            this._lowerLimitDecimalsNumericLabel.Name = "_lowerLimitDecimalsNumericLabel";
            this._lowerLimitDecimalsNumericLabel.Size = new System.Drawing.Size(58, 22);
            this._lowerLimitDecimalsNumericLabel.Text = "Decimals:";
            // 
            // _lowerLimitDecimalsNumeric
            // 
            this._lowerLimitDecimalsNumeric.Name = "_lowerLimitDecimalsNumeric";
            // 
            // _lowerLimitDecimalsNumeric
            // 
            this._lowerLimitDecimalsNumeric.NumericUpDown.Location = new System.Drawing.Point(109, 1);
            this._lowerLimitDecimalsNumeric.NumericUpDown.Name = "_lowerLimitDecimalsNumeric";
            this._lowerLimitDecimalsNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._lowerLimitDecimalsNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._lowerLimitDecimalsNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._lowerLimitDecimalsNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._lowerLimitDecimalsNumeric.NumericUpDown.Size = new System.Drawing.Size(41, 22);
            this._lowerLimitDecimalsNumeric.NumericUpDown.TabIndex = 0;
            this._lowerLimitDecimalsNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._lowerLimitDecimalsNumeric.NumericUpDown.ValueChanged += new System.EventHandler(this.LowerLimitDecimalsNumeric_ValueChanged);
            this._lowerLimitDecimalsNumeric.Size = new System.Drawing.Size(41, 22);
            this._lowerLimitDecimalsNumeric.Text = "0";
            this._lowerLimitDecimalsNumeric.ToolTipText = "Number of decimal places for setting the limits";
            this._lowerLimitDecimalsNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _lowerLimitNumericLabel
            // 
            this._lowerLimitNumericLabel.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._lowerLimitNumericLabel.Name = "_lowerLimitNumericLabel";
            this._lowerLimitNumericLabel.Size = new System.Drawing.Size(38, 22);
            this._lowerLimitNumericLabel.Text = "Value:";
            // 
            // _lowerLimitNumeric
            // 
            this._lowerLimitNumeric.Name = "_lowerLimitNumeric";
            // 
            // _lowerLimitNumeric
            // 
            this._lowerLimitNumeric.NumericUpDown.Location = new System.Drawing.Point(191, 1);
            this._lowerLimitNumeric.NumericUpDown.Name = "_lowerLimitNumeric";
            this._lowerLimitNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._lowerLimitNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._lowerLimitNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._lowerLimitNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._lowerLimitNumeric.NumericUpDown.Size = new System.Drawing.Size(41, 22);
            this._lowerLimitNumeric.NumericUpDown.TabIndex = 1;
            this._lowerLimitNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._lowerLimitNumeric.Size = new System.Drawing.Size(41, 22);
            this._lowerLimitNumeric.Text = "0";
            this._lowerLimitNumeric.ToolTipText = "Start delay in seconds";
            this._lowerLimitNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _lowerLimitBitPatternNumericButton
            // 
            this._lowerLimitBitPatternNumericButton.CheckOnClick = true;
            this._lowerLimitBitPatternNumericButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._lowerLimitBitPatternNumericButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._lowerLimitBitPatternNumericButton.Name = "_lowerLimitBitPatternNumericButton";
            this._lowerLimitBitPatternNumericButton.Size = new System.Drawing.Size(65, 22);
            this._lowerLimitBitPatternNumericButton.Text = "Source: 0x";
            this._lowerLimitBitPatternNumericButton.ToolTipText = "Toggle to switch between decimal and hex display";
            this._lowerLimitBitPatternNumericButton.CheckStateChanged += new System.EventHandler(this.LowerBitPatternNumericButton_CheckStateChanged);
            // 
            // _lowerLimitBitPatternNumeric
            // 
            this._lowerLimitBitPatternNumeric.Name = "_lowerLimitBitPatternNumeric";
            // 
            // _lowerLimitBitPatternNumeric
            // 
            this._lowerLimitBitPatternNumeric.NumericUpDown.Location = new System.Drawing.Point(297, 1);
            this._lowerLimitBitPatternNumeric.NumericUpDown.Name = "_lowerLimitBitPatternNumeric";
            this._lowerLimitBitPatternNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._lowerLimitBitPatternNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._lowerLimitBitPatternNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._lowerLimitBitPatternNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._lowerLimitBitPatternNumeric.NumericUpDown.Size = new System.Drawing.Size(41, 22);
            this._lowerLimitBitPatternNumeric.NumericUpDown.TabIndex = 2;
            this._lowerLimitBitPatternNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._lowerLimitBitPatternNumeric.Size = new System.Drawing.Size(41, 22);
            this._lowerLimitBitPatternNumeric.Text = "0";
            this._lowerLimitBitPatternNumeric.ToolTipText = "Lower limit bit pattern";
            this._lowerLimitBitPatternNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // LimitView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._limitToolStripPanel);
            this.Name = "LimitView";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(445, 76);
            this._limitToolStripPanel.ResumeLayout(false);
            this._limitToolStripPanel.PerformLayout();
            this._subsystemToolStrip.ResumeLayout(false);
            this._subsystemToolStrip.PerformLayout();
            this._upperLimitToolStrip.ResumeLayout(false);
            this._upperLimitToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._upperLimitDecimalsNumeric.NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._upperLimitNumeric.NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._upperLimitBitPatternNumeric.NumericUpDown)).EndInit();
            this._lowerLimitToolStrip.ResumeLayout(false);
            this._lowerLimitToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._lowerLimitDecimalsNumeric.NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._lowerLimitNumeric.NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._lowerLimitBitPatternNumeric.NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private ToolStripPanel _limitToolStripPanel;
        private ToolStrip _lowerLimitToolStrip;
        private ToolStripLabel _lowerLimitLabel;
        private ToolStripLabel _lowerLimitNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _lowerLimitNumeric;
        private ToolStripLabel _lowerLimitDecimalsNumericLabel;
        private ToolStrip _upperLimitToolStrip;
        private ToolStripLabel _upperLimitLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _upperLimitDecimalsNumeric;
        private ToolStripLabel _upperLimitNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _upperLimitNumeric;
        private ToolStripButton _upperLimitBitPatternNumericButton;
        private cc.isr.WinControls.ToolStripNumericUpDown _upperLimitBitPatternNumeric;
        private ToolStripButton _lowerLimitBitPatternNumericButton;
        private cc.isr.WinControls.ToolStripNumericUpDown _lowerLimitBitPatternNumeric;
        private ToolStrip _subsystemToolStrip;
        private ToolStripButton _limitEnabledToggleButton;
        private ToolStripButton _limitFailedButton;
        private ToolStripLabel _upperLimitDecimalsNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _lowerLimitDecimalsNumeric;
        private ToolStripButton _autoClearToggleButton;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _applySettingsMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private ToolStripMenuItem _performLimitStripMenuItem;
        private ToolStripMenuItem _readLimitTestMenuItem;
    }
}
