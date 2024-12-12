using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls
{
    public partial class SessionView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionView));
            _writeComboBox = new ComboBox();
            _readTextBox = new TextBox();
            _queryButton = new cc.isr.WinControls.ToolStripButton();
            _writeButton = new cc.isr.WinControls.ToolStripButton();
            _readButton = new cc.isr.WinControls.ToolStripButton();
            _readDelayNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _statusReadDelayNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _toolStrip = new ToolStrip();
            _moreOptionsSplitButton = new ToolStripSplitButton();
            _eraseDisplayMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _clearSessionMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _readStatusMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _showPollReadingsMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _showServiceRequestReadingMenuItem = new ToolStripMenuItem();
            _appendTerminationMenuItem = new ToolStripMenuItem();
            _toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _writeComboBox
            // 
            _writeComboBox.Dock = DockStyle.Top;
            _writeComboBox.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _writeComboBox.FormattingEnabled = true;
            _writeComboBox.Location = new System.Drawing.Point(1, 1);
            _writeComboBox.Margin = new Padding(3, 4, 3, 4);
            _writeComboBox.Name = "_WriteComboBox";
            _writeComboBox.Size = new System.Drawing.Size(388, 25);
            _writeComboBox.TabIndex = 53;
            // 
            // _readTextBox
            // 
            _readTextBox.Dock = DockStyle.Fill;
            _readTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _readTextBox.Location = new System.Drawing.Point(1, 54);
            _readTextBox.Margin = new Padding(3, 5, 3, 5);
            _readTextBox.Multiline = true;
            _readTextBox.Name = "_ReadTextBox";
            _readTextBox.ReadOnly = true;
            _readTextBox.ScrollBars = ScrollBars.Vertical;
            _readTextBox.Size = new System.Drawing.Size(388, 273);
            _readTextBox.TabIndex = 59;
            _readTextBox.TabStop = false;
            // 
            // _queryButton
            // 
            _queryButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _queryButton.Image = (System.Drawing.Image)resources.GetObject("_QueryButton.Image");
            _queryButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _queryButton.Margin = new Padding(0, 1, 1, 2);
            _queryButton.Name = "_QueryButton";
            _queryButton.Size = new System.Drawing.Size(47, 25);
            _queryButton.Text = "&Query";
            _queryButton.ToolTipText = "Write (send) the 'Message to Send' to the instrument, read the reply and display " + "in the 'Received data' text box";
            _queryButton.Click += new EventHandler( QueryButton_Click );
            // 
            // _writeButton
            // 
            _writeButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _writeButton.Image = (System.Drawing.Image)resources.GetObject("_WriteButton.Image");
            _writeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _writeButton.Margin = new Padding(0, 1, 2, 2);
            _writeButton.Name = "_WriteButton";
            _writeButton.Size = new System.Drawing.Size(43, 25);
            _writeButton.Text = "&Write";
            _writeButton.ToolTipText = "Write (send) the 'Message to Send' to the instrument";
            _writeButton.Click += new EventHandler( WriteButton_Click );
            // 
            // _readButton
            // 
            _readButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _readButton.Image = (System.Drawing.Image)resources.GetObject("_ReadButton.Image");
            _readButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _readButton.Margin = new Padding(0, 1, 2, 2);
            _readButton.Name = "_ReadButton";
            _readButton.Size = new System.Drawing.Size(42, 25);
            _readButton.Text = "&Read";
            _readButton.ToolTipText = "Read a message from the instrument and display in the 'Received data' text box";
            _readButton.Click += new EventHandler( ReadButton_Click );
            // 
            // _readDelayNumeric
            // 
            _readDelayNumeric.Name = "_ReadDelayNumeric";
            _readDelayNumeric.Size = new System.Drawing.Size(45, 25);
            _readDelayNumeric.Text = "10";
            _readDelayNumeric.ToolTipText = "Delay before reading in milliseconds";
            _readDelayNumeric.Value = new decimal(new int[] { 10, 0, 0, 0 });
            _readDelayNumeric.NumericUpDown.ValueChanged += new EventHandler( ReadDelayNumeric_ValueChanged );
            // 
            // _statusReadDelayNumeric
            // 
            _statusReadDelayNumeric.Name = "_StatusReadDelayNumeric";
            _statusReadDelayNumeric.Size = new System.Drawing.Size(45, 25);
            _statusReadDelayNumeric.Text = "10";
            _statusReadDelayNumeric.ToolTipText = "Delay time before reading status in milliseconds";
            _statusReadDelayNumeric.Value = new decimal(new int[] { 10, 0, 0, 0 });
            _statusReadDelayNumeric.NumericUpDown.ValueChanged += new EventHandler( StatusReadDelayNumeric_ValueChanged );
            // 
            // _toolStrip
            // 
            _toolStrip.BackColor = System.Drawing.Color.Transparent;
            _toolStrip.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _toolStrip.GripMargin = new Padding(0);
            _toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            _toolStrip.Items.AddRange(new ToolStripItem[] { _queryButton, _writeButton, _readButton, _readDelayNumeric, _statusReadDelayNumeric, _moreOptionsSplitButton });
            _toolStrip.Location = new System.Drawing.Point(1, 26);
            _toolStrip.Name = "_ToolStrip";
            _toolStrip.Size = new System.Drawing.Size(388, 28);
            _toolStrip.TabIndex = 65;
            _toolStrip.Text = "ToolStrip1";
            // 
            // _moreOptionsSplitButton
            // 
            _moreOptionsSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _moreOptionsSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _eraseDisplayMenuItem, _clearSessionMenuItem, _readStatusMenuItem, _showPollReadingsMenuItem, _showServiceRequestReadingMenuItem, _appendTerminationMenuItem });
            _moreOptionsSplitButton.Image = (System.Drawing.Image)resources.GetObject("_MoreOptionsSplitButton.Image");
            _moreOptionsSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _moreOptionsSplitButton.Name = "_MoreOptionsSplitButton";
            _moreOptionsSplitButton.Size = new System.Drawing.Size(79, 25);
            _moreOptionsSplitButton.Text = "&Options...";
            _moreOptionsSplitButton.ToolTipText = "Select from additional options";
            // 
            // _eraseDisplayMenuItem
            // 
            _eraseDisplayMenuItem.Name = "_EraseDisplayMenuItem";
            _eraseDisplayMenuItem.Size = new System.Drawing.Size(194, 22);
            _eraseDisplayMenuItem.Text = "&Erase Display";
            _eraseDisplayMenuItem.ToolTipText = "Clears the display";
            _eraseDisplayMenuItem.Click += new EventHandler( EraseDisplayMenuItem_Click );
            // 
            // _clearSessionMenuItem
            // 
            _clearSessionMenuItem.Name = "_ClearSessionMenuItem";
            _clearSessionMenuItem.Size = new System.Drawing.Size(194, 22);
            _clearSessionMenuItem.Text = "&Clear Session";
            _clearSessionMenuItem.ToolTipText = "Clears the session (*CLS)";
            _clearSessionMenuItem.Click += new EventHandler( ClearSessionMenuItem_Click );
            // 
            // _readStatusMenuItem
            // 
            _readStatusMenuItem.Name = "_ReadStatusMenuItem";
            _readStatusMenuItem.Size = new System.Drawing.Size(194, 22);
            _readStatusMenuItem.Text = "&Read Status Byte";
            _readStatusMenuItem.ToolTipText = "Reads the status byte";
            _readStatusMenuItem.Click += new EventHandler( ReadStatusMenuItem_Click );
            // 
            // _showPollReadingsMenuItem
            // 
            _showPollReadingsMenuItem.CheckOnClick = true;
            _showPollReadingsMenuItem.Name = "_ShowPollReadingsMenuItem";
            _showPollReadingsMenuItem.Size = new System.Drawing.Size(194, 22);
            _showPollReadingsMenuItem.Text = "Show &Poll Readings";
            _showPollReadingsMenuItem.ToolTipText = "Displays polled reading when session polling is enabled";
            // 
            // _showServiceRequestReadingMenuItem
            // 
            _showServiceRequestReadingMenuItem.CheckOnClick = true;
            _showServiceRequestReadingMenuItem.Name = "_ShowServiceRequestReadingMenuItem";
            _showServiceRequestReadingMenuItem.Size = new System.Drawing.Size(194, 22);
            _showServiceRequestReadingMenuItem.Text = "Show SR&Q Reading";
            _showServiceRequestReadingMenuItem.ToolTipText = "Shows service request reading when service request auto read is enabled";
            // 
            // _appendTerminationMenuItem
            // 
            _appendTerminationMenuItem.CheckOnClick = true;
            _appendTerminationMenuItem.Name = "_appendTerminationMenuItem";
            _appendTerminationMenuItem.Size = new System.Drawing.Size(194, 22);
            _appendTerminationMenuItem.Text = "&Append Termination";
            _appendTerminationMenuItem.ToolTipText = "Appends termination to instrument commands thus not having the add the terminatio" + @"n character (\n)";
            // 
            // SessionView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_readTextBox);
            Controls.Add(_toolStrip);
            Controls.Add(_writeComboBox);
            Name = "SessionView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(390, 328);
            _toolStrip.ResumeLayout(false);
            _toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
