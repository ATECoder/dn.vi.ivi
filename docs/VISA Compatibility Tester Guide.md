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

IviVisaCompatibilityTester, Version=7.2.9111.101, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
Running under .NETCoreApp,Version=v9.0.
VISA.NET Shared Components version 7.2.0.0.
VISA Shared Components version 7.2.7619.0 detected.
Loaded Keysight.Visa, Version=18.4.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
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
Command: tcpip0::192.168.0.150::inst0::instr;

Make sure that the instrument at tcpip0::192.168.0.150::inst0::instr is turned on.

IviVisaCompatibilityTester, Version=7.2.9111.101, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
Running under .NETCoreApp,Version=v9.0.
VISA.NET Shared Components version 7.2.0.0.
VISA Shared Components version 7.2.7619.0 detected.
Loaded Keysight.Visa, Version=18.4.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
Instrument found at '192.168.0.150'.
ID: Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

Shows that the program found the instrument with the specified resources name.

Enter the following command:
```
cc.isr.Visa.Compatibility.Tester TCPIP::192.168.1.150:inst0:INSTR
```

The following outcome:
```
Checking IVI VISA Compatibility
Command: tcpip0::192.168.1.150::inst0::instr;

Make sure that the instrument at tcpip0::192.168.1.150::inst0::instr is turned on.

IviVisaCompatibilityTester, Version=7.2.9111.101, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
Running under .NETCoreApp,Version=v9.0.
VISA.NET Shared Components version 7.2.0.0.
VISA Shared Components version 7.2.7619.0 detected.
Loaded Keysight.Visa, Version=18.4.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
Attempt Ping instrument at '192.168.1.150' failed.
```

Shows that the program could not find the instrument with the specified resources name.

<a name="VISA_Runtime"></a>
## VISA Runtime

The TTM Framework [Microsoft .NET] API uses the Virtual Instruments (VISA) framework from the [IVI Foundation] for communicating with the TTM instrument. [IVI VISA] is installed by members of the [IVI Foundation] such as Keysight, Rohde-Schwartz and NI (former National Instruments). Implementations such as the [IO Suite] from Keysight or [NI Visa] from NI must be installed for running applications based on [IVI VISA].

The current TTM software was developed based on version 7.2.0.0 of [IVI VISA]. Any VISA implementation, such as [IO Suite] version 21.1.47 is compatible with the ISR TTM API.

<a name="Attributions"></a>
## Attributions

Last Updated 2024-12-23 18:14:13

&copy; 2011 by Integrated Scientific Resources, Inc.  

This information is provided "AS IS" WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.

Licensed under [MIT] License.

Unless required by applicable law or agreed to in writing, this software is provided "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

Source code is hosted on [Github]

[MIT]: http://opensource.org/licenses/MIT
[GitHub]: https://www.github.com/ATECoder
[IVI VISA]: https://www,ivi.org
[IVI FOUNDATION]: https://www,ivi.org
[IO Suite]: https://www.keysight.com/us/en/lib/software-detail/computer-software/io-libraries-suite-downloads-2175637.html
[NI Visa]: http://ftp.ni.com/support/softlib/visa/VISA%20Run-Time%20Engine
[.NET Framework]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[ISR FTP Site]: http://bit.ly/aJgNDP
[cc.isr.ftp]: ftp://ftp.isr.cc
[Microsoft .NET]: https://en.wikipedia.org/wiki/.NET_Framework
[Microsoft .NET Standard]: https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-1-0
[VISA Compatibility Tester Guide]: ./Visa20Compatibility20Tester%20Guide.html
