using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class ChannelScanView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelScanView));
            _layout = new TableLayoutPanel();
            _infoTextBox = new TextBox();
            _channelView = new ChannelView();
            ScanView = new ScanView();
            _routeToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsMenuItem_Click);
            _layout.SuspendLayout();
            _routeToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _layout
            // 
            _layout.ColumnCount = 2;
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 2.0f));
            _layout.Controls.Add(_infoTextBox, 0, 3);
            _layout.Controls.Add(_channelView, 0, 1);
            _layout.Controls.Add(ScanView, 0, 2);
            _layout.Controls.Add(_routeToolStrip, 0, 0);
            _layout.Dock = DockStyle.Fill;
            _layout.Location = new System.Drawing.Point(1, 1);
            _layout.Name = "_Layout";
            _layout.RowCount = 4;
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            _layout.Size = new System.Drawing.Size(397, 274);
            _layout.TabIndex = 0;
            // 
            // _infoTextBox
            // 
            _infoTextBox.Dock = DockStyle.Fill;
            _infoTextBox.Location = new System.Drawing.Point(3, 95);
            _infoTextBox.Margin = new Padding(3, 4, 3, 4);
            _infoTextBox.Multiline = true;
            _infoTextBox.Name = "_InfoTextBox";
            _infoTextBox.ReadOnly = true;
            _infoTextBox.ScrollBars = ScrollBars.Both;
            _infoTextBox.Size = new System.Drawing.Size(389, 175);
            _infoTextBox.TabIndex = 27;
            _infoTextBox.Text = "<info>";
            // 
            // _channelView
            // 
            _channelView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _channelView.BorderStyle = BorderStyle.FixedSingle;
            _channelView.Dock = DockStyle.Top;
            _channelView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _channelView.Location = new System.Drawing.Point(3, 28);
            _channelView.Name = "_ChannelView";
            _channelView.Padding = new Padding(1);
            _channelView.Size = new System.Drawing.Size(389, 27);
            _channelView.TabIndex = 10;
            // 
            // _scanView
            // 
            ScanView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ScanView.BorderStyle = BorderStyle.FixedSingle;
            ScanView.Dock = DockStyle.Top;
            ScanView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            ScanView.Location = new System.Drawing.Point(3, 61);
            ScanView.Name = "_ScanView";
            ScanView.Padding = new Padding(1);
            ScanView.Size = new System.Drawing.Size(389, 27);
            ScanView.TabIndex = 11;
            // 
            // _routeToolStrip
            // 
            _routeToolStrip.AutoSize = false;
            _routeToolStrip.BackColor = System.Drawing.Color.Transparent;
            _routeToolStrip.Dock = DockStyle.None;
            _routeToolStrip.GripMargin = new Padding(0);
            _routeToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton });
            _routeToolStrip.Location = new System.Drawing.Point(0, 0);
            _routeToolStrip.Name = "_RouteToolStrip";
            _routeToolStrip.Size = new System.Drawing.Size(395, 25);
            _routeToolStrip.Stretch = true;
            _routeToolStrip.TabIndex = 9;
            _routeToolStrip.Text = "Trigger";
            // 
            // _subsystemSplitButton
            // 
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _readSettingsMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.Image = (System.Drawing.Image)resources.GetObject("_SubsystemSplitButton.Image");
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(53, 22);
            _subsystemSplitButton.Text = "Route";
            _subsystemSplitButton.ToolTipText = "Subsystem actions split button";
            // 
            // _readSettingsMenuItem
            // 
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(146, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            _readSettingsMenuItem.ToolTipText = "Reads settings";
            // 
            // ChannelScanView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(_layout);
            Name = "ChannelScanView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(399, 276);
            _layout.ResumeLayout(false);
            _layout.PerformLayout();
            _routeToolStrip.ResumeLayout(false);
            _routeToolStrip.PerformLayout();
            ResumeLayout(false);
        }

        private TableLayoutPanel _layout;
        private ToolStrip _routeToolStrip;
        private ToolStripSplitButton _subsystemSplitButton;
        private ChannelView _channelView;
        private TextBox _infoTextBox;
        private ToolStripMenuItem _readSettingsMenuItem;
    }
}
