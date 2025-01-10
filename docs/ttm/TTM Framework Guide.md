# Thermal Transient Meter&trade; Framework Guide

## Table of Contents

- [Components](#Components)
  - [Meter](#Meter)
  - [TTM-Firmware](#TTM Firmware)
  - [Loader](#Loader)
  - [API](#API)
  - [Console](#Console)
  - [VISA Compatibility Tester](#Visa-Compatibility-Tester)
- [Revisions](#Revisions)
- [Terminology](#Terminology)
  - [Firmware](#Firmware)
  - [Loading](#Loading)
  - [Memory](#Memory)
  - [Startup](#Startup)
- [Using the Framework](#Using_the_Framework)
- [Requirements and Resources](#Requirements_and_Resources)
  - [FTP Site](#FTP_Site)
  - [Microsoft .NET Framework](#Microsoft_.NET_Framework)
  - [VISA Runtime](#VISA_Runtime)
- [Attributions](#Attributions)

<a name="Components"></a>
## Components

The ISR Thermal Transient Meter&trade; (TTM) Framework consists of the following components, programs and documents:

- [Meter](#Meter) - The ISR Thermal Transient Meter (TTM) Instrument.
- [TTM Firmware](#TTM-Firmware) - The ISR Thermal Transient Meter (TTM) [Firmware] that is loaded onto the [Meter](#Meter).
- [Loader](#Loader) - An application that loads the custom TTM firmware onto the [Keithley series 2600] instruments.
- [API](#API) - A [Microsoft .NET] library for communicating with the instrument over TCP/IP, USB or GPIB interfaces.
- [Console](#Console) - A stand-alone [Microsoft .NET] application for making Thermal Transient measurements.
- [VISA Compatibility Tester](#VISA-Compatibility-Tester) - A stand-alone [Microsoft .NET] application for looking for and checking the compatibility of the [IVI VISA] implementation on this computer.

<a name="Meter"></a>
### Meter

The ISR Thermal Transient Meter (TTM) Instrument is at the core of the framework. This instrument measures the thermal transient response of a resistive element to short current pulses. The instrument is capable of applying current pulses over a wide range of current levels and pulse widths. The instrument also measures the resistances before and after applying the current pulse. The instrument displays measurement values and outputs a digital word that can be used for binning the tested device.

The instrument consists of a [Keithley series 2600] instrument and a custom embedded [TTM Firmware](#TTM-Firmware), which extends the capabilities of the instrument for measuring thermal transients.

The [TTM Instrument Guide] describes the usage of the TTM as a stand-alone instrument.

<a name="TTM-Firmware"></a>
### TTM Firmware

The ISR Thermal Transient Meter (TTM) [Firmware] is loaded is a custom embedded firmware, which extends the capabilities of the native [Keithley series 2600] meter for measuring thermal transients.

<a name="Loader"></a>
### Loader

The ISR Thermal Transient Meter (TTM) Firmware Loader is a Windows program that loads the custom TTM firmware onto the [Keithley series 2600] instrument.

<a name="API"></a>
### API

The ISR Thermal Transient Meter (TTM) Application Programming Interface (API) is a .NET library (also called Driver) that can be used to communicate with the TTM instrument. The library is provided both as a dynamic link library (DLL) and a Visual Studio project with source code.

The [TTM Driver API Guide] describes how to use the TTM API to interface with the instrument remotely from a Windows computer.

<a name="Console"></a>
### Console

The ISR Thermal Transient Meter (TTM) Console (or Driver Console) is an application program for testing resistive devices using the TTM instrument. The Console is included as part of the TTM Framework both as an application program executable and a Visual Studio project with source code.

The [TTM Console Guide] describes how to use the Console.

<a name="Visa-Compatibility-Tester"></a>
### VISA Compatibility Tester

The TTM Framework [Microsoft .NET] API uses the Virtual Instruments (VISA) framework from the [IVI Foundation] for communicating with the TTM instrument. The [VISA Compatibility Tester Guide] describes how to use the Visa Compatibility Tester to determine if [IVI VISA] is installed on the computer and is compatible with the TTM .NET libraries and applications. 

<a name="Revisions"></a>
## Revisions

The TTM Framework has undergone a few revisions since it's inception in 2005. The following table attempts to provide the relationships between the various components and revisions that have been actively employed.

| Component | 2011 | 2025 |
|--------|-------------|-------------|
| Firmware | 2.3.4009 | 2.4.9111 |
| Loader | isr.Ttm.Loader 2.3.4049 | cc.isr.VI.Tsp.K2600.Ttmware.Loader 8.1.9210 |
| Driver | isr.Ttm.Driver 2.3.4077 | TBA |
| VISA | NI-VISA NS 8.1.20 | IVI-VISA 7.2.0 |

<a name="Terminology"></a>
## Terminology

<a name="Firmware"></a>
### Firmware

The ISR Thermal Transient Meter is implemented by customizing an off-the-shelf [Keithley series 2600] instrument. This is done by embedding the ISR Thermal Transient Meter firmware--the TTM Firmware--in this instrument. The TTM firmware consists of an embedded program written in the programming language of the instrument.

<a name="Loading"></a>
### Loading

The ISR Thermal Transient Meter comes already installed with the TTM Firmware. A stand-alone TTM Loader program is used for updating the embedded TTM firmware. This is done by removing (unloading) the TTM firmware embedded in the instrument and loading (installing) new TTM Firmware.

<a name="Memory"></a>
### Memory

When updated, the TTM Firmware resides in the instrument volatile memory. Firmware residing in volatile memory does not persist after restarting the instrument. During installation, the TTM Loader program saves the TTM Firmware to non-volatile memory. Firmware saved in non-volatile memory persists after the instrument power is toggled.

<a name="Startup"></a>
### Startup

When starting the TTM instrument, the instrument panel displays the Thermal Transient Meter version message, e.g., ___TTM 2.4.9111___ and author (__Integerated Scientific Resources__). In addition, the TTM Firmware is initiated to prepare the instrument for measurement. This is done by the *TTM Startup* or bootstrap firmware. The startup firmware is saved in non-volatile memory when the TTM fimware is loaded.

<a name="Using_the_Framework"></a>
## Using the Framework

The TTM Framework comes with the following guides:
- [TTM Framework Guide]: This guide.
- [TTM Firmware API Guide]: Describe how to interact with the Firmware by wat of TCP/IP, USB or GPIB using [IVI VISA].
- [TTM Driver API Guide]: Describes how to use the ISR TTM Driver.
- [TTM Driver API Upgrade Guide]: Describes how to upgrade software using the legacy ISR TTM driver.
- [TTM Instrument Guide]: Describes how to use the TTM Instrument manually or from application programs.
- [TTM Console Guide]: Describes how to take measurement using the TTM Console application.
- [VISA Compatibility Tester Guide]: how to use the Visa Compatibility Tester to determine if [IVI VISA] is installed on the computer and is compatible with the TTM .NET libraries and applications.

<a name="Requirements_and_Resources"></a>
## Requirements and Resources

<a name="FTP_Site"></a>
### FTP Site

The latest information about requirements can be found in the program read me file which can be downloaded from the [ISR FTP site].

The access to the [cc.isr.ftp] FTP site is protected by a password.

The user name is: ttm@isr.cc
The password is: ttm2.2

<a name="Microsoft_.NET_Framework"></a>
### Microsoft .NET Framework

The TTM applications are based on the Microsoft [.NET Framework] 9.0 and [Microsoft .NET Standard] 2.0, which is accessible from multiple .NET implementations.

<a name="VISA_Runtime"></a>
### VISA Runtime

The TTM Framework [Microsoft .NET] API uses the Virtual Instruments (VISA) framework from the [IVI Foundation] for communicating with the TTM instrument. [IVI VISA] is installed by members of the [IVI Foundation] such as Keysight, Rohde-Schwartz and NI (former National Instruments). Implementations such as the [Keysight IO Suite] or [NI Visa] must be installed for running applications based on [IVI VISA].

The current TTM software was developed based on version 7.2.0.0 of [IVI VISA]. Any VISA implementation, such as [Keysight IO Suite] version 21.1.47 is compatible with the ISR TTM API.

<a name="Attributions"></a>
## Attributions

Last Updated 2024-12-21 12:57:26

&copy; 2011 by Integrated Scientific Resources, Inc.  

This information is provided "AS IS" WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.

Licensed under [The Fair End User] and [MIT] Licenses.

Unless required by applicable law or agreed to in writing, this software is provided "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

Source code is hosted on [GitHub]

[The Fair End User]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/Fair%20End%20User%20License.html
[MIT]: http://opensource.org/licenses/MIT
[GitHub]: https://www.github.com/ATECoder
[IVI VISA]: https://www.ivi.org
[IVI FOUNDATION]: https://www.ivi.org
[Keysight IO Suite]: https://www.keysight.com/us/en/lib/software-detail/computer-software/io-libraries-suite-downloads-2175637.html
[NI Visa]: http://ftp.ni.com/support/softlib/visa/VISA%20Run-Time%20Engine
[.NET Framework]: https://dotnet.microsoft.com/en-us/download/dotnet/9.0
[ISR FTP Site]: http://bit.ly/aJgNDP
[cc.isr.ftp]: ftp://ftp.isr.cc
[Microsoft .NET]: https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet-framework
[Microsoft .NET Standard]: https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-1-0
[Keithley series 2600]: https://www.tek.com/en/products/keithley/source-measure-units/2600b-series-sourcemeter
[TTM Framework Guide]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/TTM%20Framework%20Guide.html
[TTM Firmware API Guide]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/TTM%20Firmware%20API%20Guide.html
[TTM Driver API Guide]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/TTM%20Driver%20API%20Guide.html
[TTM Driver API Upgrade Guide]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/TTM%20Driver%20API%20Upgrade%20Guide.html
[TTM Instrument Guide]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/TTM%20Instrument%20Guide.html
[VISA Compatibility Tester Guide]: https://github.com/ATECoder/dn.vi.ivi.git/docs/kelary/Visa%20Compatibility%20Tester%20Guide.html
[TTM Console Guide]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/TTM%20Console%20Guide.html
