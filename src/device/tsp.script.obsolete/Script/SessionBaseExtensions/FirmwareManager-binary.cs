using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{

    /// <summary>   (Immutable) name of the create byte code script function. </summary>
    public const string BinaryScriptsFunctionName = "CreateBinaryScript";

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
    public static void ConvertToByteCode( this Pith.SessionBase? session, string? scriptName, VersionInfoBase instrumentFirmwareVersion )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( instrumentFirmwareVersion.FirmwareVersion is null ) throw new ArgumentNullException( $"{nameof( VersionInfo.FirmwareVersion )}.{nameof( instrumentFirmwareVersion.FirmwareVersion )}" );

        if ( 3 > instrumentFirmwareVersion.FirmwareVersion.Major )
        {
            // firmware versions 1 and 2 convert convert the script to byte code by nulling the scriptSource.
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

            // validate the existence of the function.
            if ( session.IsNil( functionName ) )
                throw new InvalidOperationException( $"{session.ResourceNameNodeCaption} {functionName} must be loaded in order to convert this script to byte code format. The function {functionName} was not found;." );

            session.SetLastAction( $"converting {scriptName} source to byte code" );
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

        // return true if the script is byte code.
        session.SetLastAction( $"checking if {scriptName} is binary" );
        if ( !session.isByteCodeScript( scriptName ) )
            throw new InvalidOperationException( $"Failed conversion {scriptName} to byte code. The converted script is not binary." );
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
    public static void ConvertToByteCode( this Pith.SessionBase? session, string? scriptName, NodeEntityBase node, VersionInfoBase instrumentFirmwareVersion )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( instrumentFirmwareVersion.FirmwareVersion is null ) throw new ArgumentNullException(
            $"{nameof( VersionInfo.FirmwareVersion )}.{nameof( instrumentFirmwareVersion.FirmwareVersion )}" );

        if ( !session.isByteCodeScript( scriptName ) )
        {
            if ( node.IsController )
                session.ConvertToByteCode( scriptName, instrumentFirmwareVersion );
            else
                // displaySubsystem.ConvertBinaryScript( binaryScriptName, node, timeoutInfo );
                throw new InvalidOperationException( "loading byte code scripts to a remote node is not supported at this time." );
        }
    }

}
