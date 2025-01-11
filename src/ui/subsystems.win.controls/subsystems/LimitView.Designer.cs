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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(LimitView));
            _limitToolStripPanel = new ToolStripPanel();
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _applySettingsMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem.Click += new EventHandler(ApplySettingsMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsMenuItem_Click);
            _performLimitStripMenuItem = new ToolStripMenuItem();
            _performLimitStripMenuItem.Click += new EventHandler(PerformLimitStripMenuItem_Click);
            _readLimitTestMenuItem = new ToolStripMenuItem();
            _readLimitTestMenuItem.Click += new EventHandler(ReadLimitTestMenuItem_Click);
            _limitEnabledToggleButton = new ToolStripButton();
            _limitEnabledToggleButton.CheckStateChanged += new EventHandler(LimitEnabledToggleButton_CheckStateChanged);
            _autoClearToggleButton = new ToolStripButton();
            _autoClearToggleButton.CheckStateChanged += new EventHandler(AutoClearButton_CheckStateChanged);
            _limitFailedButton = new ToolStripButton();
            _limitFailedButton.CheckStateChanged += new EventHandler(LimitFailedToggleButton_CheckStateChanged);
            _upperLimitToolStrip = new ToolStrip();
            _upperLimitLabel = new ToolStripLabel();
            _upperLimitDecimalsNumericLabel = new ToolStripLabel();
            _upperLimitDecimalsNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _upperLimitDecimalsNumeric.NumericUpDown.ValueChanged += new EventHandler(UpperLimitDecimalsNumeric_ValueChanged);
            _upperLimitNumericLabel = new ToolStripLabel();
            _upperLimitNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _upperLimitBitPatternNumericButton = new ToolStripButton();
            _upperLimitBitPatternNumericButton.CheckStateChanged += new EventHandler(UpperLimitBitPatternNumericButton_CheckStateChanged);
            _upperLimitBitPatternNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _lowerLimitToolStrip = new ToolStrip();
            _lowerLimitLabel = new ToolStripLabel();
            _lowerLimitDecimalsNumericLabel = new ToolStripLabel();
            _lowerLimitDecimalsNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _lowerLimitDecimalsNumeric.NumericUpDown.ValueChanged += new EventHandler(LowerLimitDecimalsNumeric_ValueChanged);
            _lowerLimitNumericLabel = new ToolStripLabel();
            _lowerLimitNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _lowerLimitBitPatternNumericButton = new ToolStripButton();
            _lowerLimitBitPatternNumericButton.CheckStateChanged += new EventHandler(LowerBitPatternNumericButton_CheckStateChanged);
            _lowerLimitBitPatternNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _limitToolStripPanel.SuspendLayout();
            _subsystemToolStrip.SuspendLayout();
            _upperLimitToolStrip.SuspendLayout();
            _lowerLimitToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _limitToolStripPanel
            // 
            _limitToolStripPanel.BackColor = System.Drawing.Color.Transparent;
            _limitToolStripPanel.Controls.Add(_subsystemToolStrip);
            _limitToolStripPanel.Controls.Add(_upperLimitToolStrip);
            _limitToolStripPanel.Controls.Add(_lowerLimitToolStrip);
            _limitToolStripPanel.Dock = DockStyle.Top;
            _limitToolStripPanel.Location = new System.Drawing.Point(1, 1);
            _limitToolStripPanel.Name = "_LimitToolStripPanel";
            _limitToolStripPanel.Orientation = Orientation.Horizontal;
            _limitToolStripPanel.RowMargin = new Padding(0);
            _limitToolStripPanel.Size = new System.Drawing.Size(387, 81);
            // 
            // _subsystemToolStrip
            // 
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.Dock = DockStyle.None;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _limitEnabledToggleButton, _autoClearToggleButton, _limitFailedButton });
            _subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(387, 25);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 3;
            // 
            // _subsystemSplitButton
            // 
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DoubleClickEnabled = true;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applySettingsMenuItem, _readSettingsMenuItem, _performLimitStripMenuItem, _readLimitTestMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.Image = (System.Drawing.Image)resources.GetObject("_SubsystemSplitButton.Image");
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(57, 22);
            _subsystemSplitButton.Text = "Limit1";
            _subsystemSplitButton.ToolTipText = "Double-click to read limit state";
            // 
            // _applySettingsMenuItem
            // 
            _applySettingsMenuItem.Name = "_applySettingsMenuItem";
            _applySettingsMenuItem.Size = new System.Drawing.Size(184, 22);
            _applySettingsMenuItem.Text = "Apply Settings";
            _applySettingsMenuItem.ToolTipText = "Applies settings onto the instrument";
            // 
            // _readSettingsMenuItem
            // 
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(184, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            _readSettingsMenuItem.ToolTipText = "Reads settings from the instrument";
            // 
            // _performLimitStripMenuItem
            // 
            _performLimitStripMenuItem.Name = "_PerformLimitStripMenuItem";
            _performLimitStripMenuItem.Size = new System.Drawing.Size(184, 22);
            _performLimitStripMenuItem.Text = "Perform Limit Test";
            _performLimitStripMenuItem.ToolTipText = "Perform limit test";
            // 
            // _readLimitTestMenuItem
            // 
            _readLimitTestMenuItem.Name = "_ReadLimitTestMenuItem";
            _readLimitTestMenuItem.Size = new System.Drawing.Size(184, 22);
            _readLimitTestMenuItem.Text = "Read Limit Test State";
            _readLimitTestMenuItem.ToolTipText = "Read limit test result";
            // 
            // _limitEnabledToggleButton
            // 
            _limitEnabledToggleButton.Checked = true;
            _limitEnabledToggleButton.CheckOnClick = true;
            _limitEnabledToggleButton.CheckState = CheckState.Indeterminate;
            _limitEnabledToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _limitEnabledToggleButton.Image = (System.Drawing.Image)resources.GetObject("_LimitEnabledToggleButton.Image");
            _limitEnabledToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _limitEnabledToggleButton.Name = "_LimitEnabledToggleButton";
            _limitEnabledToggleButton.Size = new System.Drawing.Size(56, 22);
            _limitEnabledToggleButton.Text = "Disabled";
            _limitEnabledToggleButton.ToolTipText = "toggle to enable or disable this limit";
            // 
            // _autoClearToggleButton
            // 
            _autoClearToggleButton.Checked = true;
            _autoClearToggleButton.CheckOnClick = true;
            _autoClearToggleButton.CheckState = CheckState.Indeterminate;
            _autoClearToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _autoClearToggleButton.Image = (System.Drawing.Image)resources.GetObject("_autoClearToggleButton.Image");
            _autoClearToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _autoClearToggleButton.Name = "_autoClearToggleButton";
            _autoClearToggleButton.Size = new System.Drawing.Size(90, 22);
            _autoClearToggleButton.Text = "Auto Clear: Off";
            _autoClearToggleButton.ToolTipText = "Toggle to turn auto clear on or off";
            // 
            // _limitFailedButton
            // 
            _limitFailedButton.Checked = true;
            _limitFailedButton.CheckState = CheckState.Indeterminate;
            _limitFailedButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _limitFailedButton.Image = (System.Drawing.Image)resources.GetObject("_LimitFailedButton.Image");
            _limitFailedButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _limitFailedButton.Name = "_LimitFailedButton";
            _limitFailedButton.Size = new System.Drawing.Size(47, 22);
            _limitFailedButton.Text = "Failed?";
            _limitFailedButton.ToolTipText = "displays fail state";
            // 
            // _upperLimitToolStrip
            // 
            _upperLimitToolStrip.BackColor = System.Drawing.Color.Transparent;
            _upperLimitToolStrip.Dock = DockStyle.None;
            _upperLimitToolStrip.GripMargin = new Padding(0);
            _upperLimitToolStrip.Items.AddRange(new ToolStripItem[] { _upperLimitLabel, _upperLimitDecimalsNumericLabel, _upperLimitDecimalsNumeric, _upperLimitNumericLabel, _upperLimitNumeric, _upperLimitBitPatternNumericButton, _upperLimitBitPatternNumeric });
            _upperLimitToolStrip.Location = new System.Drawing.Point(0, 25);
            _upperLimitToolStrip.Name = "_UpperLimitToolStrip";
            _upperLimitToolStrip.Size = new System.Drawing.Size(340, 28);
            _upperLimitToolStrip.TabIndex = 2;
            // 
            // _upperLimitLabel
            // 
            _upperLimitLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _upperLimitLabel.Margin = new Padding(6, 1, 0, 2);
            _upperLimitLabel.Name = "_UpperLimitLabel";
            _upperLimitLabel.Size = new System.Drawing.Size(39, 25);
            _upperLimitLabel.Text = "Upper";
            // 
            // _upperLimitDecimalsNumericLabel
            // 
            _upperLimitDecimalsNumericLabel.Name = "_UpperLimitDecimalsNumericLabel";
            _upperLimitDecimalsNumericLabel.Size = new System.Drawing.Size(58, 25);
            _upperLimitDecimalsNumericLabel.Text = "Decimals:";
            // 
            // _upperLimitDecimalsNumeric
            // 
            _upperLimitDecimalsNumeric.Name = "_UpperLimitDecimalsNumeric";
            _upperLimitDecimalsNumeric.Size = new System.Drawing.Size(41, 25);
            _upperLimitDecimalsNumeric.Text = "3";
            _upperLimitDecimalsNumeric.ToolTipText = "Number of decimal places for setting the limits";
            _upperLimitDecimalsNumeric.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // _upperLimitNumericLabel
            // 
            _upperLimitNumericLabel.Margin = new Padding(3, 1, 0, 2);
            _upperLimitNumericLabel.Name = "_UpperLimitNumericLabel";
            _upperLimitNumericLabel.Size = new System.Drawing.Size(38, 25);
            _upperLimitNumericLabel.Text = "Value:";
            // 
            // _upperLimitNumeric
            // 
            _upperLimitNumeric.Name = "_UpperLimitNumeric";
            _upperLimitNumeric.Size = new System.Drawing.Size(41, 25);
            _upperLimitNumeric.Text = "0";
            _upperLimitNumeric.ToolTipText = "Upper limit";
            _upperLimitNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // _upperLimitBitPatternNumericButton
            // 
            _upperLimitBitPatternNumericButton.CheckOnClick = true;
            _upperLimitBitPatternNumericButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _upperLimitBitPatternNumericButton.Image = (System.Drawing.Image)resources.GetObject("_UpperLimitBitPatternNumericButton.Image");
            _upperLimitBitPatternNumericButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _upperLimitBitPatternNumericButton.Name = "_UpperLimitBitPatternNumericButton";
            _upperLimitBitPatternNumericButton.Size = new System.Drawing.Size(65, 25);
            _upperLimitBitPatternNumericButton.Text = "Source: 0x";
            _upperLimitBitPatternNumericButton.ToolTipText = "Toggle to switch between decimal and hex display";
            // 
            // _upperLimitBitPatternNumeric
            // 
            _upperLimitBitPatternNumeric.Name = "_UpperLimitBitPatternNumeric";
            _upperLimitBitPatternNumeric.Size = new System.Drawing.Size(41, 25);
            _upperLimitBitPatternNumeric.Text = "0";
            _upperLimitBitPatternNumeric.ToolTipText = "Upper limit bit pattern";
            _upperLimitBitPatternNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // _lowerLimitToolStrip
            // 
            _lowerLimitToolStrip.BackColor = System.Drawing.Color.Transparent;
            _lowerLimitToolStrip.Dock = DockStyle.None;
            _lowerLimitToolStrip.GripMargin = new Padding(0);
            _lowerLimitToolStrip.Items.AddRange(new ToolStripItem[] { _lowerLimitLabel, _lowerLimitDecimalsNumericLabel, _lowerLimitDecimalsNumeric, _lowerLimitNumericLabel, _lowerLimitNumeric, _lowerLimitBitPatternNumericButton, _lowerLimitBitPatternNumeric });
            _lowerLimitToolStrip.Location = new System.Drawing.Point(0, 53);
            _lowerLimitToolStrip.Name = "_LowerLimitToolStrip";
            _lowerLimitToolStrip.Size = new System.Drawing.Size(341, 28);
            _lowerLimitToolStrip.TabIndex = 1;
            // 
            // _lowerLimitLabel
            // 
            _lowerLimitLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _lowerLimitLabel.Margin = new Padding(6, 1, 2, 2);
            _lowerLimitLabel.Name = "_LowerLimitLabel";
            _lowerLimitLabel.Size = new System.Drawing.Size(38, 25);
            _lowerLimitLabel.Text = "Lower";
            // 
            // _lowerLimitDecimalsNumericLabel
            // 
            _lowerLimitDecimalsNumericLabel.Name = "_LowerLimitDecimalsNumericLabel";
            _lowerLimitDecimalsNumericLabel.Size = new System.Drawing.Size(58, 25);
            _lowerLimitDecimalsNumericLabel.Text = "Decimals:";
            // 
            // _lowerLimitDecimalsNumeric
            // 
            _lowerLimitDecimalsNumeric.Name = "_LowerLimitDecimalsNumeric";
            _lowerLimitDecimalsNumeric.Size = new System.Drawing.Size(41, 25);
            _lowerLimitDecimalsNumeric.Text = "3";
            _lowerLimitDecimalsNumeric.ToolTipText = "Number of decimal places for setting the limits";
            _lowerLimitDecimalsNumeric.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // _lowerLimitNumericLabel
            // 
            _lowerLimitNumericLabel.Margin = new Padding(3, 1, 0, 2);
            _lowerLimitNumericLabel.Name = "_LowerLimitNumericLabel";
            _lowerLimitNumericLabel.Size = new System.Drawing.Size(38, 25);
            _lowerLimitNumericLabel.Text = "Value:";
            // 
            // _lowerLimitNumeric
            // 
            _lowerLimitNumeric.Name = "_LowerLimitNumeric";
            _lowerLimitNumeric.Size = new System.Drawing.Size(41, 25);
            _lowerLimitNumeric.Text = "0";
            _lowerLimitNumeric.ToolTipText = "Start delay in seconds";
            _lowerLimitNumeric.Value = new decimal(new int[] { 20, 0, 0, 196608 });
            // 
            // _lowerLimitBitPatternNumericButton
            // 
            _lowerLimitBitPatternNumericButton.CheckOnClick = true;
            _lowerLimitBitPatternNumericButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _lowerLimitBitPatternNumericButton.Image = (System.Drawing.Image)resources.GetObject("_LowerLimitBitPatternNumericButton.Image");
            _lowerLimitBitPatternNumericButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _lowerLimitBitPatternNumericButton.Name = "_LowerLimitBitPatternNumericButton";
            _lowerLimitBitPatternNumericButton.Size = new System.Drawing.Size(65, 25);
            _lowerLimitBitPatternNumericButton.Text = "Source: 0x";
            _lowerLimitBitPatternNumericButton.ToolTipText = "Toggle to switch between decimal and hex display";
            // 
            // _lowerLimitBitPatternNumeric
            // 
            _lowerLimitBitPatternNumeric.Name = "_LowerLimitBitPatternNumeric";
            _lowerLimitBitPatternNumeric.Size = new System.Drawing.Size(41, 25);
            _lowerLimitBitPatternNumeric.Text = "1";
            _lowerLimitBitPatternNumeric.ToolTipText = "Lower limit bit pattern";
            _lowerLimitBitPatternNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // LimitView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_limitToolStripPanel);
            Name = "LimitView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(389, 81);
            _limitToolStripPanel.ResumeLayout(false);
            _limitToolStripPanel.PerformLayout();
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            _upperLimitToolStrip.ResumeLayout(false);
            _upperLimitToolStrip.PerformLayout();
            _lowerLimitToolStrip.ResumeLayout(false);
            _lowerLimitToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
