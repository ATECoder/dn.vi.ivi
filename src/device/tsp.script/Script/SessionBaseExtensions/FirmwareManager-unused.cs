namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{
#if false
    /// <summary>   A <see cref="Pith.SessionBase"/> extension method that loads script file. </summary>
    /// <remarks>   2024-12-12. </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Contains the script name; could be empty if loading an anonymous script. </param>
    /// <param name="filePath">     full path name of the file. </param>
    public static void LoadScriptFile( this Pith.SessionBase? session, string scriptName, string filePath )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( filePath is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( filePath ) );

        string scriptNameOrAnonymous = string.IsNullOrWhiteSpace( scriptName ) ? "anonymous" : scriptName;

        // read the script source from file.
        string? scriptSource = FirmwareScriptBase.ReadScript( filePath );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new ArgumentNullException( $"Failed reading {scriptNameOrAnonymous} script source from {filePath}" );

        // This reads the entire source from the file and then loads the file line by line as source code or Binary
        (bool timedOut, _, _) = session.LoadScriptSource( scriptName, scriptSource! );
        if ( timedOut )
            throw new InvalidOperationException( $"Timeout loading {scriptNameOrAnonymous} script from file {filePath}." );
    }
#endif
}
