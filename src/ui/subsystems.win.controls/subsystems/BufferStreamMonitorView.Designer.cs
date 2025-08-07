using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class BufferStreamMonitorView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(BufferStreamMonitorView));
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _displayMenuItem = new ToolStripMenuItem();
            _clearBufferDisplayMenuItem = new ToolStripMenuItem();
            _displayBufferMenuItem = new ToolStripMenuItem();
            _triggerMenuItem = new ToolStripMenuItem();
            _initiateTriggerPlanMenuItem = new ToolStripMenuItem();
            _initiateTriggerPlanMenuItem.Click += new EventHandler(InitiateTriggerPlanMenuItem_Click);
            _abortStartTriggerPlanMenuItem = new ToolStripMenuItem();
            _abortStartTriggerPlanMenuItem.Click += new EventHandler(AbortStartTriggerPlanMenuItem_Click);
            _monitorActiveTriggerPlanMenuItem = new ToolStripMenuItem();
            _monitorActiveTriggerPlanMenuItem.Click += new EventHandler(MonitorActiveTriggerPlanMenuItem_Click);
            _initMonitorReadRepeatMenuItem = new ToolStripMenuItem();
            _initMonitorReadRepeatMenuItem.Click += new EventHandler(InitMonitorReadRepeatMenuItem_Click);
            _repeatMenuItem = new ToolStripMenuItem();
            _streamMenuItem = new ToolStripMenuItem();
            _toggleBufferStreamMenuItem = new ToolStripMenuItem();
            _toggleBufferStreamMenuItem.CheckStateChanged += new EventHandler(ToggleBufferStreamMenuItem_CheckStateChanged);
            _toggleBufferStreamMenuItem.Click += new EventHandler(ToggleBufferStreamMenuItem_Click);
            _abortButton = new ToolStripButton();
            _bufferCountLabel = new cc.isr.WinControls.ToolStripLabel();
            _lastPointNumberLabel = new cc.isr.WinControls.ToolStripLabel();
            _firstPointNumberLabel = new cc.isr.WinControls.ToolStripLabel();
            _bufferSizeNumericLabel = new ToolStripLabel();
            _bufferSizeNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _bufferSizeNumeric.Validating += new System.ComponentModel.CancelEventHandler(BufferSizeNumeric_Validating);
            _triggerStateLabel = new cc.isr.WinControls.ToolStripLabel();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _abortButton, _bufferCountLabel, _lastPointNumberLabel, _firstPointNumberLabel, _bufferSizeNumericLabel, _bufferSizeNumeric, _triggerStateLabel });
            _subsystemToolStrip.Location = new System.Drawing.Point(1, 1);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(356, 28);
            _subsystemToolStrip.TabIndex = 11;
            _subsystemToolStrip.Text = "Buffer";
            //
            // _subsystemSplitButton
            //
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _displayMenuItem, _triggerMenuItem, _streamMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(55, 25);
            _subsystemSplitButton.Text = "Buffer";
            //
            // _displayMenuItem
            //
            _displayMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _clearBufferDisplayMenuItem, _displayBufferMenuItem });
            _displayMenuItem.Name = "_DisplayMenuItem";
            _displayMenuItem.Size = new System.Drawing.Size(180, 22);
            _displayMenuItem.Text = "Display";
            //
            // _clearBufferDisplayMenuItem
            //
            _clearBufferDisplayMenuItem.Name = "_ClearBufferDisplayMenuItem";
            _clearBufferDisplayMenuItem.Size = new System.Drawing.Size(180, 22);
            _clearBufferDisplayMenuItem.Text = "Clear Buffer Display";
            _clearBufferDisplayMenuItem.ToolTipText = "Clears the display";
            //
            // _displayBufferMenuItem
            //
            _displayBufferMenuItem.Name = "_DisplayBufferMenuItem";
            _displayBufferMenuItem.Size = new System.Drawing.Size(180, 22);
            _displayBufferMenuItem.Text = "Display Buffer";
            _displayBufferMenuItem.ToolTipText = "Displays the buffer";
            //
            // _triggerMenuItem
            //
            _triggerMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _initiateTriggerPlanMenuItem, _abortStartTriggerPlanMenuItem, _monitorActiveTriggerPlanMenuItem, _initMonitorReadRepeatMenuItem, _repeatMenuItem });
            _triggerMenuItem.Name = "_TriggerMenuItem";
            _triggerMenuItem.Size = new System.Drawing.Size(180, 22);
            _triggerMenuItem.Text = "Trigger Monitor";
            //
            // _initiateTriggerPlanMenuItem
            //
            _initiateTriggerPlanMenuItem.Name = "_InitiateTriggerPlanMenuItem";
            _initiateTriggerPlanMenuItem.Size = new System.Drawing.Size(219, 22);
            _initiateTriggerPlanMenuItem.Text = "Initiate";
            _initiateTriggerPlanMenuItem.ToolTipText = "Sends the Initiate command to the instrument";
            //
            // _abortStartTriggerPlanMenuItem
            //
            _abortStartTriggerPlanMenuItem.Name = "_abortStartTriggerPlanMenuItem";
            _abortStartTriggerPlanMenuItem.Size = new System.Drawing.Size(219, 22);
            _abortStartTriggerPlanMenuItem.Text = "Abort, Clear, Initiate";
            _abortStartTriggerPlanMenuItem.ToolTipText = "Aborts, clears buffer and starts the trigger plan";
            //
            // _monitorActiveTriggerPlanMenuItem
            //
            _monitorActiveTriggerPlanMenuItem.Name = "_MonitorActiveTriggerPlanMenuItem";
            _monitorActiveTriggerPlanMenuItem.Size = new System.Drawing.Size(219, 22);
            _monitorActiveTriggerPlanMenuItem.Text = "Monitor Active Trigger State";
            _monitorActiveTriggerPlanMenuItem.ToolTipText = "Monitors active trigger state. Exits if trigger plan inactive";
            //
            // _initMonitorReadRepeatMenuItem
            //
            _initMonitorReadRepeatMenuItem.Name = "_InitMonitorReadRepeatMenuItem";
            _initMonitorReadRepeatMenuItem.Size = new System.Drawing.Size(219, 22);
            _initMonitorReadRepeatMenuItem.Text = "Init, Monitor, Read, Repeat";
            _initMonitorReadRepeatMenuItem.ToolTipText = "Initiates a trigger plan, monitors it, reads and displays buffer and repeats if a" + "uto repeat is checked";
            //
            // _repeatMenuItem
            //
            _repeatMenuItem.CheckOnClick = true;
            _repeatMenuItem.Name = "_RepeatMenuItem";
            _repeatMenuItem.Size = new System.Drawing.Size(219, 22);
            _repeatMenuItem.Text = "Repeat";
            _repeatMenuItem.ToolTipText = "Repeat initiating the trigger plan";
            //
            // _streamMenuItem
            //
            _streamMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _toggleBufferStreamMenuItem });
            _streamMenuItem.Name = "_StreamMenuItem";
            _streamMenuItem.Size = new System.Drawing.Size(180, 22);
            _streamMenuItem.Text = "Stream";
            //
            // _toggleBufferStreamMenuItem
            //
            _toggleBufferStreamMenuItem.CheckOnClick = false;
            _toggleBufferStreamMenuItem.Name = "_ToggleBufferStreamMenuItem";
            _toggleBufferStreamMenuItem.Size = new System.Drawing.Size(174, 22);
            _toggleBufferStreamMenuItem.Text = "Start Buffer Stream";
            _toggleBufferStreamMenuItem.ToolTipText = "Toggles streaming values";
            //
            // _abortButton
            //
            _abortButton.Alignment = ToolStripItemAlignment.Right;
            _abortButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _abortButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _abortButton.Name = "_abortButton";
            _abortButton.Size = new System.Drawing.Size(41, 25);
            _abortButton.Text = "Abort";
            _abortButton.ToolTipText = "Aborts triggering";
            //
            // _bufferCountLabel
            //
            _bufferCountLabel.Alignment = ToolStripItemAlignment.Right;
            _bufferCountLabel.Name = "_BufferCountLabel";
            _bufferCountLabel.Size = new System.Drawing.Size(13, 25);
            _bufferCountLabel.Text = "0";
            _bufferCountLabel.ToolTipText = "Buffer count";
            //
            // _lastPointNumberLabel
            //
            _lastPointNumberLabel.Alignment = ToolStripItemAlignment.Right;
            _lastPointNumberLabel.Name = "_LastPointNumberLabel";
            _lastPointNumberLabel.Size = new System.Drawing.Size(13, 25);
            _lastPointNumberLabel.Text = "2";
            _lastPointNumberLabel.ToolTipText = "Number of last buffer reading";
            //
            // _firstPointNumberLabel
            //
            _firstPointNumberLabel.Alignment = ToolStripItemAlignment.Right;
            _firstPointNumberLabel.Name = "_FirstPointNumberLabel";
            _firstPointNumberLabel.Size = new System.Drawing.Size(13, 25);
            _firstPointNumberLabel.Text = "1";
            _firstPointNumberLabel.ToolTipText = "Number of the first buffer reading";
            //
            // _bufferSizeNumericLabel
            //
            _bufferSizeNumericLabel.Name = "_BufferSizeNumericLabel";
            _bufferSizeNumericLabel.Size = new System.Drawing.Size(30, 25);
            _bufferSizeNumericLabel.Text = "Size:";
            //
            // _bufferSizeNumeric
            //
            _bufferSizeNumeric.AutoSize = false;
            _bufferSizeNumeric.Name = "_BufferSizeNumeric";
            _bufferSizeNumeric.Size = new System.Drawing.Size(71, 25);
            _bufferSizeNumeric.Text = "0";
            _bufferSizeNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            //
            // _triggerStateLabel
            //
            _triggerStateLabel.Name = "_TriggerStateLabel";
            _triggerStateLabel.Size = new System.Drawing.Size(26, 25);
            _triggerStateLabel.Text = "idle";
            _triggerStateLabel.ToolTipText = "Trigger state";
            //
            // BufferStreamMonitorView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_subsystemToolStrip);
            Name = "BufferStreamMonitorView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(358, 27);
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStrip _subsystemToolStrip;
        private cc.isr.WinControls.ToolStripLabel _bufferCountLabel;
        private cc.isr.WinControls.ToolStripLabel _lastPointNumberLabel;
        private cc.isr.WinControls.ToolStripLabel _firstPointNumberLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _bufferSizeNumeric;
        private ToolStripMenuItem _initiateTriggerPlanMenuItem;
        private ToolStripMenuItem _abortStartTriggerPlanMenuItem;
        private ToolStripMenuItem _monitorActiveTriggerPlanMenuItem;
        private ToolStripMenuItem _initMonitorReadRepeatMenuItem;
        private ToolStripMenuItem _repeatMenuItem;
        private ToolStripMenuItem _toggleBufferStreamMenuItem;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripLabel _bufferSizeNumericLabel;
        private ToolStripButton _abortButton;
        private cc.isr.WinControls.ToolStripLabel _triggerStateLabel;
        private ToolStripMenuItem _triggerMenuItem;
        private ToolStripMenuItem _displayMenuItem;
        private ToolStripMenuItem _clearBufferDisplayMenuItem;
        private ToolStripMenuItem _displayBufferMenuItem;
        private ToolStripMenuItem _streamMenuItem;
    }
}
