using System.ComponentModel;

namespace cc.isr.VI.Pith.Settings;

/// <summary>   The visa session SCPI exceptions settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
[CLSCompliant( false )]
public class ScpiExceptionsSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    private bool _exists;

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings were found and read from the settings file." )]
    public bool Exists
    {
        get => this._exists;
        set => _ = this.SetProperty( ref this._exists, value );
    }

    private bool _statusClearDistractive = false;

    /// <summary>
    /// Gets or sets a value indicating whether the *CLS command is distractive and clears the event
    /// enable registers.
    /// </summary>
    /// <value>
    /// True if *CLS commands clear the event enable registers, false if not.
    /// </value>
    [Description( "True if *CLS clears the event enable registers [false]" )]
    public bool StatusClearDistractive
    {
        get => this._statusClearDistractive;
        set => _ = this.SetProperty( ref this._statusClearDistractive, value );
    }

    private bool _clearsDeviceStructures = true;

    /// <summary>   True if *CLS clears the device structures [true]. </summary>
    /// <value> True if clears device structures, false if not. </value>
    [Description( "True if *CLS clears the device structures [true]" )]
    public bool ClearsDeviceStructures
    {
        get => this._clearsDeviceStructures;
        set => _ = this.SetProperty( ref this._clearsDeviceStructures, value );
    }

    private bool _splitCommonCommands;

    /// <summary>   Gets or sets a value indicating whether to split common commands when writing to the instrument. </summary>
    /// <remarks> TSP instruments do not accept common command separate by semicolon. This commands need to be split and executed as separate commands. </remarks>
    /// <value> True to split common commands when writing to the instrument, false if not. </value>
    [Description( "Compound common of TSP instruments such as the 2600 must execute as separate commands [false]" )]
    public bool SplitCommonCommands
    {
        get => this._splitCommonCommands;
        set => _ = this.SetProperty( ref this._splitCommonCommands, value );
    }
}
