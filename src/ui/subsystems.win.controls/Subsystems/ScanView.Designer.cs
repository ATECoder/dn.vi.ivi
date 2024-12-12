using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class ScanView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanView));
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _applyInternalScanListMenuItem = new ToolStripMenuItem();
            _applyInternalScanListMenuItem.Click += new EventHandler(ApplyInternalScanListMenuItem_Click);
            _applyInternalScanFunctionListMenuItem = new ToolStripMenuItem();
            _applyInternalScanFunctionListMenuItem.Click += new EventHandler(ApplyInternalScanFunctionListMenuItem_Click);
            _selectInternalScanListMenuItem = new ToolStripMenuItem();
            _selectInternalScanListMenuItem.Click += new EventHandler(SelectInternalScanListMenuItem_Click);
            _releaseInternalScanListMenuItem = new ToolStripMenuItem();
            _releaseInternalScanListMenuItem.Click += new EventHandler(ReleaseInternalScanListMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsMenuItem_Click);
            _candidateScanListComboBox = new ToolStripComboBox();
            _internalScanListLabel = new ToolStripLabel();
            _scanListFunctionLabel = new ToolStripLabel();
            _closedChannelsLabel = new ToolStripLabel();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _subsystemToolStrip
            // 
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _candidateScanListComboBox, _internalScanListLabel, _scanListFunctionLabel, _closedChannelsLabel });
            _subsystemToolStrip.Location = new System.Drawing.Point(1, 1);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(422, 25);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 0;
            _subsystemToolStrip.Text = "ChannelToolStrip";
            // 
            // _subsystemSplitButton
            // 
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applyInternalScanListMenuItem, _applyInternalScanFunctionListMenuItem, _selectInternalScanListMenuItem, _releaseInternalScanListMenuItem, _readSettingsMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.Image = (System.Drawing.Image)resources.GetObject("_SubsystemSplitButton.Image");
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(48, 22);
            _subsystemSplitButton.Text = "Scan";
            _subsystemSplitButton.ToolTipText = "Select action";
            // 
            // _applyInternalScanListMenuItem
            // 
            _applyInternalScanListMenuItem.Name = "_applyInternalScanListMenuItem";
            _applyInternalScanListMenuItem.Size = new System.Drawing.Size(246, 22);
            _applyInternalScanListMenuItem.Text = "Apply Internal Scan List";
            _applyInternalScanListMenuItem.ToolTipText = "Applies the list as an internal scan list";
            // 
            // _applyInternalScanFunctionListMenuItem
            // 
            _applyInternalScanFunctionListMenuItem.Name = "_applyInternalScanFunctionListMenuItem";
            _applyInternalScanFunctionListMenuItem.Size = new System.Drawing.Size(246, 22);
            _applyInternalScanFunctionListMenuItem.Text = "Apply Internal Scan Function List";
            _applyInternalScanFunctionListMenuItem.ToolTipText = "Applies the list as an internal function scan list";
            // 
            // _selectInternalScanListMenuItem
            // 
            _selectInternalScanListMenuItem.Name = "_SelectInternalScanListMenuItem";
            _selectInternalScanListMenuItem.Size = new System.Drawing.Size(246, 22);
            _selectInternalScanListMenuItem.Text = "Select Internal Scan List";
            _selectInternalScanListMenuItem.ToolTipText = "Select the list as an internal scan list";
            // 
            // _releaseInternalScanListMenuItem
            // 
            _releaseInternalScanListMenuItem.Name = "_ReleaseInternalScanListMenuItem";
            _releaseInternalScanListMenuItem.Size = new System.Drawing.Size(246, 22);
            _releaseInternalScanListMenuItem.Text = "Release Internal Scan List";
            _releaseInternalScanListMenuItem.ToolTipText = "Releases the internal scan list";
            // 
            // _readSettingsMenuItem
            // 
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(246, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            // 
            // _candidateScanListComboBox
            // 
            _candidateScanListComboBox.AutoSize = false;
            _candidateScanListComboBox.Items.AddRange(new object[] { "(@1:3)" });
            _candidateScanListComboBox.Name = "_CandidateScanListComboBox";
            _candidateScanListComboBox.Size = new System.Drawing.Size(140, 23);
            _candidateScanListComboBox.Text = "(@1:3)";
            _candidateScanListComboBox.ToolTipText = "Candidate scan list in the format:  (@<card>:<relay>)";
            // 
            // _internalScanListLabel
            // 
            _internalScanListLabel.Margin = new Padding(3, 1, 0, 2);
            _internalScanListLabel.Name = "_InternalScanListLabel";
            _internalScanListLabel.Size = new System.Drawing.Size(55, 22);
            _internalScanListLabel.Text = "Internal ?";
            _internalScanListLabel.ToolTipText = "Internal scan list";
            // 
            // _scanListFunctionLabel
            // 
            _scanListFunctionLabel.Margin = new Padding(3, 1, 0, 2);
            _scanListFunctionLabel.Name = "_ScanListFunctionLabel";
            _scanListFunctionLabel.Size = new System.Drawing.Size(62, 22);
            _scanListFunctionLabel.Text = "Function ?";
            _scanListFunctionLabel.ToolTipText = "Function scan list";
            // 
            // _closedChannelsLabel
            // 
            _closedChannelsLabel.Name = "_ClosedChannelsLabel";
            _closedChannelsLabel.Size = new System.Drawing.Size(51, 22);
            _closedChannelsLabel.Text = "Closed ?";
            _closedChannelsLabel.ToolTipText = "Closed channel(s)";
            // 
            // ScanView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(_subsystemToolStrip);
            Name = "ScanView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(424, 27);
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStripLabel _internalScanListLabel;
        private ToolStrip _subsystemToolStrip;
        private ToolStripLabel _scanListFunctionLabel;
        private ToolStripLabel _closedChannelsLabel;
        private ToolStripMenuItem _releaseInternalScanListMenuItem;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _applyInternalScanListMenuItem;
        private ToolStripMenuItem _applyInternalScanFunctionListMenuItem;
        private ToolStripMenuItem _selectInternalScanListMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private ToolStripComboBox _candidateScanListComboBox;
    }
}
