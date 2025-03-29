using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class ChannelView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelView));
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _closeChannelMenuItem = new ToolStripMenuItem();
            _closeChannelMenuItem.Click += new EventHandler(CloseChannelMenuItem_Click);
            _openChannelMenuItem = new ToolStripMenuItem();
            _openChannelMenuItem.Click += new EventHandler(OpenChannelMenuItem_Click);
            _openAllMenuItem = new ToolStripMenuItem();
            _openAllMenuItem.Click += new EventHandler(OpenAllMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsMenuItem_Click);
            _channelNumberNumericLabel = new ToolStripLabel();
            _channelNumberNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _closeChannelButton = new ToolStripButton();
            _closeChannelButton.Click += new EventHandler(CloseChannelButton_Click);
            _openChannelButton = new ToolStripButton();
            _openChannelButton.Click += new EventHandler(OpenChannelButton_Click);
            _openChannelsButton = new ToolStripButton();
            _openChannelsButton.Click += new EventHandler(OpenChannelsButton_Click);
            _closedChannelLabel = new ToolStripLabel();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _channelNumberNumericLabel, _channelNumberNumeric, _closeChannelButton, _openChannelButton, _openChannelsButton, _closedChannelLabel });
            _subsystemToolStrip.Location = new System.Drawing.Point(1, 1);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(382, 26);
            _subsystemToolStrip.TabIndex = 0;
            _subsystemToolStrip.Text = "ChannelToolStrip";
            //
            // _subsystemSplitButton
            //
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _closeChannelMenuItem, _openChannelMenuItem, _openAllMenuItem, _readSettingsMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.Image = (System.Drawing.Image)resources.GetObject("_SubsystemSplitButton.Image");
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(68, 23);
            _subsystemSplitButton.Text = "Channel";
            //
            // _closeChannelMenuItem
            //
            _closeChannelMenuItem.Name = "_CloseChannelMenuItem";
            _closeChannelMenuItem.Size = new System.Drawing.Size(180, 22);
            _closeChannelMenuItem.Text = "Close Channel";
            _closeChannelMenuItem.ToolTipText = "Closes the specified channel";
            //
            // _openChannelMenuItem
            //
            _openChannelMenuItem.Name = "_OpenChannelMenuItem";
            _openChannelMenuItem.Size = new System.Drawing.Size(180, 22);
            _openChannelMenuItem.Text = "Open Channel";
            _openChannelMenuItem.ToolTipText = "Opens the selected channel";
            //
            // _openAllMenuItem
            //
            _openAllMenuItem.Name = "_OpenAllMenuItem";
            _openAllMenuItem.Size = new System.Drawing.Size(180, 22);
            _openAllMenuItem.Text = "Open All";
            //
            // _readSettingsMenuItem
            //
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(180, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            _readSettingsMenuItem.ToolTipText = "Reads settings from the instrument";
            //
            // _channelNumberNumericLabel
            //
            _channelNumberNumericLabel.Name = "_ChannelNumberNumericLabel";
            _channelNumberNumericLabel.Size = new System.Drawing.Size(14, 23);
            _channelNumberNumericLabel.Text = "#";
            //
            // _channelNumberNumeric
            //
            _channelNumberNumeric.Name = "_ChannelNumberNumeric";
            _channelNumberNumeric.Size = new System.Drawing.Size(41, 23);
            _channelNumberNumeric.Text = "0";
            _channelNumberNumeric.ToolTipText = "Channel Number";
            _channelNumberNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            //
            // _closeChannelButton
            //
            _closeChannelButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _closeChannelButton.Image = (System.Drawing.Image)resources.GetObject("_CloseChannelButton.Image");
            _closeChannelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _closeChannelButton.Name = "_CloseChannelButton";
            _closeChannelButton.Size = new System.Drawing.Size(40, 23);
            _closeChannelButton.Text = "Close";
            _closeChannelButton.ToolTipText = "Closes the selected channel";
            //
            // _openChannelButton
            //
            _openChannelButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _openChannelButton.Image = (System.Drawing.Image)resources.GetObject("_OpenChannelButton.Image");
            _openChannelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _openChannelButton.Name = "_OpenChannelButton";
            _openChannelButton.Size = new System.Drawing.Size(40, 23);
            _openChannelButton.Text = "Open";
            _openChannelButton.ToolTipText = "Opens the selected channel";
            //
            // _openChannelsButton
            //
            _openChannelsButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _openChannelsButton.Image = (System.Drawing.Image)resources.GetObject("_OpenChannelsButton.Image");
            _openChannelsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _openChannelsButton.Name = "_OpenChannelsButton";
            _openChannelsButton.Size = new System.Drawing.Size(57, 23);
            _openChannelsButton.Text = "Open All";
            _openChannelsButton.ToolTipText = "Opens all channels";
            //
            // _closedChannelLabel
            //
            _closedChannelLabel.Name = "_ClosedChannelLabel";
            _closedChannelLabel.Size = new System.Drawing.Size(54, 23);
            _closedChannelLabel.Text = "Closed: ?";
            //
            // ChannelView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_subsystemToolStrip);
            Name = "ChannelView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(384, 26);
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStripLabel _channelNumberNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _channelNumberNumeric;
        private ToolStripLabel _closedChannelLabel;
        private ToolStripButton _closeChannelButton;
        private ToolStripButton _openChannelButton;
        private ToolStripButton _openChannelsButton;
        private ToolStrip _subsystemToolStrip;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _closeChannelMenuItem;
        private ToolStripMenuItem _openChannelMenuItem;
        private ToolStripMenuItem _openAllMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
    }
}
