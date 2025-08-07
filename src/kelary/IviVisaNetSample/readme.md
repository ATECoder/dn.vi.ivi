# IVI Visa Net Sample

## Directions

1. Edit the command line using the instrument TcpIP resource name;
1. Turn on the instrument;
1. Run the program from the IDE or the command window.
1. Command: IviVisualStudioCompatibilityDemo tcpip0::192.168.0.144::inst0::instr 

## Status running under the Keysight IO Suite 21.1.209

### .NET 4.8 output
```
Make sure that the instrument at tcpip0::192.168.0.150::inst0::instr is turned on.

IviVisaNetSample, Version=7.2.9243.107, Culture=neutral, PublicKeyToken=null
Running under .NETFramework,Version=v4.8.

VISA.NET Shared Components version 8.0.0.0.
VISA Shared Components version 8.0.7331.0 detected.

Loaded Visa Implementation:
   Friendly name: "Keysight VISA.NET".
       Full name: "Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73".

Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr'...

Reading instrument identification string...

tcpip0::192.168.0.150::inst0::instr Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

### .NET 9.0 output
```
IviVisaNetSample, Version=7.2.9243.107, Culture=neutral, PublicKeyToken=null
Running under .NETCoreApp,Version=v9.0.

VISA.NET Shared Components version 8.0.0.0.
VISA Shared Components version 8.0.7331.0 detected.

Loaded Visa Implementation:
   Friendly name: "Keysight VISA.NET".
       Full name: "Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73".

Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr'...
Exception: Method 'Read' in type 'Keysight.Visa.UsbSession' from assembly 'Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73' does not have an implementation.
```
