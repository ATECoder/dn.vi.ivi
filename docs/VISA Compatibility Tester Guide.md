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

The following outcome:
```
```

Shows that the program found a compatible implementation.

The following outcome:
```
```

Shows that the program failed to find a compatible implementation 

<a name="Identify"></a>
### Identify

Enter the following command:
```
cc.isr.Visa.Compatibility.Tester -r TCPIP::192.168.0.150:inst0:INSTR
```

The following outcome:
```
```

Shows that the program found the instrument with the specified resources name.

Enter the following command:
```
cc.isr.Visa.Compatibility.Tester -r TCPIP::192.168.1.150:inst0:INSTR
```

The following outcome:
```
```
Shows that the program could not find the instrument with the specified resources name.


<a name="VISA_Runtime"></a>
## VISA Runtime

The TTM Framework [Microsoft .NET] API uses the Virtual Instruments (VISA) framework from the [IVI Foundation] for communicating with the TTM instrument. [IVI VISA] is installed by members of the [IVI Foundation] such as Keysight, Rohde-Schwartz and NI (former National Instruments). Implementations such as the [IO Suite] from Keysight or [NI Visa] from NI must be installed for running applications based on [IVI VISA].

The current TTM software was developed based on version 7.2.0.0 of [IVI VISA]. Any VISA implementation, such as [IO Suite] version 21.1.47 is compatible with the ISR TTM API.

<a name="Attributions"></a>
## Attributions

Last Updated 2024-12-21 12:57:26

&copy; 2011 by Integrated Scientific Resources, Inc.  

This information is provided "AS IS" WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.

Licensed under [The Fair End User] and [MIT] Licenses.

Unless required by applicable law or agreed to in writing, this software is provided "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

Source code is hosted on [Bit Bucket].and [Github]

[The Fair End User]: http://www.isr.cc/licenses/Fair%20End%20User%20Use%20License.pdf
[MIT]: http://opensource.org/licenses/MIT
[Bit Bucket].: https://www.bitbucket.org/davidhary
[GitHub].: https://www.github.com/ATECoder
[IVI VISA]: https://www,ivi.org
[IVI FOUNDATION]: https://www,ivi.org
[IO Suite]: https://www.keysight.com/us/en/lib/software-detail/computer-software/io-libraries-suite-downloads-2175637.html
[NI Visa]: http://ftp.ni.com/support/softlib/visa/VISA%20Run-Time%20Engine
[.NET Framework]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[ISR FTP Site]: http://bit.ly/aJgNDP
[cc.isr.ftp]: ftp://ftp.isr.cc
[Microsoft .NET]: https://en.wikipedia.org/wiki/.NET_Framework
[Microsoft .NET Standard]: https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-1-0
[TTM Framework Guide]: ./TTM%20Framework%20Guide.html
[TTM Firmware Upgrade Guide]: ./TTM%20Firmware%20Upgrade%20Guide.html
[TTM Firmware API Guide]: ./TTM%20Firmware%20API%20Guide.html
[TTM Driver API Guide]: ./TTM%20Driver%20API%20Guide.html
[TTM Driver API Upgrade Guide]: ./TTM%20Driver%20API%20Upgrade%20Guide.html
[TTM Instrument Guide]: ./TTM%20Instrument%20Guide.html
[VISA Compatibility Tester Guide]: ./Visa20Compatibility20Tester%20Guide.html
