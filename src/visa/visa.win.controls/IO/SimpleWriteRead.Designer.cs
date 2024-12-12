
namespace cc.isr.Visa.WinControls
{
    partial class SimpleWriteRead
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
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.UsingTspCheckBox = new System.Windows.Forms.CheckBox();
            this.ReadStatusByteButton = new System.Windows.Forms.Button();
            this.MultipleResourcesCheckBox = new System.Windows.Forms.CheckBox();
            this.TimeoutNumeric = new System.Windows.Forms.NumericUpDown();
            this.CloseSessionButton = new System.Windows.Forms.Button();
            this.OpenSessionButton = new System.Windows.Forms.Button();
            this.TimeoutNumericLabel = new System.Windows.Forms.Label();
            this.ReadTerminationCharacterNumeric = new System.Windows.Forms.NumericUpDown();
            this.ReadTerminationCharacterNumericLabel = new System.Windows.Forms.Label();
            this.ResourceNamesComboBoxLabel = new System.Windows.Forms.Label();
            this.ResourceNamesComboBox = new System.Windows.Forms.ComboBox();
            this.ReadTextBoxLabel = new System.Windows.Forms.Label();
            this.WriteTextBoxLabel = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.ReadTextBox = new System.Windows.Forms.TextBox();
            this.WriteTextBox = new System.Windows.Forms.TextBox();
            this.ReadButton = new System.Windows.Forms.Button();
            this.WriteButton = new System.Windows.Forms.Button();
            this.QueryButton = new System.Windows.Forms.Button();
            this.ReadTerminationEnabledCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.TimeoutNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadTerminationCharacterNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // UsingTspCheckBox
            // 
            this.UsingTspCheckBox.AutoSize = true;
            this.UsingTspCheckBox.Location = new System.Drawing.Point(286, 6);
            this.UsingTspCheckBox.Name = "UsingTspCheckBox";
            this.UsingTspCheckBox.Size = new System.Drawing.Size(45, 19);
            this.UsingTspCheckBox.TabIndex = 3;
            this.UsingTspCheckBox.Text = "TSP";
            this.ToolTip?.SetToolTip(this.UsingTspCheckBox, "Check if using a TSP instrument");
            this.UsingTspCheckBox.UseVisualStyleBackColor = true;
            // 
            // ReadStatusByteButton
            // 
            this.ReadStatusByteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ReadStatusByteButton.Location = new System.Drawing.Point(113, 146);
            this.ReadStatusByteButton.Name = "ReadStatusByteButton";
            this.ReadStatusByteButton.Size = new System.Drawing.Size(48, 24);
            this.ReadStatusByteButton.TabIndex = 13;
            this.ReadStatusByteButton.Text = "&STB";
            this.ToolTip?.SetToolTip(this.ReadStatusByteButton, "Read the status byte");
            this.ReadStatusByteButton.UseVisualStyleBackColor = true;
            this.ReadStatusByteButton.Click += new System.EventHandler(this.ReadStatusByteButton_Click);
            // 
            // MultipleResourcesCheckBox
            // 
            this.MultipleResourcesCheckBox.AutoSize = true;
            this.MultipleResourcesCheckBox.Location = new System.Drawing.Point(193, 6);
            this.MultipleResourcesCheckBox.Name = "MultipleResourcesCheckBox";
            this.MultipleResourcesCheckBox.Size = new System.Drawing.Size(70, 19);
            this.MultipleResourcesCheckBox.TabIndex = 2;
            this.MultipleResourcesCheckBox.Text = "Multiple";
            this.ToolTip?.SetToolTip(this.MultipleResourcesCheckBox, "Check to open more than one session");
            this.MultipleResourcesCheckBox.UseVisualStyleBackColor = true;
            this.MultipleResourcesCheckBox.CheckedChanged += new System.EventHandler(this.MultipleResourcesCheckBox_CheckedChanged_1);
            // 
            // TimeoutNumeric
            // 
            this.TimeoutNumeric.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeoutNumeric.Location = new System.Drawing.Point(324, 31);
            this.TimeoutNumeric.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.TimeoutNumeric.Name = "TimeoutNumeric";
            this.TimeoutNumeric.Size = new System.Drawing.Size(51, 23);
            this.TimeoutNumeric.TabIndex = 8;
            this.ToolTip?.SetToolTip(this.TimeoutNumeric, "VISA timeout in milliseconds");
            this.TimeoutNumeric.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.TimeoutNumeric.ValueChanged += new System.EventHandler(this.TimeoutNumeric_ValueChanged);
            // 
            // CloseSessionButton
            // 
            this.CloseSessionButton.Location = new System.Drawing.Point(95, 3);
            this.CloseSessionButton.Name = "CloseSessionButton";
            this.CloseSessionButton.Size = new System.Drawing.Size(92, 25);
            this.CloseSessionButton.TabIndex = 1;
            this.CloseSessionButton.Text = "&Close Session";
            this.ToolTip?.SetToolTip(this.CloseSessionButton, "Closes the open session(s)");
            this.CloseSessionButton.Click += new System.EventHandler(this.CloseSessionButton_Click);
            // 
            // OpenSessionButton
            // 
            this.OpenSessionButton.Location = new System.Drawing.Point(3, 3);
            this.OpenSessionButton.Name = "OpenSessionButton";
            this.OpenSessionButton.Size = new System.Drawing.Size(92, 25);
            this.OpenSessionButton.TabIndex = 0;
            this.OpenSessionButton.Text = "&Open Session";
            this.ToolTip?.SetToolTip(this.OpenSessionButton, "Selects a resource name or resource names and opens and VISA session(s)");
            this.OpenSessionButton.Click += new System.EventHandler(this.OpenSessionButton_Click);
            // 
            // TimeoutNumericLabel
            // 
            this.TimeoutNumericLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeoutNumericLabel.AutoSize = true;
            this.TimeoutNumericLabel.Location = new System.Drawing.Point(272, 35);
            this.TimeoutNumericLabel.Name = "TimeoutNumericLabel";
            this.TimeoutNumericLabel.Size = new System.Drawing.Size(49, 15);
            this.TimeoutNumericLabel.TabIndex = 7;
            this.TimeoutNumericLabel.Text = "T/O ms:";
            // 
            // ReadTerminationCharacterNumeric
            // 
            this.ReadTerminationCharacterNumeric.Location = new System.Drawing.Point(134, 31);
            this.ReadTerminationCharacterNumeric.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ReadTerminationCharacterNumeric.Name = "ReadTerminationCharacterNumeric";
            this.ReadTerminationCharacterNumeric.Size = new System.Drawing.Size(40, 23);
            this.ReadTerminationCharacterNumeric.TabIndex = 5;
            this.ReadTerminationCharacterNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ReadTerminationCharacterNumeric.ValueChanged += new System.EventHandler(this.ReadTerminationCharacterNumeric_ValueChanged);
            // 
            // ReadTerminationCharacterNumericLabel
            // 
            this.ReadTerminationCharacterNumericLabel.AutoSize = true;
            this.ReadTerminationCharacterNumericLabel.Location = new System.Drawing.Point(3, 35);
            this.ReadTerminationCharacterNumericLabel.Name = "ReadTerminationCharacterNumericLabel";
            this.ReadTerminationCharacterNumericLabel.Size = new System.Drawing.Size(128, 15);
            this.ReadTerminationCharacterNumericLabel.TabIndex = 4;
            this.ReadTerminationCharacterNumericLabel.Text = "Read Termination Byte:";
            // 
            // ResourceNamesComboBoxLabel
            // 
            this.ResourceNamesComboBoxLabel.AutoSize = true;
            this.ResourceNamesComboBoxLabel.Location = new System.Drawing.Point(3, 55);
            this.ResourceNamesComboBoxLabel.Name = "ResourceNamesComboBoxLabel";
            this.ResourceNamesComboBoxLabel.Size = new System.Drawing.Size(129, 15);
            this.ResourceNamesComboBoxLabel.TabIndex = 9;
            this.ResourceNamesComboBoxLabel.Text = "Active Resource Name:";
            // 
            // ResourceNamesComboBox
            // 
            this.ResourceNamesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResourceNamesComboBox.FormattingEnabled = true;
            this.ResourceNamesComboBox.Location = new System.Drawing.Point(3, 73);
            this.ResourceNamesComboBox.Name = "ResourceNamesComboBox";
            this.ResourceNamesComboBox.Size = new System.Drawing.Size(372, 23);
            this.ResourceNamesComboBox.TabIndex = 10;
            this.ResourceNamesComboBox.SelectedIndexChanged += new System.EventHandler(this.ResourceNamesComboBox_SelectedIndexChanged);
            // 
            // ReadTextBoxLabel
            // 
            this.ReadTextBoxLabel.AutoSize = true;
            this.ReadTextBoxLabel.Location = new System.Drawing.Point(4, 158);
            this.ReadTextBoxLabel.Name = "ReadTextBoxLabel";
            this.ReadTextBoxLabel.Size = new System.Drawing.Size(57, 15);
            this.ReadTextBoxLabel.TabIndex = 18;
            this.ReadTextBoxLabel.Text = "Received:";
            // 
            // WriteTextBoxLabel
            // 
            this.WriteTextBoxLabel.AutoSize = true;
            this.WriteTextBoxLabel.Location = new System.Drawing.Point(3, 100);
            this.WriteTextBoxLabel.Name = "WriteTextBoxLabel";
            this.WriteTextBoxLabel.Size = new System.Drawing.Size(194, 15);
            this.WriteTextBoxLabel.TabIndex = 11;
            this.WriteTextBoxLabel.Text = "Message to write to the instrument:";
            // 
            // ClearButton
            // 
            this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearButton.Location = new System.Drawing.Point(327, 146);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(48, 24);
            this.ClearButton.TabIndex = 17;
            this.ClearButton.Text = "C&lear";
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // ReadTextBox
            // 
            this.ReadTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReadTextBox.Location = new System.Drawing.Point(3, 176);
            this.ReadTextBox.Multiline = true;
            this.ReadTextBox.Name = "ReadTextBox";
            this.ReadTextBox.ReadOnly = true;
            this.ReadTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ReadTextBox.Size = new System.Drawing.Size(372, 219);
            this.ReadTextBox.TabIndex = 1;
            this.ReadTextBox.TabStop = false;
            // 
            // WriteTextBox
            // 
            this.WriteTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WriteTextBox.Location = new System.Drawing.Point(3, 118);
            this.WriteTextBox.Name = "WriteTextBox";
            this.WriteTextBox.Size = new System.Drawing.Size(372, 23);
            this.WriteTextBox.TabIndex = 12;
            this.WriteTextBox.Text = "*IDN?\\n";
            // 
            // ReadButton
            // 
            this.ReadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ReadButton.Location = new System.Drawing.Point(274, 146);
            this.ReadButton.Name = "ReadButton";
            this.ReadButton.Size = new System.Drawing.Size(48, 24);
            this.ReadButton.TabIndex = 16;
            this.ReadButton.Text = "&Read";
            this.ReadButton.Click += new System.EventHandler(this.ReadButton_Click);
            // 
            // WriteButton
            // 
            this.WriteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WriteButton.Location = new System.Drawing.Point(221, 146);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(48, 24);
            this.WriteButton.TabIndex = 15;
            this.WriteButton.Text = "&Write";
            this.WriteButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // QueryButton
            // 
            this.QueryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.QueryButton.Location = new System.Drawing.Point(166, 146);
            this.QueryButton.Name = "QueryButton";
            this.QueryButton.Size = new System.Drawing.Size(48, 24);
            this.QueryButton.TabIndex = 14;
            this.QueryButton.Text = "&Query";
            this.QueryButton.Click += new System.EventHandler(this.QueryButton_Click);
            // 
            // ReadTerminationEnabledCheckBox
            // 
            this.ReadTerminationEnabledCheckBox.AutoSize = true;
            this.ReadTerminationEnabledCheckBox.Checked = true;
            this.ReadTerminationEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ReadTerminationEnabledCheckBox.Location = new System.Drawing.Point(177, 33);
            this.ReadTerminationEnabledCheckBox.Name = "ReadTerminationEnabledCheckBox";
            this.ReadTerminationEnabledCheckBox.Size = new System.Drawing.Size(68, 19);
            this.ReadTerminationEnabledCheckBox.TabIndex = 6;
            this.ReadTerminationEnabledCheckBox.Text = "&Enabled";
            this.ReadTerminationEnabledCheckBox.UseVisualStyleBackColor = true;
            this.ReadTerminationEnabledCheckBox.CheckedChanged += new System.EventHandler(this.ReadTerminationEnabledCheckBox_CheckedChanged);
            // 
            // SimpleWriteRead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UsingTspCheckBox);
            this.Controls.Add(this.TimeoutNumericLabel);
            this.Controls.Add(this.TimeoutNumeric);
            this.Controls.Add(this.ReadTerminationCharacterNumeric);
            this.Controls.Add(this.ReadTerminationEnabledCheckBox);
            this.Controls.Add(this.ReadTerminationCharacterNumericLabel);
            this.Controls.Add(this.ReadStatusByteButton);
            this.Controls.Add(this.MultipleResourcesCheckBox);
            this.Controls.Add(this.ResourceNamesComboBoxLabel);
            this.Controls.Add(this.ResourceNamesComboBox);
            this.Controls.Add(this.ReadTextBoxLabel);
            this.Controls.Add(this.WriteTextBoxLabel);
            this.Controls.Add(this.CloseSessionButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.ReadTextBox);
            this.Controls.Add(this.WriteTextBox);
            this.Controls.Add(this.OpenSessionButton);
            this.Controls.Add(this.ReadButton);
            this.Controls.Add(this.WriteButton);
            this.Controls.Add(this.QueryButton);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SimpleWriteRead";
            this.Size = new System.Drawing.Size(378, 398);
            ((System.ComponentModel.ISupportInitialize)(this.TimeoutNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadTerminationCharacterNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.CheckBox UsingTspCheckBox;
        private System.Windows.Forms.Button ReadStatusByteButton;
        private System.Windows.Forms.CheckBox MultipleResourcesCheckBox;
        private System.Windows.Forms.Label TimeoutNumericLabel;
        private System.Windows.Forms.NumericUpDown TimeoutNumeric;
        private System.Windows.Forms.NumericUpDown ReadTerminationCharacterNumeric;
        private System.Windows.Forms.Label ReadTerminationCharacterNumericLabel;
        private System.Windows.Forms.Label ResourceNamesComboBoxLabel;
        private System.Windows.Forms.ComboBox ResourceNamesComboBox;
        private System.Windows.Forms.Label ReadTextBoxLabel;
        private System.Windows.Forms.Label WriteTextBoxLabel;
        private System.Windows.Forms.Button CloseSessionButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.TextBox ReadTextBox;
        private System.Windows.Forms.TextBox WriteTextBox;
        private System.Windows.Forms.Button OpenSessionButton;
        private System.Windows.Forms.Button ReadButton;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.Button QueryButton;
        private System.Windows.Forms.CheckBox ReadTerminationEnabledCheckBox;
    }
}
