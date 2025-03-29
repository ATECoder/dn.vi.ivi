
namespace cc.isr.Visa.WinControls
{
    partial class ResourceNameSelector
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
            this.SelectedResourceNameTextBoxLabel = new System.Windows.Forms.Label();
            this.AvailableResourceNamesListBoxLabel = new System.Windows.Forms.Label();
            this.SelectedResourceNameTextBox = new System.Windows.Forms.TextBox();
            this.AvailableResourceNamesListBox = new System.Windows.Forms.ListBox();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.FilterComboBoxLabel = new System.Windows.Forms.Label();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.FilterComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            //
            // SelectedResourceNameTextBoxLabel
            //
            this.SelectedResourceNameTextBoxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectedResourceNameTextBoxLabel.AutoSize = true;
            this.SelectedResourceNameTextBoxLabel.Location = new System.Drawing.Point(3, 244);
            this.SelectedResourceNameTextBoxLabel.Name = "SelectedResourceNameTextBoxLabel";
            this.SelectedResourceNameTextBoxLabel.Size = new System.Drawing.Size(140, 15);
            this.SelectedResourceNameTextBoxLabel.TabIndex = 5;
            this.SelectedResourceNameTextBoxLabel.Text = "Selected Resource Name:";
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
            // SelectedResourceNameTextBox
            //
            this.SelectedResourceNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectedResourceNameTextBox.Location = new System.Drawing.Point(3, 263);
            this.SelectedResourceNameTextBox.Name = "SelectedResourceNameTextBox";
            this.SelectedResourceNameTextBox.Size = new System.Drawing.Size(287, 23);
            this.SelectedResourceNameTextBox.TabIndex = 6;
            this.SelectedResourceNameTextBox.Text = "GPIB0::2::INSTR";
            //
            // AvailableResourceNamesListBox
            //
            this.AvailableResourceNamesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AvailableResourceNamesListBox.ItemHeight = 15;
            this.AvailableResourceNamesListBox.Location = new System.Drawing.Point(3, 22);
            this.AvailableResourceNamesListBox.Name = "AvailableResourceNamesListBox";
            this.AvailableResourceNamesListBox.Size = new System.Drawing.Size(287, 169);
            this.AvailableResourceNamesListBox.TabIndex = 1;
            this.AvailableResourceNamesListBox.SelectedIndexChanged += new System.EventHandler(this.AvailableResourcesListBox_SelectedIndexChanged);
            //
            // FilterComboBoxLabel
            //
            this.FilterComboBoxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FilterComboBoxLabel.AutoSize = true;
            this.FilterComboBoxLabel.Location = new System.Drawing.Point(3, 194);
            this.FilterComboBoxLabel.Name = "FilterComboBoxLabel";
            this.FilterComboBoxLabel.Size = new System.Drawing.Size(122, 15);
            this.FilterComboBoxLabel.TabIndex = 2;
            this.FilterComboBoxLabel.Text = "Resource Name Filter:";
            this.ToolTip?.SetToolTip(this.FilterComboBoxLabel, "Select or enter a filter for searching the resource  from the available resources" +
        "");
            //
            // RefreshButton
            //
            this.RefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshButton.BackColor = System.Drawing.Color.Transparent;
            this.RefreshButton.Image = global::cc.isr.Visa.WinControls.Properties.Resources.refresh;
            this.RefreshButton.Location = new System.Drawing.Point(262, 210);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(28, 28);
            this.RefreshButton.TabIndex = 4;
            this.RefreshButton.UseVisualStyleBackColor = false;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            //
            // FilterComboBox
            //
            this.FilterComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Location = new System.Drawing.Point(3, 213);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(255, 23);
            this.FilterComboBox.TabIndex = 3;
            //
            // ResourceNameSelector
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FilterComboBox);
            this.Controls.Add(this.FilterComboBoxLabel);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.SelectedResourceNameTextBoxLabel);
            this.Controls.Add(this.AvailableResourceNamesListBoxLabel);
            this.Controls.Add(this.SelectedResourceNameTextBox);
            this.Controls.Add(this.AvailableResourceNamesListBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ResourceNameSelector";
            this.Size = new System.Drawing.Size(293, 289);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SelectedResourceNameTextBoxLabel;
        private System.Windows.Forms.Label AvailableResourceNamesListBoxLabel;
        private System.Windows.Forms.TextBox SelectedResourceNameTextBox;
        private System.Windows.Forms.ListBox AvailableResourceNamesListBox;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Label FilterComboBoxLabel;
        private System.Windows.Forms.ComboBox FilterComboBox;
    }
}
