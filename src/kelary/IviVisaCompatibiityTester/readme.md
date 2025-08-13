# IVI Visa Compatibility Tester

## Directions

1. Turn on the instrument;
1. Run the following Batch Files from the command window.
  1. check.bat
  1. identify.bat

## Keysight VISA 21.1.209 2025-04-25


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

