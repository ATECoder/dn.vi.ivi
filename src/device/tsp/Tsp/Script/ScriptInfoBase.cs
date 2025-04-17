using System.ComponentModel;

namespace cc.isr.VI.Tsp.Script;
/// <summary>   A script information base. </summary>
/// <remarks>   2025-04-15. </remarks>
public abstract class ScriptInfoBase : IScriptInfo
{
    #region " construction "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    /// <remarks>   2023-04-24. </remarks>
    protected ScriptInfoBase() { }

    #endregion

    #region " script naming "

    /// <summary>   (Immutable) the script file extension. </summary>
    public const string ScriptFileExtension = ".tsp";

    /// <summary>   (Immutable) the script binary compressed file extension. </summary>
    public const string ScriptCompressedFileExtension = ".tspc";

    /// <summary>   (Immutable) the script binary file extension. </summary>
    public const string ScriptBinaryFileExtension = ".tspb";

    /// <summary>   (Immutable) the script binary compressed file extension. </summary>
    public const string ScriptBinaryCompressedFileExtension = ".tspbc";

    /// <summary>   Select script file extension. </summary>
    /// <remarks>   2025-04-05. </remarks>
    /// <param name="fileFormat">           The file format. </param>
    /// <returns>   A string. </returns>
    public static string SelectScriptFileExtension( ScriptFileFormats fileFormat )
    {
        return (ScriptFileFormats.Binary == (fileFormat & ScriptFileFormats.Binary))
            ? (ScriptFileFormats.Compressed == (fileFormat & ScriptFileFormats.Compressed))
              ? ScriptInfoBase.ScriptBinaryCompressedFileExtension
              : ScriptInfoBase.ScriptBinaryFileExtension
            : (ScriptFileFormats.Compressed == (fileFormat & ScriptFileFormats.Compressed))
              ? ScriptInfoBase.ScriptCompressedFileExtension
              : ScriptInfoBase.ScriptFileExtension;

    }

    /// <summary>   Builds script file title. </summary>
    /// <remarks>   2025-04-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="baseTitle">            The base title. </param>
    /// <param name="fileFormat">           (Optional) [<see cref="ScriptFileFormats.None"/>] The
    ///                                     file format. </param>
    /// <param name="scriptVersion">        (Optional) [empty] The release version. Specify the
    ///                                     version only with build files. </param>
    /// <param name="baseModel">            (Optional) [empty] The base model. </param>
    /// <param name="modelMajorVersion">    (Optional) [empty] The model major version. </param>
    /// <returns>   A string. </returns>
    public static string BuildScriptFileTitle( string baseTitle, ScriptFileFormats fileFormat = ScriptFileFormats.None,
        string scriptVersion = "", string baseModel = "", string modelMajorVersion = "" )
    {
        if ( string.IsNullOrWhiteSpace( baseTitle ) )
            throw new ArgumentNullException( nameof( baseTitle ) );

        string title = baseTitle;

        if ( string.IsNullOrWhiteSpace( scriptVersion ) )
            title = $"{title}.{scriptVersion}";


        if ( ScriptFileFormats.Binary == (fileFormat & ScriptFileFormats.Binary) )
        {
            // binary files are always deployed or loaded from a deployed file.
            if ( string.IsNullOrWhiteSpace( baseModel ) )
                throw new ArgumentNullException( nameof( baseModel ) );
            if ( string.IsNullOrWhiteSpace( modelMajorVersion ) )
                throw new ArgumentNullException( nameof( modelMajorVersion ) );
            title = $"{title}.{baseModel}.{modelMajorVersion}";
        }
        return title;
    }

    /// <summary>   Builds script file name. </summary>
    /// <remarks>   2025-04-05. </remarks>
    /// <param name="baseTitle">            The base title. </param>
    /// <param name="fileFormat">           (Optional) [<see cref="ScriptFileFormats.None"/>] The file format. </param>
    /// <param name="scriptVersion">        (Optional) [empty] The release version. Specify the
    ///                                     version only with build files. </param>
    /// <param name="baseModel">            (Optional) [empty] The base model. </param>
    /// <param name="modelMajorVersion">    (Optional) [empty] The model major version. </param>
    /// <returns>   A string. </returns>
    public static string BuildScriptFileName( string baseTitle, ScriptFileFormats fileFormat = ScriptFileFormats.None,
        string scriptVersion = "", string baseModel = "", string modelMajorVersion = "" )
    {
        string title = ScriptInfoBase.BuildScriptFileTitle( baseTitle, fileFormat, scriptVersion, baseModel, modelMajorVersion );
        string ext = ScriptInfoBase.SelectScriptFileExtension( fileFormat );
        return $"{title}{ext}";
    }

    #endregion

    #region " Interface Implementation "

    /// <summary>   Initializes this object. </summary>
    /// <remarks>   2025-04-15. </remarks>
    public virtual void Initialize()
    {
        this.RebuildDynamicProperties();
    }

    /// <summary>   Rebuild dynamic properties. </summary>
    /// <remarks>   2025-04-15. </remarks>
    public virtual void RebuildDynamicProperties()
    {
        this.BuiltFileName = $"{this.Title}.{new Version( this.Version ).Build}{ScriptInfoBase.ScriptFileExtension}";
        this.TrimmedFileName = $"{this.Title}{ScriptInfoBase.ScriptFileExtension}";
        this.VersionGetter = $"_G.{this.Title}_getVersion()";
    }

    /// <summary>
    /// Gets or sets a value indicating whether this script automatically executes.
    /// </summary>
    /// <value> True if this object is automatic execute, false if not. </value>
    [Description( "Indicates whether this script automatically executes [false]" )]
    public virtual bool IsAutoexec { get; set; } = true;

    /// <summary>   Gets or sets the title of the script. </summary>
    /// <value> The name and the file title of the script. </value>
    [Description( "The name and the file title of the script [isr_autoexec]" )]
    public virtual string Title { get; set; } = "isr_ttm_autoexec";

    /// <summary>   Gets or sets the Version of the script. </summary>
    /// <value> The name and the file Version of the script. </value>
    [Description( "The name and the file Version of the script [2.4.9226]" )]
    public virtual string Version { get; set; } = "2.4.9226";

    /// <summary>   The built file name [isr_ttm_autoexec.9226.tsp]. </summary>
    /// <value> The filename of the build file. </value>
    [Description( "The build file name [isr_ttm_autoexec.9226.tsp]" )]
    public virtual string BuiltFileName { get; set; } = "isr_ttm_autoexec.9226.tsp";

    /// <summary>   The trimmed file name [isr_ttm_autoexec.tsp]. </summary>
    /// <value> The filename of the trimmed file. </value>
    [Description( "The trimmed file name [isr_ttm_autoexec.tsp]" )]
    public virtual string TrimmedFileName { get; set; } = "isr_ttm_autoexec.tsp";

    /// <summary>   Gets or sets the deploy file title. </summary>
    /// <value> The deploy file title. </value>
    public virtual string DeployFileTitle { get; set; } = string.Empty;

    /// <summary>   Gets or sets the deploy file name. </summary>
    /// <value> The deploy file name. </value>
    public virtual string DeployFileName { get; set; } = string.Empty;

    /// <summary>   Gets or sets the deploy file format. </summary>
    /// <value> The deploy file format. </value>
    public virtual ScriptFileFormats DeployFileFormat { get; set; } = ScriptFileFormats.None;

    /// <summary>   Builds the deploy file title. </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <param name="modelFamily">          Information describing the version. </param>
    /// <param name="modelMajorVersion">    [empty] The model major version. </param>
    /// <returns>   The deploy file title. </returns>
    public virtual string BuildDeployFileTitle( string modelFamily, string modelMajorVersion )
    {
        this.DeployFileTitle = ScriptFileFormats.Binary == (this.DeployFileFormat & ScriptFileFormats.Binary)
            ? $"{this.Title}.{modelFamily}.{modelMajorVersion}"
            : this.Title;
        return this.DeployFileTitle;
    }

    /// <summary>   Builds the deploy file title. </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <param name="versionInfo">  Information describing the version. </param>
    /// <returns>   The deploy file title. </returns>
    public virtual string BuildDeployFileTitle( VersionInfoBase versionInfo )
    {
        return this.BuildDeployFileTitle( versionInfo.ModelFamily, versionInfo.FirmwareVersion.Major.ToString( System.Globalization.CultureInfo.CurrentCulture ) );
    }

    /// <summary>   Builds deploy file name. </summary>
    /// <remarks>   2025-04-16. </remarks>
    /// <param name="modelFamily">          Information describing the version. </param>
    /// <param name="modelMajorVersion">    [empty] The model major version. </param>
    /// <returns>   A string. </returns>
    public virtual string BuildDeployFileName( string modelFamily, string modelMajorVersion )
    {
        _ = this.BuildDeployFileTitle( modelFamily, modelMajorVersion );
        this.DeployFileName = $"{this.DeployFileTitle}{ScriptInfoBase.SelectScriptFileExtension( this.DeployFileFormat )}";
        return this.DeployFileName;
    }

    /// <summary>   Builds deploy file name. </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <param name="versionInfo">  Information describing the version. </param>
    /// <returns>   A string. </returns>
    public virtual string BuildDeployFileName( VersionInfoBase versionInfo )
    {
        return this.BuildDeployFileName( versionInfo.ModelFamily, versionInfo.FirmwareVersion.Major.ToString( System.Globalization.CultureInfo.CurrentCulture ) );
    }

    /// <summary>   Gets or sets the Version Getter function of the script. </summary>
    /// <value> The version getter function of the script. </value>
    [Description( "The version getter method of the script [_G.isr_ttm_autoexec_getVersion()]" )]
    public virtual string VersionGetter { get; set; } = "_G.isr_ttm_autoexec_getVersion()";

    /// <summary>   The version getter element the script [_G.isr_ttm_autoexec.getVersion]. </summary>
    /// <value> The version getter element. </value>
    [Description( "The version getter method object of the script [_G.isr_ttm_autoexec.getVersion]" )]
    public virtual string VersionGetterElement => this.VersionGetter.TrimEnd( ['(', ')'] );

    /// <summary>
    /// The name of a pre-requisite script for this script [core.tsp.string.base64.lua].
    /// </summary>
    /// <value> The name of the require chunk. </value>
    [Description( "The name of a pre-requisite script for this script [core.tsp.string.base64.lua]" )]
    public virtual string RequireChunkName { get; set; } = "core.tsp.string.base64.lua";

    /// <summary>
    /// The name of this scrip as would be required independent scripts
    /// [build.isr_autoexec_version.tsp].
    /// </summary>
    /// <value> The name of the required chunk. </value>
    [Description( "The name of this scrip as would be required independent scripts [build.isr_autoexec_version.tsp]" )]
    public virtual string RequiredChunkName { get; set; } = "core.tsp.string.base64.lua";

    #endregion
}
