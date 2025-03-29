using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI.SimpleAsynchronousReadWrite
{
    public partial class SelectResourceDialog
    {
        private System.Windows.Forms.ListBox _availableResourcesListBox;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.TextBox _visaResourceNameTextBox;
        private System.Windows.Forms.Label _availableResourcesLabel;
        private System.Windows.Forms.Label _resourceStringLabel;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._availableResourcesListBox = new System.Windows.Forms.ListBox();
            this._okButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._visaResourceNameTextBox = new System.Windows.Forms.TextBox();
            this._availableResourcesLabel = new System.Windows.Forms.Label();
            this._resourceStringLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // availableResourcesListBox
            //
            this._availableResourcesListBox.Anchor = (( System.Windows.Forms.AnchorStyles ) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._availableResourcesListBox.Location = new System.Drawing.Point( 5, 18 );
            this._availableResourcesListBox.Name = "availableResourcesListBox";
            this._availableResourcesListBox.Size = new System.Drawing.Size( 282, 108 );
            this._availableResourcesListBox.TabIndex = 0;
            this._availableResourcesListBox.DoubleClick += new System.EventHandler( this.AvailableResourcesListBox_DoubleClick );
            this._availableResourcesListBox.SelectedIndexChanged += new System.EventHandler( this.AvailableResourcesListBox_SelectedIndexChanged );
            //
            // okButton
            //
            this._okButton.Anchor = (( System.Windows.Forms.AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point( 5, 187 );
            this._okButton.Name = "okButton";
            this._okButton.Size = new System.Drawing.Size( 77, 25 );
            this._okButton.TabIndex = 2;
            this._okButton.Text = "OK";
            //
            // closeButton
            //
            this._closeButton.Anchor = (( System.Windows.Forms.AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._closeButton.Location = new System.Drawing.Point( 82, 187 );
            this._closeButton.Name = "closeButton";
            this._closeButton.Size = new System.Drawing.Size( 77, 25 );
            this._closeButton.TabIndex = 3;
            this._closeButton.Text = "Cancel";
            //
            // visaResourceNameTextBox
            //
            this._visaResourceNameTextBox.Anchor = (( System.Windows.Forms.AnchorStyles ) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._visaResourceNameTextBox.Location = new System.Drawing.Point( 5, 157 );
            this._visaResourceNameTextBox.Name = "visaResourceNameTextBox";
            this._visaResourceNameTextBox.Size = new System.Drawing.Size( 282, 20 );
            this._visaResourceNameTextBox.TabIndex = 4;
            this._visaResourceNameTextBox.Text = "GPIB0::2::INSTR";
            //
            // AvailableResourcesLabel
            //
            this._availableResourcesLabel.Anchor = (( System.Windows.Forms.AnchorStyles ) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._availableResourcesLabel.Location = new System.Drawing.Point( 5, 5 );
            this._availableResourcesLabel.Name = "AvailableResourcesLabel";
            this._availableResourcesLabel.Size = new System.Drawing.Size( 279, 12 );
            this._availableResourcesLabel.TabIndex = 5;
            this._availableResourcesLabel.Text = "Available Resources:";
            //
            // ResourceStringLabel
            //
            this._resourceStringLabel.Anchor = (( System.Windows.Forms.AnchorStyles ) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this._resourceStringLabel.Location = new System.Drawing.Point( 5, 141 );
            this._resourceStringLabel.Name = "ResourceStringLabel";
            this._resourceStringLabel.Size = new System.Drawing.Size( 279, 13 );
            this._resourceStringLabel.TabIndex = 6;
            this._resourceStringLabel.Text = "Resource String:";
            //
            // SelectResource
            //
            this.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.CancelButton = this._closeButton;
            this.ClientSize = new System.Drawing.Size( 292, 220 );
            this.Controls.Add( this._resourceStringLabel );
            this.Controls.Add( this._availableResourcesLabel );
            this.Controls.Add( this._visaResourceNameTextBox );
            this.Controls.Add( this._closeButton );
            this.Controls.Add( this._okButton );
            this.Controls.Add( this._availableResourcesListBox );
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size( 177, 247 );
            this.Name = "SelectResource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Resource";
            this.Load += new System.EventHandler( this.OnLoad );
            this.ResumeLayout( false );

        }

    }
}
