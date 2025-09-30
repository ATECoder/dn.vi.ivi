using cc.isr.Std.TrimExtensions;
using cc.isr.VI.Pith;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

/// <summary>   Manager for user scripts. </summary>
/// <remarks>   2024-09-09. </remarks>
public static partial class FirmwareManager
{
    /// <summary>   Runs the named script and wait for an outcome value of 1. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="DeviceException">          Thrown when a Device error condition occurs. </exception>
    /// <exception cref="TimeoutException">         Thrown when a Timeout error condition occurs. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="nodeNumber">       Specifies the subsystem node; null if this is the controller
    ///                                 node. </param>
    /// <param name="scriptName">       Specifies the script name. </param>
    public static void RunScriptWaitOne( this Pith.SessionBase? session, int? nodeNumber, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        string returnedValue = cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue;
        session.LastNodeNumber = nodeNumber;
        if ( nodeNumber.HasValue )
        {
            session.SetLastAction( $"Running script '{scriptName}' on node {nodeNumber.Value}" );
            _ = session.WriteLine( $"node[{nodeNumber}].execute( '{scriptName}.run()' ) print('{returnedValue}') " );
        }
        else
        {
            session.SetLastAction( $"Running script '{scriptName}' on the controller node" );
            _ = session.WriteLine( $"{scriptName}.run() print('{returnedValue}') " );
        }

        // read query reply and throw if reply is not 1.
        session.ReadAndThrowIfOperationIncomplete();

        // throw if device errors
        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   Runs the named script amd wait for a query completion. </summary>
    /// <remarks>   Waits for operation completion query reply. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="DeviceException">          Thrown when a Device error condition occurs. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptName">       Specifies the script name. </param>
    public static void RunScript( this Pith.SessionBase? session, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = default;

        session.SetLastAction( $"Running script '{scriptName}' on the controller node" );
        _ = session.WriteLine( $"{scriptName}.run() {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} " );

        // read query reply and throw if reply is not 1.
        session.ReadAndThrowIfOperationIncomplete();

        // throw if device errors
        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   Runs the named script and wait for completion query reply. </summary>
    /// <remarks>   Waits for operation completion reply. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="DeviceException">          Thrown when a Device error condition occurs. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="nodeNumber">       Specifies the subsystem node; null if this is the controller
    ///                                 node. </param>
    /// <param name="scriptName">       Specifies the script name. </param>
    public static void RunScript( this Pith.SessionBase? session, int? nodeNumber, string scriptName )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );

        session.LastNodeNumber = nodeNumber;
        if ( nodeNumber.HasValue )
        {
            session.SetLastAction( $"Running script '{scriptName}' on node {nodeNumber.Value}." );
            _ = session.WriteLine( $"node[{nodeNumber}].execute( '{scriptName}.run()' ) {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} " );
        }
        else
        {
            session.SetLastAction( $"Running script '{scriptName}' on the controller node." );
            _ = session.WriteLine( $"{scriptName}.run() {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} " );
        }

        // read query reply and throw if reply is not 1.
        session.ReadAndThrowIfOperationIncomplete();

        // throw if device errors
        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   Executes the 'script' and wait for completion. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="script">   The script. </param>
    /// <param name="always">   (Optional) True to always ran. </param>
    public static void RunScript( this Pith.SessionBase? session, ScriptEntityBase script, bool always = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );

        System.Text.StringBuilder builder = new();
        script.LastScriptManagerActions = string.Empty;

        if ( !(script.FirmwareScript.IsBootScript || string.IsNullOrWhiteSpace( script.Name )) )
        {
            if ( script.Loaded )
            {
                if ( script.Activated && !always )
                {
                    _ = builder.AppendLine( $"{session.ResourceNameNodeCaption} script '{script.Name}' already activated on node {script.Node.Number}." );
                }
                else
                {
                    session.SetLastAction( $"{session.ResourceNameNodeCaption} running {script.Name} On node {script.Node.Number}." );
                    session.LastNodeNumber = script.Node.Number;

                    try
                    {
                        // if script not ran, run it now. Throws exception on failure.
                        session.RunScript( script.Node.IsController ? new int?() : script.Node.Number, script.Name );

                        script.Loaded = !session.IsNil( script.Node.IsController, script.Node.Number, script.Name );

                        if ( script.Loaded )
                        {
                            script.Activated = !session.IsNil( script.Node.IsController, script.Node.Number, script.FirmwareScript.Namespaces );

                            if ( script.Activated )
                                _ = builder.AppendLine( $"{session.ResourceNameNodeCaption} script '{script.Name}' activated on node {script.Node.Number}." );
                            else
                                _ = builder.AppendLine( $"{session.ResourceNameNodeCaption} script '{script.Name}.{script.FirmwareScript.NamespaceList}' are nil; script failed to activated on node {script.Node.Number}." );
                        }
                        else
                        {
                            script.Activated = false;
                            script.Embedded = false;
                            script.EmbeddedFirmwareVersion = string.Empty;
                            script.HasFirmwareVersionGetter = false;
                            _ = builder.AppendLine( $"{session.ResourceNameNodeCaption} script '{script.Name}' not found after running on node {script.Node.Number}." );
                        }
                        script.LastScriptManagerActions = builder.ToString().TrimEndNewLine();
                    }
                    catch ( Exception ex )
                    {
                        script.Activated = false;
                        script.Embedded = false;
                        script.EmbeddedFirmwareVersion = string.Empty;
                        script.HasFirmwareVersionGetter = false;
                        script.LastScriptManagerActions = $"Exception occurred running script: {ex}.";
                    }
                }
            }
            else
            {
                script.Activated = false;
                _ = builder.AppendLine( $"{session.ResourceNameNodeCaption} script '{script.Name}' is not loaded." );
                script.LastScriptManagerActions = builder.ToString().TrimEndNewLine();
            }
        }
    }
}
