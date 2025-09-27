# About

[cc.isr.VI.Tsp.K2600.Ttm.MSTest] unit tests the Keithley 2600 source meter TTM functionality.

While tests can be run an any order, the sections below are listed by the order in which to run tests might be ran after making significant changes thus challenging changes commensurate with the hierarchical order of the software elements from the lowest to the highst level of abstraction.

# Contents

- [Preparing the instrument for testing](#preparing_the_instrument_for_testing)
  - [Meter Tests](#meter_tests)
  - [Firmware Tests](#firmware_tests)

<a name="preparing_the_instrument_for_testing"></a>
# Preparing the instrument for testing

1. Turn on the instrument;
1. Validate the instrument settings resource name against the actual instrument IP address;
1. Ping the instrument;
    1. It has been observed, at least on a VM, that the instrument is found by the VISA resource manager only after the instrument is pinged successfully.

<a name="meter_tests"></a>
# Meter Tests

| Test Set    | Test Name               | Notes |
|-------------|-------------------------|-------|
| Meter Tests | 01. Session should open |       |


<a name="contact_tests"></a>
# Contact Tess

| Test Set       | Test Name                                            | Notes |
|----------------|------------------------------------------------------|-------|
| Firmware Tests | 01. Session should open                              |       |
|                | 02. Current should measure                          |       |
|                | 03. Current should be measured multiple times        |       |
|                | 04. TSP syntax should not fail                       |       |
|                | 05. Meter value should reset                         |       |
|                | 06. Cold resistance defaults should equal settings   |       |
|                | 07. Initial resistance should reset                  |       |
|                | 08. Final resistance should reset                    |       |
|                | 09. Estimator should reset                           |       |
|                | 10. Thermal transient defaults should equal settings |       |
|                | 11. Thermal transient should reset                   |       |
|                | 12. Framework should clear known state               |       |
|                | 13. Measurement should be triggered                  |       |


# Key Features

Test settings are derived from a single JSon file consisting of multiple settings. The settings are provided by singleton instances of the relevant setting classes.

# Key Issue

## Support for .Net 4.72 and 4.8 Framework

Presently (Visual Studio 17.10.4), this project targets .NET 8.0 Core only. Unfortunately, logging fails when targeting legacy .NET frameworks such as 4.72 or 4.8 with the following exception:

```
System.IO.FileLoadException: Could not load file or assembly 'Serilog, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10' or one of its dependencies. The located assembly's manifest definition does not match the assembly reference. (Exception from HRESULT: 0x80131040)
```

This failure occurred upon attempting to configure the Serilog logging platform from our .NETSTANDARD 2.0 logging library. We are not sure at this time how this could be remedied. Apparently, a similar issue was reported in 2017 ([2017 issue]) that was reported as fixed in Visual Studio 15.5.

# Main Types

The main types provided by this library are:

* _CommandTests_: Tests the TTM commands.
* _ScriptTests_: Tests loading embedded scripts.
 
# Feedback

[cc.isr.VI.Tsp.K2600.Ttm.MSTest] is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi
[cc.isr.VI.Tsp.K2600.Ttm.MSTest]: https://github.com/atecoder/dn.vi.ivi/src/vi/k2600.ttm/k2600.ttm.mstest
