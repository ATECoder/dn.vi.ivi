// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.ComponentModel;
using cc.isr.Enums;
using cc.isr.Std.NumericExtensions;

namespace cc.isr.VI.Tsp.K2600.Ttm;

/// <summary> Thermal transient. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-23 </para>
/// </remarks>
public partial class ThermalTransient : ThermalTransientBase, ICloneable
{
    #region " construction and cloning "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public ThermalTransient() : base()
    {
    }

    /// <summary> Clones an existing measurement. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    public ThermalTransient( ThermalTransient value ) : base( value )
    {
        if ( value is not null )
        {
        }
    }

    /// <summary> Creates a new object that is a copy of the current instance. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A new object that is a copy of this instance. </returns>
    public object Clone()
    {
        return new ThermalTransient( this );
    }

    #endregion

    #region " i presettable "

    /// <summary> Sets values to their known clear execution state. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public override void DefineClearExecutionState()
    {
        base.DefineClearExecutionState();
        this.Asymptote = new double?();
        this.TimeConstant = new double?();
        this.EstimatedVoltage = new double?();
        this.CorrelationCoefficient = new double?();
        this.StandardError = new double?();
        this.Iterations = new int?();
        this.OptimizationOutcome = new OptimizationOutcome?();
    }

    #endregion

    #region " model values "

    /// <summary> The time constant. </summary>

    /// <summary> Gets or sets the time constant. </summary>
    /// <value> The time constant. </value>
    public double? TimeConstant
    {
        get;
        set
        {
            if ( this.TimeConstant.Differs( value, 0.000000001d ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the time constant caption. </summary>
    /// <value> The time constant caption. </value>
    public string TimeConstantCaption => this.TimeConstant.HasValue ? (1000d * this.TimeConstant.Value).ToString( "G4" ) : string.Empty;

    /// <summary> Gets or sets the asymptote. </summary>
    /// <value> The asymptote. </value>
    public double? Asymptote
    {
        get;
        set
        {
            if ( this.Asymptote.Differs( value, 0.000000001d ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the asymptote caption. </summary>
    /// <value> The asymptote caption. </value>
    public string AsymptoteCaption => this.Asymptote.HasValue ? (1000d * this.Asymptote.Value).ToString( "G4" ) : string.Empty;

    /// <summary> Gets or sets the estimated voltage. </summary>
    /// <value> The estimated voltage. </value>
    public double? EstimatedVoltage
    {
        get;
        set
        {
            if ( this.EstimatedVoltage.Differs( value, 0.000000001d ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the estimated voltage caption. </summary>
    /// <value> The estimated voltage caption. </value>
    public string EstimatedVoltageCaption => this.EstimatedVoltage.HasValue ? (1000d * this.EstimatedVoltage.Value).ToString( "G4" ) : string.Empty;

    /// <summary> Gets or sets the correlation coefficient. </summary>
    /// <value> The correlation coefficient. </value>
    public double? CorrelationCoefficient
    {
        get;
        set
        {
            if ( this.CorrelationCoefficient.Differs( value, 0.000001d ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the correlation coefficient caption. </summary>
    /// <value> The correlation coefficient caption. </value>
    public string CorrelationCoefficientCaption => this.CorrelationCoefficient.HasValue ? this.CorrelationCoefficient.Value.ToString( "G4" ) : string.Empty;

    /// <summary> Gets or sets the standard error. </summary>
    /// <value> The standard error. </value>
    public double? StandardError
    {
        get;
        set
        {
            if ( this.StandardError.Differs( value, 0.000001d ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the standard error caption. </summary>
    /// <value> The standard error caption. </value>
    public string StandardErrorCaption => this.StandardError.HasValue ? (1000d * this.StandardError.Value).ToString( "G4" ) : string.Empty;

    /// <summary> Gets or sets the iterations. </summary>
    /// <value> The iterations. </value>
    public int? Iterations
    {
        get;
        set
        {
            if ( !Nullable.Equals( value, this.Iterations ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the iterations caption. </summary>
    /// <value> The iterations caption. </value>
    public string IterationsCaption => this.Iterations.HasValue ? this.Iterations.Value.ToString() : string.Empty;

    /// <summary> Gets or sets the optimization outcome. </summary>
    /// <value> The optimization outcome. </value>
    public OptimizationOutcome? OptimizationOutcome
    {
        get;
        set
        {
            if ( !Nullable.Equals( value, this.OptimizationOutcome ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary> Gets the optimization outcome caption. </summary>
    /// <value> The optimization outcome caption. </value>
    public string OptimizationOutcomeCaption => this.OptimizationOutcome.HasValue ? this.OptimizationOutcome.Value.ToString() : string.Empty;

    /// <summary> Gets information describing the optimization outcome. </summary>
    /// <value> Information describing the optimization outcome. </value>
    public string OptimizationOutcomeDescription => this.OptimizationOutcome.HasValue ? this.OptimizationOutcome.Value.Description() : string.Empty;

    #endregion
}
/// <summary> Values that represent Optimization MeasurementOutcome. </summary>
/// <remarks> David, 2020-10-12. </remarks>
public enum OptimizationOutcome
{
    /// <summary> An enum constant representing the none option. </summary>
    [Description( "Not Specified" )]
    None,

    /// <summary> An enum constant representing the exhausted option. </summary>
    [Description( "Count Out--reached maximum number of iterations" )]
    Exhausted,

    /// <summary> An enum constant representing the optimized option. </summary>
    [Description( "Optimized--within the objective function precision range" )]
    Optimized,

    /// <summary> An enum constant representing the converged option. </summary>
    [Description( "Converged--within the argument values convergence range" )]
    Converged
}
