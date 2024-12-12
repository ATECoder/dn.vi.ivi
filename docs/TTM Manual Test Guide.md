# Thermal Transient Meter&trade; Manual Test Guide

<a name="startup"></a>
## Startup
- Turn the instrument on.
- Press the _Run_ button.

<a name="test"></a>
## Thermal Transient Test
- Press the _Trig_ button.
- The instrument display the initial resistance, thermal transient and final resistance values.

<a name="source"></a>
## Toggle the source
- Press the _Menu_ button.
- Select the _Resistance_ menu.
- Select the _Source_ menu.
- Change the source from _Voltage_ to _Current_ or vice versa.
- Select the _Level_ menu.
- Verify that a voltage level is specified for a Voltage source or current level for a Current souce.
- Repeat the [test].

<a name="leads"></a>
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

<a name="compliance"></a>
## Compliance test
- Change voltage or current settings to force compliance.
- Run a [test]. The test should fail.
- Change the Status settings from 66 to 2.
- The [test] should no longer fail. 
	
