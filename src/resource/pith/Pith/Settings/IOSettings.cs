using System.ComponentModel;

namespace cc.isr.VI.Pith.Settings;

/// <summary>   The visa session IO settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
[CLSCompliant( false )]
public class IOSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
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

    private byte _readTerminationCharacter = Std.EscapeSequencesExtensions.EscapeSequencesExtensionMethods.NEW_LINE_VALUE;

    /// <summary> Gets the ASCII character used to end reading. </summary>
    /// <value> The read termination character. </value>
	[Description( "The ASCII character used to end reading [10]" )]
    public byte ReadTerminationCharacter
    {
        get => this._readTerminationCharacter;
        set => _ = this.SetProperty( ref this._readTerminationCharacter, value );
    }

    private bool _readTerminationEnabled = true;

    /// <summary>
    /// Gets the value indicating whether the read operation ends when a termination character is received.
    /// </summary>
    /// <value> The termination character enabled. </value>
	[Description( "The value indicating whether the read operation ends when a termination character is received [true]" )]
    public bool ReadTerminationEnabled
    {
        get => this._readTerminationEnabled;
        set => _ = this.SetProperty( ref this._readTerminationEnabled, value );
    }
}
