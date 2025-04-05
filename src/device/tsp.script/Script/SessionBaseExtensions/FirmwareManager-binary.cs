using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{

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
            FirmwareManager.BinaryScriptsFunctionFileName, ResourceManager.EmbeddedResourceFolderName );
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
            FirmwareManager.AccessScriptFunctionFileName, ResourceManager.EmbeddedResourceFolderName );
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
                string filePath = Path.Combine( Script.ResourceManager.ScriptsFolderPath,
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
                    Script.SessionBaseExtensions.FirmwareManager.BinaryScriptsFunctionFileName, Script.ResourceManager.EmbeddedResourceFolderName );

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
