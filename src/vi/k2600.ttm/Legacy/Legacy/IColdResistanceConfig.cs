namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines an interface for configuring the measurement of cold resistance. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>  
/// David, 2009-02-02, 2.1.3320.x. </para>
/// </remarks>
public interface IColdResistanceConfig : ICloneable
{

    /// <summary> Gets or sets the integration period in number of power line cycles for measuring the
    /// cold resistance. </summary>
    /// <value> The aperture. </value>
    float Aperture { get; set; }

    /// <summary> Gets or sets the current level to apply to the device for measuring the cold
    /// resistance. </summary>
    /// <value> The current level. </value>
    float CurrentLevel { get; set; }

    /// <summary> Gets or sets the high limit for determining measurement pass fail condition. </summary>
    /// <value> The high limit. </value>
    float HighLimit { get; set; }

    /// <summary> Gets or sets the low limit for determining measurement pass fail condition. </summary>
    /// <value> The low limit. </value>
    float LowLimit { get; set; }

    /// <summary> Restores known state. </summary>
    void ResetKnownState();

    /// <summary> Gets or sets the voltage limit. </summary>
    /// <value> The voltage limit. </value>
    float VoltageLimit { get; set; }

}
