// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;

/// <summary> Defines the Calculations SCPI subsystem (often CALC2. </summary>
/// <remarks>
/// (c) 2005 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-11-05. Created based on SCPI 5.1 library.  </para><para>
/// David, 2008-03-25, 5.0.3004. Port to new SCPI library. </para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="CalculateSubsystemBase" /> class.
/// </remarks>
/// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystemBase">status
/// subsystem</see>. </param>
[CLSCompliant( false )]
public abstract class CalculateSubsystemBase( StatusSubsystemBase statusSubsystem ) : SubsystemBase( statusSubsystem )
{
    #region " commands "

    /// <summary> Return the average of the buffer contents. </summary>
    /// <returns> The calculated buffer average. </returns>
    public double CalculateBufferAverage()
    {
        // select average
        _ = this.Session.WriteLine( "CALC2:FORM:MEAN" );

        // turn status on.
        _ = this.Session.WriteLine( "CALC2:STAT:ON" );

        // do the calculation.
        _ = this.Session.WriteLine( "CALC2:IMM" );

        // get the result
        return this.Session.Query( 0.0d, ":CALC2:DATA" );
    }

    #endregion
}
