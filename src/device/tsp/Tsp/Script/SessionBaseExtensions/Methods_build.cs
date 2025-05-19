using System.Text;
using cc.isr.VI.Tsp.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    /// <summary>   A <see cref="string"/> extension method that import script. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="fromFilePath">     The file path. </param>
    /// <param name="toFilePath">       True to delete an existing script if it exists. </param>
    /// <param name="retainOutline">    True to retain outline. </param>
    public static void TrimScript( this string fromFilePath, string toFilePath, bool retainOutline )
    {
        if ( fromFilePath is null || string.IsNullOrWhiteSpace( fromFilePath ) ) throw new ArgumentNullException( nameof( fromFilePath ) );
        if ( toFilePath is null || string.IsNullOrWhiteSpace( toFilePath ) ) throw new ArgumentNullException( nameof( toFilePath ) );
        cc.isr.VI.Syntax.Tsp.TspScriptParser.TrimTspSourceCode( fromFilePath, toFilePath, retainOutline );
    }

    /// <summary>   A ScriptInfoBase extension method that trim compress. </summary>
    /// <remarks>   2025-04-16. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="scriptInfo">           Information describing the script. </param>
    /// <param name="sourceFolder">         Full pathname of the folder file. </param>
    /// <param name="destinationFolder">    (Optional) [empty] Path of the destination folder. If
    ///                                     empty, the destination folder is set to the source folder. </param>
    public static void TrimCompress( this ScriptInfo scriptInfo, string sourceFolder, string destinationFolder = "" )
    {
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        string fromFilePath = Path.Combine( sourceFolder, scriptInfo.BuiltFileName );
        if ( string.IsNullOrWhiteSpace( destinationFolder ) )
            destinationFolder = sourceFolder;
        string trimmedFilePath = Path.Combine( destinationFolder, scriptInfo.TrimmedFileName );
        string compressedFilePath = trimmedFilePath + "c";
        if ( System.IO.File.Exists( fromFilePath ) )
        {
            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\tTrimming script file '{fromFilePath}'\r\n\t\tto '{trimmedFilePath}'" );
            fromFilePath.TrimScript( trimmedFilePath, true );

            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\tCompressing '{trimmedFilePath}'\r\n\t\tto '{compressedFilePath}'" );
            System.IO.File.WriteAllText( compressedFilePath, ScriptCompressor.Compress( System.IO.File.ReadAllText( trimmedFilePath ) ), System.Text.Encoding.Default );

        }
        else
            throw new FileNotFoundException( fromFilePath );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that trims, loads, optionally converts to
    /// byte code, and compresses the script to the deploy file.
    /// </summary>
    /// <remarks>   2025-04-15. </remarks>
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
    public static void TrimCompressLoadConvertExport( this Pith.SessionBase session, ScriptInfo scriptInfo, string buildFolder, string deployFolder, bool consoleOut = false )
    {
        if ( session is null ) throw new ArgumentNullException( nameof( session ) );
        if ( scriptInfo is null ) throw new ArgumentNullException( nameof( scriptInfo ) );

        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );

        string builtFilePath = Path.Combine( buildFolder, scriptInfo.BuiltFileName );
        string trimmedFilePath = Path.Combine( buildFolder, scriptInfo.TrimmedFileName );
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

            if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFileFormats.Compressed )
                && !scriptInfo.DeployFileFormat.HasFlag( ScriptFileFormats.ByteCode ) )
            {
                message = $"Compressing '{trimmedFilePath}'\r\n\t\tto '{deployFilePath}'";
                if ( consoleOut )
                    SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                else
                    SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );
                System.IO.File.WriteAllText( deployFilePath, ScriptCompressor.Compress( System.IO.File.ReadAllText( trimmedFilePath ) ), System.Text.Encoding.Default );
            }
        }
        else
            throw new FileNotFoundException( builtFilePath );

        // delete the script if it exists.
        session.DeleteScript( scriptInfo.Title );

        message = $"Importing script from trimmed '{trimmedFilePath}' file";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );
        session.ImportScript( scriptInfo.Title, trimmedFilePath, TimeSpan.Zero );

        // run the script to ensure the code works.
        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        // convert the script to byte code.
        session.ConvertToByteCode( scriptInfo.Title );

        // run the script to ensure the code works.
        session.RunScript( scriptInfo.Title, scriptInfo.VersionGetterElement );

        if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFileFormats.ByteCode ) )
        {
            if ( scriptInfo.DeployFileFormat.HasFlag( ScriptFileFormats.Compressed ) )
            {
                // export and compress the script.
                message = $"compressing byte code to '{deployFilePath}'";
                if ( consoleOut )
                    SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                else
                    SessionBaseExtensionMethods.TraceLastAction( $"\r\n\t{message}" );
                session.CompressScript( scriptInfo.Title, deployFilePath, true );
            }
            else
            {
                // if naked byte code, export to the deploy file name.
                SessionBaseExtensionMethods.TraceLastAction( $"\r\n\tFetching byte code to '{deployFilePath}'" );
                session.ExportScript( scriptInfo.Title, deployFilePath, true );
            }
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that trims, loads, optionally converts to
    /// byte code, and compresses all scripts the deploy files.
    /// </summary>
    /// <remarks>   2025-04-22. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="versionInfo">  The versionInfo to act on. </param>
    /// <param name="scripts">      The collection of scripts. </param>
    /// <param name="buildFolder">  Pathname of the build folder. </param>
    /// <param name="deployFolder"> Pathname of the deploy folder. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    public static void BuildUserScripts( this Pith.SessionBase session, VersionInfoBase versionInfo, ScriptInfoCollection scripts,
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
                session.TrimCompressLoadConvertExport( scriptInfo, buildFolder, deployFolder, consoleOut );
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
