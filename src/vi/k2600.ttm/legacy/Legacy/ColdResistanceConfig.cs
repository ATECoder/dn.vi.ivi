// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600.Ttm.Legacy;

/// <summary> Defines cold resistance configuration element. </summary>
/// <remarks>
/// (c) 2009 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2009-02-02, 2.1.3320.x. </para>
/// </remarks>
public class ColdResistanceConfig : IColdResistanceConfig
{

    /// <summary>   Default constructor. </summary>
    /// <remarks>   2024-11-07. </remarks>
    /// <param name="initialResistance">    The initial resistance. </param>
    /// <param name="finalResistance">      The final resistance. </param>
    [CLSCompliant( false )]
    public ColdResistanceConfig( cc.isr.VI.Tsp.K2600.Ttm.ColdResistance initialResistance, cc.isr.VI.Tsp.K2600.Ttm.ColdResistance finalResistance )
    {
        this._initialResistance = initialResistance;
        this._finalResistance = finalResistance;
        this.ResetKnownStateInternal();
    }

    /// <summary> Clones an existing configuration. </summary>
    /// <param name="value"> The value. </param>
    public ColdResistanceConfig( ColdResistanceConfig value ) : this( value._initialResistance, value._finalResistance )
    {
        if ( value is not null )
        {
            this.Aperture = value.Aperture;
            this.CurrentLevel = value.CurrentLevel;
            this.HighLimit = value.HighLimit;
            this.LowLimit = value.LowLimit;
            this.VoltageLimit = value.VoltageLimit;
        }
    }

    /// <summary> Clones the configuration. </summary>
    /// <returns> A copy of this object. </returns>
    public object Clone()
    {
        return new ColdResistanceConfig( this );
    }

    /// <summary> Restores defaults. </summary>
    private void ResetKnownStateInternal()
    {
        this.Aperture = 1f;
        this.CurrentLevel = 0.01f;
        this.LowLimit = 1.85f;
        this.HighLimit = 2.156f;
        this.VoltageLimit = 0.1f;
    }

    /// <summary> Restores defaults. </summary>
    public void ResetKnownState()
    {
        this.ResetKnownStateInternal();
    }

    private readonly cc.isr.VI.Tsp.K2600.Ttm.ColdResistance _initialResistance;
    private readonly cc.isr.VI.Tsp.K2600.Ttm.ColdResistance _finalResistance;

    /// <summary> Gets or sets the integration period in number of power line cycles for measuring the
    /// cold resistance. </summary>
    /// <value> The aperture. </value>
    public float Aperture
    {
        get => ( float ) this._initialResistance.Aperture;
        set
        {
            this._initialResistance.Aperture = value;
            this._finalResistance.Aperture = value;
        }
    }

    /// <summary> Gets or sets the current level. </summary>
    /// <value> The current level. </value>
    public float CurrentLevel
    {
        get => ( float ) this._initialResistance.CurrentLevel;
        set
        {
            this._initialResistance.CurrentLevel = value;
            this._finalResistance.CurrentLevel = value;
        }
    }

    /// <summary> Gets or sets the high limit for determining measurement pass fail condition. </summary>
    /// <value> The high limit. </value>
    public float HighLimit
    {
        get => ( float ) this._initialResistance.HighLimit;
        set
        {
            this._initialResistance.HighLimit = value;
            this._finalResistance.HighLimit = value;
        }
    }

    /// <summary> Gets or sets the low limit for determining measurement pass fail condition. </summary>
    /// <value> The low limit. </value>
    public float LowLimit
    {
        get => ( float ) this._initialResistance.LowLimit;
        set
        {
            this._initialResistance.LowLimit = value;
            this._finalResistance.LowLimit = value;
        }
    }

    /// <summary> Gets or sets the voltage limit. </summary>
    /// <value> The voltage limit. </value>
    public float VoltageLimit
    {
        get => ( float ) this._initialResistance.VoltageLimit;
        set
        {
            this._initialResistance.VoltageLimit = value;
            this._finalResistance.VoltageLimit = value;
        }
    }

}
