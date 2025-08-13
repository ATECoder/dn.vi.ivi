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

## Workaround

The above tests used the most recent IVI Foundation Nuget package. 

The following steps remove the exception:

1) Remove the reference to the IVI Foundation package and;
1) Add reference to `ivi.visa.dll` from `C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v4.0.30319\VISA.NET Shared Components 8.0.1`; or
1) Add reference to the KeysightTechnologies.Visa 18.5.73.

Listed below are the outcome of testing the application with the IVI Foundation packages and the Keysight installed `ivi.visa.dll`.

| Source             | Version       | 4.8 CLR | 9.0 CLR |
|--------------------|---------------|:-------:|:-------:|
| Keysight           | 2025.1.209    | Pass    | Pass    |
| Keysight.Visa      | 18.5.73       | Pass    | Pass    |
| IviFoundation.Visa | 8.0.0         | Pass    | Fail    |
| IviFoundation.Visa | 8.0.1         | Pass    | Fail    |
| IviFoundation.Visa | 8.0.2         | Pass    | Fail    |

Where IviFoundation.Visa and Keysight.Visa refer to Nuget packages with the specified version and *Keysight* refers to the version installed as part of the Keysight IO Suite libraries under `C:\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v4.0.30319\VISA.NET Shared Components 8.0.1`.

Apparently the IVI Foundation Nuget packages conflict with the shared component DLL that is installed with the Keysight 21.1.209 IO suite. Following are the date modified, version and sizes of the various `ivi.visa.dll` files. The IVI foundation files are under the Nuget `.\lib\net6` and the `.\ref\net4` folders

| Source             | Version       | Date Modified    | Version    | Size |
|--------------------|---------------|------------------|------------|------|
| Keysight           | 2025.1.209    | 2025-03-11 16:41 | 8.0.7511.0 | 248K |

| Source             | Net6 Version  | Date Modified    | Version    | Size |
|--------------------|---------------|------------------|------------|------|
| Keysight.Visa      | 18.5.73       | 2025-05-11 17:28 | 18.5.73    | 175K |
| IviFoundation.Visa | 8.0.0         | 2025-02-28 11:12 | 8.0.7428.0 | 254K |
| IviFoundation.Visa | 8.0.1         | 2025-03-11 09:42 | 8.0.7511.0 | 254K |
| IviFoundation.Visa | 8.0.2         | 2025-06-02 17:19 | 8.0.7803.0 | 254K |

| Source             | Net4 Version  | Date Modified    | Version    | Size |
|--------------------|---------------|------------------|------------|------|
| Keysight.Visa      | 18.5.73       | 2025-05-11 17:28 | 18.5.73    | 160K |
| IviFoundation.Visa | 8.0.0         | 2025-02-28 11:12 | 8.0.7428.0 | 111K |
| IviFoundation.Visa | 8.0.1         | 2025-03-11 09:41 | 8.0.7511.0 | 111K |
| IviFoundation.Visa | 8.0.2         | 2025-06-02 17:18 | 8.0.7803.0 | 111K |


C:\Program Files\IVI Foundation\VISA\Microsoft.NET\VendorAssemblies\kt\8.0\Keysight.Visa

