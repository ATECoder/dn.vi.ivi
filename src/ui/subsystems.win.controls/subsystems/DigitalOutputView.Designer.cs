using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class DigitalOutputView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DigitalOutputView));
            _layout = new TableLayoutPanel();
            _infoTextBox = new TextBox();
            _digitalOutputLine1View = new DigitalOutputLineView();
            _digitalOutputLine2View = new DigitalOutputLineView();
            _digitalOutputLine3View = new DigitalOutputLineView();
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _applySettingsMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem.Click += new EventHandler(ApplySettingsToolStripMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsToolStripMenuItem_Click);
            _strobeButton = new ToolStripButton();
            _strobeButton.Click += new EventHandler(InitiateButton_Click);
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
            _layout.Controls.Add(_digitalOutputLine1View, 0, 1);
            _layout.Controls.Add(_digitalOutputLine2View, 0, 2);
            _layout.Controls.Add(_digitalOutputLine3View, 0, 3);
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
            _layout.Size = new System.Drawing.Size(423, 352);
            _layout.TabIndex = 0;
            //
            // _infoTextBox
            //
            _infoTextBox.Dock = DockStyle.Fill;
            _infoTextBox.Location = new System.Drawing.Point(3, 134);
            _infoTextBox.Margin = new Padding(3, 4, 3, 4);
            _infoTextBox.Multiline = true;
            _infoTextBox.Name = "_InfoTextBox";
            _infoTextBox.ReadOnly = true;
            _infoTextBox.ScrollBars = ScrollBars.Both;
            _infoTextBox.Size = new System.Drawing.Size(415, 214);
            _infoTextBox.TabIndex = 14;
            _infoTextBox.Text = "<info>";
            //
            // _digitalOutputLine1View
            //
            _digitalOutputLine1View.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _digitalOutputLine1View.Dock = DockStyle.Top;
            _digitalOutputLine1View.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _digitalOutputLine1View.LineLevel = 0;
            _digitalOutputLine1View.LineName = "Line";
            _digitalOutputLine1View.LineNumber = 0;
            _digitalOutputLine1View.Location = new System.Drawing.Point(3, 28);
            _digitalOutputLine1View.Name = "_DigitalOutputLine1View";
            _digitalOutputLine1View.Padding = new Padding(1);
            _digitalOutputLine1View.PulseWidth = TimeSpan.Parse("00:00:00");
            _digitalOutputLine1View.Size = new System.Drawing.Size(415, 30);
            _digitalOutputLine1View.TabIndex = 0;
            //
            // _digitalOutputLine2View
            //
            _digitalOutputLine2View.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _digitalOutputLine2View.Dock = DockStyle.Top;
            _digitalOutputLine2View.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _digitalOutputLine2View.LineLevel = 0;
            _digitalOutputLine2View.LineName = "Line";
            _digitalOutputLine2View.LineNumber = 0;
            _digitalOutputLine2View.Location = new System.Drawing.Point(3, 64);
            _digitalOutputLine2View.Name = "_DigitalOutputLine2View";
            _digitalOutputLine2View.Padding = new Padding(1);
            _digitalOutputLine2View.PulseWidth = TimeSpan.Parse("00:00:00");
            _digitalOutputLine2View.Size = new System.Drawing.Size(415, 28);
            _digitalOutputLine2View.TabIndex = 1;
            //
            // _digitalOutputLine3View
            //
            _digitalOutputLine3View.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _digitalOutputLine3View.Dock = DockStyle.Top;
            _digitalOutputLine3View.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _digitalOutputLine3View.LineLevel = 0;
            _digitalOutputLine3View.LineName = "Line";
            _digitalOutputLine3View.LineNumber = 0;
            _digitalOutputLine3View.Location = new System.Drawing.Point(3, 98);
            _digitalOutputLine3View.Name = "_DigitalOutputLine3View";
            _digitalOutputLine3View.Padding = new Padding(1);
            _digitalOutputLine3View.PulseWidth = TimeSpan.Parse("00:00:00");
            _digitalOutputLine3View.Size = new System.Drawing.Size(415, 29);
            _digitalOutputLine3View.TabIndex = 2;
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.AutoSize = false;
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _strobeButton });
            _subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(421, 25);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 9;
            _subsystemToolStrip.Text = "Digital Out";
            //
            // _subsystemSplitButton
            //
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applySettingsMenuItem, _readSettingsMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(82, 22);
            _subsystemSplitButton.Text = "Digital Out";
            _subsystemSplitButton.ToolTipText = "Subsystem actions split button";
            //
            // _applySettingsMenuItem
            //
            _applySettingsMenuItem.Name = "_applySettingsMenuItem";
            _applySettingsMenuItem.Size = new System.Drawing.Size(150, 22);
            _applySettingsMenuItem.Text = "Apply Settings";
            //
            // _readSettingsMenuItem
            //
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(150, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            //
            // _strobeButton
            //
            _strobeButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _strobeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _strobeButton.Name = "_StrobeButton";
            _strobeButton.Size = new System.Drawing.Size(45, 22);
            _strobeButton.Text = "Strobe";
            _strobeButton.ToolTipText = "Outputs a binning strop signal";
            //
            // DigitalOutputView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(_layout);
            Name = "DigitalOutputView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(425, 354);
            _layout.ResumeLayout(false);
            _layout.PerformLayout();
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
        }

        private DigitalOutputLineView _digitalOutputLine1View;
        private DigitalOutputLineView _digitalOutputLine2View;
        private DigitalOutputLineView _digitalOutputLine3View;
        private TableLayoutPanel _layout;
        private ToolStrip _subsystemToolStrip;
        private ToolStripButton _strobeButton;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _applySettingsMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private TextBox _infoTextBox;
    }
}
