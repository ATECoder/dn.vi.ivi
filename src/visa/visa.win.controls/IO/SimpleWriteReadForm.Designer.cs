
namespace cc.isr.Visa.WinControls
{
    partial class SimpleWriteReadForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if ( disposing )
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region " windows form designer generated code "

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OkayButton = new System.Windows.Forms.Button();
            this.IgnoreButton = new System.Windows.Forms.Button();
            this.SimpleReadWrite = new cc.isr.Visa.WinControls.SimpleWriteRead();
            this.SuspendLayout();
            // 
            // OkayButton
            // 
            this.OkayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkayButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkayButton.Location = new System.Drawing.Point(220, 311);
            this.OkayButton.Name = "OkayButton";
            this.OkayButton.Size = new System.Drawing.Size(76, 29);
            this.OkayButton.TabIndex = 0;
            this.OkayButton.Text = "&Okay";
            this.OkayButton.UseVisualStyleBackColor = true;
            // 
            // IgnoreButton
            // 
            this.IgnoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.IgnoreButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IgnoreButton.Location = new System.Drawing.Point(301, 311);
            this.IgnoreButton.Name = "IgnoreButton";
            this.IgnoreButton.Size = new System.Drawing.Size(76, 29);
            this.IgnoreButton.TabIndex = 1;
            this.IgnoreButton.Text = "&Cancel";
            this.IgnoreButton.UseVisualStyleBackColor = true;
            // 
            // SimpleReadWrite
            // 
            this.SimpleReadWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SimpleReadWrite.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SimpleReadWrite.Location = new System.Drawing.Point(3, 3);
            this.SimpleReadWrite.Name = "SimpleReadWrite";
            this.SimpleReadWrite.Size = new System.Drawing.Size(377, 301);
            this.SimpleReadWrite.TabIndex = 2;
            // 
            // SimpleWriteReadForm
            // 
            this.AcceptButton = this.OkayButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.IgnoreButton;
            this.ClientSize = new System.Drawing.Size(383, 345);
            this.Controls.Add(this.SimpleReadWrite);
            this.Controls.Add(this.IgnoreButton);
            this.Controls.Add(this.OkayButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "SimpleWriteReadForm";
            this.Text = "Resource Write and Read";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OkayButton;
        private System.Windows.Forms.Button IgnoreButton;
        private SimpleWriteRead SimpleReadWrite;
    }
}

