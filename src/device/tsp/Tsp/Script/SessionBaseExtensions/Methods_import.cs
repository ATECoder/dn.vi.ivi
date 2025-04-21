namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads script file. </summary>
    /// <remarks>   2025-04-20. <para>
    /// Notes:</para><para>
    /// 1. The script must not include the load script command or the end script command. </para><para>
    /// 2. A binary script must include the <see cref="Syntax.Tsp.Lua.LoadStringCommand"/>. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">                  The session. </param>
    /// <param name="scriptName">               Specifies the script name. Empty for an anonymous
    ///                                         script. </param>
    /// <param name="filePath">                 Full pathname of the file. </param>
    /// <param name="lineDelay">                The line delay. </param>
    /// <param name="runScriptAfterLoading">    (Optional) [false] True to run script after loading. </param>
    /// <param name="deleteExisting">           (Optional) [false] True to delete the existing. </param>
    /// <param name="ignoreExisting">           (Optional) [false] True to ignore existing. </param>
    public static void ImportScript( this Pith.SessionBase? session, string scriptName, string filePath, TimeSpan lineDelay,
        bool runScriptAfterLoading = false, bool deleteExisting = false, bool ignoreExisting = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( filePath is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( filePath ) );

        string scriptNameOrAnonymous = string.IsNullOrWhiteSpace( scriptName ) ? "anonymous" : scriptName;

        // read the script source from file.
        string? scriptSource = filePath.ReadScript();
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new ArgumentNullException( $"Failed reading {scriptNameOrAnonymous} script source from {filePath}" );

        // This reads the entire source from the file and then loads the file line by line as source code or Binary
        session.LoadScript( scriptName, scriptSource, lineDelay, runScriptAfterLoading, deleteExisting, ignoreExisting );
    }
}
