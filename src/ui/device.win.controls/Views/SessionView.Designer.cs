using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls
{
    public partial class SessionView
    {
        // Required by the Windows Form Designer
        // private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this._writeComboBox = new System.Windows.Forms.ComboBox();
            this._readTextBox = new System.Windows.Forms.TextBox();
            this._queryButton = new cc.isr.WinControls.ToolStripButton();
            this._writeButton = new cc.isr.WinControls.ToolStripButton();
            this._readButton = new cc.isr.WinControls.ToolStripButton();
            this._readDelayNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._statusReadDelayNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._moreOptionsSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this._eraseDisplayMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._clearSessionMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._readStatusMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._showPollReadingsMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._showServiceRequestReadingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._appendTerminationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this._readDelayNumeric.NumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._statusReadDelayNumeric.NumericUpDown)).BeginInit();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _writeComboBox
            // 
            this._writeComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._writeComboBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._writeComboBox.FormattingEnabled = true;
            this._writeComboBox.Location = new System.Drawing.Point(1, 1);
            this._writeComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._writeComboBox.Name = "_writeComboBox";
            this._writeComboBox.Size = new System.Drawing.Size(444, 25);
            this._writeComboBox.TabIndex = 53;
            // 
            // _readTextBox
            // 
            this._readTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._readTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this._readTextBox.Location = new System.Drawing.Point(1, 54);
            this._readTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this._readTextBox.Multiline = true;
            this._readTextBox.Name = "_readTextBox";
            this._readTextBox.ReadOnly = true;
            this._readTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._readTextBox.Size = new System.Drawing.Size(444, 254);
            this._readTextBox.TabIndex = 59;
            this._readTextBox.TabStop = false;
            // 
            // _queryButton
            // 
            this._queryButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._queryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._queryButton.Margin = new System.Windows.Forms.Padding(0, 1, 1, 2);
            this._queryButton.Name = "_queryButton";
            this._queryButton.Size = new System.Drawing.Size(47, 25);
            this._queryButton.Text = "&Query";
            this._queryButton.ToolTipText = "Write (send) the \'Message to Send\' to the instrument, read the reply and display " +
    "in the \'Received data\' text box";
            this._queryButton.Click += new System.EventHandler(this.QueryButton_Click);
            // 
            // _writeButton
            // 
            this._writeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._writeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._writeButton.Margin = new System.Windows.Forms.Padding(0, 1, 2, 2);
            this._writeButton.Name = "_writeButton";
            this._writeButton.Size = new System.Drawing.Size(43, 25);
            this._writeButton.Text = "&Write";
            this._writeButton.ToolTipText = "Write (send) the \'Message to Send\' to the instrument";
            this._writeButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // _readButton
            // 
            this._readButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._readButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._readButton.Margin = new System.Windows.Forms.Padding(0, 1, 2, 2);
            this._readButton.Name = "_readButton";
            this._readButton.Size = new System.Drawing.Size(42, 25);
            this._readButton.Text = "&Read";
            this._readButton.ToolTipText = "Read and display a message from the instrument";
            this._readButton.Click += new System.EventHandler(this.ReadButton_Click);
            // 
            // _readDelayNumeric
            // 
            this._readDelayNumeric.Name = "_readDelayNumeric";
            // 
            // _readDelayNumeric
            // 
            this._readDelayNumeric.NumericUpDown.Location = new System.Drawing.Point(137, 1);
            this._readDelayNumeric.NumericUpDown.Name = "_readDelayNumeric";
            this._readDelayNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._readDelayNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._readDelayNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._readDelayNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._readDelayNumeric.NumericUpDown.Size = new System.Drawing.Size(45, 25);
            this._readDelayNumeric.NumericUpDown.TabIndex = 0;
            this._readDelayNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._readDelayNumeric.NumericUpDown.ValueChanged += new System.EventHandler(this.ReadDelayNumeric_ValueChanged);
            this._readDelayNumeric.Size = new System.Drawing.Size(45, 25);
            this._readDelayNumeric.Text = "0";
            this._readDelayNumeric.ToolTipText = "Delay before reading in milliseconds";
            this._readDelayNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _statusReadDelayNumeric
            // 
            this._statusReadDelayNumeric.Name = "_statusReadDelayNumeric";
            // 
            // _statusReadDelayNumeric
            // 
            this._statusReadDelayNumeric.NumericUpDown.Location = new System.Drawing.Point(182, 1);
            this._statusReadDelayNumeric.NumericUpDown.Name = "_statusReadDelayNumeric";
            this._statusReadDelayNumeric.NumericUpDown.ReadOnlyBackColor = System.Drawing.SystemColors.Control;
            this._statusReadDelayNumeric.NumericUpDown.ReadOnlyForeColor = System.Drawing.SystemColors.WindowText;
            this._statusReadDelayNumeric.NumericUpDown.ReadWriteBackColor = System.Drawing.SystemColors.Window;
            this._statusReadDelayNumeric.NumericUpDown.ReadWriteForeColor = System.Drawing.SystemColors.ControlText;
            this._statusReadDelayNumeric.NumericUpDown.Size = new System.Drawing.Size(45, 25);
            this._statusReadDelayNumeric.NumericUpDown.TabIndex = 1;
            this._statusReadDelayNumeric.NumericUpDown.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._statusReadDelayNumeric.NumericUpDown.ValueChanged += new System.EventHandler(this.StatusReadDelayNumeric_ValueChanged);
            this._statusReadDelayNumeric.Size = new System.Drawing.Size(45, 25);
            this._statusReadDelayNumeric.Text = "0";
            this._statusReadDelayNumeric.ToolTipText = "Delay time before reading status in milliseconds";
            this._statusReadDelayNumeric.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.Transparent;
            this._toolStrip.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._toolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._queryButton,
            this._writeButton,
            this._readButton,
            this._readDelayNumeric,
            this._statusReadDelayNumeric,
            this._moreOptionsSplitButton});
            this._toolStrip.Location = new System.Drawing.Point(1, 26);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(444, 28);
            this._toolStrip.TabIndex = 65;
            this._toolStrip.Text = "ToolStrip1";
            // 
            // _moreOptionsSplitButton
            // 
            this._moreOptionsSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._moreOptionsSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._eraseDisplayMenuItem,
            this._clearSessionMenuItem,
            this._readStatusMenuItem,
            this._showPollReadingsMenuItem,
            this._showServiceRequestReadingMenuItem,
            this._appendTerminationMenuItem});
            this._moreOptionsSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._moreOptionsSplitButton.Name = "_moreOptionsSplitButton";
            this._moreOptionsSplitButton.Size = new System.Drawing.Size(79, 25);
            this._moreOptionsSplitButton.Text = "&Options...";
            this._moreOptionsSplitButton.ToolTipText = "Select from additional options";
            // 
            // _eraseDisplayMenuItem
            // 
            this._eraseDisplayMenuItem.Name = "_eraseDisplayMenuItem";
            this._eraseDisplayMenuItem.Size = new System.Drawing.Size(194, 22);
            this._eraseDisplayMenuItem.Text = "&Erase Display";
            this._eraseDisplayMenuItem.ToolTipText = "Clears the display";
            this._eraseDisplayMenuItem.Click += new System.EventHandler(this.EraseDisplayMenuItem_Click);
            // 
            // _clearSessionMenuItem
            // 
            this._clearSessionMenuItem.Name = "_clearSessionMenuItem";
            this._clearSessionMenuItem.Size = new System.Drawing.Size(194, 22);
            this._clearSessionMenuItem.Text = "&Clear Session";
            this._clearSessionMenuItem.ToolTipText = "Clears the session (*CLS)";
            this._clearSessionMenuItem.Click += new System.EventHandler(this.ClearSessionMenuItem_Click);
            // 
            // _readStatusMenuItem
            // 
            this._readStatusMenuItem.Name = "_readStatusMenuItem";
            this._readStatusMenuItem.Size = new System.Drawing.Size(194, 22);
            this._readStatusMenuItem.Text = "&Read Status Byte";
            this._readStatusMenuItem.ToolTipText = "Reads the status byte";
            this._readStatusMenuItem.Click += new System.EventHandler(this.ReadStatusMenuItem_Click);
            // 
            // _showPollReadingsMenuItem
            // 
            this._showPollReadingsMenuItem.CheckOnClick = true;
            this._showPollReadingsMenuItem.Name = "_showPollReadingsMenuItem";
            this._showPollReadingsMenuItem.Size = new System.Drawing.Size(194, 22);
            this._showPollReadingsMenuItem.Text = "Show &Poll Readings";
            this._showPollReadingsMenuItem.ToolTipText = "Displays polled reading when session polling is enabled";
            // 
            // _showServiceRequestReadingMenuItem
            // 
            this._showServiceRequestReadingMenuItem.CheckOnClick = true;
            this._showServiceRequestReadingMenuItem.Name = "_showServiceRequestReadingMenuItem";
            this._showServiceRequestReadingMenuItem.Size = new System.Drawing.Size(194, 22);
            this._showServiceRequestReadingMenuItem.Text = "Show SR&Q Reading";
            this._showServiceRequestReadingMenuItem.ToolTipText = "Shows service request reading when service request auto read is enabled";
            // 
            // _appendTerminationMenuItem
            // 
            this._appendTerminationMenuItem.CheckOnClick = true;
            this._appendTerminationMenuItem.Name = "_appendTerminationMenuItem";
            this._appendTerminationMenuItem.Size = new System.Drawing.Size(194, 22);
            this._appendTerminationMenuItem.Text = "&Append Termination";
            this._appendTerminationMenuItem.ToolTipText = "Appends termination to instrument commands thus not having the add the " +
    "termination character (\n)";
            // 
            // SessionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._readTextBox);
            this.Controls.Add(this._toolStrip);
            this.Controls.Add(this._writeComboBox);
            this.Name = "SessionView";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(446, 309);
            ((System.ComponentModel.ISupportInitialize)(this._readDelayNumeric.NumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._statusReadDelayNumeric.NumericUpDown)).EndInit();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private ComboBox _writeComboBox;
        private TextBox _readTextBox;
        private cc.isr.WinControls.ToolStripButton _queryButton;
        private cc.isr.WinControls.ToolStripButton _writeButton;
        private cc.isr.WinControls.ToolStripButton _readButton;
        private cc.isr.WinControls.ToolStripNumericUpDown _readDelayNumeric;
        private cc.isr.WinControls.ToolStripNumericUpDown _statusReadDelayNumeric;
        private ToolStrip _toolStrip;
        private ToolStripSplitButton _moreOptionsSplitButton;
        private cc.isr.WinControls.ToolStripMenuItem _eraseDisplayMenuItem;
        private cc.isr.WinControls.ToolStripMenuItem _clearSessionMenuItem;
        private cc.isr.WinControls.ToolStripMenuItem _readStatusMenuItem;
        private cc.isr.WinControls.ToolStripMenuItem _showPollReadingsMenuItem;
        private ToolStripMenuItem _showServiceRequestReadingMenuItem;
        private ToolStripMenuItem _appendTerminationMenuItem;
    }
}
