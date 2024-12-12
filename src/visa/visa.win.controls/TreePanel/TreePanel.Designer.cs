using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.Visa.WinControls
{
    public partial class TreePanel
    {
        // [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)this).BeginInit();
            this.navigatorTreeView = new TreeView();
            this.topToolStrip = new System.Windows.Forms.ToolStrip();
            this.pinButton = new System.Windows.Forms.ToolStripButton();
            this.titleLabel = new System.Windows.Forms.ToolStripLabel();
            this.topToolStrip.SuspendLayout();
            Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topToolStrip
            // 
            this.topToolStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.pinButton,
            this.titleLabel} );
            this.topToolStrip.Dock = DockStyle.Top;
            this.topToolStrip.GripStyle =  ToolStripGripStyle.Hidden;
            this.topToolStrip.GripMargin = new Padding( 0, 0, 0, 0 );
            this.topToolStrip.Location = new System.Drawing.Point( 0, 0 );
            this.topToolStrip.Name = "topToolStrip";
            this.topToolStrip.Size = new System.Drawing.Size( 150, 25 );
            this.topToolStrip.TabIndex = 0;
            this.topToolStrip.Text = "Top Tool Strip";
            // 
            // pinButton
            // 
            this.pinButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.pinButton.CheckOnClick = true;
            this.pinButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pinButton.Image = global::cc.isr.Visa.WinControls.Properties.Resources.PinnedItem_16x;
            this.pinButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pinButton.Name = "pinButton";
            this.pinButton.Size = new System.Drawing.Size( 23, 22 );
            this.pinButton.Text = "pin";
            this.pinButton.ToolTipText = "Click to pin";
            // 
            // titleLabel
            // 
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size( 29, 22 );
            this.titleLabel.Text = "Title";
            this.titleLabel.ToolTipText = "Title";
            // 
            // navigatorTreeView
            // 
            this.navigatorTreeView.Dock = DockStyle.Fill;
            this.navigatorTreeView.LineColor = Color.Empty;
            this.navigatorTreeView.Location = new Point(0, 0);
            this.navigatorTreeView.Name = "navigatorTreeView";
            this.navigatorTreeView.Size = new Size(50, 100);
            this.navigatorTreeView.TabIndex = 0;
            // 
            // TreePanel
            // 
            Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Tree Panel";
            Panel1.Controls.Add( topToolStrip );
            Panel1.Controls.Add(navigatorTreeView);
            navigatorTreeView.BringToFront();
            Panel1.ResumeLayout(false);
            this.topToolStrip.ResumeLayout( false );
            this.topToolStrip.PerformLayout();
            this.ResumeLayout( false );
            this.PerformLayout();
            (( System.ComponentModel.ISupportInitialize)this).EndInit();
        }

        private TreeView navigatorTreeView;
        private System.Windows.Forms.ToolStrip topToolStrip;
        private System.Windows.Forms.ToolStripLabel titleLabel;
        private System.Windows.Forms.ToolStripButton pinButton;

    }
}
