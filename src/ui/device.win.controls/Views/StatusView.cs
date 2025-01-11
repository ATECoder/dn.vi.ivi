using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using cc.isr.Enums;
using cc.isr.Enums.WinControls.ComboBoxEnumExtensions;
using cc.isr.Json.AppSettings.Services;
using cc.isr.Json.AppSettings.ViewModels;
using cc.isr.Json.AppSettings.WinForms;
using cc.isr.VI.DeviceWinControls.BindingExtensions;

namespace cc.isr.VI.DeviceWinControls;

/// <summary> A control for handling status display and control. </summary>
/// <remarks>
/// (c) 2006 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2018-12-20, 6.0.6928.x. </para>
/// </remarks>
[Description( "Status display control" )]
[System.Drawing.ToolboxBitmap( typeof( StatusView ), "StatusView" )]
[ToolboxItem( true )]
public partial class StatusView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public StatusView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
        this.ToolTip!.IsBalloon = true;
        this._readTerminalsStateMenuItem.ToolTipText = Properties.Resources.TerminalsHint;
        this._usingStatusSubsystemMenuItem.ToolTipText = Properties.Resources.StatusSubsystemOnlyHint;
        this._traceMenuItem.ToolTipText = Properties.Resources.TraceHint;
        this._traceShowLevelComboBox.ToolTipText = Properties.Resources.TraceHint;
        this._traceLogLevelComboBox.ToolTipText = Properties.Resources.TraceHint;
        this._toggleVisaEventHandlerMenuItem.ToolTipText = Properties.Resources.TraceHint;
        this._serviceRequestAutoReadMenuItem.ToolTipText = Properties.Resources.ServiceRequestAutoReadHint;
        this._pollAutoReadMenuItem.ToolTipText = Properties.Resources.PollAutoReadHint;
    }

    /// <summary> Creates a new <see cref="StatusView"/> </summary>
    /// <returns> A <see cref="StatusView"/>. </returns>
    public static StatusView Create()
    {
        StatusView? statusView = null;
        try
        {
            statusView = new StatusView();
            return statusView;
        }
        catch
        {
            statusView?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Control" />
    /// and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources;
    /// <c>false</c> to release only unmanaged
    /// resources when called from the runtime
    /// finalize. </param>
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( !this.IsDisposed && disposing )
            {
                this.InitializingComponents = true;
                // make sure the device is unbound in case the form is closed without closing the device.
                // todo: Do we need this? Me.BindVisaSessionBase(Nothing)
                this.components?.Dispose();
                this.components = null;
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " session base: assignment "

    /// <summary> Gets the session. </summary>
    /// <value> The session. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public Pith.SessionBase? SessionBase { get; private set; }

    /// <summary> Gets the sentinel indication having an open session. </summary>
    /// <value> The is open. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public bool IsSessionOpen => this.SessionBase is not null && this.SessionBase.IsSessionOpen;

    /// <summary> Binds the Session base to its controls. </summary>
    /// <param name="visaSessionBase"> The visa session base. </param>
    private void BindSessionBase( VisaSessionBase? visaSessionBase )
    {
        if ( visaSessionBase is null )
        {
            this.BindSessionBaseThis( null );
        }
        else
        {
            this.BindSessionBaseThis( visaSessionBase?.Session );
        }
    }

    /// <summary> Bind session base. </summary>
    /// <param name="sessionBase"> The session. </param>
    private void BindSessionBaseThis( Pith.SessionBase? sessionBase )
    {
        if ( this.SessionBase is not null )
        {
            this.BindSessionViewModel( false, this.SessionBase );
            this.SessionBase = null;
        }

        this.BindStatusSubsystemBaseThis( null );
        if ( sessionBase is not null )
        {
            this.SessionBase = sessionBase;
            this.BindSessionViewModel( true, this.SessionBase );
        }
    }

    /// <summary> Binds the Session view model to its controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="viewModel"> The status subsystem view model. </param>
    private void BindSessionViewModel( bool add, Pith.SessionBase viewModel )
    {
        Binding binding = this.AddRemoveBinding( this._sessionDownDownButton, add, nameof( this.Enabled ), viewModel, nameof( Pith.SessionBase.IsDeviceOpen ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        binding = this.AddRemoveBinding( this._clearInterfaceMenuItem, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.SupportsClearInterface ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        binding = this.AddRemoveBinding( this._sessionTimeoutTextBox, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.TimeoutCandidate ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation;
        binding.FormatString = "hh\\:mm\\:ss"; // "s\.fff";
        binding = this.AddRemoveBinding( this._programServiceRequestEnableBitmaskMenuItem, add, nameof( ToolStripMenuItem.Checked ), viewModel, nameof( Pith.SessionBase.ServiceRequestEventEnabled ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        binding = this.AddRemoveBinding( this._serviceRequestEnableCommandTextBox, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.ServiceRequestEnableCommandFormat ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation;
        binding = this.AddRemoveBinding( this._serviceRequestBitMaskTextBox, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.ServiceRequestEnableBitmask ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation;
        // this format string is not accepted
        // binding .FormatString = "0x{0:X2}"

        // this works
        // binding .FormatString = "X"

        // this does not work
        // binding .FormatString = "X2"

        // this does not work
        // binding .FormatInfo = New Pith.RegisterValueFormatProvider

        // this works
        if ( add )
        {
            binding.Format += BindingEventHandlers.DisplayRegisterEventHandler;
        }
        else
        {
            binding.Format -= BindingEventHandlers.DisplayRegisterEventHandler;
        }

        if ( add )
        {
            binding.Parse += BindingEventHandlers.ParseStatusRegisterEventHandler;
        }
        else
        {
            binding.Parse -= BindingEventHandlers.ParseStatusRegisterEventHandler;
        }

        binding = this.AddRemoveBinding( this._serviceRequestSplitButton, add, nameof( this.Enabled ), viewModel, nameof( Pith.SessionBase.IsDeviceOpen ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        binding = this.AddRemoveBinding( this._pollSplitButton, add, nameof( this.Enabled ), viewModel, nameof( Pith.SessionBase.IsDeviceOpen ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;

        // termination
        binding = this.AddRemoveBinding( this._readTerminationEnabledMenuItem, add, nameof( ToolStripMenuItem.Checked ), viewModel, nameof( Pith.SessionBase.ReadTerminationCharacterEnabled ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
        binding = this.AddRemoveBinding( this._readTerminationTextBox, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.ReadTerminationCharacter ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation;
        binding = this.AddRemoveBinding( this._writeTerminationTextBox, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.TerminationSequence ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation;

        // notification
        this._sessionNotificationLevelComboBox.Enabled = true;
        if ( add )
        {
            this._sessionNotificationLevelComboBox.ComboBox.DataSource = viewModel.MessageNotificationModeKeyValuePairs;
            this._sessionNotificationLevelComboBox.ComboBox.ValueMember = nameof( KeyValuePair<Enum, string>.Key );
            this._sessionNotificationLevelComboBox.ComboBox.DisplayMember = nameof( KeyValuePair<Enum, string>.Value );
            this._sessionNotificationLevelComboBox.ComboBox.BindingContext = this.BindingContext;
            // this is necessary because the combo box binding does not set the data source value on it item change event
            this._sessionNotificationLevelComboBox.ComboBox.SelectedValueChanged += this.HandleNotificationLevelComboBoxValueChanged;
        }
        else
        {
            this._sessionNotificationLevelComboBox.ComboBox.DataSource = null;
            this._sessionNotificationLevelComboBox.ComboBox.SelectedValueChanged -= this.HandleNotificationLevelComboBoxValueChanged;
        }

        _ = this.AddRemoveBinding( this._sessionNotificationLevelComboBox, add, nameof( ToolStripComboBox.SelectedItem ), viewModel, nameof( Pith.SessionBase.MessageNotificationModes ) );
        if ( add )
        {
            _ = this._sessionNotificationLevelComboBox.ComboBox.SelectValue( viewModel.MessageNotificationModes );
        }

        _ = this.AddRemoveBinding( this._toggleVisaEventHandlerMenuItem, add, nameof( CheckBox.Checked ), viewModel, nameof( Pith.SessionBase.ServiceRequestEventEnabled ), DataSourceUpdateMode.Never );
    }

    /// <summary> Handles the notification level combo box value changed. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void HandleNotificationLevelComboBoxValueChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null )
            return;
        // _sessionNotificationLevelComboBox
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripComboBox combo )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"Setting session notification level to {combo.Text}";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.StartElapsedStopwatch();
                this.SessionBase.MessageNotificationModes = combo.SelectedEnumValue( Pith.MessageNotificationModes.None );
                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " session base: status and standard registers "

    /// <summary> Reads the status register and lets the session process the status byte. </summary>
    public void ReadStatusRegister()
    {
        string activity = string.Empty;
        try
        {
            if ( this.SessionBase is not null && this.IsSessionOpen )
            {
                activity = $"reading status byte after {this.SessionBase.StatusReadDelay.TotalMilliseconds}ms delay";
                _ = Pith.SessionBase.AsyncDelay( this.SessionBase.StatusReadDelay );
                this.SessionBase!.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.ApplyStatusByte( this.SessionBase.ReadStatusByte() );
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Session read status byte menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void SessionReadStatusByteMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null || this.VisaSessionBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} reading status byte";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.StartElapsedStopwatch();
                _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( this.SessionBase.StatusReadDelay );
                this.SessionBase.ThrowDeviceExceptionIfError( this.SessionBase.ReadStatusByte() );
                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase!.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Session read standard event register menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void SessionReadStandardEventRegisterMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null || this.VisaSessionBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} reading status register";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.StartElapsedStopwatch();
                _ = cc.isr.VI.Pith.SessionBase.AsyncDelay( this.SessionBase.StatusReadDelay );
                cc.isr.VI.Pith.ServiceRequests statusByte = this.SessionBase.ReadStatusByte();
                this.SessionBase.ThrowDeviceExceptionIfError( statusByte );
                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
                if ( this.SessionBase.IsErrorBitSet( statusByte ) )
                {
                    activity = $"{this.VisaSessionBase.ResourceNameCaption} error; not reading standard event registers";
                    this.SessionBase.StatusPrompt = activity;
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                }
                else if ( this.SessionBase.IsMessageAvailableBitSet( statusByte ) )
                {
                    activity = $"{this.VisaSessionBase.ResourceNameCaption} message to read; not reading standard event registers";
                    this.SessionBase.StatusPrompt = activity;
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                }
                else
                {
                    activity = $"{this.VisaSessionBase.ResourceNameCaption} reading standard event registers";
                    this.SessionBase.StatusPrompt = activity;
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.VisaSessionBase.StartElapsedStopwatch();
                    this.VisaSessionBase.Session?.ReadStandardEventRegisters();
                    this.VisaSessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
                }
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " session base: timeout "

    private void SessionTimeoutTextBox_Validating( object? sender, CancelEventArgs e )
    {
        // this.SessionBase.TimeoutCandidate = TimeSpan.FromSeconds( double.Parse( _sessionTimeoutTextBox.Text ) ) ;
    }

    private void SessionTimeoutTextBox_Validated( object? sender, EventArgs e )
    {
        // this.SessionBase.TimeoutCandidate = TimeSpan.FromSeconds( double.Parse( _sessionTimeoutTextBox.Text ) );
    }

    /// <summary> Applies and stores the timeout. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void StoreTimeoutMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null || this.VisaSessionBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} storing current and setting a new timeout";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.StartElapsedStopwatch();
                this.SessionBase.StoreCommunicationTimeout( this.SessionBase.TimeoutCandidate );
                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Restores timeout menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void RestoreTimeoutMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null || this.VisaSessionBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} restoring previous timeout";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.StartElapsedStopwatch();
                this.SessionBase.RestoreCommunicationTimeout();
                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " session base actions "

    /// <summary> Sends the bus trigger click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void SendBusTrigger_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null || this.VisaSessionBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} sending bus trigger";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.AssertTrigger();
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " session base: debug test code "

    /// <summary> Increments the given value. </summary>
    /// <param name="value"> The value. </param>
    /// <returns> A TraceEventType. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0010:Add missing cases", Justification = "<Pending>" )]
    private static TraceEventType Increment( TraceEventType value )
    {
        switch ( value )
        {
            case TraceEventType.Critical:
                {
                    value = TraceEventType.Error;
                    break;
                }

            case TraceEventType.Error:
                {
                    value = TraceEventType.Warning;
                    break;
                }

            case TraceEventType.Warning:
                {
                    value = TraceEventType.Information;
                    break;
                }

            case TraceEventType.Information:
                {
                    value = TraceEventType.Verbose;
                    break;
                }

            case TraceEventType.Verbose:
                {
                    value = TraceEventType.Critical;
                    break;
                }

            default:
                break;
        }

        Application.DoEvents();
        return value;
    }

    #endregion

    #region " status subsystem base: assignment  "

    /// <summary> Gets the status subsystem. </summary>
    /// <value> The status subsystem. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public StatusSubsystemBase? StatusSubsystemBase { get; private set; }

    /// <summary> Bind status subsystem view model. </summary>
    /// <param name="visaSessionBase"> The visa session view model. </param>
    private void BindStatusSubsystemBase( VisaSessionBase? visaSessionBase )
    {
        if ( visaSessionBase is null )
        {
            this.BindStatusSubsystemBaseThis( null );
        }
        else
        {
            this.BindStatusSubsystemBaseThis( visaSessionBase.StatusSubsystemBase );
        }
    }

    /// <summary> Binds the StatusSubsystem view model to its controls. </summary>
    /// <param name="statusSubsystemBase"> The status subsystem view model. </param>
    private void BindStatusSubsystemBaseThis( StatusSubsystemBase? statusSubsystemBase )
    {
        if ( this.StatusSubsystemBase is not null )
        {
            this.StatusSubsystemBase = null;
        }

        this._clearErrorReportMenuItem.Enabled = statusSubsystemBase is not null;
        this._readDeviceErrorsMenuItem.Enabled = statusSubsystemBase is not null;
        this.StatusSubsystemBase = statusSubsystemBase;
    }

    #endregion

    #region " status subsystem base: device errors "

    /// <summary> Clears the error report menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void ClearErrorReportMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null || this.VisaSessionBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} clearing error report";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.StatusSubsystemBase?.Session.ClearErrorReport();
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Reads device errors menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void ReadDeviceErrorsMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = $"{this.StatusSubsystemBase.ResourceNameCaption} reading device errors";
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            if ( sender is ToolStripMenuItem menuItem )
            {
                activity = $"{this.StatusSubsystemBase.ResourceNameCaption} reading device errors";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.StartElapsedStopwatch();

                // report device errors if the error bit is set.
                _ = this.SessionBase.TryQueryAndReportDeviceErrors( this.SessionBase.ReadStatusByte() );

                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " device (visa session base): assignment "

#pragma warning disable IDE0032
    private VisaSessionBase? _visaSessionBaseInternal;
#pragma warning restore IDE0032

    /// <summary>   Gets or sets the visa session base internal. </summary>
    /// <value> The visa session base internal. </value>
    private VisaSessionBase? VisaSessionBaseInternal
    {
        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        get => this._visaSessionBaseInternal;

        [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
        set
        {
            if ( this._visaSessionBaseInternal is not null )
            {
                this._visaSessionBaseInternal.PropertyChanged -= this.VisaSessionBase_PropertyChanged;
            }

            this._visaSessionBaseInternal = value;
            if ( this._visaSessionBaseInternal is not null )
            {
                this._visaSessionBaseInternal.PropertyChanged += this.VisaSessionBase_PropertyChanged;
            }
        }
    }

    /// <summary> Gets the visa session base. </summary>
    /// <value> The visa session base. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public VisaSessionBase? VisaSessionBase => this.VisaSessionBaseInternal;

    /// <summary> Binds the visa session to its controls. </summary>
    /// <param name="visaSessionBase"> The visa session view model. </param>
    public void BindVisaSessionBase( VisaSessionBase? visaSessionBase )
    {
        if ( this.VisaSessionBase is not null )
        {
            // RemoveHandler Me.VisaSessionBase.Opened, AddressOf Me.HandleDeviceOpened
            this.VisaSessionBase.Closed -= this.HandleDeviceClosed;
            this.BindVisaSessionViewModel( false, this.VisaSessionBase );
            this.SessionSettings = null;
            this.DeviceSettings = null;
            this.UserInterfaceSettings = null;
            this.VisaSessionBaseInternal = null;
        }

        if ( visaSessionBase is not null )
        {
            this.VisaSessionBaseInternal = visaSessionBase;
            // AddHandler visaSessionBase.Opened, AddressOf Me.HandleDeviceOpened
            visaSessionBase.Closed += this.HandleDeviceClosed;
            if ( this.VisaSessionBaseInternal.Session is not null )
                this.SessionSettings = this.VisaSessionBaseInternal.Session.Scribe;
            this.DeviceSettings = null;
            this.UserInterfaceSettings = null;
            this.BindVisaSessionViewModel( true, this.VisaSessionBase );
        }

        this.BindSessionBase( visaSessionBase );
        this.BindStatusSubsystemBase( visaSessionBase );
    }

    /// <summary> Handles the property changed. </summary>
    /// <param name="session">      The session. </param>
    /// <param name="propertyName"> Name of the property. </param>
    private void HandlePropertyChanged( VisaSessionBase? session, string? propertyName )
    {
        if ( session is null || propertyName is null )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( this.VisaSessionBase.SubsystemSupportMode ):
                {
                    this.UsingStatusSubsystemOnly = session.SubsystemSupportMode == SubsystemSupportMode.StatusOnly;
                    break;
                }

            default:
                break;
        }
    }

    /// <summary> Visa session base property changed. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Property changed event information. </param>
    private void VisaSessionBase_PropertyChanged( object? sender, PropertyChangedEventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null )
            return;
        string activity = string.Empty;
        VisaSessionBase? session = sender as VisaSessionBase;
        try
        {
            this.HandlePropertyChanged( session, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    private bool _usingStatusSubsystemOnly;

    /// <summary> Gets or sets the using status subsystem only. </summary>
    /// <value> The using status subsystem only. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public bool UsingStatusSubsystemOnly
    {
        get => this._usingStatusSubsystemOnly;
        set
        {
            if ( value != this.UsingStatusSubsystemOnly )
            {
                this._usingStatusSubsystemOnly = value;
                this.NotifyPropertyChanged();
                if ( this.VisaSessionBaseInternal is not null )
                    this.VisaSessionBaseInternal.SubsystemSupportMode = value ? SubsystemSupportMode.StatusOnly : SubsystemSupportMode.Full;
            }
        }
    }

    /// <summary> Binds the visa session view model to its controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="viewModel"> The device view model. </param>
    private void BindVisaSessionViewModel( bool add, VisaSessionBase? viewModel )
    {
        if ( viewModel is null ) { return; }

        // this could be enabled when the device opens
        this.ReadTerminalsState = null;
        // menus
        this._deviceDropDownButton.Enabled = viewModel.IsSessionOpen;
        Binding binding = this.AddRemoveBinding( this._deviceDropDownButton, add, nameof( this.Enabled ), viewModel, nameof( VI.VisaSessionBase.IsSessionOpen ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;

        // open options
        _ = this.AddRemoveBinding( this._usingStatusSubsystemMenuItem, add, nameof( ToolStripMenuItem.Checked ), this, nameof( this.UsingStatusSubsystemOnly ) );

        // trace show level
        binding = this.AddRemoveBinding( this._traceShowLevelComboBox.ComboBox, add, nameof( this.Enabled ), viewModel, nameof( VI.VisaSessionBase.IsSessionOpen ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        if ( add )
        {
            this._traceShowLevelComboBox.ComboBox.DataSource = TraceEventType.Error.ValueDescriptionPairs();
            this._traceShowLevelComboBox.ComboBox.ValueMember = nameof( KeyValuePair<Enum, string>.Key );
            this._traceShowLevelComboBox.ComboBox.DisplayMember = nameof( KeyValuePair<Enum, string>.Value );
            this._traceShowLevelComboBox.ComboBox.BindingContext = this.BindingContext;
            // this is necessary because the combo box binding does not set the data source value on it item change event
            this._traceShowLevelComboBox.ComboBox.SelectedValueChanged += this.HandleTraceShowLevelComboBoxValueChanged;
        }
        else
        {
            this._traceShowLevelComboBox.ComboBox.DataSource = null;
            this._traceShowLevelComboBox.ComboBox.SelectedValueChanged -= this.HandleTraceShowLevelComboBoxValueChanged;
        }

        _ = this.AddRemoveBinding( this._traceShowLevelComboBox, add, nameof( ToolStripComboBox.SelectedItem ),
            cc.isr.VI.SessionLogger.Instance, nameof( cc.isr.VI.SessionLogger.Instance.TraceEventWriterTraceEventType ) );
        cc.isr.WinControls.ComboBoxExtensions.ComboBoxExtensionsMethods.SelectItem( this._traceShowLevelComboBox, cc.isr.VI.SessionLogger.Instance.TraceEventWriterTraceEventType );

        // trace log level
        binding = this.AddRemoveBinding( this._traceLogLevelComboBox.ComboBox, add, nameof( this.Enabled ), viewModel, nameof( VI.VisaSessionBase.IsSessionOpen ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        if ( add )
        {
            this._traceLogLevelComboBox.ComboBox.DataSource = TraceEventType.Error.ValueDescriptionPairs();
            this._traceLogLevelComboBox.ComboBox.ValueMember = nameof( KeyValuePair<Enum, string>.Key );
            this._traceLogLevelComboBox.ComboBox.DisplayMember = nameof( KeyValuePair<Enum, string>.Value );
            this._traceLogLevelComboBox.ComboBox.BindingContext = this.BindingContext;
            // this is necessary because the combo box binding does not set the data source value on it item change event
            this._traceLogLevelComboBox.ComboBox.SelectedValueChanged += this.HandleTraceLogLevelComboBoxValueChanged;
        }
        else
        {
            this._traceLogLevelComboBox.ComboBox.DataSource = null;
            this._traceLogLevelComboBox.ComboBox.SelectedValueChanged -= this.HandleTraceLogLevelComboBoxValueChanged;
        }

        _ = this.AddRemoveBinding( this._traceLogLevelComboBox, add, nameof( ToolStripComboBox.SelectedItem ), cc.isr.VI.SessionLogger.Instance,
            nameof( cc.isr.VI.SessionLogger.Instance.TraceEventWriterTraceEventType ) );
        cc.isr.WinControls.ComboBoxExtensions.ComboBoxExtensionsMethods.SelectItem( this._traceLogLevelComboBox, cc.isr.VI.SessionLogger.Instance.TraceEventWriterTraceEventType );
        binding = this.AddRemoveBinding( this._usingStatusSubsystemMenuItem, add, nameof( ToolStripMenuItem.Enabled ), viewModel, nameof( VI.VisaSessionBase.IsSessionOpen ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.Never;
        if ( add )
        {
            binding.Format += BindingEventHandlers.InvertDisplayHandler;
        }
        else
        {
            binding.Format -= BindingEventHandlers.InvertDisplayHandler;
        }

        // SRQ
        _ = this.AddRemoveBinding( this._serviceRequestHandlerAddRemoveMenuItem, add, nameof( CheckBox.Checked ), viewModel,
            nameof( VI.VisaSessionBase.ServiceRequestHandlerAssigned ), DataSourceUpdateMode.Never );
        _ = this.AddRemoveBinding( this._serviceRequestAutoReadMenuItem, add, nameof( ToolStripMenuItem.Checked ), viewModel,
            nameof( this.VisaSessionBase.ServiceRequestAutoRead ) );

        // POLL
        binding = this.AddRemoveBinding( this._pollMessageStatusBitTextBox, add, nameof( Control.Text ), viewModel, nameof( VI.VisaSessionBase.PollMessageAvailableBitmask ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation;
        binding = this.AddRemoveBinding( this._pollIntervalTextBox, add, nameof( Control.Text ), viewModel, nameof( VI.VisaSessionBase.PollTimespan ) );
        binding.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation;
        binding.FormatString = @"s\.fff";
        _ = this.AddRemoveBinding( this._pollEnabledMenuItem, add, nameof( ToolStripMenuItem.Checked ), viewModel, nameof( VI.VisaSessionBase.PollEnabled ), DataSourceUpdateMode.Never );
        _ = this.AddRemoveBinding( this._pollAutoReadMenuItem, add, nameof( ToolStripMenuItem.Checked ), viewModel, nameof( VI.VisaSessionBase.PollAutoRead ) );
    }

    #region " visa session base (device base) event handlers "

    /// <summary> Handles the device Close. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void HandleDeviceClosed( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.VisaSessionBase?.ResourceNameCaption} UI handling device closed";
        }
        // todo: Doe we need this? Me.BindStatusSubsystemBase(VisaSessionBase)
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary> Handles the device opened. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void HandleDeviceOpened( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            activity = $"{this.VisaSessionBase?.ResourceNameCaption} UI handling device opened";
        }
        // todo: Doe we need this? Me.BindStatusSubsystemBase(VisaSessionBase)
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #endregion

    #region " visa session event handlers: reset "

    /// <summary> Clears interface. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ClearInterfaceMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem && this.VisaSessionBase is not null )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase?.ResourceNameCaption} clearing interface";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.VisaSessionBase!.StartElapsedStopwatch();
                this.VisaSessionBase.ClearInterface();
                this.VisaSessionBase.ElapsedTime = this.VisaSessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Clears device (SDC). </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void ClearDeviceMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} clearing device active state";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.VisaSessionBase.StartElapsedStopwatch();
                this.VisaSessionBase.ClearActiveState();
                this.VisaSessionBase.ElapsedTime = this.VisaSessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Clears (CLS) the execution state menu item click. </summary>
    /// <param name="sender"> <see cref="object"/>
    /// instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void ClearExecutionStateMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} clearing execution state";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.VisaSessionBase.StartElapsedStopwatch();
                this.VisaSessionBase.ClearExecutionState();
                this.VisaSessionBase.ElapsedTime = this.VisaSessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Resets (RST) the known state menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ResetKnownStateMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} resetting known state";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.VisaSessionBase.StartElapsedStopwatch();
                this.VisaSessionBase.ResetKnownState();
                this.VisaSessionBase.ElapsedTime = this.VisaSessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Initializes to known state menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void InitKnownStateMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} initializing known state";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.VisaSessionBase.StartElapsedStopwatch();
                this.VisaSessionBase.InitKnownState();
                this.VisaSessionBase.ElapsedTime = this.VisaSessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " device/session: service request "

    /// <summary> Program service request enable bitmask menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void ProgramServiceRequestEnableBitmaskMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                int bitmask = this.SessionBase.RegistersBitmasksSettings.ServiceRequestEnableEventsBitmask;
                activity = $"{this.VisaSessionBase.ResourceNameCaption} applying service request enable bitmask 0x{bitmask:x2}";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.StartElapsedStopwatch();
                this.SessionBase.ApplyStatusByteEnableBitmask( bitmask );
                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Toggle visa event handler menu item check state changed. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void ToggleVisaEventHandlerMenuItem_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} turning {(this.SessionBase.ServiceRequestEventEnabled ? "OFF" : "ON")}  VISA event handling";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.StartElapsedStopwatch();
                if ( this.SessionBase.ServiceRequestEventEnabled )
                {
                    this.SessionBase.DisableServiceRequestEventHandler();
                }
                else
                {
                    this.SessionBase.EnableServiceRequestEventHandler();
                }

                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Toggles the session service request handler . </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ServiceRequestHandlerAddRemoveMenuItem_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} {(this.VisaSessionBase.ServiceRequestHandlerAssigned ? "REMOVING" : "ADDING")} VISA SRQ event handler";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.VisaSessionBase.StartElapsedStopwatch();
                if ( this.VisaSessionBase.ServiceRequestHandlerAssigned )
                {
                    this.VisaSessionBase.RemoveServiceRequestEventHandler();
                }
                else
                {
                    this.VisaSessionBase.AddServiceRequestEventHandler();
                }

                this.VisaSessionBase.ElapsedTime = this.VisaSessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " poll "

    /// <summary> Poll enabled menu item check state changed. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void PollEnabledMenuItem_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripMenuItem menuItem )
        {
            menuItem.Text = menuItem.Checked ? "Press to Stop" : "Press to Start";
        }
    }

    /// <summary> Poll enabled menu item check state changed. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void PollEnabledMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                if ( this.VisaSessionBase.PollEnabled && this.VisaSessionBase.ServiceRequestHandlerAssigned )
                {
                    _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Info, "Service request handler must be removed before polling" );
                }
                else
                {
                    activity = $"{this.VisaSessionBase.ResourceNameCaption} {(this.VisaSessionBase.PollEnabled ? "STOPPING" : "STARTING")} status byte polling";
                    this.SessionBase.StatusPrompt = activity;
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                    this.VisaSessionBase.StartElapsedStopwatch();
                    this.VisaSessionBase.PollSynchronizingObject = this;
                    this.VisaSessionBase.PollEnabled = !this.VisaSessionBase.PollEnabled;
                    this.VisaSessionBase.ElapsedTime = this.VisaSessionBase.ReadElapsedTime( true );
                }
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Poll send menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void PollSendMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} sending {this._pollWriteCommandTextBox.Text}";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.SessionBase.StartElapsedStopwatch();
                _ = this.SessionBase.WriteEscapedLine( this._pollWriteCommandTextBox.Text );
                this.SessionBase.ElapsedTime = this.SessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " device: terminals "

    /// <summary> State of the read terminals. </summary>
    private Func<bool?>? _readTerminalsState;

    /// <summary> Gets or sets the delegate for reading the terminals state. </summary>
    /// <value> The delegate for reading the terminals state. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public Func<bool?>? ReadTerminalsState
    {
        get => this._readTerminalsState;
        set
        {
            this._readTerminalsState = value;
            this._readTerminalsStateMenuItem.Enabled = value is not null;
        }
    }

    /// <summary> Read terminals state menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void ReadTerminalsStateMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            if ( sender is ToolStripMenuItem menuItem && this.ReadTerminalsState is not null )
            {
                this.Cursor = Cursors.WaitCursor;
                this.InfoProvider?.Clear();
                activity = $"{this.VisaSessionBase.ResourceNameCaption} checking terminals state";
                this.SessionBase.StatusPrompt = activity;
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
                this.VisaSessionBase.StartElapsedStopwatch();
                _ = this.ReadTerminalsState.DynamicInvoke();
                this.VisaSessionBase.ElapsedTime = this.VisaSessionBase.ReadElapsedTime( true );
            }
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " app menu: settings of session base, device, ui "

    /// <summary> Shows the settings hint. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>" )]
    private void ShowSettingsHint()
    {
        ToolTip toolTip = new() { IsBalloon = true, ToolTipIcon = ToolTipIcon.Info, ToolTipTitle = "Settings hint" };
        // call twice per https://StackOverflow.com/questions/8716917/how-to-show-a-net-balloon-tooltip
        toolTip.Show( string.Empty, this._toolStrip );
        toolTip.Show( "Opens the settings dialog", this._toolStrip );
    }

    private cc.isr.Json.AppSettings.Models.AppSettingsScribe? _sessionSettings;

    /// <summary> Gets or sets the session settings. </summary>
    /// <value> The session settings. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public cc.isr.Json.AppSettings.Models.AppSettingsScribe? SessionSettings
    {
        get => this._sessionSettings;
        set
        {
            this._sessionSettings = value;
            this._sessionSettingsMenuItem.Enabled = value is not null;
        }
    }

    /// <summary> Session settings menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void SessionSettingsMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.SessionSettings is null ) return;
        Form form = new JsonSettingsEditorForm( "Session settings",
            new AppSettingsEditorViewModel( this.SessionSettings, SimpleServiceProvider.GetInstance() ) );
        _ = form.ShowDialog();
    }

    private cc.isr.Json.AppSettings.Models.AppSettingsScribe? _deviceSettings;

    /// <summary> Gets or sets the Device settings. </summary>
    /// <value> The Device settings. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public cc.isr.Json.AppSettings.Models.AppSettingsScribe? DeviceSettings
    {
        get => this._deviceSettings;
        set
        {
            this._deviceSettings = value;
            this._deviceSettingsMenuItem.Enabled = value is not null;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Device tool strip menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void DeviceSettingsMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.DeviceSettings is not null )
        {
            Form form = new JsonSettingsEditorForm( "Device settings",
                new AppSettingsEditorViewModel( this.DeviceSettings, SimpleServiceProvider.GetInstance() ) );
            _ = form.ShowDialog();
        }
    }

    private cc.isr.Json.AppSettings.Models.AppSettingsScribe? _userInterfaceSettings;

    /// <summary> Gets or sets the User Interface settings. </summary>
    /// <value> The User Interface settings. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public cc.isr.Json.AppSettings.Models.AppSettingsScribe? UserInterfaceSettings
    {
        get => this._userInterfaceSettings;
        set
        {
            this._userInterfaceSettings = value;
            this._userInterfaceSettingsMenuItem.Enabled = value is not null;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> UserInterface tool strip menu item click. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void UserInterfaceToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.UserInterfaceSettings is not null )
        {
            Form form = new JsonSettingsEditorForm( "User Interface Settings",
                new AppSettingsEditorViewModel( this.UserInterfaceSettings, SimpleServiceProvider.GetInstance() ) );
            _ = form.ShowDialog();
        }
    }

    /// <summary> Base settings menu item click. </summary>
    /// <remarks>   David, 2021-12-08. <para>
    /// The settings <see cref="Properties.Settings.Initialize(Type, string)"/></para> must be called before attempting to edit the settings. </remarks>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void UIBaseSettingsMenuItem_Click( object? sender, EventArgs e )
    {
        if ( Properties.Settings.Instance is not null )
        {
            Form form = new JsonSettingsEditorForm( "UI Base Settings",
                new AppSettingsEditorViewModel( Properties.Settings.Instance.Scribe!, SimpleServiceProvider.GetInstance() ) );
            _ = form.ShowDialog();
        }
    }

    /// <summary> Opens status subsystem menu item check state changed. </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void OpenStatusSubsystemMenuItem_CheckStateChanged( object? sender, EventArgs e )
    {
        if ( sender is ToolStripMenuItem menuItem )
        {
            menuItem.Text = menuItem.Checked ? "Using status subsystem only" : "Using all subsystems";
        }
    }

    /// <summary> Edit resource names menu item click. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void EditResourceNamesMenuItem_Click( object? sender, EventArgs e )
    {
        using ResourceNameInfoEditorForm editorForm = new();
        _ = editorForm.ShowDialog( this );
    }

    #endregion

    #region " app menu: trace level "

    /// <summary>
    /// Event handler. Called by _displayTraceLevelComboBox for selected value changed events.
    /// </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void HandleTraceShowLevelComboBoxValueChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.VisaSessionBase.ResourceNameCaption} selecting trace show level";
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Event handler. Called by _logTraceLevelComboBox for selected value changed events.
    /// </summary>
    /// <param name="sender"> <see cref="object"/> instance of this
    /// <see cref="Control"/> </param>
    /// <param name="e">      Event information. </param>
    private void HandleTraceLogLevelComboBoxValueChanged( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null || this.SessionBase is null
            || this.VisaSessionBase is null || this.StatusSubsystemBase is null )
            return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.VisaSessionBase.ResourceNameCaption} selecting trace log level";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
        }
        catch ( Exception ex )
        {
            this.SessionBase.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( sender, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

}
