// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI.Tsp.K2600;

/// <summary> Sense subsystem. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2013-12-14 </para>
/// </remarks>
/// <remarks> Initializes a new instance of the <see cref="SenseSubsystem" /> class. </remarks>
/// <remarks> David, 2020-10-12. </remarks>
/// <param name="statusSubsystem">  A reference to the
///                                 <see cref="P:isr.VI.Tsp.StatusSubsystemBase">TSP status
///                                 Subsystem</see>. </param>
public class SenseSubsystem( Tsp.StatusSubsystemBase statusSubsystem ) : SenseSubsystemBase( statusSubsystem )
{
}
