# VISA Compatibility Tester Guide

## Table of Contents

- [About](#About)
- [VISA Runtime](#VISA-Runtime)
- [Command Line](#Command-Line)
  - [Find Implementation](#Find-Implementation)
  - [Identify](#Identify)
- [Attributions](#Attributions)

<a name="About"></a>
## About

This guide describes how to use the `cc.isr.Visa.Compatibility.Tester` application to lookup and check the compatibility of the current instance of the [IVI VISA] implementation.

<a name="Command-Line"></a>
## Command Line

The `cc.isr.Visa.Compatibility.Tester` is run from the Windows Command Line.

<a name="Find-Implementation"></a>
### Find Implementation

Enter the following command:
```
cc.isr.Visa.Compatibility.Tester
```

or

```
.\cmds\check.bat
```

The following outcome:
```
Checking IVI VISA Compatibility

Checking IVI VISA Compatibility

IviVisaCompatibilityTester, Version=8.0.1.9356, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
        Running under .NETCoreApp,Version=v9.0 runtime .NET 9.0.8

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.73.0
```

Shows that the program found a compatible implementation.

The following outcome:
```
TBD
```

Shows that the program failed to find a compatible implementation 

<a name="Identify"></a>
### Identify

Enter the following command:
```
cc.isr.Visa.Compatibility.Tester TCPIP::192.168.0.150:inst0:INSTR
```

or

```
.\cmds\identify.bat
```


The following outcome:
```
Checking IVI VISA Compatibility
Command: TCPIP0::192.168.0.150::inst0::INSTR;

Turn on the instrument at TCPIP0::192.168.0.150::inst0::INSTR and press any key »
IviVisaCompatibilityTester, Version=8.0.2.9510, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
        Running under .NETCoreApp,Version=v10.0 runtime .NET 10.0.2
Runtime Information:
        Framework Description: .NET 10.0.2
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.26200 (is Windows 11 if build >= 22000)
         Process Architecture: X64
           Runtime Identifier: win-x64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7803.0.
        visaConfMgr version 8.0.7331.0 detected.

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
        Keysight.Visa.TcpipSession Visa session opened to 'TCPIP0::192.168.0.150::inst0::INSTR'.

Reading 'TCPIP0::192.168.0.150::inst0::INSTR' identity...
        VISA resource 'TCPIP0::192.168.0.150::inst0::INSTR' identified as:
        Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6

Identifying session implementations by type names:
        is a 'Ivi.Visa.IVisaSession'.
        is a 'Ivi.Visa.IMessageBasedSession'.
        is not a 'Ivi.Visa.ITcpipSession'.
        The 'Ivi.Visa.ITcpipSession2' type does not exist in 'Ivi.Visa'.
        is not a 'Ivi.Visa.ITcpipSocketSession'.
        The 'Ivi.Visa.ITcpipSocketSession2' type does not exist in 'Ivi.Visa'.
        is not a 'Ivi.Visa.IGpibInterfaceSession'.
        is not a 'Ivi.Visa.IGpibSession'.
        is a 'Ivi.Visa.INativeVisaSession'.
        is not a 'Ivi.Visa.IPxiBackplaneSession'.
        is not a 'Ivi.Visa.IPxiMemorySession'.
        is not a 'Ivi.Visa.IPxiSession'.
        The 'Ivi.Visa.IPxiSession2' type does not exist in 'Ivi.Visa'.
        is not a 'Ivi.Visa.IRegisterBasedSession'.
        is not a 'Ivi.Visa.ISerialSession'.
        is not a 'Ivi.Visa.IVxiBackplaneSession'.
        is not a 'Ivi.Visa.IVxiMemorySession'.
        is not a 'Ivi.Visa.IVxiSession'.

Loaded Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
        Version: 18.6.5.0.

Identifying session types by vendor type names:
        is a 'Keysight.Visa.MessageBasedSession'.
        is not a 'Keysight.Visa.GpibInterfaceSession'.
        is not a 'Keysight.Visa.GpibSession'.
        is not a 'Keysight.Visa.PxiBackplaneSession'.
        is not a 'Keysight.Visa.PxiMemorySession'.
        is not a 'Keysight.Visa.PxiSession'.
        is not a 'Keysight.Visa.RegisterBasedSession'.
        is not a 'Keysight.Visa.SerialSession'.
        is a 'Keysight.Visa.TcpipSession'.
        is not a 'Keysight.Visa.TcpipSocketSession'.
        is not a 'Keysight.Visa.UsbSession'.
        is a 'Keysight.Visa.VisaSession'.
        is not a 'Keysight.Visa.VxiBackplaneSession'.
        is not a 'Keysight.Visa.VxiMemorySession'.
        is not a 'Keysight.Visa.VxiSession'.

Identifying session interface implementations by vendor type names:
        is a 'Keysight.Visa.IKeysightNativeVisaSession'.



Closing session to 'TCPIP0::192.168.0.150::inst0::INSTR'...
```

Shows that the program found the instrument with the specified resources name.

<a name="VISA_Runtime"></a>
## VISA Runtime

The Visa Compatibility API uses the Virtual Instruments (VISA) framework from the [IVI Foundation] for communicating with the TTM instrument. [IVI VISA] is installed by members of the [IVI Foundation] such as Keysight, Rohde-Schwartz and NI (former National Instruments). Implementations such as the [IO Suite] from Keysight or [NI Visa] from NI must be installed for running applications based on [IVI VISA].

The current TTM software was developed based on version 8.0.2 of [IVI VISA], which loades Keysight VISA version 18.6.5 from the GAC.

<a name="Attributions"></a>
## Attributions

Last Updated 2026-01-14

&copy; 2011 by Integrated Scientific Resources, Inc.  

This information is provided "AS IS" WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.

Licensed under [MIT] License.

Unless required by applicable law or agreed to in writing, this software is provided "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

Source code is hosted on [GitHub]

[MIT]: http://opensource.org/licenses/MIT
[GitHub]: https://www.github.com/ATECoder
[IVI VISA]: https://www,ivi.org
[IVI FOUNDATION]: https://www,ivi.org
[IO Suite]: https://www.keysight.com/en/pd-1985909/io-libraries-suite
[Keysight I/O Suite]: https://www.keysight.com/en/pd-1985909/io-libraries-suite
[Keysight I/O Suite Downloads]: https://www.keysight.com/find/iosuite
[NI Visa]: http://ftp.ni.com/support/softlib/visa/VISA%20Run-Time%20Engine
[.NET Framework]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[ISR FTP Site]: http://bit.ly/aJgNDP
[cc.isr.ftp]: ftp://ftp.isr.cc
[Microsoft .NET]: https://en.wikipedia.org/wiki/.NET_Framework
[Microsoft .NET Standard]: https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-1-0
[VISA Compatibility Tester Guide]: ./Visa20Compatibility20Tester%20Guide.html
