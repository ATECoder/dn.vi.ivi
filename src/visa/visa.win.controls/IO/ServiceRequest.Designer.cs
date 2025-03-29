
namespace cc.isr.Visa.WinControls
{
    partial class ServiceRequest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region " component designer generated code "

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ResourceNameComboBox = new System.Windows.Forms.ComboBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.WritingGroupBox = new System.Windows.Forms.GroupBox();
            this.WriteTextBox = new System.Windows.Forms.TextBox();
            this.WriteButton = new System.Windows.Forms.Button();
            this.ReadTerminationEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.ReadTerminationLabel = new System.Windows.Forms.Label();
            this.OpenButton = new System.Windows.Forms.Button();
            this.EnableSRQButton = new System.Windows.Forms.Button();
            this.CommandTextBox = new System.Windows.Forms.TextBox();
            this.CommandLabel = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.ReadTextBox = new System.Windows.Forms.TextBox();
            this.ResourceNameComboBoxLabel = new System.Windows.Forms.Label();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ReadTerminationCharacterNumeric = new System.Windows.Forms.NumericUpDown();
            this.ReadingGroupBox = new System.Windows.Forms.GroupBox();
            this.ConfiguringGroupBox = new System.Windows.Forms.GroupBox();
            this.SelectResourceLabel = new System.Windows.Forms.Label();
            this.WritingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadTerminationCharacterNumeric)).BeginInit();
            this.ReadingGroupBox.SuspendLayout();
            this.ConfiguringGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // ResourceNameComboBox
            //
            this.ResourceNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResourceNameComboBox.FormattingEnabled = true;
            this.ResourceNameComboBox.Location = new System.Drawing.Point(100, 17);
            this.ResourceNameComboBox.Name = "ResourceNameComboBox";
            this.ResourceNameComboBox.Size = new System.Drawing.Size(269, 23);
            this.ResourceNameComboBox.TabIndex = 4;
            this.ToolTip?.SetToolTip(this.ResourceNameComboBox, "Enter or select a resource name from the list.");
            //
            // CloseButton
            //
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(264, 76);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(104, 23);
            this.CloseButton.TabIndex = 3;
            this.CloseButton.Text = "Close Session";
            this.ToolTip?.SetToolTip(this.CloseButton, "Closes the session by releasing the VISA handle to the device");
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            //
            // WritingGroupBox
            //
            this.WritingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WritingGroupBox.Controls.Add(this.WriteTextBox);
            this.WritingGroupBox.Controls.Add(this.WriteButton);
            this.WritingGroupBox.Location = new System.Drawing.Point(3, 237);
            this.WritingGroupBox.Name = "WritingGroupBox";
            this.WritingGroupBox.Size = new System.Drawing.Size(376, 56);
            this.WritingGroupBox.TabIndex = 33;
            this.WritingGroupBox.TabStop = false;
            this.WritingGroupBox.Text = "Writing";
            //
            // WriteTextBox
            //
            this.WriteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WriteTextBox.Location = new System.Drawing.Point(8, 24);
            this.WriteTextBox.Name = "WriteTextBox";
            this.WriteTextBox.Size = new System.Drawing.Size(256, 23);
            this.WriteTextBox.TabIndex = 2;
            this.WriteTextBox.Text = "*IDN?\\n";
            //
            // WriteButton
            //
            this.WriteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WriteButton.Location = new System.Drawing.Point(264, 24);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(104, 23);
            this.WriteButton.TabIndex = 1;
            this.WriteButton.Text = "Write";
            this.ToolTip?.SetToolTip(this.WriteButton, "Click Write to send a query to device. The reply will be read and displayed upon " +
        "receiving the service request.");
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            //
            // ReadTerminationEnabledCheckBox
            //
            this.ReadTerminationEnabledCheckBox.AutoSize = true;
            this.ReadTerminationEnabledCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.ReadTerminationEnabledCheckBox.Checked = true;
            this.ReadTerminationEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ReadTerminationEnabledCheckBox.Location = new System.Drawing.Point(182, 52);
            this.ReadTerminationEnabledCheckBox.Name = "ReadTerminationEnabledCheckBox";
            this.ReadTerminationEnabledCheckBox.Size = new System.Drawing.Size(68, 19);
            this.ReadTerminationEnabledCheckBox.TabIndex = 18;
            this.ReadTerminationEnabledCheckBox.Text = "&Enabled";
            this.ToolTip?.SetToolTip(this.ReadTerminationEnabledCheckBox, "Check to enable read termination.");
            this.ReadTerminationEnabledCheckBox.UseVisualStyleBackColor = false;
            //
            // ReadTerminationLabel
            //
            this.ReadTerminationLabel.AutoSize = true;
            this.ReadTerminationLabel.BackColor = System.Drawing.Color.Transparent;
            this.ReadTerminationLabel.Location = new System.Drawing.Point(8, 52);
            this.ReadTerminationLabel.Name = "ReadTerminationLabel";
            this.ReadTerminationLabel.Size = new System.Drawing.Size(128, 15);
            this.ReadTerminationLabel.TabIndex = 17;
            this.ReadTerminationLabel.Text = "Read Termination Byte:";
            //
            // OpenButton
            //
            this.OpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenButton.Location = new System.Drawing.Point(264, 49);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(104, 23);
            this.OpenButton.TabIndex = 2;
            this.OpenButton.Text = "Open Session";
            this.ToolTip?.SetToolTip(this.OpenButton, "Sets the resource name and attempts to connect to the device.");
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            //
            // EnableSRQButton
            //
            this.EnableSRQButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EnableSRQButton.Location = new System.Drawing.Point(264, 194);
            this.EnableSRQButton.Name = "EnableSRQButton";
            this.EnableSRQButton.Size = new System.Drawing.Size(104, 24);
            this.EnableSRQButton.TabIndex = 30;
            this.EnableSRQButton.Text = "Enable SRQ";
            this.ToolTip?.SetToolTip(this.EnableSRQButton, "Click to send the SRQ enable command to the device.");
            this.EnableSRQButton.Click += new System.EventHandler(this.EnableSRQButton_Click);
            //
            // CommandTextBox
            //
            this.CommandTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandTextBox.Location = new System.Drawing.Point(8, 194);
            this.CommandTextBox.Name = "CommandTextBox";
            this.CommandTextBox.Size = new System.Drawing.Size(256, 23);
            this.CommandTextBox.TabIndex = 29;
            this.CommandTextBox.Text = "*CLS; *ESE 253; *SRE 255\\n";
            this.ToolTip?.SetToolTip(this.CommandTextBox, "This instrument dependent command enables the instrument\'s SRQ event on a Message" +
        " Available (MAV) event.");
            //
            // CommandLabel
            //
            this.CommandLabel.BackColor = System.Drawing.Color.Transparent;
            this.CommandLabel.Location = new System.Drawing.Point(8, 173);
            this.CommandLabel.Name = "CommandLabel";
            this.CommandLabel.Size = new System.Drawing.Size(352, 18);
            this.CommandLabel.TabIndex = 28;
            this.CommandLabel.Text = "Type the command to enable the instrument\'s SRQ event on MAV:";
            this.CommandLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // ClearButton
            //
            this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearButton.Location = new System.Drawing.Point(264, 142);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(104, 23);
            this.ClearButton.TabIndex = 1;
            this.ClearButton.Text = "Clear";
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            //
            // ReadTextBox
            //
            this.ReadTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReadTextBox.Location = new System.Drawing.Point(6, 24);
            this.ReadTextBox.Multiline = true;
            this.ReadTextBox.Name = "ReadTextBox";
            this.ReadTextBox.ReadOnly = true;
            this.ReadTextBox.Size = new System.Drawing.Size(362, 112);
            this.ReadTextBox.TabIndex = 0;
            //
            // ResourceNameComboBoxLabel
            //
            this.ResourceNameComboBoxLabel.AutoSize = true;
            this.ResourceNameComboBoxLabel.BackColor = System.Drawing.Color.Transparent;
            this.ResourceNameComboBoxLabel.Location = new System.Drawing.Point(8, 77);
            this.ResourceNameComboBoxLabel.Name = "ResourceNameComboBoxLabel";
            this.ResourceNameComboBoxLabel.Size = new System.Drawing.Size(93, 15);
            this.ResourceNameComboBoxLabel.TabIndex = 27;
            this.ResourceNameComboBoxLabel.Text = "Resource Name:";
            this.ResourceNameComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // ReadTerminationCharacterNumeric
            //
            this.ReadTerminationCharacterNumeric.Location = new System.Drawing.Point(139, 49);
            this.ReadTerminationCharacterNumeric.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ReadTerminationCharacterNumeric.Name = "ReadTerminationCharacterNumeric";
            this.ReadTerminationCharacterNumeric.Size = new System.Drawing.Size(40, 23);
            this.ReadTerminationCharacterNumeric.TabIndex = 19;
            this.ToolTip?.SetToolTip(this.ReadTerminationCharacterNumeric, "Enter the read termination byte, which signifies teh end of the message sent from" +
        " the device.");
            this.ReadTerminationCharacterNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // ReadingGroupBox
            //
            this.ReadingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReadingGroupBox.Controls.Add(this.ClearButton);
            this.ReadingGroupBox.Controls.Add(this.ReadTextBox);
            this.ReadingGroupBox.Location = new System.Drawing.Point(3, 306);
            this.ReadingGroupBox.Name = "ReadingGroupBox";
            this.ReadingGroupBox.Size = new System.Drawing.Size(376, 176);
            this.ReadingGroupBox.TabIndex = 34;
            this.ReadingGroupBox.TabStop = false;
            this.ReadingGroupBox.Text = "Reading";
            //
            // ConfiguringGroupBox
            //
            this.ConfiguringGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConfiguringGroupBox.Controls.Add(this.ReadTerminationCharacterNumeric);
            this.ConfiguringGroupBox.Controls.Add(this.ReadTerminationEnabledCheckBox);
            this.ConfiguringGroupBox.Controls.Add(this.ReadTerminationLabel);
            this.ConfiguringGroupBox.Controls.Add(this.ResourceNameComboBox);
            this.ConfiguringGroupBox.Controls.Add(this.CloseButton);
            this.ConfiguringGroupBox.Controls.Add(this.OpenButton);
            this.ConfiguringGroupBox.Location = new System.Drawing.Point(3, 56);
            this.ConfiguringGroupBox.Name = "ConfiguringGroupBox";
            this.ConfiguringGroupBox.Size = new System.Drawing.Size(376, 168);
            this.ConfiguringGroupBox.TabIndex = 31;
            this.ConfiguringGroupBox.TabStop = false;
            this.ConfiguringGroupBox.Text = "Configuring";
            //
            // SelectResourceLabel
            //
            this.SelectResourceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectResourceLabel.Location = new System.Drawing.Point(3, 1);
            this.SelectResourceLabel.Name = "SelectResourceLabel";
            this.SelectResourceLabel.Size = new System.Drawing.Size(376, 46);
            this.SelectResourceLabel.TabIndex = 32;
            this.SelectResourceLabel.Text = "Select the Resource Name associated with your device; Press Open Session; Enter t" +
    "he command string that enables SRQ and click the Enable SRQ button.";
            //
            // ServiceRequest
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.WritingGroupBox);
            this.Controls.Add(this.EnableSRQButton);
            this.Controls.Add(this.CommandTextBox);
            this.Controls.Add(this.CommandLabel);
            this.Controls.Add(this.ResourceNameComboBoxLabel);
            this.Controls.Add(this.ReadingGroupBox);
            this.Controls.Add(this.ConfiguringGroupBox);
            this.Controls.Add(this.SelectResourceLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ServiceRequest";
            this.Size = new System.Drawing.Size(382, 482);
            this.WritingGroupBox.ResumeLayout(false);
            this.WritingGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadTerminationCharacterNumeric)).EndInit();
            this.ReadingGroupBox.ResumeLayout(false);
            this.ReadingGroupBox.PerformLayout();
            this.ConfiguringGroupBox.ResumeLayout(false);
            this.ConfiguringGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ResourceNameComboBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.GroupBox WritingGroupBox;
        private System.Windows.Forms.TextBox WriteTextBox;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.CheckBox ReadTerminationEnabledCheckBox;
        private System.Windows.Forms.Label ReadTerminationLabel;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button EnableSRQButton;
        private System.Windows.Forms.TextBox CommandTextBox;
        private System.Windows.Forms.Label CommandLabel;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.TextBox ReadTextBox;
        private System.Windows.Forms.Label ResourceNameComboBoxLabel;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.GroupBox ReadingGroupBox;
        private System.Windows.Forms.GroupBox ConfiguringGroupBox;
        private System.Windows.Forms.NumericUpDown ReadTerminationCharacterNumeric;
        private System.Windows.Forms.Label SelectResourceLabel;
    }
}
