using System.ComponentModel;
using cc.isr.Std.EscapeSequencesExtensions;
using cc.isr.VI.Pith;
using cc.isr.VI.Subsystem;

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by Status Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class StatusSubsystemBase : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IPresettable
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusSubsystemBase" /> class.
    /// </summary>
    /// <param name="session">                A reference to a <see cref="VI.Pith.SessionBase">message based
    /// session</see>. </param>
    protected StatusSubsystemBase( Pith.SessionBase session )
    {
        this._presetRefractoryPeriod = TimeSpan.FromMilliseconds( 100d );

        if ( session is null )
            throw new ArgumentNullException( nameof( session ), $"{nameof( session )} is required." );

        if ( string.IsNullOrWhiteSpace( session.ResourcesFilter ) )
            session.ResourcesFilter = SessionFactory.Instance.Factory.ResourcesProvider().ResourceFinder!.BuildMinimalResourcesFilter();

        this.Session = session;
        this.Session.PropertyChanged += this.SessionPropertyChanged;
        this.Session.DeviceErrorOccurred += this.HandleDeviceErrorOccurred;

        this._expectedLanguage = string.Empty;
        this._initializeTimeout = TimeSpan.FromMilliseconds( 5000d );

        this.OperationEventMap = new Dictionary<int, string>();
        this._identity = string.Empty;
        this._versionInfoBase = new VersionInfo();

        this.MeasurementEventsBitmasks = [];
        this.QuestionableEventsBitmasks = [];
        this.OperationEventsBitmasks = [];
        this.QuestionableEventMap = new Dictionary<int, string>();
    }

    /// <summary> Validated the given status subsystem base. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="statusSubsystemBase"> The status subsystem base. </param>
    /// <returns> A StatusSubsystemBase. </returns>
    public static VI.StatusSubsystemBase Validated( VI.StatusSubsystemBase statusSubsystemBase )
    {
        return statusSubsystemBase is null ? throw new ArgumentNullException( nameof( statusSubsystemBase ) ) : statusSubsystemBase;
    }

    #endregion

    #region " notify property change implementation "

    /// <summary>   Notifies a property changed. </summary>
    /// <remarks>   David, 2021-02-01. </remarks>
    /// <param name="propertyName"> (Optional) Name of the property. </param>
    protected void NotifyPropertyChanged( [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "" )
    {
        this.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
    }

    /// <summary>   Removes the property changed event handlers. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    protected void RemovePropertyChangedEventHandlers()
    {
        MulticastDelegate? event_delegate = ( MulticastDelegate ) this.GetType().GetField( nameof( this.PropertyChanged ),
                                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
                                        | System.Reflection.BindingFlags.GetField ).GetValue( this );
        Delegate[]? delegates = event_delegate.GetInvocationList();
        if ( delegates is not null )
        {
            foreach ( Delegate? item in delegates )
            {
                this.PropertyChanged -= ( PropertyChangedEventHandler ) item;
            }
        }
    }

    #endregion

    #region " i presettable "

    /// <summary> Executes the device open actions. </summary>
    /// <remarks>
    /// This was added in order to defer reading error status after clearing the device state when
    /// opening the device.
    /// </remarks>
    public virtual void OnDeviceOpen()
    {
        // clear the device active state
        this.Session.ClearActiveState();

        // reset device
        this.Session.ResetKnownState();

        this.DefineKnownResetState();

        // Clear the device Status and set more defaults
        this.Session.ClearExecutionState();

        // define the clear state.
        this.DefineClearExecutionState();
    }

    /// <summary>
    /// Defines the clear execution state (CLS) by setting system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    /// <remarks> Clears the queues and sets all registers to zero. </remarks>
    public virtual void DefineClearExecutionState()
    {
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Customizes the reset and clear state. A set of actions was cleared below assuming that this
    /// method rans after reset and clear.  <para>
    /// This will be watched in the future in case some instruments require additional action.
    /// For example, some instruments may not reset the error queue upon clear. So this can be used
    /// in these special cases.
    /// </para>
    /// </remarks>
    public virtual void InitKnownState()
    {
        if ( this.Session is null || !this.Session.IsSessionOpen ) return;

        Pith.SessionBase.DoEventsAction?.Invoke();

        // 20240916 cleanup
#if false
        // ?todo: testing using OPC in place of the refractory periods.
        if ( this.Session.SupportsOperationComplete )
        {
            _ = this.Session.QueryOperationCompleted();
            _ = SessionBase.AsyncDelay( this.Session.StatusReadDelay );
        }
        else
            _ = SessionBase.AsyncDelay( this.Session.SessionSettings.InitKnownStateTimeout );
#endif
        string activity = string.Empty;

#if false
        // CLS clears the error queue.
        try
        {
            this.Session.SetLastAction( "clearing error queue" );
            this.ClearErrorQueue();
        }
        catch ( Exception ex )
        {
            ex.Data.Add( $"{ex.Data.Count}-resource", this.Session.ResourceNameCaption );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, this.Session.LastAction );
        }

        try
        {
            // this is done in Define Clear State.
            activity = "clearing error cache";
            this.ClearErrorCache();

            // any pending buffer data will cause an exception and abort opening the instrument.
            // therefore, this is not necessary.
            activity = "clearing discarded data";
            this.Session.ClearDiscardedData();
        }
        catch ( Exception ex )
        {
            ex.Data.Add( $"{ex.Data.Count}-resource", this.Session.ResourceNameCaption );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }

#endif
        try
        {
            this.Session.SetLastAction( "defining wait complete bit masks with readback" );
            this.Session.EnableServiceRequestOnOperationCompletion( this.Session.RegistersBitmasksSettings.StandardEventEnableOperationCompleteBitmask,
                this.Session.RegistersBitmasksSettings.ServiceRequestEnableOperationCompleteBitmask, true );
        }
        catch ( Exception ex )
        {
            ex.Data.Add( $"{ex.Data.Count}-resource", this.Session.ResourceNameCaption );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, this.Session.LastAction );
        }

        try
        {
            this.Session.SetLastAction( "clearing output queue" );
            ServiceRequests statusByte = this.Session.DiscardUnreadData( this.Session.StatusReadDelay );
            this.Session.ThrowDeviceExceptionIfError( statusByte );
        }
        catch ( Exception ex )
        {
            ex.Data.Add( $"{ex.Data.Count}-resource", this.Session.ResourceNameCaption );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, this.Session.LastAction );
        }

        try
        {
            this.Session.SetLastAction( "querying identity" );
            _ = this.QueryIdentity();
        }
        catch ( Exception ex )
        {
            ex.Data.Add( $"{ex.Data.Count}-resource", this.Session.ResourceNameCaption );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, this.Session.LastAction );
        }

        try
        {
            this.Session.SetLastAction( "defining register event bit values" );
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.DefineEventBitmasks();
#if false
            // this defetes the previous OPC Events enable.
            this.Session.SetLastAction( "enable service request event bits" );
            this.Session.ApplyStatusByteEnableBitmask( this.Session.ServiceRequestEnableEventsBitmask );
#endif
        }
        catch ( Exception ex )
        {
            ex.Data.Add( $"{ex.Data.Count}-resource", this.Session.ResourceNameCaption );
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, this.Session.LastAction );
        }
    }

    /// <summary> The initialize operationCompletionTimeout. </summary>
    private TimeSpan _initializeTimeout;

    /// <summary> Gets or sets the time out for doing a reset and clear on the instrument. </summary>
    /// <value> The connect operationCompletionTimeout. </value>
    public TimeSpan InitializeTimeout
    {
        get => this._initializeTimeout;
        set => _ = this.SetProperty( ref this._initializeTimeout, value );
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    /// <remarks> Clears the queues and sets all registers to zero. </remarks>
    public void DefineKnownResetState()
    {
        this.Identity = string.Empty;

        this.Session.SetLastAction( "resetting registered known state" );
        this.ResetRegistersKnownState();

        this.Session.SetLastAction( "reading line frequency" );
        _ = this.QueryLineFrequency();

        this.Session.ThrowDeviceExceptionIfError();
    }

    /// <summary> Gets or sets the preset command. </summary>
    /// <remarks>
    /// SCPI: ":STAT:PRES".
    /// <see cref="VI.Syntax.ScpiSyntax.StatusPresetCommand"> </see>
    /// </remarks>
    /// <value> The preset command. </value>
    protected virtual string PresetCommand { get; set; } = VI.Syntax.ScpiSyntax.StatusPresetCommand;

    /// <summary> Returns the instrument registers to there preset power on state. </summary>
    /// <remarks>
    /// SCPI: "*???".<para>
    /// <see cref="VI.Syntax.Ieee488Syntax.ClearExecutionStateCommand"> </see> </para><para>
    /// When this command is sent, the SCPI event registers are affected as follows:<p>
    /// 1. All bits of the positive transition filter registers are set to one (1).</p><p>
    /// 2. All bits of the negative transition filter registers are cleared to zero (0).</p><p>
    /// 3. All bits of the following registers are cleared to zero (0):</p><p>
    /// a. Operation Event Enable Register.</p><p>
    /// b. Questionable Event Enable Register.</p><p>
    /// 4. All bits of the following registers are set to one (1):</p><p>
    /// a. Trigger Event Enable Register.</p><p>
    /// b. Arm Event Enable Register.</p><p>
    /// c. Sequence Event Enable Register.</p><p>
    /// Note: Registers not included in the above list are not affected by this command.</p> </para>
    /// </remarks>
    public void PresetKnownState()
    {
        if ( this.Session.IsDeviceOpen && !string.IsNullOrWhiteSpace( this.PresetCommand ) )
        {
            this.PresetRegistersKnownState();
            this.Session.OperationCompleted = new bool?();
            _ = this.Session.WriteLine( this.PresetCommand );
            _ = SessionBase.AsyncDelay( this.PresetRefractoryPeriod );
        }
    }

    private TimeSpan _presetRefractoryPeriod;

    /// <summary> Gets or sets the post-preset refractory period. </summary>
    /// <value> The post-preset refractory period. </value>
    public TimeSpan PresetRefractoryPeriod
    {
        get => this._presetRefractoryPeriod;
        set => _ = this.SetProperty( ref this._presetRefractoryPeriod, value );
    }

    #endregion

    #region " session "

    /// <summary> Gets the session. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <presentValue> The session. </presentValue>
    public Pith.SessionBase Session { get; private set; }

    /// <summary> Handles the Session property changed event. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Property Changed event information. </param>
    private void SessionPropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
        if ( sender is null || e is null )
            return;
        string activity = $"handling {nameof( Pith.SessionBase )}.{e.PropertyName} change";
        try
        {
            if ( sender is Pith.SessionBase s ) this.HandlePropertyChanged( s, e?.PropertyName );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    /// <summary>
    /// Gets a presentValue indicating whether a session to the device is open. See also
    /// <see cref="VI.Pith.SessionBase.IsDeviceOpen"/>.
    /// </summary>
    /// <presentValue> <c>true</c> if the device has an open session; otherwise, <c>false</c>. </presentValue>
    public bool IsDeviceOpen => this.Session is not null && this.Session.IsDeviceOpen;

    /// <summary> Gets the resource model. </summary>
    /// <presentValue> The resource model. </presentValue>
    public string? ResourceModelCaption => this.Session?.ResourceModelCaption;

    /// <summary> Gets the resource name caption. </summary>
    /// <presentValue> The resource name caption. </presentValue>
    public string? ResourceNameCaption => this.Session?.ResourceNameCaption;

    /// <summary>   Handles the device error occurred. </summary>
    /// <remarks>   2024-09-02. </remarks>
    /// <param name="sender">   Source of the event. </param>
    /// <param name="e">        Service request event information. </param>
    protected abstract void HandleDeviceErrorOccurred( object? sender, ServiceRequestEventArgs e );

    /// <summary> Handles the session property changed action. </summary>
    /// <param name="sender">       Source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    protected virtual void HandlePropertyChanged( Pith.SessionBase sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) ) return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( Pith.SessionBase.ErrorAvailable ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.ErrorAvailableBitmask ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.HasMeasurementEvent ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.MeasurementEventBitmask ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.MessageAvailableBitmask ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.MessageAvailable ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.HasOperationEvent ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.OperationEventSummaryBitmask ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.HasQuestionableEvent ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.QuestionableEventBitmask ):
                {
                    this.NotifyPropertyChanged( nameof( Pith.SessionBase.QuestionableEventBitmask ) );
                    break;
                }

            case nameof( Pith.SessionBase.HasStandardEvent ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.StandardEventSummaryBitmask ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.HasSystemEvent ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.SystemEventBitmask ):
                {
                    this.NotifyPropertyChanged( nameof( Pith.SessionBase.SystemEventBitmask ) );
                    break;
                }

            case nameof( Pith.SessionBase.RequestedService ):
                {
                    this.NotifyPropertyChanged( nameof( Pith.SessionBase.RequestedService ) );
                    break;
                }

            case nameof( Pith.SessionBase.RequestingServiceBitmask ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.LastMessageReceived ):
                {
                    string? value = sender.LastMessageReceived;
                    if ( !string.IsNullOrWhiteSpace( value ) )
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} received: '{value!.InsertCommonEscapeSequences()}'" );

                    break;
                }

            case nameof( Pith.SessionBase.LastMessageSent ):
                {
                    string? value = sender.LastMessageSent;
                    if ( !string.IsNullOrWhiteSpace( value ) )
                        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} sent: '{value}'" );

                    break;
                }

            case nameof( Pith.SessionBase.OperationCompleted ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.ServiceRequestStatus ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.ServiceRequestEnableBitmask ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.ServiceRequestEnableOperationCompleteBitmask ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.StandardEventEnableOperationCompleteBitmask ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.StandardEventEnableBitmask ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            default:
                break;
        }
    }

    #endregion

    #region " identity "

    private string _identity;

    /// <summary> Gets or sets the device identity string (*IDN?). </summary>
    /// <value> The identity. </value>
    public string Identity
    {
        get => this._identity;
        set
        {
            if ( string.IsNullOrWhiteSpace( value ) )
                value = string.Empty;
            if ( !string.Equals( value, this.Identity, StringComparison.Ordinal ) )
            {
                this._identity = value;
                this.NotifyPropertyChanged();
                if ( !string.IsNullOrWhiteSpace( value ) )
                    _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{this.ResourceNameCaption} identified;. as {value.Replace( Environment.NewLine, "" )}" );
            }
        }
    }

    /// <summary> Gets or sets the identity query command. </summary>
    /// <remarks>
    /// SCPI: "*IDN?".
    /// <see cref="VI.Syntax.Ieee488Syntax.IdentificationQueryCommand"> </see>
    /// </remarks>
    /// <value> The identity query command. </value>
    protected virtual string IdentificationQueryCommand { get; set; } = Syntax.Ieee488Syntax.IdentificationQueryCommand;

    /// <summary> Writes the identity query command. </summary>
    /// <remarks> This is required for systems which require asynchronous communication. </remarks>
    public void WriteIdentificationQueryCommand()
    {
        _ = this.Session.WriteLine( this.IdentificationQueryCommand );
    }

    /// <summary> Queries the Identity. </summary>
    /// <remarks> Sends the '*IDN?' query. </remarks>
    /// <returns>   A <see cref="string" />. </returns>
    public abstract string QueryIdentity();

    /// <summary>   Reads an identity. </summary>
    /// <remarks>   David, 2021-04-08. required in case the identify includes multiple lines. </remarks>
    /// <returns>   The identity. </returns>
    public virtual string ReadIdentity()
    {
        return string.Empty;
    }

    /// <summary>   Reads an identity. </summary>
    /// <remarks>   David, 2021-04-08. required in case the identify includes multiple lines. </remarks>
    /// <param name="firstLine">    The first line. </param>
    /// <returns>   The identity. </returns>
    public virtual string ReadIdentity( string firstLine )
    {
        return firstLine;
    }

    /// <summary> The version information base. </summary>
    private VersionInfoBase _versionInfoBase;

    /// <summary> Gets or sets information describing the version. </summary>
    /// <value> Information describing the version. </value>
    public VersionInfoBase VersionInfoBase
    {
        get => this._versionInfoBase;
        set
        {
            this._versionInfoBase = value;
            if ( value is not null )
            {
                this.SerialNumberReading = value.SerialNumber;
            }
        }
    }


    #region " serial number "

    /// <summary> The serial number reading. </summary>

    /// <summary> Gets or sets the serial number reading. </summary>
    /// <value> The serial number reading. </value>
    public string? SerialNumberReading
    {
        get;
        set
        {
            if ( string.IsNullOrWhiteSpace( value ) )
                value = string.Empty;
            if ( this.SetProperty( ref field, value ?? string.Empty ) )
            {
                this.SerialNumber = string.IsNullOrWhiteSpace( value )
                    ? new long?()
                    : long.TryParse( this.SerialNumberReading, System.Globalization.NumberStyles.Number,
                            System.Globalization.CultureInfo.InvariantCulture, out long numericValue )
                        ? numericValue
                        : new long?();
            }
        }
    }

    /// <summary> Gets or sets the serial number query command. </summary>
    /// <value> The serial number query command. </value>
    protected virtual string SerialNumberQueryCommand { get; set; } = string.Empty;

    /// <summary> Reads and returns the instrument serial number. </summary>
    /// <exception cref="VI.Pith.NativeException"> Thrown when a Visa error condition occurs. </exception>
    public string? QuerySerialNumber()
    {
        if ( !string.IsNullOrWhiteSpace( this.SerialNumberQueryCommand ) )
        {
            this.Session.LastNodeNumber = default;
            this.Session.SetLastAction( "reading serial number" );
            string? value = this.Session.QueryTrimEnd( this.SerialNumberQueryCommand );
            this.Session.ThrowDeviceExceptionIfError();
            this.SerialNumberReading = value;
        }
        else if ( string.IsNullOrWhiteSpace( this.SerialNumberReading ) && this.SerialNumber.HasValue )
        {
            this.SerialNumberReading = this.SerialNumber.Value.ToString();
        }

        return this.SerialNumberReading;
    }

    /// <summary> Reads and returns the instrument serial number. </summary>
    /// <value> The serial number. </value>
    public long? SerialNumber
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    #endregion

    #endregion

    #region " language "

    /// <summary> The Language. </summary>

    /// <summary> Gets or sets the device Language string (*IDN?). </summary>
    /// <value> The Language. </value>
    public string? Language
    {
        get;
        set
        {
            if ( string.IsNullOrWhiteSpace( value ) ) value = string.Empty;
            if ( !string.Equals( value, this.Language, StringComparison.Ordinal ) )
            {
                value = value!.Trim();
                field = value;
                this.NotifyPropertyChanged();
            }

            this.LanguageValidated = this.IsExpectedLanguage();
        }
    }

    /// <summary> The expected language. </summary>
    private string? _expectedLanguage;

    /// <summary> Gets or sets the device ExpectedLanguage string (*IDN?). </summary>
    /// <value> The ExpectedLanguage. </value>
    public string? ExpectedLanguage
    {
        get => this._expectedLanguage;
        set
        {
            if ( string.IsNullOrWhiteSpace( value ) ) value = string.Empty;
            if ( !string.Equals( value, this.ExpectedLanguage, StringComparison.Ordinal ) )
            {
                value = value!.Trim();
                this._expectedLanguage = value;
                this.NotifyPropertyChanged();
            }

            this.LanguageValidated = this.IsExpectedLanguage();
        }
    }

    /// <summary>
    /// QueryEnum if the language is the expected language or if the expected language is empty and thus
    /// invariant.
    /// </summary>
    /// <returns> <c>true</c> if expected language; otherwise <c>false</c> </returns>
    public bool IsExpectedLanguage()
    {
        return !string.IsNullOrWhiteSpace( this.Language ) && string.Equals( this.ExpectedLanguage, this.Language, StringComparison.Ordinal );
    }

    /// <summary> Gets or sets the sentinel indicating if the language was validated. </summary>
    /// <value> The LanguageValidated. </value>
    public bool LanguageValidated
    {
        get;
        set
        {
            if ( !Equals( value, this.LanguageValidated ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Writes and reads back the language. </summary>
    /// <param name="value"> The required language. </param>
    /// <returns> The language. </returns>
    public string? ApplyLanguage( string? value )
    {
        _ = this.WriteLanguage( value );
        return this.QueryLanguage();
    }

    /// <summary> Attempts to apply expected language from the given data. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public (bool Success, string Details) TryApplyExpectedLanguage()
    {
        (bool Success, string Details) result = (true, string.Empty);
        // check the language status.
        _ = this.QueryLanguage();
        if ( this.LanguageValidated )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Device language {this.Language} validated;. " );
        }
        else
        {
            // set the device to the correct language.
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Setting device to {this.ExpectedLanguage} language;. " );
            _ = this.ApplyLanguage( this.ExpectedLanguage );
            if ( !this.LanguageValidated )
            {
                result = (false, cc.isr.VI.SessionLogger.Instance.LogWarning( $"Incorrect {nameof( this.Language )} settings {this.Language}; must be {this.ExpectedLanguage}" ));
            }
        }

        return result;
    }

    /// <summary> Gets or sets the Language query command. </summary>
    /// <remarks> SCPI: "*LANG?". </remarks>
    /// <exception cref="cc.isr.VI.Pith.DeviceException">       Thrown when a Device error condition occurs. </exception>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The Language query command. </value>
    protected virtual string LanguageQueryCommand { get; set; } = string.Empty;

    /// <summary> Queries the Language. </summary>
    /// <remarks> Sends the '*LANG?' query. </remarks>
    /// <returns> The Language or Empty if not required or unknown. </returns>
    public virtual string? QueryLanguage()
    {
        this.Language = string.IsNullOrWhiteSpace( this.LanguageQueryCommand )
            ? string.Empty
            : this.Session.QueryTrimEnd( this.LanguageQueryCommand );
        return this.Language;
    }

    /// <summary> Gets or sets the Language command format. </summary>
    /// <remarks> *LANG {0}". </remarks>
    /// <exception cref="cc.isr.VI.Pith.DeviceException">       Thrown when a Device error condition occurs. </exception>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <value> The Language command format. </value>
    protected virtual string LanguageCommandFormat { get; set; } = string.Empty;

    /// <summary> Writes the Language value without reading back the value from the device. </summary>
    /// <remarks> Changing the language causes the instrument to reboot. </remarks>
    /// <param name="value"> The current Language. </param>
    /// <returns> The Language or Empty if not required or unknown. </returns>
    public string? WriteLanguage( string? value )
    {
        this.Language = string.IsNullOrWhiteSpace( this.LanguageCommandFormat )
            ? value
            : value is null
                ? value
                : this.Session.WriteLine( this.LanguageCommandFormat, value! );
        return this.Language;
    }

    #endregion

    #region " line frequency "

    /// <summary> Gets or sets the station line frequency. </summary>
    /// <remarks> This value is shared and used for all systems on this station. </remarks>
    /// <value> The station line frequency. </value>
    public static double? StationLineFrequency { get; set; } = new double?();

    /// <summary> Gets or sets the line frequency. </summary>
    /// <value> The line frequency. </value>
    public double? LineFrequency
    {
        get;
        set
        {
            if ( this.SetProperty( ref field, value ) )
                StatusSubsystemBase.StationLineFrequency = value;
        }
    }

    /// <summary> Gets or sets the default line frequency. </summary>
    /// <value> The default line frequency. </value>
    public static double DefaultLineFrequency { get; set; } = 60d;

    /// <summary> Gets or sets line frequency query command. </summary>
    /// <value> The line frequency query command. </value>
    protected virtual string LineFrequencyQueryCommand { get; set; } = VI.Syntax.ScpiSyntax.ReadLineFrequencyCommand;

    /// <summary> Reads the line frequency. </summary>
    /// <remarks> Sends the <see cref="LineFrequencyQueryCommand"/> query. </remarks>
    /// <returns> System.Nullable{System.Double}. </returns>
    public double? QueryLineFrequency()
    {
        if ( !this.LineFrequency.HasValue )
        {
            this.LineFrequency = string.IsNullOrWhiteSpace( this.LineFrequencyQueryCommand )
                ? DefaultLineFrequency
                : this.Session.Query( this.LineFrequency.GetValueOrDefault( DefaultLineFrequency ), this.LineFrequencyQueryCommand );
        }

        return this.LineFrequency;
    }

    /// <summary> Converts power line cycles to time span. </summary>
    /// <param name="powerLineCycles"> The power line cycles. </param>
    /// <returns>
    /// The integration period corresponding to the specified number of power line .
    /// </returns>
    public static TimeSpan FromPowerLineCycles( double powerLineCycles )
    {
        return FromPowerLineCycles( powerLineCycles, StationLineFrequency.GetValueOrDefault( DefaultLineFrequency ) );
    }

    /// <summary> Converts integration period to Power line cycles. </summary>
    /// <param name="integrationPeriod"> The integration period. </param>
    /// <returns> The number of power line cycles corresponding to the integration period. </returns>
    public static double ToPowerLineCycles( TimeSpan integrationPeriod )
    {
        return ToPowerLineCycles( integrationPeriod, StationLineFrequency.GetValueOrDefault( DefaultLineFrequency ) );
    }

    /// <summary> Converts power line cycles to time span. </summary>
    /// <param name="powerLineCycles"> The power line cycles. </param>
    /// <param name="frequency">       The frequency. </param>
    /// <returns>
    /// The integration period corresponding to the specified number of power line .
    /// </returns>
    public static TimeSpan FromPowerLineCycles( double powerLineCycles, double frequency )
    {
        return FromSecondsPrecise( powerLineCycles / frequency );
    }

    /// <summary> Converts integration period to Power line cycles. </summary>
    /// <param name="integrationPeriod"> The integration period. </param>
    /// <param name="frequency">         The frequency. </param>
    /// <returns> The number of power line cycles corresponding to the integration period. </returns>
    public static double ToPowerLineCycles( TimeSpan integrationPeriod, double frequency )
    {
        return TotalSecondsPrecise( integrationPeriod ) * frequency;
    }

    /// <summary> Total seconds precise. </summary>
    /// <param name="timespan"> The timespan. </param>
    /// <returns> The total number of seconds precise. </returns>
    public static double TotalSecondsPrecise( TimeSpan timespan )
    {
        return timespan.Ticks / ( double ) TimeSpan.TicksPerSecond;
    }

    /// <summary> Converts seconds to time span with tick timespan accuracy. </summary>
    /// <param name="seconds"> The seconds. </param>
    /// <returns> A TimeSpan. </returns>
    public static TimeSpan FromSecondsPrecise( double seconds )
    {
        return TimeSpan.FromTicks( ( long ) (TimeSpan.TicksPerSecond * seconds) );
    }

    #endregion
}
