using System.Reflection;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   A script resource manager base. </summary>
/// <remarks>   2025-04-05. </remarks>
[CLSCompliant( false )]
public abstract class ScriptResourceManagerBase : IScriptResourceManager
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

    /// <summary>   Gets or sets the pathname of the embedded resource folder. </summary>
    /// <value> The pathname of the embedded resource folder. </value>
    public string EmbeddedResourceFolderName { get; set; } = "resources";

    /// <summary>   Gets or sets the assembly namespace. </summary>
    /// <value> The assembly namespace. </value>
    public abstract string AssemblyNamespace { get; set; }

    /// <summary>   Gets or sets the identifier of the script framework. </summary>
    /// <value> The identifier of the script framework. </value>
    public string ScriptFrameworkId { get; set; } = string.Empty;

    #endregion

    #region " read embedded resource "

    /// <summary>   Ge executing assembly. </summary>
    /// <remarks>   2025-05-09. <para>
    /// Implements <see cref="Assembly.GetExecutingAssembly()"/>  </para></remarks>
    /// <returns>   An Assembly? </returns>
    public abstract Assembly GetExecutingAssembly();

    /// <summary>   Builds full resource name. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   A string. </returns>
    public string BuildFullResourceName( string resourceFileName )
    {
        return $"{this.AssemblyNamespace}.{this.EmbeddedResourceFolderName}.{resourceFileName}";
    }

    /// <summary>   Gets resource stream. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The resource stream. </returns>
    public Stream? GetResourceStream( string resourceFileName )
    {
        // gets the assembly that contains the code that is currently running.
        Assembly? assembly = this.GetExecutingAssembly();
        return assembly.GetManifestResourceStream( this.BuildFullResourceName( resourceFileName ) );
    }

    /// <summary>   Gets resource information. </summary>
    /// <remarks>   2025-04-17. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The resource information. </returns>
    public ManifestResourceInfo? GetResourceInfo( string resourceFileName )
    {
        // gets the assembly that contains the code that is currently running.
        Assembly? assembly = this.GetExecutingAssembly();
        string resourceFullName = this.BuildFullResourceName( resourceFileName );
        return assembly.GetManifestResourceInfo( resourceFullName );
    }

    /// <summary>   Query if embedded resource <paramref name="resourceFileName"/> exists. </summary>
    /// <remarks>   2025-04-17. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>
    /// A tuple; True if resource exists, false if not, the resource full name, the assembly location.
    /// </returns>
    public (bool exists, string resourceFullName, string assemlyLocation) ResourceExists( string resourceFileName )
    {
        Assembly? assembly = this.GetExecutingAssembly();
        string assemblyLocation = assembly is null ? string.Empty : assembly.Location;
        string resourceFullName = this.BuildFullResourceName( resourceFileName );
        return (assembly?.GetManifestResourceInfo( resourceFullName ) != null, resourceFullName, assemblyLocation);
    }

    /// <summary>   Gets resource reader. </summary>
    /// <remarks>   2025-04-15. <para>
    /// Returning the stream reader allows rewinding. </para> </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The resource <see cref="StreamReader"/> reader. </returns>
    public StreamReader? GetResourceReader( string resourceFileName )
    {
        // gets the assembly that contains the code that is currently running.
        Assembly? assembly = this.GetExecutingAssembly();
        string resourceFullName = this.BuildFullResourceName( resourceFileName );
        using Stream? stream = assembly.GetManifestResourceStream( resourceFullName )
            ?? throw new FileNotFoundException( $"Embedded resource '{resourceFullName}' not found for the '{assembly.FullName}' assembly." );
        using StreamReader? reader = new StreamReader( stream )
            ?? throw new FileNotFoundException( $"Embedded resource '{resourceFullName}' not found for the '{assembly.FullName}' assembly." );
        return reader;
    }

    /// <summary>   Rewind stream reader. </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="reader">   The reader. </param>
    /// <returns>   A StreamReader. </returns>
    public StreamReader RewindStreamReader( StreamReader reader )
    {
        reader.DiscardBufferedData();
        long position = reader.BaseStream.Seek( 0, SeekOrigin.Begin );
        if ( position > 0 )
            throw new InvalidOperationException( $"Failed rewinding the stream reader to position 0. Current position is {position}." );
        return reader;
    }

    /// <summary>   Reads an embedded resource given the full resource path. </summary>
    /// <remarks>   2025-04-01. </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   The embedded resource content. </returns>
    public string ReadEmbeddedResourceFromFullName( string resourceName )
    {
        // gets teh assembly that contains the code that is currently running.
        Assembly? assembly = this.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream( resourceName )
            ?? throw new FileNotFoundException( $"Embedded resource '{resourceName}' not found for the '{assembly.FullName}' assembly." );
        using StreamReader? reader = new StreamReader( stream )
            ?? throw new FileNotFoundException( $"Embedded resource '{resourceName}' not found for the '{assembly.FullName}' assembly." );
        return reader.ReadToEnd();
    }

    /// <summary>   Reads embedded resource. </summary>
    /// <remarks>   2025-04-03. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The embedded resource. </returns>
    public string ReadEmbeddedResource( string resourceFileName )
    {
        return this.ReadEmbeddedResourceFromFullName( this.BuildFullResourceName( resourceFileName ) );
    }

    #endregion

    #region " read resource file "

    /// <summary>   Gets the assembly folder path. </summary>
    /// <remarks>   2025-05-09. <para>
    /// Implements typeof( ScriptResourceManager ).Assembly.DirectoryPath </para>
    /// </remarks>
    /// <returns>   A string. </returns>
    public abstract string AssemblyFolderPath();

    /// <summary>   Builds the resource file folder path. </summary>
    /// <remarks>   2025-05-09. </remarks>
    /// <returns>   A string. </returns>
    public string BuildResourceFileFolderPath()
    {
        return Path.Combine( this.AssemblyFolderPath(), this.EmbeddedResourceFolderName, this.ScriptFrameworkId );
    }

    /// <summary>   Builds full resource file path. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   A string. </returns>
    public string BuildResourceFilePath( string resourceFileName )
    {
        return Path.Combine( this.BuildResourceFileFolderPath(), resourceFileName );
    }

    /// <summary>   Gets resource file info. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The resource file info. </returns>
    public FileInfo? GetResourceFileInfo( string resourceFileName )
    {
        string filePath = this.BuildResourceFilePath( resourceFileName );
        if ( File.Exists( filePath ) )
            return new FileInfo( filePath );
        else
            return null;
    }

    /// <summary>   Reads resource file. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The embedded resource file. </returns>
    public string ReadResourceFile( string resourceFileName )
    {
        string filePath = this.BuildResourceFilePath( resourceFileName );
        if ( !File.Exists( filePath ) )
        {
            throw new FileNotFoundException( $"Resource file '{filePath}' not found." );
        }
        return System.IO.File.ReadAllText( filePath );
    }

    /// <summary>   Gets resource file reader. </summary>
    /// <remarks>   2025-04-15. <para>
    /// Returning the stream reader allows rewinding. </para> </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The resource file reader. </returns>
    public StreamReader GetResourceFileReader( string resourceFileName )
    {
        string filePath = this.BuildResourceFilePath( resourceFileName );
        if ( !File.Exists( filePath ) )
            throw new FileNotFoundException( $"Resource file '{filePath}' not found." );
        using StreamReader? reader = new StreamReader( filePath )
            ?? throw new FileNotFoundException( $"Failed creating a reader from the '{filePath}' file." );
        return reader;
    }

    #endregion
}
