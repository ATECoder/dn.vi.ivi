using System.Diagnostics;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class EmptyView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            ToolStripPanel1 = new ToolStripPanel();
            SuspendLayout();
            // 
            // ToolStripPanel1
            // 
            ToolStripPanel1.BackColor = System.Drawing.Color.Transparent;
            ToolStripPanel1.Location = new System.Drawing.Point(51, 1);
            ToolStripPanel1.Name = "ToolStripPanel1";
            ToolStripPanel1.Orientation = Orientation.Horizontal;
            ToolStripPanel1.RowMargin = new Padding(3, 0, 0, 0);
            ToolStripPanel1.Size = new System.Drawing.Size(355, 66);
            // 
            // EmptyView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ToolStripPanel1);
            Name = "EmptyView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(445, 148);
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStripPanel ToolStripPanel1;
    }
}
