namespace cc.isr.VI.Pith;

public partial class SessionBase
{
    #region " identification "

    private string _identificationQueryCommand = Syntax.Ieee488Syntax.IdentificationQueryCommand;

    /// <summary>   Gets or sets the identification query command. </summary>
    /// <value> The identification query command. </value>
    public virtual string IdentificationQueryCommand
    {
        get => this._identificationQueryCommand;
        set => _ = this.SetProperty( ref this._identificationQueryCommand, value );
    }

    #endregion

    #region " resource name parse info "

    /// <summary> Gets or sets information describing the parsed resource name. </summary>
    /// <value> Information describing the resource. </value>
    public ResourceNameInfo? ResourceNameInfo { get; private set; }

    #endregion

    #region " filter "

    /// <summary> A filter specifying the resources. </summary>
    private string _resourcesFilter = string.Empty;

    /// <summary> Gets or sets the resources search pattern. </summary>
    /// <value> The resources search pattern. </value>
    public string ResourcesFilter
    {
        get => this._resourcesFilter;
        set => _ = base.SetProperty( ref this._resourcesFilter, value );
    }

    #endregion

    #region " name  "

    /// <summary> Updates the validated and open resource names and title. </summary>
    /// <param name="resourceName">  The name of the resource. </param>
    /// <param name="resourceModel"> The short title of the device. </param>
    public void HandleSessionOpen( string resourceName, string resourceModel )
    {
        this.ValidatedResourceName = resourceName;
        this.CandidateResourceNameConnected = string.Equals( resourceName, this.CandidateResourceName, StringComparison.Ordinal );
        this.OpenResourceName = resourceName;
        this.OpenResourceModel = resourceModel;
    }

    /// <summary> Gets the name of the designated resource. </summary>
    /// <value> The name of the designated resource. </value>
    public string DesignatedResourceName => this.CandidateResourceNameConnected ? this.ValidatedResourceName : this.CandidateResourceName;

    /// <summary>
    /// Checks if the candidate resource name exists. If so, assign to the
    /// <see cref="ValidatedResourceName">validated resource name</see>
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public abstract bool ValidateCandidateResourceName();

    /// <summary>
    /// Checks if the candidate resource name exists. If so, assign to the
    /// <see cref="ValidatedResourceName">validated resource name</see>;
    /// </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="manager"> The manager. </param>
    /// <returns> <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public bool ValidateCandidateResourceName( ResourcesProviderBase manager )
    {
        if ( manager is null ) throw new ArgumentNullException( nameof( manager ) );
        bool result = string.IsNullOrWhiteSpace( this.ResourcesFilter )
            ? manager.FindResources().ToArray().Contains( this.CandidateResourceName )
            : manager.FindResources( this.ResourcesFilter! ).ToArray().Contains( this.CandidateResourceName );
        this.ValidatedResourceName = result ? this.CandidateResourceName : string.Empty;
        this.CandidateResourceNameConnected = result;
        return result;
    }

    /// <summary> Name of the validated resource. </summary>
    private string _validatedResourceName = string.Empty;

    /// <summary> Gets or sets the name of the validated (e.g., located) resource. </summary>
    /// <value> The name of the validated (e.g., located)  resource. </value>
    public string ValidatedResourceName
    {
        get => this._validatedResourceName;
        set
        {
            if ( base.SetProperty( ref this._validatedResourceName, value ) )
                this.UpdateCaptions();
        }
    }

    /// <summary> True if candidate resource name validated. </summary>
    private bool _candidateResourceNameValidated;

    /// <summary> Gets or sets the candidate resource name validated. </summary>
    /// <value> The candidate resource name validated. </value>
    public bool CandidateResourceNameConnected
    {
        get => this._candidateResourceNameValidated;
        set
        {
            if ( base.SetProperty( ref this._candidateResourceNameValidated, value ) )
                this.UpdateCaptions();
        }
    }

    /// <summary> Name of the candidate resource. </summary>
    private string _candidateResourceName = string.Empty;

    /// <summary> Gets or sets the name of the resource. </summary>
    /// <value> The name of the candidate resource. </value>
    public string CandidateResourceName
    {
        get => this._candidateResourceName;
        set
        {
            if ( base.SetProperty( ref this._candidateResourceName, value ?? string.Empty ) )
                this.UpdateCaptions();
        }
    }

    /// <summary> Name of the open resource. </summary>
    private string _openResourceName = string.Empty;

    /// <summary> Gets or sets the name of the open resource. </summary>
    /// <value> The name of the open resource. </value>
    public string OpenResourceName
    {
        get => this._openResourceName;
        set
        {
            if ( base.SetProperty( ref this._openResourceName, value ?? string.Empty ) )
                this.UpdateCaptions();
        }
    }

    /// <summary> Updates the captions. </summary>
    private void UpdateCaptions()
    {
        if ( this.IsSessionOpen )
        {
            this.ResourceNameCaption = $"{this.OpenResourceModel}.{this.OpenResourceName}";
            this.ResourceModelCaption = this.OpenResourceModel;
            this.CandidateResourceModel = this.OpenResourceModel;
        }
        else if ( string.IsNullOrWhiteSpace( this.OpenResourceModel ) )
        {
            // if session was not open then display the candidate values
            this.ResourceNameCaption = $"{this.CandidateResourceModel}.{this.CandidateResourceName}.{this.ResourceClosedCaption}";
            this.ResourceModelCaption = $"{this.CandidateResourceModel}.{this.ResourceClosedCaption}";
        }
        else
        {
            // if session was open then display the open name as closed
            this.ResourceNameCaption = $"{this.OpenResourceModel}.{this.OpenResourceName}.{this.ResourceClosedCaption}";
            this.ResourceModelCaption = $"{this.OpenResourceModel}.{this.ResourceClosedCaption}";
        }
    }

    #endregion

    #region " title "

    /// <summary> The candidate resource model. </summary>
    private string _candidateResourceModel = string.Empty;

    /// <summary> Gets or sets the candidate resource model. </summary>
    /// <value> The candidate resource model. </value>
    public string CandidateResourceModel
    {
        get => this._candidateResourceModel;
        set
        {
            if ( base.SetProperty( ref this._candidateResourceModel, value ?? string.Empty ) )
                this.UpdateCaptions();
        }
    }

    private string _openResourceModel = string.Empty;

    /// <summary> Gets or sets a short title for the device. </summary>
    /// <value> The short title of the device. </value>
    public string OpenResourceModel
    {
        get => this._openResourceModel;
        set
        {
            if ( base.SetProperty( ref this._openResourceModel, value ?? string.Empty ) )
                this.UpdateCaptions();
        }
    }

    /// <summary> The resource model caption. </summary>
    private string _resourceModelCaption = string.Empty;

    /// <summary> Gets or sets the Title caption. </summary>
    /// <value> The Title caption. </value>
    public string ResourceModelCaption
    {
        get => this._resourceModelCaption;
        set => _ = base.SetProperty( ref this._resourceModelCaption, value );
    }

    #endregion

    #region " caption "

    /// <summary>   (Immutable) the unknow resource caption. </summary>
    public const string UnknowResourceCaption = "unknown";

    /// <summary> The resource closed caption. </summary>
    private string? _resourceClosedCaption;

    /// <summary> Gets or sets the default resource name closed caption. </summary>
    /// <value> The resource closed caption. </value>
    public string? ResourceClosedCaption
    {
        get => this._resourceClosedCaption;
        set
        {
            if ( base.SetProperty( ref this._resourceClosedCaption, value ) )
                this.UpdateCaptions();
        }
    }

    /// <summary> The resource name caption. </summary>
    private string? _resourceNameCaption;

    /// <summary> Gets or sets the resource name caption. </summary>
    /// <value> The <see cref="ResourceNameCaption"/> resource tagged as closed if not open. </value>
    public string? ResourceNameCaption
    {
        get => this._resourceNameCaption;
        set => _ = base.SetProperty( ref this._resourceNameCaption, value );
    }

    /// <summary>   Gets the resource name node caption. </summary>
    /// <value> The resource name node caption. </value>
    public string ResourceNameNodeCaption => $"{this.ResourceNameCaption ?? SessionBase.UnknowResourceCaption}{this.LastNodeNumberCaption}";

    #endregion
}
