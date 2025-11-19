// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.Visa.Gac;

/// <summary>   Implementation Vendor. </summary>
/// <remarks>   David, 2022-02-25. </remarks>
public static partial class Vendor
{

    /// <summary>   (Immutable) filename of the ivi visa file. </summary>
    public const string IVI_VISA_FILENAME = "Ivi.Visa.dll";

    /// <summary>
    /// (Immutable) The IVI visa implementation version supported by the <see href="https://www.nuget.org/packages/KeysightTechnologies.Visa/18.5.73"/>.
    /// This is the version of the ivi.visa.dll as reported by <see cref="Ivi.Visa.GlobalResourceManager.ImplementationVersion"/>.
    /// </summary>
    public const string IVI_VISA_IMPLEMENTATION_VERSION = "8.0.0.0";

    /// <summary>
    /// (Immutable) the ivi visa specification version supported by the <see href="https://www.nuget.org/packages/KeysightTechnologies.Visa/18.5.73"/>,
    /// which is hard coded into the <see cref="Ivi.Visa.GlobalResourceManager.SpecificationVersion"/>.
    /// </summary>
    public const string IVI_VISA_SPECIFICATION_VERSION = "7.4.0.0";

    /// <summary>   (Immutable) the full name IVI VISA. </summary>
    public const string IVI_VISA_FULL_NAME = "Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1";
}
