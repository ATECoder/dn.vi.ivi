using System.Reflection;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   Manager for resources. </summary>
/// <remarks>   2025-04-05. </remarks>
public interface IScriptResourceManager
{
    #region " embedded resource assembly info "

    /// <summary>   Gets or sets the pathname of the embedded resource folder. </summary>
    /// <value> The pathname of the embedded resource folder. </value>
    public string EmbeddedResourceFolderName { get; set; }

    /// <summary>
    /// Gets or sets the a resource subfolder. This could be empty.
    /// </summary>
    /// <value> The resource subfolder. </value>
    public string ResourceSubfolder { get; set; }

    /// <summary>   Gets or sets the assembly namespace. </summary>
    /// <value> The assembly namespace. </value>
    public string AssemblyNamespace { get; set; }

    #endregion

    #region " read embedded resource "

    /// <summary>   Get executing assembly. </summary>
    /// <remarks>   2025-05-09. <para>
    /// Implements <see cref="Assembly.GetExecutingAssembly()"/>  </para></remarks>
    /// <returns>   An Assembly </returns>
    public Assembly GetExecutingAssembly();

    /// <summary>   Builds full resource name. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   A string. </returns>
    public string BuildFullResourceName( string resourceFileName );

    /// <summary>   Gets resource stream. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The resource stream. </returns>
    public Stream? GetResourceStream( string resourceFileName );

    /// <summary>   Gets resource information. </summary>
    /// <remarks>   2025-04-17. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The resource information. </returns>
    public ManifestResourceInfo? GetResourceInfo( string resourceFileName );

    /// <summary>   Query if embedded resource <paramref name="resourceFileName"/> exists. </summary>
    /// <remarks>   2025-04-17. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>
    /// A tuple; True if resource exists, false if not, the resource full name, the assembly location.
    /// </returns>
    public (bool exists, string resourceFullName, string assemlyLocation) ResourceExists( string resourceFileName );

    /// <summary>   Gets resource reader. </summary>
    /// <remarks>   2025-04-15. <para>
    /// Returning the stream reader allows rewinding. </para> </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The resource <see cref="StreamReader"/> reader. </returns>
    public StreamReader? GetResourceReader( string resourceFileName );

    /// <summary>   Rewind stream reader. </summary>
    /// <remarks>   2025-04-15. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="reader">   The reader. </param>
    /// <returns>   A StreamReader. </returns>
    public StreamReader RewindStreamReader( StreamReader reader );

    /// <summary>   Reads an embedded resource given the full resource path. </summary>
    /// <remarks>   2025-04-01. </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns>   The embedded resource content. </returns>
    public string ReadEmbeddedResourceFromFullName( string resourceName );

    /// <summary>   Reads embedded resource. </summary>
    /// <remarks>   2025-04-03. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The embedded resource. </returns>
    public string ReadEmbeddedResource( string resourceFileName );

    #endregion

    #region " read resource file "

    /// <summary>   Gets the assembly folder path. </summary>
    /// <remarks>   2025-05-09. <para>
    /// Implements typeof( ResourceManagerInstance ).Assembly.DirectoryPath </para>
    /// </remarks>
    /// <returns>   A string. </returns>
    public string AssemblyFolderPath();

    /// <summary>   Builds the resource file folder path. </summary>
    /// <remarks>   2025-04-22. </remarks>
    /// <returns>   A string. </returns>
    public string BuildResourceFileFolderPath();

    /// <summary>   Builds full resource file path. </summary>
    /// <remarks>   2025-04-08. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   A string. </returns>
    public string BuildResourceFilePath( string resourceFileName );

    /// <summary>   Query if the resource file <paramref name="resourceFileName"/> exists. </summary>
    /// <param name="resourceFileName"> Filename of the resource file. </param>
    /// <returns>   The resource file information. </returns>
    public (bool exists, string resourceFullName, string assemlyLocation) ResourceFileExists( string resourceFileName );

    /// <summary>   Gets resource file info. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The resource file info. </returns>
    public FileInfo? GetResourceFileInfo( string resourceFileName );

    /// <summary>   Reads resource file. </summary>
    /// <remarks>   2025-04-07. </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The contents of the resource file or empty if the resource file is empty or not found. </returns>
    public string ReadResourceFile( string resourceFileName );

    /// <summary>   Gets resource file reader. </summary>
    /// <remarks>   2025-04-15. <para>
    /// Returning the stream reader allows rewinding. </para> </remarks>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="resourceFileName">     Filename of the resource file. </param>
    /// <returns>   The resource file reader. </returns>
    public StreamReader GetResourceFileReader( string resourceFileName );

    #endregion
}
