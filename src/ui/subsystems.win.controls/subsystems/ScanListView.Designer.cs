using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class ScanListView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            _subsystemToolStrip = new ToolStrip();
            _subsytemSplitButton = new ToolStripSplitButton();
            _applySettingsMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem.Click += new EventHandler(ApplySettingsMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsMenuItem_Click);
            _closeMenuItem = new ToolStripMenuItem();
            _closeMenuItem.Click += new EventHandler(CloseMenuItem_Click);
            _openMenuItem = new ToolStripMenuItem();
            _openMenuItem.Click += new EventHandler(OpenMenuItem_Click);
            _openAllMenuItem = new ToolStripMenuItem();
            _openAllMenuItem.Click += new EventHandler(OpenAllMenuItem_Click);
            _saveToMemoryLocationMenuItem = new ToolStripMenuItem();
            _memoryLocationTextBox = new ToolStripTextBox();
            _memoryLocationTextBox.Validating += new System.ComponentModel.CancelEventHandler(MemoryLocationTextBox_Validating);
            _SaveToMemoryLocationMenuItem = new ToolStripMenuItem();
            _SaveToMemoryLocationMenuItem.Click += new EventHandler(SaveToMemoryLocationMenuItem_Click);
            _candidateScanListComboBox = new ToolStripComboBox();
            _scanListLabel = new ToolStripLabel();
            _closedChannelsLabel = new ToolStripLabel();
            _subsystemToolStrip.SuspendLayout();
            SuspendLayout();
            //
            // _subsystemToolStrip
            //
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsytemSplitButton, _candidateScanListComboBox, _scanListLabel, _closedChannelsLabel });
            _subsystemToolStrip.Location = new System.Drawing.Point(1, 1);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(352, 25);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 15;
            _subsystemToolStrip.Text = "Subsystem Tool Strip";
            //
            // _subsytemSplitButton
            //
            _subsytemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applySettingsMenuItem, _readSettingsMenuItem, _closeMenuItem, _openMenuItem, _openAllMenuItem, _saveToMemoryLocationMenuItem });
            _subsytemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsytemSplitButton.Name = "_SubsytemSplitButton";
            _subsytemSplitButton.Size = new System.Drawing.Size(69, 22);
            _subsytemSplitButton.Text = "Scan List";
            _subsytemSplitButton.ToolTipText = "Scan List actions";
            //
            // _applySettingsMenuItem
            //
            _applySettingsMenuItem.Name = "_applySettingsMenuItem";
            _applySettingsMenuItem.Size = new System.Drawing.Size(163, 22);
            _applySettingsMenuItem.Text = "Apply Settings";
            //
            // _readSettingsMenuItem
            //
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(163, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            //
            // _closeMenuItem
            //
            _closeMenuItem.Name = "_CloseMenuItem";
            _closeMenuItem.Size = new System.Drawing.Size(163, 22);
            _closeMenuItem.Text = "Close";
            //
            // _openMenuItem
            //
            _openMenuItem.Name = "_OpenMenuItem";
            _openMenuItem.Size = new System.Drawing.Size(163, 22);
            _openMenuItem.Text = "Open";
            //
            // _openAllMenuItem
            //
            _openAllMenuItem.Name = "_OpenAllMenuItem";
            _openAllMenuItem.Size = new System.Drawing.Size(163, 22);
            _openAllMenuItem.Text = "Open All";
            //
            // _saveToMemoryLocationMenuItem
            //
            _saveToMemoryLocationMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _memoryLocationTextBox, _SaveToMemoryLocationMenuItem });
            _saveToMemoryLocationMenuItem.Name = "_SaveToMemoryLocationMenuItem";
            _saveToMemoryLocationMenuItem.Size = new System.Drawing.Size(163, 22);
            _saveToMemoryLocationMenuItem.Text = "Memory Location";
            //
            // _memoryLocationTextBox
            //
            _memoryLocationTextBox.Font = new System.Drawing.Font("Segoe UI", 9.0f);
            _memoryLocationTextBox.Name = "_MemoryLocationTextBox";
            _memoryLocationTextBox.Size = new System.Drawing.Size(100, 23);
            _memoryLocationTextBox.Text = "1";
            //
            // _SaveToMemoryLocationMenuItem
            //
            _SaveToMemoryLocationMenuItem.Name = "_SaveToMemoryLocationMenuItem";
            _SaveToMemoryLocationMenuItem.Size = new System.Drawing.Size(160, 22);
            _SaveToMemoryLocationMenuItem.Text = "Save";
            //
            // _candidateScanListComboBox
            //
            _candidateScanListComboBox.Items.AddRange(new object[] { "(@1!1:1!10)", "(@M1,M2)", "(@ 4!1,5!1)", "(@1!1)", "(@1!2:1!10, 2!3!6)", "(@1!30:1!40, 2!1:2!10)", "(@1!4!1:1!4!10, 2!1!1:2!4!10)" });
            _candidateScanListComboBox.Name = "_CandidateScanListComboBox";
            _candidateScanListComboBox.Size = new System.Drawing.Size(141, 25);
            _candidateScanListComboBox.Text = "(@1!1:1!10)";
            _candidateScanListComboBox.ToolTipText = "Edits the scan list";
            //
            // _scanListLabel
            //
            _scanListLabel.Name = "_ScanListLabel";
            _scanListLabel.Size = new System.Drawing.Size(12, 22);
            _scanListLabel.Text = "?";
            //
            // _closedChannelsLabel
            //
            _closedChannelsLabel.Name = "_ClosedChannelsLabel";
            _closedChannelsLabel.Size = new System.Drawing.Size(57, 22);
            _closedChannelsLabel.Text = "<closed>";
            _closedChannelsLabel.ToolTipText = "Scan list of closed channels";
            //
            // ScanListView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_subsystemToolStrip);
            Name = "ScanListView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(354, 28);
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStripComboBox _candidateScanListComboBox;
        private ToolStrip _subsystemToolStrip;
        private ToolStripSplitButton _subsytemSplitButton;
        private ToolStripMenuItem _applySettingsMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private ToolStripLabel _scanListLabel;
        private ToolStripTextBox _memoryLocationTextBox;
        private ToolStripMenuItem _SaveToMemoryLocationMenuItem;
        private ToolStripMenuItem _saveToMemoryLocationMenuItem;
        private ToolStripMenuItem _openAllMenuItem;
        private ToolStripMenuItem _closeMenuItem;
        private ToolStripMenuItem _openMenuItem;
        private ToolStripLabel _closedChannelsLabel;
    }
}
