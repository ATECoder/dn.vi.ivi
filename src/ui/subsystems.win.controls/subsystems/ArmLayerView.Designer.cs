using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class ArmLayerView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this._toolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this._subsystemToolStrip = new System.Windows.Forms.ToolStrip();
            this._subsystemSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this._applyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._readMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._countNumericLabel = new System.Windows.Forms.ToolStripLabel();
            this._countNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._infiniteCountButton = new System.Windows.Forms.ToolStripButton();
            this._sourceComboLabel = new System.Windows.Forms.ToolStripLabel();
            this._sourceComboBox = new System.Windows.Forms.ToolStripComboBox();
            this._triggerConfigurationToolStrip = new System.Windows.Forms.ToolStrip();
            this._triggerCondigureLabel = new System.Windows.Forms.ToolStripLabel();
            this._inputLineNumericLabel = new System.Windows.Forms.ToolStripLabel();
            this._inputLineNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._outputLineNumericLabel = new System.Windows.Forms.ToolStripLabel();
            this._outputLineNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._bypassToggleButton = new System.Windows.Forms.ToolStripButton();
            this._layerTimingToolStrip = new System.Windows.Forms.ToolStrip();
            this._layerTimingLabel = new System.Windows.Forms.ToolStripLabel();
            this._delayNumericLabel = new System.Windows.Forms.ToolStripLabel();
            this._delayNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._timerIntervalNumericLabel = new System.Windows.Forms.ToolStripLabel();
            this._timerIntervalNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._toolStripPanel.SuspendLayout();
            this._subsystemToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._countNumeric.NumericUpDown)).BeginInit();
            this._triggerConfigurationToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._inputLineNumeric.NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._outputLineNumeric.NumericUpDown)).BeginInit();
            this._layerTimingToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._delayNumeric.NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._timerIntervalNumeric.NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // _toolStripPanel
            // 
            this._toolStripPanel.BackColor = System.Drawing.Color.Transparent;
            this._toolStripPanel.Controls.Add(this._subsystemToolStrip);
            this._toolStripPanel.Controls.Add(this._triggerConfigurationToolStrip);
            this._toolStripPanel.Controls.Add(this._layerTimingToolStrip);
            this._toolStripPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._toolStripPanel.Location = new System.Drawing.Point(1, 1);
            this._toolStripPanel.Name = "_toolStripPanel";
            this._toolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this._toolStripPanel.RowMargin = new System.Windows.Forms.Padding(0);
            this._toolStripPanel.Size = new System.Drawing.Size(430, 75);
            // 
            // _subsystemToolStrip
            // 
            this._subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            this._subsystemToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._subsystemToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this._subsystemToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._subsystemSplitButton,
            this._countNumericLabel,
            this._countNumeric,
            this._infiniteCountButton,
            this._sourceComboLabel,
            this._sourceComboBox});
            this._subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            this._subsystemToolStrip.Name = "_subsystemToolStrip";
            this._subsystemToolStrip.Size = new System.Drawing.Size(430, 25);
            this._subsystemToolStrip.Stretch = true;
            this._subsystemToolStrip.TabIndex = 3;
            // 
            // _subsystemSplitButton
            // 
            this._subsystemSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._subsystemSplitButton.DoubleClickEnabled = true;
            this._subsystemSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._applyMenuItem,
            this._readMenuItem});
            this._subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._subsystemSplitButton.Name = "_subsystemSplitButton";
            this._subsystemSplitButton.Size = new System.Drawing.Size(51, 22);
            this._subsystemSplitButton.Text = "Arm1";
            // 
            // _applyMenuItem
            // 
            this._applyMenuItem.Name = "_applyMenuItem";
            this._applyMenuItem.Size = new System.Drawing.Size(105, 22);
            this._applyMenuItem.Text = "Apply";
            this._applyMenuItem.ToolTipText = "Applies settings onto the instrument";
            this._applyMenuItem.Click += new System.EventHandler(this.ApplyMenuIte_Click);
            // 
            // _readMenuItem
            // 
            this._readMenuItem.Name = "_readMenuItem";
            this._readMenuItem.Size = new System.Drawing.Size(105, 22);
            this._readMenuItem.Text = "Read";
            this._readMenuItem.ToolTipText = "Reads settings from the instrument";
            this._readMenuItem.Click += new System.EventHandler(this.ReadMenuIte_Click);
            // 
            // _countNumericLabel
            // 
            this._countNumericLabel.Name = "_countNumericLabel";
            this._countNumericLabel.Size = new System.Drawing.Size(43, 22);
            this._countNumericLabel.Text = "Count:";
            // 
            // _countNumeric
            // 
            this._countNumeric.AutoSize = false;
            this._countNumeric.Name = "_countNumeric";
            // 
            // _countNumeric
            // 
            this._countNumeric.NumericUpDown.Location = new System.Drawing.Point(99, 1);
            this._countNumeric.NumericUpDown.Name = "_countNumeric";
            this._countNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._countNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._countNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._countNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._countNumeric.NumericUpDown.Size = new System.Drawing.Size(60, 22);
            this._countNumeric.NumericUpDown.TabIndex = 0;
            this._countNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._countNumeric.Size = new System.Drawing.Size(60, 22);
            this._countNumeric.Text = "0";
            this._countNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _infiniteCountButton
            // 
            this._infiniteCountButton.Checked = true;
            this._infiniteCountButton.CheckOnClick = true;
            this._infiniteCountButton.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this._infiniteCountButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._infiniteCountButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._infiniteCountButton.Name = "_infiniteCountButton";
            this._infiniteCountButton.Size = new System.Drawing.Size(37, 22);
            this._infiniteCountButton.Text = "INF ?";
            this._infiniteCountButton.ToolTipText = "Toggle to enable infinite count";
            this._infiniteCountButton.CheckStateChanged += new System.EventHandler(this.InfiniteCountButton_CheckStateChanged);
            // 
            // _sourceComboLabel
            // 
            this._sourceComboLabel.Name = "_sourceComboLabel";
            this._sourceComboLabel.Size = new System.Drawing.Size(46, 22);
            this._sourceComboLabel.Text = "Source:";
            // 
            // _sourceComboBox
            // 
            this._sourceComboBox.Name = "_sourceComboBox";
            this._sourceComboBox.Size = new System.Drawing.Size(80, 25);
            this._sourceComboBox.Text = "Immediate";
            this._sourceComboBox.ToolTipText = "Selects the source";
            // 
            // _triggerConfigurationToolStrip
            // 
            this._triggerConfigurationToolStrip.BackColor = System.Drawing.Color.Transparent;
            this._triggerConfigurationToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._triggerConfigurationToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this._triggerConfigurationToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._triggerCondigureLabel,
            this._inputLineNumericLabel,
            this._inputLineNumeric,
            this._outputLineNumericLabel,
            this._outputLineNumeric,
            this._bypassToggleButton});
            this._triggerConfigurationToolStrip.Location = new System.Drawing.Point(0, 25);
            this._triggerConfigurationToolStrip.Name = "_triggerConfigurationToolStrip";
            this._triggerConfigurationToolStrip.Size = new System.Drawing.Size(430, 25);
            this._triggerConfigurationToolStrip.Stretch = true;
            this._triggerConfigurationToolStrip.TabIndex = 2;
            // 
            // _triggerCondigureLabel
            // 
            this._triggerCondigureLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._triggerCondigureLabel.Margin = new System.Windows.Forms.Padding(6, 1, 0, 2);
            this._triggerCondigureLabel.Name = "_triggerCondigureLabel";
            this._triggerCondigureLabel.Size = new System.Drawing.Size(44, 22);
            this._triggerCondigureLabel.Text = "Trigger";
            // 
            // _inputLineNumericLabel
            // 
            this._inputLineNumericLabel.Name = "_inputLineNumericLabel";
            this._inputLineNumericLabel.Size = new System.Drawing.Size(63, 22);
            this._inputLineNumericLabel.Text = "Input Line:";
            // 
            // _inputLineNumeric
            // 
            this._inputLineNumeric.Name = "_inputLineNumeric";
            // 
            // _inputLineNumeric
            // 
            this._inputLineNumeric.NumericUpDown.Location = new System.Drawing.Point(118, 1);
            this._inputLineNumeric.NumericUpDown.Name = "_inputLineNumeric";
            this._inputLineNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._inputLineNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._inputLineNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._inputLineNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._inputLineNumeric.NumericUpDown.Size = new System.Drawing.Size(41, 22);
            this._inputLineNumeric.NumericUpDown.TabIndex = 0;
            this._inputLineNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._inputLineNumeric.Size = new System.Drawing.Size(41, 22);
            this._inputLineNumeric.Text = "0";
            this._inputLineNumeric.ToolTipText = "Asynchronous trigger link input line";
            this._inputLineNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _outputLineNumericLabel
            // 
            this._outputLineNumericLabel.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._outputLineNumericLabel.Name = "_outputLineNumericLabel";
            this._outputLineNumericLabel.Size = new System.Drawing.Size(73, 22);
            this._outputLineNumericLabel.Text = "Output Line:";
            // 
            // _outputLineNumeric
            // 
            this._outputLineNumeric.Name = "_outputLineNumeric";
            // 
            // _outputLineNumeric
            // 
            this._outputLineNumeric.NumericUpDown.Location = new System.Drawing.Point(235, 1);
            this._outputLineNumeric.NumericUpDown.Name = "_outputLineNumeric";
            this._outputLineNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._outputLineNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._outputLineNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._outputLineNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._outputLineNumeric.NumericUpDown.Size = new System.Drawing.Size(41, 22);
            this._outputLineNumeric.NumericUpDown.TabIndex = 1;
            this._outputLineNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._outputLineNumeric.Size = new System.Drawing.Size(41, 22);
            this._outputLineNumeric.Text = "0";
            this._outputLineNumeric.ToolTipText = "Trigger Link output line";
            this._outputLineNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _bypassToggleButton
            // 
            this._bypassToggleButton.Checked = true;
            this._bypassToggleButton.CheckOnClick = true;
            this._bypassToggleButton.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this._bypassToggleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._bypassToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._bypassToggleButton.Name = "_bypassToggleButton";
            this._bypassToggleButton.Size = new System.Drawing.Size(55, 22);
            this._bypassToggleButton.Text = "Bypass ?";
            this._bypassToggleButton.ToolTipText = "Toggle to bypass or enable triggers";
            this._bypassToggleButton.CheckStateChanged += new System.EventHandler(this.BypassToggleButton_CheckStateChanged);
            // 
            // _layerTimingToolStrip
            // 
            this._layerTimingToolStrip.BackColor = System.Drawing.Color.Transparent;
            this._layerTimingToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._layerTimingToolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this._layerTimingToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._layerTimingLabel,
            this._delayNumericLabel,
            this._delayNumeric,
            this._timerIntervalNumericLabel,
            this._timerIntervalNumeric});
            this._layerTimingToolStrip.Location = new System.Drawing.Point(0, 50);
            this._layerTimingToolStrip.Name = "_layerTimingToolStrip";
            this._layerTimingToolStrip.Size = new System.Drawing.Size(430, 25);
            this._layerTimingToolStrip.Stretch = true;
            this._layerTimingToolStrip.TabIndex = 1;
            // 
            // _layerTimingLabel
            // 
            this._layerTimingLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._layerTimingLabel.Margin = new System.Windows.Forms.Padding(6, 1, 2, 2);
            this._layerTimingLabel.Name = "_layerTimingLabel";
            this._layerTimingLabel.Size = new System.Drawing.Size(43, 22);
            this._layerTimingLabel.Text = "Timing";
            // 
            // _delayNumericLabel
            // 
            this._delayNumericLabel.Name = "_delayNumericLabel";
            this._delayNumericLabel.Size = new System.Drawing.Size(50, 22);
            this._delayNumericLabel.Text = "Delay, s:";
            // 
            // _delayNumeric
            // 
            this._delayNumeric.AutoSize = false;
            this._delayNumeric.Name = "_delayNumeric";
            // 
            // _delayNumeric
            // 
            this._delayNumeric.NumericUpDown.Location = new System.Drawing.Point(106, 1);
            this._delayNumeric.NumericUpDown.Name = "_delayNumeric";
            this._delayNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._delayNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._delayNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._delayNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._delayNumeric.NumericUpDown.Size = new System.Drawing.Size(61, 22);
            this._delayNumeric.NumericUpDown.TabIndex = 0;
            this._delayNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._delayNumeric.Size = new System.Drawing.Size(61, 22);
            this._delayNumeric.Text = "0";
            this._delayNumeric.ToolTipText = "Delay in seconds";
            this._delayNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _timerIntervalNumericLabel
            // 
            this._timerIntervalNumericLabel.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._timerIntervalNumericLabel.Name = "_timerIntervalNumericLabel";
            this._timerIntervalNumericLabel.Size = new System.Drawing.Size(51, 22);
            this._timerIntervalNumericLabel.Text = "Timer, s:";
            // 
            // _timerIntervalNumeric
            // 
            this._timerIntervalNumeric.AutoSize = false;
            this._timerIntervalNumeric.Name = "_timerIntervalNumeric";
            // 
            // _timerIntervalNumeric
            // 
            this._timerIntervalNumeric.NumericUpDown.Location = new System.Drawing.Point(221, 1);
            this._timerIntervalNumeric.NumericUpDown.Name = "_timerIntervalNumeric";
            this._timerIntervalNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._timerIntervalNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._timerIntervalNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._timerIntervalNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._timerIntervalNumeric.NumericUpDown.Size = new System.Drawing.Size(61, 22);
            this._timerIntervalNumeric.NumericUpDown.TabIndex = 1;
            this._timerIntervalNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._timerIntervalNumeric.Size = new System.Drawing.Size(61, 22);
            this._timerIntervalNumeric.Text = "0";
            this._timerIntervalNumeric.ToolTipText = "Timer interval in seconds";
            this._timerIntervalNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // ArmLayerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._toolStripPanel);
            this.Name = "ArmLayerView";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(432, 79);
            this._toolStripPanel.ResumeLayout(false);
            this._toolStripPanel.PerformLayout();
            this._subsystemToolStrip.ResumeLayout(false);
            this._subsystemToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._countNumeric.NumericUpDown)).EndInit();
            this._triggerConfigurationToolStrip.ResumeLayout(false);
            this._triggerConfigurationToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._inputLineNumeric.NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._outputLineNumeric.NumericUpDown)).EndInit();
            this._layerTimingToolStrip.ResumeLayout(false);
            this._layerTimingToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._delayNumeric.NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._timerIntervalNumeric.NumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private ToolStripPanel _toolStripPanel;
        private ToolStrip _layerTimingToolStrip;
        private ToolStripLabel _layerTimingLabel;
        private ToolStripLabel _timerIntervalNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _timerIntervalNumeric;
        private ToolStripLabel _delayNumericLabel;
        private ToolStrip _triggerConfigurationToolStrip;
        private ToolStripLabel _triggerCondigureLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _inputLineNumeric;
        private ToolStripLabel _outputLineNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _outputLineNumeric;
        private ToolStripButton _bypassToggleButton;
        private ToolStrip _subsystemToolStrip;
        private ToolStripLabel _inputLineNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _delayNumeric;
        private ToolStripLabel _countNumericLabel;
        private ToolStripButton _infiniteCountButton;
        private cc.isr.WinControls.ToolStripNumericUpDown _countNumeric;
        private ToolStripLabel _sourceComboLabel;
        private ToolStripComboBox _sourceComboBox;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _applyMenuItem;
        private ToolStripMenuItem _readMenuItem;
    }
}
