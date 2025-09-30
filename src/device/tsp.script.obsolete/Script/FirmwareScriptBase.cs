using System.Text;
using cc.isr.Std.TrimExtensions;
using cc.isr.VI.Syntax.Tsp;
using cc.isr.VI.Tsp.Script.SessionBaseExtensions;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   Provides the contract for a Firmware Script. </summary>
/// <remarks>
/// David, 2009-03-02, 3.0.3348. <para>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. </para><para>
/// Licensed under The MIT License. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class FirmwareScriptBase
{
    #region " construction and cleanup "

    /// <summary>   Specialized default constructor for use only by derived classes. </summary>
    /// <remarks>   2024-09-05. </remarks>
    protected FirmwareScriptBase() : base()
    {
        this.Name = string.Empty;
        this.ModelMask = string.Empty;
        this.ModelVersion = string.Empty;
        this.FileTitle = string.Empty;
        this.FolderPath = string.Empty;
        this.FirmwareVersion = string.Empty;
        this._source = string.Empty;
    }

    /// <summary>   Constructs this class. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="name">         Specifies the script name. </param>
    /// <param name="modelMask">    Specifies the model families for this script. e.g., 2600A. </param>
    /// <param name="modelVersion"> The model version. </param>
    protected FirmwareScriptBase( string name, string modelMask, Version modelVersion ) : this()
    {
        this.Name = name;
        this.ModelMask = modelMask;
        this.ModelVersion = modelVersion.ToString();
    }

    #endregion

    #region " script naming "

    /// <summary>   (Immutable) the script file extension. </summary>
    public const string ScriptFileExtension = ".tsp";

    /// <summary>   (Immutable) the script binary compressed file extension. </summary>
    public const string ScriptCompressedFileExtension = ".tspc";

    /// <summary>   (Immutable) the script binary file extension. </summary>
    public const string ScriptBinaryFileExtension = ".tspb";

    /// <summary>   (Immutable) the script binary compressed file extension. </summary>
    public const string ScriptBinaryCompressedFileExtension = ".tspbc";

    /// <summary>   Select script file extension. </summary>
    /// <remarks>   2025-04-05. </remarks>
    /// <param name="fileFormat">           The file format. </param>
    /// <returns>   A string. </returns>
    public static string SelectScriptFileExtension( ScriptFileFormats fileFormat )
    {
        return (ScriptFileFormats.Binary == (fileFormat & ScriptFileFormats.Binary))
            ? (ScriptFileFormats.Compressed == (fileFormat & ScriptFileFormats.Compressed))
              ? cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeCompressedFileExtension
              : cc.isr.VI.Tsp.Script.ScriptInfo.ScriptByteCodeFileExtension
            : (ScriptFileFormats.Compressed == (fileFormat & ScriptFileFormats.Compressed))
              ? cc.isr.VI.Tsp.Script.ScriptInfo.ScriptCompressedFileExtension
              : cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension;

    }

    /// <summary>   Builds script file title. </summary>
    /// <remarks>   2025-04-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <param name="baseTitle">            The base title. </param>
    /// <param name="fileFormat">           (Optional) [<see cref="ScriptFileFormats.None"/>] The
    ///                                     file format. </param>
    /// <param name="scriptVersion">        (Optional) [empty] The release version. Specify the
    ///                                     version only with build files. </param>
    /// <param name="baseModel">            (Optional) [empty] The base model. </param>
    /// <param name="modelMajorVersion">    (Optional) [empty] The model major version. </param>
    /// <returns>   A string. </returns>
    public static string BuildScriptFileTitle( string baseTitle, ScriptFileFormats fileFormat = ScriptFileFormats.None,
        string scriptVersion = "", string baseModel = "", string modelMajorVersion = "" )
    {
        if ( string.IsNullOrWhiteSpace( baseTitle ) )
            throw new ArgumentNullException( nameof( baseTitle ) );

        string title = baseTitle;

        if ( string.IsNullOrWhiteSpace( scriptVersion ) )
            title = $"{title}.{scriptVersion}";


        if ( ScriptFileFormats.Binary == (fileFormat & ScriptFileFormats.Binary) )
        {
            // binary files are always deployed or loaded from a deployed file.
            if ( string.IsNullOrWhiteSpace( baseModel ) )
                throw new ArgumentNullException( nameof( baseModel ) );
            if ( string.IsNullOrWhiteSpace( modelMajorVersion ) )
                throw new ArgumentNullException( nameof( modelMajorVersion ) );
            title = $"{title}.{baseModel}.{modelMajorVersion}";
        }
        return title;
    }

    /// <summary>   Builds script file name. </summary>
    /// <remarks>   2025-04-05. </remarks>
    /// <param name="baseTitle">            The base title. </param>
    /// <param name="fileFormat">           (Optional) [<see cref="ScriptFileFormats.None"/>] The file format. </param>
    /// <param name="scriptVersion">        (Optional) [empty] The release version. Specify the
    ///                                     version only with build files. </param>
    /// <param name="baseModel">            (Optional) [empty] The base model. </param>
    /// <param name="modelMajorVersion">    (Optional) [empty] The model major version. </param>
    /// <returns>   A string. </returns>
    public static string BuildScriptFileName( string baseTitle, ScriptFileFormats fileFormat = ScriptFileFormats.None,
        string scriptVersion = "", string baseModel = "", string modelMajorVersion = "" )
    {
        string title = cc.isr.VI.Tsp.Script.ScriptInfo.BuildScriptFileTitle( baseTitle, fileFormat, scriptVersion, baseModel, modelMajorVersion );
        string ext = cc.isr.VI.Tsp.Script.ScriptInfo.SelectScriptFileExtension( fileFormat );
        return $"{title}{ext}";
    }

    /// <summary>   Query if 'value' includes any of the characters. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="value">        The value. </param>
    /// <param name="characters">   The characters. </param>
    /// <returns>   <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    public static bool IncludesAny( string value, string characters )
    {
        // 2954: changed to [characters] from ^[characters]+$
        System.Text.RegularExpressions.Regex r = new( $"[{characters}]", System.Text.RegularExpressions.RegexOptions.IgnoreCase );
        return r.IsMatch( value );
    }

    /// <summary>   Query if 'value' is valid script name. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="value">    The value. </param>
    /// <returns>   <c>true</c> if valid script name; otherwise <c>false</c> </returns>
    public static bool IsValidScriptName( string value )
    {
        return !string.IsNullOrWhiteSpace( value ) && !IncludesAny( value, Syntax.Tsp.Constants.IllegalScriptNameCharacters );
    }

    /// <summary>   Query if 'value' is valid script file name. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="value">    The value. </param>
    /// <returns>   <c>true</c> if valid script name; otherwise <c>false</c> </returns>
    public static bool IsValidScriptFileName( string value )
    {
        return !string.IsNullOrWhiteSpace( value ) && !IncludesAny( value, Syntax.Tsp.Constants.IllegalFileCharacters );
    }

    /// <summary>   Returns the file size. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="path"> The path. </param>
    /// <returns>   System.Int64. </returns>
    public static long FileSize( string path )
    {
        long size = 0L;
        if ( !string.IsNullOrWhiteSpace( path ) )
        {
            FileInfo info = new( path );
            if ( info.Exists )
                size = info.Length;
        }

        return size;
    }

    /// <summary>   Queries if a given script name exists. </summary>
    /// <remarks>   2024-09-06. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="scriptNames">  List of names of the scripts. </param>
    /// <param name="scriptName">   Specifies the script name. </param>
    /// <returns>   True if it succeeds, false if it fails. </returns>
    public static bool ScriptNameExists( string? scriptNames, string scriptName )
    {
        if ( scriptNames is null ) throw new ArgumentNullException( nameof( scriptNames ) );
        return !string.IsNullOrWhiteSpace( scriptNames ) && !string.IsNullOrWhiteSpace( scriptName )
            && scriptNames.IndexOf( scriptName + ",", 0, StringComparison.OrdinalIgnoreCase ) >= 0;
    }

    /// <summary>   Queries if a given script name exists. </summary>
    /// <remarks>   2025-04-12. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="session">      The session. </param>
    /// <param name="nodeNumber">   The node number. </param>
    /// <returns>   True if the script was embedded, false if it fails. </returns>
    public bool IsEmbedded( Pith.SessionBase session, int nodeNumber )
    {
        return session != null && session.IsEmbeddedScript( this.Name, nodeNumber );
    }

    #endregion

    #region " compression "

    /// <summary>   Returns the compressed code prefix. </summary>
    /// <value> The compressed prefix. </value>
    public static string CompressedPrefix => "<COMPRESSED>";

    /// <summary>   Returns the compressed code suffix. </summary>
    /// <value> The compressed suffix. </value>
    public static string CompressedSuffix => "</COMPRESSED>";

    /// <summary>   Returns a compressed value. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="value">    The string being chopped. </param>
    /// <returns>   Compressed value. </returns>
    public static string Compress( string value )
    {
        if ( string.IsNullOrWhiteSpace( value ) )
        {
            return string.Empty;
        }

        string result = string.Empty;

        // Compress the byte array
        using ( MemoryStream memoryStream = new() )
        {
            using System.IO.Compression.GZipStream compressedStream = new( memoryStream, System.IO.Compression.CompressionMode.Compress );

            // Convert the uncompressed string into a byte array
            byte[] values = System.Text.Encoding.UTF8.GetBytes( value );
            compressedStream.Write( values, 0, values.Length );

            // Don't FLUSH here - it possibly leads to data loss!
            compressedStream.Close();
            byte[] compressedValues = memoryStream.ToArray();

            // Convert the compressed byte array back to a string
            result = Convert.ToBase64String( compressedValues );
            memoryStream.Close();
        }

        return result;
    }

    /// <summary>   Returns the decompressed string of the value. </summary>
    /// <remarks>
    /// David, 2009-04-09, 1.1.3516. Bug fix in getting the size. Changed  memoryStream.Length - 5 to
    /// memoryStream.Length - 4. <para>
    /// David, 2024-09-26: the unit test failed decompressing the entire
    /// file. It might be that the file was read or written partially. </para>
    /// </remarks>
    /// <param name="value">    The string being chopped. </param>
    /// <param name="prefix">   The prefix. </param>
    /// <param name="suffix">   The suffix. </param>
    /// <returns>   Decompressed value. </returns>
    public static string Decompress( string value, string prefix, string suffix )
    {
        string source = string.Empty;
        if ( value.StartsWith( prefix, false, System.Globalization.CultureInfo.CurrentCulture ) )
        {
            int fromIndex = value.IndexOf( prefix, StringComparison.OrdinalIgnoreCase ) + prefix.Length;
            int toIndex = value.IndexOf( suffix, StringComparison.OrdinalIgnoreCase ) - 1;
            source = value.Substring( fromIndex, toIndex - fromIndex + 1 );
            source = FirmwareScriptBase.Decompress( source );
        }
        return source;
    }

    /// <summary>   Returns the decompressed string of the value. </summary>
    /// <remarks>
    /// David, 2009-04-09, 1.1.3516. Bug fix in getting the size. Changed  memoryStream.Length - 5 to
    /// memoryStream.Length - 4. <para>
    /// David, 2024-09-26: the unit test failed decompressing the entire
    /// file. It might be that the file was read or written partially. </para>
    /// </remarks>
    /// <param name="value">    The string being chopped. </param>
    /// <returns>   Decompressed value. </returns>
    public static string Decompress( string value )
    {
        if ( string.IsNullOrWhiteSpace( value ) )
            return string.Empty;

        string result = string.Empty;

        // Convert the compressed string into a byte array
        byte[] compressedValues = Convert.FromBase64String( value );

        // Decompress the byte array
        using ( MemoryStream memoryStream = new( compressedValues ) )
        {
            using System.IO.Compression.GZipStream compressedStream = new( memoryStream, System.IO.Compression.CompressionMode.Decompress );

            // it looks like we are getting a bogus size.
            byte[] sizeBytes = new byte[4];
            memoryStream.Position = memoryStream.Length - 4L;
            _ = memoryStream.Read( sizeBytes, 0, 4 );
            int outputSize = BitConverter.ToInt32( sizeBytes, 0 );
            memoryStream.Position = 0L;
            byte[] values = new byte[outputSize];
            _ = compressedStream.Read( values, 0, outputSize );

            // Convert the decompressed byte array back to a string
            result = System.Text.Encoding.UTF8.GetString( values );
        }

        return result;
    }

    #endregion

    #region " TSP Script Loading Syntax "

    /// <summary>   Decorate a byte code script with the Load String syntax. </summary>
    /// <remarks>   2024-12-05. </remarks>
    /// <param name="source">   Contains the script code line by line. </param>
    /// <returns>   A string. </returns>
    public static string BuildLoadStringSyntax( string source )
    {
        if ( FirmwareScriptBase.isByteCodeSource( source ) )
        {
            if ( source[..50].Trim().StartsWith( "{", true, System.Globalization.CultureInfo.CurrentCulture ) )
            {
                StringBuilder builder = new();
                _ = builder.AppendLine( $"{cc.isr.VI.Syntax.Tsp.Lua.LoadStringCommand}(table.concat(" );
                _ = builder.Append( source );
                _ = builder.AppendLine( "))()" );
                return builder.ToString();
            }
            else
                return source;
        }
        else
            return source;
    }

    /// <summary>   Builds load script syntax. </summary>
    /// <remarks>   2025-04-02. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="source">               Contains the script code line by line. </param>
    /// <param name="scriptName">           Specifies the script name or empty if anonymous script. </param>
    /// <param name="runScriptAfterLoad">   True to run script after load. </param>
    /// <returns>   A string. </returns>
    public static string BuildLoadScriptSyntax( string source, string scriptName, bool runScriptAfterLoad )
    {
        string sourceStart = source[..50].Trim();
        if ( FirmwareScriptBase.isByteCodeSource( source ) )
        {
            if ( sourceStart.StartsWith( "{", true, System.Globalization.CultureInfo.CurrentCulture ) )
            {
                StringBuilder builder = new( source.Length + 512 );
                _ = builder.AppendLine( $"{cc.isr.VI.Syntax.Tsp.Lua.LoadStringCommand}(table.concat(" );
                _ = builder.Append( source );
                _ = builder.AppendLine( "))()" );
                return FirmwareScriptBase.BuildLoadScriptSyntax( builder.ToString(), scriptName, runScriptAfterLoad );
            }
            else if ( sourceStart.StartsWith( cc.isr.VI.Syntax.Tsp.Lua.LoadStringCommand, StringComparison.Ordinal ) )
            {
                StringBuilder builder = new( source.Length + 512 );
                if ( runScriptAfterLoad )
                    _ = builder.AppendLine( $"{cc.isr.VI.Syntax.Tsp.Script.LoadAndRunScriptCommand} {scriptName}" );
                else
                    _ = builder.AppendLine( $"{cc.isr.VI.Syntax.Tsp.Script.LoadScriptCommand} {scriptName}" );
                _ = builder.Append( source );
                _ = builder.AppendLine( cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand );
                return builder.ToString();
            }
            else if ( runScriptAfterLoad && !sourceStart.StartsWith( cc.isr.VI.Syntax.Tsp.Script.LoadAndRunScriptCommand, StringComparison.Ordinal ) )
                throw new InvalidOperationException( $"'{sourceStart}' must start with {cc.isr.VI.Syntax.Tsp.Script.LoadAndRunScriptCommand}" );
            else if ( !runScriptAfterLoad && !sourceStart.StartsWith( cc.isr.VI.Syntax.Tsp.Script.LoadScriptCommand, StringComparison.Ordinal ) )
                throw new InvalidOperationException( $"'{sourceStart}' must start with {cc.isr.VI.Syntax.Tsp.Script.LoadScriptCommand}" );
            else if ( !source[50..].Trim().Contains( cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand, StringComparison.Ordinal ) )
                throw new InvalidOperationException( $"'{source[50..].Trim()}' must contain {cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand}" );
            else
                return source;
        }
        else
        {
            if ( runScriptAfterLoad && sourceStart.StartsWith( cc.isr.VI.Syntax.Tsp.Script.LoadAndRunScriptCommand, StringComparison.Ordinal ) )
            {
                if ( !source[50..].Trim().Contains( cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand, StringComparison.Ordinal ) )
                    throw new InvalidOperationException( $"'{source[50..].Trim()}' must contain {cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand}" );
                else
                    return source;
            }
            else if ( !runScriptAfterLoad && sourceStart.StartsWith( cc.isr.VI.Syntax.Tsp.Script.LoadScriptCommand, StringComparison.Ordinal ) )
            {
                if ( !source[50..].Trim().Contains( cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand, StringComparison.Ordinal ) )
                    throw new InvalidOperationException( $"'{source[50..].Trim()}' must contain {cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand}" );
                else
                    return source;
            }
            else
            {
                StringBuilder builder = new( source.Length + 512 );
                if ( runScriptAfterLoad )
                    _ = builder.AppendLine( $"{cc.isr.VI.Syntax.Tsp.Script.LoadAndRunScriptCommand} {scriptName}" );
                else
                    _ = builder.AppendLine( $"{cc.isr.VI.Syntax.Tsp.Script.LoadScriptCommand} {scriptName}" );
                _ = builder.Append( source );
                _ = builder.AppendLine( cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand );
                return builder.ToString();
            }
        }
    }

    /// <summary>   Gets or sets the script write lines delay. </summary>
    /// <value> The script write lines delay. </value>
    public static TimeSpan WriteLinesDelay { get; set; } = TimeSpan.Zero;

    #endregion

    #region " firmware file "

    /// <summary>   Gets or sets the firmware version of the script. </summary>
    /// <value> The firmware version of the script. </value>
    public string FirmwareVersion { get; set; }

    /// <summary>   Gets or sets the full path name of the folder. </summary>
    /// <value> The full path name of the folder. </value>
    public string FolderPath { get; set; }

    /// <summary>   Gets or sets the script file title. </summary>
    /// <remarks> Typically this is the script name. </remarks>
    /// <value> The title of the script file. </value>
    public string FileTitle { get; set; }

    /// <summary>   Gets or sets the filename of the build file. </summary>
    /// <remarks> Currently the build file includes the file title and script version. </remarks>
    /// <value> The filename of the build file. </value>
    public string BuildFileName => $"{this.FileTitle}.{this.FirmwareVersion}{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}";

    /// <summary>   Gets the filename of the trimmed file. </summary>
    /// <value> The filename of the trimmed file. </value>
    public string TrimmedFileName => $"{this.FileTitle}.{this.FirmwareVersion}{cc.isr.VI.Tsp.Script.ScriptInfo.ScriptFileExtension}";

    /// <summary>   Gets the filename of the deploy file. </summary>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <value> The filename of the deploy file. </value>
    public string DeployFileName
    {
        get
        {
            string title = this.FileTitle;
            if ( ScriptFileFormats.Binary == (this.DeployFileFormat & ScriptFileFormats.Binary) )
            {
                // binary files are always deployed or loaded from a deployed file.
                if ( string.IsNullOrWhiteSpace( this.ModelMask ) )
                    throw new InvalidOperationException( $"The {nameof( this.ModelMask )} is not specified." );
                if ( string.IsNullOrWhiteSpace( this.ModelVersion ) )
                    throw new InvalidOperationException( $"The {nameof( this.ModelVersion )} is not specified." );
                title = $"{title}.{this.ModelMask}.{new Version( this.ModelVersion ).Major}";
            }
            string ext = cc.isr.VI.Tsp.Script.ScriptInfo.SelectScriptFileExtension( this.DeployFileFormat );
            return $"{title}{ext}";
        }
    }

    /// <summary>
    /// Gets or sets the <see cref="ScriptFileFormats"/> of the deploy file. Defaults to uncompressed format.
    /// </summary>
    /// <value> The <see cref="ScriptFileFormats"/> of the resource file. </value>
    public ScriptFileFormats DeployFileFormat { get; set; }

    /// <summary>   Builds the <see cref="ScriptFileFormats"/> of a deployed script. </summary>
    /// <remarks>   2024-08-20. </remarks>
    /// <param name="binaryScript">     True if the file is binary. </param>
    /// <param name="compressedScript"> True if the file is compressed. </param>
    /// <returns>   The ScriptFileFormats. </returns>
    public static ScriptFileFormats BuildScriptFileFormat( bool binaryScript, bool compressedScript )
    {
        ScriptFileFormats scriptFileFormats = ScriptFileFormats.None;
        if ( binaryScript ) scriptFileFormats |= ScriptFileFormats.Binary;
        if ( compressedScript ) scriptFileFormats |= ScriptFileFormats.Compressed;
        return scriptFileFormats;
    }

    /// <summary>
    /// Gets or sets the condition indicating if the script was already exported to file. This property
    /// is set <c>true</c> if the script source is already in the correct format so no new file needs
    /// to be embedded.
    /// </summary>
    /// <value> True if exported to file. </value>
    public bool ExportedToFile { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user script source should be embedded to non-
    /// volatile catalog of user scripts. The Deploy format tells us if the script should be
    /// converted to byte code before saving.
    /// </summary>
    /// <value> True if this user script is to be embedded as a binary user script, false if not. </value>
    public bool SaveToNonVolatileMemory { get; set; }

    /// <summary>   Gets a value indicating whether the convert to byte code. </summary>
    /// <value> True if convert to byte code, false if not. </value>
    public bool ConvertToByteCode => ScriptFileFormats.Binary == (this.DeployFileFormat & ScriptFileFormats.Binary);

    #endregion

    #region " script management "

    /// <summary>   Indicates if the script requires update from file. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>
    /// <c>true</c> if the script requires update from file; otherwise, <c>false</c>.
    /// </returns>
    public bool RequiresReadParseWrite => this.FirmwareVersion.Trim().StartsWith( "+", true, System.Globalization.CultureInfo.CurrentCulture );

    #endregion

    #region " model management "

    /// <summary>   Specifies the instrument firmware version for this script. </summary>
    /// <remarks>
    /// this is required if deploying a byte code script, which might be specific to an instrument model
    /// firmware version.
    /// </remarks>
    /// <value> The model version. </value>
    public string ModelVersion { get; set; }

    /// <summary>   Gets the model major version. </summary>
    /// <value> The model major version. </value>
    public string ModelMajorVersion => $"{new Version( this.ModelVersion ).Major}";

    /// <summary>   Specifies the family of instrument models for this script. </summary>
    /// <value> The model mask. </value>
    public string ModelMask { get; private set; }

    /// <summary>
    /// Checks if the <paramref name="model">model</paramref> matches the
    /// <see cref="ModelMask">mask</see>.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="model">    Actual mode. </param>
    /// <param name="mask">     Model mask using '%' to signify ignored characters and * to specify
    ///                         wildcard suffix. </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="model">model</paramref> matches the
    /// <see cref="ModelMask">mask</see>.
    /// </returns>
    public static bool IsModelMatch( string model, string mask )
    {
        char wildcard = '*';
        char ignore = '%';
        if ( string.IsNullOrWhiteSpace( mask ) )
        {
            return true;
        }
        else if ( string.IsNullOrWhiteSpace( model ) )
        {
            return false;
        }
        else if ( mask.Contains( wildcard.ToString() ) )
        {
            int length = mask.IndexOf( wildcard );
            char[] m = mask[..length].ToCharArray();
            char[] candidate = model[..length].ToCharArray();
            for ( int i = 0, loopTo1 = m.Length - 1; i <= loopTo1; i++ )
            {
                char c = m[i];
                if ( c != ignore && c != candidate[i] )
                {
                    return false;
                }
            }
        }
        else if ( mask.Length != model.Length )
        {
            return false;
        }
        else
        {
            char[] m = mask.ToCharArray();
            char[] candidate = model.ToCharArray();
            for ( int i = 0, loopTo = m.Length - 1; i <= loopTo; i++ )
            {
                char c = m[i];
                if ( c != ignore && c != candidate[i] )
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if the <paramref name="model">model</paramref> matches the
    /// <see cref="ModelMask">mask</see>.
    /// </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="model">    The model. </param>
    /// <returns>
    /// <c>true</c> if the <paramref name="model">model</paramref> matches the
    /// <see cref="ModelMask">mask</see>.
    /// </returns>
    public bool IsModelMatch( string model )
    {
        return IsModelMatch( model, this.ModelMask );
    }

    #endregion

    #region " script specifications "

    /// <summary>
    /// Gets the sentinel indicating if this is a byte code script. This is determined when setting the
    /// source.
    /// </summary>
    /// <value> <c>true</c> if this is a byte code script; otherwise, <c>false</c>. </value>
    public bool isByteCodeScript { get; private set; }

    /// <summary>   Gets or sets the sentinel indicating if this is a Boot script. </summary>
    /// <value> <c>true</c> if this is a Boot script; otherwise, <c>false</c>. </value>
    public bool IsBootScript { get; set; }

    /// <summary>   Gets or sets the sentinel indicating if this is a Primary script. </summary>
    /// <remarks> The primary script is used to provide the following items: <para>
    /// A version getter that can be used to get the overall version of the framework, </para><para>
    /// A script name that is used to determine if any part of the framework was loaded. </para></remarks>
    /// <value> <c>true</c> if this is a Primary script; otherwise, <c>false</c>. </value>
    public bool IsPrimaryScript { get; set; }

    /// <summary>   Gets or sets the sentinel indicating if this is a Support script. </summary>
    /// <value> <c>true</c> if this is a Support script; otherwise, <c>false</c>. </value>
    public bool IsSupportScript { get; set; }

    /// <summary>   Gets the name of the script. </summary>
    /// <value> The name. </value>
    public string Name { get; private set; }

    /// <summary>   Query if 'source' is a compressed source. </summary>
    /// <remarks>   2024-10-11. </remarks>
    /// <param name="source">   specifies the source code for the script. </param>
    /// <returns>   True if compressed source, false if not. </returns>
    public static bool IsCompressedSource( string source )
    {
        return source.StartsWith( CompressedPrefix, false, System.Globalization.CultureInfo.CurrentCulture );
    }

    /// <summary>   Query if 'source' is byte code source. </summary>
    /// <remarks>   2024-10-11. </remarks>
    /// <param name="source">   specifies the source code for the script. </param>
    /// <returns>   True if byte code source, false if not. </returns>
    public static bool isByteCodeSource( string source )
    {
        return source.Contains( @"\27LuaP\0\4\4\4\", StringComparison.Ordinal );
    }

    /// <summary>   Parse source. </summary>
    /// <remarks>   2024-09-23. </remarks>
    /// <param name="value">    The string being chopped. </param>
    /// <returns>   The ScriptFileFormats. </returns>
    private ScriptFileFormats ParseSource( string value )
    {
        ScriptFileFormats sourceFormat = ScriptFileFormats.None;
        bool isByteCodeScript = false;
        string source = string.Empty;
        if ( !this.RequiresReadParseWrite )
        {
            if ( value.StartsWith( CompressedPrefix, false, System.Globalization.CultureInfo.CurrentCulture ) )
            {
                int fromIndex = value.IndexOf( CompressedPrefix, StringComparison.OrdinalIgnoreCase ) + CompressedPrefix.Length;
                int toIndex = value.IndexOf( CompressedSuffix, StringComparison.OrdinalIgnoreCase ) - 1;
                source = value.Substring( fromIndex, toIndex - fromIndex + 1 );
                source = FirmwareScriptBase.Decompress( source );
                sourceFormat |= ScriptFileFormats.Compressed;
            }
            else
                source = value;

            if ( !string.IsNullOrWhiteSpace( this.Source ) )
            {
                isByteCodeScript = FirmwareScriptBase.isByteCodeSource( source );
            }

            if ( !source.EndsWith( " ", true, System.Globalization.CultureInfo.CurrentCulture ) )
                source = source.Insert( source.Length, " " );

            if ( isByteCodeScript )
                sourceFormat |= ScriptFileFormats.Binary;
        }
        this._source = source;
        this.isByteCodeScript = isByteCodeScript;

        // tag file as exported if source format and file format match.
        this.ExportedToFile = sourceFormat == this.DeployFileFormat;
        return sourceFormat;
    }


    /// <summary> Source for the. </summary>
    private string _source;

    /// <summary>   Gets or sets the source for the script. </summary>
    /// <value> The source. </value>
    public string Source
    {
        get => this._source;
        set
        {
            if ( string.IsNullOrWhiteSpace( value ) )
            {
                this._source = value;
                this.isByteCodeScript = false;
                this.ExportedToFile = true;
            }
            else
                _ = this.ParseSource( value );
        }
    }

    #endregion

    #region " load scripts "

    /// <summary>   Builds script loader script. </summary>
    /// <remarks>   2024-09-07. </remarks>
    /// <param name="loadingScriptName">    Name of the loading script. </param>
    /// <param name="nodeNumber">           The node number. </param>
    /// <returns>   A string. </returns>
    public string BuildScriptLoaderScript( string loadingScriptName, int nodeNumber )
    {
        string prefix = $"{cc.isr.VI.Syntax.Tsp.Lua.LoadStringCommand}(table.concat(";
        string suffix = "))()";
        bool binaryDecorationRequire = this.isByteCodeScript && !this.Source.Contains( prefix );
        System.Text.StringBuilder loadCommands = new( this.Source.Length + 512 );
        _ = loadCommands.Append( $"{cc.isr.VI.Syntax.Tsp.Script.LoadAndRunScriptCommand} {loadingScriptName}" );
        _ = loadCommands.AppendLine();
        _ = loadCommands.AppendLine( "do" );
        _ = loadCommands.Append( $"node[{nodeNumber}].dataqueue.add([[" );
        if ( binaryDecorationRequire )
        {
            _ = loadCommands.Append( prefix );
        }

        _ = loadCommands.AppendLine( this.Source );
        if ( binaryDecorationRequire )
        {
            _ = loadCommands.AppendLine( suffix );
        }

        // was _ = loadCommands.Append( $"]]) waitcomplete()" );

        _ = loadCommands.Append( $"]]) waitcomplete({nodeNumber})" );
        _ = loadCommands.AppendLine();
        _ = loadCommands.Append( $"node[{nodeNumber}].execute([[waitcomplete() {this.Name}=script.new(dataqueue.next(),'{this.Name}')]])" );
        _ = loadCommands.AppendLine();
        _ = loadCommands.Append( $"waitcomplete({nodeNumber})" );
        _ = loadCommands.AppendLine( " waitcomplete()" );
        _ = loadCommands.AppendLine( "end" );
        _ = loadCommands.AppendLine( cc.isr.VI.Syntax.Tsp.Script.EndScriptCommand );
        return loadCommands.ToString().TrimEndNewLine();
    }

    #endregion

    #region " read and write "

    /// <summary>   Reads a source. </summary>
    /// <remarks>   2024-11-16. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="scriptsFolderPath">    full path name of the scripts folder file. </param>
    public void ReadSource( string scriptsFolderPath )
    {
        if ( string.IsNullOrWhiteSpace( scriptsFolderPath ) ) throw new ArgumentNullException( nameof( scriptsFolderPath ) );
        string filePath = System.IO.Path.Combine( scriptsFolderPath, this.BuildFileName );
        if ( System.IO.File.Exists( filePath ) )
            this.Source = System.IO.File.ReadAllText( filePath );
        else
            throw new FileNotFoundException( $"{filePath} not found." );
    }

    /// <summary>   Reads the script from the script file. </summary>
    /// <remarks>   2024-07-09. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="filePath"> Specifies the script file path. </param>
    /// <returns>   The script. </returns>
    public static string? ReadScript( string? filePath )
    {
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );
        using StreamReader? tspFile = FirmwareFileInfo.OpenScriptFile( filePath! );
        string? source = tspFile?.ReadToEnd();

        if ( source is null || string.IsNullOrWhiteSpace( source ) )
            throw new InvalidOperationException( $"Failed reading script;. file '{filePath}' includes no source." );
        else if ( source.Length < 2 )
            throw new InvalidOperationException( $"Failed reading script;. file '{filePath}' includes no source." );
        else if ( IsCompressedSource( source ) )
            source = Decompress( source, FirmwareScriptBase.CompressedPrefix, FirmwareScriptBase.CompressedSuffix );

        return source;
    }

    /// <summary>   Writes the script to file. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="source">   specifies the source code for the script. </param>
    /// <param name="filePath"> Specifies the script file path. </param>
    public static void WriteScript( string? source, string? filePath )
    {
        if ( source is null ) throw new ArgumentNullException( nameof( source ) );
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );
        using StreamWriter tspFile = new( filePath );
        tspFile.Write( source );
    }

    /// <summary>   Reads the scripts, parses them and saves them to file. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="FileNotFoundException">        Thrown when the requested file is not
    ///                                                 present. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when operation failed to execute. </exception>
    /// <param name="folderPath">       Specifies the folder where scripts are stored. </param>
    /// <param name="buildFileName">    The filename of the build file. </param>
    /// <param name="trimmedFileName">  The filename of the trimmed file. </param>
    /// <param name="retainOutline">    (Optional) Specifies if the code outline is retained or
    ///                                 trimmed. </param>
    public static void ReadParseWriteScript( string? folderPath, string buildFileName, string trimmedFileName, bool retainOutline = false )
    {
        if ( string.IsNullOrWhiteSpace( folderPath ) ) throw new ArgumentNullException( nameof( folderPath ) );

        // check if file exists.
        if ( FirmwareScriptBase.FileSize( folderPath! ) <= 2L )
            throw new System.IO.FileNotFoundException( "Script file not found", folderPath );
        else
        {
            string? scriptSource = ReadScript( Path.Combine( folderPath, buildFileName ) );
            if ( string.IsNullOrWhiteSpace( scriptSource ) )
                throw new InvalidOperationException( $"Failed reading script;. file '{folderPath}' includes no source." );
            else
            {
                scriptSource = Lua.TrimLuaSourceCode( scriptSource!, retainOutline );
                if ( string.IsNullOrWhiteSpace( scriptSource ) )
                    throw new InvalidOperationException( $"Failed reading script;. parsed script from '{folderPath}' is empty." );
                else
                {
                    WriteScript( scriptSource, Path.Combine( folderPath, trimmedFileName ) );
                }
            }
        }
    }

    /// <summary>   Reads the scripts, parses them and saves them to file. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments
    ///                                                 are null. </exception>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="scripts">          Specifies the collection of scripts. </param>
    /// <param name="retainOutline">    (Optional) Specifies if the code outline is retained or
    ///                                 trimmed. </param>
    public static void ReadParseWriteScripts( FirmwareScriptBaseCollection<FirmwareScriptBase> scripts, bool retainOutline = false )
    {
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );
        foreach ( FirmwareScriptBase script in scripts )
        {
            if ( string.IsNullOrWhiteSpace( script.Name ) )
                throw new InvalidOperationException( $"{nameof( FirmwareScriptBase )}.{nameof( FirmwareScriptBase.Name )} not specified for script." );
            else if ( script.RequiresReadParseWrite )
            {
                if ( string.IsNullOrWhiteSpace( script.FolderPath ) )
                    throw new InvalidOperationException( $"{nameof( FirmwareScriptBase )}.{nameof( FirmwareScriptBase.FolderPath )} not specified for script." );
                else if ( string.IsNullOrWhiteSpace( script.BuildFileName ) )
                    throw new InvalidOperationException( $"{nameof( FirmwareScriptBase )}.{nameof( FirmwareScriptBase.BuildFileName )} not specified for script." );
                else if ( string.IsNullOrWhiteSpace( script.TrimmedFileName ) )
                    throw new InvalidOperationException( $"{nameof( FirmwareScriptBase )}.{nameof( FirmwareScriptBase.TrimmedFileName )} not specified for script." );
                else
                    FirmwareScriptBase.ReadParseWriteScript( script.FolderPath, script.BuildFileName, script.TrimmedFileName, retainOutline );
            }
        }
    }

    #endregion
}

/// <summary>
/// A <see cref="System.Collections.ObjectModel.KeyedCollection{TKey, TItem}">collection</see> of
/// <see cref="FirmwareScriptBase">script entity</see>
/// items keyed by the <see cref="FirmwareScriptBase.Name">name.</see>
/// </summary>
/// <remarks>
/// David, 2009-03-02, 3.0.3348. <para>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.</para><para>
/// Licensed under The MIT License.</para>
/// </remarks>
[CLSCompliant( false )]
public class ScriptFirmwareCollection : FirmwareScriptBaseCollection<FirmwareScriptBase>
{
    /// <summary>   Gets key for item. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="item"> The item. </param>
    /// <returns>   The key for item. </returns>
    protected override string GetKeyForItem( FirmwareScriptBase item )
    {
        return base.GetKeyForItem( item );
    }
}

