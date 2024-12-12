using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls
{
    public partial class StatusView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusView));
            _toolStrip = new ToolStrip();
            _applicationDropDownButton = new ToolStripDropDownButton();
            EditSettingsToolStripMenuItem = new ToolStripMenuItem();
            _sessionSettingsMenuItem = new ToolStripMenuItem();
            _deviceSettingsMenuItem = new ToolStripMenuItem();
            _userInterfaceSettingsMenuItem = new ToolStripMenuItem();
            _uIBaseSettingsMenuItem = new ToolStripMenuItem();
            _traceMenuItem = new ToolStripMenuItem();
            _traceLogLevelMenuItem = new ToolStripMenuItem();
            _traceLogLevelComboBox = new cc.isr.WinControls.ToolStripComboBox();
            _traceShowLevelMenuItem = new ToolStripMenuItem();
            _traceShowLevelComboBox = new cc.isr.WinControls.ToolStripComboBox();
            _sessionNotificationLevelMenuItem = new ToolStripMenuItem();
            _sessionNotificationLevelComboBox = new cc.isr.WinControls.ToolStripComboBox();
            _usingStatusSubsystemMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _deviceDropDownButton = new cc.isr.WinControls.ToolStripDropDownButton();
            _resetMenuItem = new ToolStripMenuItem();
            _clearInterfaceMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _clearDeviceMenuItem = new ToolStripMenuItem();
            _resetKnownStateMenuItem = new ToolStripMenuItem();
            _clearExecutionStateMenuItem = new ToolStripMenuItem();
            _initKnownStateMenuItem = new ToolStripMenuItem();
            _clearErrorReportMenuItem = new ToolStripMenuItem();
            _readDeviceErrorsMenuItem = new ToolStripMenuItem();
            _readTerminalsStateMenuItem = new ToolStripMenuItem();
            _sessionDownDownButton = new cc.isr.WinControls.ToolStripDropDownButton();
            _readTerminationMenuItem = new ToolStripMenuItem();
            _readTerminationTextBox = new cc.isr.WinControls.ToolStripTextBox();
            _readTerminationEnabledMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _writeTerminationMenuItem = new ToolStripMenuItem();
            _writeTerminationTextBox = new cc.isr.WinControls.ToolStripTextBox();
            _sessionTimeoutMenuItem = new ToolStripMenuItem();
            _sessionTimeoutTextBox = new cc.isr.WinControls.ToolStripTextBox();
            _storeTimeoutMenuItem = new ToolStripMenuItem();
            _restoreTimeoutMenuItem = new ToolStripMenuItem();
            _sessionReadStatusByteMenuItem = new ToolStripMenuItem();
            _sessionReadStandardEventRegisterMenuItem = new ToolStripMenuItem();
            _sendBusTriggerMenuItem = new ToolStripMenuItem();
            _pollSplitButton = new cc.isr.WinControls.ToolStripDropDownButton();
            _pollMessageStatusBitMenuItem = new ToolStripMenuItem();
            _pollMessageStatusBitTextBox = new cc.isr.WinControls.ToolStripTextBox();
            _pollCommandMenuItem = new ToolStripMenuItem();
            _pollWriteCommandTextBox = new cc.isr.WinControls.ToolStripTextBox();
            _pollIntervalMenuItem = new ToolStripMenuItem();
            _pollIntervalTextBox = new cc.isr.WinControls.ToolStripTextBox();
            _pollEnabledMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _pollEnabledMenuItem.CheckStateChanged += new EventHandler(PollEnabledMenuItem_CheckStateChanged);
            _pollAutoReadMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _pollSendMenuItem = new ToolStripMenuItem();
            _serviceRequestSplitButton = new cc.isr.WinControls.ToolStripDropDownButton();
            _serviceRequestBitmaskMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _serviceRequestBitMaskTextBox = new cc.isr.WinControls.ToolStripTextBox();
            _serviceRequestEnableCommandMenuItem = new ToolStripMenuItem();
            _serviceRequestEnableCommandTextBox = new cc.isr.WinControls.ToolStripTextBox();
            _programServiceRequestEnableBitmaskMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _toggleVisaEventHandlerMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _serviceRequestHandlerAddRemoveMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            _serviceRequestAutoReadMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            EditResourceNamesMenuItem = new ToolStripMenuItem();
            _toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _toolStrip
            // 
            _toolStrip.BackColor = Color.Transparent;
            _toolStrip.Dock = DockStyle.Fill;
            _toolStrip.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _toolStrip.GripMargin = new Padding(0);
            _toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            _toolStrip.Items.AddRange(new ToolStripItem[] { _applicationDropDownButton, _deviceDropDownButton, _sessionDownDownButton, _pollSplitButton, _serviceRequestSplitButton });
            _toolStrip.Location = new Point(0, 0);
            _toolStrip.Name = "_ToolStrip";
            _toolStrip.Size = new Size(474, 31);
            _toolStrip.TabIndex = 12;
            // 
            // _applicationDropDownButton
            // 
            _applicationDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _applicationDropDownButton.DropDownItems.AddRange(new ToolStripItem[] { EditSettingsToolStripMenuItem, _traceMenuItem, _sessionNotificationLevelMenuItem, _usingStatusSubsystemMenuItem, EditResourceNamesMenuItem });
            _applicationDropDownButton.Image = (Image)resources.GetObject("_applicationDropDownButton.Image");
            _applicationDropDownButton.ImageTransparentColor = Color.Magenta;
            _applicationDropDownButton.Name = "_applicationDropDownButton";
            _applicationDropDownButton.Size = new Size(46, 28);
            _applicationDropDownButton.Text = "App";
            _applicationDropDownButton.ToolTipText = "Application options";
            // 
            // EditSettingsToolStripMenuItem
            // 
            EditSettingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _sessionSettingsMenuItem, _deviceSettingsMenuItem, _userInterfaceSettingsMenuItem, _uIBaseSettingsMenuItem });
            EditSettingsToolStripMenuItem.Name = "EditSettingsToolStripMenuItem";
            EditSettingsToolStripMenuItem.Size = new Size(256, 22);
            EditSettingsToolStripMenuItem.Text = "Edit Settings";
            // 
            // _sessionSettingsMenuItem
            // 
            _sessionSettingsMenuItem.Name = "_SessionSettingsMenuItem";
            _sessionSettingsMenuItem.Size = new Size(134, 22);
            _sessionSettingsMenuItem.Text = "Session...";
            _sessionSettingsMenuItem.ToolTipText = "Opens session settings dialog";
            _sessionSettingsMenuItem.Click += new EventHandler( SessionSettingsMenuItem_Click );
            // 
            // _deviceSettingsMenuItem
            // 
            _deviceSettingsMenuItem.Name = "_DeviceSettingsMenuItem";
            _deviceSettingsMenuItem.Size = new Size(134, 22);
            _deviceSettingsMenuItem.Text = "Device...";
            _deviceSettingsMenuItem.ToolTipText = "Opens device settings editor dialog";
            _deviceSettingsMenuItem.Click += new EventHandler( DeviceSettingsMenuItem_Click );
            // 
            // _userInterfaceSettingsMenuItem
            // 
            _userInterfaceSettingsMenuItem.Name = "_UserInterfaceSettingsMenuItem";
            _userInterfaceSettingsMenuItem.Size = new Size(134, 22);
            _userInterfaceSettingsMenuItem.Text = "UI...";
            _userInterfaceSettingsMenuItem.ToolTipText = "Opens User Interface settings editor dialog";
            _userInterfaceSettingsMenuItem.Click += new EventHandler( UserInterfaceToolStripMenuItem_Click );
            // 
            // _uIBaseSettingsMenuItem
            // 
            _uIBaseSettingsMenuItem.Name = "_UIBaseSettingsMenuItem";
            _uIBaseSettingsMenuItem.Size = new Size(134, 22);
            _uIBaseSettingsMenuItem.Text = "UI Base...";
            _uIBaseSettingsMenuItem.ToolTipText = "Edits the user interface base settings";
            _uIBaseSettingsMenuItem.Click += new EventHandler( UIBaseSettingsMenuItem_Click );
            // 
            // _traceMenuItem
            // 
            _traceMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _traceLogLevelMenuItem, _traceShowLevelMenuItem });
            _traceMenuItem.Name = "_TraceMenuItem";
            _traceMenuItem.Size = new Size(256, 22);
            _traceMenuItem.Text = "Select Trace Levels";
            _traceMenuItem.ToolTipText = "Opens the trace menus";
            // 
            // _traceLogLevelMenuItem
            // 
            _traceLogLevelMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _traceLogLevelComboBox });
            _traceLogLevelMenuItem.Name = "_TraceLogLevelMenuItem";
            _traceLogLevelMenuItem.Size = new Size(194, 22);
            _traceLogLevelMenuItem.Text = "Trace Log Level";
            _traceLogLevelMenuItem.ToolTipText = "Selects trace level for logging";
            // 
            // _traceLogLevelComboBox
            // 
            _traceLogLevelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _traceLogLevelComboBox.Name = "_TraceLogLevelComboBox";
            _traceLogLevelComboBox.Size = new Size(100, 22);
            _traceLogLevelComboBox.ToolTipText = "Selects trace level for logging";
            // 
            // _traceShowLevelMenuItem
            // 
            _traceShowLevelMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _traceShowLevelComboBox });
            _traceShowLevelMenuItem.Name = "_TraceShowLevelMenuItem";
            _traceShowLevelMenuItem.Size = new Size(194, 22);
            _traceShowLevelMenuItem.Text = "Display Trace Level";
            _traceShowLevelMenuItem.ToolTipText = "Selects trace level for display";
            // 
            // _traceShowLevelComboBox
            // 
            _traceShowLevelComboBox.Name = "_TraceShowLevelComboBox";
            _traceShowLevelComboBox.Size = new Size(100, 22);
            _traceShowLevelComboBox.ToolTipText = "Selects trace level for display";
            // 
            // _sessionNotificationLevelMenuItem
            // 
            _sessionNotificationLevelMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _sessionNotificationLevelComboBox });
            _sessionNotificationLevelMenuItem.Name = "_SessionNotificationLevelMenuItem";
            _sessionNotificationLevelMenuItem.Size = new Size(256, 22);
            _sessionNotificationLevelMenuItem.Text = "Session Notification Level";
            _sessionNotificationLevelMenuItem.ToolTipText = "Shows the session notification level selector";
            // 
            // _sessionNotificationLevelComboBox
            // 
            _sessionNotificationLevelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _sessionNotificationLevelComboBox.Name = "_SessionNotificationLevelComboBox";
            _sessionNotificationLevelComboBox.Size = new Size(100, 22);
            _sessionNotificationLevelComboBox.ToolTipText = "Select the session notification level";
            // 
            // _usingStatusSubsystemMenuItem
            // 
            _usingStatusSubsystemMenuItem.CheckOnClick = true;
            _usingStatusSubsystemMenuItem.Name = "_UsingStatusSubsystemMenuItem";
            _usingStatusSubsystemMenuItem.Size = new Size(256, 22);
            _usingStatusSubsystemMenuItem.Text = "Using Status Subsystem Only";
            _usingStatusSubsystemMenuItem.ToolTipText = "Toggle to use status or all subsystems";
            _usingStatusSubsystemMenuItem.CheckStateChanged += new EventHandler( OpenStatusSubsystemMenuItem_CheckStateChanged );
            // 
            // _deviceDropDownButton
            // 
            _deviceDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _deviceDropDownButton.DropDownItems.AddRange(new ToolStripItem[] { _resetMenuItem, _clearErrorReportMenuItem, _readDeviceErrorsMenuItem, _readTerminalsStateMenuItem });
            _deviceDropDownButton.ImageTransparentColor = Color.Magenta;
            _deviceDropDownButton.Name = "_DeviceDropDownButton";
            _deviceDropDownButton.Size = new Size(62, 28);
            _deviceDropDownButton.Text = "Device";
            _deviceDropDownButton.ToolTipText = "Device resets, Service Requests and trace management";
            // 
            // _resetMenuItem
            // 
            _resetMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _clearInterfaceMenuItem, _clearDeviceMenuItem, _resetKnownStateMenuItem, _clearExecutionStateMenuItem, _initKnownStateMenuItem });
            _resetMenuItem.Name = "_ResetMenuItem";
            _resetMenuItem.Size = new Size(205, 22);
            _resetMenuItem.Text = "Reset...";
            _resetMenuItem.ToolTipText = "Opens the reset menus";
            // 
            // _clearInterfaceMenuItem
            // 
            _clearInterfaceMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _clearInterfaceMenuItem.Name = "_ClearInterfaceMenuItem";
            _clearInterfaceMenuItem.Size = new Size(242, 22);
            _clearInterfaceMenuItem.Text = "Clear Interface";
            _clearInterfaceMenuItem.ToolTipText = "Issues an interface clear command";
            _clearInterfaceMenuItem.Click += new EventHandler( ClearInterfaceMenuItem_Click );
            // 
            // _clearDeviceMenuItem
            // 
            _clearDeviceMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _clearDeviceMenuItem.Name = "_ClearDeviceMenuItem";
            _clearDeviceMenuItem.Size = new Size(242, 22);
            _clearDeviceMenuItem.Text = "Clear Device (SDC)";
            _clearDeviceMenuItem.ToolTipText = "Issues Selective Device Clear";
            _clearDeviceMenuItem.Click += new EventHandler( ClearDeviceMenuItem_Click );
            // 
            // _resetKnownStateMenuItem
            // 
            _resetKnownStateMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _resetKnownStateMenuItem.Name = "_ResetKnownStateMenuItem";
            _resetKnownStateMenuItem.Size = new Size(242, 22);
            _resetKnownStateMenuItem.Text = "Reset Known State (RST)";
            _resetKnownStateMenuItem.ToolTipText = "Issues *RST";
            _resetKnownStateMenuItem.Click += new EventHandler( ResetKnownStateMenuItem_Click );
            // 
            // _clearExecutionStateMenuItem
            // 
            _clearExecutionStateMenuItem.Name = "_ClearExecutionStateMenuItem";
            _clearExecutionStateMenuItem.Size = new Size(242, 22);
            _clearExecutionStateMenuItem.Text = "Clear Execution State (CLS)";
            _clearExecutionStateMenuItem.ToolTipText = "Clears execution state (CLS)";
            _clearExecutionStateMenuItem.Click += new EventHandler( ClearExecutionStateMenuItem_Click );
            // 
            // _initKnownStateMenuItem
            // 
            _initKnownStateMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _initKnownStateMenuItem.Name = "_InitKnownStateMenuItem";
            _initKnownStateMenuItem.Size = new Size(242, 22);
            _initKnownStateMenuItem.Text = "Initialize Known State";
            _initKnownStateMenuItem.ToolTipText = "Issues *RST, CLear and initialize to custom known state";
            _initKnownStateMenuItem.Click += new EventHandler( InitKnownStateMenuItem_Click );
            // 
            // _clearErrorReportMenuItem
            // 
            _clearErrorReportMenuItem.Name = "_ClearErrorReportMenuItem";
            _clearErrorReportMenuItem.Size = new Size(205, 22);
            _clearErrorReportMenuItem.Text = "Clear Error Report";
            _clearErrorReportMenuItem.ToolTipText = "Clears the error report";
            _clearErrorReportMenuItem.Click += new EventHandler( ClearErrorReportMenuItem_Click );
            // 
            // _readDeviceErrorsMenuItem
            // 
            _readDeviceErrorsMenuItem.Name = "_ReadDeviceErrorsMenuItem";
            _readDeviceErrorsMenuItem.Size = new Size(205, 22);
            _readDeviceErrorsMenuItem.Text = "Read Device Errors";
            _readDeviceErrorsMenuItem.ToolTipText = "Reads device errors";
            _readDeviceErrorsMenuItem.Click += new EventHandler( ReadDeviceErrorsMenuItem_Click );
            // 
            // _readTerminalsStateMenuItem
            // 
            _readTerminalsStateMenuItem.Name = "_ReadTerminalsStateMenuItem";
            _readTerminalsStateMenuItem.Size = new Size(205, 22);
            _readTerminalsStateMenuItem.Text = "Read Terminals State";
            _readTerminalsStateMenuItem.Click += new EventHandler( ReadTerminalsStateMenuItem_Click );
            // 
            // _sessionDownDownButton
            // 
            _sessionDownDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _sessionDownDownButton.DropDownItems.AddRange(new ToolStripItem[] { _readTerminationMenuItem, _writeTerminationMenuItem, _sessionTimeoutMenuItem, _sessionReadStatusByteMenuItem, _sessionReadStandardEventRegisterMenuItem, _sendBusTriggerMenuItem });
            _sessionDownDownButton.Image = (Image)resources.GetObject("_SessionDownDownButton.Image");
            _sessionDownDownButton.ImageTransparentColor = Color.Magenta;
            _sessionDownDownButton.Name = "_SessionDownDownButton";
            _sessionDownDownButton.Size = new Size(67, 28);
            _sessionDownDownButton.Text = "Session";
            _sessionDownDownButton.ToolTipText = "Select Session Options";
            // 
            // _readTerminationMenuItem
            // 
            _readTerminationMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _readTerminationTextBox, _readTerminationEnabledMenuItem });
            _readTerminationMenuItem.Name = "_ReadTerminationMenuItem";
            _readTerminationMenuItem.Size = new Size(257, 22);
            _readTerminationMenuItem.Text = "Read Termination";
            _readTerminationMenuItem.ToolTipText = "Read termination. Typically Line Feed (10)";
            // 
            // _readTerminationTextBox
            // 
            _readTerminationTextBox.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _readTerminationTextBox.Name = "_ReadTerminationTextBox";
            _readTerminationTextBox.Size = new Size(100, 22);
            _readTerminationTextBox.ToolTipText = "Read termination in Hex";
            // 
            // _readTerminationEnabledMenuItem
            // 
            _readTerminationEnabledMenuItem.CheckOnClick = true;
            _readTerminationEnabledMenuItem.Name = "_ReadTerminationEnabledMenuItem";
            _readTerminationEnabledMenuItem.Size = new Size(160, 22);
            _readTerminationEnabledMenuItem.Text = "Enabled";
            _readTerminationEnabledMenuItem.ToolTipText = "Checked to enable read termination";
            // 
            // _writeTerminationMenuItem
            // 
            _writeTerminationMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _writeTerminationTextBox });
            _writeTerminationMenuItem.Name = "_WriteTerminationMenuItem";
            _writeTerminationMenuItem.Size = new Size(257, 22);
            _writeTerminationMenuItem.Text = "Write Termination";
            _writeTerminationMenuItem.ToolTipText = @"Write termination; typically \n (line feed)";
            // 
            // _writeTerminationTextBox
            // 
            _writeTerminationTextBox.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _writeTerminationTextBox.Name = "_WriteTerminationTextBox";
            _writeTerminationTextBox.Size = new Size(100, 22);
            _writeTerminationTextBox.ToolTipText = @"Write termination escape sequence, e.g., \n\r";
            // 
            // _sessionTimeoutMenuItem
            // 
            _sessionTimeoutMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _sessionTimeoutTextBox, _storeTimeoutMenuItem, _restoreTimeoutMenuItem });
            _sessionTimeoutMenuItem.Name = "_SessionTimeoutMenuItem";
            _sessionTimeoutMenuItem.Size = new Size(257, 22);
            _sessionTimeoutMenuItem.Text = "Timeout";
            // 
            // _sessionTimeoutTextBox
            // 
            _sessionTimeoutTextBox.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _sessionTimeoutTextBox.Name = "_SessionTimeoutTextBox";
            _sessionTimeoutTextBox.Size = new Size(100, 22);
            _sessionTimeoutTextBox.ToolTipText = "Timeout in seconds";
            // 
            // _storeTimeoutMenuItem
            // 
            _storeTimeoutMenuItem.Name = "_StoreTimeoutMenuItem";
            _storeTimeoutMenuItem.Size = new Size(172, 22);
            _storeTimeoutMenuItem.Text = "Store and Set";
            _storeTimeoutMenuItem.ToolTipText = "Pushes the previous timeout onto the stack and set a new timeout value";
            _storeTimeoutMenuItem.Click += new EventHandler( StoreTimeoutMenuItem_Click );
            // 
            // _restoreTimeoutMenuItem
            // 
            _restoreTimeoutMenuItem.Name = "_RestoreTimeoutMenuItem";
            _restoreTimeoutMenuItem.Size = new Size(172, 22);
            _restoreTimeoutMenuItem.Text = "Restore and Set";
            _restoreTimeoutMenuItem.ToolTipText = "Pulls the last timeout from the stack and sets it";
            _restoreTimeoutMenuItem.Click += new EventHandler( RestoreTimeoutMenuItem_Click );
            // 
            // _sessionReadStatusByteMenuItem
            // 
            _sessionReadStatusByteMenuItem.Name = "_SessionReadStatusByteMenuItem";
            _sessionReadStatusByteMenuItem.Size = new Size(257, 22);
            _sessionReadStatusByteMenuItem.Text = "Read Status Register";
            _sessionReadStatusByteMenuItem.ToolTipText = "Reads the session status byte";
            _sessionReadStatusByteMenuItem.Click += new EventHandler( SessionReadStatusByteMenuItem_Click );
            // 
            // _sessionReadStandardEventRegisterMenuItem
            // 
            _sessionReadStandardEventRegisterMenuItem.Name = "_SessionReadStandardEventRegisterMenuItem";
            _sessionReadStandardEventRegisterMenuItem.Size = new Size(257, 22);
            _sessionReadStandardEventRegisterMenuItem.Text = "Read Standard Event Register";
            _sessionReadStandardEventRegisterMenuItem.ToolTipText = "Reads the standard event register";
            _sessionReadStandardEventRegisterMenuItem.Click += new EventHandler( SessionReadStandardEventRegisterMenuItem_Click );
            // 
            // _sendBusTriggerMenuItem
            // 
            _sendBusTriggerMenuItem.Name = "_SendBusTriggerMenuItem";
            _sendBusTriggerMenuItem.Size = new Size(257, 22);
            _sendBusTriggerMenuItem.Text = "Send Bus Trigger";
            _sendBusTriggerMenuItem.ToolTipText = "Sends a bus trigger to the instrument";
            _sendBusTriggerMenuItem.Click += new EventHandler( SendBusTrigger_Click );
            // 
            // _pollSplitButton
            // 
            _pollSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _pollSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _pollMessageStatusBitMenuItem, _pollCommandMenuItem, _pollIntervalMenuItem, _pollEnabledMenuItem, _pollAutoReadMenuItem, _pollSendMenuItem });
            _pollSplitButton.Image = (Image)resources.GetObject("_PollSplitButton.Image");
            _pollSplitButton.ImageTransparentColor = Color.Magenta;
            _pollSplitButton.Name = "_PollSplitButton";
            _pollSplitButton.Size = new Size(45, 28);
            _pollSplitButton.Text = "Poll";
            // 
            // _pollMessageStatusBitMenuItem
            // 
            _pollMessageStatusBitMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _pollMessageStatusBitTextBox });
            _pollMessageStatusBitMenuItem.Name = "_PollMessageStatusBitMenuItem";
            _pollMessageStatusBitMenuItem.Size = new Size(193, 22);
            _pollMessageStatusBitMenuItem.Text = "Bitmask:";
            _pollMessageStatusBitMenuItem.ToolTipText = "Message is flagged as received when status byte message bit  is on";
            // 
            // _pollMessageStatusBitTextBox
            // 
            _pollMessageStatusBitTextBox.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _pollMessageStatusBitTextBox.Name = "_PollMessageStatusBitTextBox";
            _pollMessageStatusBitTextBox.Size = new Size(100, 22);
            _pollMessageStatusBitTextBox.ToolTipText = "Message status bit used to detect if a message is available for reading";
            // 
            // _pollCommandMenuItem
            // 
            _pollCommandMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _pollWriteCommandTextBox });
            _pollCommandMenuItem.Name = "_PollCommandMenuItem";
            _pollCommandMenuItem.Size = new Size(193, 22);
            _pollCommandMenuItem.Text = "Command:";
            _pollCommandMenuItem.ToolTipText = "Click Send Command to send this command to the instrument";
            // 
            // _pollWriteCommandTextBox
            // 
            _pollWriteCommandTextBox.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _pollWriteCommandTextBox.Name = "_PollWriteCommandTextBox";
            _pollWriteCommandTextBox.Size = new Size(100, 22);
            _pollWriteCommandTextBox.Text = @"*IDN?\n";
            _pollWriteCommandTextBox.ToolTipText = "Command to write";
            // 
            // _pollIntervalMenuItem
            // 
            _pollIntervalMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _pollIntervalTextBox });
            _pollIntervalMenuItem.Name = "_PollIntervalMenuItem";
            _pollIntervalMenuItem.Size = new Size(193, 22);
            _pollIntervalMenuItem.Text = "Interval:";
            // 
            // _pollIntervalTextBox
            // 
            _pollIntervalTextBox.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _pollIntervalTextBox.Name = "_PollIntervalTextBox";
            _pollIntervalTextBox.Size = new Size(100, 22);
            _pollIntervalTextBox.ToolTipText = "Sets the polling interval in seconds";
            // 
            // _pollEnabledMenuItem
            // 
            _pollEnabledMenuItem.Name = "_PollEnabledMenuItem";
            _pollEnabledMenuItem.Size = new Size(193, 22);
            _pollEnabledMenuItem.Text = "Press to Start";
            _pollEnabledMenuItem.ToolTipText = "Check to enable polling the instrument service request on a timer";
            _pollEnabledMenuItem.Click += new EventHandler( PollEnabledMenuItem_Click );
            // 
            // _pollAutoReadMenuItem
            // 
            _pollAutoReadMenuItem.CheckOnClick = true;
            _pollAutoReadMenuItem.Name = "_PollAutoReadMenuItem";
            _pollAutoReadMenuItem.Size = new Size(193, 22);
            _pollAutoReadMenuItem.Text = "Auto Read Enabled";
            _pollAutoReadMenuItem.ToolTipText = "Reads upon detecting the message available bit";
            // 
            // _pollSendMenuItem
            // 
            _pollSendMenuItem.Name = "_PollSendMenuItem";
            _pollSendMenuItem.Size = new Size(193, 22);
            _pollSendMenuItem.Text = "Send Command";
            _pollSendMenuItem.ToolTipText = "Click to send the poll command.  A command can also be sent from the Session pane" + "l.";
            _pollSendMenuItem.Click += new EventHandler( PollSendMenuItem_Click );
            // 
            // _serviceRequestSplitButton
            // 
            _serviceRequestSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _serviceRequestSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _serviceRequestBitmaskMenuItem, _serviceRequestEnableCommandMenuItem, _programServiceRequestEnableBitmaskMenuItem, _toggleVisaEventHandlerMenuItem, _serviceRequestHandlerAddRemoveMenuItem, _serviceRequestAutoReadMenuItem });
            _serviceRequestSplitButton.Image = (Image)resources.GetObject("_ServiceRequestSplitButton.Image");
            _serviceRequestSplitButton.ImageTransparentColor = Color.Magenta;
            _serviceRequestSplitButton.Name = "_ServiceRequestSplitButton";
            _serviceRequestSplitButton.Size = new Size(46, 28);
            _serviceRequestSplitButton.Text = "SRQ";
            // 
            // _serviceRequestBitmaskMenuItem
            // 
            _serviceRequestBitmaskMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _serviceRequestBitMaskTextBox });
            _serviceRequestBitmaskMenuItem.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _serviceRequestBitmaskMenuItem.Name = "_ServiceRequestBitmaskMenuItem";
            _serviceRequestBitmaskMenuItem.Size = new Size(232, 22);
            _serviceRequestBitmaskMenuItem.Text = "Events Enable Bitmask:";
            _serviceRequestBitmaskMenuItem.ToolTipText = "Specifies which instrument events invoke service request";
            // 
            // _serviceRequestBitMaskTextBox
            // 
            _serviceRequestBitMaskTextBox.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _serviceRequestBitMaskTextBox.Name = "_ServiceRequestBitMaskTextBox";
            _serviceRequestBitMaskTextBox.Size = new Size(100, 22);
            _serviceRequestBitMaskTextBox.ToolTipText = "Service request Bit Mask in Hex";
            // 
            // _serviceRequestEnableCommandMenuItem
            // 
            _serviceRequestEnableCommandMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _serviceRequestEnableCommandTextBox });
            _serviceRequestEnableCommandMenuItem.Name = "_ServiceRequestEnableCommandMenuItem";
            _serviceRequestEnableCommandMenuItem.Size = new Size(232, 22);
            _serviceRequestEnableCommandMenuItem.Text = "Events Enable Command:";
            // 
            // _serviceRequestEnableCommandTextBox
            // 
            _serviceRequestEnableCommandTextBox.Font = new Font("Segoe UI", 9.0f);
            _serviceRequestEnableCommandTextBox.Name = "_ServiceRequestEnableCommandTextBox";
            _serviceRequestEnableCommandTextBox.Size = new Size(100, 22);
            _serviceRequestEnableCommandTextBox.ToolTipText = "Enter the service request enable command, e.g., *SRE {0:D}\" ";
            // 
            // _programServiceRequestEnableBitmaskMenuItem
            // 
            _programServiceRequestEnableBitmaskMenuItem.Name = "_ProgramServiceRequestEnableBitmaskMenuItem";
            _programServiceRequestEnableBitmaskMenuItem.Size = new Size(232, 22);
            _programServiceRequestEnableBitmaskMenuItem.Text = "Apply SRQ Bitmask";
            _programServiceRequestEnableBitmaskMenuItem.ToolTipText = "Sets the service request enable bitmask";
            _programServiceRequestEnableBitmaskMenuItem.Click += new EventHandler( ProgramServiceRequestEnableBitmaskMenuItem_Click );
            // 
            // _toggleVisaEventHandlerMenuItem
            // 
            _toggleVisaEventHandlerMenuItem.Name = "_ToggleVisaEventHandlerMenuItem";
            _toggleVisaEventHandlerMenuItem.Size = new Size(232, 22);
            _toggleVisaEventHandlerMenuItem.Text = "Toggle Visa Events";
            _toggleVisaEventHandlerMenuItem.Click += new EventHandler( ToggleVisaEventHandlerMenuItem_CheckStateChanged );
            // 
            // _serviceRequestHandlerAddRemoveMenuItem
            // 
            _serviceRequestHandlerAddRemoveMenuItem.Name = "_ServiceRequestHandlerAddRemoveMenuItem";
            _serviceRequestHandlerAddRemoveMenuItem.Size = new Size(232, 22);
            _serviceRequestHandlerAddRemoveMenuItem.Text = "Toggle SRQ Handling";
            _serviceRequestHandlerAddRemoveMenuItem.ToolTipText = "Check to handle device service request";
            _serviceRequestHandlerAddRemoveMenuItem.Click += new EventHandler( ServiceRequestHandlerAddRemoveMenuItem_CheckStateChanged );
            // 
            // _serviceRequestAutoReadMenuItem
            // 
            _serviceRequestAutoReadMenuItem.CheckOnClick = true;
            _serviceRequestAutoReadMenuItem.Name = "_ServiceRequestAutoReadMenuItem";
            _serviceRequestAutoReadMenuItem.Size = new Size(232, 22);
            _serviceRequestAutoReadMenuItem.Text = "Auto Read Enabled";
            _serviceRequestAutoReadMenuItem.ToolTipText = "Enables reading messages when service request is enabled";
            // 
            // EditResourceNamesMenuItem
            // 
            EditResourceNamesMenuItem.Name = "_EditResourceNamesMenuItem";
            EditResourceNamesMenuItem.Size = new Size(256, 22);
            EditResourceNamesMenuItem.Text = "Edit Resource Names";
            EditResourceNamesMenuItem.Click += new EventHandler( EditResourceNamesMenuItem_Click );
            // 
            // StatusView
            // 
            BackColor = Color.Transparent;
            Controls.Add(_toolStrip);
            Name = "StatusView";
            Size = new Size(474, 31);
            _toolStrip.ResumeLayout(false);
            _toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStrip _toolStrip;
        private cc.isr.WinControls.ToolStripDropDownButton _deviceDropDownButton;
        private ToolStripMenuItem _resetMenuItem;
        private cc.isr.WinControls.ToolStripMenuItem _clearInterfaceMenuItem;
        private ToolStripMenuItem _clearDeviceMenuItem;
        private ToolStripMenuItem _resetKnownStateMenuItem;
        private ToolStripMenuItem _initKnownStateMenuItem;
        private ToolStripMenuItem _clearExecutionStateMenuItem;
        private cc.isr.WinControls.ToolStripDropDownButton _sessionDownDownButton;
        private cc.isr.WinControls.ToolStripMenuItem _readTerminationEnabledMenuItem;
        private ToolStripMenuItem _writeTerminationMenuItem;
        private cc.isr.WinControls.ToolStripTextBox _writeTerminationTextBox;
        private ToolStripMenuItem _readTerminationMenuItem;
        private cc.isr.WinControls.ToolStripTextBox _readTerminationTextBox;
        private ToolStripMenuItem _sessionTimeoutMenuItem;
        private cc.isr.WinControls.ToolStripTextBox _sessionTimeoutTextBox;
        private ToolStripMenuItem _storeTimeoutMenuItem;
        private cc.isr.WinControls.ToolStripDropDownButton _pollSplitButton;
        private ToolStripMenuItem _pollCommandMenuItem;
        private cc.isr.WinControls.ToolStripTextBox _pollWriteCommandTextBox;
        private ToolStripMenuItem _pollIntervalMenuItem;
        private cc.isr.WinControls.ToolStripTextBox _pollIntervalTextBox;
        private ToolStripMenuItem _pollMessageStatusBitMenuItem;
        private cc.isr.WinControls.ToolStripTextBox _pollMessageStatusBitTextBox;
        private ToolStripMenuItem _sessionReadStatusByteMenuItem;
        private ToolStripMenuItem _restoreTimeoutMenuItem;
        private ToolStripDropDownButton _applicationDropDownButton;
        private ToolStripMenuItem _traceMenuItem;
        private ToolStripMenuItem _traceLogLevelMenuItem;
        private cc.isr.WinControls.ToolStripComboBox _traceLogLevelComboBox;
        private ToolStripMenuItem _traceShowLevelMenuItem;
        private cc.isr.WinControls.ToolStripComboBox _traceShowLevelComboBox;
        private ToolStripMenuItem _sessionNotificationLevelMenuItem;
        private cc.isr.WinControls.ToolStripComboBox _sessionNotificationLevelComboBox;
        private cc.isr.WinControls.ToolStripMenuItem _usingStatusSubsystemMenuItem;
        private ToolStripMenuItem _clearErrorReportMenuItem;
        private ToolStripMenuItem _readDeviceErrorsMenuItem;
        private ToolStripMenuItem _sessionReadStandardEventRegisterMenuItem;
        private ToolStripMenuItem _pollSendMenuItem;
        private cc.isr.WinControls.ToolStripMenuItem _pollEnabledMenuItem;
        private cc.isr.WinControls.ToolStripMenuItem _pollAutoReadMenuItem;
        private cc.isr.WinControls.ToolStripDropDownButton _serviceRequestSplitButton;
        private cc.isr.WinControls.ToolStripMenuItem _serviceRequestBitmaskMenuItem;
        private cc.isr.WinControls.ToolStripTextBox _serviceRequestBitMaskTextBox;
        private ToolStripMenuItem _serviceRequestEnableCommandMenuItem;
        private cc.isr.WinControls.ToolStripTextBox _serviceRequestEnableCommandTextBox;
        private cc.isr.WinControls.ToolStripMenuItem _programServiceRequestEnableBitmaskMenuItem;
        private cc.isr.WinControls.ToolStripMenuItem _serviceRequestHandlerAddRemoveMenuItem;
        private cc.isr.WinControls.ToolStripMenuItem _serviceRequestAutoReadMenuItem;
        private ToolStripMenuItem _readTerminalsStateMenuItem;
        private cc.isr.WinControls.ToolStripMenuItem _toggleVisaEventHandlerMenuItem;
        private ToolStripMenuItem _sendBusTriggerMenuItem;
        private ToolStripMenuItem EditSettingsToolStripMenuItem;
        private ToolStripMenuItem _sessionSettingsMenuItem;
        private ToolStripMenuItem _deviceSettingsMenuItem;
        private ToolStripMenuItem _userInterfaceSettingsMenuItem;
        private ToolStripMenuItem _uIBaseSettingsMenuItem;
        private ToolStripMenuItem EditResourceNamesMenuItem;
    }
}
