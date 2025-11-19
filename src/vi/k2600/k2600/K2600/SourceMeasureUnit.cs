// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600;

/// <summary> Source Measure Unit subsystem. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-14 </para>
/// </remarks>
public class SourceMeasureUnit : SourceMeasureUnitBase
{
    #region " construction and cleanup "

    /// <summary>   Initializes a new instance of the <see cref="SourceMeasureUnit" /> class. </summary>
    /// <remarks>   David, 2020-10-12. </remarks>
    /// <param name="statusSubsystem">  A reference to the
    ///                                 <see cref="P:isr.VI.Tsp.StatusSubsystemBase">TSP status
    ///                                 Subsystem</see>. </param>
    public SourceMeasureUnit( Tsp.StatusSubsystemBase statusSubsystem ) : base( statusSubsystem )
    {
    }

    /// <summary> Initializes a new instance of the <see cref="SourceMeasureUnit" /> class. </summary>
    /// <remarks>
    /// Note that the local node status clear command only clears the SMU status.  So, issue a CLS
    /// and RST as necessary when adding an SMU.
    /// </remarks>
    /// <param name="statusSubsystem"> A reference to a <see cref="StatusSubsystem">TSP status
    ///                                Subsystem</see>. </param>
    /// <param name="nodeNumber">      Specifies the node number. </param>
    /// <param name="smuNumber">       Specifies the SMU (either 'a' or 'b'. </param>
    public SourceMeasureUnit( StatusSubsystemBase statusSubsystem, int nodeNumber, string smuNumber ) : base( statusSubsystem, nodeNumber, smuNumber )
    {
    }

    #endregion
}
