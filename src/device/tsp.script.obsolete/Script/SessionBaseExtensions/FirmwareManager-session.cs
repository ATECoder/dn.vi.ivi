namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
    #region " constants "

    /// <summary> The author prefix. </summary>
    private const string Author_Prefix = "isr_";

    #endregion

    #region " copy script "

    /// <summary>   Copies a script source. </summary>
    /// <remarks>
    /// For byte code scripts, the controller and remote nodes must be binary compatible.
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
    /// For byte code scripts, the controller and remote nodes must be binary compatible.
    /// </remarks>
    /// <param name="session">          The session. </param>
    /// <param name="nodeNumber">       . </param>
    /// <param name="sourceName">       The script name on the controller node. </param>
    /// <param name="destinationName">  The script name on the remote node. </param>
    public static void CopyScript( this Pith.SessionBase? session, int nodeNumber, string sourceName, string destinationName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        // loads and runs the specified script.
        string commands = string.Format( System.Globalization.CultureInfo.CurrentCulture,
            "node[{0}].execute('waitcomplete() {2}=script.new({1}.source,[[{2}]])') waitcomplete({0}) waitcomplete()", nodeNumber, sourceName, destinationName );
        // replaced by the code below.
        // LoadString( session, commands );
        session.LoadScript( commands );
    }

    /// <summary>   Copies script source from one script to another. </summary>
    /// <remarks>
    /// For byte code scripts, the controller and remote nodes must be binary compatible.
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="node">             Specifies a node on which to copy. </param>
    /// <param name="sourceName">       The script name on the controller node. </param>
    /// <param name="destinationName">  The script name on the remote node. </param>
    public static void CopyScript( this Pith.SessionBase? session, NodeEntityBase node, string sourceName, string destinationName )
    {
        if ( node is null ) throw new ArgumentNullException( nameof( node ) );
        if ( node.IsController )
            CopyScript( session, sourceName, destinationName );
        else
            CopyScript( session, node.Number, sourceName, destinationName );
    }

    /// <summary>   Loads and runs an anonymous script. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="commands"> Specifies the script commands. </param>
    /// <returns>   <c>true</c> if okay; otherwise, <c>false</c>. </returns>
    public static bool LoadRunAnonymousScript( this Pith.SessionBase? session, string commands )
    {
        string prefix = cc.isr.VI.Syntax.Tsp.Script.LoadAndRunScriptCommand;
        string suffix = $"{cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand} {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand}";
        string loadCommand = string.Format( System.Globalization.CultureInfo.CurrentCulture, "{1}{0}{2}{0}{3}", Environment.NewLine, prefix, commands, suffix );
        // replaced by the code blows.
        // LoadString( session, loadCommand );
        session.LoadScript( loadCommand );
        return true;
    }

    #endregion

    #region " misc queries "

    /// <summary>   Checks if the script is byte code. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Specifies the name of the script. </param>
    /// <param name="node">         Specifies the node entity. </param>
    /// <returns>   <c>true</c> if the script is a byte code script; otherwise, <c>false</c>. </returns>
    public static bool? isByteCodeScript( this Pith.SessionBase session, string scriptName, NodeEntityBase? node )
    {
        if ( node is not null )
            session.SetLastAction( $"checking if loaded script '{scriptName}' is binary on node {node.Number}" );
        return node is null
            ? throw new ArgumentNullException( nameof( node ) )
            : node.IsController
                ? session.IsStatementTrue( "_G.isr.script.isByteCode({0})", scriptName )
                : session.IsStatementTrue( "_G.isr.script.isByteCode({0},{1})", scriptName, node.Number );
    }

    /// <summary>   Checks if the script is byte code. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="script">   The script. </param>
    /// <returns>   <c>true</c> if the script is a byte code script; otherwise, <c>false</c>. </returns>
    public static bool? isByteCodeScript( this Pith.SessionBase session, ScriptEntityBase script )
    {
        return script.Node.IsController
                ? session.IsStatementTrue( "_G.isr.script.isByteCode({0})", script.Name )
                : session.IsStatementTrue( "_G.isr.script.isByteCode({0},{1})", script.Name, script.Node.Number );
    }

    /// <summary>   A Pith.SessionBase extension method that query if 'script' name is nil. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="script">   The script. </param>
    /// <returns>   True if nil, false if not. </returns>
    public static bool IsNil( this Pith.SessionBase session, ScriptEntityBase script )
    {
        return script.Node.IsController
            ? session.IsNil( script.Name )
            : session.IsNil( script.Node.Number, script.Name );
    }

    /// <summary>
    /// A Pith.SessionBase extension method that query if the script namespaces exist.
    /// </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <param name="session">  The session. </param>
    /// <param name="script">   The script. </param>
    /// <returns>   True if script namespaces, false if not. </returns>
    public static bool HasScriptNamespaces( this Pith.SessionBase session, ScriptEntityBase script )
    {
        return script.FirmwareScript.Namespaces is not null && (script.FirmwareScript.Namespaces.Length > 0)
            && (script.Node.IsController
                ? session.IsNil( script.FirmwareScript.Namespaces )
                : session.IsNil( script.Node.Number, script.FirmwareScript.Namespaces ));
    }

    #endregion
}
