using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class ScannerView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ScannerView));
            _layout = new TableLayoutPanel();
            _relayView = new RelayView();
            _scanListView = new ScanListView();
            _slotView = new SlotView();
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _externalTriggerPlanToolMenuItem = new ToolStripMenuItem();
            _explainExternalTriggerPlanMenuItem = new ToolStripMenuItem();
            _explainExternalTriggerPlanMenuItem.Click += new EventHandler(ExplainExternalTriggerPlanMenuItem_Click);
            _configureExternalTriggerPlanMenuItem = new ToolStripMenuItem();
            _configureExternalTriggerPlanMenuItem.Click += new EventHandler(ConfigureExternalTriggerPlanMenuItem_Click);
            _busTriggerPlanMenuItem = new ToolStripMenuItem();
            _explainBusTriggerPlanMenuItem = new ToolStripMenuItem();
            _explainBusTriggerPlanMenuItem.Click += new EventHandler(ExplainBusTriggerPlanMenuItem_Click);
            _configureBusTriggerPlanMenuItem = new ToolStripMenuItem();
            _configureBusTriggerPlanMenuItem.Click += new EventHandler(ConfigureBusTriggerPlanMenuItem_Click);
            _initiateButton = new ToolStripButton();
            _initiateButton.Click += new EventHandler(InitiateButton_Click);
            _sendBusTriggerButton = new ToolStripButton();
            _sendBusTriggerButton.Click += new EventHandler(SendBusTriggerButton_Click);
            _abortButton = new ToolStripButton();
            _abortButton.Click += new EventHandler(AbortButton_Click);
            _serviceRequestView = new ServiceRequestView();
            _infoTextBox = new TextBox();
            _layout.SuspendLayout();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            //
            // _layout
            //
            _layout.ColumnCount = 2;
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 2.0f));
            _layout.Controls.Add(_relayView, 0, 4);
            _layout.Controls.Add(_scanListView, 0, 3);
            _layout.Controls.Add(_slotView, 0, 1);
            _layout.Controls.Add(_subsystemToolStrip, 0, 0);
            _layout.Controls.Add(_serviceRequestView, 0, 2);
            _layout.Controls.Add(_infoTextBox, 0, 5);
            _layout.Dock = DockStyle.Fill;
            _layout.Location = new System.Drawing.Point(1, 1);
            _layout.Name = "_Layout";
            _layout.RowCount = 6;
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            _layout.Size = new System.Drawing.Size(421, 324);
            _layout.TabIndex = 0;
            //
            // _relayView
            //
            _relayView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _relayView.Dock = DockStyle.Top;
            _relayView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _relayView.Location = new System.Drawing.Point(3, 132);
            _relayView.Name = "_RelayView";
            _relayView.Padding = new Padding(1);
            _relayView.Size = new System.Drawing.Size(413, 28);
            _relayView.TabIndex = 17;
            //
            // _scanListView
            //
            _scanListView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _scanListView.Dock = DockStyle.Top;
            _scanListView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _scanListView.Location = new System.Drawing.Point(3, 100);
            _scanListView.Name = "_ScanListView";
            _scanListView.Padding = new Padding(1);
            _scanListView.Size = new System.Drawing.Size(413, 26);
            _scanListView.TabIndex = 16;
            //
            // _slotView
            //
            _slotView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _slotView.Dock = DockStyle.Top;
            _slotView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _slotView.Location = new System.Drawing.Point(3, 28);
            _slotView.Name = "_SlotView";
            _slotView.Padding = new Padding(1);
            _slotView.Size = new System.Drawing.Size(413, 31);
            _slotView.TabIndex = 1;
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _initiateButton, _sendBusTriggerButton, _abortButton });
            _subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(419, 25);
            _subsystemToolStrip.TabIndex = 2;
            _subsystemToolStrip.Text = "Subsystem Tool strip";
            //
            // _subsystemSplitButton
            //
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _externalTriggerPlanToolMenuItem, _busTriggerPlanMenuItem });
            _subsystemSplitButton.Image = (System.Drawing.Image)resources.GetObject("_SubsystemSplitButton.Image");
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(65, 22);
            _subsystemSplitButton.Text = "Scanner";
            _subsystemSplitButton.ToolTipText = "Device options and actions";
            //
            // _externalTriggerPlanToolMenuItem
            //
            _externalTriggerPlanToolMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _explainExternalTriggerPlanMenuItem, _configureExternalTriggerPlanMenuItem });
            _externalTriggerPlanToolMenuItem.Name = "_ExternalTriggerPlanToolMenuItem";
            _externalTriggerPlanToolMenuItem.Size = new System.Drawing.Size(190, 22);
            _externalTriggerPlanToolMenuItem.Text = "External Trigger Plan...";
            _externalTriggerPlanToolMenuItem.ToolTipText = "Configures an external trigger plan";
            //
            // _explainExternalTriggerPlanMenuItem
            //
            _explainExternalTriggerPlanMenuItem.Name = "_ExplainExternalTriggerPlanMenuItem";
            _explainExternalTriggerPlanMenuItem.Size = new System.Drawing.Size(127, 22);
            _explainExternalTriggerPlanMenuItem.Text = "Info";
            _explainExternalTriggerPlanMenuItem.ToolTipText = "Displays information about the external trigger plan";
            //
            // _configureExternalTriggerPlanMenuItem
            //
            _configureExternalTriggerPlanMenuItem.Name = "_ConfigureExternalTriggerPlanMenuItem";
            _configureExternalTriggerPlanMenuItem.Size = new System.Drawing.Size(127, 22);
            _configureExternalTriggerPlanMenuItem.Text = "Configure";
            _configureExternalTriggerPlanMenuItem.ToolTipText = "Configures an external trigger plan";
            //
            // _busTriggerPlanMenuItem
            //
            _busTriggerPlanMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _explainBusTriggerPlanMenuItem, _configureBusTriggerPlanMenuItem });
            _busTriggerPlanMenuItem.Name = "_BusTriggerPlanMenuItem";
            _busTriggerPlanMenuItem.Size = new System.Drawing.Size(190, 22);
            _busTriggerPlanMenuItem.Text = "Bus Trigger Plan...";
            _busTriggerPlanMenuItem.ToolTipText = "Configures a bus trigger plan";
            //
            // _explainBusTriggerPlanMenuItem
            //
            _explainBusTriggerPlanMenuItem.Name = "_ExplainBusTriggerPlanMenuItem";
            _explainBusTriggerPlanMenuItem.Size = new System.Drawing.Size(127, 22);
            _explainBusTriggerPlanMenuItem.Text = "Info";
            _explainBusTriggerPlanMenuItem.ToolTipText = "Displays information about the simple trigger plan";
            //
            // _configureBusTriggerPlanMenuItem
            //
            _configureBusTriggerPlanMenuItem.Name = "_ConfigureBusTriggerPlanMenuItem";
            _configureBusTriggerPlanMenuItem.Size = new System.Drawing.Size(127, 22);
            _configureBusTriggerPlanMenuItem.Text = "Configure";
            _configureBusTriggerPlanMenuItem.ToolTipText = "Configures a bus trigger plan";
            //
            // _initiateButton
            //
            _initiateButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _initiateButton.Image = (System.Drawing.Image)resources.GetObject("_InitiateButton.Image");
            _initiateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _initiateButton.Name = "_InitiateButton";
            _initiateButton.Size = new System.Drawing.Size(47, 22);
            _initiateButton.Text = "Initiate";
            _initiateButton.ToolTipText = "Starts the trigger plan";
            //
            // _sendBusTriggerButton
            //
            _sendBusTriggerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _sendBusTriggerButton.Image = (System.Drawing.Image)resources.GetObject("_SendBusTriggerButton.Image");
            _sendBusTriggerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _sendBusTriggerButton.Name = "_SendBusTriggerButton";
            _sendBusTriggerButton.Size = new System.Drawing.Size(43, 22);
            _sendBusTriggerButton.Text = "Assert";
            _sendBusTriggerButton.ToolTipText = "Sends a bus trigger to the instrument";
            //
            // _abortButton
            //
            _abortButton.Alignment = ToolStripItemAlignment.Right;
            _abortButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _abortButton.Image = (System.Drawing.Image)resources.GetObject("_abortButton.Image");
            _abortButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _abortButton.Margin = new Padding(10, 1, 0, 2);
            _abortButton.Name = "_abortButton";
            _abortButton.Size = new System.Drawing.Size(41, 22);
            _abortButton.Text = "Abort";
            _abortButton.ToolTipText = "Aborts a trigger plan";
            //
            // _serviceRequestView
            //
            _serviceRequestView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _serviceRequestView.Dock = DockStyle.Top;
            _serviceRequestView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _serviceRequestView.Location = new System.Drawing.Point(3, 65);
            _serviceRequestView.Name = "_ServiceRequestView";
            _serviceRequestView.Padding = new Padding(1);
            _serviceRequestView.Size = new System.Drawing.Size(413, 29);
            _serviceRequestView.TabIndex = 3;
            //
            // _infoTextBox
            //
            _infoTextBox.Dock = DockStyle.Fill;
            _infoTextBox.Location = new System.Drawing.Point(3, 166);
            _infoTextBox.Multiline = true;
            _infoTextBox.Name = "_InfoTextBox";
            _infoTextBox.ReadOnly = true;
            _infoTextBox.ScrollBars = ScrollBars.Both;
            _infoTextBox.Size = new System.Drawing.Size(413, 155);
            _infoTextBox.TabIndex = 4;
            _infoTextBox.Text = "<info>";
            //
            // ScannerView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(_layout);
            Name = "ScannerView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(423, 326);
            _layout.ResumeLayout(false);
            _layout.PerformLayout();
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
        }

        private TableLayoutPanel _layout;
        private ToolStrip _subsystemToolStrip;
        private ToolStripSplitButton _subsystemSplitButton;
        private SlotView _slotView;
        private TextBox _infoTextBox;
        private ToolStripMenuItem _externalTriggerPlanToolMenuItem;
        private ToolStripMenuItem _explainExternalTriggerPlanMenuItem;
        private ToolStripMenuItem _configureExternalTriggerPlanMenuItem;
        private ToolStripMenuItem _busTriggerPlanMenuItem;
        private ToolStripMenuItem _explainBusTriggerPlanMenuItem;
        private ToolStripMenuItem _configureBusTriggerPlanMenuItem;
        private ToolStripButton _initiateButton;
        private ToolStripButton _sendBusTriggerButton;
        private ToolStripButton _abortButton;
        private ServiceRequestView _serviceRequestView;
        private ScanListView _scanListView;
        private RelayView _relayView;
    }
}
