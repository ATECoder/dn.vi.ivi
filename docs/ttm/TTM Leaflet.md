# Thermal Transient Meter&trade; Leaflet

## System Description:

cc.isr.TTM.2600 is a Thermal Transient Meter (TTM) for non-destructively testing airbag ignitors (squibs). The TTM measures the ignitor resistance before and after measuring the ignitor thermal transient response to a brief current pulse. The test is used to determine the fitness of the ignitor. Testing can be initiated from the instrument front panel, by external triggering, or from  Microsoft Windows applications.

## System Components:

The TTM consists of the following parts:

- A Keithley Source Meter (currently the 2601B).
- The `cc.isr.TTM.Firmware`, which is embedded in the source meter for making the thermal transient measurements.
- The `cc.isr.VI.Tsp.K2600.Ttmware.Loader` application for loading or updating the embedded firmware.
- The `cc.isr.VI.Tsp.K2600.Ttm` Application Programming Interface (API) for communicating with the instrument by way of the `cc.isr.TTM.Firmware`.
- The `cc.isr.VI.Tsp.K2600.Ttm.Console` program for conducting ignitor tests from the desktop.
- The `cc.isr.VI.Tsp.K2600.Ttm` Application Programming Interface (API) for communicating with the instrument by way of the `cc.isr.TTM.Firmware`.
- The `cc.isr.VI.Tsp.K2600.Ttm.Legacy` Application Programming Interface (API) that is compatible with earlier applications of the TTM system.
- The `cc.isr.VI.Tsp.K2600.Ttm.Legacy.Console` program for conducting ignitor tests from the desktop using the legacy API.

## System Features

- Instrument front panel, externally triggered, or Microsoft Windows testing.
- Microsoft Windows based monitoring and data logging.

## Thermal Transient Specifications

- Cold Resistance Current: 1 - 100 mA.
- Transient Source Current: 0.1 - 1 Ampere.
- Transient Pulse Duration: 10 ms and up.
- Maximum Voltage: 40 V
- Equivalent Noise: 0.1 mA RMS
- Contact checking

## Licenses

The system comes with the following licenses:
- [Fair End User License Agreement for embedded software] for all users.
- [The MIT License] for the Windows Based applications.
- The [Creative Commons Attribution 4.0 International Public License]  for the open source Windows Based applications and libraries.

[Fair End User License Agreement for embedded software]: https://docs.google.com/document/d/1873_SHHYkyg_qMJ4Gp6BrXX5hfXCLOAuWs3A3-gYXO0/edit?hl=en&pli=1&tab=t.0
[The MIT License]: https://www.lua.org/license.html
[Creative Commons Attribution 4.0 International Public License]: https://github.com/ATECoder/dn.vi.ivi/blob/main/license
