# Thermal Transient Meter&trade; Manual Test Guide

## Startup
- Turn the instrument on.
- Press the __Run__ button.

<a name="Thermal-Transient-Test"></a>
## Thermal Transient Test
- Press the __Trig__ button.
- The instrument display the initial resistance, thermal transient and final resistance values.

<a name="Toggle-The-Source"></a>
## Toggle the source
- Press the __Menu__ button.
- Select the __Resistance__ menu.
- Select the __Source__ menu.
- Change the source from __Voltage__ to __Current__ or vice versa.
- Select the __Level__ menu.
- Verify that a voltage level is specified for a Voltage source or current level for a Current souce.
- Repeat the [test](#Thermal-Transient-Test).

<a name="Open-Leads-Test"></a>
## Open leads test
- Run an open leads test with contact check enabled.
  - Verify open leads display.
- Run an open leads and open DUT test.
  - Verify open leads display.
- Turn off contact check.
  - Run an open leads test -- should overflow.
  - Run a normal test.
- Restore contact check settings.
  - Restart the instrument and validate contact check settings.

<a name="Compliance-Test"></a>
## Compliance Test
- Change voltage or current settings to force compliance.
- Run a [test](#Thermal-Transient-Test). The test should fail.
- Change the Status settings from 66 to 2.
- The [test](#Thermal-Transient-Test) should no longer fail. 
