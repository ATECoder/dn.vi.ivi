// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.VI;
/// <summary>
/// Dictionary of ranges ordered by integer key, which could come from function modes.
/// </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-01-16 </para>
/// </remarks>
public class RangeDictionary : Dictionary<int, Std.Primitives.RangeR>
{ }
/// <summary> Dictionary of boolean values, which could be searched by function modes. </summary>
/// <remarks>
/// (c) 2016 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2016-02-08 </para>
/// </remarks>
public class BooleanDictionary : Dictionary<int, bool>
{ }
/// <summary>
/// Dictionary of <see cref="cc.isr.UnitsAmounts.Unit"/> which could be searched by function modes.
/// </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-03-30 </para>
/// </remarks>
public class UnitDictionary : Dictionary<int, cc.isr.UnitsAmounts.Unit>
{ }
/// <summary> Dictionary of integer values, which could be searched by function modes. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-03-30 </para>
/// </remarks>
public class IntegerDictionary : Dictionary<int, int>
{ }
