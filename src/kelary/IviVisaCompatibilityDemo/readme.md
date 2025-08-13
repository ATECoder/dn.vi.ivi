# IVI Visa Compatibility Demo

## Directions

1. Edit the command line using the instrument TcpIP resource name;
1. Turn on the instrument;
1. Run the program from the IDE or the command window.
1. Command: `IviVisualStudioCompatibilityDemo tcpip0::192.168.0.144::inst0::instr` 

## Keysight VISA 21.1.209 2025-04-25 with NI-VISA 8.0.1 referenced as a transitive package

### VISA

- [Keysight VISA.NET 21.1.209](https://www.keysight.com/us/en/software/keysight-io-suite.html)

### Packages
- [KeysightTechnologies.Visa 18.5.73](https://www.nuget.org/packages/KeysightTechnologies.Visa/)

In this case, the IVI Visa is picked up from the Nuget folder, which is a transitive dependency of the Keysight VISA package.

#### .NET 4.7.2
```
IviVisaCompatibilityDemo, Version=8.0.1.9356, Culture=neutral, PublicKeyToken=null
        Running under .NETFramework,Version=v4.7.2 runtime .NET Framework 4.8.9310.0

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.48.0

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR'...
        Reading instrument identification string...
        ID: Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

#### .NET 4.8

```
IviVisaCompatibilityDemo, Version=8.0.1.9356, Culture=neutral, PublicKeyToken=null
        Running under .NETFramework,Version=v4.8 runtime .NET Framework 4.8.9310.0

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.48.0

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR'...
        Reading instrument identification string...
        ID: Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

#### .NET 9.0
```
IviVisaCompatibilityDemo, Version=8.0.1.9356, Culture=neutral, PublicKeyToken=null
        Running under .NETCoreApp,Version=v9.0 runtime .NET 9.0.8

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.73.0

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR'...
        Reading instrument identification string...
        ID: Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

### Packages
- [Ivi.Visa 8.0.2](https://www.nuget.org/packages/Ivi.Visa/)
- [KeysightTechnologies.Visa 18.5.73](https://www.nuget.org/packages/KeysightTechnologies.Visa/)

### NI-VISA 8.0.2 referenced as a package

#### .NET 4.7.2
```
IviVisaCompatibilityDemo, Version=8.0.1.9352, Culture=neutral, PublicKeyToken=null
        Running under .NETFramework,Version=v4.7.2 runtime .NET Framework 4.8.9310.0

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.48.0

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR'...

Reading instrument identification string...

TCPIP0::192.168.0.150::inst0::INSTR Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

#### .NET 4.8
```
IviVisaCompatibilityDemo, Version=8.0.1.9352, Culture=neutral, PublicKeyToken=null
        Running under .NETFramework,Version=v4.8 runtime .NET Framework 4.8.9310.0

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.48.0

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR'...

Reading instrument identification string...

TCPIP0::192.168.0.150::inst0::INSTR Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

#### .NET 9.0
```
IviVisaCompatibilityDemo, Version=8.0.1.9352, Culture=neutral, PublicKeyToken=null
        Running under .NETCoreApp,Version=v9.0 runtime .NET 9.0.8

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7803.0.
        visaConfMgr version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.73.0

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR'...

Reading instrument identification string...

TCPIP0::192.168.0.150::inst0::INSTR Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

## Keysight VISA 21.1.17 2024-11-20

IVI Compatibility Package: Kelary.Ivi.Visa 7.2.0

### .NET 4.7.2
```
IviVisaCompatibilityDemo, Version=7.2.9110.36096, Culture=neutral, PublicKeyToken=null
Running under .NETFramework,Version=v4.7.2.
VISA.NET Shared Components version 7.2.0.0.
VISA Shared Components version 7.2.7619.0 detected.
Loaded Keysight.Visa, Version=18.4.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
Reading instrument identification string...
tcpip0::192.168.0.150::inst0::instr Identification string:
Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```
### .NET 4.8
```
IviVisaCompatibilityDemo, Version=7.2.9110.36096, Culture=neutral, PublicKeyToken=null
Running under .NETFramework,Version=v4.8.
VISA.NET Shared Components version 7.2.0.0.
VISA Shared Components version 7.2.7619.0 detected.
Loaded Keysight.Visa, Version=18.4.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
Reading instrument identification string...
tcpip0::192.168.0.150::inst0::instr Identification string:
Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

### .NET 9.0
```
IviVisaCompatibilityDemo, Version=7.2.9110.36096, Culture=neutral, PublicKeyToken=null
Running under .NETCoreApp,Version=v9.0.
VISA.NET Shared Components version 7.2.0.0.
VISA Shared Components version 7.2.7619.0 detected.
Loaded Keysight.Visa, Version=18.4.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
Reading instrument identification string...
tcpip0::192.168.0.150::inst0::instr Identification string:
Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

## Feedback

IviVisaComparibilityDemo is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi

