# [cc.isr.Visa]

[cc.isr.Visa] is a .NET standard 2.0 library providing a unifying API for IVI supporting implementations from Keysight and NI visa.

## Description:

Starting with version 8.0.0, the IVI Foundationad began distributing their own version of the IVI.Visa assembly via NuGet. These packages are compatible with both .NET Code and .NET framework 4.x and load the compatible vendor implementations from the GAC when included in .NetStandard 2.0 projects. This library demonstrates how to use the IVI.Visa assembly from .NET Standard 2.showing how the implementation libraries load upon execution of the shared library (Ivi.Visa) parse method.

## [.Net Stadnard Compatibility Mode]

.NET Standard and .NET Core projects can reference .NET Framework libraries. .Net Framework libraries can be targeted from .NET Standard 2.0 using the [.Net Standard Compatibility Mode] with some exceptions such as if the library uses Windows Presentation Foundation (WPF) APIs.

## Using [Kelary.Ivi.Visa] with pre 8.0 versions of the IVI.Visa library

As described by the [Kelary Visa Net Sample Repository], IVI.Visa can be called via the [Kelary.Ivi.Visa] package provided that the vendor specific implementations are pre-loaded for .NET 5.0 and above.

# How to Use

This repository includes a sample console application that demonstrates how to use the [cc.isr.Visa] library to connect to a VISA compatible instrument and perform basic operations such as querying the instrument's identity and sending commands.

# Key Features

## GacLoader

The GacLoader class is responsible for loading the appropriate VISA implementation from the Global Assembly Cache (GAC) based on the vendor specified in the configuration. It ensures that the correct assembly is loaded and available for use by the application.

The loader console output provides information about the loaded assembly, including its name, version, and location. This allows developers to verify that the correct VISA implementation is being used.

# Main Types

The main types provided by this library are:

* GacLoader: A class responsible for loading the appropriate VISA implementation from the Global Assembly Cache (GAC) based on the vendor specified in the configuration.
* VisaAssemblyInfo: A class that contains information about the loaded VISA assembly, including its name, version, and location.
* IviVisaAssemblyInfo, KeysightVisaAssemblyInfo and NiVisaAssemblyInfo: Classes that inherit from VisaAssemblyInfo and provide specific information about the loaded VISA assembly for the IVI Foundation, Keysight, and NI implementations respectively.

# Feedback

[cc.isr.Visa] is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi
[cc.isr.Visa]: https://github.com/atecoder/dn.vi.ivi/src/visa
[Kelary Visa Net Sample Repository]: https://github.com/klry/IviVisaNetSample
[Kelary.Ivi.Visa]: https://www.nuget.org/packages/Kelary.Ivi.Visa
[Keysight IO Suite 21.1.17 2024-11-20]: https://www.keysight.com/us/en/software/keysight-io-suite.html
[Keysight IO Suite 21.1.209 2025-04-25]: https://www.keysight.com/us/en/software/keysight-io-suite.html
[Keysight IO Suite Suite 21.2.207 2025-09-10]: https://www.keysight.com/us/en/software/keysight-io-suite.html
[IviFoundation.Visa 8.0.2]: https://www.nuget.org/packages/IviFoundation.Visa/
[KeysightTechnologies.Visa 18.5.73]: https://www.nuget.org/packages/KeysightTechnologies.Visa/
[KeysightTechnologies.Visa 18.6.6]: https://www.nuget.org/packages/KeysightTechnologies.Visa/

