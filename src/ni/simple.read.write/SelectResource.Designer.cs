using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NI.SimpleReadWrite
{
    public partial class SelectResource
    {
        private ListBox AvailableResourcesListBox;
        private Button okButton;
        private Button closeButton;
        private TextBox visaResourceNameTextBox;
        private Label AvailableResourcesLabel;
        private Label ResourceStringLabel;

        #region "windows form designer generated code"

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AvailableResourcesListBox = new System.Windows.Forms.ListBox();
            this.okButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.visaResourceNameTextBox = new System.Windows.Forms.TextBox();
            this.AvailableResourcesLabel = new System.Windows.Forms.Label();
            this.ResourceStringLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AvailableResourcesListBox
            // 
            this.AvailableResourcesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AvailableResourcesListBox.ItemHeight = 15;
            this.AvailableResourcesListBox.Location = new System.Drawing.Point(5, 21);
            this.AvailableResourcesListBox.Name = "AvailableResourcesListBox";
            this.AvailableResourcesListBox.Size = new System.Drawing.Size(282, 94);
            this.AvailableResourcesListBox.TabIndex = 0;
            this.AvailableResourcesListBox.SelectedIndexChanged += new System.EventHandler(this.AvailableResourcesListBox_SelectedIndexChanged);
            this.AvailableResourcesListBox.DoubleClick += new System.EventHandler(this.AvailableResourcesListBox_DoubleClick);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(5, 187);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(77, 25);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(82, 187);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(77, 25);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Cancel";
            // 
            // visaResourceNameTextBox
            // 
            this.visaResourceNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.visaResourceNameTextBox.Location = new System.Drawing.Point(5, 157);
            this.visaResourceNameTextBox.Name = "visaResourceNameTextBox";
            this.visaResourceNameTextBox.Size = new System.Drawing.Size(282, 23);
            this.visaResourceNameTextBox.TabIndex = 4;
            this.visaResourceNameTextBox.Text = "GPIB0::2::INSTR";
            // 
            // AvailableResourcesLabel
            // 
            this.AvailableResourcesLabel.AutoSize = true;
            this.AvailableResourcesLabel.Location = new System.Drawing.Point(5, 5);
            this.AvailableResourcesLabel.Name = "AvailableResourcesLabel";
            this.AvailableResourcesLabel.Size = new System.Drawing.Size(114, 15);
            this.AvailableResourcesLabel.TabIndex = 5;
            this.AvailableResourcesLabel.Text = "Available Resources:";
            // 
            // ResourceStringLabel
            // 
            this.ResourceStringLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ResourceStringLabel.AutoSize = true;
            this.ResourceStringLabel.Location = new System.Drawing.Point(5, 140);
            this.ResourceStringLabel.Name = "ResourceStringLabel";
            this.ResourceStringLabel.Size = new System.Drawing.Size(92, 15);
            this.ResourceStringLabel.TabIndex = 6;
            this.ResourceStringLabel.Text = "Resource String:";
            // 
            // SelectResource
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(292, 220);
            this.Controls.Add(this.ResourceStringLabel);
            this.Controls.Add(this.AvailableResourcesLabel);
            this.Controls.Add(this.visaResourceNameTextBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.AvailableResourcesListBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(177, 247);
            this.Name = "SelectResource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Resource";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


    }
}
