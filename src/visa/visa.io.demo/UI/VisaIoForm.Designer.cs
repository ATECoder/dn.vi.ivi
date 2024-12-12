
namespace cc.isr.Visa.IO.Demo.UI;

partial class VisaIoForm
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
        this.VisaIoControl = new VisaIoControl();
        this.SuspendLayout();
        // 
        // VisaIoControl
        // 
        this.VisaIoControl.Dock = System.Windows.Forms.DockStyle.Fill;
        this.VisaIoControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.VisaIoControl.Location = new System.Drawing.Point(0, 0);
        this.VisaIoControl.Name = "VisaIoControl";
        this.VisaIoControl.Size = new System.Drawing.Size(596, 459);
        this.VisaIoControl.TabIndex = 0;
        // 
        // VisaIoForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(596, 459);
        this.Controls.Add(this.VisaIoControl);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.KeyPreview = true;
        this.Name = "VisaIoForm";
        this.Text = "Visa Resource Interactive IO";
        this.ResumeLayout(false);

    }

    #endregion

    private VisaIoControl VisaIoControl;
}

