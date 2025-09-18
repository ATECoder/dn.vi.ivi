namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>   A Pith.SessionBase? extension method that loads embedded resource. </summary>
    /// <remarks>   2025-04-03. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="resourceName">         Name of the resource. </param>
    /// <param name="resourceFolderName">   (Optional) name of the folder holding the resource. </param>
    public static void LoadEmbeddedResource( this Pith.SessionBase? session, string resourceName, string resourceFolderName = "resources" )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        string source = ResourceManager.ReadEmbeddedResource( resourceName, resourceFolderName );
        if ( source is null || string.IsNullOrWhiteSpace( source ) )
            throw new InvalidOperationException( $"Failed reading the source from the embedded resource {ResourceManager.AssemblyNamespace}.{resourceFolderName}.{resourceName}." );

        FirmwareManager.LoadAnonymousScript( session, source );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that loads an embedded resource as an
    /// anonymous script.
    /// </summary>
    /// <remarks>   2025-04-03. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="scriptName">           Name of the script. </param>
    /// <param name="runScriptAfterLoad">   True to run script after load. </param>
    /// <param name="resourceName">         Name of the resource. </param>
    /// <param name="resourceFolderName">   (Optional) name of the folder holding the resource. </param>
    public static void LoadEmbeddedResource( this Pith.SessionBase? session, string scriptName, bool runScriptAfterLoad, string resourceName, string resourceFolderName = "resources" )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        string source = ResourceManager.ReadEmbeddedResource( resourceName, resourceFolderName );
        if ( source is null || string.IsNullOrWhiteSpace( source ) )
            throw new InvalidOperationException( $"Failed reading the source from the embedded resource {ResourceManager.AssemblyNamespace}.{resourceFolderName}.{resourceName}." );

        FirmwareManager.LoadNamedScript( session, scriptName, source, runScriptAfterLoad );
    }


    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that loads an embedded resource as an anonymous script.
    /// </summary>
    /// <remarks>   2025-04-01. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="validationObjectName"> Name of the validation object. Skip if exists. </param>
    /// <param name="resourceName">         Name of the resource. </param>
    /// <param name="resourceFolderName">   (Optional) name of the folder holding the resource. </param>
    public static void LoadEmbeddedResource( this Pith.SessionBase? session, string validationObjectName, string resourceName, string resourceFolderName = "resources" )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        // skip if function already exists.
        if ( !session.IsNil( validationObjectName ) ) return;

        // load the byte code script function from the embedded resource.
        FirmwareManager.LoadEmbeddedResource( session, resourceName, resourceFolderName );

        // check that the function was created
        session.SetLastAction( $"returning 'true' if {validationObjectName} exists in the instrument" );
        if ( !session.IsNil( validationObjectName ) )
            throw new InvalidOperationException( $"Failed loading source for the validation object {validationObjectName}; the object was not found in the instrument." );
    }
}
