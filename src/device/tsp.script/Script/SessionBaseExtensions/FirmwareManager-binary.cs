using System.Reflection;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>   (Immutable) pathname of the embedded resource folder. </summary>
    public const string EmbeddedResourceFolderName = "resources";

    /// <summary>   (Immutable) the assembly namespace. </summary>
    public const string AssemblyNamespace = "cc.isr.VI.Tsp";

    /// <summary>   Gets or sets the full pathname of the script files folder. </summary>
    /// <value> The full pathname of the script files folder. </value>
    public static string ScriptsFolderPath { get; set; } = "C:\\my\\private\\ttm\\deploy";

    /// <summary>   (Immutable) name of the create binary script function. </summary>
    public const string BinaryScriptsFunctionName = "CreateBinaryScript";

    /// <summary>   (Immutable) filename of the binary script function file. </summary>
    public const string BinaryScriptsFunctionFileName = "binaryScripts.tspb";

    /// <summary>
    /// Gets or sets a value indicating whether the binary scripts function from file should be
    /// loaded.
    /// </summary>
    /// <value> True if load binary scripts function from file, false if not. </value>
    public static bool LoadBinaryScriptsFunctionFromFile { get; set; } = false;

    /// <summary>   (Immutable) filename of the access script function file. </summary>
    public const string AccessScriptFunctionFileName = "access.tspb";

    /// <summary>   (Immutable) name of the access script function. </summary>
    public const string AccessScriptFunctionName = "isr.access.get1";

    /// <summary>
    /// Gets or sets a value indicating whether the access script function from file should be
    /// loaded.
    /// </summary>
    /// <value> True if load access script function from file, false if not. </value>
    public static bool LoadAccessScriptFunctionFromFile { get; set; } = false;

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
        return FirmwareManager.ReadEmbeddedResource( $"{FirmwareManager.AssemblyNamespace}.{resourceFolderName}.{resourceFileName}" );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that loads an anonymous script.
    /// </summary>
    /// <remarks>   2024-10-01. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptSource"> The script source. </param>
    public static void LoadAnonymousScript( this Pith.SessionBase? session, string? scriptSource )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptSource is null || string.IsNullOrWhiteSpace( scriptSource ) ) throw new ArgumentNullException( nameof( scriptSource ) );

        // decorate source with load script end script and load string as necessary. Must run an anonymous script after load
        scriptSource = FirmwareScriptBase.BuildLoadScriptSyntax( scriptSource, string.Empty, true );

        // write the script source to the instrument.
        session.SetLastAction( "loading anonymous script" );
        session.WriteLines( scriptSource, Environment.NewLine, FirmwareScriptBase.WriteLinesDelay );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        _ = session.WriteLine( cc.isr.VI.Syntax.Tsp.Lua.WaitCommand );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   A Pith.SessionBase? extension method that loads named script. </summary>
    /// <remarks>   2025-04-03. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">              The session. </param>
    /// <param name="scriptName">           Name of the script. </param>
    /// <param name="scriptSource">         The script source. </param>
    /// <param name="runScriptAfterLoad">   True to run script after load. </param>
    public static void LoadNamedScript( this Pith.SessionBase? session, string scriptName, string? scriptSource, bool runScriptAfterLoad )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptSource is null || string.IsNullOrWhiteSpace( scriptSource ) ) throw new ArgumentNullException( nameof( scriptSource ) );

        // decorate source with load script end script and load string as necessary. Must run an anonymous script after load
        scriptSource = FirmwareScriptBase.BuildLoadScriptSyntax( scriptSource, scriptName, runScriptAfterLoad );

        // write the script source to the instrument.
        session.SetLastAction( $"loading script {scriptName}" );
        session.WriteLines( scriptSource, Environment.NewLine, FirmwareScriptBase.WriteLinesDelay );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        _ = session.WriteLine( cc.isr.VI.Syntax.Tsp.Lua.WaitCommand );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        session.ThrowDeviceExceptionIfError();
    }

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

        string source = FirmwareManager.ReadEmbeddedResource( resourceName, resourceFolderName );
        if ( source is null || string.IsNullOrWhiteSpace( source ) )
            throw new InvalidOperationException( $"Failed reading the source from the embedded resource {FirmwareManager.AssemblyNamespace}.{resourceFolderName}.{resourceName}." );

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

        string source = FirmwareManager.ReadEmbeddedResource( resourceName, resourceFolderName );
        if ( source is null || string.IsNullOrWhiteSpace( source ) )
            throw new InvalidOperationException( $"Failed reading the source from the embedded resource {FirmwareManager.AssemblyNamespace}.{resourceFolderName}.{resourceName}." );

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

        // load the binary script function from the embedded resource.
        FirmwareManager.LoadEmbeddedResource( session, resourceName, resourceFolderName );

        // check that the function was created
        session.SetLastAction( $"returning 'true' if {validationObjectName} exists in the instrument" );
        if ( !session.IsNil( validationObjectName ) )
            throw new InvalidOperationException( $"Failed loading source for the validation object {validationObjectName}; the object was not found in the instrument." );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that loads the binary scripts function from
    /// an embedded resource.
    /// </summary>
    /// <remarks>   2024-10-01. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    public static void LoadBinaryScriptFunction( this Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        FirmwareManager.LoadEmbeddedResource( session, FirmwareManager.BinaryScriptsFunctionName,
            FirmwareManager.BinaryScriptsFunctionFileName, FirmwareManager.EmbeddedResourceFolderName );
    }

    /// <summary>
    /// A Pith.SessionBase? extension method that loads access script function from an embedded resource.
    /// </summary>
    /// <remarks>   2025-04-03. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    public static void LoadAccessScriptFunction( this Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        FirmwareManager.LoadEmbeddedResource( session, FirmwareManager.AccessScriptFunctionName,
            FirmwareManager.AccessScriptFunctionFileName, FirmwareManager.EmbeddedResourceFolderName );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that converts a session to a binary.
    /// </summary>
    /// <remarks>   2024-10-01. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">                      The session. </param>
    /// <param name="scriptName">                   Name of the script. </param>
    /// <param name="instrumentFirmwareVersion">    The instrument model family. </param>
    public static void ConvertToBinary( this Pith.SessionBase? session, string? scriptName, VersionInfoBase instrumentFirmwareVersion )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( instrumentFirmwareVersion.FirmwareVersion is null ) throw new ArgumentNullException( $"{nameof( VersionInfo.FirmwareVersion )}.{nameof( instrumentFirmwareVersion.FirmwareVersion )}" );

        if ( 3 > instrumentFirmwareVersion.FirmwareVersion.Major )
        {
            // firmware versions 1 and 2 convert convert the script to binary by nulling the scriptSource.
            session.SetLastAction( $"set {scriptName} source to nil" );
            _ = session.WriteLine( $"{scriptName}.source = nil " );

            _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
            _ = session.WriteLine( cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand );

            try
            {
                session.StoreCommunicationTimeout( TimeSpan.FromMilliseconds( 10000 ) );
                // read query reply and throw if reply is not 1.
                session.ReadAndThrowIfOperationIncomplete();

                // throw if device errors
                session.ThrowDeviceExceptionIfError();
            }
            catch ( Exception )
            {
                throw;
            }
            finally
            {
                session.RestoreCommunicationTimeout();
            }

        }
        else
        {
            string functionName = FirmwareManager.BinaryScriptsFunctionName;

            // firmware 3 and up require the binary conversion function to convert scripts to binary.
            if ( Script.SessionBaseExtensions.FirmwareManager.LoadBinaryScriptsFunctionFromFile )
            {
                // load the binary script function from the file.
                string filePath = Path.Combine( Script.SessionBaseExtensions.FirmwareManager.ScriptsFolderPath,
                    Script.SessionBaseExtensions.FirmwareManager.BinaryScriptsFunctionFileName );
                session.SetLastAction( $"loading {functionName} from {filePath}" );
                session.LoadScript( filePath );

                // validate the existence of the function.
                if ( session.IsNil( functionName ) )
                    throw new InvalidOperationException( $"{session.ResourceNameNodeCaption} failed loading binary script conversion function from {filePath}. The function {functionName} was not found;." );
            }
            else
            {
                // load the binary script function from the firmware.
                session.SetLastAction( $"loading {functionName} from firmware" );

                session.LoadEmbeddedResource( Script.SessionBaseExtensions.FirmwareManager.BinaryScriptsFunctionName,
                    Script.SessionBaseExtensions.FirmwareManager.BinaryScriptsFunctionFileName, Script.SessionBaseExtensions.FirmwareManager.EmbeddedResourceFolderName );

                // validate the existence of the function.
                if ( session.IsNil( functionName ) )
                    throw new InvalidOperationException( $"{session.ResourceNameNodeCaption} failed loading binary script conversion function. The function {functionName} was not found;." );
            }

            // load binary conversion function.

            session.SetLastAction( $"converting {scriptName} source to binary" );
            _ = session.WriteLine( $"{functionName}('{scriptName}', {scriptName}) {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} " );

            // read query reply and throw if reply is not 1.
            try
            {
                session.StoreCommunicationTimeout( TimeSpan.FromMilliseconds( 10000 ) );

                // read query reply and throw if reply is not 1.
                session.ReadAndThrowIfOperationIncomplete();

                // throw if device errors
                session.ThrowDeviceExceptionIfError();
            }
            catch ( Exception )
            {

                throw;
            }
            finally
            {
                session.RestoreCommunicationTimeout();
            }
        }

        // return true if the script is binary.
        session.SetLastAction( $"checking if {scriptName} is binary" );
        if ( !session.IsBinaryScript( scriptName ) )
            throw new InvalidOperationException( $"Failed conversion {scriptName} to binary. The converted script is not binary." );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that converts a session to a binary.
    /// </summary>
    /// <remarks>   2024-11-23. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">                      The session. </param>
    /// <param name="scriptName">                   Name of the script. </param>
    /// <param name="node">                         The node. </param>
    /// <param name="instrumentFirmwareVersion">    The instrument model family. </param>
    public static void ConvertToBinary( this Pith.SessionBase? session, string? scriptName, NodeEntityBase node, VersionInfoBase instrumentFirmwareVersion )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( instrumentFirmwareVersion.FirmwareVersion is null ) throw new ArgumentNullException(
            $"{nameof( VersionInfo.FirmwareVersion )}.{nameof( instrumentFirmwareVersion.FirmwareVersion )}" );

        if ( !session.IsBinaryScript( scriptName ) )
        {
            if ( node.IsController )
                session.ConvertToBinary( scriptName, instrumentFirmwareVersion );
            else
                // displaySubsystem.ConvertBinaryScript( binaryScriptName, node, timeoutInfo );
                throw new InvalidOperationException( "loading binary scripts to a remote node is not supported at this time." );
        }
    }

}
