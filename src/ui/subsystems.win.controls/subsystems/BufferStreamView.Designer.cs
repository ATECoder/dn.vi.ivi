using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class BufferStreamView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(BufferStreamView));
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _displayBufferMenuItem = new ToolStripMenuItem();
            _displayBufferMenuItem.Click += new EventHandler(DisplayBufferMenuItem_Click);
            _clearBufferDisplayMenuItem = new ToolStripMenuItem();
            _clearBufferDisplayMenuItem.Click += new EventHandler(ClearBufferDisplayMenuItem_Click);
            _bufferStreamingMenuItem = new ToolStripMenuItem();
            _explainBufferStreamingMenuItem = new ToolStripMenuItem();
            _explainBufferStreamingMenuItem.Click += new EventHandler(ExplainBufferStreamingMenuItem_Click);
            _usingScanCardMenuItem = new ToolStripMenuItem();
            _configureStreamingMenuItem = new ToolStripMenuItem();
            _configureStreamingMenuItem.CheckStateChanged += new EventHandler(ConfigureStreamingMenuItem_CheckStateChanged);
            _startBufferStreamMenuItem = new ToolStripMenuItem();
            _startBufferStreamMenuItem.CheckStateChanged += new EventHandler(StartBufferStreamMenuItem_CheckStateChanged);
            _abortButton = new ToolStripButton();
            _bufferCountLabel = new cc.isr.WinControls.ToolStripLabel();
            _lastPointNumberLabel = new cc.isr.WinControls.ToolStripLabel();
            _firstPointNumberLabel = new cc.isr.WinControls.ToolStripLabel();
            _bufferSizeNumericLabel = new ToolStripLabel();
            _bufferSizeNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _bufferSizeNumeric.Validating += new System.ComponentModel.CancelEventHandler(BufferSizeNumeric_Validating);
            _triggerStateLabel = new cc.isr.WinControls.ToolStripLabel();
            _assertBusTriggerButton = new ToolStripButton();
            _assertBusTriggerButton.Click += new EventHandler(AssertBusTriggerButton_Click);
            UseTriggerPlanMenuItem = new ToolStripMenuItem();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _abortButton, _bufferCountLabel, _lastPointNumberLabel, _firstPointNumberLabel, _bufferSizeNumericLabel, _bufferSizeNumeric, _triggerStateLabel, _assertBusTriggerButton });
            _subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Padding = new Padding(0);
            _subsystemToolStrip.Size = new System.Drawing.Size(349, 27);
            _subsystemToolStrip.TabIndex = 11;
            _subsystemToolStrip.Text = "Buffer";
            //
            // _subsystemSplitButton
            //
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _displayBufferMenuItem, _clearBufferDisplayMenuItem, _bufferStreamingMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(55, 24);
            _subsystemSplitButton.Text = "Buffer";
            //
            // _displayBufferMenuItem
            //
            _displayBufferMenuItem.Name = "_DisplayBufferMenuItem";
            _displayBufferMenuItem.Size = new System.Drawing.Size(180, 22);
            _displayBufferMenuItem.Text = "Display Buffer";
            _displayBufferMenuItem.ToolTipText = "Displays the buffer";
            //
            // _clearBufferDisplayMenuItem
            //
            _clearBufferDisplayMenuItem.Name = "_ClearBufferDisplayMenuItem";
            _clearBufferDisplayMenuItem.Size = new System.Drawing.Size(180, 22);
            _clearBufferDisplayMenuItem.Text = "Clear Buffer Display";
            _clearBufferDisplayMenuItem.ToolTipText = "Clears the display";
            //
            // _bufferStreamingMenuItem
            //
            _bufferStreamingMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _explainBufferStreamingMenuItem, _usingScanCardMenuItem, UseTriggerPlanMenuItem, _configureStreamingMenuItem, _startBufferStreamMenuItem });
            _bufferStreamingMenuItem.Name = "_BufferStreamingMenuItem";
            _bufferStreamingMenuItem.Size = new System.Drawing.Size(180, 22);
            _bufferStreamingMenuItem.Text = "Buffer Streaming";
            _bufferStreamingMenuItem.ToolTipText = "Select action";
            //
            // _explainBufferStreamingMenuItem
            //
            _explainBufferStreamingMenuItem.Name = "_ExplainBufferStreamingMenuItem";
            _explainBufferStreamingMenuItem.Size = new System.Drawing.Size(180, 22);
            _explainBufferStreamingMenuItem.Text = "Info";
            _explainBufferStreamingMenuItem.ToolTipText = "Display buffer streaming settings";
            //
            // _usingScanCardMenuItem
            //
            _usingScanCardMenuItem.CheckOnClick = true;
            _usingScanCardMenuItem.Name = "_UsingScanCardMenuItem";
            _usingScanCardMenuItem.Size = new System.Drawing.Size(180, 22);
            _usingScanCardMenuItem.Text = "Use Scan Card";
            _usingScanCardMenuItem.ToolTipText = "Use scan card";
            //
            // _configureStreamingMenuItem
            //
            _configureStreamingMenuItem.CheckOnClick = true;
            _configureStreamingMenuItem.Name = "_ConfigureStreamingMenuItem";
            _configureStreamingMenuItem.Size = new System.Drawing.Size(180, 22);
            _configureStreamingMenuItem.Text = "Configure";
            _configureStreamingMenuItem.ToolTipText = "Configure buffer streaming";
            //
            // _startBufferStreamMenuItem
            //
            _startBufferStreamMenuItem.CheckOnClick = true;
            _startBufferStreamMenuItem.Name = "_StartBufferStreamMenuItem";
            _startBufferStreamMenuItem.Size = new System.Drawing.Size(180, 22);
            _startBufferStreamMenuItem.Text = "Start";
            _startBufferStreamMenuItem.ToolTipText = "Continuously reads new values while a trigger plan is active";
            //
            // _abortButton
            //
            _abortButton.Alignment = ToolStripItemAlignment.Right;
            _abortButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _abortButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _abortButton.Name = "_abortButton";
            _abortButton.Size = new System.Drawing.Size(45, 24);
            _abortButton.Text = "Abort";
            _abortButton.ToolTipText = "Aborts triggering";
            //
            // _bufferCountLabel
            //
            _bufferCountLabel.Alignment = ToolStripItemAlignment.Right;
            _bufferCountLabel.Name = "_BufferCountLabel";
            _bufferCountLabel.Size = new System.Drawing.Size(15, 24);
            _bufferCountLabel.Text = "0";
            _bufferCountLabel.ToolTipText = "Buffer count";
            //
            // _lastPointNumberLabel
            //
            _lastPointNumberLabel.Alignment = ToolStripItemAlignment.Right;
            _lastPointNumberLabel.Name = "_LastPointNumberLabel";
            _lastPointNumberLabel.Size = new System.Drawing.Size(15, 24);
            _lastPointNumberLabel.Text = "2";
            _lastPointNumberLabel.ToolTipText = "Number of last buffer reading";
            //
            // _firstPointNumberLabel
            //
            _firstPointNumberLabel.Alignment = ToolStripItemAlignment.Right;
            _firstPointNumberLabel.Name = "_FirstPointNumberLabel";
            _firstPointNumberLabel.Size = new System.Drawing.Size(15, 24);
            _firstPointNumberLabel.Text = "1";
            _firstPointNumberLabel.ToolTipText = "Number of the first buffer reading";
            //
            // _bufferSizeNumericLabel
            //
            _bufferSizeNumericLabel.Name = "_BufferSizeNumericLabel";
            _bufferSizeNumericLabel.Size = new System.Drawing.Size(34, 24);
            _bufferSizeNumericLabel.Text = "Size:";
            //
            // _bufferSizeNumeric
            //
            _bufferSizeNumeric.AutoSize = false;
            _bufferSizeNumeric.Name = "_BufferSizeNumeric";
            _bufferSizeNumeric.Size = new System.Drawing.Size(60, 24);
            _bufferSizeNumeric.Text = "0";
            _bufferSizeNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            //
            // _triggerStateLabel
            //
            _triggerStateLabel.Name = "_TriggerStateLabel";
            _triggerStateLabel.Size = new System.Drawing.Size(29, 24);
            _triggerStateLabel.Text = "idle";
            _triggerStateLabel.ToolTipText = "Trigger state";
            //
            // _assertBusTriggerButton
            //
            _assertBusTriggerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _assertBusTriggerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _assertBusTriggerButton.Name = "_assertBusTriggerButton";
            _assertBusTriggerButton.Size = new System.Drawing.Size(48, 24);
            _assertBusTriggerButton.Text = "Assert";
            _assertBusTriggerButton.ToolTipText = "Assert BUS trigger";
            //
            // UseTriggerPlanMenuItem
            //
            UseTriggerPlanMenuItem.CheckOnClick = true;
            UseTriggerPlanMenuItem.Name = "UseTriggerPlanMenuItem";
            UseTriggerPlanMenuItem.Size = new System.Drawing.Size(180, 22);
            UseTriggerPlanMenuItem.Text = "Use Trigger Plan";
            UseTriggerPlanMenuItem.ToolTipText = "Select trigger plan or application settings for trigger source and trigger count." + "";
            //
            // BufferStreamView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_subsystemToolStrip);
            Name = "BufferStreamView";
            Size = new System.Drawing.Size(349, 26);
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
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripLabel _bufferSizeNumericLabel;
        private ToolStripButton _abortButton;
        private cc.isr.WinControls.ToolStripLabel _triggerStateLabel;
        private ToolStripMenuItem _displayBufferMenuItem;
        private ToolStripMenuItem _clearBufferDisplayMenuItem;
        private ToolStripMenuItem _usingScanCardMenuItem;
        private ToolStripMenuItem _configureStreamingMenuItem;
        private ToolStripMenuItem _startBufferStreamMenuItem;
        private ToolStripMenuItem _bufferStreamingMenuItem;
        private ToolStripMenuItem _explainBufferStreamingMenuItem;
        private ToolStripButton _assertBusTriggerButton;
        private ToolStripMenuItem UseTriggerPlanMenuItem;
    }
}
