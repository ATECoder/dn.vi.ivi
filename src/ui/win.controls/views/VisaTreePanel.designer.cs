using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.WinControls
{
    public partial class VisaTreePanel
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            StatusLabel = new ToolStripStatusLabel();
            StatusStrip = new StatusStrip();
            ProgressBar = new cc.isr.WinControls.StatusStripCustomProgressBar();
            _layout = new TableLayoutPanel();
            TraceMessagesBox = new cc.isr.Logging.TraceLog.WinForms.MessagesBox();
            _treePanel = new cc.isr.WinControls.TreePanel();
            _treePanel.AfterNodeSelected += new EventHandler<TreeViewEventArgs>(TreePanel_afterNodeSelected);
            StatusStrip.SuspendLayout();
            _layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_treePanel).BeginInit();
            _treePanel.SuspendLayout();
            SuspendLayout();
            //
            // _statusLabel
            //
            StatusLabel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            StatusLabel.Name = "_StatusLabel";
            StatusLabel.Overflow = ToolStripItemOverflow.Never;
            StatusLabel.Size = new System.Drawing.Size(663, 20);
            StatusLabel.Spring = true;
            StatusLabel.Text = "Loading...";
            StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // _statusStrip
            //
            StatusStrip.Items.AddRange(new ToolStripItem[] { StatusLabel, ProgressBar });
            StatusStrip.Location = new System.Drawing.Point(0, 615);
            StatusStrip.Name = "_StatusStrip";
            StatusStrip.Padding = new Padding(1, 0, 16, 0);
            StatusStrip.Size = new System.Drawing.Size(780, 25);
            StatusStrip.TabIndex = 7;
            StatusStrip.Text = "StatusStrip1";
            //
            // _progressBar
            //
            ProgressBar.Maximum = 100;
            ProgressBar.Name = "_ProgressBar";
            ProgressBar.Size = new System.Drawing.Size(100, 23);
            ProgressBar.Text = "0 %";
            ProgressBar.Value = 0;
            //
            // _layout
            //
            _layout.ColumnCount = 3;
            _layout.ColumnStyles.Add(new ColumnStyle());
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            _layout.ColumnStyles.Add(new ColumnStyle());
            _layout.Controls.Add(_treePanel, 1, 1);
            _layout.Location = new System.Drawing.Point(27, 167);
            _layout.Name = "_Layout";
            _layout.RowCount = 3;
            _layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 3.0f));
            _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            _layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 3.0f));
            _layout.Size = new System.Drawing.Size(341, 217);
            _layout.TabIndex = 13;
            //
            // _traceMessagesBox
            //
            TraceMessagesBox.BackColor = System.Drawing.SystemColors.Info;
            TraceMessagesBox.CausesValidation = false;
            TraceMessagesBox.Font = new System.Drawing.Font("Consolas", 8.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TraceMessagesBox.Location = new System.Drawing.Point(113, 52);
            TraceMessagesBox.Multiline = true;
            TraceMessagesBox.Name = "_TraceMessagesBox";
            TraceMessagesBox.ReadOnly = true;
            TraceMessagesBox.ScrollBars = ScrollBars.Both;
            TraceMessagesBox.Size = new System.Drawing.Size(601, 471);
            TraceMessagesBox.TabIndex = 15;
            //
            // _treePanel
            //
            _treePanel.Dock = DockStyle.Fill;
            _treePanel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _treePanel.Location = new System.Drawing.Point(3, 6);
            _treePanel.Name = "_TreePanel";
            _treePanel.Size = new System.Drawing.Size(335, 205);
            _treePanel.SplitterDistance = 111;
            _treePanel.TabIndex = 0;
            //
            // SplitVisaView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(TraceMessagesBox);
            Controls.Add(_layout);
            Controls.Add(StatusStrip);
            Name = "SplitVisaView";
            Size = new System.Drawing.Size(780, 640);
            StatusStrip.ResumeLayout(false);
            StatusStrip.PerformLayout();
            _layout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_treePanel).EndInit();
            _treePanel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private TableLayoutPanel _layout;
        private cc.isr.WinControls.TreePanel _treePanel;
    }
}
