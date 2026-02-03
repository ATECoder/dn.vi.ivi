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
            this._resourceNameComboBoxLabel = new Label();
            this._openButton = new Button();
            this._closeButton = new Button();
            this._commandLabel = new Label();
            this._commandTextBox = new TextBox();
            this._enableSRQButton = new Button();
            this._configuringGroupBox = new GroupBox();
            this.serialPollCheckBox = new CheckBox();
            this._readTerminationCharacterNumeric = new NumericUpDown();
            this._readTerminationEnabledCheckBox = new CheckBox();
            this._readTerminationLabel = new Label();
            this._resourceNameComboBox = new ComboBox();
            this._selectResourceLabel = new Label();
            this._writingGroupBox = new GroupBox();
            this._writeTextBox = new TextBox();
            this._writeButton = new Button();
            this._readingGroupBox = new GroupBox();
            this.statusLabelLabel = new Label();
            this.statusLabel = new Label();
            this.readButton = new Button();
            this._clearButton = new Button();
            this._readTextBox = new TextBox();
            this._toolTip = new ToolTip( this.components );
            this.actualEseLabelLabel = new Label();
            this.ActualSreLabelLabel = new Label();
            this.actualEseLabel = new Label();
            this.actualSreLabel = new Label();
            this._configuringGroupBox.SuspendLayout();
            (( System.ComponentModel.ISupportInitialize ) this._readTerminationCharacterNumeric).BeginInit();
            this._writingGroupBox.SuspendLayout();
            this._readingGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _resourceNameComboBoxLabel
            // 
            this._resourceNameComboBoxLabel.AutoSize = true;
            this._resourceNameComboBoxLabel.BackColor = Color.Transparent;
            this._resourceNameComboBoxLabel.Location = new Point( 16, 82 );
            this._resourceNameComboBoxLabel.Name = "_resourceNameComboBoxLabel";
            this._resourceNameComboBoxLabel.Size = new Size( 93, 15 );
            this._resourceNameComboBoxLabel.TabIndex = 1;
            this._resourceNameComboBoxLabel.Text = "Resource Name:";
            this._resourceNameComboBoxLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // _openButton
            // 
            this._openButton.Anchor =    AnchorStyles.Top | AnchorStyles.Right;
            this._openButton.Location = new Point( 277, 49 );
            this._openButton.Name = "_openButton";
            this._openButton.Size = new Size( 104, 23 );
            this._openButton.TabIndex = 2;
            this._openButton.Text = "Open Session";
            this._openButton.Click += this.OpenButton_Click;
            // 
            // _closeButton
            // 
            this._closeButton.Anchor =    AnchorStyles.Top | AnchorStyles.Right;
            this._closeButton.Location = new Point( 277, 76 );
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new Size( 104, 23 );
            this._closeButton.TabIndex = 3;
            this._closeButton.Text = "Close Session";
            this._closeButton.Click += this.CloseButton_Click;
            // 
            // _commandLabel
            // 
            this._commandLabel.Anchor =    AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this._commandLabel.BackColor = Color.Transparent;
            this._commandLabel.Location = new Point( 16, 201 );
            this._commandLabel.Name = "_commandLabel";
            this._commandLabel.Size = new Size( 374, 20 );
            this._commandLabel.TabIndex = 4;
            this._commandLabel.Text = "Type the command to enable the instrument's SRQ event on MAV:";
            this._commandLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _commandTextBox
            // 
            this._commandTextBox.Anchor =    AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this._commandTextBox.Location = new Point( 16, 224 );
            this._commandTextBox.Name = "_commandTextBox";
            this._commandTextBox.Size = new Size( 270, 23 );
            this._commandTextBox.TabIndex = 5;
            this._commandTextBox.Text = "*CLS; *ESE 253; *SRE 255\\n";
            // 
            // _enableSRQButton
            // 
            this._enableSRQButton.Anchor =    AnchorStyles.Top | AnchorStyles.Right;
            this._enableSRQButton.Location = new Point( 286, 224 );
            this._enableSRQButton.Name = "_enableSRQButton";
            this._enableSRQButton.Size = new Size( 104, 23 );
            this._enableSRQButton.TabIndex = 6;
            this._enableSRQButton.Text = "Enable SRQ";
            this._enableSRQButton.Click += this.EnableSRQButton_Click;
            // 
            // _configuringGroupBox
            // 
            this._configuringGroupBox.Anchor =    AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this._configuringGroupBox.Controls.Add( this.actualSreLabel );
            this._configuringGroupBox.Controls.Add( this.actualEseLabel );
            this._configuringGroupBox.Controls.Add( this.ActualSreLabelLabel );
            this._configuringGroupBox.Controls.Add( this.actualEseLabelLabel );
            this._configuringGroupBox.Controls.Add( this.serialPollCheckBox );
            this._configuringGroupBox.Controls.Add( this._readTerminationCharacterNumeric );
            this._configuringGroupBox.Controls.Add( this._readTerminationEnabledCheckBox );
            this._configuringGroupBox.Controls.Add( this._readTerminationLabel );
            this._configuringGroupBox.Controls.Add( this._resourceNameComboBox );
            this._configuringGroupBox.Controls.Add( this._closeButton );
            this._configuringGroupBox.Controls.Add( this._openButton );
            this._configuringGroupBox.Location = new Point( 8, 61 );
            this._configuringGroupBox.Name = "_configuringGroupBox";
            this._configuringGroupBox.Size = new Size( 390, 224 );
            this._configuringGroupBox.TabIndex = 7;
            this._configuringGroupBox.TabStop = false;
            this._configuringGroupBox.Text = "Configuring";
            // 
            // serialPollCheckBox
            // 
            this.serialPollCheckBox.AutoSize = true;
            this.serialPollCheckBox.Location = new Point( 8, 118 );
            this.serialPollCheckBox.Name = "serialPollCheckBox";
            this.serialPollCheckBox.Size = new Size( 77, 19 );
            this.serialPollCheckBox.TabIndex = 20;
            this.serialPollCheckBox.Text = "Serial Poll";
            this.serialPollCheckBox.UseVisualStyleBackColor = true;
            // 
            // _readTerminationCharacterNumeric
            // 
            this._readTerminationCharacterNumeric.Location = new Point( 137, 49 );
            this._readTerminationCharacterNumeric.Maximum = new decimal( new int[] { 255, 0, 0, 0 } );
            this._readTerminationCharacterNumeric.Name = "_readTerminationCharacterNumeric";
            this._readTerminationCharacterNumeric.Size = new Size( 40, 23 );
            this._readTerminationCharacterNumeric.TabIndex = 19;
            this._readTerminationCharacterNumeric.Value = new decimal( new int[] { 10, 0, 0, 0 } );
            // 
            // _readTerminationEnabledCheckBox
            // 
            this._readTerminationEnabledCheckBox.AutoSize = true;
            this._readTerminationEnabledCheckBox.BackColor = Color.Transparent;
            this._readTerminationEnabledCheckBox.Checked = true;
            this._readTerminationEnabledCheckBox.CheckState = CheckState.Checked;
            this._readTerminationEnabledCheckBox.Location = new Point( 138, 76 );
            this._readTerminationEnabledCheckBox.Name = "_readTerminationEnabledCheckBox";
            this._readTerminationEnabledCheckBox.Size = new Size( 68, 19 );
            this._readTerminationEnabledCheckBox.TabIndex = 18;
            this._readTerminationEnabledCheckBox.Text = "&Enabled";
            this._readTerminationEnabledCheckBox.UseVisualStyleBackColor = false;
            // 
            // _readTerminationLabel
            // 
            this._readTerminationLabel.AutoSize = true;
            this._readTerminationLabel.BackColor = Color.Transparent;
            this._readTerminationLabel.Location = new Point( 6, 52 );
            this._readTerminationLabel.Name = "_readTerminationLabel";
            this._readTerminationLabel.Size = new Size( 129, 15 );
            this._readTerminationLabel.TabIndex = 17;
            this._readTerminationLabel.Text = "Read Termination Byte:";
            // 
            // _resourceNameComboBox
            // 
            this._resourceNameComboBox.Anchor =    AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this._resourceNameComboBox.FormattingEnabled = true;
            this._resourceNameComboBox.Location = new Point( 107, 17 );
            this._resourceNameComboBox.Name = "_resourceNameComboBox";
            this._resourceNameComboBox.Size = new Size( 274, 23 );
            this._resourceNameComboBox.TabIndex = 4;
            // 
            // _selectResourceLabel
            // 
            this._selectResourceLabel.Anchor =    AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this._selectResourceLabel.Location = new Point( 8, 8 );
            this._selectResourceLabel.Name = "_selectResourceLabel";
            this._selectResourceLabel.Size = new Size( 390, 46 );
            this._selectResourceLabel.TabIndex = 8;
            this._selectResourceLabel.Text = "Select the Resource Name associated with your device and press the Configure Device button. Then enter the command string that enables SRQ and click the Enable SRQ button.";
            // 
            // _writingGroupBox
            // 
            this._writingGroupBox.Anchor =    AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this._writingGroupBox.Controls.Add( this._writeTextBox );
            this._writingGroupBox.Controls.Add( this._writeButton );
            this._writingGroupBox.Location = new Point( 8, 294 );
            this._writingGroupBox.Name = "_writingGroupBox";
            this._writingGroupBox.Size = new Size( 390, 56 );
            this._writingGroupBox.TabIndex = 9;
            this._writingGroupBox.TabStop = false;
            this._writingGroupBox.Text = "Writing";
            // 
            // _writeTextBox
            // 
            this._writeTextBox.Anchor =    AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this._writeTextBox.Location = new Point( 8, 24 );
            this._writeTextBox.Name = "_writeTextBox";
            this._writeTextBox.Size = new Size( 270, 23 );
            this._writeTextBox.TabIndex = 2;
            this._writeTextBox.Text = "*IDN?\\n";
            // 
            // _writeButton
            // 
            this._writeButton.Anchor =    AnchorStyles.Top | AnchorStyles.Right;
            this._writeButton.Location = new Point( 278, 24 );
            this._writeButton.Name = "_writeButton";
            this._writeButton.Size = new Size( 104, 23 );
            this._writeButton.TabIndex = 1;
            this._writeButton.Text = "Write";
            this._writeButton.Click += this.WriteButton_Click;
            // 
            // _readingGroupBox
            // 
            this._readingGroupBox.Anchor =    AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this._readingGroupBox.Controls.Add( this.statusLabelLabel );
            this._readingGroupBox.Controls.Add( this.statusLabel );
            this._readingGroupBox.Controls.Add( this.readButton );
            this._readingGroupBox.Controls.Add( this._clearButton );
            this._readingGroupBox.Controls.Add( this._readTextBox );
            this._readingGroupBox.Location = new Point( 8, 359 );
            this._readingGroupBox.Name = "_readingGroupBox";
            this._readingGroupBox.Size = new Size( 390, 119 );
            this._readingGroupBox.TabIndex = 10;
            this._readingGroupBox.TabStop = false;
            this._readingGroupBox.Text = "Reading";
            // 
            // statusLabelLabel
            // 
            this.statusLabelLabel.Anchor =    AnchorStyles.Bottom | AnchorStyles.Left;
            this.statusLabelLabel.AutoSize = true;
            this.statusLabelLabel.Location = new Point( 10, 87 );
            this.statusLabelLabel.Name = "statusLabelLabel";
            this.statusLabelLabel.Size = new Size( 45, 15 );
            this.statusLabelLabel.TabIndex = 4;
            this.statusLabelLabel.Text = "Status: ";
            this.statusLabelLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor =    AnchorStyles.Bottom | AnchorStyles.Left;
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new Point( 59, 87 );
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new Size( 30, 15 );
            this.statusLabel.TabIndex = 3;
            this.statusLabel.Text = "0x00";
            // 
            // readButton
            // 
            this.readButton.Anchor =    AnchorStyles.Bottom | AnchorStyles.Right;
            this.readButton.Enabled = false;
            this.readButton.Location = new Point( 197, 85 );
            this.readButton.Name = "readButton";
            this.readButton.Size = new Size( 75, 23 );
            this.readButton.TabIndex = 2;
            this.readButton.Text = "Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += this.ReadButton_Click;
            // 
            // _clearButton
            // 
            this._clearButton.Anchor =    AnchorStyles.Bottom | AnchorStyles.Right;
            this._clearButton.Location = new Point( 278, 85 );
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new Size( 104, 23 );
            this._clearButton.TabIndex = 1;
            this._clearButton.Text = "Clear";
            this._clearButton.Click += this.ClearButton_Click;
            // 
            // _readTextBox
            // 
            this._readTextBox.Anchor =    AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this._readTextBox.Location = new Point( 8, 24 );
            this._readTextBox.Multiline = true;
            this._readTextBox.Name = "_readTextBox";
            this._readTextBox.ReadOnly = true;
            this._readTextBox.Size = new Size( 374, 55 );
            this._readTextBox.TabIndex = 0;
            // 
            // actualEseLabelLabel
            // 
            this.actualEseLabelLabel.AutoSize = true;
            this.actualEseLabelLabel.Location = new Point( 12, 198 );
            this.actualEseLabelLabel.Name = "actualEseLabelLabel";
            this.actualEseLabelLabel.Size = new Size( 65, 15 );
            this.actualEseLabelLabel.TabIndex = 21;
            this.actualEseLabelLabel.Text = "Actual ESE:";
            this.actualEseLabelLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // ActualSreLabelLabel
            // 
            this.ActualSreLabelLabel.AutoSize = true;
            this.ActualSreLabelLabel.Location = new Point( 144, 198 );
            this.ActualSreLabelLabel.Name = "ActualSreLabelLabel";
            this.ActualSreLabelLabel.Size = new Size( 66, 15 );
            this.ActualSreLabelLabel.TabIndex = 22;
            this.ActualSreLabelLabel.Text = "Actual SRE:";
            this.ActualSreLabelLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // actualEseLabel
            // 
            this.actualEseLabel.AutoSize = true;
            this.actualEseLabel.BorderStyle = BorderStyle.Fixed3D;
            this.actualEseLabel.Location = new Point( 82, 197 );
            this.actualEseLabel.Name = "actualEseLabel";
            this.actualEseLabel.Size = new Size( 32, 17 );
            this.actualEseLabel.TabIndex = 23;
            this.actualEseLabel.Text = "0x00";
            // 
            // actualSreLabel
            // 
            this.actualSreLabel.AutoSize = true;
            this.actualSreLabel.BorderStyle = BorderStyle.Fixed3D;
            this.actualSreLabel.Location = new Point( 215, 197 );
            this.actualSreLabel.Name = "actualSreLabel";
            this.actualSreLabel.Size = new Size( 32, 17 );
            this.actualSreLabel.TabIndex = 24;
            this.actualSreLabel.Text = "0x00";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new Size( 6, 16 );
            this.ClientSize = new Size( 406, 490 );
            this.Controls.Add( this._readingGroupBox );
            this.Controls.Add( this._writingGroupBox );
            this.Controls.Add( this._selectResourceLabel );
            this.Controls.Add( this._enableSRQButton );
            this.Controls.Add( this._commandTextBox );
            this.Controls.Add( this._commandLabel );
            this.Controls.Add( this._resourceNameComboBoxLabel );
            this.Controls.Add( this._configuringGroupBox );
            this.Font = new Font( "Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point,    0 );
            this.MaximizeBox = false;
            this.MinimumSize = new Size( 296, 482 );
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Service Request";
            this._configuringGroupBox.ResumeLayout( false );
            this._configuringGroupBox.PerformLayout();
            (( System.ComponentModel.ISupportInitialize ) this._readTerminationCharacterNumeric).EndInit();
            this._writingGroupBox.ResumeLayout( false );
            this._writingGroupBox.PerformLayout();
            this._readingGroupBox.ResumeLayout( false );
            this._readingGroupBox.PerformLayout();
            this.ResumeLayout( false );
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

        private Button readButton;
        private CheckBox serialPollCheckBox;
        private Label statusLabelLabel;
        private Label statusLabel;
        private Label actualEseLabelLabel;
        private Label actualSreLabel;
        private Label actualEseLabel;
        private Label ActualSreLabelLabel;
    }
}
