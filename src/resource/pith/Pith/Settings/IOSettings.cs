using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Pith.Settings;

/// <summary>   The visa session IO settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
[CLSCompliant( false )]
public partial class IOSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings were found and read from the settings file." )]
    public bool Exists
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    /// <summary> Gets the ASCII character used to end reading. </summary>
    /// <value> The read termination character. </value>
    [Description( "The ASCII character used to end reading [10]" )]
    public byte ReadTerminationCharacter
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = Std.EscapeSequencesExtensions.EscapeSequencesExtensionMethods.NEW_LINE_VALUE;

    /// <summary>
    /// Gets the value indicating whether the read operation ends when a termination character is received.
    /// </summary>
    /// <value> The termination character enabled. </value>
    [Description( "The value indicating whether the read operation ends when a termination character is received [true]" )]
    public bool ReadTerminationEnabled
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    } = true;

    /// <summary>   Gets or sets the session message notification modes. </summary>
    /// <value> The session message notification modes. </value>
    [ObservableProperty]
    [Description( "Specifies the modes of session notification, e.g., none or message received, sent or both [None; 0]" )]
    public partial MessageNotificationModes SessionMessageNotificationModes { get; set; } = MessageNotificationModes.None;
}
