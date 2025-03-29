using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls
{
    public partial class VisaView
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
            _tabs = new TabControl();
            _sessionTabPage = new TabPage();
            _sessionView = new SessionView();
            StatusView = new StatusView();
            _selectorOpener = new cc.isr.WinControls.SelectorOpener();
            _messagesTabPage = new TabPage();
            _traceMessagesBox = new cc.isr.Logging.TraceLog.WinForms.MessagesBox();
            _statusStrip = new StatusStrip();
            _statusPromptLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _panel = new Panel();
            DisplayView = new DisplayView();
            _layout = new TableLayoutPanel();
            _tabs.SuspendLayout();
            _sessionTabPage.SuspendLayout();
            _messagesTabPage.SuspendLayout();
            _statusStrip.SuspendLayout();
            _panel.SuspendLayout();
            _layout.SuspendLayout();
            SuspendLayout();
            //
            // _tabs
            //
            _tabs.Controls.Add(_sessionTabPage);
            _tabs.Controls.Add(_messagesTabPage);
            _tabs.Dock = DockStyle.Fill;
            _tabs.ItemSize = new System.Drawing.Size(52, 22);
            _tabs.Location = new System.Drawing.Point(0, 143);
            _tabs.Name = "_Tabs";
            _tabs.SelectedIndex = 0;
            _tabs.Size = new System.Drawing.Size(364, 285);
            _tabs.TabIndex = 5;
            _tabs.DrawItem += new DrawItemEventHandler( Tabs_DrawItem );
            //
            // _sessionTabPage
            //
            _sessionTabPage.Controls.Add(_sessionView);
            _sessionTabPage.Controls.Add(StatusView);
            _sessionTabPage.Controls.Add(_selectorOpener);
            _sessionTabPage.Location = new System.Drawing.Point(4, 26);
            _sessionTabPage.Name = "_SessionTabPage";
            _sessionTabPage.Size = new System.Drawing.Size(356, 255);
            _sessionTabPage.TabIndex = 0;
            _sessionTabPage.Text = "Session";
            _sessionTabPage.UseVisualStyleBackColor = true;
            //
            // _sessionView
            //
            _sessionView.Dock = DockStyle.Fill;
            _sessionView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _sessionView.Location = new System.Drawing.Point(0, 0);
            _sessionView.Margin = new Padding(3, 4, 3, 4);
            _sessionView.Name = "_SessionView";
            _sessionView.Size = new System.Drawing.Size(356, 195);
            _sessionView.TabIndex = 25;
            //
            // _statusView
            //
            StatusView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            StatusView.BackColor = System.Drawing.Color.Transparent;
            StatusView.Dock = DockStyle.Bottom;
            StatusView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            StatusView.Location = new System.Drawing.Point(0, 195);
            StatusView.Margin = new Padding(0);
            StatusView.Name = "_StatusView";
            StatusView.Size = new System.Drawing.Size(356, 31);
            StatusView.TabIndex = 27;
            //
            // _selectorOpener
            //
            _selectorOpener.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _selectorOpener.BackColor = System.Drawing.Color.Transparent;
            _selectorOpener.Dock = DockStyle.Bottom;
            _selectorOpener.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _selectorOpener.Location = new System.Drawing.Point(0, 226);
            _selectorOpener.Margin = new Padding(0);
            _selectorOpener.Name = "_SelectorOpener";
            _selectorOpener.SelectedValueChangeCount = 0;
            _selectorOpener.Size = new System.Drawing.Size(356, 29);
            _selectorOpener.TabIndex = 26;
            //
            // _messagesTabPage
            //
            _messagesTabPage.Controls.Add(_traceMessagesBox);
            _messagesTabPage.Location = new System.Drawing.Point(4, 26);
            _messagesTabPage.Name = "_MessagesTabPage";
            _messagesTabPage.Size = new System.Drawing.Size(356, 273);
            _messagesTabPage.TabIndex = 3;
            _messagesTabPage.Text = "Log";
            _messagesTabPage.UseVisualStyleBackColor = true;
            //
            // _traceMessagesBox
            //
            _traceMessagesBox.BackColor = System.Drawing.SystemColors.Info;
            _traceMessagesBox.CausesValidation = false;
            _traceMessagesBox.Dock = DockStyle.Fill;
            _traceMessagesBox.Font = new System.Drawing.Font("Consolas", 8.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _traceMessagesBox.Location = new System.Drawing.Point(0, 0);
            _traceMessagesBox.Multiline = true;
            _traceMessagesBox.Name = "_TraceMessagesBox";
            _traceMessagesBox.ReadOnly = true;
            _traceMessagesBox.ScrollBars = ScrollBars.Both;
            _traceMessagesBox.Size = new System.Drawing.Size(356, 273);
            _traceMessagesBox.TabIndex = 1;
            //
            // _statusStrip
            //
            _statusStrip.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _statusStrip.GripMargin = new Padding(0);
            _statusStrip.Items.AddRange(new ToolStripItem[] { _statusPromptLabel });
            _statusStrip.Location = new System.Drawing.Point(0, 428);
            _statusStrip.Name = "_StatusStrip";
            _statusStrip.Padding = new Padding(1, 0, 16, 0);
            _statusStrip.ShowItemToolTips = true;
            _statusStrip.Size = new System.Drawing.Size(364, 22);
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
            _statusPromptLabel.Size = new System.Drawing.Size(347, 17);
            _statusPromptLabel.Spring = true;
            _statusPromptLabel.Text = "<Status>";
            _statusPromptLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            _statusPromptLabel.ToolTipText = "Status prompt";
            //
            // _panel
            //
            _panel.Controls.Add(_tabs);
            _panel.Controls.Add(DisplayView);
            _panel.Controls.Add(_statusStrip);
            _panel.Dock = DockStyle.Fill;
            _panel.Location = new System.Drawing.Point(0, 0);
            _panel.Margin = new Padding(0);
            _panel.Name = "_Panel";
            _panel.Size = new System.Drawing.Size(364, 450);
            _panel.TabIndex = 16;
            //
            // _displayView
            //
            DisplayView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            DisplayView.Dock = DockStyle.Top;
            DisplayView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            DisplayView.Location = new System.Drawing.Point(0, 0);
            DisplayView.Margin = new Padding(3, 4, 3, 4);
            DisplayView.Name = "_DisplayView";
            DisplayView.Size = new System.Drawing.Size(364, 143);
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
            _layout.Size = new System.Drawing.Size(364, 450);
            _layout.TabIndex = 17;
            //
            // VisaView
            //
            Controls.Add(_layout);
            Name = "VisaView";
            Size = new System.Drawing.Size(364, 450);
            _tabs.ResumeLayout(false);
            _sessionTabPage.ResumeLayout(false);
            _messagesTabPage.ResumeLayout(false);
            _messagesTabPage.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            _panel.ResumeLayout(false);
            _panel.PerformLayout();
            _layout.ResumeLayout(false);
            ResumeLayout(false);
        }

        private TabPage _sessionTabPage;
        private TabPage _messagesTabPage;
        private TabControl _tabs;
        private Panel _panel;
        private TableLayoutPanel _layout;
        private cc.isr.Logging.TraceLog.WinForms.MessagesBox _traceMessagesBox;
        private StatusStrip _statusStrip;
        private cc.isr.WinControls.ToolStripStatusLabel _statusPromptLabel;
        private SessionView _sessionView;
        private cc.isr.WinControls.SelectorOpener _selectorOpener;
    }
}
