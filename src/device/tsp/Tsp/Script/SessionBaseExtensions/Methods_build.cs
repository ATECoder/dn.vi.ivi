using System.Text;
using cc.isr.VI.Tsp.Script.ExportExtensions;
using cc.isr.VI.Tsp.Script.ScriptInfoExtensions;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   A <see cref="string"/> extension method that trims a script file. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="inputFilePath">    Full path name of the input file to act on. </param>
    /// <param name="outputFilePath">   Full pathname of the output file. </param>
    /// <param name="retainOutline">    True to retain outline. </param>
    public static void TrimScript( this string inputFilePath, string outputFilePath, bool retainOutline )
    {
        if ( inputFilePath is null || string.IsNullOrWhiteSpace( inputFilePath ) ) throw new ArgumentNullException( nameof( inputFilePath ) );
        if ( outputFilePath is null || string.IsNullOrWhiteSpace( outputFilePath ) ) throw new ArgumentNullException( nameof( outputFilePath ) );
        cc.isr.VI.Syntax.Tsp.TspScriptParser.TrimTspSourceCode( inputFilePath, outputFilePath, retainOutline );
    }

    /// <summary>
    /// A <see cref="ScriptInfo"/> extension method that builds (trims, compresses and encrypts) to files.
    /// </summary>
    /// <remarks>   2025-04-16. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="scriptInfo">       Information describing the script. </param>
    /// <param name="inputFolder">      Full pathname of the input folder where the untrimmed script
    ///                                 resides and the trimmed file will reside. </param>
    /// <param name="outputFolder">     Full pathname of the output folder where the compressed and
    ///                                 encrypted script will reside. </param>
    /// <param name="scriptFileFormat"> The script file format. </param>
    /// <param name="overWrite">        (Optional) True to over write. </param>
    /// <param name="validate">         (Optional) True to validate. </param>
    public static void BuildScript( this ScriptInfo scriptInfo, string inputFolder, string outputFolder,
        ScriptFormats scriptFileFormat, bool overWrite = true, bool validate = true )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        string inputFilePath = Path.Combine( inputFolder, scriptInfo.BuiltFileName );
        if ( string.IsNullOrWhiteSpace( outputFolder ) )
            outputFolder = inputFolder;
        string outFilePath = Path.Combine( outputFolder, scriptInfo.TrimmedFileName );
        if ( System.IO.File.Exists( inputFilePath ) )
        {
            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\tTrimming script file '{inputFilePath}'\r\n\t\tto '{outFilePath}'" );
            inputFilePath.TrimScript( outFilePath, true );

            inputFilePath = outFilePath;

            outFilePath = Path.Combine( outputFolder, scriptInfo.Title + ScriptInfo.SelectScriptFileExtension( scriptFileFormat ) );
            if ( scriptFileFormat.HasFlag( ScriptFormats.Compressed ) || scriptFileFormat.HasFlag( ScriptFormats.Encrypted ) )
            {
                string action = scriptFileFormat.HasFlag( ScriptFormats.Encrypted ) ? "Compressing and encrypting" : "Compressing";
                SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{action} '{inputFilePath}'\r\n\t\tto '{outFilePath}'" );
                inputFilePath.CompressScriptFile( outFilePath, scriptFileFormat, scriptInfo.Compressor, scriptInfo.Encryptor, overWrite, validate );
            }
        }
        else
            throw new FileNotFoundException( inputFilePath );
    }

    /// <summary>
    /// A <see cref="cc.isr.VI.Tsp.Script.ScriptInfo"/> extension method that builds the script. The
    /// script is trimmed and compressed and encrypted based on the <see cref="ScriptInfo"/>.<see cref="ScriptInfo.DeployFileFormat"/>
    /// unless the format is <see cref="ScriptFormats.ByteCode"/> in which case the script is
    /// compressed and encrypted after it is converted to byte code (compiled) by the
    ///  <see cref="CompileScript(Pith.SessionBase, ScriptInfo, string, string, bool)"/> method.
    /// </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="scriptInfo">   Information describing the script. </param>
    /// <param name="buildFolder">  Pathname of the build folder. </param>
    /// <param name="deployFolder"> Pathname of the deploy folder. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    public static void BuildScript( this ScriptInfo scriptInfo, string buildFolder, string deployFolder, bool consoleOut = false )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        string builtFilePath = Path.Combine( buildFolder, scriptInfo.BuiltFileName );
        string trimmedFilePath = Path.Combine( buildFolder, scriptInfo.TrimmedFileName );
        string compressedFilePath = Path.Combine( deployFolder, $"{scriptInfo.Title}{ScriptInfo.ScriptCompressedFileExtension}" );
        string deployFilePath = Path.Combine( deployFolder, scriptInfo.DeployFileName );
        string message;
        if ( System.IO.File.Exists( builtFilePath ) )
        {
            message = $"Trimming script file\r\n\t\tfrom '{builtFilePath}'\r\n\t\tto '{trimmedFilePath}'";
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );
            builtFilePath.TrimScript( trimmedFilePath, true );

            if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFormats.Compressed )
                && !scriptInfo.DeployFileFormat.HasFlag( ScriptFormats.ByteCode ) )
            {
                string filePath = scriptInfo.DeployFileFormat.HasFlag( ScriptFormats.Encrypted )
                    ? compressedFilePath
                    : deployFilePath;

                message = $"Compressing '{trimmedFilePath}'\r\n\t\tto '{filePath}'";
                if ( consoleOut )
                    SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                else
                    SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );

                System.IO.File.WriteAllText( filePath, scriptInfo.Compressor.CompressToBase64( System.IO.File.ReadAllText( trimmedFilePath ) ), System.Text.Encoding.Default );

                if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFormats.Encrypted ) )
                {
                    message = $"encrypting '{filePath}'\r\n\t\tto '{deployFilePath}'";
                    if ( consoleOut )
                        SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                    else
                        SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );
                    System.IO.File.WriteAllText( deployFilePath, scriptInfo.Encryptor.EncryptToBase64( System.IO.File.ReadAllText( filePath ) ), System.Text.Encoding.Default );
                }
            }
        }
        else
            throw new FileNotFoundException( builtFilePath );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that compiles and exports a loaded user script 
    /// based on the <see cref="ScriptInfo"/>.<see cref="ScriptInfo.DeployFileFormat"/> if <see cref="ScriptFormats.ByteCode"/>.
    /// </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptInfo">   Information describing the script. </param>
    /// <param name="deployFolder"> Pathname of the deploy folder. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    public static void CompileLoadedScript( this Pith.SessionBase session, ScriptInfo scriptInfo, string deployFolder, bool consoleOut = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        if ( !scriptInfo.DeployFileFormat.HasFlag( ScriptFormats.ByteCode ) )
            throw new InvalidOperationException( $"{nameof( scriptInfo )}.{nameof( scriptInfo.DeployFileFormat )} does include {nameof( ScriptFormats.ByteCode )}." );

        string deployFilePath = Path.Combine( deployFolder, scriptInfo.DeployFileName );
        string message;

        // convert the script to byte code.
        if ( !session.IsByteCodeScript( scriptInfo.Title ) )
        {
            message = $"converting to byte code '{scriptInfo.Title}'";
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );
            session.ConvertToByteCode( scriptInfo.Title );
        }

        // run the script to ensure the code works.
        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        // fetch the script to ensure it is not empty.
        string scriptSource = session.FetchScript( scriptInfo.Title );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"The script {scriptInfo.Title} cannot be exported because it is empty." );

        if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFormats.Compressed ) )
        {
            message = $"compressing '{scriptInfo.Title}'";
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );
            scriptSource = scriptInfo.Compressor.CompressToBase64( scriptSource );
        }

        if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFormats.Encrypted ) )
        {
            message = $"encrypting '{scriptInfo.Title}'";
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );
            scriptSource = scriptInfo.Encryptor.EncryptToBase64( scriptSource );
        }

        message = $"exporting '{scriptInfo.Title}' to '{deployFilePath}'";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );

        // write the source to file.
        System.IO.File.WriteAllText( deployFilePath, scriptSource, System.Text.Encoding.Default );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that trims, compressed and encrypts 
    /// the script based on the <see cref="ScriptInfo"/>.<see cref="ScriptInfo.DeployFileFormat"/>,
    /// loads and runs the script and, optionally, exports the script to compressed and encrypted
    /// byte code.
    /// </summary>
    /// <remarks>   2025-04-15. <para>
    /// This loads and runs the script leaving it in volatile memory. </para></remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptInfo">   Information describing the script. </param>
    /// <param name="buildFolder">  Pathname of the build folder. </param>
    /// <param name="deployFolder"> Pathname of the deploy folder. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    public static void CompileScript( this Pith.SessionBase session, ScriptInfo scriptInfo, string buildFolder, string deployFolder, bool consoleOut = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        // trim and optionally compress and encrypt the script to the deploy file name.
        scriptInfo.BuildScript( buildFolder, deployFolder, consoleOut );

        // delete the script if it exists.
        session.DeleteScript( scriptInfo.Title );

        string trimmedFilePath = Path.Combine( buildFolder, scriptInfo.TrimmedFileName );
        string message = $"Importing script from trimmed '{trimmedFilePath}' file";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );

        session.ImportScript( scriptInfo.Title, trimmedFilePath, TimeSpan.Zero );

        message = $"Running script '{scriptInfo.Title}'";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );

        // run the script to ensure the code works.
        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFormats.ByteCode ) )
            session.CompileLoadedScript( scriptInfo, deployFolder, consoleOut );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that trims, compressed and encrypts all
    /// framework scripts based on the <see cref="ScriptInfo"/>.<see cref="ScriptInfo.DeployFileFormat"/>,
    /// loads and runs each script and, optionally, exports the script to compressed and encrypted
    /// byte code.
    /// </summary>
    /// <remarks>
    /// 2025-04-22. <para>
    /// This loads and runs all scripts leaving the framework active in volatile memory. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="versionInfo">  The versionInfo to act on. </param>
    /// <param name="scripts">      The collection of scripts. </param>
    /// <param name="buildFolder">  Pathname of the build folder. </param>
    /// <param name="deployFolder"> Pathname of the deploy folder. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    public static void CompileScripts( this Pith.SessionBase session, VersionInfoBase versionInfo, ScriptInfoCollection scripts,
        string buildFolder, string deployFolder, bool consoleOut = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        session.LastNodeNumber = default;

        try
        {
            session.Display( $"Building TTM", "Building scripts..." );

            foreach ( ScriptInfo scriptInfo in scripts )
            {
                if ( scriptInfo is null ) continue;
                if ( scriptInfo.VersionGetter is null ) continue;

                // build the deploy file name based on the FrameworkInfo default file format for this script.
                _ = scriptInfo.BuildDeployFileName( versionInfo );

                session.DisplayLine( $"Building {scriptInfo.Title} ...", 2, 1 );
                session.CompileScript( scriptInfo, buildFolder, deployFolder, consoleOut );
            }
        }
        catch ( Exception )
        {
            throw;
        }
        finally
        {
            session.RestoreMainDisplay();
        }
    }

    /// <summary>   A helper method that returns a script for time display clear script. </summary>
    /// <remarks>   2025-05-13. </remarks>
    /// <returns>   A Tuple: script name, script source. script function name. </returns>
    public static (string scriptName, string scriptSource, string scriptFunctionName) BuildTimeDisplayClearScript()
    {
        string scriptName = "timeDisplayClear";
        string timeDisplayFunctionName = "timeDisplay";
        StringBuilder scriptSource = new();
        _ = scriptSource.AppendLine( $"{cc.isr.VI.Syntax.Tsp.Script.LoadScriptCommand} {scriptName}" );
        _ = scriptSource.AppendLine( "do" );
        _ = scriptSource.AppendLine( $"  {timeDisplayFunctionName}=function()" );
        _ = scriptSource.AppendLine( "    timer.reset()" );
        _ = scriptSource.AppendLine( "    display.clear()" );
        _ = scriptSource.AppendLine( $"    local elapsed = timer.measure.t()" );
        _ = scriptSource.AppendLine( "    timer.reset()" );
        _ = scriptSource.AppendLine( $"    _G.display.screen = _G.display.MAIN or 0 {Syntax.Tsp.Lua.WaitCommand}" );
        _ = scriptSource.AppendLine( "    return elapsed" );
        _ = scriptSource.AppendLine( "  end" );
        _ = scriptSource.AppendLine( "end" );
        _ = scriptSource.AppendLine( cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand );

        return (scriptName, scriptSource.ToString(), timeDisplayFunctionName);
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that loads time display clear script.
    /// </summary>
    /// <remarks>   2025-05-13. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">  The session. </param>
    /// <returns>   The time display clear script. </returns>
    public static (string scriptName, string scriptFunctionName) LoadTimeDisplayClearScript( this Pith.SessionBase session )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        (string scriptName, string scriptSource, string scriptFunctionName) = SessionBaseExtensionMethods.BuildTimeDisplayClearScript();
        if ( !session.IsNil( scriptName ) )
        {
            session.TraceLastAction( $"\r\n\tDeleting {scriptName} script" );
            session.DeleteScript( scriptName );
        }
        session.TraceLastAction( $"\r\n\tLoading {scriptName} script" );
        session.LoadScript( scriptName, scriptSource, TimeSpan.Zero );
        return (scriptName, scriptFunctionName);
    }
}
