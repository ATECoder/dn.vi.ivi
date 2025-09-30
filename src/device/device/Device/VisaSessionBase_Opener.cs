using System.ComponentModel;
using cc.isr.Std;
using cc.isr.Std.TaskExtensions;
using cc.isr.VI.ExceptionExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI;

public partial class VisaSessionBase : cc.isr.Std.Notifiers.OpenResourceBase
{
    #region " selector "

    /// <summary> Gets or sets the resources search pattern. </summary>
    /// <value> The resources search pattern. </value>
    public string ResourcesFilter
    {
        get => this._sessionFactory?.ResourcesFilter ?? string.Empty;
        set
        {
            if ( this._sessionFactory is not null && !string.Equals( value, this.ResourcesFilter ) )
            {
                this._sessionFactory.ResourcesFilter = value;
                this.NotifyPropertyChanged();

                _ = (this.Session?.ResourcesFilter = value);
            }
        }
    }

    /// <summary> The session factory. </summary>
    private SessionFactory? _sessionFactory;

    /// <summary> Gets the session factory. </summary>
    /// <value> The session factory. </value>
    public SessionFactory? SessionFactory
    {
        get
        {
            // enable validation; Validation is disabled by default to facilitate
            // resource selection in case of resource manager mismatch between
            // VISA implementations.
            _ = (this._sessionFactory?.ValidationEnabled = this.ValidationEnabled);
            return this._sessionFactory;
        }
    }

    /// <summary> Attempts to validate resource from the given data. </summary>
    /// <remarks> David, 2020-07-20. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public (bool Success, string Details) TryValidateResource( string resourceName )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) ) resourceName = string.Empty;

        if ( this.Session is null ) throw new NativeException( "Session is not open." );
        if ( this.SessionFactory is null ) throw new NativeException( "Session factory is null." );

        (bool Success, string Details) result = (true, string.Empty);
        string activity = string.Empty;
        try
        {
            activity = $"{this.ResourceModelCaption} validating the resource";
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            this.Session.StatusPrompt = activity;
            result = this.SessionFactory.TryValidateResource( resourceName );
            if ( !string.IsNullOrWhiteSpace( result.Details ) )
                _ = cc.isr.VI.SessionLogger.Instance.LogInformation( $"{activity};. reported {result.Details}" );
            this.Session.StatusPrompt = result.Success ? $"done {activity}" : $"failed {activity}";
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            result = (false, $"Exception {activity};. {ex.BuildMessage()}");
        }
        finally
        {
            // this enables opening if the validated name has value
            this.ValidatedResourceName = this.SessionFactory.ValidatedResourceName;
        }

        return result;
    }

    /// <summary> Validates the resource name described by resourceName. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    public void ValidateResourceName( string resourceName )
    {
        _ = this.TryValidateResource( resourceName );
    }

    #endregion

    #region " resource name validation task "

    /// <summary> Gets or sets the resource name validation asynchronous task. </summary>
    /// <value> The resource name validation asynchronous task. </value>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0052:Remove unread private members", Justification = "<Pending>" )]
    private Task? ResourceNameValidationAsyncTask { get; set; }

    /// <summary> Gets or sets the resource name validation task. </summary>
    /// <value> The resource name validation task. </value>
    private Task? ResourceNameValidationTask { get; set; }

    /// <summary> QueryEnum if the task validating the resource name is active. </summary>
    /// <returns>
    /// <c>true</c> if the task validating the resource name is active; otherwise <c>false</c>
    /// </returns>
    public bool IsValidatingResourceName()
    {
        return this.ResourceNameValidationTask?.IsTaskActive() ?? false;
    }

    /// <summary> Await resource name validation. </summary>
    /// <param name="timeout"> The timeout. </param>
    public void AwaitResourceNameValidation( TimeSpan timeout )
    {
        _ = this.ResourceNameValidationTask?.Wait( timeout );
    }

    /// <summary> Asynchronously validate the resource name. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> A Threading.Tasks.Task. </returns>
    public Task AsyncValidateResourceName( string resourceName )
    {
        this.ResourceNameValidationAsyncTask = this.AsyncAwaitValidateResourceName( resourceName );
        return this.ResourceNameValidationTask!;
    }

    /// <summary> Asynchronous await validate resource name. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> A Task. </returns>
    private async Task AsyncAwaitValidateResourceName( string resourceName )
    {
        this.ResourceNameValidationTask = Task.Run( () => this.ValidateResourceName( resourceName ) );
        await this.ResourceNameValidationTask;
    }

    #endregion

    #region " resource name "

    /// <summary> Gets or sets the name of the candidate resource. </summary>
    /// <value> The name of the candidate resource. </value>
    public override string CandidateResourceName
    {
        get => base.CandidateResourceName;
        set
        {
            if ( this.Session is not null && this.SessionFactory is not null &&
                (!string.Equals( value, this.Session.CandidateResourceName, StringComparison.Ordinal )
                 || !string.Equals( value, base.CandidateResourceName, StringComparison.Ordinal )) )
            {
                this.Session.CandidateResourceName = value;
                this.SessionFactory.CandidateResourceName = value;
                base.CandidateResourceName = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the candidate resource name validated. </summary>
    /// <value> The candidate resource name validated. </value>
    public override bool CandidateResourceNameValidated
    {
        get => base.CandidateResourceNameValidated;
        set
        {
            _ = (this.Session?.CandidateResourceNameConnected = value);
            base.CandidateResourceNameValidated = value;
            this.NotifyPropertyChanged();
        }
    }

    /// <summary> Returns true if the validated candidate resource name was also connected. </summary>
    /// <value> The resource name connected. </value>
    public bool ResourceNameConnected => this.CandidateResourceNameValidated && string.Equals( this.CandidateResourceName, this.OpenResourceName, StringComparison.Ordinal );

    /// <summary> Gets or sets the name of the open resource. </summary>
    /// <value> The name of the open resource. </value>
    public override string OpenResourceName
    {
        get => base.OpenResourceName;
        set
        {
            if ( this.Session is not null &&
                (!string.Equals( value, this.Session.OpenResourceName, StringComparison.Ordinal )
                || !string.Equals( value, base.OpenResourceName, StringComparison.Ordinal )) )
            {
                this.Session.OpenResourceName = value;
                base.OpenResourceName = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the candidate resource model. </summary>
    /// <value> The candidate resource model. </value>
    public override string CandidateResourceModel
    {
        get => base.CandidateResourceModel;
        set
        {
            if ( this.Session is not null &&
                (!string.Equals( value, this.Session.CandidateResourceModel, StringComparison.Ordinal )
                  || !string.Equals( value, base.CandidateResourceModel, StringComparison.Ordinal )) )
            {
                this.Session.CandidateResourceModel = value;
                base.CandidateResourceModel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the resource model. </summary>
    /// <value> The resource model. </value>
    public override string OpenResourceModel
    {
        get => base.OpenResourceModel;
        set
        {
            if ( this.Session is not null &&
                (!string.Equals( value, this.Session.OpenResourceModel, StringComparison.Ordinal )
                || !string.Equals( value, base.OpenResourceModel, StringComparison.Ordinal )) )
            {
                this.Session.OpenResourceModel = value;
                base.OpenResourceModel = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the resource name caption. </summary>
    /// <value>
    /// The caption for the <see cref="VI.Pith.SessionBase.OpenResourceName"/> open or
    /// <see cref="VI.Pith.SessionBase.CandidateResourceName"/>resource names.
    /// </value>
    public override string ResourceNameCaption
    {
        get => base.ResourceNameCaption;
        set
        {
            if ( this.Session is not null &&
                (!string.Equals( value, this.Session.ResourceNameCaption, StringComparison.Ordinal )
                  || !string.Equals( value, base.ResourceNameCaption, StringComparison.Ordinal )) )
            {
                this.Session.ResourceNameCaption = value;
                base.ResourceNameCaption = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets or sets the resource Title caption. </summary>
    /// <value>
    /// The caption for the <see cref="VI.Pith.SessionBase.OpenResourceModel"/> open,
    /// <see cref="VI.Pith.SessionBase.CandidateResourceModel"/> or identity.
    /// </value>
    public override string ResourceModelCaption
    {
        get => base.ResourceModelCaption;
        set
        {
            if ( this.Session is not null &&
                (!string.Equals( value, this.Session.ResourceModelCaption, StringComparison.Ordinal )
                  || !string.Equals( value, base.ResourceModelCaption, StringComparison.Ordinal )) )
            {
                this.Session.ResourceModelCaption = value;
                base.ResourceModelCaption = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion

    #region " opener "

    /// <summary> Gets the Open status. </summary>
    /// <value> The Open status. </value>
    public override bool IsOpen => this.IsDeviceOpen;

    /// <summary> Gets the is session open. </summary>
    /// <value> The is session open. </value>
    public bool IsSessionOpen => this.Session is not null && this.Session.IsSessionOpen;

    /// <summary> Gets the is device open. </summary>
    /// <remarks>
    /// The session device open property can be toggled without having the actual session open.  This
    /// is useful when emulating session functionality.
    /// </remarks>
    /// <value> The is device open. </value>
    public bool IsDeviceOpen => this.Session is not null && this.Session.IsDeviceOpen;

    /// <summary> Initializes the session prior to opening access to the instrument. </summary>
    protected virtual void BeforeOpening()
    {
        // this.ApplySettings();
    }

    /// <summary> Notifies an open changed. </summary>
    public override void NotifyOpenChanged()
    {
        this.NotifyPropertyChanged( nameof( this.IsDeviceOpen ) );
        this.NotifyPropertyChanged( nameof( this.IsSessionOpen ) );
        base.NotifyOpenChanged();
    }

    /// <summary> Opens the session. </summary>
    /// <remarks> Register the device trace notifier before opening the session. </remarks>
    /// <exception cref="InvalidOperationException">   Thrown when operation failed to execute. </exception>
    /// <exception cref="OperationCanceledException"> Thrown when an Operation Canceled error condition
    /// occurs. </exception>
    /// <param name="resourceName">  Name of the resource. </param>
    /// <param name="resourceModel"> The resource model. </param>
    public virtual void OpenSession( string resourceName, string resourceModel )
    {
        bool success = false;
        try
        {
            if ( this.SubsystemSupportMode == SubsystemSupportMode.Full )
            {
                this.CandidateResourceName = resourceName;
                this.CandidateResourceModel = resourceModel;
            }

            string activity = $"Opening session to {resourceName}";
            if ( this.Enabled )
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{activity};. " );
            // initializes the session keep-alive commands.
            this.BeforeOpening();
            this.Session!.OpenSession( resourceName, resourceModel );
            if ( this.Session.IsSessionOpen )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"Session open to {resourceName};. " );
            }
            else if ( this.Session.Enabled )
            {
                throw new OperationFailedException( $"Unable to open session to {resourceName};. " );
            }
            else if ( !this.IsDeviceOpen )
            {
                throw new OperationFailedException( $"Unable to emulate {resourceName};. " );
            }

            if ( this.Session.IsSessionOpen || (this.IsDeviceOpen && !this.Session.Enabled) )
            {
                activity = $"{resourceModel}:{resourceName} handling opening actions";
                CancelEventArgs e = new();
                this.OnOpening( e );
                if ( e.Cancel )
                    throw new OperationCanceledException( $"{activity} canceled;. " );
                activity = $"{resourceModel}:{resourceName} handling opened actions";

                // this clears any existing errors using SDC, RST, and CLS.
                this.OnOpened( EventArgs.Empty );
                if ( this.IsDeviceOpen )
                {
                    activity = $"tagging {resourceModel}:{resourceName} as open";
                    base.OpenResource( resourceName, resourceModel );
                    // this causes a second device reset in order to initialize all the subsystems.
                    // which is a duplication.  Possibly at this point there is no need to do a full reset etc.
                    activity = $"{resourceModel}:{resourceName} handling initialization";
                    this.OnInitializing( e );
                    if ( e.Cancel )
                        throw new OperationCanceledException( $"{activity} canceled;. " );
                    this.OnInitialized( e );
                }
                else
                {
                    throw new OperationFailedException( $"{activity} failed;. " );
                }
            }
            else
            {
                throw new OperationFailedException( $"{activity} failed;. " );
            }

            success = true;
        }
        catch
        {
            throw;
        }
        finally
        {
            if ( !success )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{resourceName} failed connecting. Disconnecting." );
                _ = this.TryCloseSession();
            }
        }
    }

    /// <summary> Allows the derived device to take actions after closing. </summary>
    /// <remarks> This override should occur as the last call of the overriding method. </remarks>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected override void OnClosed( EventArgs e )
    {
        base.OnClosed( e );
    }

    /// <summary> Closes the session. </summary>
    /// <exception cref="InvalidOperationException"> Thrown when operation failed to execute. </exception>
    public virtual void CloseSession()
    {
        string activity = string.Empty;
        try
        {
            // check if already closed.
            if ( this.IsDisposed || !this.IsDeviceOpen )
                return;
            CancelEventArgs e = new();
            activity = $"{this.OpenResourceModel}:{this.OpenResourceName} handling closing actions";
            this.OnClosing( e );
            if ( e.Cancel )
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{activity} canceled;. " );
            else
            {
                activity = $"{this.CandidateResourceModel} removing service request handler";
                this.RemoveServiceRequestEventHandler();

                activity = $"{this.CandidateResourceModel} disabling service request handler";
                this.Session?.DisableServiceRequestEventHandler();

                activity = $"{this.CandidateResourceModel} closing session";
                this.Session?.CloseSession();

                activity = $"{this.CandidateResourceModel} handling closed actions";
                this.OnClosed( EventArgs.Empty );
            }
        }
        catch ( NativeException ex )
        {
            _ = ex.AddExceptionData();
            throw new OperationFailedException( $"Failed {activity} while closing the VISA session.", ex );
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            throw new OperationFailedException( $"Exception occurred {activity} while closing the session.", ex );
        }
    }

    #endregion

    #region " opener device or status only "

    /// <summary> Allows the derived device to take actions before opening. </summary>
    /// <remarks>
    /// This override should occur as the first call of the overriding method. After this call, the
    /// parent class adds the subsystems.
    /// </remarks>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected override void OnOpening( CancelEventArgs e )
    {
        if ( e is null ) throw new ArgumentNullException( nameof( e ) );
        if ( this.Subsystems is not null && this.SubsystemSupportMode == SubsystemSupportMode.StatusOnly )
        {
            if ( this.Subsystems.Count > 1 )
            {
                _ = cc.isr.VI.SessionLogger.Instance.LogWarning( $"{this.ResourceNameCaption} subsystem count {this.Subsystems.Count} on opening;. " );
                this.Subsystems.Clear();
                if ( this.StatusSubsystemBase is not null )
                    this.Subsystems.Add( this.StatusSubsystemBase );
            }
            else
            {
                this.IsInitialized = false;
            }
        }

        base.OnOpening( e );
    }

    /// <summary>   Reports session open and enabled. </summary>
    /// <remarks>   2024-08-29. </remarks>
    public void LogSessionOpenAndEnabled()
    {
        System.Text.StringBuilder sb = new();
        _ = sb.Append( $"{this.ResourceNameCaption} {nameof( this.Session )} is " );
        _ = sb.Append( this.Session is null ? "null" : this.Session.IsSessionOpen ? "open" : "close" );
        _ = sb.Append( this.Session is null ? "." : this.Session.Enabled ? " and enabled." : " and disabled." );

        Microsoft.Extensions.Logging.LogLevel outcome = this.Session is null
            ? Microsoft.Extensions.Logging.LogLevel.Warning
            : !this.Session.IsSessionOpen & this.Session.Enabled
                ? Microsoft.Extensions.Logging.LogLevel.Warning
                : Microsoft.Extensions.Logging.LogLevel.Information;

        _ = cc.isr.VI.SessionLogger.Instance.LogMessage( outcome, sb.ToString() );
    }

    /// <summary> Allows the derived device to take actions after opening. </summary>
    /// <remarks>
    /// This override should occur as the last call of the overriding method. The subsystems are
    /// added as part of the <see cref="OnOpening(CancelEventArgs)"/> method. The synchronization
    /// context is captured as part of the property change and other event handlers and is no longer
    /// needed here.
    /// </remarks>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected override void OnOpened( EventArgs e )
    {
        e ??= EventArgs.Empty;
        if ( this.SubsystemSupportMode == SubsystemSupportMode.Full )
        {
            // saving the resource names in full mode only
            this.OpenResourceModel = this.Session!.OpenResourceModel;
            this.OpenResourceName = this.Session.OpenResourceName;

            this.LogSessionOpenAndEnabled();

            ResourceNameInfoCollection.AddNewResource( this.OpenResourceName );
        }

        // A talker is assigned to the subsystem when the device is constructed.
        // This talker is assigned to each subsystem when it is added.
        // Me.Subsystems.AssignTalker(Me.Talker)

        // The synchronization context is captured as part of the property change and other event
        // handlers and is no longer needed here.

        // reset and clear the device to remove any existing errors.
        if ( this.SubsystemSupportMode != SubsystemSupportMode.Native )
        {
            if ( this.StatusSubsystemBase is null )
                throw new InvalidOperationException( $"{nameof( this.StatusSubsystemBase )} is null whereas the {nameof( this.SubsystemSupportMode )} is {this.SubsystemSupportMode}." );
            this.StatusSubsystemBase.OnDeviceOpen();
        }

        // 20181219: this is included in the base method: Me.NotifyPropertyChanged(NameOf(VisaSessionBase.IsDeviceOpen))
        // 2016/01/18: this was done before adding listeners, which was useful when using the device
        // as a class in a 'meter'. As a result, the actions taken when handling the Opened event,
        // such as Reset and Initialize do not get reported.
        // The solution was to add the device Initialize event to process publishing and initialization of device
        // and subsystems.
        base.OnOpened( e );
    }

    /// <summary> Opens a resource. </summary>
    /// <param name="resourceName">  The name of the resource. </param>
    /// <param name="resourceModel"> The resource model. </param>
    public override void OpenResource( string resourceName, string resourceModel )
    {
        this.OpenSession( resourceName, resourceModel );
        if ( this.IsOpen )
            // at this point, the resource might have been updated using the instrument identity information.
            base.OpenResource( this.OpenResourceName, this.OpenResourceModel );
        else
            base.OpenResource( resourceName, resourceModel );
    }

    /// <summary>   Attempts to open resource from the given data. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <param name="resourceName">     The name of the resource. </param>
    /// <param name="resourceModel">    The resource model. </param>
    /// <returns>   <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public override (bool success, string Details) TryOpen( string resourceName, string resourceModel )
    {
        return this.TryOpenSession( resourceName, resourceModel );
    }

    /// <summary> Try open session. </summary>
    /// <param name="resourceName">  Name of the resource. </param>
    /// <param name="resourceModel"> The resource model. </param>
    /// <returns>
    /// The (Success As Boolean, Details As String); Success: <c>true</c> if session opened;
    /// otherwise <c>false</c>
    /// </returns>
    public virtual (bool Success, string Details) TryOpenSession( string resourceName, string resourceModel )
    {
        string activity = $"opening VISA session to {resourceModel}:{resourceName}";
        bool success = true;
        string details = string.Empty;
        try
        {
            this.OpenSession( resourceName, resourceModel );
        }
        catch ( Exception ex )
        {
            success = false;
            _ = ex.AddExceptionData();
            details = $"Exception {activity};. {ex.BuildMessage()}";
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }

        return (success && this.IsDeviceOpen, details);
    }

    /// <summary> Closes the resource. </summary>
    public override void CloseResource()
    {
        this.CloseSession();
    }

    /// <summary>   Attempts to close resource from the given data. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <returns>   <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public override (bool success, string Details) TryClose()
    {
        return this.TryCloseSession();
    }

    /// <summary> Try close session. </summary>
    /// <returns>
    /// The (Success As Boolean, Details As String); Success: <c>true</c> if session closed;
    /// otherwise <c>false</c>
    /// </returns>
    public virtual (bool Success, string Details) TryCloseSession()
    {
        string activity = $"closing VISA session to {this.OpenResourceModel}:{this.OpenResourceName}";
        bool success = true;
        string details = string.Empty;
        try
        {
            this.CloseResource();
            if ( !this.IsDeviceOpen )
                details = $"failed {activity}";
        }
        catch ( OperationFailedException ex )
        {
            success = false;
            details = $"Exception {activity};. {ex.BuildMessage()}";
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }

        return (success && !this.IsDeviceOpen, details);
    }

    /// <summary> Gets or sets the subsystem support mode. </summary>
    /// <value> The subsystem support mode. </value>
    public SubsystemSupportMode SubsystemSupportMode
    {
        get;
        set
        {
            if ( value != this.SubsystemSupportMode )
            {
                field = value;
                this.NotifyOpenChanged();
            }
        }
    }

    #endregion

    #region " initialize "

    /// <summary> Executes the initializing actions. </summary>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected override void OnInitializing( CancelEventArgs e )
    {
        if ( this.StatusSubsystemBase is not null && e is not null )
        {
            base.OnInitializing( e );
            // if starting a Visa Session (w/o a status subsystem, skip initializing.
            if ( e is not null && !e.Cancel )
            {
                /*  Moved to the Define Known Reset State, Clear State and Init Known State
                    // 16-2-18: replaces calls to reset and then init known states
                    // 20241115: called from the Open Session methods. Me.ResetClearInit()
                    //           No longer the case: 12/2019: Reset clear init is already part of the On Opened function.
                    if ( this.SubsystemSupportMode == SubsystemSupportMode.StatusOnly )
                    {
                        this.StatusSubsystemBase.DefineKnownResetState();
                        this.StatusSubsystemBase.DefineClearExecutionState();
                        this.StatusSubsystemBase.InitKnownState();
                    }
                    else if ( this.SubsystemSupportMode == SubsystemSupportMode.Full )
                    {
                        this.DefineResetKnownState();
                        this.DefineClearExecutionState();
                        this.InitKnownState();
                    }
                 */

            }
        }
    }

    /// <summary> Executes the initialized action. </summary>
    /// <param name="e"> Event information to send to registered event handlers. </param>
    protected override void OnInitialized( EventArgs e )
    {
        bool hasIdentity = this.StatusSubsystemBase?.Identity is not null;
        this.Identity = hasIdentity ? this.StatusSubsystemBase!.Identity : string.Empty;
        if ( this.SubsystemSupportMode == SubsystemSupportMode.Full )
        {
            // resource name is cached only with full support
            // update the session open resource model, which updates the visa session base title as well.
            if ( !string.IsNullOrWhiteSpace( this.StatusSubsystemBase!.VersionInfoBase.Model ) )
            {
                this.Session!.OpenResourceModel = this.StatusSubsystemBase.VersionInfoBase.Model;
            }
        }

        base.OnInitialized( e );
    }

    #endregion
}
/// <summary> Values that represent session subsystem support mode. </summary>
public enum SubsystemSupportMode
{
    /// <summary> An enum constant representing the full option. </summary>
    [Description( "Full subsystems support" )]
    Full,

    /// <summary> An enum constant representing the status only option. </summary>
    [Description( "Status subsystem support" )]
    StatusOnly,

    /// <summary> An enum constant representing the native option. </summary>
    [Description( "No subsystem support" )]
    Native
}
