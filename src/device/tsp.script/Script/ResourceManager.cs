using System.Reflection;
using cc.isr.Std.AssemblyExtensions;

namespace cc.isr.VI.Tsp.Script;
/// <summary>   Manager for resources. </summary>
/// <remarks>   2025-04-05. </remarks>
[CLSCompliant( false )]
public static class ResourceManager
{

    #region " create resources "

    /// <summary>   Copies a file. </summary>
    /// <remarks>
    /// 2025-04-08. <para>
    /// This is used for testing if a file can be copied while linked as a resource. </para>
    /// </remarks>
    /// <param name="fromPath"> Full pathname of from file. </param>
    /// <param name="toPath">   Full pathname of to file. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the test passes, false if the test fails. </returns>
    public static bool CopyFile( string fromPath, string toPath, out string details )
    {
        if ( !System.IO.File.Exists( fromPath ) )
        {
            details = $"{fromPath} not found.";
            return false;
        }
        else
        {
            System.IO.File.Copy( fromPath, toPath, true );
            if ( !System.IO.File.Exists( toPath ) )
            {
                details = $"to file {toPath} not found; copy failed";
                return false;
            }
            else
            {
                details = string.Empty;
                return true;
            }
        }
    }

    /// <summary>   Writes a file. </summary>
    /// <remarks>
    /// 2025-04-08. <para>
    /// This is used for testing if a file can be created while linked as a resource. </para>
    /// </remarks>
    /// <param name="toPath">   Full pathname of to file. </param>
    /// <param name="contents"> The contents. </param>
    /// <param name="details">  [out] The details. </param>
    /// <returns>   True if the test passes, false if the test fails. </returns>
    public static bool WriteFile( string toPath, string contents, out string details )
    {
        System.IO.File.WriteAllText( toPath, contents );
        if ( !System.IO.File.Exists( toPath ) )
        {
            details = $"to file {toPath} not found; copy failed";
            return false;
        }
        else
        {
            details = string.Empty;
            return true;
        }
    }

    #endregion

    #region " embedded resource assembly info "

    /// <summary>   (Immutable) pathname of the embedded resource folder. </summary>
    public const string EmbeddedResourceFolderName = "resources";

    /// <summary>   (Immutable) the assembly namespace. </summary>
    public const string AssemblyNamespace = "cc.isr.VI.Tsp";

    #endregion

    #region " read embedded resource "

    /// <summary>   Builds full resource name. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <param name="resourceFolderName">   (Optional) ['resources'] name of the folder holding the resource. </param>
    /// <returns>   A string. </returns>
    public static string BuildFullResourceName( string resourceFileName, string resourceFolderName = "resources" )
    {
        return $"{ResourceManager.AssemblyNamespace}.{resourceFolderName}.{resourceFileName}";
    }

    /// <summary>   Gets resource stream. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <param name="resourceFolderName">   (Optional) ['resources'] name of the folder holding the resource. </param>
    /// <returns>   The resource stream. </returns>
    public static Stream? GetResourceStream( string resourceFileName, string resourceFolderName = "resources" )
    {
        // gets the assembly that contains the code that is currently running.
        Assembly? assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream( ResourceManager.BuildFullResourceName( resourceFileName, resourceFolderName ) );
    }

    /// <summary>   Reads an embedded resource given the full resource path. </summary>
    /// <remarks>   2025-04-01. </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   The embedded resource content. </returns>
    public static string ReadEmbeddedResourceFromFullName( string resourceName )
    {
        // gets teh assembly that contains the code that is currently running.
        Assembly? assembly = Assembly.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream( resourceName )
            ?? throw new FileNotFoundException( $"Embedded resource '{resourceName}' not found for the '{assembly.FullName}' assembly." );
        using StreamReader reader = new( stream );
        return reader.ReadToEnd();
    }

    /// <summary>   Reads embedded resource. </summary>
    /// <remarks>   2025-04-03. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <param name="resourceFolderName">   (Optional) ['resources'] name of the folder holding the resource. </param>
    /// <returns>   The embedded resource. </returns>
    public static string ReadEmbeddedResource( string resourceFileName, string resourceFolderName = "resources" )
    {
        return ResourceManager.ReadEmbeddedResourceFromFullName( ResourceManager.BuildFullResourceName( resourceFileName, resourceFolderName ) );
    }

    #endregion

    #region " read resource file "

    /// <summary>   Builds full resource file path. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <param name="resourceFolderName">   (Optional) ['resources'] name of the folder holding the resource. </param>
    /// <returns>   A string. </returns>
    public static string BuildResourceFilePath( string resourceFileName, string resourceFolderName = "resources" )
    {
        return Path.Combine( typeof( ResourceManager ).Assembly.DirectoryPath(), resourceFolderName, resourceFileName );
    }

    /// <summary>   Gets resource file info. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <param name="resourceFolderName">   (Optional) ['resources'] name of the folder holding the resource. </param>
    /// <returns>   The resource file info. </returns>
    public static FileInfo? GetResourceFileInfo( string resourceFileName, string resourceFolderName = "resources" )
    {
        string filePath = ResourceManager.BuildResourceFilePath( resourceFileName, resourceFolderName );
        if ( File.Exists( filePath ) )
            return new FileInfo( filePath );
        else
            return null;
    }

    /// <summary>   Reads resource file. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <param name="resourceFolderName">   (Optional) ['resources'] name of the folder holding the resource. </param>
    /// <returns>   The embedded resource file. </returns>
    public static string ReadResourceFile( string resourceFileName, string resourceFolderName = "resources" )
    {
        string filePath = ResourceManager.BuildResourceFilePath( resourceFileName, resourceFolderName );
        if ( !File.Exists( filePath ) )
        {
            throw new FileNotFoundException( $"Embedded resource '{filePath}' not found." );
        }
        return System.IO.File.ReadAllText( filePath );
    }

    #endregion
}
