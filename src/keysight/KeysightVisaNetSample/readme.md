# Keysight Visa Net Sample

## VISA

- [Keysight VISA.NET 21.2.207](https://www.keysight.com/us/en/software/keysight-io-suite.html)

## Required Packages
- [Ivi.Visa 8.0.2](https://www.nuget.org/packages/Ivi.Visa/)
- [KeysightTechnologies.Visa 18.6.6](https://www.nuget.org/packages/KeysightTechnologies.Visa/)

## Directions

1. Edit the command line using the instrument TcpIP resource name;
1. Turn on the instrument;
1. Run the program from the IDE or the command window.
1. Command: IviVisualStudioCompatibilityDemo tcpip0::192.168.0.144::inst0::instr 

## Output under the Keysight IO Suite 21.2.207

### Keysight 8.6.6 Package .NET 4.8 output
```
Running under .NETFramework,Version=v4.8 runtime .NET Framework 4.8.9221.0

VISA.NET Shared Components version 8.0.0.0.
VISA Shared Components version 8.0.7331.0 detected.

Executing GlobalResourceManager.Find( "TCPIP?*INSTR"  )

IVI GlobalResourceManager ImplementationVersion:8.0.0.0 SpecificationVersion:7.4.0.0
Selected the VISA.NET Implementation by Keysight Technologies, Inc. version 18.6.0.0


tcpip0::192.168.0.150::inst0::instr Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6

```

### .NET 10.0 output
```
Running under .NETCoreApp,Version=v10.0 runtime .NET 10.0.2

VISA.NET Shared Components version 8.0.0.0.
VISA Shared Components version 8.0.7331.0 detected.

Executing GlobalResourceManager.Find( "TCPIP?*INSTR"  )

IVI GlobalResourceManager ImplementationVersion:8.0.0.0 SpecificationVersion:7.4.0.0
Selected the VISA.NET Implementation by Keysight Technologies, Inc. version 18.6.0.0


tcpip0::192.168.0.150::inst0::instr Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6```

## Feedback

KeysightVisaNetSample is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi
