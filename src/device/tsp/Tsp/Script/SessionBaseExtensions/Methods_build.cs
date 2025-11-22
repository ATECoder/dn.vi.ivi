using System.ComponentModel.DataAnnotations;
using System.Text;
using cc.isr.Std;
using cc.isr.Std.CompressExtensions;
using cc.isr.Std.LineEndingExtensions;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseMethods
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

    /// <summary>   A <see cref="string"/> extension method that trims a script file. </summary>
    /// <remarks>   2025-10-03. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ValidationException">      Thrown when a Validation error condition occurs. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="scriptInfo">   Information describing the script. </param>
    /// <param name="inputFolder">  Full pathname of the input folder where the untrimmed script
    ///                             resides and the trimmed file will reside. </param>
    /// <param name="outputFolder"> Full pathname of the output folder where the compressed and
    ///                             encrypted script will reside. Set to the <paramref name="inputFolder"/>
    ///                             if null or empty. </param>
    /// <param name="overwrite">    (Optional) True to overwrite an exiting file. </param>
    public static void TrimScript( this ScriptInfo scriptInfo, string inputFolder, string outputFolder, bool overwrite = true )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );
        if ( string.IsNullOrWhiteSpace( inputFolder ) ) throw new ArgumentNullException( nameof( inputFolder ) ); ;

        if ( string.IsNullOrWhiteSpace( outputFolder ) ) outputFolder = inputFolder;
        string outFilePath = Path.Combine( outputFolder, scriptInfo.TrimmedFileName );

        if ( !overwrite && System.IO.File.Exists( outFilePath ) )
            throw new ValidationException( $"The file '{outFilePath}' already exists." );

        string inputFilePath = Path.Combine( inputFolder, scriptInfo.BuiltFileName );

        if ( System.IO.File.Exists( inputFilePath ) )
        {
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\tTrimming script file '{inputFilePath}'\r\n\t\tto '{outFilePath}'" );
            inputFilePath.TrimScript( outFilePath, true );
        }
        else
            throw new FileNotFoundException( inputFilePath );
    }

    /// <summary>
    /// A <see cref="ScriptInfo"/> extension method that builds (trims, compresses and encrypts) to
    /// files.
    /// </summary>
    /// <remarks>   2025-04-16. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="ValidationException">      Thrown when a Validation error condition occurs. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="scriptInfo">       Information describing the script. </param>
    /// <param name="inputFolder">      Full pathname of the input folder where the untrimmed script
    ///                                 resides and the trimmed file will reside. </param>
    /// <param name="outputFolder">     Full pathname of the output folder where the compressed and
    ///                                 encrypted script will reside. Set to the <paramref name="inputFolder"/>
    ///                                 if null or empty. </param>
    /// <param name="scriptFileFormat"> The script file format. </param>
    /// <param name="overwrite">        (Optional) True to over write an exiting file. </param>
    /// <param name="validate">         (Optional) True to validate. </param>
    public static void BuildScript( this ScriptInfo scriptInfo, string inputFolder, string outputFolder,
        FileFormats scriptFileFormat, bool overwrite = true, bool validate = true )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );
        if ( string.IsNullOrWhiteSpace( inputFolder ) ) throw new ArgumentNullException( nameof( inputFolder ) ); ;

        if ( string.IsNullOrWhiteSpace( outputFolder ) ) outputFolder = inputFolder;
        string outFilePath = Path.Combine( outputFolder, scriptInfo.TrimmedFileName );

        if ( !overwrite && System.IO.File.Exists( outFilePath ) )
            throw new ValidationException( $"The file '{outFilePath}' already exists." );

        string inputFilePath = Path.Combine( inputFolder, scriptInfo.BuiltFileName );

        if ( !overwrite && System.IO.File.Exists( outFilePath ) )
            throw new ValidationException( $"The file '{outFilePath}' already exists." );

        if ( System.IO.File.Exists( inputFilePath ) )
        {
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\tTrimming script file '{inputFilePath}'\r\n\t\tto '{outFilePath}'" );
            inputFilePath.TrimScript( outFilePath, true );

            inputFilePath = outFilePath;

            outFilePath = Path.Combine( outputFolder, scriptInfo.Title + ScriptInfo.SelectScriptFileExtension( scriptFileFormat ) );
            if ( scriptFileFormat.HasFlag( FileFormats.Compressed ) || scriptFileFormat.HasFlag( FileFormats.Encrypted ) )
            {
                string action = scriptFileFormat.HasFlag( FileFormats.Encrypted ) ? "Compressing and encrypting" : "Compressing";
                _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{action} '{inputFilePath}'\r\n\t\tto '{outFilePath}'" );
                inputFilePath.CompressToFile( outFilePath, scriptFileFormat, scriptInfo.Compressor, scriptInfo.Encryptor, overwrite, validate );
            }
        }
        else
            throw new FileNotFoundException( inputFilePath );
    }

    /// <summary>
    /// A <see cref="cc.isr.VI.Tsp.Script.ScriptInfo"/> extension method that builds the script. The
    /// script is trimmed and compressed and encrypted based on the <see cref="ScriptInfo"/>.<see cref="ScriptInfo.DeployResourceFileFormat"/>
    /// unless the format is <see cref="FileFormats.ByteCode"/> in which case the script is
    /// compressed and encrypted after it is converted to byte code (compiled) by the
    ///  <see cref="CompileScript(Pith.SessionBase, ScriptInfo, string, string, bool, bool)"/> method.
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
        string deployFilePath = Path.Combine( deployFolder, scriptInfo.DeployResourceFileName );
        string message;
        if ( System.IO.File.Exists( builtFilePath ) )
        {
            message = $"Trimming script file\r\n\t\tfrom '{builtFilePath}'\r\n\t\tto '{trimmedFilePath}'";
            if ( consoleOut )
                _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            else
                _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );
            builtFilePath.TrimScript( trimmedFilePath, true );

            if ( scriptInfo.DeployResourceFileFormat.HasFlag( FileFormats.Compressed )
                && !scriptInfo.DeployResourceFileFormat.HasFlag( FileFormats.ByteCode ) )
            {
                string filePath = scriptInfo.DeployResourceFileFormat.HasFlag( FileFormats.Encrypted )
                    ? compressedFilePath
                    : deployFilePath;

                message = $"Compressing '{trimmedFilePath}'\r\n\t\tto '{filePath}'";
                if ( consoleOut )
                    _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
                else
                    _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );

                System.IO.File.WriteAllText( filePath, scriptInfo.Compressor.CompressToBase64( System.IO.File.ReadAllText( trimmedFilePath ) ), System.Text.Encoding.Default );

                if ( scriptInfo.DeployResourceFileFormat.HasFlag( FileFormats.Encrypted ) )
                {
                    message = $"encrypting '{filePath}'\r\n\t\tto '{deployFilePath}'";
                    if ( consoleOut )
                        _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
                    else
                        _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );
                    System.IO.File.WriteAllText( deployFilePath, scriptInfo.Encryptor.EncryptToBase64( System.IO.File.ReadAllText( filePath ) ), System.Text.Encoding.Default );
                }
            }
        }
        else
            throw new FileNotFoundException( builtFilePath );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that exports a loaded script to a file
    /// based on the <see cref="ScriptInfo"/>.<see cref="ScriptInfo.FileName"/> and <see cref="ScriptInfo"/>.
    /// 
    /// <see cref="ScriptInfo.FileFormat"/>.
    /// </summary>
    /// <remarks>
    /// 2025-09-19. <para>
    /// This method prevents exporting byte code scripts for deployment because the script must be
    /// instrument independent whereas byte code is instrument dependent.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="scriptInfo">   Information describing the script. </param>
    /// <param name="exportFolder"> Pathname of the export folder. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    public static void ExportLoadedScript( this Pith.SessionBase session, ScriptInfo scriptInfo, string exportFolder, bool validate = true, bool consoleOut = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        FileFormats fileFormat = scriptInfo.FileFormat;
        if ( !fileFormat.HasFlag( FileFormats.ByteCode ) && session.IsByteCodeScript( scriptInfo.Title ) )
            _ = fileFormat != FileFormats.ByteCode;

        string exportFilePath = Path.Combine( exportFolder, scriptInfo.FileName );
        string message;

        // run the script to ensure the code works.
        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        // fetch the script to ensure it is not empty.
        string scriptSource = session.FetchScript( scriptInfo.Title );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"The script {scriptInfo.Title} cannot be exported because it is empty." );

        // terminate the contents with a single line ending.
        scriptSource = scriptSource.TrimMultipleLineEndings();

        // replace line endings with Windows new line validating with the LineEndingExtensions.ReplaceLineEnding method
        scriptSource = scriptSource.TerminateLines( validate );

        string compressed = scriptSource;
        if ( fileFormat.HasFlag( FileFormats.Compressed ) )
        {
            message = $"compressing '{scriptInfo.Title}'";
            if ( consoleOut )
                _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            else
                _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );
            compressed = scriptInfo.Compressor.CompressToBase64( compressed );
        }

        if ( fileFormat.HasFlag( FileFormats.Encrypted ) )
        {
            message = $"encrypting '{scriptInfo.Title}'";
            if ( consoleOut )
                _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            else
                _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );
            compressed = scriptInfo.Encryptor.EncryptToBase64( compressed );
        }

        if ( validate && (fileFormat.HasFlag( FileFormats.Compressed ) || fileFormat.HasFlag( FileFormats.Encrypted )) )
        {
            string decompressed = compressed;
            if ( fileFormat.HasFlag( FileFormats.Encrypted ) )
                decompressed = scriptInfo.Encryptor.DecryptFromBase64( decompressed );
            if ( fileFormat.HasFlag( FileFormats.Compressed ) )
                decompressed = scriptInfo.Compressor.DecompressFromBase64( decompressed );

            if ( scriptSource.Length != decompressed.Length )
                throw new InvalidOperationException( $"The compressed script source could not be validated; the de-compressed length {decompressed.Length} does not match the end-of-line trimmed source length {scriptSource.Length}." );

            if ( !scriptSource.Equals( decompressed, StringComparison.Ordinal ) )
                throw new InvalidOperationException( $"The compressed script source could not be validated; the de-compressed content does not match the original source." );
        }

        message = $"exporting '{scriptInfo.Title}' to '{exportFilePath}'";
        if ( consoleOut )
            _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        else
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );

        // write the source to file.
        System.IO.File.WriteAllText( exportFilePath, scriptSource, System.Text.Encoding.Default );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that trims, compressed and encrypts the
    /// script based on the <see cref="ScriptInfo"/>.<see cref="ScriptInfo.DeployResourceFileFormat"/>, loads
    /// and runs the script and, optionally, exports the script to compressed and encrypted byte code.
    /// </summary>
    /// <remarks>
    /// 2025-04-15. <para>
    /// This loads and runs the script leaving it in volatile memory. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scriptInfo">       Information describing the script. </param>
    /// <param name="buildFolder">      Pathname of the build folder. </param>
    /// <param name="deployFolder">     Pathname of the deploy folder. </param>
    /// <param name="deleteExisting">   True to delete the existing script prior to loading the new script. </param>
    /// <param name="consoleOut">       True to output to the console. </param>
    public static void CompileScript( this Pith.SessionBase session, ScriptInfo scriptInfo, string buildFolder, string deployFolder, bool deleteExisting, bool consoleOut )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        // trim and optionally compress and encrypt the script to the deploy file name.
        scriptInfo.BuildScript( buildFolder, deployFolder, consoleOut );

        string message;

        // delete the existing script.
        if ( deleteExisting && session.IsScriptExists( scriptInfo.Title, out string details ) )
        {
            message = $"Deleting '{scriptInfo.Title}';. {details}";
            if ( consoleOut )
                _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            else
                _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );
            session.DeleteScript( scriptInfo.Title, true );
        }

        string trimmedFilePath = Path.Combine( buildFolder, scriptInfo.TrimmedFileName );
        message = $"Importing script from trimmed '{trimmedFilePath}' file";
        if ( consoleOut )
            _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        else
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );

        session.ImportScript( scriptInfo.Title, trimmedFilePath, TimeSpan.Zero, false, false );

        message = $"Running script '{scriptInfo.Title}'";
        if ( consoleOut )
            _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        else
            _ = cc.isr.Std.TraceExtensions.TraceMethods.TraceMemberMessage( $"\r\n\t{message}" );

        // run the script to ensure the code works.
        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that trims, loads, runs and exports the framework scripts 
    /// based on the <see cref="ScriptInfo"/>.<see cref="ScriptInfo.DeployResourceFileFormat"/>.
    /// </summary>
    /// <remarks>
    /// 2025-04-22. <para>
    /// This loads and runs all scripts leaving the framework active in volatile memory. </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">          The session. </param>
    /// <param name="scripts">          The collection of scripts. </param>
    /// <param name="buildFolder">      Pathname of the build folder. </param>
    /// <param name="deployFolder">     Pathname of the deploy folder. </param>
    /// <param name="deleteExisting">   True to delete the existing script prior to building nad importing. </param>
    /// <param name="consoleOut">       True to console out. </param>
    public static void CompileScripts( this Pith.SessionBase session, ScriptInfoCollection scripts,
        string buildFolder, string deployFolder, bool deleteExisting, bool consoleOut )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );

        session.LastNodeNumber = default;

        try
        {
            session.Display( $"Compiling TTM", "Compiling scripts..." );

            foreach ( ScriptInfo scriptInfo in scripts )
            {
                if ( scriptInfo is null ) continue;
                if ( scriptInfo.VersionGetter is null ) continue;

                session.DisplayLine( $"Compiling {scriptInfo.Title} ...", 2, 1 );
                session.CompileScript( scriptInfo, buildFolder, deployFolder, deleteExisting, consoleOut );

                // set the script file name and format for export.
                scriptInfo.FileName = scriptInfo.DeployResourceFileName;
                scriptInfo.FileFormat = scriptInfo.DeployResourceFileFormat;

                // export the loaded script to file.
                session.ExportLoadedScript( scriptInfo, deployFolder, consoleOut );
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
        (string scriptName, string scriptSource, string scriptFunctionName) = SessionBaseMethods.BuildTimeDisplayClearScript();
        if ( session.IsScriptExists( scriptName, out string details ) )
        {
            session.TraceLastAction( $"\r\n\tDeleting {scriptName} script;. {details}" );
            session.DeleteScript( scriptName, true );
        }
        session.TraceLastAction( $"\r\n\tLoading {scriptName} script" );
        session.LoadScript( scriptName, scriptSource, TimeSpan.Zero, false, false );
        return (scriptName, scriptFunctionName);
    }
}
