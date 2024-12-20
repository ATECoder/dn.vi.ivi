# About

[cc.isr.VI.Tsp.K2600.Ttm.MSTest] unit tests the Keithley 2600 source meter TTM functionality.

# How to Use

## Preeparing for testing

1. Turn on the instrument;
1. Validate the instrument settings resource name against the actual instrument IP address;
1. Ping the instrument;
    1. If has been observer, at least on a VM, that the instrument is found by the VISA resource manager only after the instrument is pinged once.

## Running Tests

When doing exploratory testing, for example, after new vendor implementation is installed, test must be best run in the following order:

1. Command Tests;
1. Script Tests;

# Key Features

Test settings are derived from a single JSon file consisting of multiple settings. The settings are provided by singleton instances of the relevant setting classes.

# Key Issue

## Support for .Net 4.72 and 4.8 Framework

Presently (Visual Studio 17.10.4), this project targets .NET 8.0 Core only. Unfortunately, logging fails when targeting legacy .NET frameworks such as 4.72 or 4.8 with the following exception:

```
System.IO.FileLoadException: Could not load file or assembly 'Serilog, Version=2.0.0.0, Culture=neutral, PublicKeyToken=24c2f752a8e58a10' or one of its dependencies. The located assembly's manifest definition does not match the assembly reference. (Exception from HRESULT: 0x80131040)
```

This failure occurred upon attempting to configure the Serilog logging platform form our .NETSTANDARD 2.0 logging library. We are not sure at this time how this could be remedied. Apparently, a similar issue was reported in 2017 ([2017 issue]) that was reported as fixed in Visual Studio 15.5.

# Main Types

The main types provided by this library are:

* _CommandTests_: Tests the TTM commands.
* _ScriptTests_: Tests loading embedded scripts.
 
# Feedback

[cc.isr.VI.Tsp.K2600.Ttm.MSTest] is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi
[cc.isr.VI.Tsp.K2600.Ttm.MSTest]: https://github.com/atecoder/dn.vi.ivi/src/vi/k2600.ttm/k2600.ttm.mstest
