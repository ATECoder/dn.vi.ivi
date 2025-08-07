using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class DigitalOutputLineView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DigitalOutputLineView));
            _toolStripPanel = new ToolStripPanel();
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _applySettingsMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem.Click += new EventHandler(ApplySettingsMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsMenuItem_Click);
            _lineNumberNumericLabel = new ToolStripLabel();
            _lineNumberBox = new cc.isr.WinControls.ToolStripNumberBox();
            _activeLevelComboBoxLabel = new ToolStripLabel();
            _activeLevelComboBox = new ToolStripComboBox();
            _readButton = new ToolStripButton();
            _readButton.Click += new EventHandler(ReadButton_Click);
            _lineLevelNumberBox = new cc.isr.WinControls.ToolStripNumberBox();
            _writeButton = new ToolStripButton();
            _writeButton.Click += new EventHandler(WriteButton_Click);
            _pulseButton = new ToolStripButton();
            _pulseButton.Click += new EventHandler(PulseButton_Click);
            _toggleButton = new ToolStripButton();
            _toggleButton.Click += new EventHandler(ToggleButton_Click);
            _pulseWidthNumberBox = new cc.isr.WinControls.ToolStripNumberBox();
            _toolStripPanel.SuspendLayout();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            //
            // _toolStripPanel
            //
            _toolStripPanel.BackColor = System.Drawing.Color.Transparent;
            _toolStripPanel.Controls.Add(_subsystemToolStrip);
            _toolStripPanel.Dock = DockStyle.Top;
            _toolStripPanel.Location = new System.Drawing.Point(1, 1);
            _toolStripPanel.Name = "_ToolStripPanel";
            _toolStripPanel.Orientation = Orientation.Horizontal;
            _toolStripPanel.RowMargin = new Padding(0);
            _toolStripPanel.Size = new System.Drawing.Size(451, 28);
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.Dock = DockStyle.None;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _lineNumberNumericLabel, _lineNumberBox, _activeLevelComboBoxLabel, _activeLevelComboBox, _readButton, _lineLevelNumberBox, _writeButton, _toggleButton, _pulseButton, _pulseWidthNumberBox });
            _subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(451, 28);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 3;
            //
            // _subsystemSplitButton
            //
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DoubleClickEnabled = true;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applySettingsMenuItem, _readSettingsMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(45, 25);
            _subsystemSplitButton.Text = "Line";
            //
            // _applySettingsMenuItem
            //
            _applySettingsMenuItem.Name = "_applySettingsMenuItem";
            _applySettingsMenuItem.Size = new System.Drawing.Size(150, 22);
            _applySettingsMenuItem.Text = "Apply Settings";
            _applySettingsMenuItem.ToolTipText = "Applies settings onto the instrument";
            //
            // _readSettingsMenuItem
            //
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(150, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            _readSettingsMenuItem.ToolTipText = "Reads settings from the instrument";
            //
            // _lineNumberNumericLabel
            //
            _lineNumberNumericLabel.Name = "_LineNumberNumericLabel";
            _lineNumberNumericLabel.Size = new System.Drawing.Size(14, 25);
            _lineNumberNumericLabel.Text = "#";
            //
            // _lineNumberBox
            //
            _lineNumberBox.BackColor = System.Drawing.SystemColors.Window;
            _lineNumberBox.ForeColor = System.Drawing.SystemColors.ControlText;
            _lineNumberBox.MaxValue = 4.0d;
            _lineNumberBox.MinValue = 1.0d;
            _lineNumberBox.Name = "_LineNumberBox";
            _lineNumberBox.Prefix = null;
            _lineNumberBox.Size = new System.Drawing.Size(22, 25);
            _lineNumberBox.NumberFormatString = "D";
            _lineNumberBox.Suffix = null;
            _lineNumberBox.Text = "-1";
            _lineNumberBox.ValueAsByte = 0;
            _lineNumberBox.ValueAsDouble = 0d;
            _lineNumberBox.ValueAsFloat = 0f;
            _lineNumberBox.ValueAsInt16 = 0;
            _lineNumberBox.ValueAsInt32 = 0;
            _lineNumberBox.ValueAsInt64 = 0;
            _lineNumberBox.ValueAsSByte = 0;
            _lineNumberBox.ValueAsUInt16 = 0;
            _lineNumberBox.ValueAsUInt32 = 0U;
            _lineNumberBox.ValueAsUInt64 = 0UL;
            //
            // _activeLevelComboBoxLabel
            //
            _activeLevelComboBoxLabel.Name = "_activeLevelComboBoxLabel";
            _activeLevelComboBoxLabel.Size = new System.Drawing.Size(43, 25);
            _activeLevelComboBoxLabel.Text = "Active:";
            //
            // _activeLevelComboBox
            //
            _activeLevelComboBox.AutoSize = false;
            _activeLevelComboBox.Items.AddRange(new object[] { "Low", "High" });
            _activeLevelComboBox.Name = "_activeLevelComboBox";
            _activeLevelComboBox.Size = new System.Drawing.Size(50, 23);
            _activeLevelComboBox.Text = "Low";
            _activeLevelComboBox.ToolTipText = "Selects the active level";
            //
            // _readButton
            //
            _readButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _readButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _readButton.Name = "_ReadButton";
            _readButton.Size = new System.Drawing.Size(37, 25);
            _readButton.Text = "Read";
            _readButton.ToolTipText = "Read the digital level";
            //
            // _lineLevelNumberBox
            //
            _lineLevelNumberBox.BackColor = System.Drawing.SystemColors.Window;
            _lineLevelNumberBox.ForeColor = System.Drawing.SystemColors.ControlText;
            _lineLevelNumberBox.MaxValue = 1.0d;
            _lineLevelNumberBox.MinValue = -2.0d;
            _lineLevelNumberBox.Name = "_LineLevelNumberBox";
            _lineLevelNumberBox.Prefix = null;
            _lineLevelNumberBox.Size = new System.Drawing.Size(17, 25);
            _lineLevelNumberBox.NumberFormatString = "D";
            _lineLevelNumberBox.Suffix = null;
            _lineLevelNumberBox.Text = "0";
            _lineLevelNumberBox.ToolTipText = "0 = inactive; 1 = active; -1 = not specified; -2 not set.";
            _lineLevelNumberBox.ValueAsByte = 0;
            _lineLevelNumberBox.ValueAsDouble = 0d;
            _lineLevelNumberBox.ValueAsFloat = 0f;
            _lineLevelNumberBox.ValueAsInt16 = 0;
            _lineLevelNumberBox.ValueAsInt32 = 0;
            _lineLevelNumberBox.ValueAsInt64 = 0;
            _lineLevelNumberBox.ValueAsSByte = 0;
            _lineLevelNumberBox.ValueAsUInt16 = 0;
            _lineLevelNumberBox.ValueAsUInt32 = 0U;
            _lineLevelNumberBox.ValueAsUInt64 = 0UL;
            //
            // _writeButton
            //
            _writeButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _writeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _writeButton.Name = "_WriteButton";
            _writeButton.Size = new System.Drawing.Size(39, 25);
            _writeButton.Text = "Write";
            _writeButton.ToolTipText = "Output the value";
            //
            // _pulseButton
            //
            _pulseButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _pulseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _pulseButton.Name = "_PulseButton";
            _pulseButton.Size = new System.Drawing.Size(61, 25);
            _pulseButton.Text = "Pulse";
            _pulseButton.ToolTipText = "Output a pulse for the specified duration in milliseconds";
            //
            // _toggleButton
            //
            _toggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _toggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _toggleButton.Name = "_ToggleButton";
            _toggleButton.Size = new System.Drawing.Size(46, 25);
            _toggleButton.Text = "Toggle";
            _toggleButton.ToolTipText = "Toggle the line level";
            //
            // _pulseWidthNumberBox
            //
            _pulseWidthNumberBox.BackColor = System.Drawing.SystemColors.Window;
            _pulseWidthNumberBox.ForeColor = System.Drawing.SystemColors.ControlText;
            _pulseWidthNumberBox.MaxValue = 10000.0d;
            _pulseWidthNumberBox.MinValue = 1.0d;
            _pulseWidthNumberBox.Name = "_PulseWidthNumberBox";
            _pulseWidthNumberBox.Prefix = null;
            _pulseWidthNumberBox.Size = new System.Drawing.Size(17, 25);
            _pulseWidthNumberBox.NumberFormatString = "N1";
            _pulseWidthNumberBox.Suffix = "ms";
            _pulseWidthNumberBox.Text = "1";
            _pulseWidthNumberBox.ToolTipText = "Pulse width in milliseconds";
            _pulseWidthNumberBox.ValueAsByte = 0;
            _pulseWidthNumberBox.ValueAsDouble = 0d;
            _pulseWidthNumberBox.ValueAsFloat = 0f;
            _pulseWidthNumberBox.ValueAsInt16 = 0;
            _pulseWidthNumberBox.ValueAsInt32 = 0;
            _pulseWidthNumberBox.ValueAsInt64 = 0;
            _pulseWidthNumberBox.ValueAsSByte = 0;
            _pulseWidthNumberBox.ValueAsUInt16 = 0;
            _pulseWidthNumberBox.ValueAsUInt32 = 0U;
            _pulseWidthNumberBox.ValueAsUInt64 = 0UL;
            //
            // DigitalOutputLineView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_toolStripPanel);
            Name = "DigitalOutputLineView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(453, 31);
            _toolStripPanel.ResumeLayout(false);
            _toolStripPanel.PerformLayout();
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStripPanel _toolStripPanel;
        private ToolStrip _subsystemToolStrip;
        private ToolStripLabel _lineNumberNumericLabel;
        private ToolStripLabel _activeLevelComboBoxLabel;
        private ToolStripComboBox _activeLevelComboBox;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _applySettingsMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private ToolStripButton _readButton;
        private cc.isr.WinControls.ToolStripNumberBox _lineLevelNumberBox;
        private ToolStripButton _writeButton;
        private cc.isr.WinControls.ToolStripNumberBox _lineNumberBox;
        private ToolStripButton _toggleButton;
        private ToolStripButton _pulseButton;
        private cc.isr.WinControls.ToolStripNumberBox _pulseWidthNumberBox;
    }
}
