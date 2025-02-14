namespace cc.isr.VI.Tsp.K2600.Ttm.Syntax;

/// <summary> Syntax of the thermal transient subsystem. </summary>
/// <remarks> (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para> </remarks>
public static partial class ThermalTransient
{
    /// <summary> The version query command. </summary>
    public const string VersionQueryCommand = "_G.print(_G.isr.meters.thermalTransient.version())";

    /// <summary> Name of the global. </summary>
    public const string GlobalName = "_G";

    /// <summary> Initial entity name. </summary>
    public const string InitialEntityName = "nil";

    /// <summary> Name of the thermal transient base entity. </summary>
    public const string ThermalTransientBaseEntityName = "_G.ttm";

    /// <summary> Name of the final resistance entity. </summary>
    public const string FinalResistanceEntityName = "_G.ttm.fr";

    /// <summary> Name of the initial resistance entity. </summary>
    public const string InitialResistanceEntityName = "_G.ttm.ir";

    /// <summary> Name of the thermal transient entity. </summary>
    public const string ThermalTransientEntityName = "_G.ttm.tr";

    /// <summary> Name of the thermal transient estimator entity. </summary>
    public const string ThermalTransientEstimatorEntityName = "_G.ttm.est";

    /// <summary>   (Immutable) name of the cold resistance defaults. </summary>
    public const string ColdResistanceDefaultsName = "_G.ttm.coldResistance.Defaults";

    /// <summary>   (Immutable) name of the trace entity defaults. </summary>
    public const string TraceEntityDefaultsName = "_G.ttm.trace.Defaults";

    /// <summary>   (Immutable) name of the estimator entity defaults. </summary>
    public const string EstimatorEntityDefaultsName = "_G.ttm.estimator.Defaults";

    /// <summary>   (Immutable) name of the shunt resistance entity defaults. </summary>
    public const string ShuntResistanceEntityDefaultsName = "";

    /// <summary>   (Immutable) name of the global entity defaults. </summary>
    public const string GlobalEntityDefaultsName = "";

    /// <summary>   Gets the name of the meter entity defaults. </summary>
    /// <value> The name of the meter entity defaults. </value>
    public static string MeterEntityDefaultsName => MeterSubsystem.LegacyFirmware ? "_G.ttm" : "_G.ttm.meterDefaults";

    /// <summary>   Gets or sets the default source meter name. </summary>
    /// <value> The default source meter name. </value>
    public static string DefaultSourceMeterName { get; set; } = "smua";

    /// <summary>   Gets or sets a list of names of the source measure units. </summary>
    /// <value> A list of names of the source measure units. </value>
    public static string SourceMeasureUnitNames { get; set; } = "smua,smub,smu";

    /// <summary>   Gets the name of the outcome bits. </summary>
    /// <value> The name of the outcome bits. </value>
    public static string OutcomeBitsName => MeterSubsystem.LegacyFirmware ? "_G.ttm.Outcomes" : "_G.ttm.OutcomeBits";

    /// <summary>   Gets the name of the outcome bits okay. </summary>
    /// <value> The name of the outcome bits okay. </value>
    public static string OutcomeBitsOkayName => $"{ThermalTransient.OutcomeBitsName}.okay";

    /// <summary>   Gets the name of the outcome bits open leads. </summary>
    /// <value> The name of the outcome bits open leads. </value>
    public static string OutcomeBitsOpenLeadsName => MeterSubsystem.LegacyFirmware ? "" : "_G.ttm.OutcomeBits.openLeads";

}
