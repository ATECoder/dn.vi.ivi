using System.Diagnostics;
using cc.isr.VI.Pith;
using cc.isr.VI.Syntax.Tsp;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions.Legacy;

/// <summary>   A user script manager legacy. </summary>
/// <remarks>   2024-09-09. </remarks>
internal static class FirmwareManager
{

    #region " load string - no longer used "

    /// <summary>
    /// Load the code. Code could be embedded as a comma separated string table format, in which case
    /// the script should be concatenated first.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="sourceCode">   Includes the source code of the script as a long string. </param>
    [Obsolete( "replaced by Session Base Write Lines methods and Load Script for anonymous script." )]
    public static void LoadString( this Pith.SessionBase? session, string sourceCode )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( sourceCode is null ) throw new ArgumentNullException( nameof( sourceCode ) );
        session.WriteLines( sourceCode, Environment.NewLine, FirmwareScriptBase.WriteLinesDelay );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
    }

    /// <summary>
    /// Load the code. Code could be embedded as a comma separated string table format, in which case
    /// the script should be concatenated first.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptLines">  Includes the script in lines. </param>
    [Obsolete( "replaced by Session Base Write Lines methods and Load Script for anonymous script." )]
    public static void LoadString( this Pith.SessionBase? session, string[] scriptLines )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptLines is null ) throw new ArgumentNullException( nameof( scriptLines ) );
        session.WriteLines( scriptLines, session.ReadAfterWriteDelay );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
#if false
        foreach ( string line in scriptLines )
        {
            string scriptLine = line.Trim();
            if ( !string.IsNullOrWhiteSpace( scriptLine ) )
            {
                _ = session.WriteLine( scriptLine );
                _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay );
            }
        }

        // wait a bit and then check for device errors.
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError();

        // query and throw operation complete if failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        // wait a bit and then check for device errors.
        session.ThrowDeviceExceptionIfError();
#endif
    }

    #endregion

    #region " load script file - no longer used "

    /// <summary>
    /// Loads a named script into the instrument allowing control over how errors and prompts are
    /// handled. For loading a script that does not includes functions, turn off errors and turn on
    /// the prompt.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="IOException">              Thrown when an IO failure occurred. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptName">       Specifies the script name. </param>
    /// <param name="filePath">         Specifies the script file scriptName. </param>
    /// <param name="retainOutline">    Specifies if the code outline is retained or trimmed. </param>
    public static void LoadScriptFileLegacy( SessionBase? session, string scriptName, string filePath, bool retainOutline )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( filePath is null || string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        string chunkLine;
        string commandLine;
        try
        {
            using ( StreamReader? tspFile = FirmwareFileInfo.OpenScriptFile( filePath ) )
            {
                if ( tspFile is null )
                    // now report the error to the calling module
                    throw new IOException( "Failed opening TSP Script File '" + filePath + "'." );

                string trimmedFileSuffix = ".trimmed.tsp";
                string trimmedFilePath = filePath.Replace( ".tsp", trimmedFileSuffix );
                if ( string.Equals( trimmedFilePath, filePath ) )
                    trimmedFilePath = filePath + trimmedFileSuffix;

                using StreamWriter debugFile = new( trimmedFilePath );
                bool isInCommentBlock;
                isInCommentBlock = false;
                int lineNumber;
                lineNumber = 0;
                bool isFirstLine;
                isFirstLine = true;
                bool wasInCommentBlock;
                LuaChunkLineContentType lineType;
                while ( !tspFile.EndOfStream )
                {
                    string line = tspFile.ReadLine();
                    chunkLine = Lua.ReplaceTabs( line );
                    chunkLine = retainOutline ? chunkLine.TrimEnd() : chunkLine.Trim();

                    lineNumber += 1;
                    wasInCommentBlock = isInCommentBlock;
                    lineType = Lua.ParseLuaChuckLine( chunkLine, isInCommentBlock );
                    if ( lineType == LuaChunkLineContentType.None )
                    {
                        // if no data, nothing to do.

                    }
                    else if ( wasInCommentBlock )
                    {
                        // if was in a comment block exit the comment block if
                        // received a end of comment block
                        if ( lineType == LuaChunkLineContentType.EndCommentBlock )
                            isInCommentBlock = false;
                    }
                    else if ( lineType == LuaChunkLineContentType.StartCommentBlock )
                        isInCommentBlock = true;
                    else if ( lineType == LuaChunkLineContentType.Comment )
                    {
                        // if comment line do nothing

                    }
                    else if ( lineType is LuaChunkLineContentType.Syntax or LuaChunkLineContentType.SyntaxStartCommentBlock )
                    {
                        if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                            chunkLine = chunkLine[..chunkLine.IndexOf( Lua.StartCommentChunk, StringComparison.OrdinalIgnoreCase )];

                        // end each line with a space
                        chunkLine += " ";
                        if ( isFirstLine )
                        {
                            // issue a start of script command.  The command
                            // The load script command identifies the beginning of the script.
                            commandLine = $"{Syntax.Tsp.Script.LoadScriptCommand} {scriptName} ";
                            _ = session.WriteLine( commandLine );
                            isFirstLine = false;
                            // this.StatusSubsystem.ThrowDeviceExceptionIfError( true, session.StatusReadDelay,
                            // "sending a load script command for script '{2}';. from file '{1}'", scriptName, filePath );
                        }

                        _ = session.WriteLine( chunkLine );

                        // increment debug line number
                        debugFile.WriteLine( chunkLine );
                        if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                            isInCommentBlock = true;
                    }
                }
            }

            // Tell TSP complete script has been downloaded.
            commandLine = $"{Syntax.Tsp.Script.EndScriptCommand} {cc.isr.VI.Syntax.Tsp.Lua.OperationCompletedQueryCommand} ";
            session.SetLastAction( $"sending an '{Syntax.Tsp.Script.EndScriptCommand}' for script '{scriptName}'; from file '{filePath}'" );
            _ = session.WriteLine( commandLine );
            // wait till we get a reply from the instrument or completionTimeout.
            Stopwatch sw = Stopwatch.StartNew();
            TimeSpan timeout = TimeSpan.FromMilliseconds( 3000d );
            string value = string.Empty;
            do
            {
                do
                    _ = SessionBase.AsyncDelay( TimeSpan.FromMilliseconds( 50 ) );
                while ( !session.QueryMessageAvailableBit() && sw.Elapsed <= timeout );
                if ( session.QueryMessageAvailableBit() )
                    value = session.ReadLine();
            }
            while ( !value.StartsWith( cc.isr.VI.Syntax.ScpiSyntax.OperationCompletedValue, StringComparison.OrdinalIgnoreCase ) && sw.Elapsed <= timeout );
            if ( sw.Elapsed > timeout )
            {
                // Throw New cc.isr.Tsp.ScriptCallException("Timeout waiting operation completion loading the script '" & Me._name & "'")
            }

            // add a wait to ensure the system returns the last status.
            Thread.Sleep( 100 );

        }
        catch
        {
            throw;
        }
        finally
        {
        }
    }

    /// <summary>   Loads the specified TSP script from file. </summary>
    /// <remarks>   2024-07-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="displaySubsystem"> A reference to a
    ///                                 <see cref="DisplaySubsystemBase">display
    ///                                 subsystem</see>. </param>
    /// <param name="folderPath">       Specifies the script folder path. </param>
    /// <param name="script">           Specifies reference to a valid
    ///                                       <see cref="ScriptEntityBase">script</see> </param>
    public static void LoadUserScriptFile( DisplaySubsystemBase displaySubsystem, string? folderPath, ScriptEntityBase? script )
    {
        if ( displaySubsystem is null ) throw new ArgumentNullException( nameof( displaySubsystem ) );
        if ( folderPath is null ) throw new ArgumentNullException( nameof( folderPath ) );
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );

        SessionBase session = displaySubsystem.Session;
        session.LastNodeNumber = default;

        session.SetLastAction( $"checking if script '{script.Name}' is loaded" );
        if ( !session.IsNil( script.Name ) )
        {
            // script already exists
            _ = SessionLogger.Instance.LogVerbose( $"{session.ResourceNameNodeCaption} script '{script.Name}' already exists;. " );
            return;
        }

        string filePath = Path.Combine( folderPath, script.FirmwareScript.BuildFileName );
        // check if file exists.
        if ( FirmwareScriptBase.FileSize( filePath ) <= 2L )
            throw new FileNotFoundException( "Script file not found or is empty;. ", script.FirmwareScript.BuildFileName );

        displaySubsystem.DisplayLine( 2, $"Loading {script.Name} from file" );
        try
        {
            session.SetLastAction( $"loading script '{script.Name}' from {filePath}" );
            LoadScriptFileLegacy( session, script.Name, filePath, true );
        }
        catch
        {
            displaySubsystem.DisplayLine( 2, $"Failed loading {script.Name} from file" );
            throw;
        }

        if ( !session.CollectGarbageQueryComplete( script.Node.Number ) )
            _ = session.TraceWarning( message: $"garbage collection incomplete (reply not '1') after loading script {script.Name} from file {filePath}" );

        _ = session.TraceDeviceExceptionIfError( failureMessage: $"ignoring error after after Loading user script {script.Name} file on node {script.Node.Number}" );

        _ = SessionLogger.Instance.LogVerbose( $"{session.ResourceNameNodeCaption} {script.Name} script loaded;. " );

        displaySubsystem.DisplayLine( 2, $"Done loading {script.Name} from file" );
    }

    #endregion
}
