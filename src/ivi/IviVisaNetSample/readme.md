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

## Status running under the Keysight IO Suite 21.2.207

### IVIFoundation.Visa 8.0.2 Package .NET 4.8

The VISA implementation picks up Keysight 18.6.5 from the GAC which is compiled against .NET 4.0.

```
IviVisaNetSample, Version=8.0.2.9510, Culture=neutral, PublicKeyToken=null
        Running under .NETFramework,Version=v4.8 runtime .NET Framework 4.8.9221.0
Runtime Information:
        Framework Description: .NET Framework 4.8.9221.0
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.26200  (is Windows 11 if build >= 22000)
         Process Architecture: X64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.

Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
        Keysight.Visa.TcpipSession Visa session opened to 'tcpip0::192.168.0.150::inst0::instr'.

Reading 'tcpip0::192.168.0.150::inst0::instr' identity...
        VISA resource 'tcpip0::192.168.0.150::inst0::instr' identified as:
        Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6

Identifying session implementations by type names:
        is a 'Ivi.Visa.IVisaSession'.
        is a 'Ivi.Visa.IMessageBasedSession'.
        is a 'Ivi.Visa.ITcpipSession'.
        is a 'Ivi.Visa.ITcpipSession2'.
        is not a 'Ivi.Visa.ITcpipSocketSession'.
        is not a 'Ivi.Visa.ITcpipSocketSession2'.
        is not a 'Ivi.Visa.IGpibInterfaceSession'.
        is not a 'Ivi.Visa.IGpibSession'.
        is a 'Ivi.Visa.INativeVisaSession'.
        is not a 'Ivi.Visa.IPxiBackplaneSession'.
        is not a 'Ivi.Visa.IPxiMemorySession'.
        is not a 'Ivi.Visa.IPxiSession'.
        is not a 'Ivi.Visa.IPxiSession2'.
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



Closing session to 'tcpip0::192.168.0.150::inst0::instr'...

```

### IVIFoundation.Visa 8.0.2 Package .NET 10.0

The VISA implementation picks up Keysight 18.6.5 from the GAC which is compiled against .NET 10. 

```
IviVisaNetSample, Version=8.0.2.9510, Culture=neutral, PublicKeyToken=null
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

Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
        Keysight.Visa.TcpipSession Visa session opened to 'tcpip0::192.168.0.150::inst0::instr'.

Reading 'tcpip0::192.168.0.150::inst0::instr' identity...
        VISA resource 'tcpip0::192.168.0.150::inst0::instr' identified as:
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



Closing session to 'tcpip0::192.168.0.150::inst0::instr'...

```

## Status running under the Keysight IO Suite 21.2.207

### Keysight 18.6.6 Package .NET 4.8 output

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

Loading VISA implementation assemblies
        Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.6.5.0
        Loaded Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
        Version: 18.6.5.0.

Pinging 'tcpip0::192.168.0.150::inst0::instr'...

Identifying 'tcpip0::192.168.0.150::inst0::instr' session interface implementations and vendor types:

Identifying session implementations by type names:
        is a 'Ivi.Visa.IVisaSession'.
        is a 'Ivi.Visa.IMessageBasedSession'.
        is a 'Ivi.Visa.ITcpipSession'.
        is a 'Ivi.Visa.ITcpipSession2'.
        is not a 'Ivi.Visa.ITcpipSocketSession'.
        is not a 'Ivi.Visa.ITcpipSocketSession2'.
        is not a 'Ivi.Visa.IGpibInterfaceSession'.
        is not a 'Ivi.Visa.IGpibSession'.
        is a 'Ivi.Visa.INativeVisaSession'.
        is not a 'Ivi.Visa.IPxiBackplaneSession'.
        is not a 'Ivi.Visa.IPxiMemorySession'.
        is not a 'Ivi.Visa.IPxiSession'.
        is not a 'Ivi.Visa.IPxiSession2'.
        is not a 'Ivi.Visa.IRegisterBasedSession'.
        is not a 'Ivi.Visa.ISerialSession'.
        is not a 'Ivi.Visa.IVxiBackplaneSession'.
        is not a 'Ivi.Visa.IVxiMemorySession'.
        is not a 'Ivi.Visa.IVxiSession'.


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


Reading 'tcpip0::192.168.0.150::inst0::instr' identity...
Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
        VISA Session open by Keysight Technologies, Inc. VISA.NET Implementation version 18.6.0.0
        Reading instrument identification string...
        VISA resource 'tcpip0::192.168.0.150::inst0::instr' identified as:
        Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

### Keysight 18.6.6 Package .NET 9.0 output
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

Loading VISA implementation assemblies
        Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.6.6.0
        Loaded Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
        Version: 18.6.6.0.

Pinging 'tcpip0::192.168.0.150::inst0::instr'...

Identifying 'tcpip0::192.168.0.150::inst0::instr' session interface implementations and vendor types:

Identifying session implementations by type names:
        is a 'Ivi.Visa.IVisaSession'.
        is a 'Ivi.Visa.IMessageBasedSession'.
        is a 'Ivi.Visa.ITcpipSession'.
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


Reading 'tcpip0::192.168.0.150::inst0::instr' identity...
Opening a VISA session to 'tcpip0::192.168.0.150::inst0::instr' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
        VISA Session open by Keysight Technologies, Inc. VISA.NET Implementation version 18.6.0.0
        Reading instrument identification string...
        VISA resource 'tcpip0::192.168.0.150::inst0::instr' identified as:
        Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

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
