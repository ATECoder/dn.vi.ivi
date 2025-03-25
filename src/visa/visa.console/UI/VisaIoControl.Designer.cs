using cc.isr.Visa.WinControls;

namespace cc.isr.Visa.Console.UI
{
    partial class VisaIoControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing )
            {
                components?.Dispose();
            }
            base.Dispose( disposing );
        }

        #region " component designer generated code "

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TreePanel = new cc.isr.Visa.WinControls.TreePanel();
            ((System.ComponentModel.ISupportInitialize)(this.TreePanel)).BeginInit();
            this.TreePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TreePanel
            // 
            this.TreePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreePanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TreePanel.Location = new System.Drawing.Point(0, 0);
            this.TreePanel.MinTreeSize = 25;
            this.TreePanel.Name = "TreePanel";
            this.TreePanel.Size = new System.Drawing.Size(534, 410);
            this.TreePanel.SplitterDistance = 178;
            this.TreePanel.TabIndex = 0;
            // 
            // VisaIoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TreePanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VisaIoControl";
            this.Size = new System.Drawing.Size(534, 410);
            this.Load += new System.EventHandler(this.VisaIoControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TreePanel)).EndInit();
            this.TreePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TreePanel TreePanel;
    }
}
