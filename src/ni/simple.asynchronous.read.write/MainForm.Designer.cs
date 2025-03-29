using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI.SimpleAsynchronousReadWrite
{
    public partial class MainForm
    {
        private System.Windows.Forms.TextBox _writeTextBox;
        private System.Windows.Forms.TextBox _readTextBox;
        private System.Windows.Forms.Button _writeButton;
        private System.Windows.Forms.Button _readButton;
        private System.Windows.Forms.Button _openSessionButton;
        private System.Windows.Forms.Button _clearButton;
        private System.Windows.Forms.Button _closeSessionButton;
        private System.Windows.Forms.Label _stringToWriteLabel;
        private System.Windows.Forms.Label _stringToReadLabel;
        private System.Windows.Forms.Button _terminateButton;
        private System.Windows.Forms.Label _elementsTransferredLabel;
        private System.Windows.Forms.TextBox _elementsTransferredTextBox;
        private System.Windows.Forms.TextBox _lastIOStatusTextBox;
        private System.Windows.Forms.Label _lastIOStatusLabel;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._writeButton = new System.Windows.Forms.Button();
            this._readButton = new System.Windows.Forms.Button();
            this._openSessionButton = new System.Windows.Forms.Button();
            this._writeTextBox = new System.Windows.Forms.TextBox();
            this._readTextBox = new System.Windows.Forms.TextBox();
            this._clearButton = new System.Windows.Forms.Button();
            this._closeSessionButton = new System.Windows.Forms.Button();
            this._stringToWriteLabel = new System.Windows.Forms.Label();
            this._stringToReadLabel = new System.Windows.Forms.Label();
            this._terminateButton = new System.Windows.Forms.Button();
            this._elementsTransferredLabel = new System.Windows.Forms.Label();
            this._elementsTransferredTextBox = new System.Windows.Forms.TextBox();
            this._lastIOStatusTextBox = new System.Windows.Forms.TextBox();
            this._lastIOStatusLabel = new System.Windows.Forms.Label();
            this._inLabel = new System.Windows.Forms.Label();
            this._elapsedLabel = new System.Windows.Forms.Label();
            this._msLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // _writeButton
            //
            this._writeButton.Location = new System.Drawing.Point(5, 83);
            this._writeButton.Name = "_WriteButton";
            this._writeButton.Size = new System.Drawing.Size(74, 23);
            this._writeButton.TabIndex = 3;
            this._writeButton.Text = "&Write";
            this._writeButton.Click += new System.EventHandler(this.Write_Click);
            //
            // _readButton
            //
            this._readButton.Location = new System.Drawing.Point(79, 83);
            this._readButton.Name = "_ReadButton";
            this._readButton.Size = new System.Drawing.Size(74, 23);
            this._readButton.TabIndex = 4;
            this._readButton.Text = "&Read";
            this._readButton.Click += new System.EventHandler(this.Read_Click);
            //
            // _openSessionButton
            //
            this._openSessionButton.Location = new System.Drawing.Point(5, 5);
            this._openSessionButton.Name = "_OpenSessionButton";
            this._openSessionButton.Size = new System.Drawing.Size(92, 22);
            this._openSessionButton.TabIndex = 0;
            this._openSessionButton.Text = "&Open Session";
            this._openSessionButton.Click += new System.EventHandler(this.OpenSession_Click);
            //
            // _writeTextBox
            //
            this._writeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._writeTextBox.Location = new System.Drawing.Point(5, 54);
            this._writeTextBox.Name = "_WriteTextBox";
            this._writeTextBox.Size = new System.Drawing.Size(275, 20);
            this._writeTextBox.TabIndex = 2;
            this._writeTextBox.Text = "*IDN?\\n";
            //
            // _readTextBox
            //
            this._readTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._readTextBox.Location = new System.Drawing.Point(5, 136);
            this._readTextBox.Multiline = true;
            this._readTextBox.Name = "_ReadTextBox";
            this._readTextBox.ReadOnly = true;
            this._readTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._readTextBox.Size = new System.Drawing.Size(275, 158);
            this._readTextBox.TabIndex = 6;
            this._readTextBox.TabStop = false;
            //
            // _clearButton
            //
            this._clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._clearButton.Location = new System.Drawing.Point(6, 347);
            this._clearButton.Name = "_ClearButton";
            this._clearButton.Size = new System.Drawing.Size(275, 24);
            this._clearButton.TabIndex = 6;
            this._clearButton.Text = "C&lear";
            this._clearButton.Click += new System.EventHandler(this.Clear_Click);
            //
            // _closeSessionButton
            //
            this._closeSessionButton.Location = new System.Drawing.Point(97, 5);
            this._closeSessionButton.Name = "_CloseSessionButton";
            this._closeSessionButton.Size = new System.Drawing.Size(92, 22);
            this._closeSessionButton.TabIndex = 1;
            this._closeSessionButton.Text = "&Close Session";
            this._closeSessionButton.Click += new System.EventHandler(this.CloseSession_Click);
            //
            // _stringToWriteLabel
            //
            this._stringToWriteLabel.AutoSize = true;
            this._stringToWriteLabel.Location = new System.Drawing.Point(5, 39);
            this._stringToWriteLabel.Name = "_StringToWriteLabel";
            this._stringToWriteLabel.Size = new System.Drawing.Size(93, 13);
            this._stringToWriteLabel.TabIndex = 8;
            this._stringToWriteLabel.Text = "Message to Write:";
            //
            // _stringToReadLabel
            //
            this._stringToReadLabel.AutoSize = true;
            this._stringToReadLabel.Location = new System.Drawing.Point(5, 120);
            this._stringToReadLabel.Name = "_StringToReadLabel";
            this._stringToReadLabel.Size = new System.Drawing.Size(66, 13);
            this._stringToReadLabel.TabIndex = 9;
            this._stringToReadLabel.Text = "String Read:";
            //
            // _terminateButton
            //
            this._terminateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._terminateButton.Enabled = false;
            this._terminateButton.Location = new System.Drawing.Point(205, 83);
            this._terminateButton.Name = "_TerminateButton";
            this._terminateButton.Size = new System.Drawing.Size(74, 23);
            this._terminateButton.TabIndex = 5;
            this._terminateButton.Text = "&Terminate";
            this._terminateButton.Click += new System.EventHandler(this.Terminate_Click);
            //
            // _elementsTransferredLabel
            //
            this._elementsTransferredLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._elementsTransferredLabel.AutoSize = true;
            this._elementsTransferredLabel.Location = new System.Drawing.Point(5, 303);
            this._elementsTransferredLabel.Name = "_ElementsTransferredLabel";
            this._elementsTransferredLabel.Size = new System.Drawing.Size(110, 13);
            this._elementsTransferredLabel.TabIndex = 11;
            this._elementsTransferredLabel.Text = "Elements Transferred:";
            //
            // _elementsTransferredTextBox
            //
            this._elementsTransferredTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._elementsTransferredTextBox.Location = new System.Drawing.Point(118, 300);
            this._elementsTransferredTextBox.Name = "_ElementsTransferredTextBox";
            this._elementsTransferredTextBox.ReadOnly = true;
            this._elementsTransferredTextBox.Size = new System.Drawing.Size(54, 20);
            this._elementsTransferredTextBox.TabIndex = 12;
            this._elementsTransferredTextBox.TabStop = false;
            //
            // _lastIOStatusTextBox
            //
            this._lastIOStatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lastIOStatusTextBox.Location = new System.Drawing.Point(118, 322);
            this._lastIOStatusTextBox.Name = "_LastIOStatusTextBox";
            this._lastIOStatusTextBox.ReadOnly = true;
            this._lastIOStatusTextBox.Size = new System.Drawing.Size(161, 20);
            this._lastIOStatusTextBox.TabIndex = 14;
            this._lastIOStatusTextBox.TabStop = false;
            //
            // _lastIOStatusLabel
            //
            this._lastIOStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._lastIOStatusLabel.AutoSize = true;
            this._lastIOStatusLabel.Location = new System.Drawing.Point(33, 325);
            this._lastIOStatusLabel.Name = "_LastIOStatusLabel";
            this._lastIOStatusLabel.Size = new System.Drawing.Size(82, 13);
            this._lastIOStatusLabel.TabIndex = 13;
            this._lastIOStatusLabel.Text = "Last I/O Status:";
            //
            // _inLabel
            //
            this._inLabel.AutoSize = true;
            this._inLabel.Location = new System.Drawing.Point(178, 304);
            this._inLabel.Name = "_InLabel";
            this._inLabel.Size = new System.Drawing.Size(15, 13);
            this._inLabel.TabIndex = 15;
            this._inLabel.Text = "in";
            //
            // _elapsedLabel
            //
            this._elapsedLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._elapsedLabel.Location = new System.Drawing.Point(197, 303);
            this._elapsedLabel.Name = "_ElapsedLabel";
            this._elapsedLabel.Size = new System.Drawing.Size(57, 15);
            this._elapsedLabel.TabIndex = 15;
            //
            // _msLabel
            //
            this._msLabel.AutoSize = true;
            this._msLabel.Location = new System.Drawing.Point(257, 304);
            this._msLabel.Name = "_msLabel";
            this._msLabel.Size = new System.Drawing.Size(20, 13);
            this._msLabel.TabIndex = 15;
            this._msLabel.Text = "ms";
            //
            // MainForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(287, 376);
            this.Controls.Add(this._msLabel);
            this.Controls.Add(this._elapsedLabel);
            this.Controls.Add(this._inLabel);
            this.Controls.Add(this._lastIOStatusTextBox);
            this.Controls.Add(this._elementsTransferredTextBox);
            this.Controls.Add(this._readTextBox);
            this.Controls.Add(this._writeTextBox);
            this.Controls.Add(this._lastIOStatusLabel);
            this.Controls.Add(this._elementsTransferredLabel);
            this.Controls.Add(this._terminateButton);
            this.Controls.Add(this._stringToReadLabel);
            this.Controls.Add(this._stringToWriteLabel);
            this.Controls.Add(this._closeSessionButton);
            this.Controls.Add(this._clearButton);
            this.Controls.Add(this._openSessionButton);
            this.Controls.Add(this._readButton);
            this.Controls.Add(this._writeButton);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(295, 316);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simple Asynchronous Read/Write";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label _inLabel;
        private System.Windows.Forms.Label _elapsedLabel;
        private System.Windows.Forms.Label _msLabel;
    }
}
