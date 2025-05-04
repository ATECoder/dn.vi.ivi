using System.Diagnostics;
using cc.isr.VI.Pith;
using cc.isr.VI.Syntax.Tsp;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class FirmwareManager
{

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

    /// <summary>   Loads the script embedded in the string. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Contains the script name. Could be empty if loading an anonymous script. </param>
    /// <param name="source">       Contains the script code line by line. </param>
    public static void LoadScript( this Pith.SessionBase? session, string scriptName, string source )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( string.IsNullOrWhiteSpace( source ) ) throw new ArgumentNullException( nameof( source ) );
        source = FirmwareScriptBase.BuildLoadStringSyntax( source );
        string[] scriptLines = source.Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
        session.LoadScript( scriptName, scriptLines );
    }

    /// <summary>   Loads the script embedded in the string array and issues a wait complete. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   Contains the script name. Could be empty if loading an anonymous script. </param>
    /// <param name="scriptLines">  Contains the script code line by line. </param>
    public static void LoadScript( this Pith.SessionBase? session, string scriptName, string[] scriptLines )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( scriptLines is null ) throw new ArgumentNullException( nameof( scriptLines ) );

        string firstLine = scriptLines[0];
        // check if we already have the load/end constructs.
        session.LastNodeNumber = default;
        // was if ( firstLine.Contains( scriptName ) || firstLine.Contains( Syntax.Tsp.Script.LoadScriptCommand ) )
        if ( (string.IsNullOrWhiteSpace( scriptName ) && firstLine.Contains( Syntax.Tsp.Script.LoadAndRunScriptCommand )) ||
              firstLine.Contains( Syntax.Tsp.Script.LoadAndRunScriptCommand ) )
        {
            session.SetLastAction( $"loading {(string.IsNullOrWhiteSpace( scriptName ) ? """anonymous""" : scriptName)} script" );
            session.LastNodeNumber = default;
            session.WriteLines( scriptLines, FirmwareScriptBase.WriteLinesDelay );
        }
        else
        {
            session.SetLastAction( $"initiating load for {(string.IsNullOrWhiteSpace( scriptName ) ? """anonymous""" : scriptName)} script" );
            if ( string.IsNullOrEmpty( scriptName ) )
                _ = session.WriteLine( $"{Syntax.Tsp.Script.LoadAndRunScriptCommand}" );
            else
                _ = session.WriteLine( $"{Syntax.Tsp.Script.LoadScriptCommand} {scriptName}" );

            session.SetLastAction( $"loading lines for {(string.IsNullOrWhiteSpace( scriptName ) ? """anonymous""" : scriptName)} script" );
            session.WriteLines( scriptLines, FirmwareScriptBase.WriteLinesDelay );

            session.SetLastAction( $"ending  for {(string.IsNullOrWhiteSpace( scriptName ) ? """anonymous""" : scriptName)} script" );
            _ = session.WriteLine( Syntax.Tsp.Script.EndScriptCommand );
            _ = session.WriteLine( cc.isr.VI.Syntax.Tsp.Lua.WaitCommand );
        }

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   Loads an anonymous script embedded in the string. </summary>
    /// <remarks>   2025-04-02. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptLines">  Contains the script code line by line. </param>
    public static void LoadScript( this Pith.SessionBase? session, string[] scriptLines )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptLines is null ) throw new ArgumentNullException( nameof( scriptLines ) );

        string firstLine = scriptLines[0];
        // check if we already have the load/end constructs.
        session.LastNodeNumber = default;
        if ( firstLine.Contains( Syntax.Tsp.Script.LoadAndRunScriptCommand ) )
        {
            session.SetLastAction( $"loading anonymous script" );
            session.LastNodeNumber = default;
            session.WriteLines( scriptLines, FirmwareScriptBase.WriteLinesDelay );
        }
        else if ( firstLine.Contains( Syntax.Tsp.Script.LoadScriptCommand ) )
            throw new InvalidOperationException( $"The '{firstLine}' first line must be {Syntax.Tsp.Script.LoadAndRunScriptCommand}." );
        else
        {
            session.SetLastAction( "initiating load script for anonymous script" );
            _ = session.WriteLine( Syntax.Tsp.Script.LoadAndRunScriptCommand );

            session.SetLastAction( "loading script lines for anonymous script" );
            session.WriteLines( scriptLines, FirmwareScriptBase.WriteLinesDelay );

            session.SetLastAction( "ending script for anonymous script" );
            _ = session.WriteLine( Syntax.Tsp.Script.EndScriptCommand );
            _ = session.WriteLine( cc.isr.VI.Syntax.Tsp.Lua.WaitCommand );
        }

        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError();

        // query and throw if operation complete query failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        session.ThrowDeviceExceptionIfError();
    }

    /// <summary>   Loads an anonymous script embedded in the string. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <param name="source">   Contains the script code line by line. </param>
    public static void LoadScript( this Pith.SessionBase? session, string source )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( string.IsNullOrWhiteSpace( source ) ) throw new ArgumentNullException( nameof( source ) );

        source = FirmwareScriptBase.BuildLoadStringSyntax( source );
        session.WriteLines( source, Environment.NewLine, FirmwareScriptBase.WriteLinesDelay );

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

    /// <summary>
    /// A Pith.SessionBase extension method that loads script file wait complete.
    /// Loads a named script into the instrument. This loads the script from the file one line at a
    /// time while saving the script to the 'debug' file.
    /// </summary>
    /// <remarks>   2024-09-20. </remarks>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptName">       Name of the script. </param>
    /// <param name="resourceFilePath"> full path name of the resource file. </param>
    /// <param name="retainOutline">    Specifies if the code outline is retained or trimmed. </param>
    /// <param name="loadTimeout">      The load timeout. </param>
    /// <returns>   The script file wait complete. </returns>
    public static (bool TimedOut, ServiceRequests Status, TimeSpan Elapsed) LoadScriptFileWaitComplete( this Pith.SessionBase session, string scriptName,
        string resourceFilePath, bool retainOutline, TimeSpan loadTimeout )
    {
        if ( session is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( session )} is null." );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( resourceFilePath is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( resourceFilePath ) );

        session.LastNodeNumber = default;

        string chunkLine;
        bool isInCommentBlock;
        isInCommentBlock = false;
        int lineNumber;
        lineNumber = 0;

        bool isFirstLine;
        isFirstLine = true;
        string commandLine;
        bool wasInCommentBlock;
        LuaChunkLineContentType lineType;
        string activity;
        string actionDetails;

        activity = "enabling wait completion";
        session.SetLastAction( activity );
        session.EnableServiceRequestOnOperationCompletion();
        session.ThrowDeviceExceptionIfError();

        using ( StreamReader? tspFile = FirmwareFileInfo.OpenScriptFile( resourceFilePath ) )
        {
            if ( tspFile is null )
                throw new System.IO.FileNotFoundException( "Failed opening TSP Script file", resourceFilePath );

            string trimmedFileSuffix = $".trimmed{FirmwareScriptBase.ScriptFileExtension}";
            string trimmedFilePath = resourceFilePath.Replace( FirmwareScriptBase.ScriptFileExtension, trimmedFileSuffix );
            if ( string.Equals( trimmedFilePath, resourceFilePath ) )
                trimmedFilePath = resourceFilePath + trimmedFileSuffix;

            using StreamWriter trimmedFile = new( trimmedFilePath );
            while ( !tspFile.EndOfStream )
            {
                string line = tspFile.ReadLine();
                chunkLine = Lua.ReplaceTabs( line );
                chunkLine = retainOutline ? chunkLine.TrimEnd() : chunkLine.Trim();

                lineNumber += 1;
                wasInCommentBlock = isInCommentBlock;
                lineType = Lua.ParseLuaChunkLine( chunkLine, isInCommentBlock );
                if ( lineType == LuaChunkLineContentType.None )
                {
                    // if no data, nothing to do.
                }

                else if ( wasInCommentBlock )
                {
                    // if was in a comment block exit the comment block if
                    // received a end of comment block
                    if ( lineType == LuaChunkLineContentType.EndCommentBlock )
                    {
                        isInCommentBlock = false;
                    }
                }
                else if ( lineType == LuaChunkLineContentType.StartCommentBlock )
                {
                    isInCommentBlock = true;
                }
                else if ( lineType == LuaChunkLineContentType.Comment )
                {
                    // if comment line do nothing
                }
                else if ( lineType is LuaChunkLineContentType.Syntax or LuaChunkLineContentType.SyntaxStartCommentBlock )
                {
                    if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                    {
                        chunkLine = chunkLine[..chunkLine.IndexOf( Syntax.Tsp.Lua.StartCommentBlockChunk, StringComparison.OrdinalIgnoreCase )];
                    }

                    // end each line with a space
                    chunkLine += " ";
                    if ( isFirstLine )
                    {
                        // issue a start of script command. The load script command identifies the beginning of the script.
                        commandLine = $"{Syntax.Tsp.Script.LoadScriptCommand} {scriptName} ";
                        activity = $"sending {commandLine} for script '{scriptName}' from file '{resourceFilePath}'";
                        session.SetLastAction( activity );
                        session.SetLastActionDetails( activity );
                        _ = session.WriteLine( commandLine );

                        isFirstLine = false;
                    }
                    // TSP received the first script line successfully; waiting for next line.

                    activity = $"sending syntax line #{lineNumber} for script '{scriptName}' from file '{resourceFilePath}'";
                    actionDetails = $"{activity}:\n\t{chunkLine}";
                    session.SetLastAction( activity );
                    session.SetLastActionDetails( actionDetails );

                    _ = session.WriteLine( chunkLine );

                    // increment debug line number
                    trimmedFile.WriteLine( chunkLine );

                    if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                        isInCommentBlock = true;
                }
            }
        }


        // Tell TSP complete script has been downloaded.
        commandLine = $"{Syntax.Tsp.Script.EndScriptCommand} {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} ";
        activity = $"sending {commandLine} for script '{scriptName}' from file '{resourceFilePath}'";
        session.SetLastAction( activity );
        session.SetLastActionDetails( activity );

        _ = session.WriteLine( commandLine );

        session.ThrowDeviceExceptionIfError();

        // await completion
        return session.AwaitOperationCompletion( loadTimeout );
    }

    /// <summary>
    /// Read script from file, parse each line, load to the instrument and output the lines to the
    /// 'debug' file.
    /// </summary>
    /// <remarks>   2024-09-11. This method uses the status byte to monitor errors. </remarks>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ArgumentException">        Thrown when one or more arguments have
    ///                                             unsupported or illegal values. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptName">       Name of the script. </param>
    /// <param name="scriptFilePath">   full path name of the script file. </param>
    /// <param name="trimmedFilePath">  full path name of the trimmed file. </param>
    /// <param name="retainOutline">    Specifies if the code outline is retained or trimmed. </param>
    /// <returns>   The script file. </returns>
    public static (bool TimedOut, ServiceRequests Status, TimeSpan Elapsed) ParseAndLoadScriptFromFile( this Pith.SessionBase session, string scriptName,
        string scriptFilePath, string trimmedFilePath, bool retainOutline )
    {
        if ( session is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( session )} is null." );
        if ( scriptName is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( scriptFilePath is null || string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptFilePath ) );
        if ( string.Equals( trimmedFilePath, scriptFilePath ) )
            throw new ArgumentException( $"{scriptFilePath} and {trimmedFilePath} should not point tot he same file." );

        session.LastNodeNumber = default;

        string chunkLine;
        bool isInCommentBlock;
        isInCommentBlock = false;
        int lineNumber;
        lineNumber = 0;

        bool isFirstLine;
        isFirstLine = true;
        string commandLine;
        bool wasInCommentBlock;
        LuaChunkLineContentType lineType;
        string activity;
        string actionDetails;

        Stopwatch sw = Stopwatch.StartNew();


        using ( StreamReader? tspFile = FirmwareFileInfo.OpenScriptFile( scriptFilePath ) )
        {
            if ( tspFile is null )
                throw new System.IO.FileNotFoundException( "Failed opening TSP Script file", scriptFilePath );

            using StreamWriter trimmedFile = new( trimmedFilePath );
            while ( !tspFile.EndOfStream )
            {
                string line = tspFile.ReadLine();
                chunkLine = Lua.ReplaceTabs( line );
                chunkLine = retainOutline ? chunkLine.TrimEnd() : chunkLine.Trim();

                lineNumber += 1;
                wasInCommentBlock = isInCommentBlock;
                lineType = Lua.ParseLuaChunkLine( chunkLine, isInCommentBlock );
                if ( lineType == LuaChunkLineContentType.None )
                {
                    // if no data, nothing to do.
                }

                else if ( wasInCommentBlock )
                {
                    // if was in a comment block exit the comment block if
                    // received a end of comment block
                    if ( lineType == LuaChunkLineContentType.EndCommentBlock )
                    {
                        isInCommentBlock = false;
                    }
                }
                else if ( lineType == LuaChunkLineContentType.StartCommentBlock )
                {
                    isInCommentBlock = true;
                }
                else if ( lineType == LuaChunkLineContentType.Comment )
                {
                    // if comment line do nothing
                }
                else if ( lineType is LuaChunkLineContentType.Syntax or LuaChunkLineContentType.SyntaxStartCommentBlock )
                {
                    if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                    {
                        chunkLine = chunkLine[..chunkLine.IndexOf( Syntax.Tsp.Lua.StartCommentBlockChunk, StringComparison.OrdinalIgnoreCase )];
                    }

                    // end each line with a space
                    chunkLine += " ";
                    if ( isFirstLine )
                    {
                        // issue a start of script command. The load script command identifies the beginning of the named script.
                        commandLine = $"{Syntax.Tsp.Script.LoadScriptCommand} {scriptName} ";
                        activity = $"sending {commandLine} for script '{scriptName}' from file '{scriptFilePath}'";
                        session.SetLastAction( activity );
                        session.SetLastActionDetails( activity );
                        _ = session.WriteLine( commandLine );

                        isFirstLine = false;
                    }
                    // TSP received the first script line successfully; waiting for next line.

                    activity = $"sending syntax line #{lineNumber} for script '{scriptName}' from file '{scriptFilePath}'";
                    actionDetails = $"{activity}:\n\t{chunkLine}";
                    session.SetLastAction( activity );
                    session.SetLastActionDetails( actionDetails );

                    _ = session.WriteLine( chunkLine );

                    // increment debug line number
                    trimmedFile.WriteLine( chunkLine );

                    if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                        isInCommentBlock = true;
                }
            }
        }

        // complete the script.
        commandLine = $"{Syntax.Tsp.Script.EndScriptCommand} {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} ";
        activity = $"sending {commandLine} for script '{scriptName}'";
        session.SetLastAction( activity );
        session.SetLastActionDetails( activity );

        _ = session.WriteLine( commandLine );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError();

        // query and throw operation complete if failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

#if false
        // output the operation completion result of '1'
        commandLine = cc.isr.VI.Syntax.Tsp.Lua.OperationCompleteQueryCommand;
        _ = session.WriteLine( commandLine );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

        session.ReadAndThrowIfOperationIncomplete();
#endif
        session.ThrowDeviceExceptionIfError();

        return (false, session.ReadStatusByte(), sw.Elapsed);
    }

    /// <summary>   A Pith.SessionBase extension method that loads script source. </summary>
    /// <remarks>
    /// 2024-10-11. If the script source is binary, the script is loaded using
    /// <see cref="LoadScript(SessionBase?, string, string[])"/> otherwise, the script is loaded using
    /// <see cref="ParseAndLoadScriptSource"/>
    /// </remarks>
    /// <param name="session">      The session. </param>
    /// <param name="scriptName">   The script name; could be empty if loading an anonymous script. </param>
    /// <param name="scriptSource"> The script source. </param>
    /// <returns>   The script source. </returns>
    public static (bool TimedOut, ServiceRequests Status, TimeSpan Elapsed) LoadScriptSource( this Pith.SessionBase session, string scriptName,
        string scriptSource )
    {
        if ( scriptSource.IsByteCodeScript() )
        {
            Stopwatch sw = Stopwatch.StartNew();
            session.LoadScript( scriptName, scriptSource );
            return (false, session.ReadStatusByte(), sw.Elapsed);
        }
        else
            return session.ParseAndLoadScriptSource( scriptName, scriptSource );
    }

    /// <summary>   Parse each line fo the script source and load into the instrument. </summary>
    /// <remarks>   2024-09-11. This method uses the status byte to monitor errors. </remarks>
    /// <exception cref="NativeException">          Thrown when a Native error condition occurs. </exception>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptName">       The script name; could be empty if loading an anonymous script. </param>
    /// <param name="scriptSource">     The script source. </param>
    /// <param name="retainOutline">    (Optional) (false) Specifies if the code outline is retained or
    ///                                 trimmed. </param>
    /// <returns>   The script source. </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0306:Simplify collection initialization", Justification = "<Pending>" )]
    public static (bool TimedOut, ServiceRequests Status, TimeSpan Elapsed) ParseAndLoadScriptSource( this Pith.SessionBase session, string scriptName,
        string scriptSource, bool retainOutline = false )
    {
        if ( session is null ) throw new cc.isr.VI.Pith.NativeException( $"{nameof( session )} is null." );
        if ( scriptSource is null || string.IsNullOrWhiteSpace( scriptSource ) ) throw new ArgumentNullException( nameof( scriptSource ) );

        session.LastNodeNumber = default;

        Queue<string> chunkLines = new( scriptSource.Split( Environment.NewLine.ToCharArray() ) );
        string chunkLine;
        bool isInCommentBlock;
        isInCommentBlock = false;
        int lineNumber;
        lineNumber = 0;

        bool isFirstLine;
        isFirstLine = true;
        string commandLine;
        bool wasInCommentBlock;
        LuaChunkLineContentType lineType;
        string activity;
        string actionDetails;

        string scriptNameOrAnonymous = string.IsNullOrWhiteSpace( scriptName ) ? "anonymous" : scriptName;

        Stopwatch sw = Stopwatch.StartNew();

        while ( chunkLines.Count > 0 )
        {
            string line = chunkLines.Dequeue();
            chunkLine = Lua.ReplaceTabs( line );
            chunkLine = retainOutline ? chunkLine.TrimEnd() : chunkLine.Trim();
            lineNumber += 1;
            wasInCommentBlock = isInCommentBlock;
            lineType = Lua.ParseLuaChunkLine( chunkLine, isInCommentBlock );
            if ( lineType == LuaChunkLineContentType.None )
            {
                // if no data, nothing to do.
            }

            else if ( wasInCommentBlock )
            {
                // if was in a comment block exit the comment block if
                // received a end of comment block
                if ( lineType == LuaChunkLineContentType.EndCommentBlock )
                {
                    isInCommentBlock = false;
                }
            }
            else if ( lineType == LuaChunkLineContentType.StartCommentBlock )
            {
                isInCommentBlock = true;
            }
            else if ( lineType == LuaChunkLineContentType.Comment )
            {
                // if comment line do nothing
            }
            else if ( lineType is LuaChunkLineContentType.Syntax or LuaChunkLineContentType.SyntaxStartCommentBlock )
            {
                if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                {
                    chunkLine = chunkLine[..chunkLine.IndexOf( Syntax.Tsp.Lua.StartCommentBlockChunk, StringComparison.OrdinalIgnoreCase )];
                }

                // end each line with a space
                chunkLine += " ";
                if ( isFirstLine )
                {
                    // issue a start of script command. The load script command identifies the beginning of the named script.
                    if ( string.IsNullOrWhiteSpace( scriptName ) )
                        commandLine = $"{Syntax.Tsp.Script.LoadAndRunScriptCommand} ";
                    else
                        commandLine = $"{Syntax.Tsp.Script.LoadScriptCommand} {scriptName} ";
                    activity = $"sending {commandLine} for '{scriptNameOrAnonymous}' script";
                    session.SetLastAction( activity );
                    session.SetLastActionDetails( activity );
                    _ = session.WriteLine( commandLine );

                    isFirstLine = false;
                }
                // TSP received the first script line successfully; waiting for next line.

                activity = $"sending syntax line #{lineNumber} for '{scriptNameOrAnonymous}' script";
                actionDetails = $"{activity}:\n\t{chunkLine}";
                session.SetLastAction( activity );
                session.SetLastActionDetails( actionDetails );

                _ = session.WriteLine( chunkLine );

                if ( lineType == LuaChunkLineContentType.SyntaxStartCommentBlock )
                    isInCommentBlock = true;
            }
        }
        // complete the script.
        commandLine = $"{Syntax.Tsp.Script.EndScriptCommand} {cc.isr.VI.Syntax.Tsp.Lua.WaitCommand} ";
        activity = $"sending {commandLine} for '{scriptNameOrAnonymous}' script";
        session.SetLastAction( activity );
        session.SetLastActionDetails( activity );

        _ = session.WriteLine( commandLine );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ThrowDeviceExceptionIfError();

        // query and throw operation complete if failed
        session.QueryAndThrowIfOperationIncomplete();
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );

#if false
        // output the operation completion result of '1'
        commandLine = cc.isr.VI.Syntax.Tsp.Lua.OpertationCompleteQueryCommand;
        _ = session.WriteLine( commandLine );
        _ = SessionBase.AsyncDelay( session.ReadAfterWriteDelay + session.StatusReadDelay );
        session.ReadAndThrowIfOperationIncomplete();
#endif
        session.ThrowDeviceExceptionIfError();

        return (false, session.ReadStatusByte(), sw.Elapsed);
    }
}
