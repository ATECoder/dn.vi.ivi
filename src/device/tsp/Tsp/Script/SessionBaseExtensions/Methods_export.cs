using cc.isr.VI.Pith;
using cc.isr.VI.Tsp.Script.ExportExtensions;

namespace cc.isr.VI.Tsp.Script.SessionBaseExtensions;

public static partial class SessionBaseExtensionMethods
{
    #region " support methods "

    /// <summary>   Gets temporary path. </summary>
    /// <remarks>   2025-06-03. </remarks>
    /// <param name="subFolderName">    (optional) [RegisterTests] Pathname of the sub folder. </param>
    /// <returns>   The temporary path. </returns>
    public static string GetTempPath( string subFolderName = "SessionBaseExtensions" )
    {
        string tempPath = Path.Combine( Path.GetTempPath(), "~cc.isr", subFolderName );
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

    /// <summary>
    /// A <see cref="Pith.SessionBase"/> extension method that export embedded scripts.
    /// </summary>
    /// <remarks>   2025-05-08. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="session">                  The session to act on. </param>
    /// <param name="compressor">               The compressor. </param>
    /// <param name="encryptor">                The encryptor. </param>
    /// <param name="versionInfo">              Information describing the version. </param>
    /// <param name="frameworkScripts">         The framework scripts. </param>
    /// <param name="frameworkVersionGetters">  The version getters. </param>
    /// <param name="topPath">                  Full pathname of the top file. </param>
    /// <param name="fileFormat">               The file format. </param>
    /// <param name="consoleOut">               (Optional) True to console out. </param>
    /// <param name="validate">                 (Optional) True to validate. </param>
    public static void ExportEmbeddedScripts( this Pith.SessionBase session, IScriptCompressor compressor, IScriptEncryptor encryptor,
        VersionInfoBase versionInfo, List<string[]> frameworkScripts, List<string[]> frameworkVersionGetters,
        string topPath, ScriptFormats fileFormat, bool consoleOut = false, bool validate = true )
    {
        if ( session is null ) throw new InvalidOperationException( $"{nameof( session )} is null" );
        if ( !session.IsDeviceOpen ) throw new InvalidOperationException( $"{nameof( session )} is not open" );
        if ( versionInfo is null ) throw new InvalidOperationException( $"{nameof( versionInfo )} is null" );

        if ( string.IsNullOrWhiteSpace( topPath ) )
            topPath = System.IO.Path.GetTempPath();

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
            string majorRevision = versionInfo.FirmwareVersion.Major.ToString( System.Globalization.CultureInfo.CurrentCulture );
            string model = versionInfo.Model;
            string serialNumber = versionInfo.SerialNumber;
            if ( string.IsNullOrWhiteSpace( model ) )
                throw new InvalidOperationException( $"{nameof( VersionInfoBase )}.{nameof( VersionInfoBase.Model )} must not be null or empty." );

            string folderPath = Path.Combine( topPath, $"{model}.{majorRevision}" );
            if ( !Directory.Exists( folderPath ) )
                _ = Directory.CreateDirectory( folderPath );

            folderPath = Path.Combine( folderPath, serialNumber );
            if ( !Directory.Exists( folderPath ) )
                _ = Directory.CreateDirectory( folderPath );


            foreach ( string scriptName in embeddedScriptsNames.Split( ',' ) )
            {
                string scriptTitle = scriptName.Trim();
                message = $"Fetching '{scriptTitle}'";
                if ( consoleOut )
                    SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                else
                    _ = SessionLogger.Instance.LogDebug( message );

                string scriptSource = session.FetchScript( scriptTitle );
                if ( string.IsNullOrWhiteSpace( scriptSource ) )
                    throw new InvalidOperationException( $"Failed fetching script {scriptTitle} source;. " );

                // run the script
                session.RunScript( scriptTitle );

                string actualVersion = Syntax.Tsp.Lua.NilValue;
                // lookup the script version.
                for ( int i = 0; i < frameworkScripts.Count; i++ )
                {
                    string[] versionGetters = frameworkVersionGetters[i];
                    string[] scriptNames = frameworkScripts[i];
                    if ( scriptNames.Contains( scriptTitle ) )
                    {
                        int idx = Array.IndexOf( scriptNames, scriptTitle );
                        // get the version from the script.
                        actualVersion = session.QueryFirmwareVersion( versionGetters[idx] );
                        if ( Syntax.Tsp.Lua.NilValue != actualVersion )
                            break;
                    }
                }
                actualVersion = Syntax.Tsp.Lua.NilValue == actualVersion
                    ? "0.0.9999"
                    : actualVersion;

                // Correcting the assignment to use the appropriate method to select the file extension
                ScriptFormats exportFileFormat = scriptSource.IsByteCodeScript()
                        ? fileFormat | ScriptFormats.ByteCode
                        : fileFormat;

                string fileExtension = Tsp.Script.ScriptInfo.SelectScriptFileExtension( exportFileFormat );

                string buildNumber = new Version( actualVersion ).Build.ToString( System.Globalization.CultureInfo.CurrentCulture );
                string fileName = $"{scriptTitle}.{buildNumber}.{System.DateTime.Now:yyyyMMddHHmm}{fileExtension}";

                string filePath = Path.Combine( folderPath, fileName );

                message = $"Exporting '{scriptTitle}'\r\n\t\tto '{filePath}'";
                if ( consoleOut )
                    SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                else
                    _ = SessionLogger.Instance.LogDebug( message );

                if ( exportFileFormat.HasFlag( ScriptFormats.Compressed ) )
                {
                    message = $"Compressing '{scriptTitle}'";
                    if ( consoleOut )
                        SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                    else
                        _ = SessionLogger.Instance.LogDebug( message );

                    if ( validate )
                    {
                        // validate the compression
                        string decompressed = compressor.DecompressFromBase64( compressor.CompressToBase64( scriptSource ) );
                        if ( !string.Equals( scriptSource, decompressed, StringComparison.Ordinal ) )
                            throw new InvalidOperationException( $"Failed validating the compression of the script {scriptTitle} source;. " );
                    }

                    scriptSource = compressor.CompressToBase64( scriptSource );
                }

                if ( exportFileFormat.HasFlag( ScriptFormats.Encrypted ) )
                {
                    message = $"Encrypting '{scriptTitle}'";
                    if ( consoleOut )
                        SessionBaseExtensionMethods.ConsoleOutputMemberMessage( message );
                    else
                        _ = SessionLogger.Instance.LogDebug( message );

                    if ( validate )
                    {
                        // validate the encryption
                        string decrypted = encryptor.DecryptFromBase64( encryptor.EncryptToBase64( scriptSource ) );
                        if ( !string.Equals( scriptSource, decrypted, StringComparison.Ordinal ) )
                            throw new InvalidOperationException( $"Failed validating the encryption of the script {scriptTitle} source;. " );
                    }

                    scriptSource = encryptor.EncryptToBase64( scriptSource );
                }

                scriptSource.ExportScript( filePath );

                if ( validate )
                {
                    string importedContents = System.IO.File.ReadAllText( filePath );
                    if ( exportFileFormat.HasFlag( ScriptFormats.Encrypted ) )
                        importedContents = encryptor.DecryptFromBase64( importedContents );
                    if ( exportFileFormat.HasFlag( ScriptFormats.Compressed ) )
                        importedContents = compressor.DecompressFromBase64( importedContents );
                    if ( !string.Equals( scriptSource, importedContents, StringComparison.Ordinal ) )
                        throw new InvalidOperationException( $"Failed validating the the exported script {scriptTitle} source;. " );
                }
            }
        }
    }

}
