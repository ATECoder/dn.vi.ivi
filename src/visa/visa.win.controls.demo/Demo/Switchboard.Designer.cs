
namespace cc.isr.Visa.WinControls.Demo;

partial class Switchboard
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

    #region " windows form designer generated code "

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.AvailableForms = new System.Windows.Forms.ListBox();
        this.OpenFormButton = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // AvailableForms
        // 
        this.AvailableForms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.AvailableForms.FormattingEnabled = true;
        this.AvailableForms.ItemHeight = 15;
        this.AvailableForms.Location = new System.Drawing.Point(14, 14);
        this.AvailableForms.Name = "AvailableForms";
        this.AvailableForms.Size = new System.Drawing.Size(422, 109);
        this.AvailableForms.TabIndex = 0;
        // 
        // OpenFormButton
        // 
        this.OpenFormButton.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.OpenFormButton.Location = new System.Drawing.Point(14, 130);
        this.OpenFormButton.Name = "OpenFormButton";
        this.OpenFormButton.Size = new System.Drawing.Size(422, 30);
        this.OpenFormButton.TabIndex = 1;
        this.OpenFormButton.Text = "Open Selected Form";
        this.OpenFormButton.UseVisualStyleBackColor = true;
        this.OpenFormButton.Click += new System.EventHandler(this.OpenFormButton_Click);
        // 
        // Switchboard
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(450, 166);
        this.Controls.Add(this.OpenFormButton);
        this.Controls.Add(this.AvailableForms);
        this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Name = "Switchboard";
        this.Text = "Switchboard";
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox AvailableForms;
    private System.Windows.Forms.Button OpenFormButton;
}
