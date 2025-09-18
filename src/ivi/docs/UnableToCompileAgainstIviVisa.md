# Unable to Compile Against IVI Visa DLL with I/O Suites After 21.1.17

## Summary
Reported below are observations indicating that applications using Keysight I/O suites 21.1.209 and 2.2.207 no longer work when referencing only the IVI Visa DLL.

This breaks our ability to write vendor-independent applications, which has been the core raison d'etre of the IVI foundation.

## Overview

For some years I have been using the Kelary package (https://www.nuget.org/packages/Kelary.Ivi.Visa) under .NET 4.72, 4.8, 8.0 and 9.0 with earlier editions of Keysight VISA either with the 5.11 and 7.2 implementations of IVI Visa. Thus, while my applications were referencing only the IVI.Visa.dll and were coded against the defined interfaces only, they ran with any vendor implementation that the user chose. On https://www.nuget.org/packages/Kelary.Ivi.Visa readme are listed quite a few of compatible vendor implementations including NI, Keysight and Rohde-Schwartz. 

That capability broke down with the release of Keysight I/O suites 2.1.209 and 2.2.207.

Namely, referencing only the IVI.Visa.dll or IVI Foundation packages 8.0.1 (for 21.1.209) or 8.0.2 (for 21.2.20&) no longer works. A reference to the Keysight.VISA package is now required. 

## Specifically

As shown below, referencing the IVI Visa DLL alone breaks the application under .NET 4.8 or 9.0 depending on the version of Keysight VISA DLL installed in the GAC.

Keysight released two versions of VISA with I/O suite 21.2.207: 18.6.5 and 18.6.6 each partially compatible with IVI Visa 8.0.7803 depending on the .NET framework version.

## Request and Questions

I have the following questions/requests:

1) Will an application referencing the Keysight.VISA package work with other implementations such as NI or Rohde-Schwartz?
2) Will Keysight fix these limitations (bugs?).

I am reporting below the exceptions I am getting hoping the R&D group can restore what was working in earlier versions of Keysight VISA.

## Observations

### Keysight 2.1.209 with IVIFoundation.Visa package 8.0.1

In this case, the application works with .NET 4.0 but not with .NET 9.0

#### .NET 4.8
```
IviVisaCompatibilityDemo, Version=7.2.9243.107, Culture=neutral, PublicKeyToken=null
Running under .NETFramework,Version=v4.8.
VISA.NET Shared Components version 8.0.0.0.
VISA Shared Components version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR'...

Reading instrument identification string...

TCPIP0::192.168.0.150::inst0::INSTR Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

#### .NET 9.0

```
IviVisaCompatibilityDemo, Version=7.2.9243.107, Culture=neutral, PublicKeyToken=null
Running under .NETCoreApp,Version=v9.0.
VISA.NET Shared Components version 8.0.0.0.
VISA Shared Components version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR'...
Exception: Method 'Read' in type 'Keysight.Visa.UsbSession' from assembly 'Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73' does not have an implementation.
```

### Keysight 21.2.207 with IVIFoundation.Visa package 8.0.2

Here again, the application works with .NET 4.0 but not with .NET 9.0

#### .NET 4.8

```
IviVisaNetSample, Version=8.0.2.9391, Culture=neutral, PublicKeyToken=null
        Running under .NETFramework,Version=v4.8 runtime .NET Framework 4.8.9310.0
Runtime Information:
        Framework Description: .NET Framework 4.8.9310.0
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.22631  (is Windows 11 if build >= 22000)
         Process Architecture: X64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.

Preloading VISA implementation assembly
Loaded Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.6.5.0
Identifying 'tcpip0::192.168.0.150::inst0::instr' session implementations by type casting:
        is a 'Ivi.Visa.IMessageBasedSession'.
        The type 'Keysight.Visa.MessageBasedSession' does not exist in the namespace 'Ivi.Visa'.
        The type 'Keysight.Visa.TcpipSession' does not exist in the namespace 'Ivi.Visa'.
        is a 'Ivi.Visa.IVisaSession'.
        is a 'Ivi.Visa.ITcpipSession'.
        is a 'Ivi.Visa.ITcpipSession2'.


Reading 'tcpip0::192.168.0.150::inst0::instr' identity...
Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
        VISA Session open by Keysight Technologies, Inc. VISA.NET Implementation version 18.6.0.0
        Reading instrument identification string...
        VISA resource 'tcpip0::192.168.0.150::inst0::instr' identified as:
        Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

#### .NET 9.0

```
IviVisaNetSample, Version=8.0.2.9391, Culture=neutral, PublicKeyToken=null
        Running under .NETCoreApp,Version=v9.0 runtime .NET 9.0.9
Runtime Information:
        Framework Description: .NET 9.0.9
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.22631 (is Windows 11 if build >= 22000)
         Process Architecture: X64
           Runtime Identifier: win-x64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7803.0.
        visaConfMgr version 8.0.7331.0 detected.

Preloading VISA implementation assembly
Loaded Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.6.5.0
Exception: Method 'Read' in type 'Keysight.Visa.UsbSession' from assembly 'Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73' does not have an implementation.

Reading 'tcpip0::192.168.0.150::inst0::instr' identity...
Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
Exception: Method 'Read' in type 'Keysight.Visa.UsbSession' from assembly 'Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73' does not have an implementation.
Failed to identify VISA resource 'tcpip0::192.168.0.150::inst0::instr'.
```

## Replace Keysight.VISA 18.6.5 in the GAC with Keysight.VISA 18.6.6.

I/O Suite 21.2.207 installs Keysight Visa 18.6.5 in the GAC whereas the version of Keysight Visa DLL in the Nuget directory is 18.6.6.

So, I copied Keysight 18.6.6 to the GAC and here is what I get:

#### .NET 9: Works

```
IviVisaNetSample, Version=8.0.2.9391, Culture=neutral, PublicKeyToken=null
        Running under .NETCoreApp,Version=v9.0 runtime .NET 9.0.9
Runtime Information:
        Framework Description: .NET 9.0.9
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.22631 (is Windows 11 if build >= 22000)
         Process Architecture: X64
           Runtime Identifier: win-x64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7803.0.
        visaConfMgr version 8.0.7331.0 detected.

Preloading VISA implementation assembly
Loaded Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.6.6.0
Identifying 'tcpip0::192.168.0.150::inst0::instr' session implementations by type casting:
        is a 'Ivi.Visa.IMessageBasedSession'.
        The type 'Keysight.Visa.MessageBasedSession' does not exist in the namespace 'Ivi.Visa'.
        The type 'Keysight.Visa.TcpipSession' does not exist in the namespace 'Ivi.Visa'.
        is a 'Ivi.Visa.IVisaSession'.
        is a 'Ivi.Visa.ITcpipSession'.

Identifying 'tcpip0::192.168.0.150::inst0::instr' session implementations by type names:
        is a 'Ivi.Visa.ITcpipSession'.
        The type 'Ivi.Visa.ITcpipSession2' does not exist in the namespace 'Ivi.Visa'.


Reading 'tcpip0::192.168.0.150::inst0::instr' identity...
Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
        VISA Session open by Keysight Technologies, Inc. VISA.NET Implementation version 18.6.0.0
        Reading instrument identification string...
        VISA resource 'tcpip0::192.168.0.150::inst0::instr' identified as:
        Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

#### .NET 4.8 Fails

```
IviVisaNetSample, Version=8.0.2.9391, Culture=neutral, PublicKeyToken=null
        Running under .NETFramework,Version=v4.8 runtime .NET Framework 4.8.9310.0
Runtime Information:
        Framework Description: .NET Framework 4.8.9310.0
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.22631  (is Windows 11 if build >= 22000)
         Process Architecture: X64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.

Preloading VISA implementation assembly
Loaded Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.6.6.0
Exception: Failed to parse tcpip0::192.168.0.150::inst0::instr

Reading 'tcpip0::192.168.0.150::inst0::instr' identity...
Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
Exception: Failed to parse tcpip0::192.168.0.150::inst0::instr
Failed to identify VISA resource 'tcpip0::192.168.0.150::inst0::instr'.
```

## Suggestions / Requests

1) Please restore the previous capabilities allowing the reference only the IVI Visa DLL when compiling the application;

2) Consider compiling the packages against .NET Standard 2.0

As suggested and wished-for by Kelary and myself, these intricacies can be removed by compiling IVI Visa and Keysight VISA against .NET standard 2.0. 

Indeed, to validate this premise, I decompiled and then recompiled IVI Visa 5.11 and the Keysight Visa under .net standard 2.0 and could call the underlying implementation with no issues. 

I believe and so did Kelary that compiling IVI Visa and Keysight VISA under .NET standard 2.0 is doable and sensible and will solve tons of compatibility issues like the one reported above naturally.


