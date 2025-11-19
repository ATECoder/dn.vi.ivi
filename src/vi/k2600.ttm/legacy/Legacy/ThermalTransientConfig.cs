// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs


namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines thermal resistance configuration element. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2009-02-02, 2.1.3320.x. </para><para>
/// David, 2013-08-05, 3.0.3053.x. </para>
/// </remarks>
public class ThermalTransientConfig : IThermalTransientConfig
{
    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-11-07. </remarks>
    /// <param name="thermalTransient"> The thermal transient. </param>
    [CLSCompliant( false )]
    public ThermalTransientConfig( cc.isr.VI.Tsp.K2600.Ttm.ThermalTransient thermalTransient )
    {
        this._thermalTransient = thermalTransient;
        this.ResetKnownStateInternal();
    }

    /// <summary> Clones an existing configuration. </summary>
    /// <param name="value"> The value. </param>
    public ThermalTransientConfig( ThermalTransientConfig value ) : this( value._thermalTransient )
    {
        if ( value is not null )
        {
            this.AllowedVoltageChange = value.AllowedVoltageChange;
            this.Aperture = value.Aperture;
            this.CurrentLevel = value.CurrentLevel;
            this.HighLimit = value.HighLimit;
            this.LowLimit = value.LowLimit;
            this.SamplingInterval = value.SamplingInterval;
            this.TracePoints = value.TracePoints;
        }
    }

    /// <summary> Clones the configuration. </summary>
    /// <returns> A copy of this object. </returns>
    public object Clone()
    {
        return new ThermalTransientConfig( this );
    }

    /// <summary> Resets the known state. </summary>
    private void ResetKnownStateInternal()
    {
        this.AllowedVoltageChange = 0.099f;
        this.Aperture = 0.004f;
        this.CurrentLevel = 0.27f;
        this.LowLimit = 0.006f;
        this.HighLimit = 0.017f;
        this.SamplingInterval = 0.0001f;
        this.TracePoints = 100;
    }

    /// <summary> Restores known state. </summary>
    public void ResetKnownState()
    {
        this.ResetKnownStateInternal();
    }

    /// <summary>   (Immutable) the thermal transient. </summary>
    private readonly cc.isr.VI.Tsp.K2600.Ttm.ThermalTransient _thermalTransient;

    /// <summary> Gets or sets the maximum expected transient voltage. </summary>
    /// <value> The allowed voltage change. </value>
    public float AllowedVoltageChange
    {
        get => ( float ) this._thermalTransient.AllowedVoltageChange;
        set => this._thermalTransient.AllowedVoltageChange = value;
    }

    /// <summary> Gets or sets the integration period in power line cycles for measuring the thermal
    /// resistance. </summary>
    /// <value> The aperture. </value>
    public float Aperture
    {
        get => ( float ) this._thermalTransient.Aperture;
        set => this._thermalTransient.Aperture = value;
    }

    /// <summary> Gets or sets the current level to apply to the device for measuring the thermal
    /// resistance. </summary>
    /// <value> The current level. </value>
    public float CurrentLevel
    {
        get => ( float ) this._thermalTransient.CurrentLevel;
        set => this._thermalTransient.CurrentLevel = value;
    }

    /// <summary> Gets or sets the high limit for determining measurement pass fail condition. </summary>
    /// <value> The high limit. </value>
    public float HighLimit
    {
        get => ( float ) this._thermalTransient.HighLimit;
        set => this._thermalTransient.HighLimit = value;
    }

    /// <summary> Gets or sets the low limit for determining measurement pass fail condition. </summary>
    /// <value> The low limit. </value>
    public float LowLimit
    {
        get => ( float ) this._thermalTransient.LowLimit;
        set => this._thermalTransient.LowLimit = value;
    }

    /// <summary> Gets or sets the sampling interval. </summary>
    /// <value> The sampling interval. </value>
    public float SamplingInterval
    {
        get => ( float ) this._thermalTransient.SamplingInterval;
        set => this._thermalTransient.SamplingInterval = value;
    }

    /// <summary> Gets or sets the number of trace points to measure. </summary>
    /// <value> The trace points. </value>
    public int TracePoints
    {
        get => this._thermalTransient.TracePoints;
        set => this._thermalTransient.TracePoints = value;
    }

}
