using cc.isr.Std;
using cc.isr.Std.ReaderExtensions;
using cc.isr.VI.Pith;
using cc.isr.VI.Pith.ExceptionExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseMethods
{
    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that fetches and exports a script to file.
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="IOException">              Thrown when an I/O failure occurred. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="overwrite">    True to overwrite, false to preserve. </param>
    public static void ExportScript( this SessionBase session, string scriptName, string filePath, bool overwrite )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new IOException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        if ( !overwrite && System.IO.File.Exists( filePath ) )
            throw new IOException( $"The script {scriptName} cannot be exported because the file '{filePath}' exists." );

        session.SetLastAction( $"checking if the {scriptName} script exists;. " );
        session.LastNodeNumber = default;
        if ( session.IsNil( scriptName ) )
            throw new IOException( $"The script {scriptName} cannot be exported because it was not found." );

        string scriptSource = session.FetchScript( scriptName );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new IOException( $"The script {scriptName} cannot be exported because it is empty." );

        System.IO.File.WriteAllText( filePath, scriptSource, System.Text.Encoding.Default );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that fetches and exports a script to file.
    /// </summary>
    /// <remarks>   2025-04-10. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="IOException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="scriptName">   Name of the script. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="overwrite">    True to overwrite, false to preserve. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryExportScript( this SessionBase session, string scriptName, string filePath, bool overwrite, out string details )
    {
        details = string.Empty;
        try
        {
            session.ExportScript( scriptName, filePath, overwrite );
        }
        catch ( Exception ex )
        {
            if ( ex is IOException or InvalidOperationException )
            {
                details = ex.Message;
            }
            else
                details = ex.BuildMessage();
        }
        return string.IsNullOrWhiteSpace( details );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that exports a script to file with
    /// compression and encryption.
    /// </summary>
    /// <remarks>
    /// 2025-10-01. <para>
    /// The instrument model and major firmware version is added to the script file title if the
    /// export format is in byte code as follows: title.model.mode_major_version, e.g.,
    /// isr_support.2602.3
    /// </para>
    /// </remarks>
    /// <exception cref="IOException">  Thrown when an I/O failure occurred. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="compressor">   The compressor. </param>
    /// <param name="encryptor">    The encryptor. </param>
    /// <param name="scriptTitle">  The script title. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="fileFormat">   The file format. </param>
    /// <param name="overwrite">    True to overwrite, false to preserve. </param>
    /// <param name="validate">     True to validate. </param>
    public static void ExportScript( this Pith.SessionBase session, IStringCompressor compressor, IStringEncryptor encryptor,
        string scriptTitle, string filePath, FileFormats fileFormat, bool overwrite, bool validate )
    {
        if ( session is null ) throw new IOException( $"{nameof( session )} is null" );
        if ( !session.IsDeviceOpen ) throw new IOException( $"{nameof( session )} is not open" );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new IOException( $"{nameof( filePath )} is null or empty" );

        if ( !overwrite && System.IO.File.Exists( filePath ) )
            throw new IOException( $"The script {scriptTitle} cannot be exported because the file '{filePath}' exists." );

        string message = $"Finding '{scriptTitle}'";
        // _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        _ = SessionLogger.Instance.LogDebug( message );

        if ( session.IsNil( scriptTitle ) )
            throw new IOException( $"Failed exporting script {scriptTitle};. The script is not loaded." );

        message = $"Fetching '{scriptTitle}'";
        // _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        _ = SessionLogger.Instance.LogDebug( message );

        string scriptSource = session.FetchScript( scriptTitle );
        if ( string.IsNullOrWhiteSpace( scriptSource ) )
            throw new IOException( $"Failed fetching script {scriptTitle} source;. " );

        if ( fileFormat.HasFlag( FileFormats.Compressed ) )
        {
            message = $"Compressing '{scriptTitle}'";
            // _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            _ = SessionLogger.Instance.LogDebug( message );

            scriptSource = compressor.CompressToBase64( scriptSource );

            if ( validate )
            {
                // validate the compression
                string decompressed = compressor.DecompressFromBase64( compressor.CompressToBase64( scriptSource ) );
                if ( !string.Equals( scriptSource, decompressed, StringComparison.Ordinal ) )
                    throw new IOException( $"Failed validating the compression of the script {scriptTitle} source;. " );
            }
        }

        if ( fileFormat.HasFlag( FileFormats.Encrypted ) )
        {
            message = $"Encrypting '{scriptTitle}'";
            // _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            _ = SessionLogger.Instance.LogDebug( message );

            scriptSource = encryptor.EncryptToBase64( scriptSource );

            if ( validate )
            {
                // validate the encryption
                string decrypted = encryptor.DecryptFromBase64( encryptor.EncryptToBase64( scriptSource ) );
                if ( !string.Equals( scriptSource, decrypted, StringComparison.Ordinal ) )
                    throw new IOException( $"Failed validating the encryption of the script {scriptTitle} source;. " );
            }

        }

        message = $"Exporting '{scriptTitle}'\r\n\t\tto '{filePath}'";
        // _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        _ = SessionLogger.Instance.LogDebug( message );

        scriptSource.ExportToFile( filePath, overwrite, validate );

        if ( validate )
        {
            message = $"Validating '{scriptTitle}'\r\n\t\tfrom '{filePath}'";
            // _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            _ = SessionLogger.Instance.LogDebug( message );

            scriptSource = session.FetchScript( scriptTitle );
            string importedContents = System.IO.File.ReadAllText( filePath );
            if ( fileFormat.HasFlag( FileFormats.Encrypted ) )
                importedContents = encryptor.DecryptFromBase64( importedContents );
            if ( fileFormat.HasFlag( FileFormats.Compressed ) )
                importedContents = compressor.DecompressFromBase64( importedContents );
            if ( !string.Equals( scriptSource, importedContents, StringComparison.Ordinal ) )
                throw new IOException( $"Failed validating the the exported script {scriptTitle} source;. " );
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that exports a script to file with
    /// compression and encryption.
    /// </summary>
    /// <remarks>
    /// 2025-10-01. <para>
    /// The instrument model and major firmware version is added to the script file title if the
    /// export format is in byte code as follows: title.model.mode_major_version, e.g.,
    /// isr_support.2602.3
    /// </para>
    /// </remarks>
    /// <exception cref="IOException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">      The session to act on. </param>
    /// <param name="compressor">   The compressor. </param>
    /// <param name="encryptor">    The encryptor. </param>
    /// <param name="scriptTitle">  The script title. </param>
    /// <param name="filePath">     Full pathname of the file. </param>
    /// <param name="fileFormat">   The file format. </param>
    /// <param name="overwrite">    True to overwrite, false to preserve. </param>
    /// <param name="validate">     True to validate. </param>
    /// <param name="details">      [out] The details. </param>
    public static bool TryExportScript( this Pith.SessionBase session, IStringCompressor compressor, IStringEncryptor encryptor,
        string scriptTitle, string filePath, FileFormats fileFormat, bool overwrite, bool validate, out string details )
    {
        details = string.Empty;
        try
        {
            session.ExportScript( compressor, encryptor, scriptTitle, filePath, fileFormat, overwrite, validate );
        }
        catch ( Exception ex )
        {
            if ( ex is IOException or InvalidOperationException )
            {
                details = ex.Message;
            }
            else
                details = ex.BuildMessage();
        }
        return string.IsNullOrWhiteSpace( details );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that export embedded script.
    /// </summary>
    /// <remarks>
    /// 2025-10-01. <para>
    /// Previously, the script file title was built as follows: title.build_number,date+hour+minute,
    /// e.g., isr_support.9243.202510071638. Since the new export folder includes the date, the date
    /// is no longer included in the file name and it is assumed that the actual minute is not
    /// necessary, e.g., isr_support.9243.
    /// </para>
    /// </remarks>
    /// <exception cref="IOException">  Thrown when an I/O failure occurred. </exception>
    /// <param name="session">              The session to act on. </param>
    /// <param name="compressor">           The compressor. </param>
    /// <param name="encryptor">            The encryptor. </param>
    /// <param name="scriptTitle">          The script title. </param>
    /// <param name="scriptVersionGetter">  The script version getter. </param>
    /// <param name="versionInfo">          Information describing the version. </param>
    /// <param name="folderPath">           Full pathname of the top folder. </param>
    /// <param name="fileFormat">           The file format. </param>
    /// <param name="overwrite">            True to overwrite, false to preserve. </param>
    /// <param name="validate">             True to validate compression and encryption. </param>
    public static void ExportScript( this Pith.SessionBase session, IStringCompressor compressor, IStringEncryptor encryptor,
        string scriptTitle, string scriptVersionGetter, VersionInfoBase versionInfo,
        string folderPath, FileFormats fileFormat, bool overwrite, bool validate )
    {
        if ( session is null ) throw new IOException( $"{nameof( session )} is null" );
        if ( !session.IsDeviceOpen ) throw new IOException( $"{nameof( session )} is not open" );
        if ( string.IsNullOrWhiteSpace( folderPath ) ) throw new IOException( $"{nameof( folderPath )} is null or empty" );

        string message = $"Fetching '{scriptTitle}'";
        // _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
        _ = SessionLogger.Instance.LogDebug( message );

        if ( session.IsNil( scriptTitle ) )
            throw new IOException( $"Failed exporting script {scriptTitle};. The script is not loaded." );

        // run the script
        session.RunScript( scriptTitle );

        string actualVersion = session.QueryFirmwareVersion( scriptVersionGetter );
        actualVersion = Syntax.Tsp.Lua.NilValue == actualVersion
            ? "0.0.9999"
            : actualVersion;

        // Correcting the assignment to use the appropriate method to select the file extension
        FileFormats exportFileFormat = session.IsByteCodeScript( scriptTitle )
                ? fileFormat | FileFormats.ByteCode
                : fileFormat;

        string buildNumber = new Version( actualVersion ).Build.ToString( System.Globalization.CultureInfo.CurrentCulture );

        string fileExtension = Tsp.Script.ScriptInfo.SelectScriptFileExtension( exportFileFormat );

        string fileName = $"{scriptTitle}.{buildNumber}.{versionInfo.ModelFamily}.{versionInfo.FirmwareVersion.Major}{fileExtension}";

        // string fileName = $"{scriptTitle}.{buildNumber}.{System.DateTime.Now:yyyyMMddHHmm}{fileExtension}";

        string filePath = Path.Combine( folderPath, fileName );

        // export the script
        session.ExportScript( compressor, encryptor, scriptTitle, filePath, exportFileFormat, overwrite, validate );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that export embedded script.
    /// </summary>
    /// <remarks>
    /// 2025-10-01. <para>
    /// Previously, the script file title was built as follows: title.build_number,date+hour+minute,
    /// e.g., isr_support.9243.202510071638. Since the new export folder includes the date, the date
    /// is no longer included in the file name and it is assumed that the actual minute is not
    /// necessary, e.g., isr_support.9243.
    /// </para>
    /// </remarks>
    /// <exception cref="IOException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">              The session to act on. </param>
    /// <param name="compressor">           The compressor. </param>
    /// <param name="encryptor">            The encryptor. </param>
    /// <param name="scriptTitle">          The script title. </param>
    /// <param name="scriptVersionGetter">  The script version getter. </param>
    /// <param name="versionInfo">          Information describing the version. </param>
    /// <param name="folderPath">           Full pathname of the top folder. </param>
    /// <param name="fileFormat">           The file format. </param>
    /// <param name="overwrite">            True to overwrite, false to preserve. </param>
    /// <param name="validate">             True to validate compression and encryption. </param>
    /// <param name="details">              [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryExportScript( this Pith.SessionBase session, IStringCompressor compressor, IStringEncryptor encryptor,
        string scriptTitle, string scriptVersionGetter, VersionInfoBase versionInfo,
        string folderPath, FileFormats fileFormat, bool overwrite, bool validate, out string details )
    {
        details = string.Empty;
        try
        {
            session.ExportScript( compressor, encryptor, scriptTitle, scriptVersionGetter, versionInfo, folderPath, fileFormat, overwrite, validate );
        }
        catch ( Exception ex )
        {
            if ( ex is IOException or InvalidOperationException )
            {
                details = ex.Message;
            }
            else
                details = ex.BuildMessage();
        }
        return string.IsNullOrWhiteSpace( details );
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that export embedded scripts.
    /// </summary>
    /// <remarks>   2025-05-08. </remarks>
    /// <exception cref="IOException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">                  The session to act on. </param>
    /// <param name="compressor">               The compressor. </param>
    /// <param name="encryptor">                The encryptor. </param>
    /// <param name="scriptsNames">             The framework scripts. </param>
    /// <param name="scriptsVersionGetters">    The version getters. </param>
    /// <param name="versionInfo">              Information describing the version. </param>
    /// <param name="folderPath">               Full pathname of the export folder. </param>
    /// <param name="fileFormat">               The file format. </param>
    /// <param name="overwrite">                True to overwrite, false to preserve. </param>
    /// <param name="validate">                 True to validate. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static void ExportEmbeddedScripts( this Pith.SessionBase session, IStringCompressor compressor, IStringEncryptor encryptor,
        string[] scriptsNames, string[] scriptsVersionGetters, VersionInfoBase versionInfo,
        string folderPath, FileFormats fileFormat, bool overwrite, bool validate )
    {
        if ( session is null ) throw new IOException( $"{nameof( session )} is null" );
        if ( !session.IsDeviceOpen ) throw new IOException( $"{nameof( session )} is not open" );
        if ( string.IsNullOrWhiteSpace( folderPath ) )
            folderPath = cc.isr.Std.PathExtensions.PathMethods.GetTempPath( ["~cc.isr.vi.device.tdp"] );

        string embeddedScriptsNames = session.FetchEmbeddedScriptsNames().Trim();

        string message = "No embedded scripts were found.";
        if ( string.IsNullOrWhiteSpace( embeddedScriptsNames ) )
        {
            // _ = cc.isr.Std.ConsoleExtensions.ConsoleMethods.ConsoleOutputMemberMessage( message );
            _ = SessionLogger.Instance.LogDebug( message );
            throw new IOException( message );
        }

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
                session.ExportScript( compressor, encryptor, scriptTitle, versionGetter, versionInfo,
                    folderPath, fileFormat, overwrite, validate );
            }
        }
    }

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that export embedded scripts.
    /// </summary>
    /// <remarks>   2025-05-08. </remarks>
    /// <exception cref="IOException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">                  The session to act on. </param>
    /// <param name="compressor">               The compressor. </param>
    /// <param name="encryptor">                The encryptor. </param>
    /// <param name="scriptsNames">             The framework scripts. </param>
    /// <param name="scriptsVersionGetters">    The version getters. </param>
    /// <param name="versionInfo">              Information describing the version. </param>
    /// <param name="folderPath">               Full pathname of the export folder. </param>
    /// <param name="fileFormat">               The file format. </param>
    /// <param name="overwrite">                True to overwrite, false to preserve. </param>
    /// <param name="validate">                 True to validate. </param>
    /// <param name="details">                  [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryExportEmbeddedScripts( this Pith.SessionBase session, IStringCompressor compressor, IStringEncryptor encryptor,
        string[] scriptsNames, string[] scriptsVersionGetters, VersionInfoBase versionInfo,
        string folderPath, FileFormats fileFormat, bool overwrite, bool validate, out string details )
    {
        details = string.Empty;
        try
        {
            session.ExportEmbeddedScripts( compressor, encryptor, scriptsNames, scriptsVersionGetters, versionInfo,
                folderPath, fileFormat, overwrite, validate );
        }
        catch ( Exception ex )
        {
            if ( ex is IOException or InvalidOperationException )
            {
                details = ex.Message;
            }
            else
                details = ex.BuildMessage();
        }
        return string.IsNullOrWhiteSpace( details );
    }

}
