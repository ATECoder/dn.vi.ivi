namespace cc.isr.VI.Tsp.Script;
/// <summary>   Interface for script information. </summary>
/// <remarks>   2025-04-15. </remarks>
public interface IScriptInfo
{
    /// <summary>   Initializes this script info. </summary>
    public void Initialize();

    /// <summary>   Rebuild dynamic properties. </summary>
    /// <remarks>   2025-04-15. </remarks>
    public void RebuildDynamicProperties();

    /// <summary>
    /// Gets or sets a value indicating whether this object is automatic execute.
    /// </summary>
    /// <value> True if this object is automatic execute, false if not. </value>
    public bool IsAutoexec { get; set; }

    /// <summary>   Gets or sets the title of the script. </summary>
    /// <value> The name and the file title of the script. </value>
    public string Title { get; set; }

    /// <summary>   Gets or sets the Version of the script. </summary>
    /// <value> The name and the file Version of the script. </value>
    public string Version { get; set; }

    /// <summary>   The built file name [isr_certify.9226.tsp]. </summary>
    /// <value> The filename of the build file. </value>
    public string BuiltFileName { get; set; }

    /// <summary>   The trimmed file name [isr_certify.tsp]. </summary>
    /// <value> The filename of the trimmed file. </value>
    public string TrimmedFileName { get; set; }

    /// <summary>   Gets or sets the deploy file title. </summary>
    /// <value> The version getter. </value>
    public string DeployFileTitle { get; set; }

    /// <summary>   Gets or sets the deploy file name. </summary>
    /// <value> The deploy file name. </value>
    public string DeployFileName { get; set; }

    /// <summary>   Gets or sets the deploy file format. </summary>
    /// <value> The deploy file format. </value>
    public ScriptFileFormats DeployFileFormat { get; set; }

    /// <summary>   Builds the deploy file title. </summary>
    /// <value> The deploy file title. </value>
    public string BuildDeployFileTitle( VersionInfoBase versionInfo );

    /// <summary>   Gets or sets the Version Getter function of the script. </summary>
    /// <value> The version getter function of the script. </value>
    public string VersionGetter { get; set; }

    /// <summary>   The version getter element the script [_G.isr_certify.getVersion]. </summary>
    /// <value> The version getter element. </value>
    public string VersionGetterElement { get; }

    /// <summary>
    /// The name of a pre-requisite script for this script [core.lua.require.lua].
    /// </summary>
    /// <value> The name of the require chunk. </value>
    public string RequireChunkName { get; set; }

    /// <summary>
    /// The name of this scrip as would be required independent scripts
    /// [build.isr_certify_version.tsp].
    /// </summary>
    /// <value> The name of the required chunk. </value>
    public string RequiredChunkName { get; set; }
}
