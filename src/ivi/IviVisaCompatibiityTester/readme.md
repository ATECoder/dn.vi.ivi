# IVI Visa Compatibility Tester

## Directions

1. Turn on the instrument;
1. Run the following Batch Files from the command window.
  1. check.bat
  1. identify.bat

## Keysight VISA 21.2.207 2025-09-10 .NET 10

### Check
```
Checking IVI VISA Compatibility

IviVisaCompatibilityTester, Version=8.0.2.9391, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
        Running under .NETCoreApp,Version=v10.0 runtime .NET 10.0.1
Runtime Information:
        Framework Description: .NET 10.0.1
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.26200 (is Windows 11 if build >= 22000)
         Process Architecture: X64
           Runtime Identifier: win-x64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7803.0.
        visaConfMgr version 8.0.7331.0 detected.
*** resourceName cannot be null or empty.
```

### Identify
```
IviVisaCompatibilityTester, Version=8.0.2.9391, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
        Running under .NETCoreApp,Version=v10.0 runtime .NET 10.0.1
Runtime Information:
        Framework Description: .NET 10.0.1
              OS Architecture: X64
               OS Description: Microsoft Windows 10.0.26200 (is Windows 11 if build >= 22000)
         Process Architecture: X64
           Runtime Identifier: win-x64

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7803.0.
        visaConfMgr version 8.0.7331.0 detected.

Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
        Keysight.Visa.TcpipSession Visa session opened to 'TCPIP0::192.168.0.150::inst0::INSTR'.

Reading 'TCPIP0::192.168.0.150::inst0::INSTR' identity...
        VISA resource 'TCPIP0::192.168.0.150::inst0::INSTR' identified as:
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

        *** Failed to load assembly Keysight VISA.NET: Secondary AppDomains are not supported on this platform.
Loading assembly from ' Keysight.Visa, Version=18.6.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73'...
        Unloading assembly.


Closing session to 'TCPIP0::192.168.0.150::inst0::INSTR'...
```

## Keysight VISA 21.2.207 2025-09-10 .NET 9

### Check
```
IviVisaCompatibilityTester, Version=8.0.2.9391, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
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
resourceName is empty.
```

### Identify
```
IviVisaCompatibilityTester, Version=8.0.2.9391, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
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

Pinging 'TCPIP0::192.168.0.150::inst0::INSTR'...

Identifying 'TCPIP0::192.168.0.150::inst0::INSTR' session interface implementations and vendor types:

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


Reading 'TCPIP0::192.168.0.150::inst0::INSTR' identity...
        Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR' by:
        Ivi.Visa.GlobalResourceManager.ImplementationVersion:8.0.0.0
        Ivi.Visa.GlobalResourceManager.SpecificationVersion:7.4.0.0
        VISA Session open by Keysight Technologies, Inc. VISA.NET Implementation version 18.6.0.0
        Reading instrument identification string...
        VISA resource 'TCPIP0::192.168.0.150::inst0::INSTR' identified as:
        Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

## Keysight VISA 21.1.209 2025-04-25 .NET 9


### Check
```
Checking IVI VISA Compatibility

IviVisaCompatibilityTester, Version=8.0.1.9356, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
        Running under .NETCoreApp,Version=v9.0 runtime .NET 9.0.8

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.73.0
```

### Identify
```
Checking IVI VISA Compatibility
Command: TCPIP0::192.168.0.150::inst0::INSTR;

Make sure that the instrument at TCPIP0::192.168.0.150::inst0::INSTR is turned on.

IviVisaCompatibilityTester, Version=8.0.1.9356, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
        Running under .NETCoreApp,Version=v9.0 runtime .NET 9.0.8

VISA.NET Shared Components Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1.
        Version: 8.0.7511.0.
        visaConfMgr version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73, 18.5.73.0
Opening a VISA session to 'TCPIP0::192.168.0.150::inst0::INSTR'...
        Reading instrument identification string...
ID: Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

## Feedback

IviVisaComparibilityTester is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi

