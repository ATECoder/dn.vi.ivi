using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls
{
    public partial class VisaTreeView
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
            _sessionView = new SessionView();
            StatusView = new StatusView();
            _selectorOpener = new cc.isr.WinControls.SelectorOpener();
            _traceMessagesBox = new cc.isr.Logging.TraceLog.WinForms.MessagesBox();
            _statusStrip = new StatusStrip();
            _statusPromptLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _panel = new Panel();
            _treePanel = new cc.isr.WinControls.TreePanel();
            _treePanel.AfterNodeSelected += new EventHandler<TreeViewEventArgs>(TreePanel_afterNodeSelected);
            _sessionPanel = new Panel();
            DisplayView = new DisplayView();
            _layout = new TableLayoutPanel();
            _statusStrip.SuspendLayout();
            _panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_treePanel).BeginInit();
            _treePanel.SuspendLayout();
            _sessionPanel.SuspendLayout();
            _layout.SuspendLayout();
            SuspendLayout();
            //
            // _sessionView
            //
            _sessionView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _sessionView.Dock = DockStyle.Fill;
            _sessionView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _sessionView.Location = new System.Drawing.Point(0, 3);
            _sessionView.Margin = new Padding(3, 4, 3, 4);
            _sessionView.Name = "_SessionView";
            _sessionView.Padding = new Padding(1);
            _sessionView.Size = new System.Drawing.Size(217, 117);
            _sessionView.TabIndex = 25;
            //
            // _statusView
            //
            StatusView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            StatusView.BackColor = System.Drawing.Color.Transparent;
            StatusView.Dock = DockStyle.Bottom;
            StatusView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            StatusView.Location = new System.Drawing.Point(0, 149);
            StatusView.Margin = new Padding(0);
            StatusView.Name = "_StatusView";
            StatusView.Size = new System.Drawing.Size(217, 31);
            StatusView.TabIndex = 27;
            StatusView.UsingStatusSubsystemOnly = false;
            //
            // _selectorOpener
            //
            _selectorOpener.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _selectorOpener.BackColor = System.Drawing.Color.Transparent;
            _selectorOpener.Dock = DockStyle.Bottom;
            _selectorOpener.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _selectorOpener.Location = new System.Drawing.Point(0, 120);
            _selectorOpener.Margin = new Padding(0);
            _selectorOpener.Name = "_SelectorOpener";
            _selectorOpener.Size = new System.Drawing.Size(217, 29);
            _selectorOpener.TabIndex = 26;
            //
            // _traceMessagesBox
            //
            _traceMessagesBox.BackColor = System.Drawing.SystemColors.Info;
            _traceMessagesBox.CausesValidation = false;
            _traceMessagesBox.Font = new System.Drawing.Font("Consolas", 8.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _traceMessagesBox.Location = new System.Drawing.Point(315, 365);
            _traceMessagesBox.Multiline = true;
            _traceMessagesBox.Name = "_TraceMessagesBox";
            _traceMessagesBox.ReadOnly = true;
            _traceMessagesBox.ScrollBars = ScrollBars.Both;
            _traceMessagesBox.Size = new System.Drawing.Size(134, 49);
            _traceMessagesBox.TabIndex = 1;
            //
            // _statusStrip
            //
            _statusStrip.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _statusStrip.GripMargin = new Padding(0);
            _statusStrip.Items.AddRange(new ToolStripItem[] { _statusPromptLabel });
            _statusStrip.Location = new System.Drawing.Point(0, 426);
            _statusStrip.Name = "_StatusStrip";
            _statusStrip.Padding = new Padding(1, 0, 16, 0);
            _statusStrip.ShowItemToolTips = true;
            _statusStrip.Size = new System.Drawing.Size(495, 22);
            _statusStrip.SizingGrip = false;
            _statusStrip.TabIndex = 15;
            _statusStrip.Text = "StatusStrip1";
            //
            // _statusPromptLabel
            //
            _statusPromptLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _statusPromptLabel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _statusPromptLabel.Name = "_StatusPromptLabel";
            _statusPromptLabel.Overflow = ToolStripItemOverflow.Never;
            _statusPromptLabel.Size = new System.Drawing.Size(478, 17);
            _statusPromptLabel.Spring = true;
            _statusPromptLabel.Text = "<Status>";
            _statusPromptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            _statusPromptLabel.ToolTipText = "Status prompt";
            //
            // _panel
            //
            _panel.Controls.Add(_treePanel);
            _panel.Controls.Add(_traceMessagesBox);
            _panel.Controls.Add(_sessionPanel);
            _panel.Controls.Add(DisplayView);
            _panel.Controls.Add(_statusStrip);
            _panel.Dock = DockStyle.Fill;
            _panel.Location = new System.Drawing.Point(0, 0);
            _panel.Margin = new Padding(0);
            _panel.Name = "_Panel";
            _panel.Size = new System.Drawing.Size(495, 448);
            _panel.TabIndex = 16;
            //
            // _treePanel
            //
            _treePanel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _treePanel.Location = new System.Drawing.Point(27, 174);
            _treePanel.Name = "_TreePanel";
            _treePanel.Size = new System.Drawing.Size(78, 84);
            _treePanel.SplitterDistance = 25;
            _treePanel.TabIndex = 18;
            //
            // _sessionPanel
            //
            _sessionPanel.Controls.Add(_sessionView);
            _sessionPanel.Controls.Add(_selectorOpener);
            _sessionPanel.Controls.Add(StatusView);
            _sessionPanel.Location = new System.Drawing.Point(123, 168);
            _sessionPanel.Name = "_SessionPanel";
            _sessionPanel.Padding = new Padding(0, 3, 0, 0);
            _sessionPanel.Size = new System.Drawing.Size(217, 180);
            _sessionPanel.TabIndex = 17;
            //
            // _displayView
            //
            DisplayView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            DisplayView.Dock = DockStyle.Top;
            DisplayView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            DisplayView.Location = new System.Drawing.Point(0, 0);
            DisplayView.Margin = new Padding(3, 4, 3, 4);
            DisplayView.Name = "_DisplayView";
            DisplayView.Padding = new Padding(1);
            DisplayView.Size = new System.Drawing.Size(495, 152);
            DisplayView.TabIndex = 16;
            //
            // _layout
            //
            _layout.ColumnCount = 1;
            _layout.ColumnStyles.Add(new ColumnStyle());
            _layout.Controls.Add(_panel, 0, 1);
            _layout.Dock = DockStyle.Fill;
            _layout.Location = new System.Drawing.Point(0, 0);
            _layout.Margin = new Padding(0);
            _layout.Name = "_Layout";
            _layout.RowCount = 2;
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            _layout.Size = new System.Drawing.Size(495, 448);
            _layout.TabIndex = 17;
            //
            // VisaTreeView
            //
            Controls.Add(_layout);
            Name = "VisaTreeView";
            Size = new System.Drawing.Size(495, 448);
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            _panel.ResumeLayout(false);
            _panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_treePanel).EndInit();
            _treePanel.ResumeLayout(false);
            _sessionPanel.ResumeLayout(false);
            _layout.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel _panel;
        private TableLayoutPanel _layout;
        private cc.isr.Logging.TraceLog.WinForms.MessagesBox _traceMessagesBox;
        private StatusStrip _statusStrip;
        private cc.isr.WinControls.ToolStripStatusLabel _statusPromptLabel;
        private SessionView _sessionView;
        private cc.isr.WinControls.SelectorOpener _selectorOpener;
        private Panel _sessionPanel;
        private cc.isr.WinControls.TreePanel _treePanel;
    }
}
