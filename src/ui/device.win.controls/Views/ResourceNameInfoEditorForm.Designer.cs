using System.Diagnostics;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls
{
    public partial class ResourceNameInfoEditorForm
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
            ResourceNameInfoEditor = new ResourceNameInfoEditor();
            SuspendLayout();
            // 
            // ResourceNameInfoEditor
            // 
            ResourceNameInfoEditor.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ResourceNameInfoEditor.Dock = DockStyle.Fill;
            ResourceNameInfoEditor.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            ResourceNameInfoEditor.Location = new System.Drawing.Point(0, 0);
            ResourceNameInfoEditor.Name = "ResourceNameInfoEditor";
            ResourceNameInfoEditor.Size = new System.Drawing.Size(423, 423);
            ResourceNameInfoEditor.TabIndex = 0;
            // 
            // ResourceNameInfoEditorForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(423, 423);
            Controls.Add(ResourceNameInfoEditor);
            Name = "ResourceNameInfoEditorForm";
            Opacity = 0.999d;
            Text = "ResourceNameInfoEditorForm";
            ResumeLayout(false);
        }

        private ResourceNameInfoEditor ResourceNameInfoEditor;
    }
}
