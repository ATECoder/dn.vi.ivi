namespace cc.isr.VI.Tsp.Script;

/// <summary>   Information about the firmware file. </summary>
/// <remarks>   2024-09-19. </remarks>
[CLSCompliant( false )]
public class FirmwareFileInfo : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    /// <summary>   Constructor. </summary>
    /// <remarks>   2024-09-19. </remarks>
    /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
    ///                                         illegal values. </exception>
    /// <param name="parentFolderPath"> full path name of the parent folder file. </param>
    /// <param name="filesFolderName">  Pathname of the files folder. </param>
    public FirmwareFileInfo( string parentFolderPath, string filesFolderName )
    {
        if ( string.IsNullOrEmpty( filesFolderName ) ) throw new ArgumentException( $"'{nameof( filesFolderName )}' cannot be null or empty.", nameof( filesFolderName ) );
        this._firmwareDirectoryInfo = new( parentFolderPath, filesFolderName );
    }

    /// <summary>   Constructor. </summary>
    /// <remarks>   2024-09-19. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="directoryInfo">    Information describing the directory. </param>
    public FirmwareFileInfo( FirmwareDirectoryInfo directoryInfo )
    {
        if ( directoryInfo is null ) throw new ArgumentNullException( nameof( directoryInfo ) );
        this._firmwareDirectoryInfo = directoryInfo;
    }

    private FirmwareDirectoryInfo _firmwareDirectoryInfo;

    /// <summary>   Gets or sets information describing the firmware directory. </summary>
    /// <value> Information describing the firmware directory. </value>
    public FirmwareDirectoryInfo FirmwareDirectoryInfo
    {
        get => this._firmwareDirectoryInfo;
        set => _ = this.SetProperty( ref this._firmwareDirectoryInfo, value );
    }

    /// <summary>   Gets or sets the name of the firmware file. </summary>
    /// <value> The filename of the firmware file. </value>
    public string FileName
    {
        get;
        set
        {
            if ( this.SetProperty( ref field, value )
                && !string.IsNullOrEmpty( value ) && !string.IsNullOrWhiteSpace( this.FirmwareDirectoryInfo.FilesFolderPath ) )
                this.FilePath = System.IO.Path.Combine( this.FirmwareDirectoryInfo.FilesFolderPath, field );
        }
    } = string.Empty;

    /// <summary>   Gets the full path of the firmware file. </summary>
    /// <value> The full path of the firmware file. </value>
    public string FilePath
    {
        get;
        private set => _ = this.SetProperty( ref field, value );
    } = string.Empty;

    /// <summary>   Opens a firmware file as a <see cref="System.IO.StreamReader"/>. </summary>
    /// <remarks>   2024-09-10. </remarks>
    /// <returns>   A reference to an open <see cref="System.IO.StreamReader"/>. </returns>
    public System.IO.StreamReader? OpenScriptFile()
    {
        return Script.FirmwareFileInfo.OpenScriptFile( this.FilePath );
    }

    /// <summary>   Opens a firmware file as a <see cref="System.IO.StreamReader"/>. </summary>
    /// <remarks>   2024-09-05. </remarks>
    /// <param name="filePath"> Specifies the firmware file path. </param>
    /// <returns>   A reference to an open <see cref="System.IO.StreamReader"/>. </returns>
    public static System.IO.StreamReader? OpenScriptFile( string? filePath )
    {
        return string.IsNullOrWhiteSpace( filePath ) || !System.IO.File.Exists( filePath )
            ? null
            : new System.IO.StreamReader( filePath );
    }

}
