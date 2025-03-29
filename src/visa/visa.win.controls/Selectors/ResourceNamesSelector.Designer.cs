
namespace cc.isr.Visa.WinControls
{
    partial class ResourceNamesSelector
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

        #region " component designer generated code "

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SelectedResourceNamesListBoxLabel = new System.Windows.Forms.Label();
            this.AvailableResourceNamesListBoxLabel = new System.Windows.Forms.Label();
            this.AvailableResourceNamesListBox = new System.Windows.Forms.ListBox();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.FilterComboBoxLabel = new System.Windows.Forms.Label();
            this.RemoveResourceNameButton = new System.Windows.Forms.Button();
            this.AddResourceNameButton = new System.Windows.Forms.Button();
            this.FilterComboBox = new System.Windows.Forms.ComboBox();
            this.SelectedResourceNamesListBox = new System.Windows.Forms.ListBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // SelectedResourceNamesListBoxLabel
            //
            this.SelectedResourceNamesListBoxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectedResourceNamesListBoxLabel.AutoSize = true;
            this.SelectedResourceNamesListBoxLabel.Location = new System.Drawing.Point(3, 200);
            this.SelectedResourceNamesListBoxLabel.Name = "SelectedResourceNamesListBoxLabel";
            this.SelectedResourceNamesListBoxLabel.Size = new System.Drawing.Size(140, 15);
            this.SelectedResourceNamesListBoxLabel.TabIndex = 5;
            this.SelectedResourceNamesListBoxLabel.Text = "Selected Resource Name:";
            //
            // AvailableResourceNamesListBoxLabel
            //
            this.AvailableResourceNamesListBoxLabel.AutoSize = true;
            this.AvailableResourceNamesListBoxLabel.Location = new System.Drawing.Point(3, 3);
            this.AvailableResourceNamesListBoxLabel.Name = "AvailableResourceNamesListBoxLabel";
            this.AvailableResourceNamesListBoxLabel.Size = new System.Drawing.Size(114, 15);
            this.AvailableResourceNamesListBoxLabel.TabIndex = 0;
            this.AvailableResourceNamesListBoxLabel.Text = "Available Resources:";
            //
            // AvailableResourceNamesListBox
            //
            this.AvailableResourceNamesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AvailableResourceNamesListBox.ItemHeight = 15;
            this.AvailableResourceNamesListBox.Location = new System.Drawing.Point(3, 21);
            this.AvailableResourceNamesListBox.Name = "AvailableResourceNamesListBox";
            this.AvailableResourceNamesListBox.Size = new System.Drawing.Size(304, 124);
            this.AvailableResourceNamesListBox.TabIndex = 1;
            //
            // FilterComboBoxLabel
            //
            this.FilterComboBoxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FilterComboBoxLabel.AutoSize = true;
            this.FilterComboBoxLabel.Location = new System.Drawing.Point(3, 150);
            this.FilterComboBoxLabel.Name = "FilterComboBoxLabel";
            this.FilterComboBoxLabel.Size = new System.Drawing.Size(122, 15);
            this.FilterComboBoxLabel.TabIndex = 2;
            this.FilterComboBoxLabel.Text = "Resource Name Filter:";
            this.ToolTip?.SetToolTip(this.FilterComboBoxLabel, "Select or enter a filter for searching the resource  from the available resources" +
        "");
            //
            // RemoveResourceNameButton
            //
            this.RemoveResourceNameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveResourceNameButton.Image = global::cc.isr.Visa.WinControls.Properties.Resources.Remove_color_16x;
            this.RemoveResourceNameButton.Location = new System.Drawing.Point(285, 316);
            this.RemoveResourceNameButton.Name = "RemoveResourceNameButton";
            this.RemoveResourceNameButton.Size = new System.Drawing.Size(22, 22);
            this.RemoveResourceNameButton.TabIndex = 8;
            this.ToolTip?.SetToolTip(this.RemoveResourceNameButton, "Remove the selected resource name from the list");
            this.RemoveResourceNameButton.UseVisualStyleBackColor = true;
            this.RemoveResourceNameButton.Click += new System.EventHandler(this.RemoveResourceNameButton_Click);
            //
            // AddResourceNameButton
            //
            this.AddResourceNameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddResourceNameButton.Image = global::cc.isr.Visa.WinControls.Properties.Resources.Add_16x;
            this.AddResourceNameButton.Location = new System.Drawing.Point(257, 316);
            this.AddResourceNameButton.Name = "AddResourceNameButton";
            this.AddResourceNameButton.Size = new System.Drawing.Size(22, 22);
            this.AddResourceNameButton.TabIndex = 7;
            this.ToolTip?.SetToolTip(this.AddResourceNameButton, "Add the selected resource name to the list");
            this.AddResourceNameButton.UseVisualStyleBackColor = true;
            this.AddResourceNameButton.Click += new System.EventHandler(this.AddResourceNameButton_Click);
            //
            // FilterComboBox
            //
            this.FilterComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Location = new System.Drawing.Point(3, 169);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(272, 23);
            this.FilterComboBox.TabIndex = 3;
            //
            // SelectedResourceNamesListBox
            //
            this.SelectedResourceNamesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedResourceNamesListBox.FormattingEnabled = true;
            this.SelectedResourceNamesListBox.ItemHeight = 15;
            this.SelectedResourceNamesListBox.Location = new System.Drawing.Point(3, 217);
            this.SelectedResourceNamesListBox.Name = "SelectedResourceNamesListBox";
            this.SelectedResourceNamesListBox.Size = new System.Drawing.Size(304, 94);
            this.SelectedResourceNamesListBox.TabIndex = 6;
            //
            // RefreshButton
            //
            this.RefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshButton.BackColor = System.Drawing.Color.Transparent;
            this.RefreshButton.Image = global::cc.isr.Visa.WinControls.Properties.Resources.refresh;
            this.RefreshButton.Location = new System.Drawing.Point(279, 166);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(28, 28);
            this.RefreshButton.TabIndex = 4;
            this.RefreshButton.UseVisualStyleBackColor = false;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            //
            // ResourceNamesSelector
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RemoveResourceNameButton);
            this.Controls.Add(this.AddResourceNameButton);
            this.Controls.Add(this.SelectedResourceNamesListBox);
            this.Controls.Add(this.FilterComboBox);
            this.Controls.Add(this.FilterComboBoxLabel);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.SelectedResourceNamesListBoxLabel);
            this.Controls.Add(this.AvailableResourceNamesListBoxLabel);
            this.Controls.Add(this.AvailableResourceNamesListBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ResourceNamesSelector";
            this.Size = new System.Drawing.Size(310, 342);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SelectedResourceNamesListBoxLabel;
        private System.Windows.Forms.Label AvailableResourceNamesListBoxLabel;
        private System.Windows.Forms.ListBox AvailableResourceNamesListBox;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Label FilterComboBoxLabel;
        private System.Windows.Forms.ComboBox FilterComboBox;
        private System.Windows.Forms.Button RemoveResourceNameButton;
        private System.Windows.Forms.Button AddResourceNameButton;
        private System.Windows.Forms.ListBox SelectedResourceNamesListBox;
    }
}
