using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class BinningView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(BinningView));
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _applySettingsMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem.Click += new EventHandler(ApplySettingsMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsMenuItem_Click);
            _preformLimitTestMenuItem = new ToolStripMenuItem();
            _preformLimitTestMenuItem.Click += new EventHandler(PreformLimitTestMenuItem_Click);
            _readLimitTestStateMenuItem = new ToolStripMenuItem();
            _readLimitTestStateMenuItem.Click += new EventHandler(ReadStateMenuItem_Click);
            _passBitPatternNumericButton = new ToolStripButton();
            _passBitPatternNumericButton.CheckStateChanged += new EventHandler(PassBitPatternNumericButton_CheckStateChanged);
            _passBitPatternNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _BinningStrobeToggleButton = new ToolStripButton();
            _BinningStrobeToggleButton.CheckStateChanged += new EventHandler(BinningStrobeToggleButton_CheckStateChanged);
            _limitFailedButton = new ToolStripButton();
            _limitFailedButton.CheckStateChanged += new EventHandler(LimitFailedButton_CheckStateChanged);
            _binningStrobeDurationNumericLabel = new ToolStripLabel();
            _binningStrobeDurationNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _binningStrobeDurationNumericUnitLabel = new ToolStripLabel();
            _limit2View = new LimitView();
            _limit1View = new LimitView();
            _toolStripPanel = new ToolStripPanel();
            _infoTextBox = new TextBox();
            _digitalOutputActiveHighMenuItem = new ToolStripMenuItem();
            _subsystemToolStrip.SuspendLayout();
            _toolStripPanel.SuspendLayout();
            SuspendLayout();
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.AutoSize = false;
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.Dock = DockStyle.None;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _passBitPatternNumericButton, _passBitPatternNumeric, _BinningStrobeToggleButton, _limitFailedButton, _binningStrobeDurationNumericLabel, _binningStrobeDurationNumeric, _binningStrobeDurationNumericUnitLabel });
            _subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(403, 28);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 0;
            //
            // _subsystemSplitButton
            //
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applySettingsMenuItem, _readSettingsMenuItem, _preformLimitTestMenuItem, _readLimitTestStateMenuItem, _digitalOutputActiveHighMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(42, 25);
            _subsystemSplitButton.Text = "Bin";
            //
            // _applySettingsMenuItem
            //
            _applySettingsMenuItem.Name = "_applySettingsMenuItem";
            _applySettingsMenuItem.Size = new System.Drawing.Size(215, 22);
            _applySettingsMenuItem.Text = "Apply Settings";
            _applySettingsMenuItem.ToolTipText = "Applies the limit settings to the instrument";
            //
            // _readSettingsMenuItem
            //
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(215, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            _readSettingsMenuItem.ToolTipText = "Read current settings from the instrument";
            //
            // _preformLimitTestMenuItem
            //
            _preformLimitTestMenuItem.Name = "_PreformLimitTestMenuItem";
            _preformLimitTestMenuItem.Size = new System.Drawing.Size(215, 22);
            _preformLimitTestMenuItem.Text = "Preform Limit Test";
            _preformLimitTestMenuItem.ToolTipText = "Performs the limit test";
            //
            // _readLimitTestStateMenuItem
            //
            _readLimitTestStateMenuItem.Name = "_ReadLimitTestStateMenuItem";
            _readLimitTestStateMenuItem.Size = new System.Drawing.Size(215, 22);
            _readLimitTestStateMenuItem.Text = "Read Limit Test State";
            _readLimitTestStateMenuItem.ToolTipText = "Performs and reads limit test results";
            //
            // _passBitPatternNumericButton
            //
            _passBitPatternNumericButton.CheckOnClick = true;
            _passBitPatternNumericButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _passBitPatternNumericButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _passBitPatternNumericButton.Name = "_PassBitPatternNumericButton";
            _passBitPatternNumericButton.Size = new System.Drawing.Size(52, 25);
            _passBitPatternNumericButton.Text = "Pass: 0x";
            //
            // _passBitPatternNumeric
            //
            _passBitPatternNumeric.Name = "_PassBitPatternNumeric";
            _passBitPatternNumeric.Size = new System.Drawing.Size(41, 25);
            _passBitPatternNumeric.Text = "2";
            _passBitPatternNumeric.ToolTipText = "Pass bit pattern";
            _passBitPatternNumeric.Value = new decimal(new int[] { 2, 0, 0, 0 });
            //
            // _BinningStrobeToggleButton
            //
            _BinningStrobeToggleButton.Checked = true;
            _BinningStrobeToggleButton.CheckOnClick = true;
            _BinningStrobeToggleButton.CheckState = CheckState.Indeterminate;
            _BinningStrobeToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _BinningStrobeToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _BinningStrobeToggleButton.Name = "_BinningStrobeToggleButton";
            _BinningStrobeToggleButton.Size = new System.Drawing.Size(52, 25);
            _BinningStrobeToggleButton.Text = "Strobe: ?";
            _BinningStrobeToggleButton.ToolTipText = "Toggle to enable or disable binning strobe";
            //
            // _limitFailedButton
            //
            _limitFailedButton.Checked = true;
            _limitFailedButton.CheckState = CheckState.Indeterminate;
            _limitFailedButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _limitFailedButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _limitFailedButton.Name = "_LimitFailedButton";
            _limitFailedButton.Size = new System.Drawing.Size(47, 25);
            _limitFailedButton.Text = "Failed?";
            _limitFailedButton.ToolTipText = "Displays limit state";
            //
            // _binningStrobeDurationNumericLabel
            //
            _binningStrobeDurationNumericLabel.Margin = new Padding(3, 1, 0, 2);
            _binningStrobeDurationNumericLabel.Name = "_BinningStrobeDurationNumericLabel";
            _binningStrobeDurationNumericLabel.Size = new System.Drawing.Size(56, 25);
            _binningStrobeDurationNumericLabel.Text = "Duration:";
            //
            // _binningStrobeDurationNumeric
            //
            _binningStrobeDurationNumeric.Name = "_BinningStrobeDurationNumeric";
            _binningStrobeDurationNumeric.Size = new System.Drawing.Size(41, 25);
            _binningStrobeDurationNumeric.Text = "0";
            _binningStrobeDurationNumeric.ToolTipText = "Duration of binning strobe to use for timing the trigger plan";
            _binningStrobeDurationNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            //
            // _binningStrobeDurationNumericUnitLabel
            //
            _binningStrobeDurationNumericUnitLabel.Name = "_BinningStrobeDurationNumericUnitLabel";
            _binningStrobeDurationNumericUnitLabel.Size = new System.Drawing.Size(23, 25);
            _binningStrobeDurationNumericUnitLabel.Text = "ms";
            //
            // _limit2View
            //
            _limit2View.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _limit2View.Dock = DockStyle.Top;
            _limit2View.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _limit2View.Location = new System.Drawing.Point(1, 111);
            _limit2View.Name = "_Limit2View";
            _limit2View.Padding = new Padding(1);
            _limit2View.Size = new System.Drawing.Size(403, 82);
            _limit2View.TabIndex = 3;
            //
            // _limit1View
            //
            _limit1View.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _limit1View.Dock = DockStyle.Top;
            _limit1View.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _limit1View.Location = new System.Drawing.Point(1, 29);
            _limit1View.Name = "_Limit1View";
            _limit1View.Padding = new Padding(1);
            _limit1View.Size = new System.Drawing.Size(403, 82);
            _limit1View.TabIndex = 1;
            //
            // _toolStripPanel
            //
            _toolStripPanel.BackColor = System.Drawing.Color.Transparent;
            _toolStripPanel.Controls.Add(_subsystemToolStrip);
            _toolStripPanel.Dock = DockStyle.Top;
            _toolStripPanel.Location = new System.Drawing.Point(1, 1);
            _toolStripPanel.Name = "_ToolStripPanel";
            _toolStripPanel.Orientation = Orientation.Horizontal;
            _toolStripPanel.RowMargin = new Padding(3, 0, 0, 0);
            _toolStripPanel.Size = new System.Drawing.Size(403, 28);
            //
            // _infoTextBox
            //
            _infoTextBox.Dock = DockStyle.Fill;
            _infoTextBox.Location = new System.Drawing.Point(1, 193);
            _infoTextBox.Margin = new Padding(3, 4, 3, 4);
            _infoTextBox.Multiline = true;
            _infoTextBox.Name = "_InfoTextBox";
            _infoTextBox.ReadOnly = true;
            _infoTextBox.ScrollBars = ScrollBars.Both;
            _infoTextBox.Size = new System.Drawing.Size(403, 147);
            _infoTextBox.TabIndex = 26;
            _infoTextBox.Text = "<info>";
            //
            // _digitalOutputActiveHighMenuItem
            //
            _digitalOutputActiveHighMenuItem.CheckOnClick = true;
            _digitalOutputActiveHighMenuItem.Name = "_DigitalOutputActiveHighMenuItem";
            _digitalOutputActiveHighMenuItem.Size = new System.Drawing.Size(215, 22);
            _digitalOutputActiveHighMenuItem.Text = "Digital Output Active High";
            _digitalOutputActiveHighMenuItem.ToolTipText = "Toggles binning digital output polarity";
            //
            // BinningView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_infoTextBox);
            Controls.Add(_limit2View);
            Controls.Add(_limit1View);
            Controls.Add(_toolStripPanel);
            Name = "BinningView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(405, 341);
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            _toolStripPanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStrip _subsystemToolStrip;
        private ToolStripButton _passBitPatternNumericButton;
        private cc.isr.WinControls.ToolStripNumericUpDown _passBitPatternNumeric;
        private LimitView _limit2View;
        private ToolStripButton _BinningStrobeToggleButton;
        private ToolStripButton _limitFailedButton;
        private ToolStripMenuItem _applySettingsMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private ToolStripMenuItem _readLimitTestStateMenuItem;
        private LimitView _limit1View;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripLabel _binningStrobeDurationNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _binningStrobeDurationNumeric;
        private ToolStripLabel _binningStrobeDurationNumericUnitLabel;
        private ToolStripPanel _toolStripPanel;
        private ToolStripMenuItem _preformLimitTestMenuItem;
        private TextBox _infoTextBox;
        private ToolStripMenuItem _digitalOutputActiveHighMenuItem;
    }
}
