using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls
{
    public partial class DisplayView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            _measureStatusStrip = new StatusStrip();
            _terminalsStatusLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _measureMetaStatusLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _readingAmountLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _measureElapsedTimeLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _standardRegisterLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _statusRegisterLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _layout = new TableLayoutPanel();
            _layout.Resize += new EventHandler(Layout_Resize);
            _sessionReadingStatusStrip = new StatusStrip();
            _lastReadingLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _sessionElapsedTimeLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _subsystemStatusStrip = new StatusStrip();
            _subsystemReadingLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _subsystemElapsedTimeLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _titleStatusStrip = new StatusStrip();
            _titleLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _sessionOpenCloseStatusLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _errorStatusStrip = new StatusStrip();
            _errorLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _statusStrip = new StatusStrip();
            _serviceRequestEnableBitmaskLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _standardEventEnableBitmaskLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _serviceRequestEnabledLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _operationSummaryBitLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _serviceRequestBitLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _eventSummaryBitLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _messageAvailableBitLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _questionableSummaryBitLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _errorAvailableBitLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _systemEventBitLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _measurementEventBitLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _powerOnEventLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _userRequestEventLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _commandErrorEventLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _executionErrorEventLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _deviceDependentErrorEventToolLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _queryErrorEventLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _operationCompleteEventLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _serviceRequestHandledLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _measurementEventStatusLabel = new cc.isr.WinControls.ToolStripStatusLabel();
            _measureStatusStrip.SuspendLayout();
            _layout.SuspendLayout();
            _sessionReadingStatusStrip.SuspendLayout();
            _subsystemStatusStrip.SuspendLayout();
            _titleStatusStrip.SuspendLayout();
            _errorStatusStrip.SuspendLayout();
            _statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _measureStatusStrip
            // 
            _measureStatusStrip.BackColor = System.Drawing.Color.Transparent;
            _measureStatusStrip.Font = new System.Drawing.Font("Segoe UI", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _measureStatusStrip.GripMargin = new Padding(0);
            _measureStatusStrip.Items.AddRange(new ToolStripItem[] { _terminalsStatusLabel, _measureMetaStatusLabel, _readingAmountLabel, _measureElapsedTimeLabel });
            _measureStatusStrip.Location = new System.Drawing.Point(0, 22);
            _measureStatusStrip.Name = "_MeasureStatusStrip";
            _measureStatusStrip.ShowItemToolTips = true;
            _measureStatusStrip.Size = new System.Drawing.Size(441, 42);
            _measureStatusStrip.SizingGrip = false;
            _measureStatusStrip.TabIndex = 18;
            // 
            // _terminalsStatusLabel
            // 
            _terminalsStatusLabel.Font = new System.Drawing.Font("Segoe UI", 20.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _terminalsStatusLabel.ForeColor = System.Drawing.SystemColors.Info;
            _terminalsStatusLabel.Name = "_TerminalsStatusLabel";
            _terminalsStatusLabel.Size = new System.Drawing.Size(30, 37);
            _terminalsStatusLabel.Text = "F";
            _terminalsStatusLabel.ToolTipText = "Terminals status";
            // 
            // _measureMetaStatusLabel
            // 
            _measureMetaStatusLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _measureMetaStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _measureMetaStatusLabel.ForeColor = System.Drawing.Color.LimeGreen;
            _measureMetaStatusLabel.Margin = new Padding(0);
            _measureMetaStatusLabel.Name = "_MeasureMetaStatusLabel";
            _measureMetaStatusLabel.Overflow = ToolStripItemOverflow.Never;
            _measureMetaStatusLabel.Size = new System.Drawing.Size(16, 42);
            _measureMetaStatusLabel.Text = "p";
            _measureMetaStatusLabel.ToolTipText = "Measure meta status";
            // 
            // _readingAmountLabel
            // 
            _readingAmountLabel.AutoSize = false;
            _readingAmountLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _readingAmountLabel.ForeColor = System.Drawing.Color.Aquamarine;
            _readingAmountLabel.Margin = new Padding(0);
            _readingAmountLabel.Name = "_ReadingAmountLabel";
            _readingAmountLabel.Overflow = ToolStripItemOverflow.Never;
            _readingAmountLabel.Size = new System.Drawing.Size(357, 42);
            _readingAmountLabel.Spring = true;
            _readingAmountLabel.Text = "-.------- mV";
            _readingAmountLabel.ToolTipText = "Reading value and unit";
            // 
            // _measureElapsedTimeLabel
            // 
            _measureElapsedTimeLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _measureElapsedTimeLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _measureElapsedTimeLabel.ForeColor = System.Drawing.SystemColors.Info;
            _measureElapsedTimeLabel.Margin = new Padding(0);
            _measureElapsedTimeLabel.Name = "_MeasureElapsedTimeLabel";
            _measureElapsedTimeLabel.Size = new System.Drawing.Size(23, 42);
            _measureElapsedTimeLabel.Text = "ms";
            _measureElapsedTimeLabel.ToolTipText = "Measure last action time in ms";
            // 
            // _standardRegisterLabel
            // 
            _standardRegisterLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _standardRegisterLabel.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _standardRegisterLabel.ForeColor = System.Drawing.Color.DarkOrange;
            _standardRegisterLabel.Margin = new Padding(0);
            _standardRegisterLabel.Name = "_StandardRegisterLabel";
            _standardRegisterLabel.Size = new System.Drawing.Size(19, 22);
            _standardRegisterLabel.Text = "0x..";
            _standardRegisterLabel.ToolTipText = "Standard Register Value";
            _standardRegisterLabel.Visible = false;
            // 
            // _statusRegisterLabel
            // 
            _statusRegisterLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _statusRegisterLabel.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _statusRegisterLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _statusRegisterLabel.Margin = new Padding(0);
            _statusRegisterLabel.Name = "_StatusRegisterLabel";
            _statusRegisterLabel.Size = new System.Drawing.Size(19, 22);
            _statusRegisterLabel.Text = "0x..";
            _statusRegisterLabel.ToolTipText = "Status Register Value";
            // 
            // _layout
            // 
            _layout.AutoSize = true;
            _layout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _layout.BackColor = System.Drawing.Color.FromArgb( 30, 38, 44 );
            _layout.ColumnCount = 1;
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            _layout.Controls.Add(_sessionReadingStatusStrip, 0, 2);
            _layout.Controls.Add(_measureStatusStrip, 0, 1);
            _layout.Controls.Add(_subsystemStatusStrip, 0, 3);
            _layout.Controls.Add(_titleStatusStrip, 0, 0);
            _layout.Controls.Add(_errorStatusStrip, 0, 4);
            _layout.Controls.Add(_statusStrip, 0, 5);
            _layout.Dock = DockStyle.Top;
            _layout.Location = new System.Drawing.Point(1, 1);
            _layout.Margin = new Padding(0);
            _layout.Name = "_Layout";
            _layout.RowCount = 6;
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle());
            _layout.Size = new System.Drawing.Size(441, 152);
            _layout.TabIndex = 23;
            // 
            // _sessionReadingStatusStrip
            // 
            _sessionReadingStatusStrip.BackColor = System.Drawing.Color.FromArgb( 30, 38, 44 );
            _sessionReadingStatusStrip.GripMargin = new Padding(0);
            _sessionReadingStatusStrip.Items.AddRange(new ToolStripItem[] { _measurementEventStatusLabel, _lastReadingLabel, _sessionElapsedTimeLabel });
            _sessionReadingStatusStrip.Location = new System.Drawing.Point(0, 64);
            _sessionReadingStatusStrip.Name = "_SessionReadingStatusStrip";
            _sessionReadingStatusStrip.ShowItemToolTips = true;
            _sessionReadingStatusStrip.Size = new System.Drawing.Size(441, 22);
            _sessionReadingStatusStrip.SizingGrip = false;
            _sessionReadingStatusStrip.TabIndex = 0;
            _sessionReadingStatusStrip.Text = "Session Status Strip";
            // 
            // _lastReadingLabel
            // 
            _lastReadingLabel.ForeColor = System.Drawing.Color.Aquamarine;
            _lastReadingLabel.Margin = new Padding(0);
            _lastReadingLabel.Name = "_LastReadingLabel";
            _lastReadingLabel.Size = new System.Drawing.Size(343, 22);
            _lastReadingLabel.Spring = true;
            _lastReadingLabel.Text = "last reading";
            _lastReadingLabel.ToolTipText = "Last reading";
            // 
            // _sessionElapsedTimeLabel
            // 
            _sessionElapsedTimeLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _sessionElapsedTimeLabel.ForeColor = System.Drawing.SystemColors.Info;
            _sessionElapsedTimeLabel.Name = "_SessionElapsedTimeLabel";
            _sessionElapsedTimeLabel.Size = new System.Drawing.Size(23, 17);
            _sessionElapsedTimeLabel.Text = "ms";
            _sessionElapsedTimeLabel.ToolTipText = "Session last action time in ms";
            // 
            // _subsystemStatusStrip
            // 
            _subsystemStatusStrip.BackColor = System.Drawing.Color.FromArgb( 30, 38, 44 );
            _subsystemStatusStrip.GripMargin = new Padding(0);
            _subsystemStatusStrip.Items.AddRange(new ToolStripItem[] { _subsystemReadingLabel, _subsystemElapsedTimeLabel });
            _subsystemStatusStrip.Location = new System.Drawing.Point(0, 86);
            _subsystemStatusStrip.Name = "_SubsystemStatusStrip";
            _subsystemStatusStrip.ShowItemToolTips = true;
            _subsystemStatusStrip.Size = new System.Drawing.Size(441, 22);
            _subsystemStatusStrip.SizingGrip = false;
            _subsystemStatusStrip.TabIndex = 19;
            _subsystemStatusStrip.Text = "Subsystem Status Strip";
            // 
            // _subsystemReadingLabel
            // 
            _subsystemReadingLabel.BackColor = System.Drawing.Color.Transparent;
            _subsystemReadingLabel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemReadingLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _subsystemReadingLabel.Name = "_SubsystemReadingLabel";
            _subsystemReadingLabel.Size = new System.Drawing.Size(403, 17);
            _subsystemReadingLabel.Spring = true;
            _subsystemReadingLabel.Text = "-.-------- mA";
            _subsystemReadingLabel.ToolTipText = "Subsystem reading";
            // 
            // _subsystemElapsedTimeLabel
            // 
            _subsystemElapsedTimeLabel.ForeColor = System.Drawing.SystemColors.Info;
            _subsystemElapsedTimeLabel.Name = "_SubsystemElapsedTimeLabel";
            _subsystemElapsedTimeLabel.Size = new System.Drawing.Size(23, 17);
            _subsystemElapsedTimeLabel.Text = "ms";
            _subsystemElapsedTimeLabel.ToolTipText = "Subsystem last action time in ms";
            // 
            // _titleStatusStrip
            // 
            _titleStatusStrip.BackColor = System.Drawing.Color.FromArgb( 30, 38, 44 );
            _titleStatusStrip.GripMargin = new Padding(0);
            _titleStatusStrip.Items.AddRange(new ToolStripItem[] { _titleLabel, _sessionOpenCloseStatusLabel });
            _titleStatusStrip.Location = new System.Drawing.Point(0, 0);
            _titleStatusStrip.Name = "_TitleStatusStrip";
            _titleStatusStrip.ShowItemToolTips = true;
            _titleStatusStrip.Size = new System.Drawing.Size(441, 22);
            _titleStatusStrip.SizingGrip = false;
            _titleStatusStrip.TabIndex = 20;
            _titleStatusStrip.Text = "Title Status Strip";
            // 
            // _titleLabel
            // 
            _titleLabel.BackColor = System.Drawing.Color.Transparent;
            _titleLabel.ForeColor = System.Drawing.SystemColors.Info;
            _titleLabel.Name = "_TitleLabel";
            _titleLabel.Size = new System.Drawing.Size(426, 17);
            _titleLabel.Spring = true;
            _titleLabel.Text = "Multimeter";
            _titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            _titleLabel.ToolTipText = "Device title";
            // 
            // _sessionOpenCloseStatusLabel
            // 
            _sessionOpenCloseStatusLabel.Name = "_SessionOpenCloseStatusLabel";
            _sessionOpenCloseStatusLabel.Size = new System.Drawing.Size(0, 17);
            _sessionOpenCloseStatusLabel.ToolTipText = "Session open status";
            // 
            // _errorStatusStrip
            // 
            _errorStatusStrip.BackColor = System.Drawing.Color.FromArgb( 30, 38, 44 );
            _errorStatusStrip.GripMargin = new Padding(0);
            _errorStatusStrip.Items.AddRange(new ToolStripItem[] { _errorLabel });
            _errorStatusStrip.Location = new System.Drawing.Point(0, 108);
            _errorStatusStrip.Name = "_ErrorStatusStrip";
            _errorStatusStrip.ShowItemToolTips = true;
            _errorStatusStrip.Size = new System.Drawing.Size(441, 22);
            _errorStatusStrip.SizingGrip = false;
            _errorStatusStrip.TabIndex = 21;
            _errorStatusStrip.Text = "Error Status Strip";
            // 
            // _errorLabel
            // 
            _errorLabel.BackColor = System.Drawing.Color.Transparent;
            _errorLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _errorLabel.ForeColor = System.Drawing.Color.LimeGreen;
            _errorLabel.Name = "_ErrorLabel";
            _errorLabel.Size = new System.Drawing.Size(426, 17);
            _errorLabel.Spring = true;
            _errorLabel.Text = "000, No Errors";
            _errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            _errorLabel.TextChanged += new EventHandler( ErrorLabel_TextChanges );
            // 
            // _statusStrip
            // 
            _statusStrip.BackColor = System.Drawing.Color.FromArgb( 30, 38, 44 );
            _statusStrip.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _statusStrip.GripMargin = new Padding(0);
            _statusStrip.Items.AddRange(new ToolStripItem[] { _serviceRequestEnableBitmaskLabel, _standardEventEnableBitmaskLabel, _serviceRequestEnabledLabel, _statusRegisterLabel, _operationSummaryBitLabel, _serviceRequestBitLabel, _eventSummaryBitLabel, _messageAvailableBitLabel, _questionableSummaryBitLabel, _errorAvailableBitLabel, _systemEventBitLabel, _measurementEventBitLabel, _standardRegisterLabel, _powerOnEventLabel, _userRequestEventLabel, _commandErrorEventLabel, _executionErrorEventLabel, _deviceDependentErrorEventToolLabel, _queryErrorEventLabel, _operationCompleteEventLabel, _serviceRequestHandledLabel });
            _statusStrip.Location = new System.Drawing.Point(0, 130);
            _statusStrip.Name = "_StatusStrip";
            _statusStrip.ShowItemToolTips = true;
            _statusStrip.Size = new System.Drawing.Size(441, 22);
            _statusStrip.SizingGrip = false;
            _statusStrip.TabIndex = 22;
            _statusStrip.Text = "Status Strip";
            // 
            // _serviceRequestEnableBitmaskLabel
            // 
            _serviceRequestEnableBitmaskLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _serviceRequestEnableBitmaskLabel.Name = "_ServiceRequestEnableBitmaskLabel";
            _serviceRequestEnableBitmaskLabel.Size = new System.Drawing.Size(19, 17);
            _serviceRequestEnableBitmaskLabel.Text = "0x..";
            _serviceRequestEnableBitmaskLabel.ToolTipText = "Service Request Enable Bitmask";
            // 
            // _standardEventEnableBitmaskLabel
            // 
            _standardEventEnableBitmaskLabel.ForeColor = System.Drawing.Color.DarkOrange;
            _standardEventEnableBitmaskLabel.Name = "_StandardEventEnableBitmaskLabel";
            _standardEventEnableBitmaskLabel.Size = new System.Drawing.Size(19, 17);
            _standardEventEnableBitmaskLabel.Text = "0x..";
            _standardEventEnableBitmaskLabel.ToolTipText = "Standard Event Enable Bitmask";
            // 
            // _serviceRequestEnabledLabel
            // 
            _serviceRequestEnabledLabel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _serviceRequestEnabledLabel.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _serviceRequestEnabledLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _serviceRequestEnabledLabel.Image = Properties.Resources.lightning_16;
            _serviceRequestEnabledLabel.ImageScaling = ToolStripItemImageScaling.None;
            _serviceRequestEnabledLabel.Name = "_ServiceRequestEnabledLabel";
            _serviceRequestEnabledLabel.Size = new System.Drawing.Size(16, 17);
            _serviceRequestEnabledLabel.Text = "SRE";
            _serviceRequestEnabledLabel.ToolTipText = "Service request is enabled";
            _serviceRequestEnabledLabel.Visible = false;
            // 
            // _operationSummaryBitLabel
            // 
            _operationSummaryBitLabel.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _operationSummaryBitLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _operationSummaryBitLabel.Margin = new Padding(0);
            _operationSummaryBitLabel.Name = "_OperationSummaryBitLabel";
            _operationSummaryBitLabel.Size = new System.Drawing.Size(21, 22);
            _operationSummaryBitLabel.Text = "osb";
            _operationSummaryBitLabel.ToolTipText = " Operation Summary Bit (OSB) is on";
            _operationSummaryBitLabel.Visible = false;
            // 
            // _serviceRequestBitLabel
            // 
            _serviceRequestBitLabel.BackColor = System.Drawing.Color.Transparent;
            _serviceRequestBitLabel.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _serviceRequestBitLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _serviceRequestBitLabel.Margin = new Padding(0);
            _serviceRequestBitLabel.Name = "_ServiceRequestBitLabel";
            _serviceRequestBitLabel.Size = new System.Drawing.Size(19, 22);
            _serviceRequestBitLabel.Text = "rqs";
            _serviceRequestBitLabel.ToolTipText = "Request for Service (RQS) bit or the Main Summary Status Bit (MSB) bit is on";
            _serviceRequestBitLabel.Visible = false;
            // 
            // _eventSummaryBitLabel
            // 
            _eventSummaryBitLabel.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _eventSummaryBitLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _eventSummaryBitLabel.Margin = new Padding(0);
            _eventSummaryBitLabel.Name = "_EventSummaryBitLabel";
            _eventSummaryBitLabel.Size = new System.Drawing.Size(20, 22);
            _eventSummaryBitLabel.Text = "esb";
            _eventSummaryBitLabel.ToolTipText = "Event Summary Bit (ESB) is on";
            _eventSummaryBitLabel.Visible = false;
            // 
            // _messageAvailableBitLabel
            // 
            _messageAvailableBitLabel.BackColor = System.Drawing.Color.Transparent;
            _messageAvailableBitLabel.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _messageAvailableBitLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _messageAvailableBitLabel.Margin = new Padding(0);
            _messageAvailableBitLabel.Name = "_MessageAvailableBitLabel";
            _messageAvailableBitLabel.Size = new System.Drawing.Size(23, 22);
            _messageAvailableBitLabel.Text = "mav";
            _messageAvailableBitLabel.ToolTipText = "Message Available Bit (MAV) is on";
            _messageAvailableBitLabel.Visible = false;
            // 
            // _questionableSummaryBitLabel
            // 
            _questionableSummaryBitLabel.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _questionableSummaryBitLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _questionableSummaryBitLabel.Margin = new Padding(0);
            _questionableSummaryBitLabel.Name = "_QuestionableSummaryBitLabel";
            _questionableSummaryBitLabel.Size = new System.Drawing.Size(21, 22);
            _questionableSummaryBitLabel.Text = "qsb";
            _questionableSummaryBitLabel.ToolTipText = "Questionable Summary Bit (QSB) is on";
            _questionableSummaryBitLabel.Visible = false;
            // 
            // _errorAvailableBitLabel
            // 
            _errorAvailableBitLabel.BackColor = System.Drawing.Color.Transparent;
            _errorAvailableBitLabel.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _errorAvailableBitLabel.ForeColor = System.Drawing.Color.Red;
            _errorAvailableBitLabel.Margin = new Padding(0);
            _errorAvailableBitLabel.Name = "_ErrorAvailableBitLabel";
            _errorAvailableBitLabel.Size = new System.Drawing.Size(20, 22);
            _errorAvailableBitLabel.Text = "Eav";
            _errorAvailableBitLabel.ToolTipText = "Error Available Bit (EAV) is on";
            _errorAvailableBitLabel.Visible = false;
            // 
            // _systemEventBitLabel
            // 
            _systemEventBitLabel.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _systemEventBitLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _systemEventBitLabel.Margin = new Padding(0);
            _systemEventBitLabel.Name = "_SystemEventBitLabel";
            _systemEventBitLabel.Size = new System.Drawing.Size(19, 22);
            _systemEventBitLabel.Text = "ssb";
            _systemEventBitLabel.ToolTipText = "System Event Bit (SSB) is on";
            _systemEventBitLabel.Visible = false;
            // 
            // _measurementEventBitLabel
            // 
            _measurementEventBitLabel.Font = new System.Drawing.Font("Segoe UI", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _measurementEventBitLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _measurementEventBitLabel.Margin = new Padding(0);
            _measurementEventBitLabel.Name = "_MeasurementEventBitLabel";
            _measurementEventBitLabel.Size = new System.Drawing.Size(23, 22);
            _measurementEventBitLabel.Text = "msb";
            _measurementEventBitLabel.ToolTipText = "Measurement Event Bit (MEB) is on";
            _measurementEventBitLabel.Visible = false;
            // 
            // _powerOnEventLabel
            // 
            _powerOnEventLabel.ForeColor = System.Drawing.Color.DarkOrange;
            _powerOnEventLabel.Name = "_PowerOnEventLabel";
            _powerOnEventLabel.Size = new System.Drawing.Size(22, 17);
            _powerOnEventLabel.Text = "pon";
            _powerOnEventLabel.ToolTipText = "Power on";
            _powerOnEventLabel.Visible = false;
            // 
            // _userRequestEventLabel
            // 
            _userRequestEventLabel.ForeColor = System.Drawing.Color.DarkOrange;
            _userRequestEventLabel.Name = "_UserRequestEventLabel";
            _userRequestEventLabel.Size = new System.Drawing.Size(20, 17);
            _userRequestEventLabel.Text = "urq";
            _userRequestEventLabel.ToolTipText = "User request ";
            _userRequestEventLabel.Visible = false;
            // 
            // _commandErrorEventLabel
            // 
            _commandErrorEventLabel.ForeColor = System.Drawing.Color.DarkOrange;
            _commandErrorEventLabel.Name = "_CommandErrorEventLabel";
            _commandErrorEventLabel.Size = new System.Drawing.Size(22, 17);
            _commandErrorEventLabel.Text = "cme";
            _commandErrorEventLabel.ToolTipText = "Command error ";
            _commandErrorEventLabel.Visible = false;
            // 
            // _executionErrorEventLabel
            // 
            _executionErrorEventLabel.ForeColor = System.Drawing.Color.DarkOrange;
            _executionErrorEventLabel.Name = "_ExecutionErrorEventLabel";
            _executionErrorEventLabel.Size = new System.Drawing.Size(20, 17);
            _executionErrorEventLabel.Text = "exe";
            _executionErrorEventLabel.ToolTipText = "Execution error";
            _executionErrorEventLabel.Visible = false;
            // 
            // _deviceDependentErrorEventToolLabel
            // 
            _deviceDependentErrorEventToolLabel.ForeColor = System.Drawing.Color.DarkOrange;
            _deviceDependentErrorEventToolLabel.Name = "_DeviceDependentErrorEventToolLabel";
            _deviceDependentErrorEventToolLabel.Size = new System.Drawing.Size(22, 17);
            _deviceDependentErrorEventToolLabel.Text = "dde";
            _deviceDependentErrorEventToolLabel.ToolTipText = "Device dependent error";
            _deviceDependentErrorEventToolLabel.Visible = false;
            // 
            // _queryErrorEventLabel
            // 
            _queryErrorEventLabel.ForeColor = System.Drawing.Color.DarkOrange;
            _queryErrorEventLabel.Name = "_QueryErrorEventLabel";
            _queryErrorEventLabel.Size = new System.Drawing.Size(21, 17);
            _queryErrorEventLabel.Text = "qye";
            _queryErrorEventLabel.ToolTipText = "Query error";
            _queryErrorEventLabel.Visible = false;
            // 
            // _operationCompleteEventLabel
            // 
            _operationCompleteEventLabel.ForeColor = System.Drawing.Color.DarkOrange;
            _operationCompleteEventLabel.Name = "_OperationCompleteEventLabel";
            _operationCompleteEventLabel.Size = new System.Drawing.Size(21, 17);
            _operationCompleteEventLabel.Text = "opc";
            _operationCompleteEventLabel.ToolTipText = "Operation complete";
            _operationCompleteEventLabel.Visible = false;
            // 
            // _serviceRequestHandledLabel
            // 
            _serviceRequestHandledLabel.Image = Properties.Resources.EventHandleGreen16;
            _serviceRequestHandledLabel.Name = "_ServiceRequestHandledLabel";
            _serviceRequestHandledLabel.Size = new System.Drawing.Size(16, 17);
            _serviceRequestHandledLabel.ToolTipText = "Service request handler assigned";
            _serviceRequestHandledLabel.Visible = false;
            // 
            // _measurementEventStatusLabel
            // 
            _measurementEventStatusLabel.ForeColor = System.Drawing.Color.LightSkyBlue;
            _measurementEventStatusLabel.Name = "_MeasurementEventStatusLabel";
            _measurementEventStatusLabel.Size = new System.Drawing.Size(29, 17);
            _measurementEventStatusLabel.Text = "mes";
            _measurementEventStatusLabel.ToolTipText = "Measurement Event Status";
            // 
            // DisplayView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_layout);
            Name = "DisplayView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(443, 148);
            _measureStatusStrip.ResumeLayout(false);
            _measureStatusStrip.PerformLayout();
            _layout.ResumeLayout(false);
            _layout.PerformLayout();
            _sessionReadingStatusStrip.ResumeLayout(false);
            _sessionReadingStatusStrip.PerformLayout();
            _subsystemStatusStrip.ResumeLayout(false);
            _subsystemStatusStrip.PerformLayout();
            _titleStatusStrip.ResumeLayout(false);
            _titleStatusStrip.PerformLayout();
            _errorStatusStrip.ResumeLayout(false);
            _errorStatusStrip.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private StatusStrip _measureStatusStrip;
        private cc.isr.WinControls.ToolStripStatusLabel _measureMetaStatusLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _readingAmountLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _statusRegisterLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _standardRegisterLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _measureElapsedTimeLabel;
        private StatusStrip _sessionReadingStatusStrip;
        private cc.isr.WinControls.ToolStripStatusLabel _lastReadingLabel;
        private StatusStrip _subsystemStatusStrip;
        private cc.isr.WinControls.ToolStripStatusLabel _subsystemReadingLabel;
        private StatusStrip _titleStatusStrip;
        private cc.isr.WinControls.ToolStripStatusLabel _titleLabel;
        private StatusStrip _errorStatusStrip;
        private cc.isr.WinControls.ToolStripStatusLabel _errorLabel;
        private TableLayoutPanel _layout;
        private cc.isr.WinControls.ToolStripStatusLabel _sessionElapsedTimeLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _serviceRequestBitLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _messageAvailableBitLabel;
        private StatusStrip _statusStrip;
        private cc.isr.WinControls.ToolStripStatusLabel _serviceRequestEnabledLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _errorAvailableBitLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _subsystemElapsedTimeLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _terminalsStatusLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _sessionOpenCloseStatusLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _operationSummaryBitLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _eventSummaryBitLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _questionableSummaryBitLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _systemEventBitLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _measurementEventBitLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _powerOnEventLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _userRequestEventLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _commandErrorEventLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _executionErrorEventLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _deviceDependentErrorEventToolLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _queryErrorEventLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _operationCompleteEventLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _serviceRequestEnableBitmaskLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _standardEventEnableBitmaskLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _serviceRequestHandledLabel;
        private cc.isr.WinControls.ToolStripStatusLabel _measurementEventStatusLabel;
    }
}
