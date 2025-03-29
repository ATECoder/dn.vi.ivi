namespace cc.isr.Visa.ResourceExplorer;

partial class ExplorerForm
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
        this.globalTextBox = new System.Windows.Forms.TextBox();
        this.toolStrip1 = new System.Windows.Forms.ToolStrip();
        this.fileToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
        this.fileExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.implementationsToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
        this.implementationsListAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        this.statusToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
        this.toolStrip1.SuspendLayout();
        this.statusStrip1.SuspendLayout();
        this.SuspendLayout();
        //
        // globalTextBox
        //
        this.globalTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
        this.globalTextBox.Location = new System.Drawing.Point(0, 25);
        this.globalTextBox.Multiline = true;
        this.globalTextBox.Name = "globalTextBox";
        this.globalTextBox.Size = new System.Drawing.Size(686, 343);
        this.globalTextBox.TabIndex = 0;
        //
        // toolStrip1
        //
        this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.fileToolStripSplitButton,
        this.implementationsToolStripSplitButton});
        this.toolStrip1.Location = new System.Drawing.Point(0, 0);
        this.toolStrip1.Name = "toolStrip1";
        this.toolStrip1.Size = new System.Drawing.Size(686, 25);
        this.toolStrip1.TabIndex = 1;
        this.toolStrip1.Text = "toolStrip1";
        //
        // fileToolStripSplitButton
        //
        this.fileToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.fileToolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.fileExitMenuItem});
        this.fileToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.fileToolStripSplitButton.Name = "fileToolStripSplitButton";
        this.fileToolStripSplitButton.Size = new System.Drawing.Size(41, 22);
        this.fileToolStripSplitButton.Text = "&File";
        this.fileToolStripSplitButton.ToolTipText = "File split button";
        //
        // fileExitMenuItem
        //
        this.fileExitMenuItem.Name = "fileExitMenuItem";
        this.fileExitMenuItem.Size = new System.Drawing.Size(180, 22);
        this.fileExitMenuItem.Text = "&Exit";
        this.fileExitMenuItem.ToolTipText = "Close ";
        this.fileExitMenuItem.Click += new System.EventHandler(this.FileExitMenuItem_Click);
        //
        // implementationsToolStripSplitButton
        //
        this.implementationsToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.implementationsToolStripSplitButton.DoubleClickEnabled = true;
        this.implementationsToolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.implementationsListAllMenuItem});
        this.implementationsToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.implementationsToolStripSplitButton.Name = "implementationsToolStripSplitButton";
        this.implementationsToolStripSplitButton.Size = new System.Drawing.Size(113, 22);
        this.implementationsToolStripSplitButton.Text = "&Implementations";
        this.implementationsToolStripSplitButton.ToolTipText = "Implementations menu";
        //
        // implementationsListAllMenuItem
        //
        this.implementationsListAllMenuItem.Name = "implementationsListAllMenuItem";
        this.implementationsListAllMenuItem.Size = new System.Drawing.Size(180, 22);
        this.implementationsListAllMenuItem.Text = "List &All";
        this.implementationsListAllMenuItem.ToolTipText = "Lists all implementations";
        this.implementationsListAllMenuItem.Click += new System.EventHandler(this.ImplementationsListAllMenuItem_Click);
        //
        // statusStrip1
        //
        this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.statusToolStripStatusLabel});
        this.statusStrip1.Location = new System.Drawing.Point(0, 368);
        this.statusStrip1.Name = "statusStrip1";
        this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
        this.statusStrip1.Size = new System.Drawing.Size(686, 22);
        this.statusStrip1.TabIndex = 2;
        this.statusStrip1.Text = "statusStrip1";
        //
        // statusToolStripStatusLabel
        //
        this.statusToolStripStatusLabel.Name = "statusToolStripStatusLabel";
        this.statusToolStripStatusLabel.Size = new System.Drawing.Size(54, 17);
        this.statusToolStripStatusLabel.Text = "<status>";
        //
        // ExplorerForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(686, 390);
        this.Controls.Add(this.globalTextBox);
        this.Controls.Add(this.statusStrip1);
        this.Controls.Add(this.toolStrip1);
        this.Name = "ExplorerForm";
        this.Text = "Form1";
        this.toolStrip1.ResumeLayout(false);
        this.toolStrip1.PerformLayout();
        this.statusStrip1.ResumeLayout(false);
        this.statusStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox globalTextBox;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripSplitButton fileToolStripSplitButton;
    private System.Windows.Forms.ToolStripMenuItem fileExitMenuItem;
    private System.Windows.Forms.ToolStripSplitButton implementationsToolStripSplitButton;
    private System.Windows.Forms.ToolStripMenuItem implementationsListAllMenuItem;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel statusToolStripStatusLabel;
}

