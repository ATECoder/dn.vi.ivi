using System.Reflection;

namespace cc.isr.VI.Tsp.Script;
/// <summary>   Manager for resources. </summary>
/// <remarks>   2025-04-05. </remarks>
public class ResourceManager
{

    /// <summary>   (Immutable) pathname of the embedded resource folder. </summary>
    public const string EmbeddedResourceFolderName = "resources";

    /// <summary>   (Immutable) the assembly namespace. </summary>
    public const string AssemblyNamespace = "cc.isr.VI.Tsp";

    /// <summary>   Gets or sets the full pathname of the script files folder. </summary>
    /// <value> The full pathname of the script files folder. </value>
    public static string ScriptsFolderPath { get; set; } = "C:\\my\\private\\ttm\\deploy";

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
}
