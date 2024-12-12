using cc.isr.VI.Pith;

namespace cc.isr.VI;

/// <summary> Defines the contract that must be implemented by a Prober Subsystem. </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="OutputSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
[CLSCompliant( false )]
public abstract class ProberSubsystemBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.ErrorRead = false;
        this.LastReading = string.Empty;
        this.IdentityRead = false;
        this.MessageCompleted = false;
        this.MessageFailed = false;
        this.PatternCompleteReceived = new bool?();
        this.SetModeSent = false;
        this.IsFirstTestStart = new bool?();
        this.RetestRequested = new bool?();
        this.TestAgainRequested = new bool?();
        this.TestCompleteSent = new bool?();
        this.TestStartReceived = new bool?();
        this.WaferStartReceived = new bool?();
    }

    #endregion

    #region " error read "

    /// <summary> True to error read. </summary>
    private bool _errorRead;

    /// <summary> Gets or sets the Error Read sentinel. </summary>
    /// <value> The Error Read. </value>
    public bool ErrorRead
    {
        get => this._errorRead;
        set
        {
            this._errorRead = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " identity read "

    /// <summary> True to identity read. </summary>
    private bool _identityRead;

    /// <summary> Gets or sets the Identity Read sentinel. </summary>
    /// <value> The Identity Read. </value>
    public bool IdentityRead
    {
        get => this._identityRead;
        set
        {
            this._identityRead = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " message failed "

    /// <summary> True if message failed. </summary>
    private bool _messageFailed;

    /// <summary> Gets or sets the message failed. </summary>
    /// <value> The message failed. </value>
    public bool MessageFailed
    {
        get => this._messageFailed;
        set
        {
            this._messageFailed = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " message completed "

    /// <summary> True if message completed. </summary>
    private bool _messageCompleted;

    /// <summary> Gets or sets the message Completed. </summary>
    /// <value> The message Completed. </value>
    public bool MessageCompleted
    {
        get => this._messageCompleted;
        set
        {
            this._messageCompleted = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " pattern complete "

    /// <summary> A sentinel indicating that the Pattern Complete message was received. </summary>
    private bool? _patternCompleteReceived;

    /// <summary> Gets or sets the cached Pattern Complete message sentinel. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if Pattern Complete was received; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? PatternCompleteReceived
    {
        get => this._patternCompleteReceived;

        protected set
        {
            this._patternCompleteReceived = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
            if ( value.GetValueOrDefault( false ) )
            {
                // if pattern completed, turn off all other flags.
                this.UnhandledMessageReceived = false;
                this.TestCompleteSent = false;
                this.TestStartReceived = false;
                this.WaferStartReceived = false;
            }
        }
    }

    #endregion

    #region " set mode sent "

    /// <summary> A sentinel indicating that the Set Mode message was Sent. </summary>
    private bool _setModeSent;

    /// <summary> Gets or sets the cached Set Mode message sentinel. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if Set Mode was Sent; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool SetModeSent
    {
        get => this._setModeSent;

        protected set
        {
            this._setModeSent = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " test complete sent "

    /// <summary> A sentinel indicating that the Test Complete message was Sent. </summary>
    private bool? _testCompleteSent;

    /// <summary> Gets or sets the cached Test Complete message sentinel. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if Test Complete was Sent; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? TestCompleteSent
    {
        get => this._testCompleteSent;
        set
        {
            this._testCompleteSent = value;
            if ( value.GetValueOrDefault( false ) )
            {
                // if pattern completed, turn off all other flags.
                this.UnhandledMessageReceived = false;
                this.TestStartReceived = false;
            }

            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " test start "

    /// <summary> A sentinel indicating that the test start message was received. </summary>
    private bool? _testStartReceived;

    /// <summary> Gets or sets the cached test start message sentinel. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if test start was received; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? TestStartReceived
    {
        get => this._testStartReceived;

        protected set
        {
            this._testStartReceived = value;
            if ( value.GetValueOrDefault( false ) )
            {
                // turn off relevant sentinels.
                this.UnhandledMessageReceived = false;
                this.PatternCompleteReceived = false;
                this.TestCompleteSent = false;
            }

            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    /// <summary> A sentinel indicating that the retest is requested. </summary>
    private bool? _retestRequested;

    /// <summary> Gets or sets the cached retest requested sentinel. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if retest is request; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? RetestRequested
    {
        get => this._retestRequested;

        protected set
        {
            this._retestRequested = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    /// <summary> A sentinel indicating that the Test Again is requested. </summary>
    private bool? _testAgainRequested;

    /// <summary> Gets or sets the cached Test Again requested sentinel. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if TestAgain is request; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? TestAgainRequested
    {
        get => this._testAgainRequested;

        protected set
        {
            this._testAgainRequested = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " first test start "

    /// <summary> A sentinel indicating that the test start message was received. </summary>
    private bool? _isFirstTestStart;

    /// <summary> Gets or sets the cached first test start message sentinel. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if test start was received; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? IsFirstTestStart
    {
        get => this._isFirstTestStart;

        protected set
        {
            this._isFirstTestStart = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " wafer start "

    /// <summary> A sentinel indicating that the Wafer start message was received. </summary>
    private bool? _waferStartReceived;

    /// <summary> Gets or sets the cached Wafer start message sentinel. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if Wafer start was received; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? WaferStartReceived
    {
        get => this._waferStartReceived;

        protected set
        {
            this._waferStartReceived = value;
            if ( value.GetValueOrDefault( false ) )
            {
                // turn off relevant sentinels.
                this.UnhandledMessageReceived = false;
                this.PatternCompleteReceived = false;
                this.TestCompleteSent = false;
                this.TestStartReceived = false;
            }

            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " unhandled message received "

    /// <summary> A sentinel indicating that the Unhandled Message was received. </summary>
    private bool? _unhandledMessageReceived;

    /// <summary> Gets or sets the cached Unhandled Message sentinel. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if Unhandled Message was received; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? UnhandledMessageReceived
    {
        get => this._unhandledMessageReceived;

        protected set
        {
            this._unhandledMessageReceived = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " unhandled message sent "

    /// <summary> A sentinel indicating that the Unhandled Message was Sent. </summary>
    private bool? _unhandledMessageSent;

    /// <summary> Gets or sets the cached Unhandled Message sentinel. </summary>
    /// <value>
    /// <c>null</c> if state is not known; <c>true</c> if Unhandled Message was Sent; otherwise,
    /// <c>false</c>.
    /// </value>
    public bool? UnhandledMessageSent
    {
        get => this._unhandledMessageSent;

        protected set
        {
            this._unhandledMessageSent = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    #endregion

    #region " fetch "

    /// <summary> The last reading. </summary>
    private string? _lastReading;

    /// <summary> Gets or sets the last reading. </summary>
    /// <value> The last reading. </value>
    public string? LastReading
    {
        get => this._lastReading;
        set
        {
            if ( string.IsNullOrWhiteSpace( this.LastReading ) )
            {
                if ( string.IsNullOrWhiteSpace( value ) )
                {
                    return;
                }
                else
                {
                    this._lastReading = string.Empty;
                }
            }

            if ( string.IsNullOrWhiteSpace( value ) )
                value = string.Empty;
            this._lastReading = value;
            this.NotifyPropertyChanged();
            Pith.SessionBase.DoEventsAction?.Invoke();
        }
    }

    /// <summary> Parses the message. </summary>
    /// <param name="reading"> The reading. </param>
    public abstract void ParseReading( string reading );

    /// <summary>
    /// Fetches and parses a message from the instrument. The message must already be present.
    /// </summary>
    /// <param name="readStatusDelay"> The read status delay. </param>
    /// <param name="readDelay">       The read delay. </param>
    public void FetchAndParse( TimeSpan readStatusDelay, TimeSpan readDelay )
    {
        if ( this.Session is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( this.Session )} is null." );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"waiting read status delay {readStatusDelay:ss\\.fff};. " );
        _ = SessionBase.AsyncDelay( readStatusDelay );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"reading status;. " );

        this.Session.ThrowDeviceExceptionIfError( this.Session.ReadStatusByte() );

        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"waiting read delay {readDelay:ss\\.fff};. " );
        _ = SessionBase.AsyncDelay( readDelay );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Fetching;. " );
        this.Session.SetLastAction( "fetching a reading" );
        this.LastReading = this.Session.ReadLineTrimEnd();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Parsing;. {0}", this.LastReading );
        this.ParseReading( this.LastReading );
    }

    /// <summary>
    /// Fetches and parses a message from the instrument. The message must already be present.
    /// </summary>
    public void FetchAndParse()
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Fetching;. " );
        this.Session.SetLastAction( "fetching a reading" );
        this.LastReading = this.Session.ReadLineTrimEnd();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Parsing;. {0}", this.LastReading );
        this.ParseReading( this.LastReading );
    }

    /// <summary>
    /// Fetches and parses a message from the instrument. The message must already be present.
    /// </summary>
    /// <param name="readDelay"> The read delay. </param>
    public void FetchAndParse( TimeSpan readDelay )
    {
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"waiting read delay {readDelay:ss\\.fff};. " );
        _ = SessionBase.AsyncDelay( readDelay );
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( "Fetching;. " );
        this.LastReading = this.Session.ReadLineTrimEnd();
        _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Parsing;. {this.LastReading};. " );
        this.ParseReading( this.LastReading );
    }

    #endregion
}
