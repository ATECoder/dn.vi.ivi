# Thermal Transient Meter&trade; Framework Guide

## Table of Contents

- [Components](#Components)
  - [Thermal Transient Meter](#Meter)
  - [Firmware Loader](#Loader)
  - [Application Programming Interface](#API)
  -  [Tester](#Tester)
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

The ISR Thermal Transient Meter (TTM) Framework consists of the following components, programs and documents:

<a name="Meter"></a>
### Meter

The ISR Thermal Transient Meter (TTM) Instrument is at the core of the framework. This instrument measures the thermal transient response of a resistive element to short current pulses. The instrument is capable of applying current pulses over a wide range of current levels and durations. The instrument also measures the resistances before and after applying the current pulse. The instrument displays measurement values and outputs a digital word that can be used for binning the tested device.

The instrument consists of a Keithley series 2600 instrument and a custom embedded firmware, which extends the capabilities of the instrument for measuring thermal transients.

The TTM Instrument Guide describes the usage of the TTM as a stand-alone instrument.

<a name="Loader"></a>
### Loader

The ISR Thermal Transient Meter (TTM) Firmware Loader is a Windows program that loads the custom TTM firmware onto the Keithley 2600 series instrument.

The TTM Loader Guide describes how to use the TTM Loader program.

<a name="API"></a>
### API

The ISR Thermal Transient Meter (TTM) Application Programming Interface (API) is a .NET library (ISR.TTM.Driver) that can be used to communicate with the TTM instrument. The library is provided both as a dynamic link library (DLL) and a Visual Studio project with source code.

The TTM Driver API Guide describes how to use the TTM API to interface with the instrument remotely from a Windows computer.

<a name="Tester"></a>
### Tester

The ISR Thermal Transient Meter (TTM) Tester (or Driver Tester) is an application program for testing resistive devices using the TTM instrument. The tester is included as part of the TTM Framework both as an application program executable and a Visual Studio project with source code.

The TTM Tester Guide describes how to use the Tester.

<a name="Terminology"></a>
## Terminology

<a name="Firmware"></a>
### Firmware

The ISR Thermal Transient Meter is implemented by customizing an off-the-shelf Keithley instrument. This is done by embedding the ISR Thermal Transient Meter firmware--the TTM Firmware--in this instrument. The TTM firmware consists of an embedded program written in the programming language of the instrument.

<a name="Loading"></a>
### Loading

The ISR Thermal Transient Meter comes already installed with the TTM Firmware. The TTM Loader program is used for updating the embedded TTM firmware. This is done by removing (unloading) the TTM firmware embedded in the instrument and loading (installing) new TTM Firmware.

<a name="Memory"></a>
### Memory

When updated, the TTM Firmware resides in the instrument volatile memory. Firmware residing in volatile memory is erased when turning off the instrument. The TTM Loader program is capable of saving the TTM Firmware to non-volatile memory. Firmware saved in non-volatile memory persists in the instrument.

<a name="Startup"></a>
### Startup

When starting the TTM instrument, the instrument panel displays the Thermal Transient Meter welcome message. In addition, the TTM Firmware is initiated to prepare the instrument for measurement. This is done by the *TTM Startup* or bootstrap firmware. The startup firmware is saved in non-volatile memory during the TTM loading process.

<a name="Using_the_Framework"></a>
## Using the Framework

The Application programming interface for the latest (2.4.9040) is yet to be released. 

The following information will become relevant once the new framework API and the console are released:

### TTM Loader Guide

An how to guide for the TTM Loader application

### TTM Instrument Guide

A user guide for bench testing resistive devices,

### TTM Tester Guide

A user guide for testing resistive devices from a PC using the TTM Driver Tester application.

### TTM Driver API Guide

A user guide for writing programs to control the TTM Instrument.

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

The TTM applications are based on the Microsoft [.NET Framework] 8.0. 

<a name="VISA_Runtime"></a>
### VISA Runtime

The TTM applications require the installation of either the [IO Suite] from Keysight or [NI Visa] from National Instruments, with support for the IVI Foundation IVI-VISA implementation version 5.7.

The current TTM software (2.4.9040) was developed and tested using the Keysight [IO Suite] version 18.2.27313, which can be downloaded from the *Previous Versions* section of the [IO Suite] downloads page. The most recent edition of [NI Visa] most likely supports a later version of the IVI-Visa thus requiring an earlier version of [NI Visa].

<a name="Attributions"></a>
## Attributions

Last Updated 2024-10-09

&copy; 2011 by Integrated Scientific Resources, Inc.  

This information is provided "AS IS" WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.

Licensed under [The Fair End User] and [MIT] Licenses.

Unless required by applicable law or agreed to in writing, this software is provided "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

Source code is hosted on [Bit Bucket].

[The Fair End User]: http://www.isr.cc/licenses/Fair%20End%20User%20Use%20License.pdf
[MIT]: http://opensource.org/licenses/MIT
[Bit Bucket]: https://bitbucket.org/davidhary
[IO Suite]: https://www.keysight.com/us/en/lib/software-detail/computer-software/io-libraries-suite-downloads-2175637.html
[NI Visa]: http://ftp.ni.com/support/softlib/visa/VISA%20Run-Time%20Engine
[.NET Framework]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[ISR FTP Site]: http://bit.ly/aJgNDP
[cc.isr.ftp]: ftp://ftp.isr.cc
[TTM Framework Guide]: TTM%20Framework%20Guide.md
[TTM Loader Guide]: TTM%20Loader%20Guide.md
[TTM API Guide]: TTM%20API%20Guide.md
