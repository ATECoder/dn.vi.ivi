using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Meter settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class MeterSettings() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " exists "

    /// <summary>
    /// Gets or sets a value indicating whether this settings section exists and the values were thus
    /// fetched from the settings file.
    /// </summary>
    /// <value> True if this settings section exists in the settings file, false if not. </value>
    [Description( "True if this settings section exists and was read from the JSon settings file." )]
    public bool Exists
    {
        get;
        set => _ = this.SetProperty( ref field, value );
    }

    #endregion

    #region " meter "

    /// <summary>   Gets or sets a value indicating whether the contact check is enabled. </summary>
    /// <value> True if contact check enabled, false if not. </value>
    [ObservableProperty]
    [Description( "The Contact Check Enabled State (false)." )]
    public partial bool ContactCheckEnabled { get; set; } = false;

    /// <summary>   Gets or sets options for controlling the contact check. </summary>
    /// <value> Options that control the contact check. </value>
    [ObservableProperty]
    [Description( "The Contact Check Options (15)." )]
    public partial cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions ContactCheckOptions { get; set; } = cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Start
        | cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Initial
        | cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Trace
        | cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Final;

    /// <summary>   Gets or sets the contact check threshold. </summary>
    /// <value> The contact check threshold. </value>
    [ObservableProperty]
    [Description( "The Contact Check Threshold (75)." )]
    public partial int ContactCheckThreshold { get; set; } = 75;

    /// <summary>   Gets or sets the legacy driver flag. </summary>
    /// <value> The legacy driver flag. </value>
    [ObservableProperty]
    [Description( "The Post Transient Delay Time in Seconds (0.5)." )]
    public partial int LegacyDriver { get; set; } = 1;

    /// <summary>   Gets or sets the open limit value. </summary>
    /// <value> The open limit default value. </value>
    [ObservableProperty]
    [Description( "The Open Limit (1000)." )]
    public partial int OpenLimit { get; set; } = 1000;

    /// <summary>   Gets or sets the post transient delay. </summary>
    /// <value> The post transient delay. </value>
    [ObservableProperty]
    [Description( "The Post Transient Delay Time in Seconds (0.01)." )]
    public partial double PostTransientDelay { get; set; } = 0.01;

    /// <summary>   Gets or sets source measure unit. </summary>
    /// <value> The source measure unit. </value>
    [ObservableProperty]
    [Description( "The Source Measure Unit Name (smua)." )]
    public partial string SourceMeasureUnit { get; set; } = Syntax.ThermalTransient.DefaultSourceMeterName;

    /// <summary>   Gets or sets the source-sense shunt resistance value. </summary>
    /// <value> The source-sense shunt resistance value. </value>
    [ObservableProperty]
    [Description( "The source-sense shunt (3300)." )]
    public partial int SourceSenseShunt { get; set; } = 1000;

    #endregion
}
