namespace NI.FindResources
{
    public partial class CustomFilterDialog
    {
        private System.Windows.Forms.Label _customFilterLabel;
        private System.Windows.Forms.TextBox _customFilterTextBox;
        private System.Windows.Forms.Button _okayButton;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._customFilterTextBox = new System.Windows.Forms.TextBox();
            this._customFilterLabel = new System.Windows.Forms.Label();
            this._okayButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // customFilterTextBox
            //
            this._customFilterTextBox.Anchor = (( System.Windows.Forms.AnchorStyles ) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._customFilterTextBox.Location = new System.Drawing.Point( 16, 24 );
            this._customFilterTextBox.Name = "customFilterTextBox";
            this._customFilterTextBox.Size = new System.Drawing.Size( 152, 20 );
            this._customFilterTextBox.TabIndex = 0;
            this._customFilterTextBox.Text = "?*";
            //
            // customFilterLabel
            //
            this._customFilterLabel.Location = new System.Drawing.Point( 16, 8 );
            this._customFilterLabel.Name = "customFilterLabel";
            this._customFilterLabel.Size = new System.Drawing.Size( 144, 16 );
            this._customFilterLabel.TabIndex = 1;
            this._customFilterLabel.Text = "Enter Custom Filter String:";
            //
            // okButton
            //
            this._okayButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._okayButton.Location = new System.Drawing.Point( 56, 56 );
            this._okayButton.Name = "okButton";
            this._okayButton.TabIndex = 2;
            this._okayButton.Text = "OK";
            this._okayButton.Click += new System.EventHandler( this.OkayButton_Click );
            //
            // CustomFilterForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.ClientSize = new System.Drawing.Size( 184, 78 );
            this.Controls.Add( this._okayButton );
            this.Controls.Add( this._customFilterLabel );
            this.Controls.Add( this._customFilterTextBox );
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size( 384, 112 );
            this.MinimumSize = new System.Drawing.Size( 192, 112 );
            this.Name = "CustomFilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Filter";
            this.ResumeLayout( false );

        }

    }
}
