// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Collections.ObjectModel;
using System.ComponentModel;
using cc.isr.Std.TrimExtensions;

namespace cc.isr.VI.Tsp.Script;
/// <summary>   A script information. </summary>
/// <remarks>   2025-04-15. </remarks>
public class ScriptInfo
{
    #region " construction "

    /// <summary>   Constructor. </summary>
    /// <remarks>   2025-09-19. </remarks>
    /// <param name="compressor">   The compressor. </param>
    /// <param name="encryptor">    The encryptor. </param>
    public ScriptInfo( IScriptCompressor compressor, IScriptEncryptor encryptor )
    {
        this.Encryptor = encryptor;
        this.Compressor = compressor;
    }

    /// <summary>   Copy constructor. </summary>
    /// <remarks>   2025-04-25. </remarks>
    /// <param name="script">   The script. </param>
    public ScriptInfo( ScriptInfo script )
    {
        this.BuiltFileName = script.BuiltFileName;
        this.Compressor = script.Compressor;
        this.EmbedAsByteCode = script.EmbedAsByteCode;
        this.EmbeddedVersion = script.EmbeddedVersion;
        this.Encryptor = script.Encryptor;
        this.DeployResourceFileFormat = script.DeployResourceFileFormat;
        this.DeployResourceFileName = script.DeployResourceFileName;
        this.FileFormat = script.FileFormat;
        this.FileName = script.FileName;
        this.IsAutoexec = script.IsAutoexec;
        this.IsEncrypted = script.IsEncrypted;
        this.IsPrimary = script.IsPrimary;
        this.IsSupport = script.IsSupport;
        this.NextVersion = script.NextVersion;
        this.PriorVersion = script.PriorVersion;
        this.RequireChunkName = script.RequireChunkName;
        this.RequiredChunkName = script.RequiredChunkName;
        this.ScriptStatus = script.ScriptStatus;
        this.Title = script.Title;
        this.TrimmedFileName = script.TrimmedFileName;
        this.VersionGetter = script.VersionGetter;
        this.VersionStatus = script.VersionStatus;
    }

    #endregion

    #region " script naming "

    /// <summary>   (Immutable) the script file extension. </summary>
    public const string ScriptFileExtension = ".tsp";

    /// <summary>   (Immutable) the script encrypted file extension. </summary>
    public const string ScriptEncryptedFileExtension = ".tspe";

    /// <summary>   (Immutable) the script compressed file extension. </summary>
    public const string ScriptCompressedFileExtension = ".tspc";

    /// <summary>   (Immutable) the script compressed and encrypted file extension. </summary>
    public const string ScriptCompressedEncryptedFileExtension = ".tspce";

    /// <summary>   (Immutable) the script byte code file extension. </summary>
    public const string ScriptByteCodeFileExtension = ".tspb";

    /// <summary>   (Immutable) the script byte code and encrypted file extension. </summary>
    public const string ScriptByteCodeEncryptedFileExtension = ".tspbe";

    /// <summary>   (Immutable) the script byte code compressed file extension. </summary>
    public const string ScriptByteCodeCompressedFileExtension = ".tspbc";

    /// <summary>   (Immutable) the script byte code compressed and encrypted file extension. </summary>
    public const string ScriptByteCodeCompressedEncryptedFileExtension = ".tspbce";

    /// <summary>   Select script file extension. </summary>
    /// <remarks>   2025-04-05. </remarks>
    /// <param name="fileFormat">   The script file format. </param>
    /// <returns>   A string. </returns>
    public static string SelectScriptFileExtension( ScriptFormats fileFormat )
    {
        return (ScriptFormats.ByteCode == (fileFormat & ScriptFormats.ByteCode))
            ? (ScriptFormats.Compressed == (fileFormat & ScriptFormats.Compressed))
              ? (ScriptFormats.Encrypted == (fileFormat & ScriptFormats.Encrypted))
                 ? ScriptInfo.ScriptByteCodeCompressedEncryptedFileExtension
                 : ScriptInfo.ScriptByteCodeCompressedFileExtension
              : (ScriptFormats.Encrypted == (fileFormat & ScriptFormats.Encrypted))
                 ? ScriptInfo.ScriptByteCodeEncryptedFileExtension
                 : ScriptInfo.ScriptByteCodeFileExtension
            : (ScriptFormats.Compressed == (fileFormat & ScriptFormats.Compressed))
              ? (ScriptFormats.Encrypted == (fileFormat & ScriptFormats.Encrypted))
                 ? ScriptInfo.ScriptCompressedEncryptedFileExtension
                 : ScriptInfo.ScriptCompressedFileExtension
              : (ScriptFormats.Encrypted == (fileFormat & ScriptFormats.Encrypted))
                 ? ScriptInfo.ScriptEncryptedFileExtension
                 : ScriptInfo.ScriptFileExtension;
    }

    /// <summary>   Parse the script format from the provided file extension. </summary>
    /// <remarks>   2025-09-30. </remarks>
    /// <param name="fileExtension">    The file extension. </param>
    /// <returns>   The <see cref="ScriptFormats"/>. </returns>
    public static ScriptFormats ParseFileFormats( string fileExtension )
    {
        return fileExtension.ToLowerInvariant() switch
        {
            ScriptInfo.ScriptFileExtension => ScriptFormats.None,
            ScriptInfo.ScriptEncryptedFileExtension => ScriptFormats.Encrypted,
            ScriptInfo.ScriptCompressedFileExtension => ScriptFormats.Compressed,
            ScriptInfo.ScriptCompressedEncryptedFileExtension => ScriptFormats.Compressed | ScriptFormats.Encrypted,
            ScriptInfo.ScriptByteCodeFileExtension => ScriptFormats.ByteCode,
            ScriptInfo.ScriptByteCodeEncryptedFileExtension => ScriptFormats.ByteCode | ScriptFormats.Encrypted,
            ScriptInfo.ScriptByteCodeCompressedFileExtension => ScriptFormats.ByteCode | ScriptFormats.Compressed,
            ScriptInfo.ScriptByteCodeCompressedEncryptedFileExtension => ScriptFormats.ByteCode | ScriptFormats.Compressed | ScriptFormats.Encrypted,
            _ => ScriptFormats.None,
        };
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
    /// <param name="fileFormat">           (Optional) [<see cref="ScriptFormats.None"/>] The
    ///                                     script file format. </param>
    /// <param name="scriptVersion">        (Optional) [empty] The release version. Specify the
    ///                                     version only with build files. </param>
    /// <param name="modelFamily">          (Optional) [empty] The model family, e.g., 2600A. </param>
    /// <param name="modelMajorVersion">    (Optional) [empty] The model major version. </param>
    /// <returns>   A string. </returns>
    public static string BuildScriptFileTitle( string baseTitle, ScriptFormats fileFormat = ScriptFormats.None,
        string scriptVersion = "", string modelFamily = "", string modelMajorVersion = "" )
    {
        if ( string.IsNullOrWhiteSpace( baseTitle ) )
            throw new ArgumentNullException( nameof( baseTitle ) );

        string title = string.IsNullOrWhiteSpace( scriptVersion )
            ? baseTitle
            : $"{baseTitle}.{scriptVersion}";


        if ( ScriptFormats.ByteCode == (fileFormat & ScriptFormats.ByteCode) )
        {
            // byte code files are always deployed or loaded from a deployed file.
            if ( string.IsNullOrWhiteSpace( modelFamily ) )
                throw new ArgumentNullException( nameof( modelFamily ) );
            if ( string.IsNullOrWhiteSpace( modelMajorVersion ) )
                throw new ArgumentNullException( nameof( modelMajorVersion ) );
            title = $"{title}.{modelFamily}.{modelMajorVersion}";
        }
        return title;
    }

    /// <summary>   Builds script file name. </summary>
    /// <remarks>   2025-04-05. </remarks>
    /// <param name="baseTitle">            The base title. </param>
    /// <param name="fileFormat">           (Optional) [<see cref="ScriptFormats.None"/>] The script
    ///                                     file format. </param>
    /// <param name="scriptVersion">        (Optional) [empty] The release version. Specify the
    ///                                     version only with build files. </param>
    /// <param name="modelFamily">          (Optional) [empty] The model family, e.g., 2600A. </param>
    /// <param name="modelMajorVersion">    (Optional) [empty] The model major version. </param>
    /// <returns>   A string. </returns>
    public static string BuildScriptFileName( string baseTitle, ScriptFormats fileFormat = ScriptFormats.None,
        string scriptVersion = "", string modelFamily = "", string modelMajorVersion = "" )
    {
        string title = ScriptInfo.BuildScriptFileTitle( baseTitle, fileFormat, scriptVersion, modelFamily, modelMajorVersion );
        string ext = ScriptInfo.SelectScriptFileExtension( fileFormat );
        return $"{title}{ext}";
    }

    /// <summary>   Query if 'value' includes any of the characters. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="value">        The value. </param>
    /// <param name="characters">   The characters. </param>
    /// <returns>   <c>true</c> if it succeeds; otherwise <c>false</c> </returns>
    private static bool IncludesAny( string value, string characters )
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

    #endregion

    #region " Implementation "

    /// <summary>   Gets the encryption engine. </summary>
    /// <value> The encryptor. </value>
    [Description( "Implements encryption" )]
    public virtual IScriptEncryptor Encryptor { get; }

    /// <summary>   Implements compression. </summary>
    /// <value> The compressor. </value>
    [Description( "Implements compression" )]
    public virtual IScriptCompressor Compressor { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this script automatically executes.
    /// </summary>
    /// <value> True if this object is automatic execute, false if not. </value>
    [Description( "Indicates whether this script automatically executes [false]" )]
    public virtual bool IsAutoexec { get; set; } = false;

    /// <summary>   Indicates whether this script is a primary deploy script [true]. </summary>
    /// <value> True if this object is primary, false if not. </value>
    [Description( "Indicates whether this script is a primary deploy script [true]" )]
    public virtual bool IsPrimary { get; set; } = true;

    /// <summary>   Indicates whether this script is a support script [false]. The support script is used to determine if the script is certified.  </summary>
    /// <value> True if this object is support, false if not. </value>
    [Description( "Indicates whether this script is a support script [false]" )]
    public virtual bool IsSupport { get; set; } = true;

    /// <summary>   Indicates whether this script is encrypted [false]. </summary>
    /// <value> True if this object is encrypted, false if not. </value>
    [Description( "Indicates whether this script is encrypted [false]" )]
    public virtual bool IsEncrypted { get; set; } = false;

    /// <summary>   Indicates whether this script is to be embedded as byte code [true]. </summary>
    /// <value> True if the script is to be embedded as byte code, false if not. </value>
    [Description( "Indicates whether this script is to be embedded as byte code [true]" )]
    public virtual bool EmbedAsByteCode { get; set; } = true;

    /// <summary>   Gets or sets the title of the script. </summary>
    /// <value> The name and the file title of the script. </value>
    [Description( "The name and the file title of the script []" )]
    public virtual string Title { get; set; } = string.Empty;

    /// <summary>   Gets or sets the prior version of the script. </summary>
    /// <remarks>
    /// This version might be the same as the <see cref="EmbeddedVersion"/> that is currently
    /// installed in the instrument.
    /// </remarks>
    /// <value> The prior version of the script. </value>
    [Description( "The prior version of the script []" )]
    public virtual string PriorVersion { get; set; } = string.Empty;

    /// <summary>
    /// The next version of the script that might be installed to replace the prior script.
    /// </summary>
    /// <remarks>
    /// This the version of the new script to be installed. Thus, this is the version of the built
    /// script.
    /// </remarks>
    /// <value> The next version of the script. </value>
    [Description( "The next version of the script that might be installed to replace this script" )]
    public virtual string NextVersion { get; set; } = string.Empty;

    /// <summary>   Gets or sets the version of the embedded script as read from the instrument. </summary>
    /// <value> The version of the embedded script as read from the instrument. </value>
    [Description( "The version of the embedded script as read from the instrument" )]
    public virtual string EmbeddedVersion { get; set; } = string.Empty;

    /// <summary>   The built file name [isr_ttm_autoexec.xxxx.tsp]. </summary>
    /// <value> The filename of the build file. </value>
    [Description( "The build file name []" )]
    public virtual string BuiltFileName { get; set; } = string.Empty;

    /// <summary>   The trimmed file name. </summary>
    /// <value> The filename of the trimmed file. </value>
    [Description( "The trimmed file name []" )]
    public virtual string TrimmedFileName { get; set; } = string.Empty;

    /// <summary>   Gets or sets the deployed resource file name. </summary>
    /// <value> The deployed resource file name. </value>
    public virtual string DeployResourceFileName { get; private set; } = string.Empty;

    /// <summary>   Gets or sets the deployed resource file format. </summary>
    /// <value> The deployed resource file format. </value>
    public virtual ScriptFormats DeployResourceFileFormat { get; private set; } = ScriptFormats.None;

    /// <summary>   Sets deployed resource file format. </summary>
    /// <remarks>   2025-11-08. </remarks>
    /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
    ///                                                 invalid. </exception>
    /// <param name="format">   Describes the <see cref="ScriptFormats"/> in which to export the
    ///                         script file to be included as a resource for deployment. </param>
    public virtual void SetDeployResourceFileFormat( ScriptFormats format )
    {
        if ( format.HasFlag( ScriptFormats.ByteCode ) )
            throw new InvalidOperationException( "Cannot set the deploy file format to byte code." );
        this.DeployResourceFileFormat = format;
        this.DeployResourceFileName = ScriptInfo.BuildScriptFileName( this.Title, format );
    }

    /// <summary>   Gets or sets the script file name as stored on disk for import and export. </summary>
    /// <value> The script file name. </value>
    public virtual string FileName { get; set; } = string.Empty;

    /// <summary>   Gets or sets the script file format as stored on disk for import and export. </summary>
    /// <value> The script file format. </value>
    public virtual ScriptFormats FileFormat { get; set; } = ScriptFormats.None;

    /// <summary>   Gets or sets the Version Getter function of the script. </summary>
    /// <value> The version getter function of the script. </value>
    [Description( "The version getter method of the script []" )]
    public virtual string VersionGetter { get; set; } = string.Empty;

    /// <summary>   The version getter element the script [_G.isr_ttm_autoexec.getVersion]. </summary>
    /// <value> The version getter element. </value>
    [Description( "The version getter method object of the script []" )]
    public virtual string VersionGetterElement => string.IsNullOrWhiteSpace( this.VersionGetter ) ? string.Empty : this.VersionGetter.TrimEnd( ['(', ')'] );

    /// <summary>
    /// The name of a pre-requisite script for this script.
    /// </summary>
    /// <value> The name of the require chunk. </value>
    [Description( "The name of a pre-requisite script for this script []" )]
    public virtual string RequireChunkName { get; set; } = string.Empty;

    /// <summary>
    /// The name of this scrip as would be required independent scripts.
    /// </summary>
    /// <value> The name of the required chunk. </value>
    [Description( "The name of this scrip as would be required independent scripts []" )]
    public virtual string RequiredChunkName { get; set; } = string.Empty;

    /// <summary>   Gets or sets the script status. </summary>
    /// <value> The script status. </value>
    public virtual ScriptStatuses ScriptStatus { get; set; } = ScriptStatuses.Unknown;

    /// <summary>   Gets or sets the version status. </summary>
    /// <value> The version status. </value>
    public virtual FirmwareVersionStatus VersionStatus { get; set; } = FirmwareVersionStatus.Unknown;

    #endregion

    #region " status report "

    /// <summary>   Builds script state report. </summary>
    /// <remarks>   2025-04-26. </remarks>
    /// <returns>   A string. </returns>
    public string BuildScriptStateReport()
    {
        System.Text.StringBuilder builder = new();

        if ( string.IsNullOrWhiteSpace( this.Title ) )
            return "Firmware script title is empty";

        _ = builder.AppendLine( $"Info for firmware '{this.Title}':" );
        _ = builder.AppendLine( $"\tVersions:" );
        _ = builder.AppendLine( $"\t\t     New: {(string.IsNullOrEmpty( this.NextVersion ) ? "unknown" : this.NextVersion)}." );
        _ = builder.AppendLine( $"\t\t   Prior: {(string.IsNullOrEmpty( this.PriorVersion ) ? "unknown" : this.PriorVersion)}." );
        _ = builder.AppendLine( $"\t\tEmbedded: {(string.IsNullOrEmpty( this.EmbeddedVersion ) ? "unknown" : this.EmbeddedVersion)}." );
        if ( string.IsNullOrWhiteSpace( this.EmbeddedVersion ) )
            _ = builder.AppendLine( $"\t{(string.IsNullOrWhiteSpace( this.VersionGetterElement ) ? "has a" : "does not have a")} firmware version getter." );

        _ = builder.AppendLine( $"\t'{this.Title}' firmware state:" );
        if ( ScriptStatuses.Loaded == (this.ScriptStatus & ScriptStatuses.Loaded) )
        {
            if ( ScriptStatuses.ByteCode == (this.ScriptStatus & ScriptStatuses.ByteCode) )
                _ = builder.AppendLine( $"\t\t   Loaded: Byte code." );
            else
                _ = builder.AppendLine( $"\t\t  Loaded: Plain code." );

            if ( ScriptStatuses.Activated == (this.ScriptStatus & ScriptStatuses.Activated) )
            {
                _ = builder.AppendLine( $"\t\tActivated: True." );
            }
            else
                _ = builder.AppendLine( $"\t\tActivated: False." );

            if ( ScriptStatuses.Embedded == (this.ScriptStatus & ScriptStatuses.Embedded) )
                _ = builder.AppendLine( $"\t\t Embedded: True." );
            else
                _ = builder.AppendLine( $"\t\t Embedded: False." );
        }
        else
            _ = builder.AppendLine( $"\t\t   Loaded: False." );

        switch ( this.VersionStatus )
        {
            case FirmwareVersionStatus.Current:
                {
                    _ = builder.AppendLine( "\tAction required: None." );
                    _ = builder.AppendLine( $"\t\tThe embedded '{this.Title}' is current." );
                    break;
                }

            case FirmwareVersionStatus.Missing:
                {
                    _ = builder.AppendLine( "\tAction required: Contact the developer." );
                    _ = builder.AppendLine( $"\t\tA version function was not found for the '{this.Title}' firmware." );
                    break;
                }

            case FirmwareVersionStatus.Newer:
                {
                    _ = builder.AppendLine( "\tAction required: Get the new Loader program." );
                    _ = builder.AppendLine( $"\t\tThe embedded '{this.Title}' version {this.EmbeddedVersion} is newer than the candidate version of the '{this.Title}' firmware ({this.NextVersion}). A newer version of this program is available." );
                    break;
                }

            case FirmwareVersionStatus.Older:
                {
                    _ = builder.AppendLine( "\tAction required: Update the embedded firmware." );
                    _ = builder.AppendLine( $"\t\tOutdated Firmware: The embedded '{this.Title}' version {this.EmbeddedVersion} is older than the version of the new '{this.Title}' firmware ({this.NextVersion})." );
                    break;
                }

            case FirmwareVersionStatus.NextVersionNotSet:
                {
                    _ = builder.AppendLine( "\tAction required: Contact the developer." );
                    _ = builder.AppendLine( $"\t\tThe version of the next '{this.Title}' firmware is not specified." );
                    break;
                }

            case FirmwareVersionStatus.Unknown:
                {
                    _ = builder.AppendLine( "\tAction required: Contact the developer." );
                    _ = builder.AppendLine( $"\t\tThe version of the embedded '{this.Title}' firmware is not known." );
                    break;
                }

            case FirmwareVersionStatus.None:
                _ = builder.AppendLine( "\tAction required: Contact the developer." );
                _ = builder.AppendLine( $"\t\tThe status of the '{this.Title}' firmware version was not set." );
                break;

            default:
                {
                    _ = builder.AppendLine( "\tAction required: Contact the developer." );
                    _ = builder.AppendLine( $"\t\tThe {this.VersionStatus} item was not handled when building the firmware status report." );
                    break;
                }
        }

        return builder.ToString().TrimEndNewLine();
    }

    #endregion
}

/// <summary>   Collection of script information bases. </summary>
/// <remarks>   2025-04-22. </remarks>
/// <typeparam name="TItem">    Type of the item. </typeparam>
public class ScriptInfoBaseCollection<TItem> : System.Collections.ObjectModel.KeyedCollection<string, TItem> where TItem : ScriptInfo
{
    #region " construction and cleanup "

    /// <summary>   Constructor. </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="scripts">  The scripts. </param>
    public ScriptInfoBaseCollection( ScriptInfoBaseCollection<ScriptInfo> scripts )
    {
        foreach ( ScriptInfo script in scripts )
        {
            _ = this.AddScriptItem( script );
        }
    }

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2025-04-22. </remarks>
    public ScriptInfoBaseCollection()
    {
    }

    /// <summary>   Clone constructor. </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="scripts">  The scripts. </param>
    public ScriptInfoBaseCollection( ScriptInfoBaseCollection<TItem> scripts )
    {
        this.NodeNumber = scripts.NodeNumber;
        this.Model = scripts.Model;
        this.ModelFamily = scripts.ModelFamily;
        this.SerialNumber = scripts.SerialNumber;
        foreach ( ScriptInfo script in scripts )
            _ = this.AddScriptItem( new ScriptInfo( script ) );
    }

    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <remarks>   2025-04-22. </remarks>
    /// <param name="item"> The element from which to extract the key. </param>
    /// <returns>   The key for the specified element. </returns>
    protected override string GetKeyForItem( TItem item )
    {
        return item.Title;
    }

    #endregion

    #region " Add script "

    /// <summary>
    /// Adds an object to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1"></see>.
    /// </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <param name="script">   The object to be added to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1">
    ///                         </see>. The value can be null for reference types. </param>
    /// <returns>   A var. </returns>
    public Collection<TItem> AddScriptItem( ScriptInfo script )
    {
        return this.AddScriptItem( ( TItem ) script );
    }

    /// <summary>
    /// Adds an object to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1"></see>.
    /// </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <param name="script">   The object to be added to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1">
    ///                         </see>. The value can be null for reference types. </param>
    public Collection<TItem> AddScriptItem( TItem script )
    {
        if ( script is null ) throw new ArgumentNullException( nameof( script ) );
        this.Items.Add( script );
        return this;
    }

    /// <summary>   Script names. </summary>
    /// <remarks>   2025-05-14. </remarks>
    /// <returns>   A List{string}; </returns>
    public List<string> ScriptNames()
    {
        List<string> names = [];
        foreach ( TItem script in this.Items )
        {
            if ( !string.IsNullOrWhiteSpace( script.Title ) )
                names.Add( script.Title );
        }
        return names;
    }

    #endregion

    #region " TSP Device Inforamtion"

    /// <summary>   Gets or sets the node number. </summary>
    /// <remarks> The default node number represents the controller node. </remarks>
    /// <value> The node number. </value>
    public int NodeNumber { get; set; } = 0;

    /// <summary>   Gets or sets the instrument model, e.g.,. 2612A. </summary>
    /// <value> The instrument model. </value>
    public string Model { get; set; } = string.Empty;

    /// <summary>   Gets or sets the instrument model family, e.g.,. 2600A. </summary>
    /// <value> The instrument model family. </value>
    public string ModelFamily { get; set; } = string.Empty;

    /// <summary>   Gets or sets the serial number. </summary>
    /// <value> The serial number. </value>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>   Sets the device information. </summary>
    /// <remarks>   2025-05-14. </remarks>
    /// <param name="versionInfo">  Information describing the device version. </param>
    /// <param name="nodeNumber">   The device node number. </param>
    public void SetDeviceInfo( VersionInfoBase versionInfo, int nodeNumber )
    {
        this.NodeNumber = nodeNumber;
        this.Model = versionInfo.Model;
        this.ModelFamily = versionInfo.ModelFamily;
        this.SerialNumber = versionInfo.SerialNumber;
    }

    #endregion

    #region " script status report "

    /// <summary>   Builds script state report. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <returns>   A string. </returns>
    public string BuildScriptStateReport()
    {
        System.Text.StringBuilder builder = new();
        _ = builder.AppendLine( $"Info for '{this.Model}' SN {this.SerialNumber} node {this.NodeNumber}:" );
        foreach ( ScriptInfo script in this.Items )
        {
            if ( !string.IsNullOrWhiteSpace( script.Title ) )
                _ = builder.AppendLine( script.BuildScriptStateReport() );
        }
        return builder.ToString().TrimEndNewLine();
    }

    /// <summary>   Gets embedded scripts names. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   The embedded scripts names. </returns>
    public string GetEmbeddedScriptsNames()
    {
        System.Text.StringBuilder builder = new();
        foreach ( ScriptInfo script in this.Items )
        {
            if ( ScriptStatuses.Embedded == (script.ScriptStatus & ScriptStatuses.Embedded) )
            {
                if ( builder.Length > 0 )
                    _ = builder.Append( ", " );
                _ = builder.Append( script.Title );
            }
        }
        return builder.ToString();
    }

    /// <summary>   Select autoexec script. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   A ScriptInfo? </returns>
    public ScriptInfo? SelectAutoexecScript()
    {
        foreach ( ScriptInfo script in this.Items )
        {
            if ( !string.IsNullOrWhiteSpace( script.Title ) )
            {
                if ( script.IsAutoexec )
                    return script;
            }
        }
        return null;
    }

    /// <summary>   Determines if all scripts were loaded. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool AllLoaded()
    {
        foreach ( ScriptInfo script in this.Items )
        {
            if ( !string.IsNullOrWhiteSpace( script.Title ) )
            {
                if ( ScriptStatuses.Loaded != (script.ScriptStatus & ScriptStatuses.Loaded) )
                    return false;
            }
        }
        return true;
    }

    /// <summary>   Determines if we can any scripts loaded. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool AnyLoaded()
    {
        foreach ( ScriptInfo script in this.Items )
        {
            if ( !string.IsNullOrWhiteSpace( script.Title ) )
            {
                if ( ScriptStatuses.Loaded == (script.ScriptStatus & ScriptStatuses.Loaded) )
                    return true;
            }
        }
        return false;
    }

    /// <summary>   Determines if we can any activated. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool AnyActivated()
    {
        foreach ( ScriptInfo script in this.Items )
        {
            if ( !string.IsNullOrWhiteSpace( script.Title ) )
            {
                if ( ScriptStatuses.Activated == (script.ScriptStatus & ScriptStatuses.Activated) )
                    return true;
            }
        }
        return false;
    }

    /// <summary>   Determines if any script is embedded. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   True if one or more scripts are embedded; otherwise, false. </returns>
    public bool AnyEmbedded()
    {
        foreach ( ScriptInfo script in this.Items )
        {
            if ( !string.IsNullOrWhiteSpace( script.Title ) )
            {
                if ( ScriptStatuses.Embedded == (script.ScriptStatus & ScriptStatuses.Embedded) )
                    return true;
            }
        }
        return false;
    }

    /// <summary>   Gets a value indicating whether all scripts are embedded. </summary>
    /// <value> True if all scripts are embedded, false if not. </value>
    public bool AllEmbedded
    {
        get
        {
            foreach ( ScriptInfo script in this.Items )
            {
                if ( !string.IsNullOrWhiteSpace( script.Title ) )
                {
                    if ( ScriptStatuses.Embedded != (script.ScriptStatus & ScriptStatuses.Embedded) )
                        return false;
                }
            }
            return true;
        }
    }

    /// <summary>   Gets a value indicating whether one or more scripts can be deleted. </summary>
    /// <value> True if may delete, false if not. </value>
    public bool MayDelete => this.AnyLoaded();

    /// <summary>   Gets a value indicating whether one or more scripts must be loaded. </summary>
    /// <value> True if we must load, false if not. </value>
    public bool MustLoad => !this.AllLoaded();

    /// <summary>   Gets a value indicating whether one or more scripts must be embedded. </summary>
    /// <value> True if we must embed, false if not. </value>
    public bool MustEmbed => this.AnyLoaded() && !this.AllEmbedded;

    #endregion
}

/// <summary>   Collection of script information. </summary>
/// <remarks>   2025-04-25. </remarks>
public class ScriptInfoCollection : ScriptInfoBaseCollection<ScriptInfo>
{
    #region " construction and cleanup "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2025-04-25. </remarks>
    public ScriptInfoCollection()
    {
    }

    /// <summary>   Gets key for item. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="item"> The item. </param>
    /// <returns>   The key for item. </returns>
    protected override string GetKeyForItem( ScriptInfo item )
    {
        return base.GetKeyForItem( item );
    }

    #endregion
}

/// <summary>   Collection of node scripts. </summary>
/// <remarks>   2025-04-25. </remarks>
public class NodesScriptsCollection : Dictionary<int, ScriptInfoCollection>
{
    #region " script status report "

    /// <summary>   Builds script state report. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <returns>   A string. </returns>
    public string BuildScriptStateReport()
    {
        System.Text.StringBuilder builder = new();
        foreach ( ScriptInfoCollection scriptInfoCollection in this.Values )
        {
            _ = builder.AppendLine( scriptInfoCollection.BuildScriptStateReport() );
        }
        return builder.ToString().TrimEndNewLine();
    }

    /// <summary>   Determines if we can all loaded. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool AllLoaded()
    {
        foreach ( ScriptInfoCollection scriptInfoCollection in this.Values )
        {
            if ( !scriptInfoCollection.AllLoaded() )
                return false;
        }
        return true;
    }

    /// <summary>   Determines if we can any loaded. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool AnyLoaded()
    {
        foreach ( ScriptInfoCollection scriptInfoCollection in this.Values )
        {
            if ( scriptInfoCollection.AnyLoaded() )
                return true;
        }
        return false;
    }

    /// <summary>   Determines if we can any activated. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   True if it succeeds; otherwise, false. </returns>
    public bool AnyActivated()
    {
        foreach ( ScriptInfoCollection scriptInfoCollection in this.Values )
        {
            if ( scriptInfoCollection.AnyActivated() )
                return true;
        }
        return false;
    }

    /// <summary>   Determines if we any script is embedded. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   True if one or more scripts are embedded; otherwise, false. </returns>
    public bool AnyEmbedded()
    {
        foreach ( ScriptInfoCollection scriptInfoCollection in this.Values )
        {
            if ( scriptInfoCollection.AnyEmbedded() )
                return true;
        }
        return false;
    }

    /// <summary>   Determines if we can all scripts are embedded. </summary>
    /// <remarks>   2025-04-30. </remarks>
    /// <returns>   True if all scripts are embedded; otherwise, false. </returns>
    public bool AllEmbedded()
    {
        foreach ( ScriptInfoCollection scriptInfoCollection in this.Values )
        {
            if ( !scriptInfoCollection.AllEmbedded )
                return false;
        }
        return true;
    }

    /// <summary>   Gets a value indicating whether one or more scripts can be deleted. </summary>
    /// <value> True if may delete, false if not. </value>
    public bool MayDelete => this.AnyLoaded();

    /// <summary>   Gets a value indicating whether one or more scripts must be loaded. </summary>
    /// <value> True if we must load, false if not. </value>
    public bool MustLoad => !this.AllLoaded();

    /// <summary>   Gets a value indicating whether one or more scripts must be embedded. </summary>
    /// <value> True if we must embed, false if not. </value>
    public bool MustEmbed => this.AnyLoaded() && !this.AllEmbedded();

    #endregion
}
