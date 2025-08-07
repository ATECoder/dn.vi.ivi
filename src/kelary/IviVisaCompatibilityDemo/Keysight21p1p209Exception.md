# Keysight IO Suite 21.1.209 2025-04-25 VISA fails opening a TCPIP session under .NET 9.0

https://support.keysight.com/KeysightdCX/s/my-support-cases?sq.all=01909947

I am seeing an exception attempting to open a TCPIP session with the Keysight IO Suite 21.1.209.

The exception occurs when running the IviVisaCompatibilityDemo console application under .NET 9.0.

Keysight IO Suite 21.1.17 worked fine.

## .NET Framework 4.8 output

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

## .NET 9.0 exception

```
IviVisaCompatibilityDemo, Version=7.2.9243.107, Culture=neutral, PublicKeyToken=null
Running under .NETCoreApp,Version=v9.0.
VISA.NET Shared Components version 8.0.0.0.
VISA Shared Components version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR'...
Exception: Method 'Read' in type 'Keysight.Visa.UsbSession' from assembly 'Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73' does not have an implementation.
```

## Code

The code that produces the error is:
```
    Console.WriteLine( $"Opening a VISA session to '{resourceName}'..." );
    using IVisaSession resource = Ivi.Visa.GlobalResourceManager.Open( resourceName, AccessModes.ExclusiveLock, 2000 );
```
