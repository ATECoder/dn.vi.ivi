namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines an interface for configuring the measurement of thermal resistance. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>  
/// David, 2013-12-12, 3.0.3053.x. </para>
/// </remarks>

public interface IThermalTransientConfig : ICloneable
{
    /// <summary> Gets or sets the current level to apply to the device for measuring the thermal
    /// resistance. </summary>
    /// <value> The current level. </value>
    public float CurrentLevel { get; set; }

    /// <summary> Gets or sets the maximum expected transient voltage. </summary>
    /// <value> The allowed voltage change. </value>
    public float AllowedVoltageChange { get; set; }

    /// <summary> Gets or sets the integration period in power line cycles for measuring the thermal
    /// resistance. </summary>
    /// <remarks> Defaults to 0.004. </remarks>
    /// <value> The aperture. </value>
    public float Aperture { get; set; }

    /// <summary> Gets or sets the high limit for determining measurement pass fail condition. </summary>
    /// <value> The high limit. </value>
    public float HighLimit { get; set; }

    /// <summary> Gets or sets the low limit for determining measurement pass fail condition. </summary>
    /// <value> The low limit. </value>
    public float LowLimit { get; set; }

    /// <summary> Restores known state. </summary>
    public void ResetKnownState();

    /// <summary> Gets or sets the sampling interval. </summary>
    /// <remarks> Defaults to 0.0001. </remarks>
    /// <value> The sampling interval. </value>
    public float SamplingInterval { get; set; }

    /// <summary> Gets or sets the number of trace points to measure. </summary>
    /// <remarks> Defaults to 100. </remarks>
    /// <value> The trace points. </value>
    public int TracePoints { get; set; }


}
