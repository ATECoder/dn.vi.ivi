# IVI Visa Net Sample

## VISA

- [Keysight VISA.NET 21.1.209](https://www.keysight.com/us/en/software/keysight-io-suite.html)

## Required Packages
- [Ivi.Visa 8.0.2](https://www.nuget.org/packages/Ivi.Visa/)
- [KeysightTechnologies.Visa 18.5.73](https://www.nuget.org/packages/KeysightTechnologies.Visa/)

## Directions

1. Edit the command line using the instrument TcpIP resource name;
1. Turn on the instrument;
1. Run the program from the IDE or the command window.
1. Command: IviVisualStudioCompatibilityDemo tcpip0::192.168.0.144::inst0::instr 

## Status running under the Keysight IO Suite 21.1.209

### .NET 4.8 output
```
IviVisaNetSample, Version=8.0.1.9356, Culture=neutral, PublicKeyToken=null
        Running under .NETFramework,Version=v4.8 runtime .NET Framework 4.8.9310.0
Runtime Information:
        Framework Description: .NET Framework 4.8.9310.0
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.22631  (is Windows 11 if build >= 22000)
         Process Architecture: X64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.

Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.48.0

Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr'...

Reading instrument identification string...

tcpip0::192.168.0.150::inst0::instr Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

### .NET 9.0 output
```
IviVisaNetSample, Version=8.0.1.9356, Culture=neutral, PublicKeyToken=null
        Running under .NETCoreApp,Version=v9.0 runtime .NET 9.0.8
Runtime Information:
        Framework Description: .NET 9.0.8
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.22631 (is Windows 11 if build >= 22000)
         Process Architecture: X64
           Runtime Identifier: win-x64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7803.0.
        visaConfMgr version 8.0.7331.0 detected.

Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.73.0

Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr'...

Reading instrument identification string...

tcpip0::192.168.0.150::inst0::instr Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```
