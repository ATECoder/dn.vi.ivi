# About

[cc.isr.VI.Tsp.K2600.Ttm.Legacy.MSTest] unit tests the Keithley 2600 source meter TTM Legacy Driver functionality.

# Contents

  - [Preparing for testing](#preparing_for_testing)
  - [Device Tests](#device_tests)
  - [Contact Tests](#contact_tests)

<a name="preparing_for_testing"></a>
# Preparing for testing

1. Turn on the instrument;
1. Validate the instrument settings resource name against the actual instrument IP address;
1. Ping the instrument;
    1. It has been observed, at least on a VM, that the instrument is found by the VISA resource manager only after the instrument is pinged successfully.

<a name="device_tests"></a>
# Device Tests

| Test Set     | Test Name                         | Notes |
|--------------|-----------------------------------|-------|
| Device Tests | 01. Session should open           |       |
|              | 02. Meter should initialize       |       |
|              | 03. Meter should preset           |       |
|              | 04. Measurements Should Configure |       |
|              | 05. Measurement should trigger    |       |
|              | 06. Trigger cycle should abort    |       |

<a name="contact_tests"></a>
# Contact Tess

| Test Set      | Test Name                                | Notes |
|---------------|------------------------------------------|-------|
| Contact Tests | 01. Measurement should fail open contact |       |

# Key Features

Test settings are derived from a single JSon file consisting of multiple settings. The settings are provided by singleton instances of the relevant setting classes.

# Key Issue

# Main Types

The main types provided by this library are:

* _CommandTests_: Tests the TTM commands.
* _ScriptTests_: Tests loading embedded scripts.
 
# Feedback

[cc.isr.VI.Tsp.K2600.Ttm.Legacy.MSTest] is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi
[cc.isr.VI.Tsp.K2600.Ttm.Legacy.MSTest]: https://github.com/atecoder/dn.vi.ivi/src/vi/k2600.ttm/k2600.ttm.legacy.mstest
