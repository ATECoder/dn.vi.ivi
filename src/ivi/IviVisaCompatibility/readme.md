# IVI Visa Compatibility

## Category: VISA

## Description:

Starting with version 8.0.0, the IVI Foundationad began distributing their own version of the IVI.Visa assembly via NuGet. These packages are compatible with both .NET Code and .NET framework 4.x and load the compatible vendor implementations from the GAC when included in .NetStandard 2.0 projects. This library demonstrates how to use the IVI.Visa assembly from .NET Standard 2.showing how the implementation libraries load upon execution of the shared library (Ivi.Visa) parse method.

## [.Net Stadnard Compatibility Mode]

.NET Standard and .NET Core projects can reference .NET Framework libraries. .Net Framework libraries can be targeted from .NET Standard 2.0 using the [.Net Standard Compatibility Mode] with some exceptions such as if the library uses Windows Presentation Foundation (WPF) APIs.

## Using [Kelary.Ivi.Visa] with pre 8.0 versions of the IVI.Visa library

As described by the [Kelary Visa Net Sample Repository], IVI.Visa can be called via the [Kelary.Ivi.Visa] package provided that the vendor specific implementations are pre-loaded for .NET 5.0 and above.

## GacLoader

The GacLoader class is responsible for loading the appropriate VISA implementation from the Global Assembly Cache (GAC) based on the vendor specified in the configuration. It ensures that the correct assembly is loaded and available for use by the application.

The loader console output provides information about the loaded assembly, including its name, version, and location. This allows developers to verify that the correct VISA implementation is being used.

## Language: C#  

## Required Software: IVI-VISA  


## Required Hardware: Any message-based device

[IVI Foundation]: https://www.ivifoundation.org
[Keysight I/O Suite]: https://www.keysight.com/en/pd-1985909/io-libraries-suite
[NI VISA]: https://www.ni.com/en-us/support/downloads/drivers/download.ni-visa.html#346210
[Microsoft /.NET Framework]: https://dotnet.microsoft.com/download

[.NET standard]: https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-1-0
[Kelary Visa Net Sample Repository]: https://github.com/klry/IviVisaNetSample
[Kelary.Ivi.Visa]: https://www.nuget.org/packages/Kelary.Ivi.Visa
[cc.isr.Visa]: https://github.com/ATECoder/dn.vi.ivi.git
[VISA.NET Shared Components]: https://www.ivifoundation.org/Shared-Components/default.html#visa-and-visanet-shared-components
[VPP-4.3.6]: https://www.ivifoundation.org/downloads/VISA/vpp436_2024-02-08.pdf
[.Net Stadnard Compatibility Mode]: https://learn.microsoft.com/en-us/dotnet/core/porting/third-party-deps#net-framework-compatibility-mode
[Keysight IO Suite 21.1.17 2024-11-20]: https://www.keysight.com/us/en/software/keysight-io-suite.html
[Keysight IO Suite 21.1.209 2025-04-25]: https://www.keysight.com/us/en/software/keysight-io-suite.html
[Keysight IO Suite Suite 21.2.207 2025-09-10]: https://www.keysight.com/us/en/software/keysight-io-suite.html
[IviFoundation.Visa 8.0.2]: https://www.nuget.org/packages/IviFoundation.Visa/
[KeysightTechnologies.Visa 18.5.73]: https://www.nuget.org/packages/KeysightTechnologies.Visa/
[KeysightTechnologies.Visa 18.6.6]: https://www.nuget.org/packages/KeysightTechnologies.Visa/
[Kelary.Ivi.Visa 7.2.0]: https://www.nuget.org/packages/Kelary.Ivi.Visa
