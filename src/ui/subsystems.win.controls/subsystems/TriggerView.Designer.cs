using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class TriggerView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(TriggerView));
            _layout = new TableLayoutPanel();
            _infoTextBox = new TextBox();
            _armLayer1View = new ArmLayerView();
            _armLayer2View = new ArmLayerView();
            _triggerLayer1View = new TriggerLayerView();
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _applySettingsMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem.Click += new EventHandler(ApplySettingsToolStripMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsToolStripMenuItem_Click);
            _initiateButton = new ToolStripButton();
            _initiateButton.Click += new EventHandler(InitiateButton_Click);
            _abortButton = new ToolStripButton();
            _abortButton.Click += new EventHandler(AbortButton_Click);
            _sendBusTriggerButton = new ToolStripButton();
            _sendBusTriggerButton.Click += new EventHandler(SendBusTriggerButton_Click);
            _layout.SuspendLayout();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            //
            // _layout
            //
            _layout.ColumnCount = 2;
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 2.0f));
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20.0f));
            _layout.Controls.Add(_infoTextBox, 0, 4);
            _layout.Controls.Add(_armLayer1View, 0, 1);
            _layout.Controls.Add(_armLayer2View, 0, 2);
            _layout.Controls.Add(_triggerLayer1View, 0, 3);
            _layout.Controls.Add(_subsystemToolStrip, 0, 0);
            _layout.Dock = DockStyle.Fill;
            _layout.Location = new System.Drawing.Point(1, 1);
            _layout.Name = "_Layout";
            _layout.RowCount = 5;
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            _layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20.0f));
            _layout.Size = new System.Drawing.Size(415, 352);
            _layout.TabIndex = 0;
            //
            // _infoTextBox
            //
            _infoTextBox.Dock = DockStyle.Fill;
            _infoTextBox.Location = new System.Drawing.Point(3, 310);
            _infoTextBox.Margin = new Padding(3, 4, 3, 4);
            _infoTextBox.Multiline = true;
            _infoTextBox.Name = "_InfoTextBox";
            _infoTextBox.ReadOnly = true;
            _infoTextBox.ScrollBars = ScrollBars.Both;
            _infoTextBox.Size = new System.Drawing.Size(407, 38);
            _infoTextBox.TabIndex = 14;
            _infoTextBox.Text = "<info>";
            //
            // _armLayer1View
            //
            _armLayer1View.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _armLayer1View.Dock = DockStyle.Top;
            _armLayer1View.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _armLayer1View.LayerNumber = 1;
            _armLayer1View.Location = new System.Drawing.Point(3, 28);
            _armLayer1View.Name = "_armLayer1View";
            _armLayer1View.Padding = new Padding(1);
            _armLayer1View.Size = new System.Drawing.Size(407, 88);
            _armLayer1View.TabIndex = 0;
            //
            // _armLayer2View
            //
            _armLayer2View.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _armLayer2View.Dock = DockStyle.Top;
            _armLayer2View.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _armLayer2View.LayerNumber = 2;
            _armLayer2View.Location = new System.Drawing.Point(3, 122);
            _armLayer2View.Name = "_armLayer2View";
            _armLayer2View.Padding = new Padding(1);
            _armLayer2View.Size = new System.Drawing.Size(407, 88);
            _armLayer2View.TabIndex = 1;
            //
            // _triggerLayer1View
            //
            _triggerLayer1View.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _triggerLayer1View.Dock = DockStyle.Top;
            _triggerLayer1View.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _triggerLayer1View.LayerNumber = 1;
            _triggerLayer1View.Location = new System.Drawing.Point(3, 216);
            _triggerLayer1View.Name = "_TriggerLayer1View";
            _triggerLayer1View.Padding = new Padding(1);
            _triggerLayer1View.Size = new System.Drawing.Size(407, 87);
            _triggerLayer1View.TabIndex = 2;
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.AutoSize = false;
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _initiateButton, _abortButton, _sendBusTriggerButton });
            _subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(413, 25);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 9;
            _subsystemToolStrip.Text = "Trigger";
            //
            // _subsystemSplitButton
            //
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applySettingsMenuItem, _readSettingsMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(60, 22);
            _subsystemSplitButton.Text = "Trigger";
            _subsystemSplitButton.ToolTipText = "Subsystem actions split button";
            //
            // _applySettingsMenuItem
            //
            _applySettingsMenuItem.Name = "_applySettingsMenuItem";
            _applySettingsMenuItem.Size = new System.Drawing.Size(180, 22);
            _applySettingsMenuItem.Text = "Apply Settings";
            //
            // _readSettingsMenuItem
            //
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(180, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            //
            // _initiateButton
            //
            _initiateButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _initiateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _initiateButton.Name = "_InitiateButton";
            _initiateButton.Size = new System.Drawing.Size(47, 22);
            _initiateButton.Text = "Initiate";
            _initiateButton.ToolTipText = "Starts the trigger plan";
            //
            // _abortButton
            //
            _abortButton.Alignment = ToolStripItemAlignment.Right;
            _abortButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _abortButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _abortButton.Margin = new Padding(10, 1, 0, 2);
            _abortButton.Name = "_abortButton";
            _abortButton.Size = new System.Drawing.Size(41, 22);
            _abortButton.Text = "Abort";
            _abortButton.ToolTipText = "Aborts a trigger plan";
            //
            // _sendBusTriggerButton
            //
            _sendBusTriggerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _sendBusTriggerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _sendBusTriggerButton.Name = "_SendBusTriggerButton";
            _sendBusTriggerButton.Size = new System.Drawing.Size(43, 22);
            _sendBusTriggerButton.Text = "Assert";
            _sendBusTriggerButton.ToolTipText = "Sends a bus trigger to the instrument";
            //
            // TriggerView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(_layout);
            Name = "TriggerView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(417, 354);
            _layout.ResumeLayout(false);
            _layout.PerformLayout();
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
        }

        private ArmLayerView _armLayer1View;
        private ArmLayerView _armLayer2View;
        private TriggerLayerView _triggerLayer1View;
        private TableLayoutPanel _layout;
        private ToolStrip _subsystemToolStrip;
        private ToolStripButton _initiateButton;
        private ToolStripButton _abortButton;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _applySettingsMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private ToolStripButton _sendBusTriggerButton;
        private TextBox _infoTextBox;
    }
}
