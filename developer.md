### Developer notes

These libraries includes the source code for accessing the Virtual Instrument Software Architecture (VISA) implementation by the [IVI Foundation].

#### Compatibility with IVI Visa

This [VI Repository] class libraries targets  VISA implementation such as [NI VISA] or [Keysight I/O Suite] from .NET Standard 2.0 using the [.Net Standard Compatibility Mode], which means that *... .NET Standard and .NET Core projects [can] reference .NET Framework libraries. Referencing .NET Framework libraries doesn't work for all projects, such as if the library uses Windows Presentation Foundation (WPF) APIs, but it does unblock many porting scenarios.*

As noted by [Kelary.Ivi.Visa], IVI can be called via the [Kelary.Ivi.Visa] package from .NET 5.0 and above by pre-loading the support libraries. This library demonstrated that Ivi.Visa can be called from .NET Standard 2.0 by pre-loading the support libraries.

#### [Kelary Notes](https://www.nuget.org/packages/Kelary.Ivi.Visa)

In the traditional approach, [VISA.NET Shared Components] are distributed solely as part of a vendor's installer for its VISA implementation.

This approach necessitates the installation of the vendor's VISA library implementation on the CI server environment, even if communication with instruments is not required at this stage.

If a developed application is intended to work with various third-party VISA implementations, and no VISA libraries are installed, it would be beneficial if it could provide only a part of the functionality or report the necessity of the VISA library.

This is a simple example of an application bypassing those limitations by using the unofficial NuGet VISA.NET Shared Components distribution, [Kelary.Ivi.Visa].

##### Using VISA.NET in .NET 5+ solutions ###

.NET Standard, introduced in 2016, marked a pivotal moment in the evolution of the .NET ecosystem.
Since then, many versions of .NET have emerged, diminishing the relevance of the traditional .NET Framework for new projects.

Despite these advancements, VISA.NET remains tied to .NET Framework 2.0, with no indication from the IVI Foundation of plans to embrace modern technologies.

A notable challenge is VISA.NET's limited integration into projects with contemporary .NET runtimes. However, compatibility has improved since .NET Core 2, allowing referencing of .NET Framework assemblies in compatibility mode. This suggests potential usability of the VISA.NET library and .NET Framework implementations within a modern .NET runtime.

Another challenge arises as the modern .NET runtime no longer uses the GAC, which VISA.NET relies on. This issue is mitigated by pre-loading VISA.NET implementations.

In this repository, an **experimental** .NET 8 example application showcases potential VISA.NET usage with various vendors implementations.

Despite progress, successful execution is not guaranteed, as the .NET runtime may lack support for all .NET Framework APIs.
Basic functions have been tested with Rohde & Schwarz and Keysight implementations, but some functions may require further testing.

We look forward to the IVI Foundation moving the VISA.NET library from the .NET Framework to the .Net Standard, which will allow VISA.NET to become not only interchangeable, but also cross-platform.

IVI Foundation announces in [VPP-4.3.6] that the *VISA.NET* shared components version 7.3 and later will be available in both *.NET Framework* versions that target *.NET 4.5*, and *.NET* versions that target *.NET 6.0* or after.

[IVI Foundation]: https://www.ivifoundation.org
[Keysight I/O Suite]: https://www.keysight.com/en/pd-1985909/io-libraries-suite
[NI VISA]: https://www.ni.com/en-us/support/downloads/drivers/download.ni-visa.html#346210
[Microsoft /.NET Framework]: https://dotnet.microsoft.com/download

[.NET standard]: https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-1-0
[Kelary.Ivi.Visa]: https://www.nuget.org/packages/Kelary.Ivi.Visa
[cc.isr.Visa]: https://www.bitbucket.org/davidhary/dn.visa
[VI Repository]: https://www.bitbucket.org/davidhary/dn.vi
[VISA.NET Shared Components]: https://www.ivifoundation.org/Shared-Components/default.html#visa-and-visanet-shared-components
[VPP-4.3.6]: https://www.ivifoundation.org/downloads/VISA/vpp436_2024-02-08.pdf
