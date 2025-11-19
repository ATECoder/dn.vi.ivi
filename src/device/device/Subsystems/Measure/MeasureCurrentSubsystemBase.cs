// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

using System.Diagnostics;
using cc.isr.Std.TimeSpanExtensions;

namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a Measure Current Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class MeasureCurrentSubsystemBase : MeasureSubsystemBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Initializes a new instance of the <see cref="MeasureCurrentSubsystemBase" /> class.
    /// </summary>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
    /// subsystem</see>. </param>
    /// <param name="readingAmounts">  The reading amounts. </param>
    protected MeasureCurrentSubsystemBase( StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : base( statusSubsystem, readingAmounts ) => this.DefaultFunctionUnit = cc.isr.UnitsAmounts.StandardUnits.ElectricUnits.Ampere;

    #endregion

    #region " i presettable "

    /// <summary>
    /// Defines the know reset state (RST) by setting system properties to the their Reset (RST)
    /// default values.
    /// </summary>
    public override void DefineKnownResetState()
    {
        base.DefineKnownResetState();
        this.Level = 0;
    }

    #endregion

    #region " measure current "

    /// <summary> Waits for the current to exceed a current level. </summary>
    /// <param name="limen">   The threshold. </param>
    /// <param name="timeout"> The timeout. </param>
    /// <returns>
    /// <c>true</c> if level was reached before the <paramref name="timeout">timeout</paramref>
    /// expired; <c>false</c> otherwise.
    /// </returns>
    public bool AwaitMinimumLevel( double limen, TimeSpan timeout )
    {
        Stopwatch sw = Stopwatch.StartNew();
        do
        {
            _ = TimeSpan.FromMilliseconds( 1 ).AsyncWait();
            _ = this.MeasureReadingAmounts();
        }
        while ( (!this.Level.HasValue || limen > this.Level.Value) && sw.Elapsed <= timeout );
        return this.Level.HasValue && limen <= this.Level.Value;
    }

    /// <summary> Waits for the Current to attain a level. </summary>
    /// <param name="targetLevel"> The target level. </param>
    /// <param name="delta">       The delta. </param>
    /// <param name="timeout">     The timeout. </param>
    /// <returns>
    /// <c>true</c> if level was reached before the <paramref name="timeout">timeout</paramref>
    /// expired; <c>false</c> otherwise.
    /// </returns>
    public bool AwaitLevel( double targetLevel, double delta, TimeSpan timeout )
    {
        Stopwatch sw = Stopwatch.StartNew();
        bool hasValue;
        do
        {
            _ = TimeSpan.FromMilliseconds( 1 ).AsyncWait();
            _ = this.MeasureReadingAmounts();
            hasValue = this.Level.HasValue && Math.Abs( targetLevel - this.Level.Value ) <= delta;
        }
        while ( !hasValue && sw.Elapsed <= timeout );
        return hasValue;
    }

    /// <summary> Gets or sets the cached Current level. </summary>
    /// <value> The Current. </value>
    public double? Level
    {
        get;

        protected set
        {
            if ( !Nullable.Equals( this.Level, value ) )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    #endregion
}
