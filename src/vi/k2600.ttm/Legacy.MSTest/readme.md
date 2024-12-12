### About

[cc.isr.VI.Tsp.K2600.Ttm.Legacy.MSTest] unit tests the Keithley 2600 source meter TTM Legacy Driver functionality.

### How to Use

#### Preeparing for testing

1. Turn on the instrument;
1. Validate the instrument settings resource name against the actual instrument IP address;
1. Ping the instrument;
    1. If has been observer, at least on a VM, that the instrument is found by the VISA resource manager only after the instrument is pinged once.

#### Running Tests

When doing exploratory testing, for example, after new vendor implementation is installed, test must be best run in the following order:

1. Command Tests;
1. Script Tests;

### Key Features

Test settings are derived from a single JSon file consisting of multiple settings. The settings are provided by singleton instances of the relevant setting classes.

### Key Issue

### Main Types

The main types provided by this library are:

* _CommandTests_: Tests the TTM commands.
* _ScriptTests_: Tests loading embedded scripts.
 
### Feedback

[cc.isr.VI.Tsp.K2600.Ttm.Legacy.MSTest] is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://bitbucket.org/davidhary/dn.vi
[cc.isr.VI.Tsp.K2600.Ttm.Legacy.MSTest]: https://bitbucket.org/davidhary/dn.vi/src/vi/k2600.ttm/k2600.ttm.legacy.mstest
