using System.ComponentModel;
using cc.isr.VI.ExceptionExtensions;

namespace cc.isr.VI;

/// <summary> A session factory. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-29 </para>
/// </remarks>
[CLSCompliant( false )]
public partial class SessionFactory : cc.isr.Std.Notifiers.SelectResourceBase
{
    #region " construction "

    /// <summary> Default constructor. </summary>
    public SessionFactory() : base()
    {
        this.Searchable = true;
        this.PingFilterEnabled = false;
        this.Factory = new Foundation.SessionFactory();
    }

    /// <summary> Gets or sets the factory. </summary>
    /// <value> The factory. </value>
    public Pith.SessionFactoryBase Factory { get; set; }

    /// <summary>   (Immutable) the service request handling options token. </summary>
    private static readonly Lazy<SessionFactory> _instance = new( () => new SessionFactory() );

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static SessionFactory Instance => _instance.Value;

    /// <summary>   (Immutable) the service request handling options token. </summary>
    /// <remakrs>   The resource finder is set when the factory is constructed. </remakrs>
    private static readonly Lazy<Pith.ResourceFinderBase> _resourceFinder = new( () => SessionFactory.Instance.Factory.ResourcesProvider().ResourceFinder! );

    /// <summary>   Gets the instance. </summary>
    /// <value> The instance. </value>
    public static Pith.ResourceFinderBase ResourceFinder => _resourceFinder.Value;


    #endregion

    #region " resoure manager "

    /// <summary> A filter specifying the resources. </summary>
    private string? _resourcesFilter;

    /// <summary> Gets or sets the resources search pattern. </summary>
    /// <value> The resources search pattern. </value>
    public string ResourcesFilter
    {
        get
        {
            if ( string.IsNullOrWhiteSpace( this._resourcesFilter ) )
                this._resourcesFilter = SessionFactory.ResourceFinder.BuildMinimalResourcesFilter();

            return this._resourcesFilter!;
        }

        set => _ = base.SetProperty( ref this._resourcesFilter, value ?? string.Empty );
    }

    /// <summary> True to enable, false to disable the ping filter. </summary>
    private bool _pingFilterEnabled;

    /// <summary>
    /// Gets or sets the Ping Filter enabled sentinel. When enabled, Tcp/IP resources are added only
    /// if they can be pinged.
    /// </summary>
    /// <value> The ping filter enabled. </value>
    public bool PingFilterEnabled
    {
        get => this._pingFilterEnabled;
        set
        {
            if ( value != this.PingFilterEnabled )
            {
                this._pingFilterEnabled = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Enumerate resource names. </summary>
    /// <param name="applyEnabledFilters"> True to apply filters if enabled. </param>
    /// <returns> A list of <see cref="string" />. </returns>
    public override BindingList<string> EnumerateResources( bool applyEnabledFilters )
    {
        IEnumerable<string> resources = [];
        using ( Pith.ResourcesProviderBase rm = this.Factory.ResourcesProvider() )
        {
            resources = string.IsNullOrWhiteSpace( this.ResourcesFilter ) ? rm.FindResources() : rm.FindResources( this.ResourcesFilter );
        }
        // todo: ignore ping option at this time since this is the only filter we use.
        // If applyEnabledFilters AndAlso Me.PingFilterEnabled Then resources = VI.Pith.ResourceNamesManager.PingFilter(resources)
        if ( applyEnabledFilters )
            resources = Pith.ResourceNamesManager.PingFilter( resources );
        return this.EnumerateResources( resources );
    }

    /// <summary> Enumerates the default resource name patterns in this collection. </summary>
    /// <returns>
    /// An enumerator that allows for each to be used to process the default resource name patters in
    /// this collection.
    /// </returns>
    public static BindingList<string> EnumerateDefaultResourceNamePatterns()
    {
        return ["GPIB[board]::number[::INSTR]", "GPIB[board]::INTFC", "TCPIP[board]::host address[::LAN device name][::INSTR]", "TCPIP[board]::host address::port::SOCKET"];
    }

    /// <summary> Validates the functional visa versions. </summary>
    /// <remarks> David, 2020-04-16. </remarks>
    /// <returns> The (Success As Boolean, Details As String) </returns>
    public static (bool Success, string Details) ValidateFunctionalVisaVersions()
    {
        using Pith.ResourcesProviderBase rm = SessionFactory.Instance.Factory.ResourcesProvider();
        return rm.ValidateFunctionalVisaVersions();
    }

    #endregion

    #region " resource selection "

    /// <summary> Queries if a given check resource exists. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public override bool QueryResourceExists( string resourceName )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) ) throw new ArgumentNullException( nameof( resourceName ) );
        using Pith.ResourcesProviderBase rm = this.Factory.ResourcesProvider();
        return rm.Exists( resourceName );
    }

    /// <summary> Attempts to select resource from the given data. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public override (bool Success, string Details) TryValidateResource( string resourceName )
    {
        if ( string.IsNullOrWhiteSpace( resourceName ) )
            resourceName = string.Empty;
        (bool Success, string Details) result = (true, string.Empty);
        string activity = string.Empty;
        try
        {
            this.CandidateResourceName = resourceName;
            activity = "checking if need to select the resource";
            if ( string.IsNullOrWhiteSpace( resourceName ) )
            {
                activity = "attempted to select an empty resource name";
                result = (true, activity);
            }
            else if ( string.Equals( resourceName, this.ValidatedResourceName, StringComparison.OrdinalIgnoreCase ) )
            {
                activity = $"resource {resourceName} already validated";
                result = (true, activity);
            }
            else if ( this.ValidationEnabled )
            {
                activity = $"finding resource {resourceName}";
                if ( this.QueryResourceExists( resourceName ) )
                {
                    activity = $"setting validated resource name to {resourceName}";
                    this.ValidatedResourceName = resourceName;
                }
                else
                {
                    activity = $"resource {resourceName} not found; clearing validated resource name";
                    this.ValidatedResourceName = string.Empty;
                }
            }
            else
            {
                activity = $"validation disabled--setting validated resource name to {resourceName}";
                this.ValidatedResourceName = resourceName;
            }
        }
        catch ( Exception ex )
        {
            _ = ex.AddExceptionData();
            result = (false, $"Exception {activity};. {ex.BuildMessage()}");
        }
        finally
        {
        }

        return result;
    }

    /// <summary>   Used to notify of start of the validation of resource name. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <param name="resourceName"> The name of the resource. </param>
    /// <param name="e">            Event information to send to registered event handlers. </param>
    protected override void OnValidatingResourceName( string resourceName, CancelEventArgs e )
    {
        e.Cancel = !this.TryValidateResource( resourceName ).Success;
    }

    /// <summary>   Used to notify of validation of resource name. </summary>
    /// <remarks>   David, 2021-06-28. </remarks>
    /// <param name="resourceName"> The name of the resource. </param>
    /// <param name="e">            Action event information. </param>
    protected override void OnResourceNameValidated( string resourceName, EventArgs e )
    {
    }

    #endregion
}
