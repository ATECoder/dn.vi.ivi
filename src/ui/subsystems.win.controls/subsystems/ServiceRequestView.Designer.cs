using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class ServiceRequestView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceRequestView));
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _serviceRequestEnabledMenuItem = new ToolStripMenuItem();
            _usingNegativeTransitionsMenuItem = new ToolStripMenuItem();
            _toggleServiceRequestMenuItem = new ToolStripMenuItem();
            _toggleServiceRequestMenuItem.Click += new EventHandler(ToggleServiceRequestMenuItem_Click);
            _hexadecimalToolStripMenuItem = new ToolStripMenuItem();
            _hexadecimalToolStripMenuItem.Click += new EventHandler(HexadecimalToolStripMenuItem_Click);
            _serviceRequestFlagsComboBox = new ToolStripComboBox();
            _serviceRequestMaskAddButton = new ToolStripButton();
            _serviceRequestMaskRemoveButton = new ToolStripButton();
            _serviceRequestMaskNumericLabel = new ToolStripLabel();
            _serviceRequestMaskNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _serviceRequestFlagsComboBox, _serviceRequestMaskAddButton, _serviceRequestMaskRemoveButton, _serviceRequestMaskNumericLabel, _serviceRequestMaskNumeric });
            _subsystemToolStrip.Location = new System.Drawing.Point(1, 1);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(340, 26);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 1;
            _subsystemToolStrip.Text = "Service Request Toolstrip";
            //
            // _subsystemSplitButton
            //
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _serviceRequestEnabledMenuItem, _usingNegativeTransitionsMenuItem, _toggleServiceRequestMenuItem, _hexadecimalToolStripMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(45, 23);
            _subsystemSplitButton.Text = "SRQ";
            _subsystemSplitButton.ToolTipText = "SRQ Options and actions";
            //
            // _serviceRequestEnabledMenuItem
            //
            _serviceRequestEnabledMenuItem.CheckOnClick = true;
            _serviceRequestEnabledMenuItem.Name = "_ServiceRequestEnabledMenuItem";
            _serviceRequestEnabledMenuItem.Size = new System.Drawing.Size(213, 22);
            _serviceRequestEnabledMenuItem.Text = "Service Request Enabled";
            _serviceRequestEnabledMenuItem.ToolTipText = "Service request handling enabled when checked";
            //
            // _usingNegativeTransitionsMenuItem
            //
            _usingNegativeTransitionsMenuItem.CheckOnClick = true;
            _usingNegativeTransitionsMenuItem.Name = "_UsingNegativeTransitionsMenuItem";
            _usingNegativeTransitionsMenuItem.Size = new System.Drawing.Size(213, 22);
            _usingNegativeTransitionsMenuItem.Text = "Using Negative transitions";
            _usingNegativeTransitionsMenuItem.ToolTipText = "Uses negative transition when checked";
            //
            // _toggleServiceRequestMenuItem
            //
            _toggleServiceRequestMenuItem.CheckOnClick = true;
            _toggleServiceRequestMenuItem.Name = "_ToggleServiceRequestMenuItem";
            _toggleServiceRequestMenuItem.Size = new System.Drawing.Size(213, 22);
            _toggleServiceRequestMenuItem.Text = "Enable Service Request";
            //
            // _hexadecimalToolStripMenuItem
            //
            _hexadecimalToolStripMenuItem.Checked = true;
            _hexadecimalToolStripMenuItem.CheckOnClick = true;
            _hexadecimalToolStripMenuItem.CheckState = CheckState.Checked;
            _hexadecimalToolStripMenuItem.Name = "_HexadecimalToolStripMenuItem";
            _hexadecimalToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            _hexadecimalToolStripMenuItem.Text = "Hexadecimal";
            _hexadecimalToolStripMenuItem.ToolTipText = "Toggles displays of service request max in hex or decimal";
            //
            // _serviceRequestFlagsComboBox
            //
            _serviceRequestFlagsComboBox.Name = "_ServiceRequestFlagsComboBox";
            _serviceRequestFlagsComboBox.Size = new System.Drawing.Size(141, 26);
            //
            // _serviceRequestMaskAddButton
            //
            _serviceRequestMaskAddButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _serviceRequestMaskAddButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _serviceRequestMaskAddButton.Name = "_ServiceRequestMaskAddButton";
            _serviceRequestMaskAddButton.Size = new System.Drawing.Size(23, 23);
            _serviceRequestMaskAddButton.Text = "+";
            _serviceRequestMaskAddButton.ToolTipText = "Add to mask";
            //
            // _serviceRequestMaskRemoveButton
            //
            _serviceRequestMaskRemoveButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _serviceRequestMaskRemoveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _serviceRequestMaskRemoveButton.Name = "_ServiceRequestMaskRemoveButton";
            _serviceRequestMaskRemoveButton.Size = new System.Drawing.Size(23, 23);
            _serviceRequestMaskRemoveButton.Text = "-";
            //
            // _serviceRequestMaskNumericLabel
            //
            _serviceRequestMaskNumericLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _serviceRequestMaskNumericLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            _serviceRequestMaskNumericLabel.Name = "_ServiceRequestMaskNumericLabel";
            _serviceRequestMaskNumericLabel.Size = new System.Drawing.Size(19, 23);
            _serviceRequestMaskNumericLabel.Text = "0x";
            //
            // _serviceRequestMaskNumeric
            //
            _serviceRequestMaskNumeric.Name = "_ServiceRequestMaskNumeric";
            _serviceRequestMaskNumeric.Size = new System.Drawing.Size(41, 23);
            _serviceRequestMaskNumeric.Text = "0";
            _serviceRequestMaskNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            //
            // ServiceRequestView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_subsystemToolStrip);
            Name = "ServiceRequestView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(342, 28);
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStrip _subsystemToolStrip;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _serviceRequestEnabledMenuItem;
        private ToolStripMenuItem _usingNegativeTransitionsMenuItem;
        private ToolStripMenuItem _toggleServiceRequestMenuItem;
        private ToolStripComboBox _serviceRequestFlagsComboBox;
        private ToolStripButton _serviceRequestMaskAddButton;
        private ToolStripButton _serviceRequestMaskRemoveButton;
        private ToolStripLabel _serviceRequestMaskNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _serviceRequestMaskNumeric;
        private ToolStripMenuItem _hexadecimalToolStripMenuItem;
    }
}
