using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NI.SimpleReadWrite
{
    public partial class MainForm
    {
        #region "windows form designer generated code"

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container _components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MainForm ) );
            this.QueryButton = new Button();
            this.WriteButton = new Button();
            this.ReadButton = new Button();
            this.OpenSessionButton = new Button();
            this._writeTextBox = new TextBox();
            this._readTextBox = new TextBox();
            this.ClearButton = new Button();
            this.CloseSessionButton = new Button();
            this._writeTextBoxLabel = new Label();
            this._readTextBoxLabel = new Label();
            this.ResourceNamesComboBox = new ComboBox();
            this.ResourceNamesComboBoxLabel = new Label();
            this.MultipleResourcesCheckBox = new CheckBox();
            this.ReadStatusByte = new Button();
            this.ReadTerminationCharacterNumericLabel = new Label();
            this.ReadTerminationEnabledCheckBox = new CheckBox();
            this.ReadTerminationCharacterNumeric = new NumericUpDown();
            this.TimeoutNumeric = new NumericUpDown();
            this.TimeoutNumericLabel = new Label();
            this.UsingTspCheckBox = new CheckBox();
            this.ToolTip = new ToolTip( this.components );
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.panel2 = new Panel();
            this.panel1 = new Panel();
            (( System.ComponentModel.ISupportInitialize ) this.ReadTerminationCharacterNumeric).BeginInit();
            (( System.ComponentModel.ISupportInitialize ) this.TimeoutNumeric).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // QueryButton
            //
            this.QueryButton.Anchor =    AnchorStyles.Top | AnchorStyles.Right;
            this.QueryButton.Location = new Point( 460, 3 );
            this.QueryButton.Name = "QueryButton";
            this.QueryButton.Size = new Size( 48, 23 );
            this.QueryButton.TabIndex = 3;
            this.QueryButton.Text = "&Query";
            this.QueryButton.Click += this.Query_Click;
            //
            // WriteButton
            //
            this.WriteButton.Anchor =    AnchorStyles.Top | AnchorStyles.Right;
            this.WriteButton.Location = new Point( 515, 3 );
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new Size( 48, 23 );
            this.WriteButton.TabIndex = 4;
            this.WriteButton.Text = "&Write";
            this.WriteButton.Click += this.Write_Click;
            //
            // ReadButton
            //
            this.ReadButton.Anchor =    AnchorStyles.Top | AnchorStyles.Right;
            this.ReadButton.Location = new Point( 568, 3 );
            this.ReadButton.Name = "ReadButton";
            this.ReadButton.Size = new Size( 48, 23 );
            this.ReadButton.TabIndex = 5;
            this.ReadButton.Text = "&Read";
            this.ReadButton.Click += this.Read_Click;
            //
            // OpenSessionButton
            //
            this.OpenSessionButton.Location = new Point( 3, 3 );
            this.OpenSessionButton.Name = "OpenSessionButton";
            this.OpenSessionButton.Size = new Size( 92, 22 );
            this.OpenSessionButton.TabIndex = 0;
            this.OpenSessionButton.Text = "Open Session";
            this.OpenSessionButton.Click += this.OpenSession_Click;
            //
            // _writeTextBox
            //
            this._writeTextBox.Dock = DockStyle.Fill;
            this._writeTextBox.Location = new Point( 8, 124 );
            this._writeTextBox.Multiline = true;
            this._writeTextBox.Name = "_writeTextBox";
            this._writeTextBox.ScrollBars = ScrollBars.Both;
            this._writeTextBox.Size = new Size( 621, 193 );
            this._writeTextBox.TabIndex = 2;
            this._writeTextBox.Text = "*IDN?\\n";
            this._writeTextBox.WordWrap = false;
            //
            // _readTextBox
            //
            this._readTextBox.Dock = DockStyle.Fill;
            this._readTextBox.Location = new Point( 8, 374 );
            this._readTextBox.Multiline = true;
            this._readTextBox.Name = "_readTextBox";
            this._readTextBox.ReadOnly = true;
            this._readTextBox.ScrollBars = ScrollBars.Both;
            this._readTextBox.Size = new Size( 621, 243 );
            this._readTextBox.TabIndex = 6;
            this._readTextBox.TabStop = false;
            this._readTextBox.WordWrap = false;
            //
            // ClearButton
            //
            this.ClearButton.Dock = DockStyle.Bottom;
            this.ClearButton.Location = new Point( 8, 623 );
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new Size( 621, 15 );
            this.ClearButton.TabIndex = 7;
            this.ClearButton.Text = "Clear";
            this.ClearButton.Click += this.Clear_Click;
            //
            // CloseSessionButton
            //
            this.CloseSessionButton.Location = new Point( 95, 3 );
            this.CloseSessionButton.Name = "CloseSessionButton";
            this.CloseSessionButton.Size = new Size( 92, 22 );
            this.CloseSessionButton.TabIndex = 1;
            this.CloseSessionButton.Text = "Close Session";
            this.CloseSessionButton.Click += this.CloseSession_Click;
            //
            // _writeTextBoxLabel
            //
            this._writeTextBoxLabel.AutoSize = true;
            this._writeTextBoxLabel.Dock = DockStyle.Bottom;
            this._writeTextBoxLabel.Location = new Point( 8, 106 );
            this._writeTextBoxLabel.Name = "_writeTextBoxLabel";
            this._writeTextBoxLabel.Size = new Size( 621, 15 );
            this._writeTextBoxLabel.TabIndex = 8;
            this._writeTextBoxLabel.Text = "String to Write:";
            //
            // _readTextBoxLabel
            //
            this._readTextBoxLabel.AutoSize = true;
            this._readTextBoxLabel.Dock = DockStyle.Bottom;
            this._readTextBoxLabel.Location = new Point( 8, 356 );
            this._readTextBoxLabel.Name = "_readTextBoxLabel";
            this._readTextBoxLabel.Size = new Size( 621, 15 );
            this._readTextBoxLabel.TabIndex = 9;
            this._readTextBoxLabel.Text = "Received:";
            //
            // ResourceNamesComboBox
            //
            this.ResourceNamesComboBox.Dock = DockStyle.Bottom;
            this.ResourceNamesComboBox.FormattingEnabled = true;
            this.ResourceNamesComboBox.Location = new Point( 0, 77 );
            this.ResourceNamesComboBox.Name = "ResourceNamesComboBox";
            this.ResourceNamesComboBox.Size = new Size( 621, 23 );
            this.ResourceNamesComboBox.TabIndex = 10;
            this.ResourceNamesComboBox.SelectedIndexChanged += this.ResourcesComboBox_SelectedIndexChanged;
            //
            // ResourceNamesComboBoxLabel
            //
            this.ResourceNamesComboBoxLabel.AutoSize = true;
            this.ResourceNamesComboBoxLabel.Location = new Point( 3, 55 );
            this.ResourceNamesComboBoxLabel.Name = "ResourceNamesComboBoxLabel";
            this.ResourceNamesComboBoxLabel.Size = new Size( 98, 15 );
            this.ResourceNamesComboBoxLabel.TabIndex = 11;
            this.ResourceNamesComboBoxLabel.Text = "Resource Names:";
            //
            // MultipleResourcesCheckBox
            //
            this.MultipleResourcesCheckBox.AutoSize = true;
            this.MultipleResourcesCheckBox.Location = new Point( 193, 6 );
            this.MultipleResourcesCheckBox.Name = "MultipleResourcesCheckBox";
            this.MultipleResourcesCheckBox.Size = new Size( 70, 19 );
            this.MultipleResourcesCheckBox.TabIndex = 12;
            this.MultipleResourcesCheckBox.Text = "Multiple";
            this.MultipleResourcesCheckBox.UseVisualStyleBackColor = true;
            this.MultipleResourcesCheckBox.CheckedChanged += this.MultipleResourcesCheckBox_CheckedChanged;
            //
            // ReadStatusByte
            //
            this.ReadStatusByte.Anchor =    AnchorStyles.Top | AnchorStyles.Right;
            this.ReadStatusByte.Location = new Point( 407, 3 );
            this.ReadStatusByte.Name = "ReadStatusByte";
            this.ReadStatusByte.Size = new Size( 48, 23 );
            this.ReadStatusByte.TabIndex = 13;
            this.ReadStatusByte.Text = "&STB";
            this.ReadStatusByte.UseVisualStyleBackColor = true;
            this.ReadStatusByte.Click += this.ReadStatusByte_Click;
            //
            // ReadTerminationCharacterNumericLabel
            //
            this.ReadTerminationCharacterNumericLabel.AutoSize = true;
            this.ReadTerminationCharacterNumericLabel.Location = new Point( 3, 35 );
            this.ReadTerminationCharacterNumericLabel.Name = "ReadTerminationCharacterNumericLabel";
            this.ReadTerminationCharacterNumericLabel.Size = new Size( 128, 15 );
            this.ReadTerminationCharacterNumericLabel.TabIndex = 14;
            this.ReadTerminationCharacterNumericLabel.Text = "Read Termination Byte:";
            //
            // ReadTerminationEnabledCheckBox
            //
            this.ReadTerminationEnabledCheckBox.AutoSize = true;
            this.ReadTerminationEnabledCheckBox.Checked = true;
            this.ReadTerminationEnabledCheckBox.CheckState = CheckState.Checked;
            this.ReadTerminationEnabledCheckBox.Location = new Point( 177, 33 );
            this.ReadTerminationEnabledCheckBox.Name = "ReadTerminationEnabledCheckBox";
            this.ReadTerminationEnabledCheckBox.Size = new Size( 68, 19 );
            this.ReadTerminationEnabledCheckBox.TabIndex = 15;
            this.ReadTerminationEnabledCheckBox.Text = "&Enabled";
            this.ReadTerminationEnabledCheckBox.UseVisualStyleBackColor = true;
            this.ReadTerminationEnabledCheckBox.CheckedChanged += this.ReadTerminationCharacter_ValueChanged;
            //
            // ReadTerminationCharacterNumeric
            //
            this.ReadTerminationCharacterNumeric.Location = new Point( 134, 31 );
            this.ReadTerminationCharacterNumeric.Maximum = new decimal( new int[] { 255, 0, 0, 0 } );
            this.ReadTerminationCharacterNumeric.Name = "ReadTerminationCharacterNumeric";
            this.ReadTerminationCharacterNumeric.Size = new Size( 40, 23 );
            this.ReadTerminationCharacterNumeric.TabIndex = 16;
            this.ReadTerminationCharacterNumeric.Value = new decimal( new int[] { 10, 0, 0, 0 } );
            this.ReadTerminationCharacterNumeric.ValueChanged += this.ReadTerminationCharacter_ValueChanged;
            //
            // TimeoutNumeric
            //
            this.TimeoutNumeric.Location = new Point( 316, 31 );
            this.TimeoutNumeric.Maximum = new decimal( new int[] { 10000, 0, 0, 0 } );
            this.TimeoutNumeric.Name = "TimeoutNumeric";
            this.TimeoutNumeric.Size = new Size( 51, 23 );
            this.TimeoutNumeric.TabIndex = 17;
            this.TimeoutNumeric.Value = new decimal( new int[] { 1000, 0, 0, 0 } );
            this.TimeoutNumeric.ValueChanged += this.TimeoutNumeric_ValueChanged;
            //
            // TimeoutNumericLabel
            //
            this.TimeoutNumericLabel.AutoSize = true;
            this.TimeoutNumericLabel.Location = new Point( 264, 35 );
            this.TimeoutNumericLabel.Name = "TimeoutNumericLabel";
            this.TimeoutNumericLabel.Size = new Size( 49, 15 );
            this.TimeoutNumericLabel.TabIndex = 18;
            this.TimeoutNumericLabel.Text = "T/O ms:";
            //
            // UsingTspCheckBox
            //
            this.UsingTspCheckBox.AutoSize = true;
            this.UsingTspCheckBox.Location = new Point( 286, 6 );
            this.UsingTspCheckBox.Name = "UsingTspCheckBox";
            this.UsingTspCheckBox.Size = new Size( 45, 19 );
            this.UsingTspCheckBox.TabIndex = 19;
            this.UsingTspCheckBox.Text = "TSP";
            this.UsingTspCheckBox.UseVisualStyleBackColor = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add( new ColumnStyle( SizeType.Absolute, 5F ) );
            this.tableLayoutPanel1.ColumnStyles.Add( new ColumnStyle( SizeType.Percent, 100F ) );
            this.tableLayoutPanel1.ColumnStyles.Add( new ColumnStyle( SizeType.Absolute, 5F ) );
            this.tableLayoutPanel1.Controls.Add( this.panel2, 1, 0 );
            this.tableLayoutPanel1.Controls.Add( this.panel1, 1, 3 );
            this.tableLayoutPanel1.Controls.Add( this.ClearButton, 1, 6 );
            this.tableLayoutPanel1.Controls.Add( this._readTextBox, 1, 5 );
            this.tableLayoutPanel1.Controls.Add( this._readTextBoxLabel, 1, 4 );
            this.tableLayoutPanel1.Controls.Add( this._writeTextBox, 1, 2 );
            this.tableLayoutPanel1.Controls.Add( this._writeTextBoxLabel, 1, 1 );
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point( 0, 0 );
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add( new RowStyle() );
            this.tableLayoutPanel1.RowStyles.Add( new RowStyle() );
            this.tableLayoutPanel1.RowStyles.Add( new RowStyle( SizeType.Percent, 44.4444427F ) );
            this.tableLayoutPanel1.RowStyles.Add( new RowStyle() );
            this.tableLayoutPanel1.RowStyles.Add( new RowStyle() );
            this.tableLayoutPanel1.RowStyles.Add( new RowStyle( SizeType.Percent, 55.5555573F ) );
            this.tableLayoutPanel1.RowStyles.Add( new RowStyle( SizeType.Absolute, 20F ) );
            this.tableLayoutPanel1.Size = new Size( 637, 641 );
            this.tableLayoutPanel1.TabIndex = 20;
            //
            // panel2
            //
            this.panel2.Controls.Add( this.OpenSessionButton );
            this.panel2.Controls.Add( this.CloseSessionButton );
            this.panel2.Controls.Add( this.UsingTspCheckBox );
            this.panel2.Controls.Add( this.ResourceNamesComboBox );
            this.panel2.Controls.Add( this.TimeoutNumericLabel );
            this.panel2.Controls.Add( this.ResourceNamesComboBoxLabel );
            this.panel2.Controls.Add( this.TimeoutNumeric );
            this.panel2.Controls.Add( this.MultipleResourcesCheckBox );
            this.panel2.Controls.Add( this.ReadTerminationCharacterNumeric );
            this.panel2.Controls.Add( this.ReadTerminationCharacterNumericLabel );
            this.panel2.Controls.Add( this.ReadTerminationEnabledCheckBox );
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point( 8, 3 );
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size( 621, 100 );
            this.panel2.TabIndex = 21;
            //
            // panel1
            //
            this.panel1.Controls.Add( this.ReadStatusByte );
            this.panel1.Controls.Add( this.QueryButton );
            this.panel1.Controls.Add( this.WriteButton );
            this.panel1.Controls.Add( this.ReadButton );
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point( 8, 323 );
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size( 621, 30 );
            this.panel1.TabIndex = 21;
            //
            // MainForm
            //
            this.AutoScaleBaseSize = new Size( 6, 16 );
            this.ClientSize = new Size( 637, 641 );
            this.Controls.Add( this.tableLayoutPanel1 );
            this.Font = new Font( "Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point,    0 );
            this.Icon = ( Icon ) resources.GetObject( "$this.Icon" );
            this.MaximizeBox = false;
            this.MinimumSize = new Size( 295, 316 );
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Simple Read/Write";
            (( System.ComponentModel.ISupportInitialize ) this.ReadTerminationCharacterNumeric).EndInit();
            (( System.ComponentModel.ISupportInitialize ) this.TimeoutNumeric).EndInit();
            this.tableLayoutPanel1.ResumeLayout( false );
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout( false );
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout( false );
            this.ResumeLayout( false );
        }

        private Button ReadStatusByte;
        private Label ReadTerminationCharacterNumericLabel;
        private CheckBox ReadTerminationEnabledCheckBox;
        private NumericUpDown ReadTerminationCharacterNumeric;
        private NumericUpDown TimeoutNumeric;
        private Label TimeoutNumericLabel;
        private ToolTip ToolTip;
        private System.ComponentModel.IContainer components;
        private CheckBox UsingTspCheckBox;
        private TextBox _writeTextBox;
        private TextBox _readTextBox;
        private Button QueryButton;
        private Button WriteButton;
        private Button ReadButton;
        private Button OpenSessionButton;
        private Button ClearButton;
        private Button CloseSessionButton;
        private Label _writeTextBoxLabel;
        private Label _readTextBoxLabel;
        private ComboBox ResourceNamesComboBox;
        private Label ResourceNamesComboBoxLabel;
        private CheckBox MultipleResourcesCheckBox;

        #endregion


        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private Panel panel1;
    }
}
