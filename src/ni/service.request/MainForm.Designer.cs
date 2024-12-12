using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NI.ServiceRequest
{
    public partial class MainForm
    {
        #region " windows form designer generated code "
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._resourceNameComboBoxLabel = new System.Windows.Forms.Label();
            this._openButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this._commandLabel = new System.Windows.Forms.Label();
            this._commandTextBox = new System.Windows.Forms.TextBox();
            this._enableSRQButton = new System.Windows.Forms.Button();
            this._configuringGroupBox = new System.Windows.Forms.GroupBox();
            this._readTerminationCharacterNumeric = new System.Windows.Forms.NumericUpDown();
            this._readTerminationEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this._readTerminationLabel = new System.Windows.Forms.Label();
            this._resourceNameComboBox = new System.Windows.Forms.ComboBox();
            this._selectResourceLabel = new System.Windows.Forms.Label();
            this._writingGroupBox = new System.Windows.Forms.GroupBox();
            this._writeTextBox = new System.Windows.Forms.TextBox();
            this._writeButton = new System.Windows.Forms.Button();
            this._readingGroupBox = new System.Windows.Forms.GroupBox();
            this._clearButton = new System.Windows.Forms.Button();
            this._readTextBox = new System.Windows.Forms.TextBox();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this._configuringGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._readTerminationCharacterNumeric)).BeginInit();
            this._writingGroupBox.SuspendLayout();
            this._readingGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _resourceNameComboBoxLabel
            // 
            this._resourceNameComboBoxLabel.AutoSize = true;
            this._resourceNameComboBoxLabel.BackColor = System.Drawing.Color.Transparent;
            this._resourceNameComboBoxLabel.Location = new System.Drawing.Point(16, 82);
            this._resourceNameComboBoxLabel.Name = "_ResourceNameComboBoxLabel";
            this._resourceNameComboBoxLabel.Size = new System.Drawing.Size(93, 15);
            this._resourceNameComboBoxLabel.TabIndex = 1;
            this._resourceNameComboBoxLabel.Text = "Resource Name:";
            this._resourceNameComboBoxLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _openButton
            // 
            this._openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._openButton.Location = new System.Drawing.Point(275, 49);
            this._openButton.Name = "_OpenButton";
            this._openButton.Size = new System.Drawing.Size(104, 23);
            this._openButton.TabIndex = 2;
            this._openButton.Text = "Open Session";
            this._openButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // _closeButton
            // 
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.Location = new System.Drawing.Point(275, 76);
            this._closeButton.Name = "_CloseButton";
            this._closeButton.Size = new System.Drawing.Size(104, 23);
            this._closeButton.TabIndex = 3;
            this._closeButton.Text = "Close Session";
            this._closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // _commandLabel
            // 
            this._commandLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._commandLabel.BackColor = System.Drawing.Color.Transparent;
            this._commandLabel.Location = new System.Drawing.Point(16, 174);
            this._commandLabel.Name = "_CommandLabel";
            this._commandLabel.Size = new System.Drawing.Size(372, 20);
            this._commandLabel.TabIndex = 4;
            this._commandLabel.Text = "Type the command to enable the instrument\'s SRQ event on MAV:";
            this._commandLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _commandTextBox
            // 
            this._commandTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._commandTextBox.Location = new System.Drawing.Point(16, 197);
            this._commandTextBox.Name = "_CommandTextBox";
            this._commandTextBox.Size = new System.Drawing.Size(268, 23);
            this._commandTextBox.TabIndex = 5;
            this._commandTextBox.Text = "*CLS; *ESE 253; *SRE 255\\n";
            // 
            // _enableSRQButton
            // 
            this._enableSRQButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._enableSRQButton.Location = new System.Drawing.Point(284, 197);
            this._enableSRQButton.Name = "_EnableSRQButton";
            this._enableSRQButton.Size = new System.Drawing.Size(104, 23);
            this._enableSRQButton.TabIndex = 6;
            this._enableSRQButton.Text = "Enable SRQ";
            this._enableSRQButton.Click += new System.EventHandler(this.EnableSRQButton_Click);
            // 
            // _configuringGroupBox
            // 
            this._configuringGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._configuringGroupBox.Controls.Add(this._readTerminationCharacterNumeric);
            this._configuringGroupBox.Controls.Add(this._readTerminationEnabledCheckBox);
            this._configuringGroupBox.Controls.Add(this._readTerminationLabel);
            this._configuringGroupBox.Controls.Add(this._resourceNameComboBox);
            this._configuringGroupBox.Controls.Add(this._closeButton);
            this._configuringGroupBox.Controls.Add(this._openButton);
            this._configuringGroupBox.Location = new System.Drawing.Point(8, 61);
            this._configuringGroupBox.Name = "_ConfiguringGroupBox";
            this._configuringGroupBox.Size = new System.Drawing.Size(388, 168);
            this._configuringGroupBox.TabIndex = 7;
            this._configuringGroupBox.TabStop = false;
            this._configuringGroupBox.Text = "Configuring";
            // 
            // _readTerminationCharacterNumeric
            // 
            this._readTerminationCharacterNumeric.Location = new System.Drawing.Point(137, 49);
            this._readTerminationCharacterNumeric.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this._readTerminationCharacterNumeric.Name = "_ReadTerminationCharacterNumeric";
            this._readTerminationCharacterNumeric.Size = new System.Drawing.Size(40, 23);
            this._readTerminationCharacterNumeric.TabIndex = 19;
            this._readTerminationCharacterNumeric.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // _readTerminationEnabledCheckBox
            // 
            this._readTerminationEnabledCheckBox.AutoSize = true;
            this._readTerminationEnabledCheckBox.BackColor = System.Drawing.Color.Transparent;
            this._readTerminationEnabledCheckBox.Checked = true;
            this._readTerminationEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._readTerminationEnabledCheckBox.Location = new System.Drawing.Point(138, 76);
            this._readTerminationEnabledCheckBox.Name = "_ReadTerminationEnabledCheckBox";
            this._readTerminationEnabledCheckBox.Size = new System.Drawing.Size(68, 19);
            this._readTerminationEnabledCheckBox.TabIndex = 18;
            this._readTerminationEnabledCheckBox.Text = "&Enabled";
            this._readTerminationEnabledCheckBox.UseVisualStyleBackColor = false;
            // 
            // _readTerminationLabel
            // 
            this._readTerminationLabel.AutoSize = true;
            this._readTerminationLabel.BackColor = System.Drawing.Color.Transparent;
            this._readTerminationLabel.Location = new System.Drawing.Point(16, 52);
            this._readTerminationLabel.Name = "_ReadTerminationLabel";
            this._readTerminationLabel.Size = new System.Drawing.Size(128, 15);
            this._readTerminationLabel.TabIndex = 17;
            this._readTerminationLabel.Text = "Read Termination Byte:";
            // 
            // _resourceNameComboBox
            // 
            this._resourceNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._resourceNameComboBox.FormattingEnabled = true;
            this._resourceNameComboBox.Location = new System.Drawing.Point(107, 17);
            this._resourceNameComboBox.Name = "_ResourceNameComboBox";
            this._resourceNameComboBox.Size = new System.Drawing.Size(272, 23);
            this._resourceNameComboBox.TabIndex = 4;
            // 
            // _selectResourceLabel
            // 
            this._selectResourceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._selectResourceLabel.Location = new System.Drawing.Point(8, 8);
            this._selectResourceLabel.Name = "_SelectResourceLabel";
            this._selectResourceLabel.Size = new System.Drawing.Size(388, 46);
            this._selectResourceLabel.TabIndex = 8;
            this._selectResourceLabel.Text = "Select the Resource Name associated with your device and press the Configure Devi" +
    "ce button. Then enter the command string that enables SRQ and click the Enable S" +
    "RQ button.";
            // 
            // _writingGroupBox
            // 
            this._writingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._writingGroupBox.Controls.Add(this._writeTextBox);
            this._writingGroupBox.Controls.Add(this._writeButton);
            this._writingGroupBox.Location = new System.Drawing.Point(8, 242);
            this._writingGroupBox.Name = "_WritingGroupBox";
            this._writingGroupBox.Size = new System.Drawing.Size(388, 56);
            this._writingGroupBox.TabIndex = 9;
            this._writingGroupBox.TabStop = false;
            this._writingGroupBox.Text = "Writing";
            // 
            // _writeTextBox
            // 
            this._writeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._writeTextBox.Location = new System.Drawing.Point(8, 24);
            this._writeTextBox.Name = "_WriteTextBox";
            this._writeTextBox.Size = new System.Drawing.Size(268, 23);
            this._writeTextBox.TabIndex = 2;
            this._writeTextBox.Text = "*IDN?\\n";
            // 
            // _writeButton
            // 
            this._writeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._writeButton.Location = new System.Drawing.Point(276, 24);
            this._writeButton.Name = "_WriteButton";
            this._writeButton.Size = new System.Drawing.Size(104, 23);
            this._writeButton.TabIndex = 1;
            this._writeButton.Text = "Write";
            this._writeButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // _readingGroupBox
            // 
            this._readingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._readingGroupBox.Controls.Add(this._clearButton);
            this._readingGroupBox.Controls.Add(this._readTextBox);
            this._readingGroupBox.Location = new System.Drawing.Point(8, 311);
            this._readingGroupBox.Name = "_ReadingGroupBox";
            this._readingGroupBox.Size = new System.Drawing.Size(388, 120);
            this._readingGroupBox.TabIndex = 10;
            this._readingGroupBox.TabStop = false;
            this._readingGroupBox.Text = "Reading";
            // 
            // _clearButton
            // 
            this._clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._clearButton.Location = new System.Drawing.Point(276, 86);
            this._clearButton.Name = "_ClearButton";
            this._clearButton.Size = new System.Drawing.Size(104, 23);
            this._clearButton.TabIndex = 1;
            this._clearButton.Text = "Clear";
            this._clearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // _readTextBox
            // 
            this._readTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._readTextBox.Location = new System.Drawing.Point(8, 24);
            this._readTextBox.Multiline = true;
            this._readTextBox.Name = "_ReadTextBox";
            this._readTextBox.ReadOnly = true;
            this._readTextBox.Size = new System.Drawing.Size(372, 56);
            this._readTextBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(404, 448);
            this.Controls.Add(this._readingGroupBox);
            this.Controls.Add(this._writingGroupBox);
            this.Controls.Add(this._selectResourceLabel);
            this.Controls.Add(this._enableSRQButton);
            this.Controls.Add(this._commandTextBox);
            this.Controls.Add(this._commandLabel);
            this.Controls.Add(this._resourceNameComboBoxLabel);
            this.Controls.Add(this._configuringGroupBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(296, 482);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Service Request";
            this._configuringGroupBox.ResumeLayout(false);
            this._configuringGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._readTerminationCharacterNumeric)).EndInit();
            this._writingGroupBox.ResumeLayout(false);
            this._writingGroupBox.PerformLayout();
            this._readingGroupBox.ResumeLayout(false);
            this._readingGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ToolTip _toolTip;
        private System.Windows.Forms.Label _selectResourceLabel;
        private System.Windows.Forms.GroupBox _configuringGroupBox;
        private System.Windows.Forms.GroupBox _writingGroupBox;
        private System.Windows.Forms.GroupBox _readingGroupBox;
        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Label _resourceNameComboBoxLabel;
        private System.Windows.Forms.Button _openButton;
        private System.Windows.Forms.Button _closeButton;
        private System.Windows.Forms.TextBox _commandTextBox;
        private System.Windows.Forms.Label _commandLabel;
        private System.Windows.Forms.Button _enableSRQButton;
        private System.Windows.Forms.TextBox _writeTextBox;
        private System.Windows.Forms.Button _writeButton;
        private System.Windows.Forms.TextBox _readTextBox;
        private System.Windows.Forms.ComboBox _resourceNameComboBox;
        private System.Windows.Forms.NumericUpDown _readTerminationCharacterNumeric;
        private System.Windows.Forms.CheckBox _readTerminationEnabledCheckBox;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label _readTerminationLabel;

        #endregion
    }
}
