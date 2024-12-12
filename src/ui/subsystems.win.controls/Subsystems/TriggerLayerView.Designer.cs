using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class TriggerLayerView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(TriggerLayerView));
            _toolStripPanel = new ToolStripPanel();
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _applySettingsMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem.Click += new EventHandler(ApplySettingsMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsMenuItem_Click);
            _sendBusTriggerMenuItem = new ToolStripMenuItem();
            _sendBusTriggerMenuItem.Click += new EventHandler(SendBusTriggerMenuItem_Click);
            _countNumericLabel = new ToolStripLabel();
            _countNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _infiniteCountButton = new ToolStripButton();
            _infiniteCountButton.CheckStateChanged += new EventHandler(InfiniteCountButton_CheckStateChanged);
            _sourceComboLabel = new ToolStripLabel();
            _sourceComboBox = new ToolStripComboBox();
            _triggerConfigurationToolStrip = new ToolStrip();
            _triggerCondigureLabel = new ToolStripLabel();
            _inputLineNumericLabel = new ToolStripLabel();
            _inputLineNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _outputLineNumericLabel = new ToolStripLabel();
            _outputLineNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _bypassToggleButton = new ToolStripButton();
            _bypassToggleButton.CheckStateChanged += new EventHandler(BypassToggleButton_CheckStateChanged);
            _timingToolStrip = new ToolStrip();
            _layerTimingLabel = new ToolStripLabel();
            _delayNumericLabel = new ToolStripLabel();
            _delayNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _timerIntervalNumericLabel = new ToolStripLabel();
            _timerIntervalNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _toolStripPanel.SuspendLayout();
            _subsystemToolStrip.SuspendLayout();
            _triggerConfigurationToolStrip.SuspendLayout();
            _timingToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _toolStripPanel
            // 
            _toolStripPanel.BackColor = System.Drawing.Color.Transparent;
            _toolStripPanel.Controls.Add(_subsystemToolStrip);
            _toolStripPanel.Controls.Add(_triggerConfigurationToolStrip);
            _toolStripPanel.Controls.Add(_timingToolStrip);
            _toolStripPanel.Dock = DockStyle.Top;
            _toolStripPanel.Location = new System.Drawing.Point(1, 1);
            _toolStripPanel.Name = "_ToolStripPanel";
            _toolStripPanel.Orientation = Orientation.Horizontal;
            _toolStripPanel.RowMargin = new Padding(0);
            _toolStripPanel.Size = new System.Drawing.Size(395, 84);
            // 
            // _subsystemToolStrip
            // 
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.Dock = DockStyle.None;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _countNumericLabel, _countNumeric, _infiniteCountButton, _sourceComboLabel, _sourceComboBox });
            _subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(395, 28);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 3;
            // 
            // _subsystemSplitButton
            // 
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DoubleClickEnabled = true;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applySettingsMenuItem, _readSettingsMenuItem, _sendBusTriggerMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(53, 25);
            _subsystemSplitButton.Text = "Trig1";
            // 
            // _applySettingsMenuItem
            // 
            _applySettingsMenuItem.Name = "_applySettingsMenuItem";
            _applySettingsMenuItem.Size = new System.Drawing.Size(162, 22);
            _applySettingsMenuItem.Text = "Apply Settings";
            _applySettingsMenuItem.ToolTipText = "Applies settings onto the instrument";
            // 
            // _readSettingsMenuItem
            // 
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(162, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            _readSettingsMenuItem.ToolTipText = "Reads settings from the instrument";
            // 
            // _sendBusTriggerMenuItem
            // 
            _sendBusTriggerMenuItem.Name = "_SendBusTriggerMenuItem";
            _sendBusTriggerMenuItem.Size = new System.Drawing.Size(162, 22);
            _sendBusTriggerMenuItem.Text = "Send Bus Trigger";
            _sendBusTriggerMenuItem.ToolTipText = "Sends a buis trigger to the instrument";
            // 
            // _countNumericLabel
            // 
            _countNumericLabel.Name = "_CountNumericLabel";
            _countNumericLabel.Size = new System.Drawing.Size(43, 25);
            _countNumericLabel.Text = "Count:";
            // 
            // _countNumeric
            // 
            _countNumeric.AutoSize = false;
            _countNumeric.Name = "_CountNumeric";
            _countNumeric.Size = new System.Drawing.Size(60, 25);
            _countNumeric.Text = "0";
            _countNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // _infiniteCountButton
            // 
            _infiniteCountButton.Checked = true;
            _infiniteCountButton.CheckOnClick = true;
            _infiniteCountButton.CheckState = CheckState.Indeterminate;
            _infiniteCountButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _infiniteCountButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _infiniteCountButton.Name = "_InfiniteCountButton";
            _infiniteCountButton.Size = new System.Drawing.Size(37, 25);
            _infiniteCountButton.Text = "INF ?";
            _infiniteCountButton.ToolTipText = "Toggle to enable infinite count";
            // 
            // _sourceComboLabel
            // 
            _sourceComboLabel.Name = "_SourceComboLabel";
            _sourceComboLabel.Size = new System.Drawing.Size(46, 25);
            _sourceComboLabel.Text = "Source:";
            // 
            // _sourceComboBox
            // 
            _sourceComboBox.Name = "_SourceComboBox";
            _sourceComboBox.Size = new System.Drawing.Size(80, 28);
            _sourceComboBox.Text = "Immediate";
            _sourceComboBox.ToolTipText = "Selects the source";
            // 
            // _triggerConfigurationToolStrip
            // 
            _triggerConfigurationToolStrip.BackColor = System.Drawing.Color.Transparent;
            _triggerConfigurationToolStrip.Dock = DockStyle.None;
            _triggerConfigurationToolStrip.GripMargin = new Padding(0);
            _triggerConfigurationToolStrip.Items.AddRange(new ToolStripItem[] { _triggerCondigureLabel, _inputLineNumericLabel, _inputLineNumeric, _outputLineNumericLabel, _outputLineNumeric, _bypassToggleButton });
            _triggerConfigurationToolStrip.Location = new System.Drawing.Point(0, 28);
            _triggerConfigurationToolStrip.Name = "_TriggerConfigurationToolStrip";
            _triggerConfigurationToolStrip.Size = new System.Drawing.Size(395, 28);
            _triggerConfigurationToolStrip.Stretch = true;
            _triggerConfigurationToolStrip.TabIndex = 2;
            // 
            // _triggerCondigureLabel
            // 
            _triggerCondigureLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _triggerCondigureLabel.Margin = new Padding(6, 1, 0, 2);
            _triggerCondigureLabel.Name = "_TriggerCondigureLabel";
            _triggerCondigureLabel.Size = new System.Drawing.Size(44, 25);
            _triggerCondigureLabel.Text = "Trigger";
            // 
            // _inputLineNumericLabel
            // 
            _inputLineNumericLabel.Name = "_InputLineNumericLabel";
            _inputLineNumericLabel.Size = new System.Drawing.Size(63, 25);
            _inputLineNumericLabel.Text = "Input Line:";
            // 
            // _inputLineNumeric
            // 
            _inputLineNumeric.Name = "_InputLineNumeric";
            _inputLineNumeric.Size = new System.Drawing.Size(41, 25);
            _inputLineNumeric.Text = "3";
            _inputLineNumeric.ToolTipText = "Asynchronous trigger link input line";
            _inputLineNumeric.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // _outputLineNumericLabel
            // 
            _outputLineNumericLabel.Margin = new Padding(3, 1, 0, 2);
            _outputLineNumericLabel.Name = "_OutputLineNumericLabel";
            _outputLineNumericLabel.Size = new System.Drawing.Size(73, 25);
            _outputLineNumericLabel.Text = "Output Line:";
            // 
            // _outputLineNumeric
            // 
            _outputLineNumeric.Name = "_OutputLineNumeric";
            _outputLineNumeric.Size = new System.Drawing.Size(41, 25);
            _outputLineNumeric.Text = "0";
            _outputLineNumeric.ToolTipText = "Trigger Link output line";
            _outputLineNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // _bypassToggleButton
            // 
            _bypassToggleButton.Checked = true;
            _bypassToggleButton.CheckOnClick = true;
            _bypassToggleButton.CheckState = CheckState.Indeterminate;
            _bypassToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _bypassToggleButton.Image = (System.Drawing.Image)resources.GetObject("_BypassToggleButton.Image");
            _bypassToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _bypassToggleButton.Name = "_BypassToggleButton";
            _bypassToggleButton.Size = new System.Drawing.Size(55, 25);
            _bypassToggleButton.Text = "Bypass ?";
            _bypassToggleButton.ToolTipText = "Toggle to bypass or enable triggers";
            // 
            // _timingToolStrip
            // 
            _timingToolStrip.BackColor = System.Drawing.Color.Transparent;
            _timingToolStrip.Dock = DockStyle.None;
            _timingToolStrip.GripMargin = new Padding(0);
            _timingToolStrip.Items.AddRange(new ToolStripItem[] { _layerTimingLabel, _delayNumericLabel, _delayNumeric, _timerIntervalNumericLabel, _timerIntervalNumeric });
            _timingToolStrip.Location = new System.Drawing.Point(0, 56);
            _timingToolStrip.Name = "_TimingToolStrip";
            _timingToolStrip.Size = new System.Drawing.Size(395, 28);
            _timingToolStrip.Stretch = true;
            _timingToolStrip.TabIndex = 1;
            // 
            // _layerTimingLabel
            // 
            _layerTimingLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _layerTimingLabel.Margin = new Padding(6, 1, 0, 2);
            _layerTimingLabel.Name = "_LayerTimingLabel";
            _layerTimingLabel.Size = new System.Drawing.Size(43, 25);
            _layerTimingLabel.Text = "Timing";
            // 
            // _delayNumericLabel
            // 
            _delayNumericLabel.Name = "_DelayNumericLabel";
            _delayNumericLabel.Size = new System.Drawing.Size(50, 25);
            _delayNumericLabel.Text = "Delay, s:";
            // 
            // _delayNumeric
            // 
            _delayNumeric.AutoSize = false;
            _delayNumeric.Name = "_DelayNumeric";
            _delayNumeric.Size = new System.Drawing.Size(61, 25);
            _delayNumeric.Text = "3";
            _delayNumeric.ToolTipText = "Delay in seconds";
            _delayNumeric.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // _timerIntervalNumericLabel
            // 
            _timerIntervalNumericLabel.Margin = new Padding(3, 1, 0, 2);
            _timerIntervalNumericLabel.Name = "_TimerIntervalNumericLabel";
            _timerIntervalNumericLabel.Size = new System.Drawing.Size(51, 25);
            _timerIntervalNumericLabel.Text = "Timer, s:";
            // 
            // _timerIntervalNumeric
            // 
            _timerIntervalNumeric.AutoSize = false;
            _timerIntervalNumeric.Name = "_TimerIntervalNumeric";
            _timerIntervalNumeric.Size = new System.Drawing.Size(61, 25);
            _timerIntervalNumeric.Text = "10";
            _timerIntervalNumeric.ToolTipText = "Timer interval in seconds";
            _timerIntervalNumeric.Value = new decimal(new int[] { 9999, 0, 0, 196608 });
            // 
            // TriggerLayerView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_toolStripPanel);
            Name = "TriggerLayerView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(397, 85);
            _toolStripPanel.ResumeLayout(false);
            _toolStripPanel.PerformLayout();
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            _triggerConfigurationToolStrip.ResumeLayout(false);
            _triggerConfigurationToolStrip.PerformLayout();
            _timingToolStrip.ResumeLayout(false);
            _timingToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStripPanel _toolStripPanel;
        private ToolStrip _timingToolStrip;
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
        private ToolStripMenuItem _applySettingsMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private ToolStripMenuItem _sendBusTriggerMenuItem;
    }
}
