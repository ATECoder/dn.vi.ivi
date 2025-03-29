using System.ComponentModel;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   Information about the firmware folders. </summary>
/// <remarks>   2024-09-09. </remarks>
[CLSCompliant( false )]
public class FirmwareDirectoryInfo : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction and cleanup "

    /// <summary>   Constructor. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <param name="parentFolderPath"> The full path name of the parent folder file. </param>
    /// <param name="filesFolderName"> Name of the folder for files.. </param>
    public FirmwareDirectoryInfo( string parentFolderPath, string filesFolderName )
    {
        if ( string.IsNullOrEmpty( filesFolderName ) ) throw new ArgumentException( $"'{nameof( filesFolderName )}' cannot be null or empty.", nameof( filesFolderName ) );

        if ( string.IsNullOrWhiteSpace( parentFolderPath ) )
            // use the execution path if the value empty or null.
            parentFolderPath = System.IO.Path.GetFullPath( AppDomain.CurrentDomain.BaseDirectory );
        this.ParentFolderPath = parentFolderPath;
        this.FilesFolderName = filesFolderName;
    }

    /// <summary>   Copy constructor. </summary>
    /// <remarks>   2024-09-11. </remarks>
    /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
    ///                                             null. </exception>
    /// <param name="directoryInfo">    Information describing the firmware directory. </param>
    public FirmwareDirectoryInfo( FirmwareDirectoryInfo directoryInfo )
    {
        if ( directoryInfo is null ) throw new ArgumentNullException( nameof( directoryInfo ) );
        this.ParentFolderPath = directoryInfo.ParentFolderPath;
        this.FilesFolderName = directoryInfo.FilesFolderName;
    }

    /// <summary>   Makes a deep copy of this object. </summary>
    /// <remarks>   2024-09-09. </remarks>
    /// <returns>   A copy of this object. </returns>
    public FirmwareDirectoryInfo Clone()
    {
        FirmwareDirectoryInfo directoryInfo = new( this.ParentFolderPath, this.FilesFolderName );
        return directoryInfo;
    }

    #endregion

    private string _parentFolderPath = string.Empty;

    /// <summary>
    /// Gets or sets the path of parent folder of the folder holding the firmware files. Empty if using the application
    /// folder.
    /// </summary>
    /// <value> The full path name of the parent folder file. </value>
    [Description( "The path of parent folder of the folder holding the firmware files. May be empty if using the application folder." )]
    public string ParentFolderPath
    {
        get => this._parentFolderPath;
        set
        {
            if ( value is null || string.IsNullOrWhiteSpace( value ) )
                // use the execution path if the value empty or null.
                value = System.IO.Path.GetFullPath( AppDomain.CurrentDomain.BaseDirectory );

            if ( this.SetProperty( ref this._parentFolderPath, value )
                && !string.IsNullOrEmpty( value ) && !string.IsNullOrWhiteSpace( this.FilesFolderName ) )
                this.FilesFolderPath = System.IO.Path.Combine( this.ParentFolderPath, this.FilesFolderName );
        }
    }

    private string _filesFolderName = string.Empty;

    /// <summary>   Gets or sets the name of folder hosting the firmware files. </summary>
    /// <value> The name of the folder hosting the firmware files. </value>
    [Description( "The name of the folder hosting the firmware files." )]
    public string FilesFolderName
    {
        get => this._filesFolderName;
        set
        {
            if ( this.SetProperty( ref this._filesFolderName, value )
                && !string.IsNullOrEmpty( value ) && !string.IsNullOrWhiteSpace( this.ParentFolderPath ) )
                this.FilesFolderPath = System.IO.Path.Combine( this.ParentFolderPath, this.FilesFolderName );
        }
    }

    private string _filesFolderPath = string.Empty;

    /// <summary>   Gets the full path name of the folder hosting the firmware files. </summary>
    /// <value> The full path name of the folder hosting the firmware files. </value>
    public string FilesFolderPath
    {
        get => this._filesFolderPath;
        private set => this.SetProperty( ref this._filesFolderPath, value );
    }
}
