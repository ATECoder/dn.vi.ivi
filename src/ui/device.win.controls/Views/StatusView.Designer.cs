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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._applicationDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.EditSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sessionSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._deviceSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._userInterfaceSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._uIBaseSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._traceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._traceLogLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._traceLogLevelComboBox = new cc.isr.WinControls.ToolStripComboBox();
            this._traceShowLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._traceShowLevelComboBox = new cc.isr.WinControls.ToolStripComboBox();
            this._sessionNotificationLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sessionNotificationLevelComboBox = new cc.isr.WinControls.ToolStripComboBox();
            this._usingStatusSubsystemMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this.EditResourceNamesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._deviceDropDownButton = new cc.isr.WinControls.ToolStripDropDownButton();
            this._resetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._clearInterfaceMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._clearDeviceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._resetKnownStateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._clearExecutionStateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._initKnownStateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._clearErrorReportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._readDeviceErrorsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._readTerminalsStateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sessionDownDownButton = new cc.isr.WinControls.ToolStripDropDownButton();
            this._readTerminationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._readTerminationTextBox = new cc.isr.WinControls.ToolStripTextBox();
            this._readTerminationEnabledMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._writeTerminationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._writeTerminationTextBox = new cc.isr.WinControls.ToolStripTextBox();
            this._sessionTimeoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sessionTimeoutTextBox = new cc.isr.WinControls.ToolStripTextBox();
            this._storeTimeoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._restoreTimeoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sessionReadStatusByteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sessionReadStandardEventRegisterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._sendBusTriggerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pollSplitButton = new cc.isr.WinControls.ToolStripDropDownButton();
            this._pollMessageStatusBitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pollMessageStatusBitTextBox = new cc.isr.WinControls.ToolStripTextBox();
            this._pollCommandMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pollWriteCommandTextBox = new cc.isr.WinControls.ToolStripTextBox();
            this._pollIntervalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._pollIntervalTextBox = new cc.isr.WinControls.ToolStripTextBox();
            this._pollEnabledMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._pollAutoReadMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._pollSendMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._serviceRequestSplitButton = new cc.isr.WinControls.ToolStripDropDownButton();
            this._serviceRequestBitmaskMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._serviceRequestBitMaskTextBox = new cc.isr.WinControls.ToolStripTextBox();
            this._serviceRequestEnableCommandMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._serviceRequestEnableCommandTextBox = new cc.isr.WinControls.ToolStripTextBox();
            this._programServiceRequestEnableBitmaskMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._toggleVisaEventHandlerMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._serviceRequestHandlerAddRemoveMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._serviceRequestAutoReadMenuItem = new cc.isr.WinControls.ToolStripMenuItem();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.Transparent;
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this._toolStrip.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._toolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._applicationDropDownButton,
            this._deviceDropDownButton,
            this._sessionDownDownButton,
            this._pollSplitButton,
            this._serviceRequestSplitButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(474, 31);
            this._toolStrip.TabIndex = 12;
            // 
            // _applicationDropDownButton
            // 
            this._applicationDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._applicationDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditSettingsToolStripMenuItem,
            this._traceMenuItem,
            this._sessionNotificationLevelMenuItem,
            this._usingStatusSubsystemMenuItem,
            this.EditResourceNamesMenuItem});
            this._applicationDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._applicationDropDownButton.Name = "_applicationDropDownButton";
            this._applicationDropDownButton.Size = new System.Drawing.Size(46, 28);
            this._applicationDropDownButton.Text = "App";
            this._applicationDropDownButton.ToolTipText = "Application options";
            // 
            // EditSettingsToolStripMenuItem
            // 
            this.EditSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._sessionSettingsMenuItem,
            this._deviceSettingsMenuItem,
            this._userInterfaceSettingsMenuItem,
            this._uIBaseSettingsMenuItem});
            this.EditSettingsToolStripMenuItem.Name = "EditSettingsToolStripMenuItem";
            this.EditSettingsToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.EditSettingsToolStripMenuItem.Text = "Edit Settings";
            // 
            // _sessionSettingsMenuItem
            // 
            this._sessionSettingsMenuItem.Name = "_sessionSettingsMenuItem";
            this._sessionSettingsMenuItem.Size = new System.Drawing.Size(134, 22);
            this._sessionSettingsMenuItem.Text = "Session...";
            this._sessionSettingsMenuItem.ToolTipText = "Opens session settings dialog";
            this._sessionSettingsMenuItem.Click += new System.EventHandler(this.SessionSettingsMenuItem_Click);
            // 
            // _deviceSettingsMenuItem
            // 
            this._deviceSettingsMenuItem.Name = "_deviceSettingsMenuItem";
            this._deviceSettingsMenuItem.Size = new System.Drawing.Size(134, 22);
            this._deviceSettingsMenuItem.Text = "Device...";
            this._deviceSettingsMenuItem.ToolTipText = "Opens device settings editor dialog";
            this._deviceSettingsMenuItem.Click += new System.EventHandler(this.DeviceSettingsMenuItem_Click);
            // 
            // _userInterfaceSettingsMenuItem
            // 
            this._userInterfaceSettingsMenuItem.Name = "_userInterfaceSettingsMenuItem";
            this._userInterfaceSettingsMenuItem.Size = new System.Drawing.Size(134, 22);
            this._userInterfaceSettingsMenuItem.Text = "UI...";
            this._userInterfaceSettingsMenuItem.ToolTipText = "Opens User Interface settings editor dialog";
            this._userInterfaceSettingsMenuItem.Click += new System.EventHandler(this.UserInterfaceToolStripMenuItem_Click);
            // 
            // _uIBaseSettingsMenuItem
            // 
            this._uIBaseSettingsMenuItem.Name = "_uIBaseSettingsMenuItem";
            this._uIBaseSettingsMenuItem.Size = new System.Drawing.Size(134, 22);
            this._uIBaseSettingsMenuItem.Text = "UI Base...";
            this._uIBaseSettingsMenuItem.ToolTipText = "Edits the user interface base settings";
            this._uIBaseSettingsMenuItem.Click += new System.EventHandler(this.UIBaseSettingsMenuItem_Click);
            // 
            // _traceMenuItem
            // 
            this._traceMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._traceLogLevelMenuItem,
            this._traceShowLevelMenuItem});
            this._traceMenuItem.Name = "_traceMenuItem";
            this._traceMenuItem.Size = new System.Drawing.Size(256, 22);
            this._traceMenuItem.Text = "Select Trace Levels";
            this._traceMenuItem.ToolTipText = "Opens the trace menus";
            // 
            // _traceLogLevelMenuItem
            // 
            this._traceLogLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._traceLogLevelComboBox});
            this._traceLogLevelMenuItem.Name = "_traceLogLevelMenuItem";
            this._traceLogLevelMenuItem.Size = new System.Drawing.Size(194, 22);
            this._traceLogLevelMenuItem.Text = "Trace Log Level";
            this._traceLogLevelMenuItem.ToolTipText = "Selects trace level for logging";
            // 
            // _traceLogLevelComboBox
            // 
            this._traceLogLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._traceLogLevelComboBox.Name = "_traceLogLevelComboBox";
            this._traceLogLevelComboBox.Size = new System.Drawing.Size(100, 22);
            this._traceLogLevelComboBox.ToolTipText = "Selects trace level for logging";
            // 
            // _traceShowLevelMenuItem
            // 
            this._traceShowLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._traceShowLevelComboBox});
            this._traceShowLevelMenuItem.Name = "_traceShowLevelMenuItem";
            this._traceShowLevelMenuItem.Size = new System.Drawing.Size(194, 22);
            this._traceShowLevelMenuItem.Text = "Display Trace Level";
            this._traceShowLevelMenuItem.ToolTipText = "Selects trace level for display";
            // 
            // _traceShowLevelComboBox
            // 
            this._traceShowLevelComboBox.Name = "_traceShowLevelComboBox";
            this._traceShowLevelComboBox.Size = new System.Drawing.Size(100, 22);
            this._traceShowLevelComboBox.ToolTipText = "Selects trace level for display";
            // 
            // _sessionNotificationLevelMenuItem
            // 
            this._sessionNotificationLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._sessionNotificationLevelComboBox});
            this._sessionNotificationLevelMenuItem.Name = "_sessionNotificationLevelMenuItem";
            this._sessionNotificationLevelMenuItem.Size = new System.Drawing.Size(256, 22);
            this._sessionNotificationLevelMenuItem.Text = "Session Notification Level";
            this._sessionNotificationLevelMenuItem.ToolTipText = "Shows the session notification level selector";
            // 
            // _sessionNotificationLevelComboBox
            // 
            this._sessionNotificationLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._sessionNotificationLevelComboBox.Name = "_sessionNotificationLevelComboBox";
            this._sessionNotificationLevelComboBox.Size = new System.Drawing.Size(100, 22);
            this._sessionNotificationLevelComboBox.ToolTipText = "Select the session notification level";
            // 
            // _usingStatusSubsystemMenuItem
            // 
            this._usingStatusSubsystemMenuItem.CheckOnClick = true;
            this._usingStatusSubsystemMenuItem.Name = "_usingStatusSubsystemMenuItem";
            this._usingStatusSubsystemMenuItem.Size = new System.Drawing.Size(256, 22);
            this._usingStatusSubsystemMenuItem.Text = "Using Status Subsystem Only";
            this._usingStatusSubsystemMenuItem.ToolTipText = "Toggle to use status or all subsystems";
            this._usingStatusSubsystemMenuItem.CheckStateChanged += new System.EventHandler(this.OpenStatusSubsystemMenuItem_CheckStateChanged);
            // 
            // EditResourceNamesMenuItem
            // 
            this.EditResourceNamesMenuItem.Name = "EditResourceNamesMenuItem";
            this.EditResourceNamesMenuItem.Size = new System.Drawing.Size(256, 22);
            this.EditResourceNamesMenuItem.Text = "Edit Resource Names";
            this.EditResourceNamesMenuItem.Click += new System.EventHandler(this.EditResourceNamesMenuItem_Click);
            // 
            // _deviceDropDownButton
            // 
            this._deviceDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._deviceDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._resetMenuItem,
            this._clearErrorReportMenuItem,
            this._readDeviceErrorsMenuItem,
            this._readTerminalsStateMenuItem});
            this._deviceDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._deviceDropDownButton.Name = "_deviceDropDownButton";
            this._deviceDropDownButton.Size = new System.Drawing.Size(62, 28);
            this._deviceDropDownButton.Text = "Device";
            this._deviceDropDownButton.ToolTipText = "Device resets, Service Requests and trace management";
            // 
            // _resetMenuItem
            // 
            this._resetMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._clearInterfaceMenuItem,
            this._clearDeviceMenuItem,
            this._resetKnownStateMenuItem,
            this._clearExecutionStateMenuItem,
            this._initKnownStateMenuItem});
            this._resetMenuItem.Name = "_resetMenuItem";
            this._resetMenuItem.Size = new System.Drawing.Size(205, 22);
            this._resetMenuItem.Text = "Reset...";
            this._resetMenuItem.ToolTipText = "Opens the reset menus";
            // 
            // _clearInterfaceMenuItem
            // 
            this._clearInterfaceMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._clearInterfaceMenuItem.Name = "_clearInterfaceMenuItem";
            this._clearInterfaceMenuItem.Size = new System.Drawing.Size(242, 22);
            this._clearInterfaceMenuItem.Text = "Clear Interface";
            this._clearInterfaceMenuItem.ToolTipText = "Issues an interface clear command";
            this._clearInterfaceMenuItem.Click += new System.EventHandler(this.ClearInterfaceMenuItem_Click);
            // 
            // _clearDeviceMenuItem
            // 
            this._clearDeviceMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._clearDeviceMenuItem.Name = "_clearDeviceMenuItem";
            this._clearDeviceMenuItem.Size = new System.Drawing.Size(242, 22);
            this._clearDeviceMenuItem.Text = "Clear Device (SDC)";
            this._clearDeviceMenuItem.ToolTipText = "Issues Selective Device Clear";
            this._clearDeviceMenuItem.Click += new System.EventHandler(this.ClearDeviceMenuItem_Click);
            // 
            // _resetKnownStateMenuItem
            // 
            this._resetKnownStateMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._resetKnownStateMenuItem.Name = "_resetKnownStateMenuItem";
            this._resetKnownStateMenuItem.Size = new System.Drawing.Size(242, 22);
            this._resetKnownStateMenuItem.Text = "Reset Known State (RST)";
            this._resetKnownStateMenuItem.ToolTipText = "Issues *RST";
            this._resetKnownStateMenuItem.Click += new System.EventHandler(this.ResetKnownStateMenuItem_Click);
            // 
            // _clearExecutionStateMenuItem
            // 
            this._clearExecutionStateMenuItem.Name = "_clearExecutionStateMenuItem";
            this._clearExecutionStateMenuItem.Size = new System.Drawing.Size(242, 22);
            this._clearExecutionStateMenuItem.Text = "Clear Execution State (CLS)";
            this._clearExecutionStateMenuItem.ToolTipText = "Clears execution state (CLS)";
            this._clearExecutionStateMenuItem.Click += new System.EventHandler(this.ClearExecutionStateMenuItem_Click);
            // 
            // _initKnownStateMenuItem
            // 
            this._initKnownStateMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._initKnownStateMenuItem.Name = "_initKnownStateMenuItem";
            this._initKnownStateMenuItem.Size = new System.Drawing.Size(242, 22);
            this._initKnownStateMenuItem.Text = "Initialize Known State";
            this._initKnownStateMenuItem.ToolTipText = "Issues *RST, CLear and initialize to custom known state";
            this._initKnownStateMenuItem.Click += new System.EventHandler(this.InitKnownStateMenuItem_Click);
            // 
            // _clearErrorReportMenuItem
            // 
            this._clearErrorReportMenuItem.Name = "_clearErrorReportMenuItem";
            this._clearErrorReportMenuItem.Size = new System.Drawing.Size(205, 22);
            this._clearErrorReportMenuItem.Text = "Clear Error Report";
            this._clearErrorReportMenuItem.ToolTipText = "Clears the error report";
            this._clearErrorReportMenuItem.Click += new System.EventHandler(this.ClearErrorReportMenuItem_Click);
            // 
            // _readDeviceErrorsMenuItem
            // 
            this._readDeviceErrorsMenuItem.Name = "_readDeviceErrorsMenuItem";
            this._readDeviceErrorsMenuItem.Size = new System.Drawing.Size(205, 22);
            this._readDeviceErrorsMenuItem.Text = "Read Device Errors";
            this._readDeviceErrorsMenuItem.ToolTipText = "Reads device errors";
            this._readDeviceErrorsMenuItem.Click += new System.EventHandler(this.ReadDeviceErrorsMenuItem_Click);
            // 
            // _readTerminalsStateMenuItem
            // 
            this._readTerminalsStateMenuItem.Name = "_readTerminalsStateMenuItem";
            this._readTerminalsStateMenuItem.Size = new System.Drawing.Size(205, 22);
            this._readTerminalsStateMenuItem.Text = "Read Terminals State";
            this._readTerminalsStateMenuItem.Click += new System.EventHandler(this.ReadTerminalsStateMenuItem_Click);
            // 
            // _sessionDownDownButton
            // 
            this._sessionDownDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._sessionDownDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._readTerminationMenuItem,
            this._writeTerminationMenuItem,
            this._sessionTimeoutMenuItem,
            this._sessionReadStatusByteMenuItem,
            this._sessionReadStandardEventRegisterMenuItem,
            this._sendBusTriggerMenuItem});
            this._sessionDownDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._sessionDownDownButton.Name = "_sessionDownDownButton";
            this._sessionDownDownButton.Size = new System.Drawing.Size(67, 28);
            this._sessionDownDownButton.Text = "Session";
            this._sessionDownDownButton.ToolTipText = "Select Session Options";
            // 
            // _readTerminationMenuItem
            // 
            this._readTerminationMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._readTerminationTextBox,
            this._readTerminationEnabledMenuItem});
            this._readTerminationMenuItem.Name = "_readTerminationMenuItem";
            this._readTerminationMenuItem.Size = new System.Drawing.Size(257, 22);
            this._readTerminationMenuItem.Text = "Read Termination";
            this._readTerminationMenuItem.ToolTipText = "Read termination. Typically Line Feed (10)";
            // 
            // _readTerminationTextBox
            // 
            this._readTerminationTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._readTerminationTextBox.Name = "_readTerminationTextBox";
            this._readTerminationTextBox.Size = new System.Drawing.Size(100, 22);
            this._readTerminationTextBox.ToolTipText = "Read termination in Hex";
            // 
            // _readTerminationEnabledMenuItem
            // 
            this._readTerminationEnabledMenuItem.CheckOnClick = true;
            this._readTerminationEnabledMenuItem.Name = "_readTerminationEnabledMenuItem";
            this._readTerminationEnabledMenuItem.Size = new System.Drawing.Size(160, 22);
            this._readTerminationEnabledMenuItem.Text = "Enabled";
            this._readTerminationEnabledMenuItem.ToolTipText = "Checked to enable read termination";
            // 
            // _writeTerminationMenuItem
            // 
            this._writeTerminationMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._writeTerminationTextBox});
            this._writeTerminationMenuItem.Name = "_writeTerminationMenuItem";
            this._writeTerminationMenuItem.Size = new System.Drawing.Size(257, 22);
            this._writeTerminationMenuItem.Text = "Write Termination";
            this._writeTerminationMenuItem.ToolTipText = "Write termination; typically \\n (line feed)";
            // 
            // _writeTerminationTextBox
            // 
            this._writeTerminationTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._writeTerminationTextBox.Name = "_writeTerminationTextBox";
            this._writeTerminationTextBox.Size = new System.Drawing.Size(100, 22);
            this._writeTerminationTextBox.ToolTipText = "Write termination escape sequence, e.g., \\n\\r";
            // 
            // _sessionTimeoutMenuItem
            // 
            this._sessionTimeoutMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._sessionTimeoutTextBox,
            this._storeTimeoutMenuItem,
            this._restoreTimeoutMenuItem});
            this._sessionTimeoutMenuItem.Name = "_sessionTimeoutMenuItem";
            this._sessionTimeoutMenuItem.Size = new System.Drawing.Size(257, 22);
            this._sessionTimeoutMenuItem.Text = "Timeout";
            // 
            // _sessionTimeoutTextBox
            // 
            this._sessionTimeoutTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._sessionTimeoutTextBox.Name = "_sessionTimeoutTextBox";
            this._sessionTimeoutTextBox.Size = new System.Drawing.Size(100, 22);
            this._sessionTimeoutTextBox.ToolTipText = "Timeout in seconds";
            // 
            // _storeTimeoutMenuItem
            // 
            this._storeTimeoutMenuItem.Name = "_storeTimeoutMenuItem";
            this._storeTimeoutMenuItem.Size = new System.Drawing.Size(172, 22);
            this._storeTimeoutMenuItem.Text = "Store and Set";
            this._storeTimeoutMenuItem.ToolTipText = "Pushes the previous timeout onto the stack and set a new timeout value";
            this._storeTimeoutMenuItem.Click += new System.EventHandler(this.StoreTimeoutMenuItem_Click);
            // 
            // _restoreTimeoutMenuItem
            // 
            this._restoreTimeoutMenuItem.Name = "_restoreTimeoutMenuItem";
            this._restoreTimeoutMenuItem.Size = new System.Drawing.Size(172, 22);
            this._restoreTimeoutMenuItem.Text = "Restore and Set";
            this._restoreTimeoutMenuItem.ToolTipText = "Pulls the last timeout from the stack and sets it";
            this._restoreTimeoutMenuItem.Click += new System.EventHandler(this.RestoreTimeoutMenuItem_Click);
            // 
            // _sessionReadStatusByteMenuItem
            // 
            this._sessionReadStatusByteMenuItem.Name = "_sessionReadStatusByteMenuItem";
            this._sessionReadStatusByteMenuItem.Size = new System.Drawing.Size(257, 22);
            this._sessionReadStatusByteMenuItem.Text = "Read Status Register";
            this._sessionReadStatusByteMenuItem.ToolTipText = "Reads the session status byte";
            this._sessionReadStatusByteMenuItem.Click += new System.EventHandler(this.SessionReadStatusByteMenuItem_Click);
            // 
            // _sessionReadStandardEventRegisterMenuItem
            // 
            this._sessionReadStandardEventRegisterMenuItem.Name = "_sessionReadStandardEventRegisterMenuItem";
            this._sessionReadStandardEventRegisterMenuItem.Size = new System.Drawing.Size(257, 22);
            this._sessionReadStandardEventRegisterMenuItem.Text = "Read Standard Event Register";
            this._sessionReadStandardEventRegisterMenuItem.ToolTipText = "Reads the standard event register";
            this._sessionReadStandardEventRegisterMenuItem.Click += new System.EventHandler(this.SessionReadStandardEventRegisterMenuItem_Click);
            // 
            // _sendBusTriggerMenuItem
            // 
            this._sendBusTriggerMenuItem.Name = "_sendBusTriggerMenuItem";
            this._sendBusTriggerMenuItem.Size = new System.Drawing.Size(257, 22);
            this._sendBusTriggerMenuItem.Text = "Send Bus Trigger";
            this._sendBusTriggerMenuItem.ToolTipText = "Sends a bus trigger to the instrument";
            this._sendBusTriggerMenuItem.Click += new System.EventHandler(this.SendBusTrigger_Click);
            // 
            // _pollSplitButton
            // 
            this._pollSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._pollSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._pollMessageStatusBitMenuItem,
            this._pollCommandMenuItem,
            this._pollIntervalMenuItem,
            this._pollEnabledMenuItem,
            this._pollAutoReadMenuItem,
            this._pollSendMenuItem});
            this._pollSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._pollSplitButton.Name = "_pollSplitButton";
            this._pollSplitButton.Size = new System.Drawing.Size(45, 28);
            this._pollSplitButton.Text = "Poll";
            // 
            // _pollMessageStatusBitMenuItem
            // 
            this._pollMessageStatusBitMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._pollMessageStatusBitTextBox});
            this._pollMessageStatusBitMenuItem.Name = "_pollMessageStatusBitMenuItem";
            this._pollMessageStatusBitMenuItem.Size = new System.Drawing.Size(193, 22);
            this._pollMessageStatusBitMenuItem.Text = "Bitmask:";
            this._pollMessageStatusBitMenuItem.ToolTipText = "Message is flagged as received when status byte message bit  is on";
            // 
            // _pollMessageStatusBitTextBox
            // 
            this._pollMessageStatusBitTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._pollMessageStatusBitTextBox.Name = "_pollMessageStatusBitTextBox";
            this._pollMessageStatusBitTextBox.Size = new System.Drawing.Size(100, 22);
            this._pollMessageStatusBitTextBox.ToolTipText = "Message status bit used to detect if a message is available for reading";
            // 
            // _pollCommandMenuItem
            // 
            this._pollCommandMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._pollWriteCommandTextBox});
            this._pollCommandMenuItem.Name = "_pollCommandMenuItem";
            this._pollCommandMenuItem.Size = new System.Drawing.Size(193, 22);
            this._pollCommandMenuItem.Text = "Command:";
            this._pollCommandMenuItem.ToolTipText = "Click Send Command to send this command to the instrument";
            // 
            // _pollWriteCommandTextBox
            // 
            this._pollWriteCommandTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._pollWriteCommandTextBox.Name = "_pollWriteCommandTextBox";
            this._pollWriteCommandTextBox.Size = new System.Drawing.Size(100, 22);
            this._pollWriteCommandTextBox.Text = "*IDN?\\n";
            this._pollWriteCommandTextBox.ToolTipText = "Command to write";
            // 
            // _pollIntervalMenuItem
            // 
            this._pollIntervalMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._pollIntervalTextBox});
            this._pollIntervalMenuItem.Name = "_pollIntervalMenuItem";
            this._pollIntervalMenuItem.Size = new System.Drawing.Size(193, 22);
            this._pollIntervalMenuItem.Text = "Interval:";
            // 
            // _pollIntervalTextBox
            // 
            this._pollIntervalTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._pollIntervalTextBox.Name = "_pollIntervalTextBox";
            this._pollIntervalTextBox.Size = new System.Drawing.Size(100, 22);
            this._pollIntervalTextBox.ToolTipText = "Sets the polling interval in seconds";
            // 
            // _pollEnabledMenuItem
            // 
            this._pollEnabledMenuItem.Name = "_pollEnabledMenuItem";
            this._pollEnabledMenuItem.Size = new System.Drawing.Size(193, 22);
            this._pollEnabledMenuItem.Text = "Press to Start";
            this._pollEnabledMenuItem.ToolTipText = "Check to enable polling the instrument service request on a timer";
            this._pollEnabledMenuItem.CheckStateChanged += new System.EventHandler(this.PollEnabledMenuItem_CheckStateChanged);
            this._pollEnabledMenuItem.Click += new System.EventHandler(this.PollEnabledMenuItem_Click);
            // 
            // _pollAutoReadMenuItem
            // 
            this._pollAutoReadMenuItem.CheckOnClick = true;
            this._pollAutoReadMenuItem.Name = "_pollAutoReadMenuItem";
            this._pollAutoReadMenuItem.Size = new System.Drawing.Size(193, 22);
            this._pollAutoReadMenuItem.Text = "Auto Read Enabled";
            this._pollAutoReadMenuItem.ToolTipText = "Reads upon detecting the message available bit";
            // 
            // _pollSendMenuItem
            // 
            this._pollSendMenuItem.Name = "_pollSendMenuItem";
            this._pollSendMenuItem.Size = new System.Drawing.Size(193, 22);
            this._pollSendMenuItem.Text = "Send Command";
            this._pollSendMenuItem.ToolTipText = "Click to send the poll command.  A command can also be sent from the Session pane" +
    "l.";
            this._pollSendMenuItem.Click += new System.EventHandler(this.PollSendMenuItem_Click);
            // 
            // _serviceRequestSplitButton
            // 
            this._serviceRequestSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._serviceRequestSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._serviceRequestBitmaskMenuItem,
            this._serviceRequestEnableCommandMenuItem,
            this._programServiceRequestEnableBitmaskMenuItem,
            this._toggleVisaEventHandlerMenuItem,
            this._serviceRequestHandlerAddRemoveMenuItem,
            this._serviceRequestAutoReadMenuItem});
            this._serviceRequestSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._serviceRequestSplitButton.Name = "_serviceRequestSplitButton";
            this._serviceRequestSplitButton.Size = new System.Drawing.Size(46, 28);
            this._serviceRequestSplitButton.Text = "SRQ";
            // 
            // _serviceRequestBitmaskMenuItem
            // 
            this._serviceRequestBitmaskMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._serviceRequestBitMaskTextBox});
            this._serviceRequestBitmaskMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._serviceRequestBitmaskMenuItem.Name = "_serviceRequestBitmaskMenuItem";
            this._serviceRequestBitmaskMenuItem.Size = new System.Drawing.Size(232, 22);
            this._serviceRequestBitmaskMenuItem.Text = "Events Enable Bitmask:";
            this._serviceRequestBitmaskMenuItem.ToolTipText = "Specifies which instrument events invoke service request";
            // 
            // _serviceRequestBitMaskTextBox
            // 
            this._serviceRequestBitMaskTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._serviceRequestBitMaskTextBox.Name = "_serviceRequestBitMaskTextBox";
            this._serviceRequestBitMaskTextBox.Size = new System.Drawing.Size(100, 22);
            this._serviceRequestBitMaskTextBox.ToolTipText = "Service request Bit Mask in Hex";
            // 
            // _serviceRequestEnableCommandMenuItem
            // 
            this._serviceRequestEnableCommandMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._serviceRequestEnableCommandTextBox});
            this._serviceRequestEnableCommandMenuItem.Name = "_serviceRequestEnableCommandMenuItem";
            this._serviceRequestEnableCommandMenuItem.Size = new System.Drawing.Size(232, 22);
            this._serviceRequestEnableCommandMenuItem.Text = "Events Enable Command:";
            // 
            // _serviceRequestEnableCommandTextBox
            // 
            this._serviceRequestEnableCommandTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._serviceRequestEnableCommandTextBox.Name = "_serviceRequestEnableCommandTextBox";
            this._serviceRequestEnableCommandTextBox.Size = new System.Drawing.Size(100, 22);
            this._serviceRequestEnableCommandTextBox.ToolTipText = "Enter the service request enable command, e.g., *SRE {0:D}\" ";
            // 
            // _programServiceRequestEnableBitmaskMenuItem
            // 
            this._programServiceRequestEnableBitmaskMenuItem.Name = "_programServiceRequestEnableBitmaskMenuItem";
            this._programServiceRequestEnableBitmaskMenuItem.Size = new System.Drawing.Size(232, 22);
            this._programServiceRequestEnableBitmaskMenuItem.Text = "Apply SRQ Bitmask";
            this._programServiceRequestEnableBitmaskMenuItem.ToolTipText = "Sets the service request enable bitmask";
            this._programServiceRequestEnableBitmaskMenuItem.Click += new System.EventHandler(this.ProgramServiceRequestEnableBitmaskMenuItem_Click);
            // 
            // _toggleVisaEventHandlerMenuItem
            // 
            this._toggleVisaEventHandlerMenuItem.Name = "_toggleVisaEventHandlerMenuItem";
            this._toggleVisaEventHandlerMenuItem.Size = new System.Drawing.Size(232, 22);
            this._toggleVisaEventHandlerMenuItem.Text = "Toggle Visa Events";
            this._toggleVisaEventHandlerMenuItem.Click += new System.EventHandler(this.ToggleVisaEventHandlerMenuItem_CheckStateChanged);
            // 
            // _serviceRequestHandlerAddRemoveMenuItem
            // 
            this._serviceRequestHandlerAddRemoveMenuItem.Name = "_serviceRequestHandlerAddRemoveMenuItem";
            this._serviceRequestHandlerAddRemoveMenuItem.Size = new System.Drawing.Size(232, 22);
            this._serviceRequestHandlerAddRemoveMenuItem.Text = "Toggle SRQ Handling";
            this._serviceRequestHandlerAddRemoveMenuItem.ToolTipText = "Check to handle device service request";
            this._serviceRequestHandlerAddRemoveMenuItem.Click += new System.EventHandler(this.ServiceRequestHandlerAddRemoveMenuItem_CheckStateChanged);
            // 
            // _serviceRequestAutoReadMenuItem
            // 
            this._serviceRequestAutoReadMenuItem.CheckOnClick = true;
            this._serviceRequestAutoReadMenuItem.Name = "_serviceRequestAutoReadMenuItem";
            this._serviceRequestAutoReadMenuItem.Size = new System.Drawing.Size(232, 22);
            this._serviceRequestAutoReadMenuItem.Text = "Auto Read Enabled";
            this._serviceRequestAutoReadMenuItem.ToolTipText = "Enables reading messages when service request is enabled";
            // 
            // StatusView
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._toolStrip);
            this.Name = "StatusView";
            this.Size = new System.Drawing.Size(474, 31);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
