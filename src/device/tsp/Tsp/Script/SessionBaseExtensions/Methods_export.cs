// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.Script.ExportExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
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
    /// <param name="overwrite">    True to overwrite, false to preserve. </param>
    /// <param name="details">      [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryExportScript( this SessionBase session, string scriptName, string filePath, bool overwrite, out string details )
    {
        if ( session == null ) throw new ArgumentNullException( nameof( session ) );
        if ( !session.IsSessionOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open." );
        if ( string.IsNullOrWhiteSpace( scriptName ) ) throw new ArgumentNullException( nameof( scriptName ) );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        if ( !overwrite && System.IO.File.Exists( filePath ) )
            details = $"The script {scriptName} cannot be exported because the file '{filePath}' exists.";
        else
        {
            details = string.Empty;

            session.SetLastAction( $"checking if the {scriptName} script exists;. " );
            session.LastNodeNumber = default;
            if ( session.IsNil( scriptName ) )
                details = $"The script {scriptName} cannot be exported because it was not found.";
            else
            {
                string scriptSource = session.FetchScript( scriptName );
                if ( string.IsNullOrWhiteSpace( scriptSource ) )
                    details = $"The script {scriptName} cannot be exported because it is empty.";
                else
                    // write the source to file.
                    System.IO.File.WriteAllText( filePath, scriptSource, System.Text.Encoding.Default );
            }
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
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
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
    public static bool TryExportScript( this Pith.SessionBase session, IScriptCompressor compressor, IScriptEncryptor encryptor,
        string scriptTitle, string filePath, ScriptFormats fileFormat, bool overwrite, bool validate, out string details )
    {
        if ( session is null ) throw new InvalidOperationException( $"{nameof( session )} is null" );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open" );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new InvalidOperationException( $"{nameof( filePath )} is null or empty" );

        if ( !overwrite && System.IO.File.Exists( filePath ) )
            details = $"The script {scriptTitle} cannot be exported because the file '{filePath}' exists.";
        else
        {
            details = string.Empty;
            try
            {
                string message = $"Finding '{scriptTitle}'";
                // _ = SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                _ = SessionLogger.Instance.LogDebug( message );

                if ( session.IsNil( scriptTitle ) )
                    details = $"Failed exporting script {scriptTitle};. The script is not loaded.";

                string scriptSource = string.Empty;

                if ( string.IsNullOrWhiteSpace( details ) )
                {
                    message = $"Fetching '{scriptTitle}'";
                    // _ = SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                    _ = SessionLogger.Instance.LogDebug( message );

                    scriptSource = session.FetchScript( scriptTitle );
                    if ( string.IsNullOrWhiteSpace( scriptSource ) )
                        details = $"Failed fetching script {scriptTitle} source;. ";
                }

                if ( string.IsNullOrWhiteSpace( details ) && fileFormat.HasFlag( ScriptFormats.Compressed ) )
                {
                    message = $"Compressing '{scriptTitle}'";
                    // _ = SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                    _ = SessionLogger.Instance.LogDebug( message );

                    scriptSource = compressor.CompressToBase64( scriptSource );

                    if ( validate )
                    {
                        // validate the compression
                        string decompressed = compressor.DecompressFromBase64( compressor.CompressToBase64( scriptSource ) );
                        if ( !string.Equals( scriptSource, decompressed, StringComparison.Ordinal ) )
                            details = $"Failed validating the compression of the script {scriptTitle} source;. ";
                    }
                }

                if ( string.IsNullOrWhiteSpace( details ) && fileFormat.HasFlag( ScriptFormats.Encrypted ) )
                {
                    message = $"Encrypting '{scriptTitle}'";
                    // _ = SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
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

                if ( string.IsNullOrWhiteSpace( details ) )
                {

                    message = $"Exporting '{scriptTitle}'\r\n\t\tto '{filePath}'";
                    // _ = SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                    _ = SessionLogger.Instance.LogDebug( message );

                    if ( scriptSource.TryExportScript( filePath, overwrite, validate, out details ) )
                    {
                        if ( validate )
                        {
                            message = $"Validating '{scriptTitle}'\r\n\t\tfrom '{filePath}'";
                            // _ = SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                            _ = SessionLogger.Instance.LogDebug( message );

                            scriptSource = session.FetchScript( scriptTitle );
                            string importedContents = System.IO.File.ReadAllText( filePath );
                            if ( fileFormat.HasFlag( ScriptFormats.Encrypted ) )
                                importedContents = encryptor.DecryptFromBase64( importedContents );
                            if ( fileFormat.HasFlag( ScriptFormats.Compressed ) )
                                importedContents = compressor.DecompressFromBase64( importedContents );
                            if ( !string.Equals( scriptSource, importedContents, StringComparison.Ordinal ) )
                                details = $"Failed validating the the exported script {scriptTitle} source;. ";
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                details = ex.Message;
            }
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
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
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
    public static bool TryExportScript( this Pith.SessionBase session, IScriptCompressor compressor, IScriptEncryptor encryptor,
        string scriptTitle, string scriptVersionGetter, VersionInfoBase versionInfo,
        string folderPath, ScriptFormats fileFormat, bool overwrite, bool validate, out string details )
    {
        if ( session is null ) throw new InvalidOperationException( $"{nameof( session )} is null" );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open" );
        if ( string.IsNullOrWhiteSpace( folderPath ) ) throw new InvalidOperationException( $"{nameof( folderPath )} is null or empty" );

        string message = $"Fetching '{scriptTitle}'";
        // _ = SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
        _ = SessionLogger.Instance.LogDebug( message );

        if ( session.IsNil( scriptTitle ) )
            details = $"Failed exporting script {scriptTitle};. The script is not loaded.";
        else
            details = string.Empty;

        if ( string.IsNullOrWhiteSpace( details ) )
        {
            // run the script
            session.RunScript( scriptTitle );

            string actualVersion = session.QueryFirmwareVersion( scriptVersionGetter );
            actualVersion = Syntax.Tsp.Lua.NilValue == actualVersion
                ? "0.0.9999"
                : actualVersion;

            // Correcting the assignment to use the appropriate method to select the file extension
            ScriptFormats exportFileFormat = session.IsByteCodeScript( scriptTitle )
                    ? fileFormat | ScriptFormats.ByteCode
                    : fileFormat;

            string buildNumber = new Version( actualVersion ).Build.ToString( System.Globalization.CultureInfo.CurrentCulture );

            string fileExtension = Tsp.Script.ScriptInfo.SelectScriptFileExtension( exportFileFormat );

            string fileName = $"{scriptTitle}.{buildNumber}.{versionInfo.ModelFamily}.{versionInfo.FirmwareVersion.Major}{fileExtension}";

            // string fileName = $"{scriptTitle}.{buildNumber}.{System.DateTime.Now:yyyyMMddHHmm}{fileExtension}";

            string filePath = Path.Combine( folderPath, fileName );

            // export the script
            _ = session.TryExportScript( compressor, encryptor, scriptTitle, filePath, exportFileFormat, overwrite, validate, out details );

        }
        return string.IsNullOrWhiteSpace( details );
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
    /// <param name="versionInfo">              Information describing the version. </param>
    /// <param name="folderPath">               Full pathname of the export folder. </param>
    /// <param name="fileFormat">               The file format. </param>
    /// <param name="overwrite">                True to overwrite, false to preserve. </param>
    /// <param name="validate">                 True to validate. </param>
    /// <param name="details">                  [out] The details. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool TryExportEmbeddedScripts( this Pith.SessionBase session, IScriptCompressor compressor, IScriptEncryptor encryptor,
        string[] scriptsNames, string[] scriptsVersionGetters, VersionInfoBase versionInfo,
        string folderPath, ScriptFormats fileFormat, bool overwrite, bool validate, out string details )
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
            // _ = SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
            _ = SessionLogger.Instance.LogDebug( message );
            details = message;
        }
        else
        {
            details = string.Empty;
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
                    if ( !session.TryExportScript( compressor, encryptor, scriptTitle, versionGetter, versionInfo,
                        folderPath, fileFormat, overwrite, validate, out details ) ) { break; }
                }
            }
        }
        return string.IsNullOrWhiteSpace( details );
    }
}
