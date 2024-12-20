# VI Device Tests

The [cc.isr.VI.Device.MSTest] includes tests for the [cc.isr.VI.Pith] classes, which provide the base classes for this VISA implementation. Also included is a Device Manager class, which supports tests of a large number of device subsystems and is to be called from test projects designed to unit test the subsystem of specific devices such as the Keithley 2450 source meter or the 2002 multimeter.

## [cc.isr.VI.Pith] Tests

The [cc.isr.VI.Pith] tests do not require an active VISA device. These tests are designed to test the implementation at the resource level:

1. Parse Tests: tests parsing of input values;
1. SessionBaseTests: Tests the termination characters;
1. VisaResourceTests: which tests loading the VISA implementations and the enumaration of the resources defined by the specific implementation.

# Feedback

[cc.isr.VI.Device.MSTest] is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi
[cc.isr.VI.Device.MSTest]: https://github.com/atecoder/dn.vi.ivi/src/device/device.mstest/
[cc.isr.VI.Pith]: https://github.com/atecoder/dn.vi.ivi/src/resource/pith/
