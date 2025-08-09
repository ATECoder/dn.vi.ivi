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

IviVisaCompatibilityTester, Version=8.0.1.9352, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
Running under .NETCoreApp,Version=v9.0.
VISA.NET Shared Components version 8.0.0.0.
VISA Shared Components version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
```

### Identify
```
Checking IVI VISA Compatibility
Command: tcpip0::192.168.0.150::inst0::instr;

Make sure that the instrument at tcpip0::192.168.0.150::inst0::instr is turned on.

IviVisaCompatibilityTester, Version=8.0.1.9352, Culture=neutral, PublicKeyToken=456c916a0c4a68ef
Running under .NETCoreApp,Version=v9.0.
VISA.NET Shared Components version 8.0.1.0.
VISA Shared Components version 8.0.7331.0 detected.
Loaded Keysight.Visa, Version=18.5.0.0, Culture=neutral, PublicKeyToken=7a01cdb2a9131f73.
Instrument found at '192.168.0.150'.
ID: Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

## Feedback

IviVisaComparibilityTester is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi

