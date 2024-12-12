using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NI.SimpleReadWrite
{
    public partial class SelectResources : Form
    {
        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this._resourceStringsListBoxLabel = new System.Windows.Forms.Label();
            this._availableResourcesListBoxLabel = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.AvailableResourceNamesListBox = new System.Windows.Forms.ListBox();
            this.SelectedResourceNamesListBox = new System.Windows.Forms.ListBox();
            this.AddResourceNameButton = new System.Windows.Forms.Button();
            this.RemoveResourceNameButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _resourceStringsListBoxLabel
            // 
            this._resourceStringsListBoxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._resourceStringsListBoxLabel.AutoSize = true;
            this._resourceStringsListBoxLabel.Location = new System.Drawing.Point(1, 188);
            this._resourceStringsListBoxLabel.Name = "_ResourceStringsListBoxLabel";
            this._resourceStringsListBoxLabel.Size = new System.Drawing.Size(97, 15);
            this._resourceStringsListBoxLabel.TabIndex = 12;
            this._resourceStringsListBoxLabel.Text = "Resource Strings:";
            // 
            // _availableResourcesListBoxLabel
            // 
            this._availableResourcesListBoxLabel.AutoSize = true;
            this._availableResourcesListBoxLabel.Location = new System.Drawing.Point(1, 5);
            this._availableResourcesListBoxLabel.Name = "_availableResourcesListBoxLabel";
            this._availableResourcesListBoxLabel.Size = new System.Drawing.Size(114, 15);
            this._availableResourcesListBoxLabel.TabIndex = 11;
            this._availableResourcesListBoxLabel.Text = "Available Resources:";
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(248, 308);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(77, 29);
            this.CloseButton.TabIndex = 9;
            this.CloseButton.Text = "Cancel";
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(167, 308);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(77, 29);
            this.OkButton.TabIndex = 8;
            this.OkButton.Text = "OK";
            // 
            // _availableResourcesListBox
            // 
            this.AvailableResourceNamesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AvailableResourceNamesListBox.ItemHeight = 15;
            this.AvailableResourceNamesListBox.Location = new System.Drawing.Point(5, 20);
            this.AvailableResourceNamesListBox.Name = "_availableResourcesListBox";
            this.AvailableResourceNamesListBox.Size = new System.Drawing.Size(324, 154);
            this.AvailableResourceNamesListBox.TabIndex = 7;
            this.AvailableResourceNamesListBox.DoubleClick += new System.EventHandler(this.AddResourceNameButton_Click);
            // 
            // SelectedResourceNamesListBox
            // 
            this.SelectedResourceNamesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedResourceNamesListBox.FormattingEnabled = true;
            this.SelectedResourceNamesListBox.ItemHeight = 15;
            this.SelectedResourceNamesListBox.Location = new System.Drawing.Point(5, 207);
            this.SelectedResourceNamesListBox.Name = "SelectedResourceNamesListBox";
            this.SelectedResourceNamesListBox.Size = new System.Drawing.Size(324, 94);
            this.SelectedResourceNamesListBox.TabIndex = 13;
            this.SelectedResourceNamesListBox.DoubleClick += new System.EventHandler(this.RemoveResourceNameButton_Click);
            // 
            // AddResourceNameButton
            // 
            this.AddResourceNameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddResourceNameButton.Location = new System.Drawing.Point(5, 308);
            this.AddResourceNameButton.Name = "AddResourceNameButton";
            this.AddResourceNameButton.Size = new System.Drawing.Size(55, 29);
            this.AddResourceNameButton.TabIndex = 14;
            this.AddResourceNameButton.Text = "Add";
            this.AddResourceNameButton.UseVisualStyleBackColor = true;
            this.AddResourceNameButton.Click += new System.EventHandler(this.AddResourceNameButton_Click);
            // 
            // RemoveResourceNameButton
            // 
            this.RemoveResourceNameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveResourceNameButton.Location = new System.Drawing.Point(64, 308);
            this.RemoveResourceNameButton.Name = "RemoveResourceNameButton";
            this.RemoveResourceNameButton.Size = new System.Drawing.Size(71, 29);
            this.RemoveResourceNameButton.TabIndex = 15;
            this.RemoveResourceNameButton.Text = "Remove";
            this.RemoveResourceNameButton.UseVisualStyleBackColor = true;
            this.RemoveResourceNameButton.Click += new System.EventHandler(this.RemoveResourceNameButton_Click);
            // 
            // SelectResources
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 343);
            this.Controls.Add(this.RemoveResourceNameButton);
            this.Controls.Add(this.AddResourceNameButton);
            this.Controls.Add(this.SelectedResourceNamesListBox);
            this.Controls.Add(this._resourceStringsListBoxLabel);
            this.Controls.Add(this._availableResourcesListBoxLabel);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.AvailableResourceNamesListBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SelectResources";
            this.Text = "Select Resources";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label _resourceStringsListBoxLabel;
        private Label _availableResourcesListBoxLabel;
        private Button CloseButton;
        private Button OkButton;
        private ListBox AvailableResourceNamesListBox;
        private ListBox SelectedResourceNamesListBox;
        private Button AddResourceNameButton;
        private Button RemoveResourceNameButton;

    }
}
