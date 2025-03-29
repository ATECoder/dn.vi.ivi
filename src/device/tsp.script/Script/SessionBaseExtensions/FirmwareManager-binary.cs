using cc.isr.Std.AssemblyExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    /// <summary>
    /// A Pith.SessionBase? extension method that loads the binary scripts function from file.
    /// </summary>
    /// <remarks>   2024-10-01. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="filePath"> full path name of the file. </param>
    /// <returns>   True if it the binary scripts function were loaded; otherwise, false. </returns>
    public static bool LoadBinaryScriptFunction( this Pith.SessionBase? session, string filePath )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        string functionName = "CreateBinaryScript";
        if ( !session.IsNil( functionName ) ) return true;

        string? source = FirmwareScriptBase.ReadScript( filePath );
        return LoadBinaryScriptFunction( session, functionName, source );
    }

    /// <summary>
    /// A Pith.SessionBase? extension method that loads the binary scripts function from the provided source.
    /// </summary>
    /// <remarks>   2024-10-01. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="functionName"> The binary scripts function name. </param>
    /// <param name="scriptSource"> The script source. </param>
    /// <returns>   True if it the binary scripts were loaded; otherwise, false. </returns>
    public static bool LoadBinaryScriptFunction( this Pith.SessionBase? session, string? functionName, string? scriptSource )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( functionName is null || string.IsNullOrWhiteSpace( functionName ) ) throw new ArgumentNullException( nameof( functionName ) );
        if ( scriptSource is null || string.IsNullOrWhiteSpace( scriptSource ) ) throw new ArgumentNullException( nameof( scriptSource ) );

        if ( !session.IsNil( functionName ) ) return true;

        // load
        string resourceName = "BinaryScripts.tsp";
        session.SetLastAction( $"loading {resourceName}" );
        // session.LoadString( scriptSource );
        session.LoadScript( scriptSource );

        // check that the script name was created
        session.SetLastAction( $"checking if {functionName} is not nul" );
        return !session.IsNil( functionName );
    }


    /// <summary>   A Pith.SessionBase? extension method that loads the binary scripts function from the embedded resources. </summary>
    /// <remarks>   2024-10-01. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <returns>   True if it the binary scripts were loaded; otherwise, false. </returns>
    public static bool LoadBinaryScriptFunction( this Pith.SessionBase? session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );

        string functionName = "CreateBinaryScript";
        if ( !session.IsNil( functionName ) ) return true;

        string resourceName = "BinaryScripts.tsp";

        // get the scriptSource code, which includes the load script call
        string source = System.Reflection.Assembly.GetExecutingAssembly().ReadEmbeddedTextResource( resourceName );

        // load
        session.SetLastAction( $"loading {resourceName}" );
        // session.LoadString( source );
        session.LoadScript( source );

        // check that the script name was created
        session.SetLastAction( $"checking if {resourceName} is not nul" );
        return !session.IsNil( functionName );
    }

    /// <summary>   A Pith.SessionBase? extension method that converts a session to a binary. </summary>
    /// <remarks>   2024-10-01. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">                      The session. </param>
    /// <param name="scriptName">                   Name of the script. </param>
    /// <param name="instrumentFirmwareVersion">    The instrument model family. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool ConvertToBinary( this Pith.SessionBase? session, string? scriptName, VersionInfoBase instrumentFirmwareVersion )
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
            string functionName = "CreateBinaryScript";
            if ( !session.IsNil( functionName ) )
            {
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
            else
                throw new InvalidOperationException( $"{functionName} not found." );
        }

        // return true if the script is binary.
        session.SetLastAction( $"checking if {scriptName} is binary" );
        return session.IsBinaryScript( scriptName );
    }

    /// <summary>   A Pith.SessionBase? extension method that converts a session to a binary. </summary>
    /// <remarks>   2024-11-23. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="NativeException">              Thrown when a Native error condition occurs. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">                      The session. </param>
    /// <param name="scriptName">                   Name of the script. </param>
    /// <param name="node">                         The node. </param>
    /// <param name="instrumentFirmwareVersion">    The instrument model family. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool ConvertToBinary( this Pith.SessionBase? session, string? scriptName, NodeEntityBase node, VersionInfoBase instrumentFirmwareVersion )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( instrumentFirmwareVersion.FirmwareVersion is null ) throw new ArgumentNullException(
            $"{nameof( VersionInfo.FirmwareVersion )}.{nameof( instrumentFirmwareVersion.FirmwareVersion )}" );

        if ( session.IsBinaryScript( scriptName ) )
            return true;
        else
        {
            if ( node.IsController )
                return session.ConvertToBinary( scriptName, instrumentFirmwareVersion );
            else
                // displaySubsystem.ConvertBinaryScript( binaryScriptName, node, timeoutInfo );
                throw new InvalidOperationException( "loading binary scripts to a remote node is not supported at this time." );
        }

    }


}
