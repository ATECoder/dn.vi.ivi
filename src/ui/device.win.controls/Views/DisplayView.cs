using System;
using System.ComponentModel;
using System.Windows.Forms;
using cc.isr.Json.AppSettings.Models;
using cc.isr.VI.DeviceWinControls.BindingExtensions;
using cc.isr.WinControls.BindingExtensions;

namespace cc.isr.VI.DeviceWinControls;

/// <summary> A control for handling status display and control. </summary>
/// <remarks>
/// David, 2018-12-20, 6.0.6928. <para>
/// Icons made by
/// <see href="https://www.flaticon.com/authors/darius-dan"/>
/// (c) 2006 Integrated Scientific Resources, Inc. All rights reserved. </para><para>
/// Licensed under The MIT License. </para>
/// </remarks>
[Description( "Displays instrument measurement, status and error" )]
[System.Drawing.ToolboxBitmap( typeof( DisplayView ), "DisplayView" )]
[ToolboxItem( true )]
public partial class DisplayView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    public DisplayView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        AllSettings.Instance.CreateScribe();
        this.BindSettings();
        this.InitializingComponents = false;
    }

    /// <summary> Creates a new <see cref="DisplayView"/> </summary>
    /// <returns> A <see cref="DisplayView"/>. </returns>
    public static DisplayView Create()
    {
        DisplayView? view = null;
        try
        {
            view = new DisplayView();
            return view;
        }
        catch
        {
            view?.Dispose();
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
                if ( this.components is not null )
                {
                    this.components?.Dispose();
                    this.components = null;
                }
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " settings class "

    /// <summary>   A settings container class for all settings. </summary>
    /// <remarks>   2024-10-16. </remarks>
    private sealed class AllSettings : cc.isr.Json.AppSettings.Settings.SettingsContainerBase
    {
        #region " construction "

        /// <summary>
        /// Constructor that prevents a default instance of this class from being created.
        /// </summary>
        /// <remarks>   2023-04-24. </remarks>
        public AllSettings() { }

        #endregion

        #region " singleton "

        /// <summary>   Gets the instance. </summary>
        /// <value> The instance. </value>
        public static AllSettings Instance => _instance.Value;

        private static readonly Lazy<AllSettings> _instance = new( () => new AllSettings(), true );

        #endregion

        #region " create scribe "

        /// <summary>   Creates a scribe. </summary>
        /// <remarks>   2025-01-13. </remarks>
        public override void CreateScribe()
        {
            this.DisplayViewSettings = new();
            this.Scribe = new( [this.DisplayViewSettings] )
            {
                SerializerOptions = AppSettingsScribe.CsvRgbColorSerializerOptions
            };
        }

        /// <summary>   Reads the settings. </summary>
        /// <remarks>   2024-08-17. <para>
        /// Creates the scribe if null.</para>
        /// </remarks>
        /// <param name="declaringType">        The Type of the declaring object. </param>
        /// <param name="settingsFileSuffix">   (Optional) [.settings] The suffix of the assembly settings file,
        ///                                     e.g., '.settings' in 'cc.isr.VI.Tsp.K2600.Device.MSTest.Session.json'
        ///                                     where cc.isr.VI.Tsp.K2600.Device.MSTest is the assembly name.</param>
        /// <param name="overwriteAllUsersFile"> (Optional) [false] True to over-write all users settings file. </param>
        /// <param name="overwriteThisUserFile"> (Optional) [false] True to over-write this user settings file. </param>
        public override void ReadSettings( Type declaringType, string settingsFileSuffix = ".Settings",
            bool overwriteAllUsersFile = false, bool overwriteThisUserFile = false )
        {
            base.ReadSettings( declaringType, settingsFileSuffix, overwriteAllUsersFile, overwriteThisUserFile );
        }

        #endregion

        #region " settings "

        /// <summary>   Gets or sets the location settings. </summary>
        /// <value> The location settings. </value>
        public Views.DisplayViewSettings DisplayViewSettings { get; private set; } = new();

        #endregion
    }

    #endregion

    #region " form methods "

    /// <summary> Layout resize. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void Layout_Resize( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents )
            return;
        this.Height = this._layout.Height;
    }

    #endregion

    #region " visa session base: device "

    /// <summary> The visa session base. </summary>

    /// <summary> Gets the visa session base. </summary>
    /// <value> The visa session base. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public VisaSessionBase? VisaSessionBase { get; private set; }

    /// <summary> Binds the visa session base (device base) to its controls. </summary>
    /// <param name="visaSessionBase"> The visa session base (device base) view model. </param>
    public void BindVisaSessionBase( VisaSessionBase? visaSessionBase )
    {
        if ( this.VisaSessionBase is not null )
        {
            this.BindViewModel( false, this.VisaSessionBase );
            this.VisaSessionBase.Opened -= this.HandleDeviceOpened;
            this.VisaSessionBase.Closed -= this.HandleDeviceClosed;
            this.VisaSessionBase = null;
        }

        if ( visaSessionBase is not null )
        {
            this.VisaSessionBase = visaSessionBase;
            this.VisaSessionBase.Opened += this.HandleDeviceOpened;
            this.VisaSessionBase.Closed += this.HandleDeviceClosed;
            this.BindViewModel( true, this.VisaSessionBase );
        }

        this.BindSessionBase( visaSessionBase );
        this.BindStatusSubsystemBase( visaSessionBase );
    }

    /// <summary> Bind view model. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="viewModel"> The status subsystem view model. </param>
    private void BindViewModel( bool add, VisaSessionBase viewModel )
    {
        _ = this.AddRemoveBinding( this._serviceRequestHandledLabel, add, nameof( this.Visible ), viewModel, nameof( this.VisaSessionBase.ServiceRequestHandlerAssigned ), DataSourceUpdateMode.Never );
    }

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
        // todo: Doe we need this? Me.BindStatusSubsystemBase(Me.VisaSessionBase)
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
        // todo: Doe we need this? Me.BindStatusSubsystemBase(me.VisaSessionBase)
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " session base "

    /// <summary> Gets or sets the session. </summary>
    /// <value> The session. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public Pith.SessionBase? SessionBase { get; private set; }

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

        this._titleStatusStrip.Visible = sessionBase is not null;
        this._sessionReadingStatusStrip.Visible = sessionBase is not null;
        this._statusStrip.Visible = sessionBase is not null || this.StatusSubsystemBase is not null;
        // hide the sense and source status strip if nothing was assigned
        this._measureStatusStrip.Visible = this.MeasureToolstripSubsystemBase is not null;
        this._subsystemStatusStrip.Visible = this.SubsystemToolStripSentinel is not null;
        if ( sessionBase is not null )
        {
            this.SessionBase = sessionBase;
            this.BindSessionViewModel( true, this.SessionBase );
        }
    }

    /// <summary> Bind visibility. </summary>
    /// <param name="label">     The label. </param>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="viewModel"> The status subsystem view model. </param>
    /// <param name="bitValue">  The bit value (must include only a single bit). </param>
    private void BindVisibility( cc.isr.WinControls.ToolStripStatusLabel label, bool add, Pith.SessionBase viewModel, Pith.ServiceRequests bitValue )
    {
        Binding binding = this.AddRemoveBinding( label, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.ServiceRequestStatus ), DataSourceUpdateMode.Never );
        void eventHandler( object? sender, ConvertEventArgs e )
        {
            if ( e is not null && ReferenceEquals( e.DesiredType, typeof( bool ) ) )
            {
                e.Value = 0 != (bitValue & ( Pith.ServiceRequests ) ( int ) (e.Value ?? 0));
            }
        }
        if ( add )
        {
            binding.Format += eventHandler;
        }
        else
        {
            binding.Format -= eventHandler;
        }
    }

    /// <summary> Bind visibility. </summary>
    /// <param name="label">     The label. </param>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="viewModel"> The status subsystem view model. </param>
    /// <param name="bitValue">  The bit value (must include only a single bit). </param>
    private void BindVisibility( cc.isr.WinControls.ToolStripStatusLabel label, bool add, Pith.SessionBase viewModel, Pith.StandardEvents bitValue )
    {
        Binding binding = this.AddRemoveBinding( label, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.StandardEventStatus ), DataSourceUpdateMode.Never );
        void eventHandler( object? sender, ConvertEventArgs e )
        {
            if ( e is not null && ReferenceEquals( e.DesiredType, typeof( bool ) ) )
            {
                e.Value = 0 != (bitValue & ( Pith.StandardEvents ) ( int ) (e.Value ?? 0));
            }
        }
        if ( add )
        {
            binding.Format += eventHandler;
        }
        else
        {
            binding.Format -= eventHandler;
        }
    }

    /// <summary> Binds the Session view model to its controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="viewModel"> The status subsystem view model. </param>
    private void BindSessionViewModel( bool add, Pith.SessionBase viewModel )
    {
        _ = this.AddRemoveBinding( this._titleLabel, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.ResourceModelCaption ), DataSourceUpdateMode.Never );
        _ = this.AddRemoveBinding( this._statusRegisterLabel, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.StatusRegisterCaption ), DataSourceUpdateMode.Never );
        if ( DisplayView.AllSettings.Instance.DisplayViewSettings.DisplayStandardServiceRequests )
        {
            this.BindVisibility( this._measurementEventBitLabel, add, viewModel, Pith.ServiceRequests.MeasurementEvent );
            this.BindVisibility( this._systemEventBitLabel, add, viewModel, Pith.ServiceRequests.SystemEvent );
            this.BindVisibility( this._errorAvailableBitLabel, add, viewModel, Pith.ServiceRequests.ErrorAvailable );
            this.BindVisibility( this._messageAvailableBitLabel, add, viewModel, Pith.ServiceRequests.MessageAvailable );
            this.BindVisibility( this._questionableSummaryBitLabel, add, viewModel, Pith.ServiceRequests.QuestionableEvent );
            this.BindVisibility( this._eventSummaryBitLabel, add, viewModel, Pith.ServiceRequests.StandardEventSummary );
            this.BindVisibility( this._serviceRequestBitLabel, add, viewModel, Pith.ServiceRequests.RequestingService );
        }
        else
        {
            _ = this.AddRemoveBinding( this._measurementEventBitLabel, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.HasMeasurementEvent ), DataSourceUpdateMode.Never );
            _ = this.AddRemoveBinding( this._systemEventBitLabel, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.HasSystemEvent ), DataSourceUpdateMode.Never );
            _ = this.AddRemoveBinding( this._errorAvailableBitLabel, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.ErrorAvailable ), DataSourceUpdateMode.Never );
            _ = this.AddRemoveBinding( this._messageAvailableBitLabel, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.MessageAvailable ), DataSourceUpdateMode.Never );
            _ = this.AddRemoveBinding( this._questionableSummaryBitLabel, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.HasQuestionableEvent ), DataSourceUpdateMode.Never );
            _ = this.AddRemoveBinding( this._eventSummaryBitLabel, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.HasStandardEvent ), DataSourceUpdateMode.Never );
            _ = this.AddRemoveBinding( this._serviceRequestBitLabel, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.RequestedService ), DataSourceUpdateMode.Never );
        }

        _ = this.AddRemoveBinding( this._serviceRequestEnabledLabel, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.ServiceRequestEventEnabled ), DataSourceUpdateMode.Never );
        Binding binding = this.AddRemoveBinding( this._serviceRequestEnableBitmaskLabel, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.ServiceRequestEnableBitmask ), DataSourceUpdateMode.Never );
        if ( add )
        {
            binding.Format += BindingEventHandlers.DisplayX2EventHandler;
        }
        else
        {
            binding.Format += BindingEventHandlers.DisplayX2EventHandler;
        }

        binding = this.AddRemoveBinding( this._standardEventEnableBitmaskLabel, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.StandardEventEnableBitmask ), DataSourceUpdateMode.Never );
        if ( add )
        {
            binding.Format += BindingEventHandlers.DisplayX2EventHandler;
        }
        else
        {
            binding.Format += BindingEventHandlers.DisplayX2EventHandler;
        }

        _ = this.AddRemoveBinding( this._sessionElapsedTimeLabel, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.ElapsedTimeCaption ), DataSourceUpdateMode.Never );
        _ = this.AddRemoveBinding( this._standardRegisterLabel, add, nameof( Control.Text ), viewModel, nameof( Pith.SessionBase.StandardRegisterCaption ), DataSourceUpdateMode.Never );
        _ = this.AddRemoveBinding( this._standardRegisterLabel, add, nameof( this.Visible ), viewModel, nameof( Pith.SessionBase.StandardEventStatusHasValue ), DataSourceUpdateMode.Never );
        this.BindVisibility( this._powerOnEventLabel, add, viewModel, Pith.StandardEvents.PowerToggled );
        binding = this.AddRemoveBinding( this._sessionOpenCloseStatusLabel, add, nameof( ToolStripLabel.Image ), viewModel, nameof( Pith.SessionBase.IsDeviceOpen ), DataSourceUpdateMode.Never );
        static void eventHandler( object? sender, ConvertEventArgs e )
        {
            e.ToggleImage( Properties.Resources.user_online_2, Properties.Resources.user_offline_2 );
        }
        // ConvertEventHandler eventHandler = ( sender, e ) => e.ToggleImage( Properties.Resources.user_online_2, Properties.Resources.user_offline_2 );
        if ( add )
        {
            binding.Format += eventHandler;
        }
        else
        {
            binding.Format -= eventHandler;
        }

        this._sessionOpenCloseStatusLabel.Image = Properties.Resources.user_offline_2;
    }

    #endregion

    #region " status subsystem base "

    /// <summary> Gets or sets the status subsystem. </summary>
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

    /// <summary> Bind status subsystem base. </summary>
    /// <param name="statusSubsystemBase"> The status subsystem. </param>
    private void BindStatusSubsystemBaseThis( StatusSubsystemBase? statusSubsystemBase )
    {
        if ( this.StatusSubsystemBase is not null )
        {
            this.BindStatusSubsystemViewModel( false, this.StatusSubsystemBase );
            this.StatusSubsystemBase = null;
        }

        this._errorStatusStrip.Visible = statusSubsystemBase is not null;
        this._statusStrip.Visible = statusSubsystemBase is not null || this.SessionBase is not null;
        // hide the sense and source status strip if nothing was assigned
        this._measureStatusStrip.Visible = this.MeasureToolstripSubsystemBase is not null;
        this._subsystemStatusStrip.Visible = this.SubsystemToolStripSentinel is not null;
        if ( statusSubsystemBase is not null )
        {
            this.StatusSubsystemBase = statusSubsystemBase;
            this.BindStatusSubsystemViewModel( true, this.StatusSubsystemBase );
        }
    }

    /// <summary> Binds the StatusSubsystem view model to its controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="viewModel"> The status subsystem view model. </param>
    private void BindStatusSubsystemViewModel( bool add, StatusSubsystemBase? viewModel )
    {
        if ( viewModel is null ) return;
        _ = this.AddRemoveBinding( this._errorLabel, add, nameof( Control.Text ), viewModel, nameof( VI.StatusSubsystemBase.Session.LastErrorCompoundErrorMessage ) );
        _ = this.AddRemoveBinding( this._errorLabel, add, nameof( this.ForeColor ), viewModel, nameof( VI.StatusSubsystemBase.Session.ErrorForeColor ) );
        _ = this.AddRemoveBinding( this._measurementEventStatusLabel, add, nameof( this.Visible ), viewModel, nameof( VI.StatusSubsystemBase.MeasurementEventStatus ), DataSourceUpdateMode.Never );
        Binding binding = this.AddRemoveBinding( this._measurementEventStatusLabel, add, nameof( Control.Text ), viewModel, nameof( VI.StatusSubsystemBase.MeasurementEventStatus ), DataSourceUpdateMode.Never );
        if ( add )
        {
            binding.Format += BindingEventHandlers.DisplayX2EventHandler;
        }
        else
        {
            binding.Format += BindingEventHandlers.DisplayX2EventHandler;
        }
    }

    /// <summary> Event handler. Called by _errorLabel for text changed events. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information to send to registered event handlers. </param>
    private void ErrorLabel_TextChanges( object? sender, EventArgs e )
    {
        string activity = string.Empty;
        try
        {
            if ( this.StatusSubsystemBase is not null && (this.StatusSubsystemBase.Session.HasDeviceError
                || this.StatusSubsystemBase.Session.HasErrorReport) )
            {
                string value = string.Empty;
                value = this.StatusSubsystemBase.Session.HasErrorReport
                    ? this.StatusSubsystemBase.Session.DeviceErrorReport
                    : this.StatusSubsystemBase.Session.HasDeviceError
                        ? this.StatusSubsystemBase.Session.LastErrorCompoundErrorMessage
                        : string.Empty;

                if ( !string.IsNullOrWhiteSpace( value ) )
                {
                    activity = $"logging error label text: {value}";
                    _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"error(s) reported: \n{this.StatusSubsystemBase.Session.DeviceErrorPreamble}\n{value}" );
                }
            }
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " measure tool strip subsystems "

    /// <summary>
    /// Gets or sets the measure tool strip subsystem sentinel, which indicates if a measure
    /// subsystem was assigned.
    /// </summary>
    /// <value> The measure subsystem sentinel. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    private SubsystemBase? MeasureToolstripSubsystemBase { get; set; }

    /// <summary> Releases the subsystem from its measure tool strip controls. </summary>
    /// <param name="subsystem"> The System subsystem. </param>
    public void ReleaseMeasureToolStrip( SubsystemBase? subsystem )
    {
        if ( subsystem is not null )
        {
            if ( subsystem is BufferSubsystemBase bufferSubsystem )
            {
                this.BindMeasureToolStrip( false, bufferSubsystem );
            }
            else
            {
                if ( subsystem is MultimeterSubsystemBase meterSubsystem )
                {
                    this.BindMeasureToolStrip( false, meterSubsystem );
                }
                else
                {
                    if ( subsystem is MeasureSubsystemBase measureSubsystem )
                    {
                        this.BindMeasureToolStrip( false, measureSubsystem );
                    }
                    else
                    {
                        if ( subsystem is MeasureCurrentSubsystemBase measureCurrentSubsystem )
                        {
                            this.BindMeasureToolStrip( false, measureCurrentSubsystem );
                        }
                        else
                        {
                            if ( subsystem is MeasureVoltageSubsystemBase measureVoltageSubsystem )
                            {
                                this.BindMeasureToolStrip( false, measureVoltageSubsystem );
                            }
                            else
                            {
                                if ( subsystem is SenseFunctionSubsystemBase senseFunctionSubsystem )
                                {
                                    this.BindMeasureToolStrip( false, senseFunctionSubsystem );
                                }
                                else
                                {
                                    if ( subsystem is SenseSubsystemBase senseSubsystem )
                                    {
                                        this.BindMeasureToolStrip( false, senseSubsystem );
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #region " buffer subsystem model view "

    /// <summary> Binds the buffer  subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The buffer subsystem. </param>
    public void BindMeasureToolStrip( BufferSubsystemBase subsystem )
    {
        if ( this.MeasureToolstripSubsystemBase is not null )
        {
            this.ReleaseMeasureToolStrip( this.MeasureToolstripSubsystemBase );
        }

        this.MeasureToolstripSubsystemBase = subsystem;
        this._measureStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindMeasureToolStrip( true, subsystem );
        }
    }

    /// <summary> Binds the buffer subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The buffer subsystem. </param>
    private void BindMeasureToolStrip( bool add, BufferSubsystemBase subsystem )
    {
        // Me.AddRemoveBinding(Me._measureMetaStatusLabel, add, NameOf(Control.ForeColor), viewModel, NameOf(isr.VI.BufferSubsystemBase.FailureColor))
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( Control.Text ), subsystem, nameof( BufferSubsystemBase.LastReadingStatus ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( BufferSubsystemBase.LastReadingStatus ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( BufferSubsystemBase.LastReading ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.Text ), subsystem, nameof( BufferSubsystemBase.LastReadingCaption ) );
        _ = this.AddRemoveBinding( this._measureElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( BufferSubsystemBase.ElapsedTimeCaption ) );
        _ = this.AddRemoveBinding( this._lastReadingLabel, add, nameof( Control.Text ), subsystem, nameof( BufferSubsystemBase.LastRawReading ) );
    }

    #endregion

    #region " harmonics measure subsystem model view "

    /// <summary> Binds the Harmonics Measure Subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The Harmonics Measure Subsystem. </param>
    public void BindMeasureToolStrip( HarmonicsMeasureSubsystemBase? subsystem )
    {
        if ( this.MeasureToolstripSubsystemBase is not null )
        {
            this.ReleaseMeasureToolStrip( this.MeasureToolstripSubsystemBase );
        }

        this.MeasureToolstripSubsystemBase = subsystem;
        this._measureStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindMeasureToolStrip( true, subsystem );
        }

        this.BindTerminalsDisplay( subsystem );
    }

    /// <summary> Binds the Harmonics Measure Subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The Harmonics Measure Subsystem. </param>
    private void BindMeasureToolStrip( bool add, HarmonicsMeasureSubsystemBase? subsystem )
    {
        if ( subsystem is null ) return;
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( this.ForeColor ), subsystem, nameof( HarmonicsMeasureSubsystemBase.FailureColor ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( Control.Text ), subsystem, nameof( HarmonicsMeasureSubsystemBase.FailureCode ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( HarmonicsMeasureSubsystemBase.FailureLongDescription ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( HarmonicsMeasureSubsystemBase.LastReading ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.Text ), subsystem, nameof( HarmonicsMeasureSubsystemBase.ReadingCaption ) );
        _ = this.AddRemoveBinding( this._measureElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( HarmonicsMeasureSubsystemBase.ElapsedTimeCaption ) );
        _ = this.AddRemoveBinding( this._lastReadingLabel, add, nameof( Control.Text ), subsystem, nameof( HarmonicsMeasureSubsystemBase.LastReading ) );
    }

    /// <summary> Binds the Harmonics Measure Subsystem to the its terminals controls. </summary>
    /// <param name="subsystem"> The System subsystem. </param>
    public void BindTerminalsDisplay( HarmonicsMeasureSubsystemBase? subsystem )
    {
        if ( subsystem?.SupportsFrontTerminalsSelectionQuery == true )
        {
            if ( this.TerminalsSubsystemBase is not null )
            {
                this.ReleaseTerminalsDisplay( this.TerminalsSubsystemBase );
            }

            this.TerminalsSubsystemBase = subsystem;
            if ( subsystem is not null )
            {
                this.BindTerminalsDisplay( true, subsystem );
            }
        }
    }

    /// <summary> Binds the Harmonics Measure Subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The System subsystem. </param>
    private void BindTerminalsDisplay( bool add, HarmonicsMeasureSubsystemBase? subsystem )
    {
        if ( subsystem is not null && subsystem.SupportsFrontTerminalsSelectionQuery )
        {
            _ = this.AddRemoveBinding( this._terminalsStatusLabel, add, nameof( Control.Text ), subsystem, nameof( HarmonicsMeasureSubsystemBase.TerminalsCaption ) );
            this._terminalsStatusLabel.Visible = add;
            if ( add )
            {
                this._terminalsStatusLabel.Text = subsystem.TerminalsCaption;
            }
        }
    }

    #endregion

    #region " measure subsystem model view "

    /// <summary> Binds the Measure subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The Measure subsystem. </param>
    public void BindMeasureToolStrip( MeasureSubsystemBase? subsystem )
    {
        if ( this.MeasureToolstripSubsystemBase is not null )
        {
            this.ReleaseMeasureToolStrip( this.MeasureToolstripSubsystemBase );
        }

        this.MeasureToolstripSubsystemBase = subsystem;
        this._measureStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindMeasureToolStrip( true, subsystem );
        }

        this.BindTerminalsDisplay( subsystem );
    }

    /// <summary> Binds the Measure subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The Measure subsystem. </param>
    private void BindMeasureToolStrip( bool add, MeasureSubsystemBase subsystem )
    {
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( this.ForeColor ), subsystem, nameof( MeasureSubsystemBase.FailureColor ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( Control.Text ), subsystem, nameof( MeasureSubsystemBase.FailureCode ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( MeasureSubsystemBase.FailureLongDescription ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( MeasureSubsystemBase.LastReading ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.Text ), subsystem, nameof( MeasureSubsystemBase.ReadingCaption ) );
        _ = this.AddRemoveBinding( this._measureElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( MeasureSubsystemBase.ElapsedTimeCaption ) );
        _ = this.AddRemoveBinding( this._lastReadingLabel, add, nameof( Control.Text ), subsystem, nameof( MeasureSubsystemBase.LastReading ) );
    }

    #endregion

    #region " measure current subsystem model view "

    /// <summary> Binds the Measure subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The Measure Current subsystem. </param>
    public void BindMeasureToolStrip( MeasureCurrentSubsystemBase subsystem )
    {
        if ( this.MeasureToolstripSubsystemBase is not null )
        {
            this.ReleaseMeasureToolStrip( this.MeasureToolstripSubsystemBase );
        }

        this.MeasureToolstripSubsystemBase = subsystem;
        this._measureStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindMeasureToolStrip( true, subsystem );
        }
    }

    /// <summary> Binds the Measure Current subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The Measure Current. </param>
    private void BindMeasureToolStrip( bool add, MeasureCurrentSubsystemBase subsystem )
    {
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( MeasureCurrentSubsystemBase.LastReading ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.Text ), subsystem, nameof( MeasureCurrentSubsystemBase.ReadingCaption ) );
        _ = this.AddRemoveBinding( this._measureElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( MeasureCurrentSubsystemBase.ElapsedTimeCaption ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( this.ForeColor ), subsystem, nameof( MeasureCurrentSubsystemBase.FailureColor ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( Control.Text ), subsystem, nameof( MeasureCurrentSubsystemBase.FailureCode ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( MeasureCurrentSubsystemBase.FailureLongDescription ) );
        _ = this.AddRemoveBinding( this._lastReadingLabel, add, nameof( Control.Text ), subsystem, nameof( MeasureCurrentSubsystemBase.LastReading ) );
    }

    #endregion

    #region " measure voltage subsystem model view "

    /// <summary> Binds the Measure Voltage subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The Measure Voltage subsystem. </param>
    public void BindMeasureToolStrip( MeasureVoltageSubsystemBase subsystem )
    {
        if ( this.MeasureToolstripSubsystemBase is not null )
        {
            this.ReleaseMeasureToolStrip( this.MeasureToolstripSubsystemBase );
        }

        this.MeasureToolstripSubsystemBase = subsystem;
        this._measureStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindMeasureToolStrip( true, subsystem );
        }
    }

    /// <summary> Binds the Measure Voltage subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The Measure Voltage subsystem. </param>
    private void BindMeasureToolStrip( bool add, MeasureVoltageSubsystemBase subsystem )
    {
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( MeasureVoltageSubsystemBase.LastReading ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.Text ), subsystem, nameof( MeasureVoltageSubsystemBase.ReadingCaption ) );
        _ = this.AddRemoveBinding( this._measureElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( MeasureVoltageSubsystemBase.ElapsedTimeCaption ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( this.ForeColor ), subsystem, nameof( MeasureVoltageSubsystemBase.FailureColor ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( Control.Text ), subsystem, nameof( MeasureVoltageSubsystemBase.FailureCode ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( MeasureVoltageSubsystemBase.FailureLongDescription ) );
        _ = this.AddRemoveBinding( this._lastReadingLabel, add, nameof( Control.Text ), subsystem, nameof( MeasureVoltageSubsystemBase.LastReading ) );
    }

    #endregion

    #region " multimeter subsystem model view "

    /// <summary> Binds the Multimeter subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The Multimeter subsystem. </param>
    public void BindMeasureToolStrip( MultimeterSubsystemBase subsystem )
    {
        if ( this.MeasureToolstripSubsystemBase is not null )
        {
            this.ReleaseMeasureToolStrip( this.MeasureToolstripSubsystemBase );
        }

        this.MeasureToolstripSubsystemBase = subsystem;
        this._measureStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindMeasureToolStrip( true, subsystem );
        }
    }

    /// <summary> Binds the Multimeter subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The Multimeter subsystem. </param>
    private void BindMeasureToolStrip( bool add, MultimeterSubsystemBase subsystem )
    {
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( MultimeterSubsystemBase.LastReading ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.Text ), subsystem, nameof( MultimeterSubsystemBase.ReadingCaption ) );
        _ = this.AddRemoveBinding( this._measureElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( MultimeterSubsystemBase.ElapsedTimeCaption ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( this.ForeColor ), subsystem, nameof( MultimeterSubsystemBase.FailureColor ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( Control.Text ), subsystem, nameof( MultimeterSubsystemBase.FailureCode ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( MultimeterSubsystemBase.FailureLongDescription ) );
        _ = this.AddRemoveBinding( this._lastReadingLabel, add, nameof( Control.Text ), subsystem, nameof( MultimeterSubsystemBase.LastReading ) );
    }

    #endregion

    #region " sense function subsystem model view "

    /// <summary> Binds the Sense Function subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The Sense Function subsystem. </param>
    public void BindMeasureToolStrip( SenseFunctionSubsystemBase subsystem )
    {
        if ( this.MeasureToolstripSubsystemBase is not null )
        {
            this.ReleaseMeasureToolStrip( this.MeasureToolstripSubsystemBase );
        }

        this.MeasureToolstripSubsystemBase = subsystem;
        this._measureStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindMeasureToolStrip( true, subsystem );
        }
    }

    /// <summary> Binds the Sense Function subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The Sense Function subsystem. </param>
    private void BindMeasureToolStrip( bool add, SenseFunctionSubsystemBase subsystem )
    {
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( SenseFunctionSubsystemBase.LastReading ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.Text ), subsystem, nameof( SenseFunctionSubsystemBase.ReadingCaption ) );
        _ = this.AddRemoveBinding( this._measureElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( SenseFunctionSubsystemBase.ElapsedTimeCaption ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( this.ForeColor ), subsystem, nameof( SenseFunctionSubsystemBase.FailureColor ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( Control.Text ), subsystem, nameof( SenseFunctionSubsystemBase.FailureCode ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( SenseFunctionSubsystemBase.FailureLongDescription ) );
        _ = this.AddRemoveBinding( this._lastReadingLabel, add, nameof( Control.Text ), subsystem, nameof( SenseFunctionSubsystemBase.LastReading ) );
    }

    #endregion

    #region " sense subsystem model view "

    /// <summary> Binds the Sense subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The Sense subsystem. </param>
    public void BindMeasureToolStrip( SenseSubsystemBase subsystem )
    {
        if ( this.MeasureToolstripSubsystemBase is not null )
        {
            this.ReleaseMeasureToolStrip( this.MeasureToolstripSubsystemBase );
        }

        this.MeasureToolstripSubsystemBase = subsystem;
        this._measureStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindMeasureToolStrip( true, subsystem );
        }
    }

    /// <summary> Binds the Sense subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The Sense Function subsystem. </param>
    private void BindMeasureToolStrip( bool add, SenseSubsystemBase subsystem )
    {
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( SenseSubsystemBase.LastReading ) );
        _ = this.AddRemoveBinding( this._readingAmountLabel, add, nameof( ToolStripItem.Text ), subsystem, nameof( SenseSubsystemBase.ReadingCaption ) );
        _ = this.AddRemoveBinding( this._measureElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( SenseSubsystemBase.ElapsedTimeCaption ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( this.ForeColor ), subsystem, nameof( SenseSubsystemBase.FailureColor ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( Control.Text ), subsystem, nameof( SenseSubsystemBase.FailureCode ) );
        _ = this.AddRemoveBinding( this._measureMetaStatusLabel, add, nameof( ToolStripItem.ToolTipText ), subsystem, nameof( SenseSubsystemBase.FailureLongDescription ) );
        _ = this.AddRemoveBinding( this._lastReadingLabel, add, nameof( Control.Text ), subsystem, nameof( SenseSubsystemBase.LastReading ) );
    }

    #endregion

    #endregion

    #region " terminals label binding "

    /// <summary>
    /// Gets or sets the terminals subsystem sentinel, which indicates if a terminals subsystem was
    /// assigned.
    /// </summary>
    /// <value> The measure subsystem sentinel. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    private SubsystemBase? TerminalsSubsystemBase { get; set; }

    /// <summary> Releases the subsystem from its terminals controls. </summary>
    /// <param name="subsystem"> The System subsystem. </param>
    public void ReleaseTerminalsDisplay( SubsystemBase subsystem )
    {
        if ( subsystem is not null )
        {
            if ( subsystem is SystemSubsystemBase sysSubsystem )
            {
                this.BindTerminalsDisplay( false, sysSubsystem );
            }
            else
            {
                if ( subsystem is MultimeterSubsystemBase meterSubsystem )
                {
                    this.BindTerminalsDisplay( false, meterSubsystem );
                }
                else
                {
                    if ( subsystem is MeasureSubsystemBase measureSubsystem )
                    {
                        this.BindTerminalsDisplay( false, measureSubsystem );
                    }
                }
            }
        }
    }

    #region " measure subsystem "

    /// <summary> Binds the Measure subsystem to the its terminals controls. </summary>
    /// <param name="subsystem"> The System subsystem. </param>
    public void BindTerminalsDisplay( MeasureSubsystemBase? subsystem )
    {
        if ( subsystem?.SupportsFrontTerminalsSelectionQuery == true )
        {
            if ( this.TerminalsSubsystemBase is not null )
            {
                this.ReleaseTerminalsDisplay( this.TerminalsSubsystemBase );
            }

            this.TerminalsSubsystemBase = subsystem;
            if ( subsystem is not null )
            {
                this.BindTerminalsDisplay( true, subsystem );
            }
        }
    }

    /// <summary> Binds the Measure subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The System subsystem. </param>
    private void BindTerminalsDisplay( bool add, MeasureSubsystemBase? subsystem )
    {
        if ( subsystem is not null && subsystem.SupportsFrontTerminalsSelectionQuery )
        {
            _ = this.AddRemoveBinding( this._terminalsStatusLabel, add, nameof( Control.Text ), subsystem, nameof( MeasureSubsystemBase.TerminalsCaption ) );
            this._terminalsStatusLabel.Visible = add;
            if ( add )
            {
                this._terminalsStatusLabel.Text = subsystem.TerminalsCaption;
            }
        }
    }

    #endregion

    #region " multimeter subsystem "

    /// <summary> Binds the multimeter subsystem to the its terminals controls. </summary>
    /// <param name="subsystem"> The System subsystem. </param>
    public void BindTerminalsDisplay( MultimeterSubsystemBase subsystem )
    {
        if ( subsystem?.SupportsFrontTerminalsSelectionQuery == true )
        {
            if ( this.TerminalsSubsystemBase is not null )
            {
                this.ReleaseTerminalsDisplay( this.TerminalsSubsystemBase );
            }

            this.TerminalsSubsystemBase = subsystem;
            if ( subsystem is not null )
            {
                this.BindTerminalsDisplay( true, subsystem );
            }
        }
    }

    /// <summary> Binds the multimeter subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The System subsystem. </param>
    private void BindTerminalsDisplay( bool add, MultimeterSubsystemBase subsystem )
    {
        if ( subsystem.SupportsFrontTerminalsSelectionQuery )
        {
            _ = this.AddRemoveBinding( this._terminalsStatusLabel, add, nameof( Control.Text ), subsystem, nameof( MultimeterSubsystemBase.TerminalsCaption ) );
            this._terminalsStatusLabel.Visible = add;
            if ( add )
            {
                this._terminalsStatusLabel.Text = subsystem.TerminalsCaption;
            }
        }
    }

    #endregion

    #region " system subsystem "

    /// <summary> Binds the System subsystem to the its terminals controls. </summary>
    /// <param name="subsystem"> The System subsystem. </param>
    public void BindTerminalsDisplay( SystemSubsystemBase subsystem )
    {
        if ( subsystem?.SupportsFrontTerminalsSelectionQuery == true )
        {
            if ( this.TerminalsSubsystemBase is not null )
            {
                this.ReleaseTerminalsDisplay( this.TerminalsSubsystemBase );
            }

            this.TerminalsSubsystemBase = subsystem;
            if ( subsystem is not null )
            {
                this.BindTerminalsDisplay( true, subsystem );
            }
        }
    }

    /// <summary> Binds the System subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The System subsystem. </param>
    private void BindTerminalsDisplay( bool add, SystemSubsystemBase subsystem )
    {
        if ( subsystem.SupportsFrontTerminalsSelectionQuery )
        {
            _ = this.AddRemoveBinding( this._terminalsStatusLabel, add, nameof( Control.Text ), subsystem, nameof( SystemSubsystemBase.TerminalsCaption ) );
            this._terminalsStatusLabel.Visible = add;
            if ( add )
            {
                this._terminalsStatusLabel.Text = subsystem.TerminalsCaption;
            }
        }
    }

    #endregion

    #endregion

    #region " subsystem tool strip sentinel "

    /// <summary> Gets or sets sentinel for the subsystem tool strip. </summary>
    /// <value> The sentinel for the subsystem tool strip. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    private SubsystemBase? SubsystemToolStripSentinel { get; set; }

    /// <summary> Releases the subsystem from its SUBSYSTEM controls. </summary>
    /// <param name="subsystem"> The System subsystem. </param>
    public void ReleaseSubsystemToolStrip( SubsystemBase subsystem )
    {
        if ( subsystem is not null )
        {
            if ( subsystem is SourceSubsystemBase sourceSubsystem )
            {
                this.BindSubsystemToolStrip( false, sourceSubsystem );
            }
            else
            {
                if ( subsystem is ChannelSubsystemBase channelSubsystem )
                {
                    this.BindSubsystemToolStrip( false, channelSubsystem );
                }
                else
                {
                    if ( subsystem is TriggerSubsystemBase triggerSubsystem )
                    {
                        this.BindSubsystemToolStrip( false, triggerSubsystem );
                    }
                }
            }
        }
    }


    #region " source subsystem model view "

    /// <summary> Binds the Source subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The Source subsystem. </param>
    public void BindSubsystemToolStrip( SourceSubsystemBase subsystem )
    {
        if ( this.SubsystemToolStripSentinel is not null )
        {
            this.ReleaseSubsystemToolStrip( this.SubsystemToolStripSentinel );
        }

        this.SubsystemToolStripSentinel = subsystem;
        this._subsystemStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindSubsystemToolStrip( true, subsystem );
        }
    }

    /// <summary> Binds the Source subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The Source subsystem. </param>
    private void BindSubsystemToolStrip( bool add, SourceSubsystemBase subsystem )
    {
        _ = this.AddRemoveBinding( this._subsystemReadingLabel, add, nameof( Control.Text ), subsystem, nameof( SourceSubsystemBase.LevelCaption ) );
        _ = this.AddRemoveBinding( this._subsystemElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( SubsystemBase.ElapsedTimeCaption ) );
    }

    #endregion

    #region " channel subsystem model view "

    /// <summary> Binds the Channel subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The Channel subsystem. </param>
    public void BindSubsystemToolStrip( ChannelSubsystemBase subsystem )
    {
        if ( this.SubsystemToolStripSentinel is not null )
        {
            this.ReleaseSubsystemToolStrip( this.SubsystemToolStripSentinel );
        }

        this.SubsystemToolStripSentinel = subsystem;
        this._subsystemStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindSubsystemToolStrip( true, subsystem );
        }
    }

    /// <summary> Binds the Channel subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The Channel subsystem. </param>
    private void BindSubsystemToolStrip( bool add, ChannelSubsystemBase subsystem )
    {
        _ = this.AddRemoveBinding( this._subsystemReadingLabel, add, nameof( Control.Text ), subsystem, nameof( ChannelSubsystemBase.ClosedChannelsCaption ) );
        _ = this.AddRemoveBinding( this._subsystemElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( SubsystemBase.ElapsedTimeCaption ) );
    }

    #endregion

    #region " trigger subsystem model view "

    /// <summary> Binds the Trigger subsystem to the its tool strip controls. </summary>
    /// <param name="subsystem"> The Trigger subsystem. </param>
    public void BindSubsystemToolStrip( TriggerSubsystemBase subsystem )
    {
        if ( this.SubsystemToolStripSentinel is not null )
        {
            this.ReleaseSubsystemToolStrip( this.SubsystemToolStripSentinel );
        }

        this.SubsystemToolStripSentinel = subsystem;
        this._subsystemStatusStrip.Visible = subsystem is not null;
        if ( subsystem is not null )
        {
            this.BindSubsystemToolStrip( true, subsystem );
        }
    }

    /// <summary> Binds the Trigger subsystem to the its tool strip controls. </summary>
    /// <param name="add">       True to add; otherwise, remove. </param>
    /// <param name="subsystem"> The Trigger subsystem. </param>
    private void BindSubsystemToolStrip( bool add, TriggerSubsystemBase subsystem )
    {
        _ = this.AddRemoveBinding( this._subsystemReadingLabel, add, nameof( Control.Text ), subsystem, nameof( TriggerSubsystemBase.TriggerStateCaption ) );
        _ = this.AddRemoveBinding( this._subsystemElapsedTimeLabel, add, nameof( Control.Text ), subsystem, nameof( SubsystemBase.ElapsedTimeCaption ) );
    }

    #endregion

    #endregion

    #region " bind settings "

    /// <summary>   Sets charcoal back color. </summary>
    /// <remarks>   2025-01-13. </remarks>
    public void SetCharcoalBackColor()
    {
        this.SetBackColors( DisplayView.AllSettings.Instance.DisplayViewSettings.CharcoalColor );
    }

    /// <summary>   Sets back colors. </summary>
    /// <remarks>   2025-01-13. </remarks>
    /// <param name="backColor">    The back color. </param>
    public void SetBackColors( System.Drawing.Color backColor )
    {
        this._layout.BackColor = backColor;
        this._sessionReadingStatusStrip.BackColor = backColor;
        this._subsystemStatusStrip.BackColor = backColor;
        this._titleStatusStrip.BackColor = backColor;
        this._errorStatusStrip.BackColor = backColor;
        this._statusStrip.BackColor = backColor;
    }

    private static void CloneBinding( Control control, Binding binding )
    {
        Binding bind = new( binding.PropertyName, binding.DataSource, binding.BindingMemberInfo.BindingMember,
            true, DataSourceUpdateMode.OnPropertyChanged );
        if ( control.DataBindings.Exists( bind ) ) { control.DataBindings.Remove( bind ); }
        control.DataBindings.Add( bind );
    }

    /// <summary> Bind settings. </summary>
    public void BindSettings()
    {
        this._layout.BackColor = DisplayView.AllSettings.Instance.DisplayViewSettings.CharcoalColor;
        Binding binding = new( nameof( this.BackColor ), DisplayView.AllSettings.Instance.DisplayViewSettings,
            nameof( DisplayView.AllSettings.Instance.DisplayViewSettings.CharcoalColor ), true, DataSourceUpdateMode.OnPropertyChanged );
        CloneBinding( this._layout, binding );
        // if ( this._layout.DataBindings.Exists( binding ) ) { this._layout.DataBindings.Remove( binding ); }
        // this._layout.DataBindings.Add( binding );

        this._sessionReadingStatusStrip.BackColor = DisplayView.AllSettings.Instance.DisplayViewSettings.CharcoalColor;
        CloneBinding( this._sessionReadingStatusStrip, binding );

        // if ( this._sessionReadingStatusStrip.DataBindings.Exists( binding ) ) { this._layout.DataBindings.Remove( binding ); }
        // this._sessionReadingStatusStrip.DataBindings.Add( binding );

        this._subsystemStatusStrip.BackColor = DisplayView.AllSettings.Instance.DisplayViewSettings.CharcoalColor;
        CloneBinding( this._subsystemStatusStrip, binding );

        // if ( this._subsystemStatusStrip.DataBindings.Exists( binding ) ) { this._layout.DataBindings.Remove( binding ); }
        // this._subsystemStatusStrip.DataBindings.Add( binding );

        this._titleStatusStrip.BackColor = DisplayView.AllSettings.Instance.DisplayViewSettings.CharcoalColor;
        CloneBinding( this._titleStatusStrip, binding );

        // if ( this._titleStatusStrip.DataBindings.Exists( binding ) ) { this._layout.DataBindings.Remove( binding ); }
        // this._titleStatusStrip.DataBindings.Add( binding );

        this._errorStatusStrip.BackColor = DisplayView.AllSettings.Instance.DisplayViewSettings.CharcoalColor;
        CloneBinding( this._errorStatusStrip, binding );

        // if ( this._errorStatusStrip.DataBindings.Exists( binding ) ) { this._layout.DataBindings.Remove( binding ); }
        // this._errorStatusStrip.DataBindings.Add( binding );

        this._statusStrip.BackColor = DisplayView.AllSettings.Instance.DisplayViewSettings.CharcoalColor;
        CloneBinding( this._statusStrip, binding );

        // if ( this._statusStrip.DataBindings.Exists( binding ) ) { this._layout.DataBindings.Remove( binding ); }
        // this._statusStrip.DataBindings.Add( binding );
    }

    #endregion
}
