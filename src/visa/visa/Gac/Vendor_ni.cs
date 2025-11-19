// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// https://github.com/dotnet/runtime/blob/f21a2666c577306e437f80fe934d76cdb15072a5/src/libraries/Common/src/Interop/Windows/Shell32/Interop.SHGetKnownFolderPath.cs

namespace cc.isr.Visa.Gac;

/// <summary>   Implementation Vendor. </summary>
/// <remarks>   David, 2022-02-25. </remarks>
public static partial class Vendor
{
#if true
    /// <summary>   (Immutable) path of the NI visa file. </summary>
    public const string NI_VISA_PATH = @"C:\Windows\assembly\GAC_MSIL\NationalInstruments.Visa\21.0.0.0__2eaa5af0834e221d";

    /// <summary>   (Immutable) filename of the NI visa file. </summary>
    public const string NI_VISA_FILENAME = "NationalInstruments.Visa.dll";

    /// <summary>   (Immutable) name of the NI visa full. </summary>
    public const string NI_VISA_FRIENDLY_NAME = "NationalInstruments.Visa";

    /// <summary>   (Immutable) full name of the NI visa. </summary>
    public const string NI_VISA_FULL_NAME = "NationalInstruments.Visa, Version=21.0.0.0, Culture=neutral, PublicKeyToken=2eaa5af0834e221d";

    /// <summary>   (Immutable) full name of the Keysight visa resource manager. </summary>
    public const string NI_RESOURCE_MANAGER_TYPE_NAME = "NationalInstruments.Visa.ResourceManager, NationalInstruments.Visa, Version=21.0.0.49304, Culture=neutral, PublicKeyToken=2eaa5af0834e221d";

#elif false
    /// <summary>   (Immutable) path of the NI visa file. </summary>
    public const string NI_VISA_PATH = @"C:\Windows\assembly\GAC_MSIL\NationalInstruments.Visa\21.0.0.0__2eaa5af0834e221d";

    /// <summary>   (Immutable) full name of the NI visa. </summary>
    public const string NI_VISA_FULL_NAME = "NationalInstruments.Visa, Version=21.0.0.0, Culture=neutral, PublicKeyToken=2eaa5af0834e221d";

    /// <summary>   (Immutable) full name of the Keysight visa resource manager. </summary>
    public const string NI_RESOURCE_MANAGER_TYPE_NAME = "NationalInstruments.Visa.ResourceManager, NationalInstruments.Visa, Version=21.0.0.49304, Culture=neutral, PublicKeyToken=2eaa5af0834e221d";

#endif
}
