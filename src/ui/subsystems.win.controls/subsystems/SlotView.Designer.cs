using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class SlotView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(SlotView));
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _applySettingsMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem.Click += new EventHandler(ApplySettingsMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsMenuItem_Click);
            _slotNumberComboBoxLabel = new ToolStripLabel();
            _slotNumberNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _cardTypeTextBoxLabel = new ToolStripLabel();
            _cardTypeTextBox = new ToolStripTextBox();
            _settlingTimeNumericLabel = new ToolStripLabel();
            _settlingTimeNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _settlingTimeUnitLabel = new ToolStripLabel();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _subsystemToolStrip
            // 
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _slotNumberComboBoxLabel, _slotNumberNumeric, _cardTypeTextBoxLabel, _cardTypeTextBox, _settlingTimeNumericLabel, _settlingTimeNumeric, _settlingTimeUnitLabel });
            _subsystemToolStrip.Location = new System.Drawing.Point(1, 1);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(447, 27);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 1;
            _subsystemToolStrip.Text = "Slot Tool Strip";
            // 
            // _subsystemSplitButton
            // 
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applySettingsMenuItem, _readSettingsMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.Image = (System.Drawing.Image)resources.GetObject("_SubsystemSplitButton.Image");
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(42, 24);
            _subsystemSplitButton.Text = "Slot";
            _subsystemSplitButton.ToolTipText = "Select action";
            // 
            // _applySettingsMenuItem
            // 
            _applySettingsMenuItem.Name = "_applySettingsMenuItem";
            _applySettingsMenuItem.Size = new System.Drawing.Size(180, 22);
            _applySettingsMenuItem.Text = "Apply Settings";
            _applySettingsMenuItem.ToolTipText = "Applies the settings onto the instrument";
            // 
            // _readSettingsMenuItem
            // 
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(180, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            // 
            // _slotNumberComboBoxLabel
            // 
            _slotNumberComboBoxLabel.Margin = new Padding(3, 1, 0, 2);
            _slotNumberComboBoxLabel.Name = "_SlotNumberComboBoxLabel";
            _slotNumberComboBoxLabel.Size = new System.Drawing.Size(14, 24);
            _slotNumberComboBoxLabel.Text = "#";
            // 
            // _slotNumberNumeric
            // 
            _slotNumberNumeric.Name = "_SlotNumberNumeric";
            _slotNumberNumeric.Size = new System.Drawing.Size(41, 24);
            _slotNumberNumeric.Text = "0";
            _slotNumberNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // _cardTypeTextBoxLabel
            // 
            _cardTypeTextBoxLabel.Name = "_CardTypeTextBoxLabel";
            _cardTypeTextBoxLabel.Size = new System.Drawing.Size(34, 24);
            _cardTypeTextBoxLabel.Text = "Type:";
            // 
            // _cardTypeTextBox
            // 
            _cardTypeTextBox.Font = new System.Drawing.Font("Segoe UI", 9.0f);
            _cardTypeTextBox.Name = "_CardTypeTextBox";
            _cardTypeTextBox.Size = new System.Drawing.Size(100, 27);
            _cardTypeTextBox.ToolTipText = "Card type";
            // 
            // _settlingTimeNumericLabel
            // 
            _settlingTimeNumericLabel.Margin = new Padding(2, 1, 0, 2);
            _settlingTimeNumericLabel.Name = "_SettlingTimeNumericLabel";
            _settlingTimeNumericLabel.Size = new System.Drawing.Size(50, 24);
            _settlingTimeNumericLabel.Text = "Settling:";
            // 
            // _settlingTimeNumeric
            // 
            _settlingTimeNumeric.AutoSize = false;
            _settlingTimeNumeric.Margin = new Padding(0, 1, 0, 1);
            _settlingTimeNumeric.Name = "_SettlingTimeNumeric";
            _settlingTimeNumeric.Size = new System.Drawing.Size(45, 25);
            _settlingTimeNumeric.Text = "0";
            _settlingTimeNumeric.ToolTipText = "Relay settling time";
            _settlingTimeNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // _settlingTimeUnitLabel
            // 
            _settlingTimeUnitLabel.Name = "_SettlingTimeUnitLabel";
            _settlingTimeUnitLabel.Size = new System.Drawing.Size(23, 24);
            _settlingTimeUnitLabel.Text = "ms";
            // 
            // SlotView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_subsystemToolStrip);
            Name = "SlotView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(449, 29);
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStrip _subsystemToolStrip;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _applySettingsMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private ToolStripLabel _slotNumberComboBoxLabel;
        private ToolStripLabel _settlingTimeNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _settlingTimeNumeric;
        private ToolStripLabel _cardTypeTextBoxLabel;
        private ToolStripTextBox _cardTypeTextBox;
        private ToolStripLabel _settlingTimeUnitLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _slotNumberNumeric;
    }
}
