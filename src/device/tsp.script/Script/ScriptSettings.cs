using System.ComponentModel;

namespace cc.isr.VI.Tsp.Script;

/// <summary>   The script settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
[CLSCompliant( false )]
public class ScriptSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2023-04-24. </remarks>
    public ScriptSettings() { }

    #endregion


    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [System.ComponentModel.Description( "True if this settings were found and read from the settings file." )]
    public bool Exists
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary>   Gets or sets the path of parent folder of the firmware files. Empty if using the application folder. </summary>
    /// <value> The path of parent folder of the firmware files. </value>
    [Description( "The path of parent folder of the firmware files. May be empty if using the application folder [c:\\my\\private\\ttm]" )]
    public string ParentFolderPath
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = "c:\\my\\private\\ttm";

    /// <summary>   Gets or sets the name of folder of the firmware files. </summary>
    /// <value> The name of folder of the firmware files. </value>
    [Description( "The name of the folder of firmware files [deploy]" )]
    public string ScriptsFolderName
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = "deploy";
}
