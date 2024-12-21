# Thermal Transient Meter&trade; (TTM) Manual Test Guide

<a name="Startup"></a>
## Startup
- Turn the instrument on.
  - The front panel displays the current version, e.g., ___TTM 2.4.9111___.
- Press the __Run__ button.
  - This opens the [Action Panel](#Action-Panel).

<a name="Action-Panel"></a>
## Action Panel
- The Action Panel displays the ___Therm. Trans. Meter.___ title, copyrights, e.g., ___(c)2011 ISR___ and optional action buttons ___\<TRIG MENU RUN\>___.

<a name="Thermal-Transient-Test"></a>
## Thermal Transient Test
- Press the __Trig__ button.
- The instrument displays the initial resistance, thermal transient and final resistance values and the action buttons ___\<TRIG MENU RUN\>___.

<a name="Toggle-The-Source"></a>
## Toggle the Source Modality
The TTM measures the cold resistance using either constant voltage or constant current. This can be tested as follows:
- Press the __Menu__ button.
- Select the __Resistance__ menu.
- Select the __Source__ menu.
- Toggle the source between __Voltage__ and __Current__.
- Select the __Level__ menu.
- Verify that a voltage level is specified is a voltage source is selected or current level is specified for a current source.
- Repeat the [Thermal Transient Test](#Thermal-Transient-Test).

<a name="Test-Open-Leads"></a>
## Test Open Leads
The TTM checks for open circuit between the source and sense leads of the 4-wire Kelvin connection to the device under test before taking the initial cold resistance measurement. This can be ascertained as follows:
- Run a [Thermal Transient Test](#Thermal-Transient-Test).
- Disconnect the either the low or high sense lead from the instrument.
- Run a [Thermal Transient Test](#Thermal-Transient-Test).
  - The instrument should display the open leads caption: ___Open leads Hi,Lo:___ in the top row, the leads resistances in the bottom row, and the action buttons ___\<TRIG MENU RUN\>___. For example, after opening the high sense lead (`S Hi`), ___9.9e+37,8.6e-02___ was displayed. The first _9.9e+37_ value represents _NaN_ (not a number) indicating that, as expected, the high sense lead was open. The second _8.6e-02_ value represents 86 milli-Ohms indicating, as expected, that the low sense lead was connected to the low source lead.

<a name="Compliance-Test"></a>
## Compliance Test
The TTM instrument is capable of detecting when the source forces the sense amount to hit a maximum value that is set from the menu. For example, taking a resistance measurement by sourcing a voltage of 100 mV and setting the current limit to 10 mA will force the source into compliance at 20 mV if the resistance of the device under test is 2 ohms. Depending on the compliance setting of the instrument, this could force the meter to abort after measuring the initial resistance if the Compliance Bit (6, 64) of the Status condition register is enabled. This can be disabled by setting the [Status](#Status-Option] from the [Resistance](#Resistance-Menu) to 2 (Over-Temp only). This can be tested as follows:
- Set the [Status](#Status-Option) value to 66.
- Set the [Voltage](#Resistance-Voltage-Option) and or [Current](#Resistance-Current-Option) settings to force compliance.
- Run a [test](#Thermal-Transient-Test). The test should abort after the initial resistance.
- Change the Status settings from 66 to 2.
- The [test](#Thermal-Transient-Test) should no longer fail. 

<a name="Main-Menu"></a>
## Main Menu

Pressing the `Menu` button from the Front Panel opens the TTM [Main Menu](#Main-menu). The font panel displays 
__TTM Main Menu__ with the following options:

| Option | Description |
|--------|-------------|
| [Meter](#Meter-Menu) | Selects the Meter Menu |
| [Resistance](#Resistance-Menu) | Selects the resistance Menu |
| [Transient](#Thermal-Transient-Menu) | Selects the thermal Thermal Transient Menu |

with the [Meter](#Meter-Menu) option blinking.

The _Navigation Wheel_ or the _Cursor Keys_ move the between the menu items.

Pressing the _Navigation wheel_ or the _ENTER_ button selects a menu item.

<a name="Meter-Menu"></a>
### Meter Menu

Selecting _Meter_ from the [Main Menu](#Main-Menu) displays the [Meter Menu](#Meter-Menu). The front panel displays __Meter__ with the following options: 

| Option | Description |
|--------|-------------|
| [Channel](#Channel-Menu) | Selects the source channel (only for meters with dual source channels ) |
| [Driver](#Driver-Menu) | Selects compatibility with the legacy TTM drivers.
| [Leads](#Leads-Menu) | Sets the leads resistance for contact check.
| [Checks](#Checks-Menu) | Sets the contact checks options.

<a name="Channel-Menu"></a>
#### Channel Menu

Selecting _Channel_ from the [Meter Menu](#Meter-Menu) displays the [Channel Menu](#Channel-Menu):

- Title: __SMU: smua__
- Options: __smua__ __smub__

indicating that the current Source Meter Unit (SMU) channel is `a`.

Navigating to `smub` and pressing the _Navigation Wheel_ selects the `b` channel.

<a name="Driver-Menu"></a>
#### Driver Menu

Selecting _Driver_ from the [Meter Menu](#Meter-Menu) displays the [Driver Menu](#Driver-Menu):

- Title: __01__
- Description: _Legacy (1) or new (0)_

Moving the cursor over the 1, the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the value between 0 and 1. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Leads-Menu"></a>
#### Leads Menu

Selecting _Leads_ from the [Meter Menu](#Meter-Menu) displays the [Leads Menu](#Leads-Menu):

- Title: __100 Ω__
- Description: _Max Resistance_

This sets the maximum allowed open leads resistance. Any leads resistance above this value will abort the measurement.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Checks-Menu"></a>
#### Checks Menu

Selecting _Checks_ from the [Meter Menu](#Meter-Menu) displays the [Checks Menu](#Checks-Menu):

- Title: __01__
- Description: _Pre IR[1]+TR[2]+FR[3]_

This sets bit values that toggle the contact check for Initial Resistance (IR) (1), Thermal Transient (TR) (2) or Final Resistance (FR) (4). For example, setting the value to 1, enables contact check before the Initial and Final Resistance measurements.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Resistance-Menu"></a>
### Resistance Menu

Selecting _Resistance_ from the [Main Menu](#Main-Menu) displays the [Resistance Menu](#Resistance-Menu). The front panel displays __Cold Resistance__ with the following options: 

| Option | Description |
|--------|-------------|
| [Source](#Source-Menu) | Selects the source modality as current or voltage |
| [Level](#Level-Menu) | Selects source level |
| [Limit](#Limit-Menu) | Sets the sense limit |
| [Aperture](#Aperture-Menu) | Sets the duration that the source is applied to the device under test |
| [Limits](#Limits-Menu) | Selects the [Limits](#Limits-Menu). |
| [Status](#Status-Menu) | Selects the [Status](#Status-Menu) condition menu. |

<a name="Source-Menu"></a>
#### Source Menu

Selecting _Source_ from the [Resistance Menu](#Resistance-Menu) displays the [Source Menu](#Source-Menu). The front panel displays __Source: Voltage__ or __Source: Current__ with the following options: 

| Option | Description |
|--------|-------------|
| Voltage | Selects the source modality as voltage |
| Current | Selects the source modality as current |

Moving the cursor over an option and pressing the _Wheel_ or _ENTER_ selects this option. Pressing _EXIT_ returns to the [Resistance Menu](#Resistance-Menu).

<a name="Level-Menu"></a>
#### Level Menu

Selecting _Level_ from the [Resistance Menu](#Resistance-Menu) displays the [Level Menu](#Level-Menu). This menu sets the level of the source modality. The display depends on the [Source Menu](#Source-Menu) option. For _Voltage_ the display is:

- Title: __0010 mV__
- Description: _Source Voltage_

For _Current_ the display is:

- Title: __0010 mA__
- Description: _Source Current_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Limit-Menu"></a>
#### Limit Menu

Selecting _Limit_ from the [Resistance Menu](#Resistance-Menu) displays the [Limit Menu](#Limit-Menu). This menu sets the Limit of the sense modality. The display depends on the [Source Menu](#Source-Menu) option. For _Voltage_ the display is:

- Title: __050 mA__
- Description: _Current Limit_

For _Current_ the display is:

- Title: __010 mV__
- Description: _Voltage Limit_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Aperture-Menu"></a>
#### Aperture Menu

Selecting _Aperture_ from the [Resistance Menu](#Resistance-Menu) displays the [Aperture Menu](#Aperture-Menu). This menu sets the Aperture, or duration, of the applied source level. The front panel displays:

- Title: __01.000 PLC__
- Description: _Integration Period_

This menu sets the duration of the measurement in power line cycles (PLC).

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Limits-Menu"></a>
#### Limits Menu

Selecting _Limits_ from the [Resistance Menu](#Resistance-Menu) displays the [Limits Menu](#Limits-Menu). The front panel displays __Limits__ with the following options: 

| Option | Description |
|--------|-------------|
| Low | Selects the [Low Limit Menu](#Low-Limit-Menu) |
| High | Selects the [High Limit Menu](#High-Limit-Menu) |

Moving the cursor over an option and pressing the _Wheel_ or _ENTER_ selects this option. Pressing _EXIT_ returns to the [Resistance Menu](#Resistance-Menu).

<a name="Low-Limit-Menu"></a>
##### Low Limit  Menu

Selecting _Low_ from the [Limits Menu](#Limits-Menu) displays the [Low Limit Menu](#Low-Limit-Menu). This menu sets the low limit of the measurement in Ohms. The front panel displays:

- Title: __01.920 Ω__
- Description: _Low Limit_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="High-Limit-Menu"></a>
##### High Limit  Menu

Selecting _High_ from the [Limits Menu](#Limits-Menu) displays the [High Limit Menu](#High-Limit-Menu). This menu sets the High limit of the measurement in Ohms. The front panel displays:

- Title: __02.160 Ω__
- Description: _High Limit_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Status-Menu"></a>
#### Status Menu

Selecting _Status_ from the [Resistance Menu](#Resistance-Menu) displays the [Status Menu](#Status-Menu):

- Title: __002__
- Description: _66=compliance 64 or over-temp 2_

This sets bit values that toggle the over-temperature failure condition (2) or compliance failure condition (64). For example, setting the value to 2 the instrument will fail under an over-temperature condition but will not fail if the sense current or voltage reaches the limit set with the [Limit Menu](#Limit-Menu).

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Thermal-Transient-Menu"></a>
### Thermal Transient Menu

Selecting _Transient_ from the [Main Menu](#Main-Menu) displays the [Thermal Transient Menu](#Thermal-Transient-Menu):

| Option | Description |
|--------|-------------|
| [Current](#Current-Menu) | Selects the current menu |
| [Voltage](#Voltage-Menu) | Selects the voltage menu |
| [Aperture](#Transient-Aperture-Menu) | Sets the sense duration |
| [Points](#Points-Menu) | Sets the number of points to acquire |
| [Period](#Period-Menu) | Selects the [Period](#Period-Menu) |
| [Delay](#Delay-Menu) | Selects the [Delay](#Delay-Menu) |
| [Limits](#Transient-Limits-Menu) | Selects the [Limits](#Transient-Limits-Menu) |

<a name="Current-Menu"></a>
#### Current Menu

Selecting _Current_ from the [Transient Menu](#Transient-Menu) displays the [Current Menu](#Current-Menu). This menu sets the Current of the source modality:

- Title: __297 mA__
- Description: _Source Current_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Voltage-Menu"></a>
#### Voltage Menu

Selecting _Voltage_ from the [Transient Menu](#Transient-Menu) displays the [Voltage Menu](#Voltage-Menu). This menu sets the Voltage limit of the sense modality:

- Title: __0.990 V__
- Description: _Voltage Limit_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Transient-Aperture-Menu"></a>
#### Transient Aperture Menu

Selecting _Aperture_ from the [Transient Menu](#Transient-Menu) displays the [Transient Aperture Menu](#Transient-Aperture-Menu). This menu sets the Aperture, or duration, of each measurement of the current pulse point:

- Title: __0.004 PLC__
- Description: _Integration Period_

This menu sets the duration of the measurement in power line cycles (PLC).

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Points-Menu"></a>
#### Points Menu

Selecting _Points_ from the [Transient Menu](#Transient-Menu) displays the [Points Menu](#Points-Menu). This menu sets the number of points of the current pulse applied during the thermal transient measurement:

- Title: __00100__
- Description: _Data Points_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Period-Menu"></a>
#### Period Menu

Selecting _Period_ from the [Transient Menu](#Transient-Menu) displays the [Period Menu](#Period-Menu). This menu sets the duration of each point of the current pulse applied during the thermal transient measurement:

- Title: __00100 µS__
- Description: _Sampling Interval_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Delay-Menu"></a>
#### Delay Menu

Selecting _Delay_ from the [Transient Menu](#Transient-Menu) displays the [Delay Menu](#Delay-Menu). This menu sets the delay time between the end of the thermal transient measurement and the beginning of the final resistance measurement:

- Title: __00011 mS__
- Description: _Final Cold Resistance Delay_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="Transient-Limits-Menu"></a>
#### Transient Limits Menu

Selecting _Limits_ from the [Transient Menu](#Transient-Menu) displays the [Transient Limits Menu](#Transient-Limits-Menu). The front panel displays __Limits__ with the following options: 

| Option | Description |
|--------|-------------|
| Low | Selects the [Low transient Voltage Limit Menu](#Low-Transient-Voltage-Limit-Menu) |
| High | Selects the [High transient Voltage Limit Menu](#High-Transient-Voltage-Limit-Menu) |

Moving the cursor over an option and pressing the _Wheel_ or _ENTER_ selects this option. Pressing _EXIT_ returns to the [Transient Menu](#Transient-Menu).

<a name="Low-Transient-Voltage-Limit-Menu"></a>
##### Low Transient Voltage Limit  Menu

Selecting _Low_ from the [Limits Menu](#Limits-Menu) displays the [Low Transient Voltage Limit Menu](#Low-Transient-Voltage-Limit-Menu). This menu sets the low Transient Voltage Limit of the measurement in Voltage. The front panel displays:

- Title: __005.4 mV__
- Description: _Low Limit_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.

<a name="High-Transient-Voltage-Limit-Menu"></a>
##### High Transient Voltage Limit  Menu

Selecting _High_ from the [Transient Limits Menu](#Transient-Limits-Menu) displays the [High Transient Voltage Limit Menu](#High-Transient-Voltage-Limit-Menu). This menu sets the High Transient Voltage Limit of the measurement in volts. The front panel displays:

- Title: __076.0 mV__
- Description: _High Limit_

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or _ENTER_ and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or _ENTER_ selects the new value.






