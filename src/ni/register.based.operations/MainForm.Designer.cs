using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivi.Visa;

namespace NI.RegisterBasedOperations;

public partial class MainForm : Form
{

    #region " windows form designer generated code "
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new( typeof( MainForm ) );
        this._resourceNameLabel = new Label();
        this._offsetNumericUpDown = new NumericUpDown();
        this._numElementsNumericUpDown = new NumericUpDown();
        this._offsetLabel = new Label();
        this._numElementsLabel = new Label();
        this._moveInButton = new Button();
        this._inButton = new Button();
        this._resultTextBox = new TextBox();
        this._resultLabel = new Label();
        this._clearButton = new Button();
        this._spaceComboBox = new ComboBox();
        this._spaceLabel = new Label();
        this._widthLabel = new Label();
        this._widthComboBox = new ComboBox();
        this._resourceNameComboBox = new ComboBox();
        this._closeButton = new Button();
        this._openButton = new Button();
        (( System.ComponentModel.ISupportInitialize ) (this._offsetNumericUpDown)).BeginInit();
        (( System.ComponentModel.ISupportInitialize ) (this._numElementsNumericUpDown)).BeginInit();
        this.SuspendLayout();
        //
        // resourceNameLabel
        //
        this._resourceNameLabel.Location = new System.Drawing.Point( 20, 8 );
        this._resourceNameLabel.Name = "resourceNameLabel";
        this._resourceNameLabel.Size = new System.Drawing.Size( 96, 16 );
        this._resourceNameLabel.TabIndex = 0;
        this._resourceNameLabel.Text = "Resource Name:";
        //
        // offsetNumericUpDown
        //
        this._offsetNumericUpDown.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._offsetNumericUpDown.Hexadecimal = true;
        this._offsetNumericUpDown.Location = new System.Drawing.Point( 20, 169 );
        this._offsetNumericUpDown.Maximum = new decimal( new int[] {
        65535,
        0,
        0,
        0} );
        this._offsetNumericUpDown.Name = "offsetNumericUpDown";
        this._offsetNumericUpDown.Size = new System.Drawing.Size( 137, 20 );
        this._offsetNumericUpDown.TabIndex = 8;
        //
        // numElementsNumericUpDown
        //
        this._numElementsNumericUpDown.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._numElementsNumericUpDown.Location = new System.Drawing.Point( 174, 169 );
        this._numElementsNumericUpDown.Name = "numElementsNumericUpDown";
        this._numElementsNumericUpDown.Size = new System.Drawing.Size( 137, 20 );
        this._numElementsNumericUpDown.TabIndex = 8;
        this._numElementsNumericUpDown.Value = new decimal( new int[] {
        1,
        0,
        0,
        0} );
        //
        // offsetLabel
        //
        this._offsetLabel.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._offsetLabel.Location = new System.Drawing.Point( 21, 150 );
        this._offsetLabel.Name = "offsetLabel";
        this._offsetLabel.Size = new System.Drawing.Size( 72, 16 );
        this._offsetLabel.TabIndex = 9;
        this._offsetLabel.Text = "Offset:";
        //
        // numElementsLabel
        //
        this._numElementsLabel.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._numElementsLabel.Location = new System.Drawing.Point( 171, 150 );
        this._numElementsLabel.Name = "numElementsLabel";
        this._numElementsLabel.Size = new System.Drawing.Size( 112, 16 );
        this._numElementsLabel.TabIndex = 10;
        this._numElementsLabel.Text = "Number of Elements:";
        //
        // moveInButton
        //
        this._moveInButton.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._moveInButton.Location = new System.Drawing.Point( 172, 219 );
        this._moveInButton.Name = "moveInButton";
        this._moveInButton.Size = new System.Drawing.Size( 139, 24 );
        this._moveInButton.TabIndex = 11;
        this._moveInButton.Text = "Move In";
        this._moveInButton.Click += new EventHandler( this.MoveInButton_Click );
        //
        // inButton
        //
        this._inButton.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._inButton.Location = new System.Drawing.Point( 20, 219 );
        this._inButton.Name = "inButton";
        this._inButton.Size = new System.Drawing.Size( 128, 24 );
        this._inButton.TabIndex = 13;
        this._inButton.Text = "In";
        this._inButton.Click += new EventHandler( this.InButton_Click );
        //
        // resultTextBox
        //
        this._resultTextBox.AcceptsReturn = true;
        this._resultTextBox.Anchor = (( AnchorStyles ) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this._resultTextBox.Location = new System.Drawing.Point( 23, 276 );
        this._resultTextBox.Multiline = true;
        this._resultTextBox.Name = "resultTextBox";
        this._resultTextBox.ReadOnly = true;
        this._resultTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this._resultTextBox.Size = new System.Drawing.Size( 292, 242 );
        this._resultTextBox.TabIndex = 17;
        //
        // resultLabel
        //
        this._resultLabel.Location = new System.Drawing.Point( 20, 257 );
        this._resultLabel.Name = "resultLabel";
        this._resultLabel.Size = new System.Drawing.Size( 64, 16 );
        this._resultLabel.TabIndex = 18;
        this._resultLabel.Text = "Result:";
        //
        // clearButton
        //
        this._clearButton.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this._clearButton.Location = new System.Drawing.Point( 235, 522 );
        this._clearButton.Name = "clearButton";
        this._clearButton.Size = new System.Drawing.Size( 80, 24 );
        this._clearButton.TabIndex = 24;
        this._clearButton.Text = "Clear";
        this._clearButton.Click += new EventHandler( this.ClearButton_Click );
        //
        // spaceComboBox
        //
        this._spaceComboBox.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._spaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this._spaceComboBox.Location = new System.Drawing.Point( 20, 121 );
        this._spaceComboBox.Name = "spaceComboBox";
        this._spaceComboBox.Size = new System.Drawing.Size( 137, 21 );
        this._spaceComboBox.TabIndex = 25;
        //
        // spaceLabel
        //
        this._spaceLabel.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._spaceLabel.Location = new System.Drawing.Point( 20, 104 );
        this._spaceLabel.Name = "spaceLabel";
        this._spaceLabel.Size = new System.Drawing.Size( 52, 14 );
        this._spaceLabel.TabIndex = 26;
        this._spaceLabel.Text = "Space:";
        //
        // widthLabel
        //
        this._widthLabel.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._widthLabel.Location = new System.Drawing.Point( 171, 104 );
        this._widthLabel.Name = "widthLabel";
        this._widthLabel.Size = new System.Drawing.Size( 40, 16 );
        this._widthLabel.TabIndex = 27;
        this._widthLabel.Text = "Width:";
        //
        // widthComboBox
        //
        this._widthComboBox.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._widthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this._widthComboBox.Location = new System.Drawing.Point( 174, 121 );
        this._widthComboBox.Name = "widthComboBox";
        this._widthComboBox.Size = new System.Drawing.Size( 137, 21 );
        this._widthComboBox.TabIndex = 28;
        //
        // resourceNameComboBox
        //
        this._resourceNameComboBox.FormattingEnabled = true;
        this._resourceNameComboBox.Location = new System.Drawing.Point( 20, 24 );
        this._resourceNameComboBox.Name = "resourceNameComboBox";
        this._resourceNameComboBox.Size = new System.Drawing.Size( 291, 21 );
        this._resourceNameComboBox.TabIndex = 29;
        //
        // closeButton
        //
        this._closeButton.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._closeButton.Location = new System.Drawing.Point( 173, 55 );
        this._closeButton.Name = "closeButton";
        this._closeButton.Size = new System.Drawing.Size( 137, 24 );
        this._closeButton.TabIndex = 30;
        this._closeButton.Text = "Close Session";
        this._closeButton.Click += new EventHandler( this.CloseButton_Click );
        //
        // openButton
        //
        this._openButton.Anchor = (( AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._openButton.Location = new System.Drawing.Point( 20, 55 );
        this._openButton.Name = "openButton";
        this._openButton.Size = new System.Drawing.Size( 137, 24 );
        this._openButton.TabIndex = 30;
        this._openButton.Text = "Open Session";
        this._openButton.Click += new EventHandler( this.OpenButton_Click );
        //
        // MainForm
        //
        this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
        this.ClientSize = new System.Drawing.Size( 327, 558 );
        this.Controls.Add( this._openButton );
        this.Controls.Add( this._closeButton );
        this.Controls.Add( this._resourceNameComboBox );
        this.Controls.Add( this._widthComboBox );
        this.Controls.Add( this._widthLabel );
        this.Controls.Add( this._spaceLabel );
        this.Controls.Add( this._spaceComboBox );
        this.Controls.Add( this._clearButton );
        this.Controls.Add( this._resultLabel );
        this.Controls.Add( this._resultTextBox );
        this.Controls.Add( this._inButton );
        this.Controls.Add( this._moveInButton );
        this.Controls.Add( this._numElementsLabel );
        this.Controls.Add( this._offsetLabel );
        this.Controls.Add( this._offsetNumericUpDown );
        this.Controls.Add( this._resourceNameLabel );
        this.Controls.Add( this._numElementsNumericUpDown );
        this.Icon = (( System.Drawing.Icon ) (resources.GetObject( "$this.Icon" )));
        this.MaximizeBox = false;
        this.MinimumSize = new System.Drawing.Size( 300, 440 );
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Register-Based Operations";
        (( System.ComponentModel.ISupportInitialize ) (this._offsetNumericUpDown)).EndInit();
        (( System.ComponentModel.ISupportInitialize ) (this._numElementsNumericUpDown)).EndInit();
        this.ResumeLayout( false );
        this.PerformLayout();

    }
    #endregion

    private IRegisterBasedSession _pxiSession;
    private Button _closeButton;
    private Button _openButton;
    private Label _resourceNameLabel;
    private ComboBox _resourceNameComboBox;
    private Label _resultLabel;
    private TextBox _resultTextBox;
    private Button _clearButton;
    private Button _moveInButton;
    private Button _inButton;
    private Label _spaceLabel;
    private Label _offsetLabel;
    private Label _widthLabel;
    private ComboBox _spaceComboBox;
    private NumericUpDown _offsetNumericUpDown;
    private ComboBox _widthComboBox;
    private NumericUpDown _numElementsNumericUpDown;
    private Label _numElementsLabel;
}
