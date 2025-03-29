
namespace cc.isr.Visa.WinControls
{
    partial class ServiceRequestForm
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
            this.ServiceRequest = new cc.isr.Visa.WinControls.ServiceRequest();
            this.IgnoreButton = new System.Windows.Forms.Button();
            this.OkayButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // ServiceRequest
            //
            this.ServiceRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ServiceRequest.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServiceRequest.Location = new System.Drawing.Point(7, 11);
            this.ServiceRequest.Name = "ServiceRequest";
            this.ServiceRequest.Size = new System.Drawing.Size(377, 439);
            this.ServiceRequest.TabIndex = 5;
            //
            // IgnoreButton
            //
            this.IgnoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.IgnoreButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.IgnoreButton.Location = new System.Drawing.Point(302, 461);
            this.IgnoreButton.Name = "IgnoreButton";
            this.IgnoreButton.Size = new System.Drawing.Size(76, 29);
            this.IgnoreButton.TabIndex = 4;
            this.IgnoreButton.Text = "&Cancel";
            this.IgnoreButton.UseVisualStyleBackColor = true;
            //
            // OkayButton
            //
            this.OkayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkayButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkayButton.Location = new System.Drawing.Point(221, 461);
            this.OkayButton.Name = "OkayButton";
            this.OkayButton.Size = new System.Drawing.Size(76, 29);
            this.OkayButton.TabIndex = 3;
            this.OkayButton.Text = "&Okay";
            this.OkayButton.UseVisualStyleBackColor = true;
            //
            // ServiceRequestForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 501);
            this.Controls.Add(this.ServiceRequest);
            this.Controls.Add(this.IgnoreButton);
            this.Controls.Add(this.OkayButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "ServiceRequestForm";
            this.Text = "Resource Read on Service Request";
            this.ResumeLayout(false);

        }

        #endregion

        private ServiceRequest ServiceRequest;
        private System.Windows.Forms.Button IgnoreButton;
        private System.Windows.Forms.Button OkayButton;
    }
}

