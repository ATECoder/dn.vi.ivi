using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Meter default settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class MeterDefaults() : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
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

    /// <summary>   Gets or sets the legacy driver flag. </summary>
    /// <value> The legacy driver flag. </value>
    [ObservableProperty]
    [Description( "The Post Transient Delay Time in Seconds (0.5)." )]
    public partial int LegacyDriver { get; set; } = 1;

    #endregion

    #region " contact check "

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

    /// <summary>   The minimum Contact Check Options (0). </summary>
    /// <value> The contact check options minimum. </value>
    [ObservableProperty]
    [Description( "The minimum Contact Check Options (0)." )]
    public partial cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions ContactCheckOptionsMinimum { get; set; } = cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.None;

    /// <summary>   The maximum Contact Check Options (15). </summary>
    /// <value> The contact check options maximum. </value>
    [ObservableProperty]
    [Description( "The maximum Contact Check Options (15)." )]
    public partial cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions ContactCheckOptionsMaximum { get; set; } = cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Start
        | cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Initial
        | cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Trace
        | cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Final;

    /// <summary>   Gets or sets the contact check threshold. </summary>
    /// <value> The contact check threshold. </value>
    [ObservableProperty]
    [Description( "The Contact Check Threshold (75)." )]
    public partial int ContactCheckThreshold { get; set; } = 75;

    /// <summary>   The minimum Contact Check (20). </summary>
    /// <value> The contact check threshold minimum. </value>
    [ObservableProperty]
    [Description( "The minimum Contact Check (20)." )]
    public partial int ContactCheckThresholdMinimum { get; set; } = 20;

    /// <summary>   The maximum Contact Check (20). </summary>
    /// <value> The contact check threshold maximum. </value>
    [ObservableProperty]
    [Description( "The maximum Contact Check (20)." )]
    public partial int ContactCheckThresholdMaximum { get; set; } = 999;

    /// <summary>   Gets or sets the open limit value. </summary>
    /// <value> The open limit default value. </value>
    [ObservableProperty]
    [Description( "The Open Limit (1000)." )]
    public partial int OpenLimit { get; set; } = 1000;

    /// <summary>   The minimum Open Limit (0). </summary>
    /// <value> The open limit minimum. </value>
    [ObservableProperty]
    [Description( "The minimum Open Limit (0)." )]
    public partial int OpenLimitMinimum { get; set; } = 0;

    /// <summary>   The maximum Open Limit (999999). </summary>
    /// <value> The open limit maximum. </value>
    [ObservableProperty]
    [Description( "The maximum Open Limit (999999)." )]
    public partial int OpenLimitMaximum { get; set; } = 999999;

    /// <summary>   Gets or sets the source-sense shunt resistance value. </summary>
    /// <value> The source-sense shunt resistance value. </value>
    [ObservableProperty]
    [Description( "The source-sense shunt (3300)." )]
    public partial int SourceSenseShunt { get; set; } = 1000;

    /// <summary>   The minimum source-sense shunt (0). </summary>
    /// <value> The source sense shunt minimum. </value>
    [ObservableProperty]
    [Description( "The minimum source-sense shunt (0)." )]
    public partial int SourceSenseShuntMinimum { get; set; } = 0;

    /// <summary>   The maximum source-sense shunt (999999). </summary>
    /// <value> The source sense shunt maximum. </value>
    [ObservableProperty]
    [Description( "The maximum source-sense shunt (999999)." )]
    public partial int SourceSenseShuntMaximum { get; set; } = 999999;

    #endregion

    #region " post transient "

    /// <summary>   Gets or sets the post transient delay. </summary>
    /// <value> The post transient delay. </value>
    [ObservableProperty]
    [Description( "The Post Transient Delay Time in Seconds (0.01)." )]
    public partial double PostTransientDelay { get; set; } = 0.01;

    /// <summary>   The Maximum Post Transient Delay Time in Seconds (10). </summary>
    /// <value> The post transient delay maximum. </value>
    [ObservableProperty]
    [Description( "The Maximum Post Transient Delay Time in Seconds (10)." )]
    public partial double PostTransientDelayMaximum { get; set; } = 10;

    /// <summary>   Gets or sets the post transient delay minimum. </summary>
    /// <value> The post transient delay minimum. </value>
    [ObservableProperty]
    [Description( "The Minimum Post Transient Delay Time in Seconds (0.001)." )]
    public partial double PostTransientDelayMinimum { get; set; } = 0.001;

    #endregion

    #region " smu "

    /// <summary>   Gets or sets source measure unit. </summary>
    /// <value> The source measure unit. </value>
    [ObservableProperty]
    [Description( "The Source Measure Unit Name (smua)." )]
    public partial string SourceMeasureUnit { get; set; } = Syntax.ThermalTransient.DefaultSourceMeterName;

    #endregion
}
