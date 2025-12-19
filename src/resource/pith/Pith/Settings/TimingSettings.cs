using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Pith.Settings;

/// <summary>   The visa session timing settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
[CLSCompliant( false )]
public partial class TimingSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
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

    /// <summary>   Gets or sets the default open session timeout. </summary>
    /// <value> The default open session timeout. </value>
    [ObservableProperty]
    [Description( "The time to wait for a session to open [0.5s]" )]
    public partial TimeSpan OpenSessionTimeout { get; set; } = TimeSpan.FromSeconds( 0.5 );

    /// <summary>   Gets or sets the default open session timeout. </summary>
    /// <value> The default open session timeout. </value>
    [ObservableProperty]
    [Description( "The time to wait query turnaround [3.05s]" )]
    public partial TimeSpan CommunicationTimeout { get; set; } = TimeSpan.FromSeconds( 3.05 );

    /// <summary>   Gets or sets the timeout for initializing all subsystem following a reset and clear. </summary>
    /// <value> The timeout for initialize the post reset clear state. </value>
    [ObservableProperty]
    [Description( "The VISA session timeout for initializing all subsystems following a reset and clear [10s]" )]
    public partial TimeSpan InitKnownStateTimeout { get; set; } = TimeSpan.FromSeconds( 10 );

    /// <summary>   Gets or sets the device clear refractory period. </summary>
    /// <value> The device clear refractory period. </value>
    [ObservableProperty]
    [Description( "The time to wait before returning control after a device clearing the instrument (*DCl) [500ms]" )]
    public partial TimeSpan DeviceClearRefractoryPeriod { get; set; } = TimeSpan.FromMilliseconds( 500 );

    /// <summary>   Gets or sets the interface clear refractory period. </summary>
    /// <value> The interface clear refractory period. </value>
    [ObservableProperty]
    [Description( "The time to wait before returning control after a clearing the instrument interface (selective device clear) [0.5s]" )]
    public partial TimeSpan InterfaceClearRefractoryPeriod { get; set; } = TimeSpan.FromMilliseconds( 500 );

    /// <summary>   Gets or sets the reset refractory period. </summary>
    /// <value> The reset refractory period. </value>
    [ObservableProperty]
    [Description( "The time to wait before returning control after resetting the session to its know state (*RST) [0.2s]" )]
    public partial TimeSpan ResetRefractoryPeriod { get; set; } = TimeSpan.FromSeconds( 0.2 );

    /// <summary>   Gets or sets the clear refractory period. </summary>
    /// <value> The clear refractory period. </value>
    [ObservableProperty]
    [Description( "The time to wait before returning control after clearing the instrument (*CLS) [100ms]" )]
    public partial TimeSpan ClearRefractoryPeriod { get; set; } = TimeSpan.FromMilliseconds( 100 );

    /// <summary>   Gets or sets the status read turnaround time. </summary>
    /// <value> The status read turnaround time. </value>
    [ObservableProperty]
    [Description( "The time it takes to read the status byte [10ms]" )]
    public partial TimeSpan StatusReadTurnaroundTime { get; set; } = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   Gets or sets the status read delay. </summary>
    /// <value> The status read delay. </value>
    [ObservableProperty]
    [Description( "The time to wait after a reading the status byte [10ms]" )]
    public partial TimeSpan StatusReadDelay { get; set; } = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   Gets or sets the read after write delay. </summary>
    /// <value> The read delay. </value>
    [ObservableProperty]
    [Description( "The time to wait before reading the instrument after a write [10ms]" )]
    public partial TimeSpan ReadAfterWriteDelay { get; set; } = TimeSpan.FromMilliseconds( 10 );

    /// <summary>   Gets or sets the time to wait for garbage collection. </summary>
    /// <value> The time to wait for garbage collection. </value>
    [ObservableProperty]
    [Description( "The time to wait for garbage collection [0.5s]" )]
    public partial TimeSpan CollectGarbageTimeout { get; set; } = TimeSpan.FromSeconds( 0.5 );

    /// <summary>
    /// Gets or sets the operation completion timeout (was status subsystem initialize refractory
    /// period) for instruments that do not support operation completion.
    /// </summary>
    /// <value> The operation completion timeout. </value>
    [ObservableProperty]
    [Description( "Specifies the time to wait for operation completion [100ms]" )]
    public partial TimeSpan OperationCompletionRefractoryPeriod { get; set; }
}
