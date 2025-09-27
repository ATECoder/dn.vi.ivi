# About

[cc.isr.VI.Tsp.K2600.MSTest] unit tests the Keithley 2600 source meter.

While tests can be run an any order, the sections below are listed by the order in which to run tests might be ran after making significant changes thus challenging changes commensurate with the hierarchical order of the software elements from the lowest to the highst level of abstraction.

# Contents

- [Preparing the instrument for testing](#preparing_the_instrument_for_testing)
- [Resource Tests](#resource_tests)
  - [Resource Manager Tests](#resource_manager_tests)
- [Visa Tests](#visa_tests)
  - [Visa Session Tests](#visa_session_tests)

<a name="preparing_the_instrument_for_testing"></a>
# Preparing the instrument for testing

1. Turn on the instrument;
1. Validate the instrument settings resource name against the actual instrument IP address;
1. Ping the instrument;
    1. It has been observed, at least on a VM, that the instrument is found by the VISA resource manager only after the instrument is pinged successfully.

<a name="resource_tests"></a>
# Resource Tests

Namespace: `cc.isr.VI.Tsp.K2600.MSTest.Resource`

<a name="resource_manager_tests"></a>
## Resource Manager Tests

Test Class: `ResourceManagerTests

### 01. Resource name should be included

<a name="visa_tests"></a>
# Visa Tests

Namespace: `cc.isr.VI.Tsp.K2600.MSTest.Visa`

<a name="visa_tests_device_tests"></a>
### Device Tests

Test Class: `DeviceTests

<a name="visa_tests_parse_tests"></a>
### Parse Tests

<a name="visa_tests_session_base_tests"></a>
### Session Base Tests

<a name="visa_tests_visa_resource_tests"></a>
### Visa Resource Tests

<a name="visa_tests_visa_session_tests"></a>
### Visa Session Tests

Test Class: `VisaSessionTests

<a name="status_only_tests"></a>
# Status Only Tests

Namespace: `cc.isr.VI.Tsp.K2600.MSTest.Status`

<a name="status_only_tests_device"></a>
## Device Status Only Tests

### 01. Device should open without device errors

<a name="subsystem_tests"></a>
# Subsystem Tests;

Namespace: `cc.isr.VI.Tsp.K2600.MSTest.Status`

<a name="status_only_tests_device"></a>
# Subsystem Tests;

# Current Source Tests;

# Resistance Tests.

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

* _CurrentSourceTests_: Tests making current source measurements.
* _DeviceStatusOnlyTests_: Tests the device status subsystem.
* _DeviceTests_: Tests the device core functions.
* _ResistanceTests_: Tests making resistance measurements.
* _ResourcemanagerTests_: Tests the VISA resource manager.
* _SubsystemsTests_: Tests instrument subsystems such as: status.
* _VisaSessionTests_: Tests accessing the instrument via the VISA session.
 
# Feedback

[cc.isr.VI.Tsp.K2600.MSTest] is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi
[cc.isr.VI.Tsp.K2600.MSTest]: https://github.com/atecoder/dn.vi.ivi/src/vi/k2600/k2600.mstest

[2017 issue]: https://developercommunity.visualstudio.com/t/could-not-load-file-or-assembly-error-when-using-s/35539
