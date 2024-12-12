using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class RelayView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(RelayView));
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _addRelayMenuItem = new ToolStripMenuItem();
            _addRelayMenuItem.Click += new EventHandler(AddChannelToList_Click);
            _addMemoryLocationToListMenuItem = new ToolStripMenuItem();
            _addMemoryLocationToListMenuItem.Click += new EventHandler(AddMemoryLocationButton_Click);
            _clearChannelLIstMenuItem = new ToolStripMenuItem();
            _clearChannelLIstMenuItem.Click += new EventHandler(ClearChannelLIstMenuItem_Click);
            _slotNumberComboBoxLabel = new ToolStripLabel();
            _slotNumberNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _relayNumberNumericLabel = new ToolStripLabel();
            _relayNumberNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _memoryLocationNumberLabel = new ToolStripLabel();
            _memoryLocationNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _subsystemToolStrip
            // 
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _slotNumberComboBoxLabel, _slotNumberNumeric, _relayNumberNumericLabel, _relayNumberNumeric, _memoryLocationNumberLabel, _memoryLocationNumeric });
            _subsystemToolStrip.Location = new System.Drawing.Point(1, 1);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(475, 26);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 2;
            _subsystemToolStrip.Text = "ChannelToolStrip";
            // 
            // _subsystemSplitButton
            // 
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _addRelayMenuItem, _addMemoryLocationToListMenuItem, _clearChannelLIstMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.Image = (System.Drawing.Image)resources.GetObject("_SubsystemSplitButton.Image");
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(52, 23);
            _subsystemSplitButton.Text = "Relay";
            _subsystemSplitButton.ToolTipText = "Select action";
            // 
            // _addRelayMenuItem
            // 
            _addRelayMenuItem.Name = "_addRelayMenuItem";
            _addRelayMenuItem.Size = new System.Drawing.Size(222, 22);
            _addRelayMenuItem.Text = "Add Relay to List";
            _addRelayMenuItem.ToolTipText = "Adds a relay to the channel list";
            // 
            // _addMemoryLocationToListMenuItem
            // 
            _addMemoryLocationToListMenuItem.Name = "_addMemoryLocationToListMenuItem";
            _addMemoryLocationToListMenuItem.Size = new System.Drawing.Size(222, 22);
            _addMemoryLocationToListMenuItem.Text = "Add Memory Location to List";
            _addMemoryLocationToListMenuItem.ToolTipText = "Adds a memory location to the list";
            // 
            // _clearChannelLIstMenuItem
            // 
            _clearChannelLIstMenuItem.Name = "_ClearChannelLIstMenuItem";
            _clearChannelLIstMenuItem.Size = new System.Drawing.Size(222, 22);
            _clearChannelLIstMenuItem.Text = "Clear List";
            _clearChannelLIstMenuItem.ToolTipText = "Clears the channel list builder";
            // 
            // _slotNumberComboBoxLabel
            // 
            _slotNumberComboBoxLabel.Margin = new Padding(3, 1, 0, 2);
            _slotNumberComboBoxLabel.Name = "_SlotNumberComboBoxLabel";
            _slotNumberComboBoxLabel.Size = new System.Drawing.Size(27, 23);
            _slotNumberComboBoxLabel.Text = "Slot";
            // 
            // _slotNumberNumeric
            // 
            _slotNumberNumeric.Name = "_SlotNumberNumeric";
            _slotNumberNumeric.Size = new System.Drawing.Size(41, 23);
            _slotNumberNumeric.Text = "0";
            _slotNumberNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // _relayNumberNumericLabel
            // 
            _relayNumberNumericLabel.Name = "_RelayNumberNumericLabel";
            _relayNumberNumericLabel.Size = new System.Drawing.Size(38, 23);
            _relayNumberNumericLabel.Text = "Relay:";
            // 
            // _relayNumberNumeric
            // 
            _relayNumberNumeric.Margin = new Padding(0, 1, 0, 1);
            _relayNumberNumeric.Name = "_RelayNumberNumeric";
            _relayNumberNumeric.Size = new System.Drawing.Size(41, 24);
            _relayNumberNumeric.Text = "0";
            _relayNumberNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // _memoryLocationNumberLabel
            // 
            _memoryLocationNumberLabel.Margin = new Padding(3, 1, 0, 2);
            _memoryLocationNumberLabel.Name = "_MemoryLocationNumberLabel";
            _memoryLocationNumberLabel.Size = new System.Drawing.Size(104, 23);
            _memoryLocationNumberLabel.Text = "Memory Location:";
            // 
            // _memoryLocationNumeric
            // 
            _memoryLocationNumeric.Name = "_MemoryLocationNumeric";
            _memoryLocationNumeric.Size = new System.Drawing.Size(41, 23);
            _memoryLocationNumeric.Text = "0";
            _memoryLocationNumeric.ToolTipText = "Memory location";
            _memoryLocationNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // RelayView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_subsystemToolStrip);
            Name = "RelayView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(477, 26);
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStrip _subsystemToolStrip;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _addRelayMenuItem;
        private ToolStripMenuItem _clearChannelLIstMenuItem;
        private ToolStripLabel _slotNumberComboBoxLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _slotNumberNumeric;
        private ToolStripLabel _relayNumberNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _relayNumberNumeric;
        private ToolStripLabel _memoryLocationNumberLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _memoryLocationNumeric;
        private ToolStripMenuItem _addMemoryLocationToListMenuItem;
    }
}
