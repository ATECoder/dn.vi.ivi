using System.ComponentModel;
using System.Diagnostics;
using cc.isr.VI.Subsystem;

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by Subsystems. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2005-01-15, 1.0.1841.x. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract partial class SubsystemBase : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IPresettable
{
    #region " construction and cleanup "

    /// <summary>   Initializes a new instance of the <see cref="SubsystemBase" /> class. </summary>
    /// <remarks>   2024-09-07. </remarks>
    /// <param name="statusSubsystem">  The status subsystem. </param>
    protected SubsystemBase( VI.StatusSubsystemBase statusSubsystem ) : base()
    {
        if ( statusSubsystem is null ) throw new ArgumentNullException( nameof( statusSubsystem ) );
        if ( statusSubsystem!.Session is null ) throw new ArgumentNullException( nameof( statusSubsystem.Session ) );
        this.StatusSubsystem = statusSubsystem;
        this.Session = statusSubsystem!.Session!;
        this.Session.PropertyChanged += this.SessionPropertyChanged;
        // this.Apply_SessionThis( session );
        this.ElapsedTimeStopwatch = new Stopwatch();
    }

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the clear execution state (CLS) by setting system properties to the their Clear
    /// Execution (CLS) default values.
    /// </summary>
    /// <remarks> Clears the queues and sets all registers to zero. </remarks>
    public virtual void DefineClearExecutionState()
    {
    }

    /// <summary> Sets the known initial post reset state. </summary>
    /// <remarks> Customizes the reset state. </remarks>
    public virtual void InitKnownState()
    {
    }

    /// <summary> Sets the subsystem known preset state. </summary>
    public virtual void PresetKnownState()
    {
    }

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    /// <remarks> Clears the queues and sets all registers to zero. </remarks>
    public virtual void DefineKnownResetState()
    {
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

    #region " status "

    /// <summary> Gets or sets the status subsystem. </summary>
    /// <value> The status subsystem. </value>
    public StatusSubsystemBase StatusSubsystem { get; private set; }

    #endregion

    #region " session "

    /// <summary> Gets the session. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <presentValue> The session. </presentValue>
    public Pith.SessionBase Session { get; private set; }

    /// <summary> Handles the session property changed action. </summary>
    /// <param name="sender">       Source of the event. </param>
    /// <param name="propertyName"> Name of the property. </param>
    protected virtual void HandlePropertyChanged( Pith.SessionBase sender, string? propertyName )
    {
        if ( sender is null || string.IsNullOrWhiteSpace( propertyName ) )
            return;
        switch ( propertyName ?? string.Empty )
        {
            case nameof( Pith.SessionBase.ResourceModelCaption ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            case nameof( Pith.SessionBase.ResourceNameCaption ):
                {
                    this.NotifyPropertyChanged( propertyName! );
                    break;
                }

            default:
                break;
        }
    }

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

    #endregion

    #region " parse subsystem message "

    /// <summary>   Parses a subsystem message assigning presentValue to a subsystem property. </summary>
    /// <remarks>   David, 2021-04-08. </remarks>
    /// <param name="messageReceived">  The message received. </param>
    /// <returns>   True if it message was parsed by the subsystem; otherwise, false. </returns>
    public virtual bool ParseSubsystemMessage( string messageReceived )
    {
        return false;
    }

    #endregion

    #region " service request handling option flags "

    /// <summary>   (Immutable) the service request handling options token. </summary>
    private readonly Lazy<Std.Concurrent.ConcurrentToken<ServiceRequestHandlingOption>> _serviceRequestHandlingOptionsToken = new( () => new Std.Concurrent.ConcurrentToken<ServiceRequestHandlingOption>() );

    /// <summary>   Gets or sets the Service Request Fetch Reading enabled service request option. </summary>
    /// <presentValue> The Service Request Fetch Reading service request option enabled. </presentValue>
    public bool ServiceRequestFetchReadingEnabled
    {
        get => ServiceRequestHandlingOption.AutoFetchReading == this._serviceRequestHandlingOptionsToken.Value.Value;
        set
        {
            if ( this.ServiceRequestFetchReadingEnabled != value )
            {
                this._serviceRequestHandlingOptionsToken.Value.Value = value ? ServiceRequestHandlingOption.AutoFetchReading : ServiceRequestHandlingOption.None;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a presentValue indicating whether the Service Request Stream Readings option is enabled.
    /// </summary>
    /// <presentValue> True if Service Request Stream Readings is enabled, false if not. </presentValue>
    public bool ServiceRequestStreamReadingsEnabled
    {
        get => ServiceRequestHandlingOption.StreamReadings == this._serviceRequestHandlingOptionsToken.Value.Value;
        set
        {
            if ( this.ServiceRequestStreamReadingsEnabled != value )
            {
                this._serviceRequestHandlingOptionsToken.Value.Value = value ? ServiceRequestHandlingOption.StreamReadings : ServiceRequestHandlingOption.None;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>   Gets or sets a presentValue indicating whether the Service Request subsystem parse message is enabled. </summary>
    /// <presentValue> True if Service Request subsystem parse message enabled, false if not. </presentValue>
    public bool ServiceRequestSubsystemParseMessageEnabled
    {
        get => ServiceRequestHandlingOption.ParseMessage == this._serviceRequestHandlingOptionsToken.Value.Value;
        set
        {
            if ( this.ServiceRequestSubsystemParseMessageEnabled != value )
            {
                this._serviceRequestHandlingOptionsToken.Value.Value = value ? ServiceRequestHandlingOption.ParseMessage : ServiceRequestHandlingOption.None;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion
}

/// <summary>   Values that represent service request handling options. </summary>
/// <remarks>   David, 2021-04-08. </remarks>
public enum ServiceRequestHandlingOption
{
    /// <summary>   An enum constant representing the none option. </summary>
    [System.ComponentModel.Description( "None" )]
    None,

    /// <summary>   An enum constant representing the parse message option. </summary>
    [System.ComponentModel.Description( "Parse Message" )]
    ParseMessage,

    /// <summary>   An enum constant representing the Automatic fetch reading option. </summary>
    [System.ComponentModel.Description( "AutoFetch Reading" )]
    AutoFetchReading,

    /// <summary>   An enum constant representing the stream readings option. </summary>
    [System.ComponentModel.Description( "Stream Readings" )]
    StreamReadings
}


