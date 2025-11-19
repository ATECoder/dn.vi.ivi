// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;
/// <summary>
/// Defines the contract that must be implemented by a SCPI Sense Voltage Subsystem.
/// </summary>
/// <remarks>
/// (c) 2012 Integrated Scientific ReSenses, Inc.<para>
/// Licensed under The MIT License. </para><para>
/// David, 2012-09-26, 1.0.4652. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="SenseResistanceSubsystemBase" /> class.
/// </remarks>
/// <remarks> David, 2020-07-28. </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
/// <param name="readingAmounts">  The reading amounts. </param>
[CLSCompliant( false )]
public abstract class SenseResistanceSubsystemBase( StatusSubsystemBase statusSubsystem, ReadingAmounts readingAmounts ) : SenseFunctionSubsystemBase( statusSubsystem, readingAmounts )
{
    #region " resistance range "

    /// <summary> The current. </summary>
    /// <summary> Gets or sets the current for the specific range. </summary>
    /// <value> The current. </value>
    public decimal Current
    {
        get;
        set
        {
            if ( value != this.Current )
            {
                field = value;
                this.NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the resistance range currents for either two- or four-wire resistance
    /// measurements.
    /// </summary>
    /// <value> The resistance range currents. </value>
    public ResistanceRangeCurrentCollection ResistanceRangeCurrents { get; private set; } = [];

    /// <summary> Gets or sets the resistance range current. </summary>
    /// <value> The resistance range current. </value>
    public ResistanceRangeCurrent? ResistanceRangeCurrent
    {
        get;
        set
        {
            if ( value is not null && (this.ResistanceRangeCurrent is null || value.ResistanceRange != this.ResistanceRangeCurrent.ResistanceRange) )
            {
                field = value;
                this.NotifyPropertyChanged();
                this.Current = value.RangeCurrent;
                _ = this.ApplyRange( ( double ) value.ResistanceRange );
            }
        }
    }

    #endregion
}
