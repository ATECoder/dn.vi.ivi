// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary>   The TTM Meter Settings. </summary>
/// <remarks>   David, 2021-02-01. </remarks>
public partial class TtmMeterSettings : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    #region " construction "

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-10-24. </remarks>
    public TtmMeterSettings() { }

    #endregion

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

    /// <summary>   Gets or sets the line frequency. </summary>
    /// <value> The line frequency. </value>
    [ObservableProperty]
    [Description( "The Instrument Line Frequency (60)." )]
    public partial double LineFrequency { get; set; } = 60;

    /// <summary>   Gets or sets the legacy driver flag. </summary>
    /// <value> The legacy driver flag. </value>
    [ObservableProperty]
    [Description( "The Post Transient Delay Time in Seconds (0.5)." )]
    public partial int LegacyDriver { get; set; } = 1;

    /// <summary>   Gets or sets the default legacy driver flag. </summary>
    /// <value> The default legacy driver flag. </value>
    [ObservableProperty]
    [Description( "The Default Legacy Driver Flag (1)." )]
    public partial int LegacyDriverDefault { get; set; } = 1;

    #endregion

    #region " contact check "

    /// <summary>   Gets or sets options for controlling the contact check. </summary>
    /// <value> Options that control the contact check. </value>
    [ObservableProperty]
    [Description( "The Contact Check Options (1)." )]
    public partial cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions ContactCheckOptions { get; set; } = cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Start;

    /// <summary>   Gets or sets options for controlling the contact check. </summary>
    /// <value> Options that control the contact check. </value>
    [ObservableProperty]
    [Description( "The Default Contact Check Options (1)." )]
    public partial cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions ContactCheckOptionsDefault { get; set; } = cc.isr.VI.Tsp.K2600.Ttm.Syntax.ContactCheckOptions.Start;

    /// <summary>   Gets or sets a value indicating whether the contact check is enabled. </summary>
    /// <value> True if contact check enabled, false if not. </value>
    [ObservableProperty]
    [Description( "The Contact Check Enabled State (false)." )]
    public partial bool ContactCheckEnabled { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the contact check enabled default.
    /// </summary>
    /// <value> True if contact check enabled default, false if not. </value>
    [ObservableProperty]
    [Description( "The Default Contact Check Enabled State (false)." )]
    public partial bool ContactCheckEnabledDefault { get; set; } = false;

    /// <summary>   Gets or sets the contact check threshold. </summary>
    /// <value> The contact check threshold. </value>
    [ObservableProperty]
    [Description( "The Contact Check Threshold (75)." )]
    public partial int ContactCheckThreshold { get; set; } = 75;

    /// <summary>   Gets or sets the contact check threshold default. </summary>
    /// <value> The contact check threshold default. </value>
    [ObservableProperty]
    [Description( "The Default Contact Check Threshold (75)." )]
    public partial int ContactCheckThresholdDefault { get; set; } = 75;

    #endregion

    #region " post transient "

    /// <summary>   Gets or sets the post transient delay. </summary>
    /// <value> The post transient delay. </value>
    [ObservableProperty]
    [Description( "The Post Transient Delay Time in Seconds (0.01)." )]
    public partial double PostTransientDelay { get; set; } = 0.01;

    /// <summary>   The Default Post Transient Delay Time in Seconds (0.01). </summary>
    /// <value> The post transient delay default. </value>
    [ObservableProperty]
    [Description( "The Default Post Transient Delay Time in Seconds (0.01)." )]
    public partial double PostTransientDelayDefault { get; set; } = 0.01;

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

    /// <summary>   Gets or sets source measure unit default. </summary>
    /// <value> The source measure unit default. </value>
    [ObservableProperty]
    [Description( "The Default Source Measure Unit Name (smua)." )]
    public partial string SourceMeasureUnitDefault { get; set; } = Syntax.ThermalTransient.DefaultSourceMeterName;

    #endregion
}
