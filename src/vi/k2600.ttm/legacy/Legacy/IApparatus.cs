using cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines an interface for the Thermal Transient Apparatus. </summary>
/// <remarks>
/// (c) 2010 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2024-11-07, 8.1.9077.x. A minimal interface striped to match the current FET implementation. </para><para>
/// David, 2010-09-17, 2.2.3912.x. </para>
/// </remarks>
public interface IApparatus : IDisposable
{
    /// <summary> Gets or sets the cold resistance configuration parameters. </summary>
    /// <value> The cold resistance configuration. </value>
    public IColdResistanceConfig ColdResistanceConfig { get; set; }

    /// <summary> Gets or sets reference to the <see cref="IColdResistance">Final cold resistance</see> </summary>
    /// <value> The final resistance. </value>
    public IColdResistance FinalResistance { get; set; }

    /// <summary> Gets or sets reference to the <see cref="IColdResistance">initial cold
    /// resistance</see> </summary>
    /// <value> The initial resistance. </value>
    public IColdResistance InitialResistance { get; set; }

    /// <summary> Gets or sets the delay time in seconds between the end of the thermal transient and
    /// the start of the final cold resistance measurement. </summary>
    /// <value> The post transient delay. </value>
    public float PostTransientDelay { get; set; }

    /// <summary> Gets the delay time in seconds between the end of the thermal transient and the start
    /// of the final cold resistance measurement. </summary>
    /// <value> The post transient delay configuration. </value>
    public float PostTransientDelayConfig { get; set; }

    /// <summary> Gets or sets reference to the <see cref="IColdResistance">initial cold
    /// resistance</see> </summary>
    /// <value> The thermal transient. </value>
    public IThermalTransient ThermalTransient { get; set; }

    /// <summary> Gets or sets the thermal transient configuration parameters. </summary>
    /// <value> The thermal transient configuration. </value>
    public IThermalTransientConfig ThermalTransientConfig { get; set; }

    /// <summary> Initializes known state. </summary>
    /// <remarks> This erases the last reading. </remarks>
    public void InitializeKnownState();
}
