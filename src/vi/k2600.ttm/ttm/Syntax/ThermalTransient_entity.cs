// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Syntax;

public static partial class ThermalTransient
{
    /// <summary>   Selects the meter entity name. </summary>
    /// <remarks>   2024-11-14. </remarks>
    /// <param name="meterEntity">  The meter entity. </param>
    /// <returns>   A string. </returns>
    public static string SelectEntityName( ThermalTransientMeterEntity meterEntity )
    {
        string value;
        switch ( meterEntity )
        {
            case ThermalTransientMeterEntity.MeterMain:
                {
                    value = Syntax.ThermalTransient.ThermalTransientBaseEntityName;
                    break;
                }

            case ThermalTransientMeterEntity.FinalResistance:
                {
                    value = Syntax.ThermalTransient.FinalResistanceEntityName;
                    break;
                }

            case ThermalTransientMeterEntity.InitialResistance:
                {
                    value = Syntax.ThermalTransient.InitialResistanceEntityName;
                    break;
                }

            case ThermalTransientMeterEntity.Transient:
                {
                    value = Syntax.ThermalTransient.ThermalTransientEntityName;
                    break;
                }

            case ThermalTransientMeterEntity.Estimator:
                {
                    value = Syntax.ThermalTransient.ThermalTransientEstimatorEntityName;
                    break;
                }

            case ThermalTransientMeterEntity.Shunt:
                {
                    value = Syntax.ThermalTransient.GlobalName;
                    break;
                }

            case ThermalTransientMeterEntity.None:
                {
                    value = Syntax.ThermalTransient.ThermalTransientBaseEntityName;
                    break;
                }

            default:
                {
                    value = "unknown";
                    Debug.Assert( !Debugger.IsAttached, "Unhandled case" );
                    break;
                }
        }
        return value;
    }

    /// <summary>   Select base entity name. </summary>
    /// <remarks>   2024-11-14. </remarks>
    /// <param name="meterEntity">  The meter entity. </param>
    /// <returns>   A string. </returns>
    public static string SelectBaseEntityName( ThermalTransientMeterEntity meterEntity )
    {
        string value;
        switch ( meterEntity )
        {
            case ThermalTransientMeterEntity.MeterMain:
                {
                    value = Syntax.ThermalTransient.ThermalTransientBaseEntityName;
                    break;
                }

            case ThermalTransientMeterEntity.FinalResistance:
                {
                    value = Syntax.ThermalTransient.ThermalTransientBaseEntityName;
                    break;
                }

            case ThermalTransientMeterEntity.InitialResistance:
                {
                    value = Syntax.ThermalTransient.ThermalTransientBaseEntityName;
                    break;
                }

            case ThermalTransientMeterEntity.Transient:
                {
                    value = Syntax.ThermalTransient.ThermalTransientBaseEntityName;
                    break;
                }

            case ThermalTransientMeterEntity.Estimator:
                {
                    value = Syntax.ThermalTransient.ThermalTransientBaseEntityName;
                    break;
                }

            case ThermalTransientMeterEntity.Shunt:
                {
                    value = Syntax.ThermalTransient.GlobalName;
                    break;
                }

            case ThermalTransientMeterEntity.None:
                {
                    value = Syntax.ThermalTransient.GlobalName;
                    break;
                }

            default:
                {
                    value = "unknown";
                    Debug.Assert( !Debugger.IsAttached, "Unhandled case" );
                    break;
                }
        }
        return value;
    }

    /// <summary>   Select entity defaults name. </summary>
    /// <remarks>   2024-11-14. </remarks>
    /// <param name="meterEntity">  The meter entity. </param>
    /// <returns>   A string. </returns>
    public static string SelectEntityDefaultsName( ThermalTransientMeterEntity meterEntity )
    {
        string value;
        switch ( meterEntity )
        {
            case ThermalTransientMeterEntity.MeterMain:
                {
                    value = Syntax.ThermalTransient.MeterEntityDefaultsName;
                    break;
                }

            case ThermalTransientMeterEntity.FinalResistance:
                {
                    value = Syntax.ThermalTransient.ColdResistanceDefaultsName;
                    break;
                }

            case ThermalTransientMeterEntity.InitialResistance:
                {
                    value = Syntax.ThermalTransient.ColdResistanceDefaultsName;
                    break;
                }

            case ThermalTransientMeterEntity.Transient:
                {
                    value = Syntax.ThermalTransient.TraceEntityDefaultsName;
                    break;
                }

            case ThermalTransientMeterEntity.Estimator:
                {
                    value = Syntax.ThermalTransient.EstimatorEntityDefaultsName;
                    break;
                }

            case ThermalTransientMeterEntity.Shunt:
                {
                    value = Syntax.ThermalTransient.ShuntResistanceEntityDefaultsName;
                    break;
                }

            case ThermalTransientMeterEntity.None:
                {
                    value = Syntax.ThermalTransient.GlobalEntityDefaultsName;
                    break;
                }

            default:
                {
                    value = "unknown";
                    Debug.Assert( !Debugger.IsAttached, "Unhandled case" );
                    break;
                }
        }
        return value;
    }
}
