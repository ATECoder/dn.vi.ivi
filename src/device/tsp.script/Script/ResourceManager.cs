using System.Reflection;

namespace cc.isr.VI.Tsp.Script;
/// <summary>   Manager for resources. </summary>
/// <remarks>   2025-04-05. </remarks>
[CLSCompliant( false )]
public static class ResourceManager
{
    /// <summary>   (Immutable) the binary script title. </summary>
    public const string BinaryScriptTitle = "isr_binaryScripts";

    /// <summary>   Define binary script. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <param name="firmwareVersion">  The firmware version, e.g., 2.4.9226. </param>
    /// <param name="modelMask">        The model mask. </param>
    /// <param name="modelVersion">     The model version. </param>
    /// <returns>   A ScriptEntity. </returns>
    public static FirmwareScriptBase DefineBinaryScript( string firmwareVersion, string modelMask, Version modelVersion )
    {
        FirmwareScript firmwareScript = new( ResourceManager.BinaryScriptTitle, modelMask, modelVersion )
        {
            FileTitle = ResourceManager.BinaryScriptTitle,
            DeployFileFormat = FirmwareScriptBase.BuildScriptFileFormat( true, false ),
            SaveToNonVolatileMemory = true,
            NamespaceList = "_G",
            FirmwareVersion = firmwareVersion,
            IsPrimaryScript = true,
        };
        return firmwareScript;
    }

    /// <summary>   Define binary script. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="firmwareVersion">          The firmware version, e.g., 2.4.9226. </param>
    /// <param name="instrumentVersionInfo">    Information describing the instrument version. </param>
    /// <param name="node">                     The node. </param>
    /// <returns>   A ScriptEntity. </returns>
    public static ScriptEntity DefineBinaryScript( string firmwareVersion, VersionInfoBase instrumentVersionInfo, NodeEntityBase node )
    {
        if ( instrumentVersionInfo is null ) throw new ArgumentNullException( nameof( instrumentVersionInfo ) );
        if ( instrumentVersionInfo.FirmwareVersion is null )
            throw new ArgumentNullException( $"{nameof( instrumentVersionInfo )}.{nameof( instrumentVersionInfo.FirmwareVersion )}" );

        FirmwareScriptBase firmwareScript = ResourceManager.DefineBinaryScript( firmwareVersion, instrumentVersionInfo.ModelFamily,
            instrumentVersionInfo.FirmwareVersion );

        ScriptEntity script = new( firmwareScript, node )
        {
            FirmwareVersionGetter = $"_G.print({firmwareVersion})"
        };

        return script;
    }

    /// <summary>   Gets or sets the script entity. </summary>
    /// <value> The script entity. </value>
    public static ScriptEntity? BinaryScriptEntity { get; set; } = null;

    /// <summary>
    /// Gets or sets the full pathname of the embedded resources files folder file.
    /// </summary>
    /// <value> The full pathname of the embedded resources files folder file. </value>
    public static string EmbeddedResourcesFilesFolderPath { get; set; } = Path.Combine( typeof( ResourceManager ).Assembly.Location,
        ResourceManager.EmbeddedResourceFolderName );

    /// <summary>   (Immutable) pathname of the embedded resource folder. </summary>
    public const string EmbeddedResourceFolderName = "resources";

    /// <summary>   (Immutable) the assembly namespace. </summary>
    public const string AssemblyNamespace = "cc.isr.VI.Tsp";

    /// <summary>   Gets resource stream. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   The resource stream. </returns>
    public static Stream? GetResourceStream( string resourceName )
    {
        // gets teh assembly that contains the code that is currently running.
        Assembly? assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream( resourceName );
    }

    /// <summary>   Reads an embedded resource given the full resource path. </summary>
    /// <remarks>   2025-04-01. </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   The embedded resource content. </returns>
    public static string ReadEmbeddedResource( string resourceName )
    {
        // gets teh assembly that contains the code that is currently running.
        Assembly? assembly = Assembly.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream( resourceName )
            ?? throw new FileNotFoundException( $"Embedded resource '{resourceName}' not found not found for the '{assembly.FullName}' assembly." );
        using StreamReader reader = new( stream );
        return reader.ReadToEnd();
    }

    /// <summary>   Reads embedded resource. </summary>
    /// <remarks>   2025-04-03. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <param name="resourceFolderName">   (Optional) name of the folder holding the resource. </param>
    /// <returns>   The embedded resource. </returns>
    public static string ReadEmbeddedResource( string resourceFileName, string resourceFolderName = "resources" )
    {
        return ResourceManager.ReadEmbeddedResource( $"{ResourceManager.AssemblyNamespace}.{resourceFolderName}.{resourceFileName}" );
    }

    /// <summary>   Gets embedded resource file. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <param name="resourceFolderName">   (Optional) name of the folder holding the resource. </param>
    /// <returns>   The embedded resource file. </returns>
    public static FileInfo? GetEmbeddedResourceFile( string resourceFileName, string resourceFolderName = "resources" )
    {
        string filePath = Path.Combine( EmbeddedResourcesFilesFolderPath, resourceFolderName, resourceFileName );
        if ( File.Exists( filePath ) )
            return new FileInfo( filePath );
        else
            return null;
    }

    /// <summary>   Reads embedded resource file. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <param name="resourceFolderName">   (Optional) name of the folder holding the resource. </param>
    /// <returns>   The embedded resource file. </returns>
    public static string ReadEmbeddedResourceFile( string resourceFileName, string resourceFolderName = "resources" )
    {
        string filePath = Path.Combine( EmbeddedResourcesFilesFolderPath, resourceFolderName, resourceFileName );
        if ( !File.Exists( filePath ) )
        {
            throw new FileNotFoundException( $"Embedded resource '{filePath}' not found." );
        }
        return System.IO.File.ReadAllText( filePath );
    }

}
