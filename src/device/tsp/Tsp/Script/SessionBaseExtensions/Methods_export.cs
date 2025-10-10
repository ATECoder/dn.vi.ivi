using System.Runtime.CompilerServices;
using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.Script.ExportExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    #region " support methods "

    /// <summary>   Gets temporary path under the '~cc.isr` and this class namespace folder name. </summary>
    /// <remarks>   2025-06-03. </remarks>
    /// <param name="firstSubfolderName">   (Optional) [CallerMemberName] Name of the second
    ///                                     subfolder. </param>
    /// <param name="secondSubfolderName">  (Optional) Name of the second subfolder. </param>
    /// <returns>   The temporary path. </returns>
    public static string GetTempPath( [CallerMemberName] string firstSubfolderName = "", string secondSubfolderName = "" )
    {
        string tempPath = Path.Combine( Path.GetTempPath(), "~cc.isr", "VI", "Tsp", "Script", "SessionBaseExtensions" );

        if ( !string.IsNullOrWhiteSpace( firstSubfolderName ) )
        {
            tempPath = Path.Combine( tempPath, firstSubfolderName );
        }

        if ( !string.IsNullOrWhiteSpace( secondSubfolderName ) )
        {
            tempPath = Path.Combine( tempPath, secondSubfolderName );
        }

        _ = System.IO.Directory.CreateDirectory( tempPath );
        return tempPath;
    }

    #endregion

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that fetches and exports a script to file.
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    public static void ExportScript( this SessionBase session, string scriptName, string filePath, bool overWrite = false )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        if ( !overWrite && System.IO.File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because the file '{filePath}' exists." );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it was not found." );

        string scriptSource = session.FetchScript( scriptName );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it is empty." );

        // write the source to file.
        System.IO.File.WriteAllText( filePath, scriptSource, System.Text.Encoding.Default );
    }

    /// <summary>
    /// A <see cref="string"/> extension method that fetches a script from the instrument and
    /// compress it to a file.
    /// </summary>
    /// <remarks>   2025-04-16. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="compressor">   The compressor. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="filePath">     Full pathname of the destination file. </param>
    /// <param name="overWrite">    (Optional) [false] True to over write. </param>
    public static void CompressScript( this SessionBase session, IScriptCompressor compressor, string scriptName, string filePath, bool overWrite = false )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        if ( !overWrite && System.IO.File.Exists( filePath ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because the file '{filePath}' exists." );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it was not found." );

        string scriptSource = session.FetchScript( scriptName );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"The script {scriptName} cannot be exported because it is empty." );

        // compress and export the source to the file as is.
        File.WriteAllText( filePath, compressor.CompressToBase64( scriptSource ), System.Text.Encoding.Default );
    }

    /// <summary>   A Pith.SessionBase extension method that export embedded script. </summary>
    /// <remarks>
    /// 2025-10-01. <para>
    /// Previously, the script file title was built as follows: title.build_number,date+hour+minute,
    /// e.g., isr_support.9243.202510071638. Since the new export folder includes the date, the date
    /// is no longer included in the file name and it is assumed that the actual minute is not
    /// necessary, e.g., isr_support.9243.
    /// </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">              The session to act on. </param>
    /// <param name="compressor">           The compressor. </param>
    /// <param name="encryptor">            The encryptor. </param>
    /// <param name="scriptTitle">          The script title. </param>
    /// <param name="scriptVersionGetter">  The script version getter. </param>
    /// <param name="folderPath">           Full pathname of the top folder. </param>
    /// <param name="fileFormat">           The file format. </param>
    /// <param name="consoleOut">           (Optional) True to console out. </param>
    /// <param name="validate">             (Optional) True to validate. </param>
    public static void ExportEmbeddedScript( this Pith.SessionBase session, IScriptCompressor compressor, IScriptEncryptor encryptor,
        string scriptTitle, string scriptVersionGetter,
        string folderPath, ScriptFormats fileFormat, bool consoleOut = false, bool validate = true )
    {
        if ( session is null ) throw new InvalidOperationException( $"{nameof( session )} is null" );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open" );

        if ( string.IsNullOrWhiteSpace( folderPath ) ) throw new InvalidOperationException( $"{nameof( folderPath )} is null or empty" );

        string message = $"Fetching '{scriptTitle}'";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            _ = SessionLogger.Instance.LogDebug( message );

        string scriptSource = session.FetchScript( scriptTitle );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"Failed fetching script {scriptTitle} source;. " );

        // run the script
        session.RunScript( scriptTitle );

        string actualVersion = session.QueryFirmwareVersion( scriptVersionGetter );
        actualVersion = Syntax.Tsp.Lua.NilValue == actualVersion
            ? "0.0.9999"
            : actualVersion;

        // Correcting the assignment to use the appropriate method to select the file extension
        ScriptFormats exportFileFormat = scriptSource.IsByteCodeScript()
                ? fileFormat | ScriptFormats.ByteCode
                : fileFormat;

        string fileExtension = Tsp.Script.ScriptInfo.SelectScriptFileExtension( exportFileFormat );

        string buildNumber = new Version( actualVersion ).Build.ToString( System.Globalization.CultureInfo.CurrentCulture );
        string fileName = $"{scriptTitle}.{buildNumber}{fileExtension}";
        // string fileName = $"{scriptTitle}.{buildNumber}.{System.DateTime.Now:yyyyMMddHHmm}{fileExtension}";

        string filePath = Path.Combine( folderPath, fileName );

        if ( exportFileFormat.HasFlag( ScriptFormats.Compressed ) )
        {
            message = $"Compressing '{scriptTitle}'";
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                _ = SessionLogger.Instance.LogDebug( message );

            scriptSource = compressor.CompressToBase64( scriptSource );

            if ( validate )
            {
                // validate the compression
                string decompressed = compressor.DecompressFromBase64( compressor.CompressToBase64( scriptSource ) );
                if ( !string.Equals( scriptSource, decompressed, StringComparison.Ordinal ) )
                    throw new InvalidOperationException( $"Failed validating the compression of the script {scriptTitle} source;. " );
            }
        }

        if ( exportFileFormat.HasFlag( ScriptFormats.Encrypted ) )
        {
            message = $"Encrypting '{scriptTitle}'";
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                _ = SessionLogger.Instance.LogDebug( message );

            scriptSource = encryptor.EncryptToBase64( scriptSource );

            if ( validate )
            {
                // validate the encryption
                string decrypted = encryptor.DecryptFromBase64( encryptor.EncryptToBase64( scriptSource ) );
                if ( !string.Equals( scriptSource, decrypted, StringComparison.Ordinal ) )
                    throw new InvalidOperationException( $"Failed validating the encryption of the script {scriptTitle} source;. " );
            }

        }

        message = $"Exporting '{scriptTitle}'\r\n\t\tto '{filePath}'";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            _ = SessionLogger.Instance.LogDebug( message );

        scriptSource.ExportScript( filePath );

        if ( validate )
        {
            scriptSource = session.FetchScript( scriptTitle );
            string importedContents = System.IO.File.ReadAllText( filePath );
            if ( exportFileFormat.HasFlag( ScriptFormats.Encrypted ) )
                importedContents = encryptor.DecryptFromBase64( importedContents );
            if ( exportFileFormat.HasFlag( ScriptFormats.Compressed ) )
                importedContents = compressor.DecompressFromBase64( importedContents );
            if ( !string.Equals( scriptSource, importedContents, StringComparison.Ordinal ) )
                throw new InvalidOperationException( $"Failed validating the the exported script {scriptTitle} source;. " );
        }
    }

    /// <summary>   A Pith.SessionBase extension method that export embedded script. </summary>
    /// <remarks>
    /// 2025-10-01. <para>
    /// The instrument model and major firmware version is added to the script file title if the
    /// export format is in byte code as follows: title.model.mode_major_version, e.g.,
    /// isr_support.2602.3
    /// </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="compressor">   The compressor. </param>
    /// <param name="encryptor">    The encryptor. </param>
    /// <param name="scriptTitle">  The script title. </param>
    /// <param name="versionInfo">  Information describing the version. </param>
    /// <param name="folderPath">   Full pathname of the top folder. </param>
    /// <param name="fileFormat">   The file format. </param>
    /// <param name="consoleOut">   (Optional) True to console out. </param>
    /// <param name="validate">     (Optional) True to validate. </param>
    public static void ExportEmbeddedScript( this Pith.SessionBase session, IScriptCompressor compressor, IScriptEncryptor encryptor,
        string scriptTitle, VersionInfoBase versionInfo,
        string folderPath, ScriptFormats fileFormat, bool consoleOut = false, bool validate = true )
    {
        if ( session is null ) throw new InvalidOperationException( $"{nameof( session )} is null" );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open" );

        if ( string.IsNullOrWhiteSpace( folderPath ) ) throw new InvalidOperationException( $"{nameof( folderPath )} is null or empty" );

        string message = $"Fetching '{scriptTitle}'";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            _ = SessionLogger.Instance.LogDebug( message );

        string scriptSource = session.FetchScript( scriptTitle );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new InvalidOperationException( $"Failed fetching script {scriptTitle} source;. " );

        // run the script
        // not getting the version. No need to run the script
        // session.RunScript( scriptTitle );

        // Correcting the assignment to use the appropriate method to select the file extension
        ScriptFormats exportFileFormat = scriptSource.IsByteCodeScript()
                ? fileFormat | ScriptFormats.ByteCode
                : fileFormat;

        string fileExtension = Tsp.Script.ScriptInfo.SelectScriptFileExtension( exportFileFormat );

        string fileName = exportFileFormat.HasFlag( ScriptFormats.ByteCode )
            ? $"{scriptTitle}.{versionInfo.ModelFamily}.{versionInfo.FirmwareVersion.Major}{fileExtension}"
            : $"{scriptTitle}{fileExtension}";

        string filePath = Path.Combine( folderPath, fileName );

        if ( exportFileFormat.HasFlag( ScriptFormats.Compressed ) )
        {
            message = $"Compressing '{scriptTitle}'";
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                _ = SessionLogger.Instance.LogDebug( message );

            scriptSource = compressor.CompressToBase64( scriptSource );

            if ( validate )
            {
                // validate the compression
                string decompressed = compressor.DecompressFromBase64( compressor.CompressToBase64( scriptSource ) );
                if ( !string.Equals( scriptSource, decompressed, StringComparison.Ordinal ) )
                    throw new InvalidOperationException( $"Failed validating the compression of the script {scriptTitle} source;. " );
            }
        }

        if ( exportFileFormat.HasFlag( ScriptFormats.Encrypted ) )
        {
            message = $"Encrypting '{scriptTitle}'";
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                _ = SessionLogger.Instance.LogDebug( message );

            scriptSource = encryptor.EncryptToBase64( scriptSource );

            if ( validate )
            {
                // validate the encryption
                string decrypted = encryptor.DecryptFromBase64( encryptor.EncryptToBase64( scriptSource ) );
                if ( !string.Equals( scriptSource, decrypted, StringComparison.Ordinal ) )
                    throw new InvalidOperationException( $"Failed validating the encryption of the script {scriptTitle} source;. " );
            }

        }

        message = $"Exporting '{scriptTitle}'\r\n\t\tto '{filePath}'";
        if ( consoleOut )
            SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        else
            _ = SessionLogger.Instance.LogDebug( message );

        scriptSource.ExportScript( filePath );

        if ( validate )
        {
            scriptSource = session.FetchScript( scriptTitle );
            string importedContents = System.IO.File.ReadAllText( filePath );
            if ( exportFileFormat.HasFlag( ScriptFormats.Encrypted ) )
                importedContents = encryptor.DecryptFromBase64( importedContents );
            if ( exportFileFormat.HasFlag( ScriptFormats.Compressed ) )
                importedContents = compressor.DecompressFromBase64( importedContents );
            if ( !string.Equals( scriptSource, importedContents, StringComparison.Ordinal ) )
                throw new InvalidOperationException( $"Failed validating the the exported script {scriptTitle} source;. " );
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that export embedded scripts.
    /// </summary>
    /// <remarks>   2025-05-08. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">                  The session to act on. </param>
    /// <param name="compressor">               The compressor. </param>
    /// <param name="encryptor">                The encryptor. </param>
    /// <param name="scriptsNames">             The framework scripts. </param>
    /// <param name="scriptsVersionGetters">    The version getters. </param>
    /// <param name="folderPath">               Full pathname of the export folder. </param>
    /// <param name="fileFormat">               The file format. </param>
    /// <param name="consoleOut">               True to console out. </param>
    /// <param name="validate">                 True to validate. </param>
    public static void ExportEmbeddedScripts( this Pith.SessionBase session, IScriptCompressor compressor, IScriptEncryptor encryptor,
        string[] scriptsNames, string[] scriptsVersionGetters,
        string folderPath, ScriptFormats fileFormat, bool consoleOut, bool validate )
    {
        if ( session is null ) throw new InvalidOperationException( $"{nameof( session )} is null" );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open" );
        if ( string.IsNullOrWhiteSpace( folderPath ) )
        {
            folderPath = System.IO.Path.GetTempPath();
            folderPath = Path.Combine( folderPath, "~cc.isr", "exports" );
        }

        string embeddedScriptsNames = session.FetchEmbeddedScriptsNames().Trim();

        string message = "No embedded scripts were found.";
        if ( string.IsNullOrWhiteSpace( embeddedScriptsNames ) )
        {
            if ( consoleOut )
                SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            else
                _ = SessionLogger.Instance.LogDebug( message );
        }
        else
        {
            foreach ( string scriptName in embeddedScriptsNames.Split( ',' ) )
            {
                string scriptTitle = scriptName.Trim();

                // lookup the script version getter.
                if ( scriptsNames.Contains( scriptTitle ) )
                {
                    int idx = Array.IndexOf( scriptsNames, scriptTitle );

                    // get the version from the script.
                    string versionGetter = scriptsVersionGetters[idx];

                    // use the version getter so that each script is identified with its version to be sure.
                    session.ExportEmbeddedScript( compressor, encryptor, scriptTitle, versionGetter,
                        folderPath, fileFormat, consoleOut, validate );
                }
            }
        }
    }
}
