// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

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

    /// <summary> Gets or sets the resources search pattern. </summary>
    /// <value> The resources search pattern. </value>
    public string ResourcesFilter
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    } = string.Empty;

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

    /// <summary> Gets or sets the name of the validated (e.g., located) resource. </summary>
    /// <value> The name of the validated (e.g., located)  resource. </value>
    public string ValidatedResourceName
    {
        get;
        set
        {
            if ( base.SetProperty( ref field, value ) )
                this.UpdateCaptions();
        }
    } = string.Empty;

    /// <summary> Gets or sets the candidate resource name validated. </summary>
    /// <value> The candidate resource name validated. </value>
    public bool CandidateResourceNameConnected
    {
        get;
        set
        {
            if ( base.SetProperty( ref field, value ) )
                this.UpdateCaptions();
        }
    }

    /// <summary> Gets or sets the name of the resource. </summary>
    /// <value> The name of the candidate resource. </value>
    public string CandidateResourceName
    {
        get;
        set
        {
            if ( base.SetProperty( ref field, value ?? string.Empty ) )
                this.UpdateCaptions();
        }
    } = string.Empty;

    /// <summary> Gets or sets the name of the open resource. </summary>
    /// <value> The name of the open resource. </value>
    public string OpenResourceName
    {
        get;
        set
        {
            if ( base.SetProperty( ref field, value ?? string.Empty ) )
                this.UpdateCaptions();
        }
    } = string.Empty;

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

    /// <summary> Gets or sets the candidate resource model. </summary>
    /// <value> The candidate resource model. </value>
    public string CandidateResourceModel
    {
        get;
        set
        {
            if ( base.SetProperty( ref field, value ?? string.Empty ) )
                this.UpdateCaptions();
        }
    } = string.Empty;

    /// <summary> Gets or sets a short title for the device. </summary>
    /// <value> The short title of the device. </value>
    public string OpenResourceModel
    {
        get;
        set
        {
            if ( base.SetProperty( ref field, value ?? string.Empty ) )
                this.UpdateCaptions();
        }
    } = string.Empty;

    /// <summary> Gets or sets the Title caption. </summary>
    /// <value> The Title caption. </value>
    public string ResourceModelCaption
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    } = string.Empty;

    #endregion

    #region " caption "

    /// <summary>   (Immutable) the unknow resource caption. </summary>
    public const string UnknowResourceCaption = "unknown";

    /// <summary> Gets or sets the default resource name closed caption. </summary>
    /// <value> The resource closed caption. </value>
    public string? ResourceClosedCaption
    {
        get;
        set
        {
            if ( base.SetProperty( ref field, value ) )
                this.UpdateCaptions();
        }
    }

    /// <summary> Gets or sets the resource name caption. </summary>
    /// <value> The <see cref="ResourceNameCaption"/> resource tagged as closed if not open. </value>
    public string? ResourceNameCaption
    {
        get;
        set => _ = base.SetProperty( ref field, value );
    }

    /// <summary>   Gets the resource name node caption. </summary>
    /// <value> The resource name node caption. </value>
    public string ResourceNameNodeCaption => $"{this.ResourceNameCaption ?? SessionBase.UnknowResourceCaption}{this.LastNodeNumberCaption}";

    #endregion
}
