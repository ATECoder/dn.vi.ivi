using cc.isr.VI.Pith;
using System.Text;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   Copies a script source. </summary>
    /// <remarks>
    /// For binary scripts, the controller and remote nodes must be binary compatible.
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="sourceName">       The name of the source script. </param>
    /// <param name="destinationName">  The name of the destination script. </param>
    public static void CopyScript( this Pith.SessionBase? session, string sourceName, string destinationName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        _ = session.WriteLine( "{1}=script.new( {0}.source , '{1}' ) waitcomplete()", sourceName, destinationName );
    }

    /// <summary>   Copies a script from the controller node to a remote node. </summary>
    /// <remarks>
    /// For binary scripts, the controller and remote nodes must be binary compatible.
    /// </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="nodeNumber">       . </param>
    /// <param name="sourceName">       The script name on the controller node. </param>
    /// <param name="destinationName">  The script name on the remote node. </param>
    public static void CopyScript( this Pith.SessionBase? session, int nodeNumber, string sourceName, string destinationName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        // loads and runs the specified script.
        string commands = string.Format( System.Globalization.CultureInfo.CurrentCulture,
            "node[{0}].execute('waitcomplete() {2}=script.new({1}.source,[[{2}]])') waitcomplete({0}) waitcomplete()", nodeNumber, sourceName, destinationName );
        StringBuilder builder = new();
        _ = builder.AppendLine( $"{cc.isr.VI.Syntax.Tsp.Lua.LoadStringCommand}(table.concat(" );
        _ = builder.Append( commands );
        _ = builder.AppendLine( "))()" );

        // load the script.
        session.WriteLines( builder.ToString(), Environment.NewLine, TimeSpan.Zero );

        // session.LoadString( source );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        _ = session.WriteLine( cc.isr.VI.Syntax.Tsp.Lua.WaitCommand );

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        session.ThrowDeviceExceptionIfError();
    }
}
