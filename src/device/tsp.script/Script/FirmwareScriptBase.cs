using System.Text;
using cc.isr.Std.TrimExtensions;
using cc.isr.VI.Syntax.Tsp;

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
        this.ModelMask = string.Empty;
        this.ResourceFileName = string.Empty;
        this._namespaceList = string.Empty;
        this.Namespaces = [];
        this.TopNamespace = string.Empty;
        this.FileName = string.Empty;
        this.FolderPath = string.Empty;
        this.ApplyNamespaceList( "" );
        this.ReleasedFirmwareVersion = string.Empty;
        this._source = string.Empty;
    }

    /// <summary>   Constructs this class. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="name">         Specifies the script name. </param>
    /// <param name="modelMask">    Specifies the model families for this script. </param>
    protected FirmwareScriptBase( string name, string modelMask ) : this()
    {
        this.Name = name;
        this.ModelMask = modelMask;
    }

    #endregion

    #region " static "

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

    /// <summary>   Builds the script source from naked binary source. </summary>
    /// <remarks>   2024-12-05. </remarks>
    /// <param name="source">   Contains the script code line by line. </param>
    /// <returns>   A string. </returns>
    public static string BuildScriptIfNakedBinarySource( string source )
    {
        if ( source[..50].Trim().StartsWith( "{", true, System.Globalization.CultureInfo.CurrentCulture ) )
        {
            StringBuilder builder = new();
            _ = builder.AppendLine( "loadstring(table.concat(" );
            _ = builder.AppendLine( source );
            _ = builder.AppendLine( "))()" );
            return builder.ToString();
        }
        else
            return source;
    }

    /// <summary>   Gets or sets the script write lines delay. </summary>
    /// <value> The script write lines delay. </value>
    public static TimeSpan WriteLinesDelay { get; set; } = TimeSpan.Zero;

    #endregion

    #region " firmware "

    /// <summary>   Gets or sets the released firmware version. </summary>
    /// <value> The released firmware version. </value>
    public string ReleasedFirmwareVersion { get; set; }

    /// <summary>   Gets or sets the full pathname of the folder. </summary>
    /// <value> The full pathname of the folder. </value>
    public string FolderPath { get; set; }

    /// <summary>   Gets or sets the filename of the script. </summary>
    /// <value> The name of the script file. </value>
    public string FileName { get; set; }

    /// <summary>   Gets the full pathname of the file. </summary>
    /// <value> The full pathname of the file. </value>
    public string FilePath => Path.Combine( this.FolderPath, this.FileName );

    /// <summary>
    /// Gets or sets the filename of the resource file, which includes the script source in the <see cref="ResourceFileFormat"/>.
    /// The resource filename is build based on the <see cref="FileName"/> by adding the <see cref="NodeEntityBase.ModelFamilyResourceFileSuffix"/>
    /// to the <see cref="FileName"/>.
    /// </summary>
    /// <value> The filename of the resource file. </value>
    public string ResourceFileName { get; set; }

    /// <summary>   Gets the full pathname of the resource file. </summary>
    /// <value> The full pathname of the resource file. </value>
    public string ResourceFilePath => Path.Combine( this.FolderPath, this.ResourceFileName );

    /// <summary>
    /// Gets or sets the format of the resource file. Defaults to uncompressed format.
    /// </summary>
    /// <value> The format of the resource file. </value>
    public ScriptFileFormats ResourceFileFormat { get; set; }

    /// <summary>   Builds the <see cref="ScriptFileFormats"/>. </summary>
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

    /// <summary>   Builds script file name. </summary>
    /// <remarks>   2024-11-16. </remarks>
    /// <param name="title">                    The title of the script. </param>
    /// <param name="binaryScript">             True if the file is binary. </param>
    /// <param name="firmwareVersion">          The firmware version. </param>
    /// <param name="instrumentVersionInfo">    Information describing the instrument version. </param>
    /// <param name="extension">                (Optional) (.tsp) The extension. </param>
    /// <returns>   A string. </returns>
    public static string BuildScriptFileName( string title, bool binaryScript, System.Version firmwareVersion, VersionInfoBase instrumentVersionInfo, string extension = ".tsp" )
    {
        StringBuilder fileName = new( title );
        _ = fileName.Append( "." );
        _ = fileName.Append( firmwareVersion.Build );

        if ( binaryScript )
        {
            _ = fileName.Append( "." );
            _ = fileName.Append( instrumentVersionInfo.ModelFamily );
            _ = fileName.Append( "." );
            _ = fileName.Append( instrumentVersionInfo.FirmwareVersion!.Major );
        }
        _ = fileName.Append( extension );
        return fileName.ToString();
    }

    /// <summary>
    /// Gets or sets the condition indicating if the script was already saved to file. This property
    /// is set <c>true</c> if the script source is already in the correct format so no new file needs
    /// to be saved.
    /// </summary>
    /// <value> The saved to file. </value>
    public bool SavedToFile { get; set; }

    /// <summary>   Gets a value indicating whether the user script source should be converted
    ///             to a binary script when saved in the non-volatile catalog of user scripts. </summary>
    /// <value> True if this user script is to be saved as a binary user script, false if not. </value>
    public bool SaveAsBinary { get; set; }

    #endregion

    #region " script management "

    /// <summary>   Indicates if the script requires update from file. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>
    /// <c>true</c> if the script requires update from file; otherwise, <c>false</c>.
    /// </returns>
    public bool RequiresReadParseWrite => this.ReleasedFirmwareVersion.Trim().StartsWith( "+", true, System.Globalization.CultureInfo.CurrentCulture );

    #endregion

    #region " model management "

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
    /// Gets the sentinel indicating if this is a binary script. This is determined when setting the
    /// source.
    /// </summary>
    /// <value> <c>true</c> if this is a binary script; otherwise, <c>false</c>. </value>
    public bool IsBinaryScript { get; private set; }

    /// <summary>   Gets or sets the sentinel indicating if this is a Boot script. </summary>
    /// <value> <c>true</c> if this is a Boot script; otherwise, <c>false</c>. </value>
    public bool IsBootScript { get; set; }

    /// <summary>   Gets or sets the sentinel indicating if this is a Primary script. </summary>
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

    /// <summary>   Query if 'source' is binary source. </summary>
    /// <remarks>   2024-10-11. </remarks>
    /// <param name="source">   specifies the source code for the script. </param>
    /// <returns>   True if binary source, false if not. </returns>
    public static bool IsBinarySource( string source )
    {
        string snippet = source[..50].Trim();
        return snippet.StartsWith( "{", true, System.Globalization.CultureInfo.CurrentCulture )
            || snippet.StartsWith( "loadstring", true, System.Globalization.CultureInfo.CurrentCulture )
            || snippet.StartsWith( "loadscript", true, System.Globalization.CultureInfo.CurrentCulture );
    }

    /// <summary>   Parse source. </summary>
    /// <remarks>   2024-09-23. </remarks>
    /// <param name="value">    The string being chopped. </param>
    /// <returns>   The ScriptFileFormats. </returns>
    private ScriptFileFormats ParseSource( string value )
    {
        ScriptFileFormats sourceFormat = ScriptFileFormats.None;
        bool isBinaryScript = false;
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
                string snippet = source[..50].Trim();
                isBinaryScript = snippet.StartsWith( "{", true, System.Globalization.CultureInfo.CurrentCulture )
                    || snippet.StartsWith( "loadstring", true, System.Globalization.CultureInfo.CurrentCulture )
                    || snippet.StartsWith( "loadscript", true, System.Globalization.CultureInfo.CurrentCulture );
            }

            if ( !source.EndsWith( " ", true, System.Globalization.CultureInfo.CurrentCulture ) )
                source = source.Insert( source.Length, " " );

            if ( isBinaryScript )
                sourceFormat |= ScriptFileFormats.Binary;
        }
        this._source = source;
        this.IsBinaryScript = isBinaryScript;

        // tag file as saved if source format and file format match.
        this.SavedToFile = sourceFormat == this.ResourceFileFormat;
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
                this.IsBinaryScript = false;
                this.SavedToFile = true;
            }
            else
                _ = this.ParseSource( value );
        }
    }

    /// <summary>   Applies the namespace list described by value. </summary>
    /// <remarks>   2024-09-06. </remarks>
    /// <param name="value">    The string being chopped. </param>
    private void ApplyNamespaceList( string value )
    {
        if ( string.IsNullOrWhiteSpace( value ) )
            value = string.Empty;
        this._namespaceList = value;
        this.Namespaces = string.IsNullOrWhiteSpace( value ) ? [] : this.NamespaceList.Split( ',' );
        this.TopNamespace = this.Namespaces is null || this.Namespaces.Length == 0 ? string.Empty : this.Namespaces[0];
    }

    /// <summary> List of namespaces. </summary>
    private string _namespaceList;

    /// <summary>   Gets or sets a list of namespaces. </summary>
    /// <value> A List of namespaces. </value>
    public string NamespaceList
    {
        get => this._namespaceList;
        set => this.ApplyNamespaceList( value );
    }

    /// <summary>   Gets the namespaces. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <returns>   A list of. </returns>
    public string[] Namespaces { get; private set; }

    /// <summary>   Gets or sets the top namespace. </summary>
    /// <value> The top namespace. </value>
    public string TopNamespace { get; private set; }

    #endregion

    #region " load scripts "

    /// <summary>   Builds script loader script. </summary>
    /// <remarks>   2024-09-07. </remarks>
    /// <param name="loadingScriptName">    Name of the loading script. </param>
    /// <param name="nodeNumber">           The node number. </param>
    /// <returns>   A string. </returns>
    public string BuildScriptLoaderScript( string loadingScriptName, int nodeNumber )
    {
        string prefix = "loadstring(table.concat(";
        string suffix = "))()";
        bool binaryDecorationRequire = this.IsBinaryScript && !this.Source.Contains( prefix );
        System.Text.StringBuilder loadCommands = new( this.Source.Length + 512 );
        _ = loadCommands.Append( $"loadandrunscript {loadingScriptName}" );
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
        _ = loadCommands.AppendLine( "endscript" );
        return loadCommands.ToString().TrimEndNewLine();
    }

    #endregion

    #region " read and write "

    /// <summary>   Reads a source. </summary>
    /// <remarks>   2024-11-16. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
    /// <param name="scriptsFolderPath">    Full pathname of the scripts folder file. </param>
    public void ReadSource( string scriptsFolderPath )
    {
        if ( string.IsNullOrWhiteSpace( scriptsFolderPath ) ) throw new ArgumentNullException( nameof( scriptsFolderPath ) );
        string filePath = System.IO.Path.Combine( scriptsFolderPath, this.ResourceFileName );
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
        return tspFile?.ReadToEnd();
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
    /// <param name="filePath">         Specifies the folder where scripts are stored. </param>
    /// <param name="extension">        (Optional) The extension. </param>
    /// <param name="retainOutline">    (Optional) Specifies if the code outline is retained or
    ///                                 trimmed. </param>
    public static void ReadParseWriteScript( string? filePath, string extension = ".debug", bool retainOutline = false )
    {
        if ( string.IsNullOrWhiteSpace( filePath ) ) throw new ArgumentNullException( nameof( filePath ) );

        // check if file exists.
        if ( FirmwareScriptBase.FileSize( filePath! ) <= 2L )
            throw new System.IO.FileNotFoundException( "Script file not found", filePath );
        else
        {
            string? scriptSource = ReadScript( filePath );
            if ( string.IsNullOrWhiteSpace( scriptSource ) )
                throw new InvalidOperationException( $"Failed reading script;. file '{filePath}' includes no source." );
            else
            {
                scriptSource = Lua.TrimLuaSourceCode( scriptSource!, retainOutline );
                if ( string.IsNullOrWhiteSpace( scriptSource ) )
                    throw new InvalidOperationException( $"Failed reading script;. parsed script from '{filePath}' is empty." );
                else
                {
                    filePath += extension;
                    WriteScript( scriptSource, filePath );
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
    /// <param name="extension">        (Optional) The extension. </param>
    /// <param name="retainOutline">    (Optional) Specifies if the code outline is retained or
    ///                                 trimmed. </param>
    public static void ReadParseWriteScripts( FirmwareScriptBaseCollection<FirmwareScriptBase> scripts, string extension = ".debug", bool retainOutline = false )
    {
        if ( scripts is null ) throw new ArgumentNullException( nameof( scripts ) );
        foreach ( FirmwareScriptBase script in scripts )
        {
            if ( script.RequiresReadParseWrite )
            {
                if ( string.IsNullOrWhiteSpace( script.FilePath ) )
                    throw new InvalidOperationException( $"File not specified for script '{script.Name}'." );
                else
                    FirmwareScriptBase.ReadParseWriteScript( script.FilePath, extension, retainOutline );
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

