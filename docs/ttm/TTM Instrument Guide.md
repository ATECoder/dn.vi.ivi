# Thermal Transient Meter&trade; Instrument User's Guide

## Table of Contents

- [This Guide](#Guide)
- [Preparations](#Preparations)
  - [Wiring the Device Under Test](#Wiring)
  - [Taking the instrument from Remote to Local Mode](#Remote-Local)
  - [Starting TTM from the Front Panel](#Starting)
  - [Exiting from TTM Front Panel Mode](#Exiting)
  - [Setup of Measurement Parameters](#Main-Menu)
	- [Meter Menu](#Meter-Menu)
	  - [Channel Menu](#Channel-Menu)
	  - [Driver Menu](#Driver-Menu)
	  - [Leads Menu](#Leads-Menu)
	  - [Contact Checks Menu](#Contacts-Check-Menu)
	- [Resistance Menu](#Resistance-Menu)
	  - [Source Menu](#Source-Menu)
	  - [Level Menu](#Level-Menu)
	  - [Limit Menu](#Limit-Menu)
	    -[Voltage Source Current Limit](#Voltage-Source-Current-Limit)
	    -[Current Source Voltage Limit](#Current-Source-Voltage-Limit)
	  - [Aperture Menu](#Aperture-Menu)
	  - [Limits Menu](#Limits-Menu)
		- [Low Limit Menu](#Low_Limit-Menu)
		- [High Limit Menu](#High_Limit-Menu)
	  - [Status Menu](#Status-Menu)
	- [Thermal Transient Menu](#Thermal-Transient-Menu)
	  - [Source Current Menu](#Source_Current-Menu)
	  - [Maximum Voltage Menu](#Maximum_Voltage-Menu)
	  - [Thermal Transient Aperture Menu](#Transient_Aperture-Menu)
	  - [Thermal Transient Points Menu](#Transient_Points-Menu)
	  - [Thermal Transient Period Menu](#Transient_Period-Menu)
	  - [Post Thermal Transient Delay Menu](#Post_Transient_Delay-Menu)
	  - [Thermal Transient Limits Menu](#Transient_Limits-Menu)
		- [Thermal Transient Low Limit Menu](#Transient_Low_Limit-Menu)
		- [Thermal Transient High Limit Menu](#Transient_High_LImit-Menu)
- [Measuring Using the Front Panel](#Measuring_Using_the_Front_Panel)
- [Measuring Using the \*TRG command](#Measuring_Using_the_TRG_command)
- [Measuring Using Digital Input](#Measuring_Using_Digital_Input)
  - [Digital I/O](#Digital_IO)
  - [Triggering](#Triggering)
  - [Reading the Outcome](#Reading)
  - [Measurement Readings](#RMeasurement_Readings)
- [Specifications](#Specifications)
  - [Thermal Transient Response](#Thermal_Transient_Response)
    - [Initial And Final Resistance](#Initial_And_Final_Resistance)
    - [Cold Resistance Default Settings](#Cold_Resistance_Default_Settings)
      - [Cold Resistance Voltage Source](#Cold_Resistance_Voltage_Source)
      - [Cold Resistance Current Source](#Cold_Resistance_Current_Source)
  - [Thermal Transient Default Settings](#Thermal_Transient_Default_Settings)
- [Attributions](#Attributions)

<a name="Guide"></a>
## Guide

This guide describes how to use the TTM instrument as a stand-along bench or production line device.

The TTM Instrument is part of the ISR TTM Framework as described in the [TTM Framework Guide].

<a name="Preparations"></a>
## Preparations

<a name="Wiring"></a>
### Wiring the Device Under Test

The device under test must be connected to the TTM Instrument using a [Four-Terminal Sensing] circuit:

![](KelvinDiagram.jpg)

<a name="Remote-Local"></a>
### Taking the instrument from Remote to Local Mode

When controlling the instrument from an application program the instrument goes into Remote mode.

Pressing the `EXIT` button on the instrument panel changes the instrument access mode to local where the instrument is controlled from the front panel rather than from an external program.

<a name="Starting"></a>
## Starting TTM from the Front Panel

<a name="Starting-from-Off-State"></a>
### Starting from the Off State

- Turn the instrument on.
  - The instrument runs self check and loads the TTM Firmware.
  - The instrument goes into its [Idle State](#Starting-from-the-Idle-State).

<a name="Starting-from-the-Idle-State"></a>
### Starting from the Idle State
  
When Idle, the instrument display the TTM starting screen:

'''
TTM 2.4.9111
Integrated Scientific Resources
'''

Where 2.4.9111 is the current version of the TTM firmware. 

- Press the _`RUN`_ button.
  - This opens the [Action Panel](#Action-Panel).


<a name="Action-Panel"></a>
### Action Panel

When at the Action state, the instrument displays the framework title, copyrights and menu options:
```
Therm. Trans. Meter.
(c)2011 ISR <TRIG MENU RUN>
```

<a name="Starting-from-the-Source-Measure-State"></a>
### Starting from the Source/Measure State
  
When in its Source/Measure State, the instrument displays the default measurement information, such as,
```
----.00mV
SrcA:+000.0000mV LimA:1.00000 A
```

- Press the _`RUN`_ button.
  - This opens the [Action Panel](#Action-Panel).

<a name="Loading"></a>
### Loading the Firmware

Follow these steps to load and run the firmware:

- From the instrument front panel press the _LOAD_ button.
- Select _USER_ from the menu and press `ENTER`.
- Select _TTM_ and press `ENTER`.
- Press _LOAD_.
- Press `RUN`.
- This opens the [Action Panel](#Action-Panel).

<a name="Exiting"></a>
### Exiting from TTM Front Panel Mode

Click the `RUN` button to exit from the run loop.

<a name="Main-Menu"></a>
## Setup of Measurement Parameters

Press `MENU` from the instrument panel to configure the instrument. This opens the TTM [Main Menu](#Main-menu). The font panel displays 
`TTM Main Menu` with the [Meter](#Meter-Menu) option blinking:

| Option | Description |
|--------|-------------|
| [Meter](#Meter-Menu) | Selects the [Meter](#Meter-Menu) Menu |
| [Resistance](#Resistance-Menu) | Selects the [Resistance](#Resistance-Menu) Menu |
| [Transient](#Thermal-Transient-Menu) | Selects the Thermal [Transient](#Thermal-Transient-Menu) Menu |

### Menu Actions
- Use the _Navigation Wheel_ or the _Cursor Keys_ to move the between the menu items.
- Press the _Navigation wheel_ or the `ENTER` button to select a menu item.
- Press `EXIT` to cancel a selection.

### Parameter Ranges
See [Specifications](#Specifications) for parameter ranges.

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

The channel menu selects either `smua` or `smub` as the active source meter for instruments with dual source meter units. This setting is enabled (turned on) only for instruments with a two source measure units.

Selecting _Channel_ from the [Meter Menu](#Meter-Menu) displays the [Channel Menu](#Channel-Menu) showing the current value, `smua` or `smub` (`smua` in this case):

- Title: __SMU: smua__.
- Options: __smua__ __smub__.

Navigating to `smub` and pressing the _Navigation Wheel_ or `ENTER` selects the `smub` channel.

<a name="Driver-Menu"></a>
#### Driver Menu

Selecting _Driver_ from the [Meter Menu](#Meter-Menu) displays the [Driver Menu](#Driver-Menu):

- Title: __01__.
- Description: _Legacy (1) or new (0)_.

Moving the cursor over the 1, the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the value between 0 and 1. Pressing the _Wheel_ or `ENTER` selects the new value. Pressing `EXIT` cancels the selection.

Set the driver option to `1` if using the meter with the legacy Visual Basic Driver that supported versions 2.3.4009 or earlier of the TTM Meter as used by the ISR TTM Driver version 2.3.4063. Set the value to `0` to use the Meter with software that supports the new features of the firmware.

<a name="Leads-Menu"></a>
#### Leads Menu

The [Leads Menu](#Leads-Menu) sets the maximum allowed open leads resistance. The measurement will be aborted after a contact check if leads resistance exceeds above this value.

The contact check function prevents measurements that may be in error due to excessive resistance in the force or sense leads when making remotely sensed (Kelvin) measurements. Potential sources for this resistance include poor contact at the device under test (DUT), failing relay contacts on a switching card or wires that are too long or thin.

The contact check function also detects an open circuit that may occur when a four-point probe is misplaced or misaligned. This relationship is shown schematically in the figure below, where R~C~ is the resistance of the mechanical contact at the DUT, and R~S~ is the series resistance of cables and any series relays.

The leads contacts are checked if the maximum leads resistance (see below) is set to a non-zero positive value. Contacts are measured before taking the initial resistance measurement. The measurement is aborted and the lead resistances are displayed if the measured lead resistances exceeds the maximum limit. 

![](ContactCheckDiagram.png)

To enable and set the maximum allowed leads resistance:

Selecting _Leads_ from the [Meter Menu](#Meter-Menu) displays the [Leads Menu](#Leads-Menu) displaying the maximum resistance beyond which the leads are taken as open. The resistance is in Ohms between 10 and 999:

- Title: __100 Ω__.
- Description: _Max Resistance_.

The range is: 10 - 999.

Moving the cursor over the digits, the digit blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

<a name="Contacts-Check-Menu"></a>
#### Contacts Check Menu

Selecting _Checks_ from the [Meter Menu](#Meter-Menu) displays the [Checks Menu](#Checks-Menu). This menu selects when contact checks are to be performed. This is done by setting a bit value that toggles the contact check for Initial Resistance (IR) (binary 001), Thermal Transient (TR) (binary 010) or Final Resistance (FR) (binary 100). For example, setting the value to 5 (binary 101), enables contact checks before the Initial and Final Resistance measurements whereas setting the value to 7 (binary 111 or 1+2+4), enables contact checks before the initial, final, and thermal transient measurements.

|Bit|Name|Hex Value|Decimal Value|Description|
|----|----|----|----|
|B0|Pre-Initial-Resistance|0x01|1|Check contacts before making the initial cold resistance measurement. This value is always set.|
|B1|Pre-Thermal-Transient|0x02|2|Check contacts before making the thermal transient measurement|
|B2|Pre-Final-Resistance|0x04|4|Check contacts before making the final cold resistance measurement|

- Title: __01__.
- Description: _Pre IR[1]+TR[2]+FR[3]_.

The expected range are: 1, 3, 5, 7.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

<a name="Resistance-Menu"></a>
### Resistance Menu

Selecting _Resistance_ from the [Main Menu](#Main-Menu) displays the [Resistance Menu](#Resistance-Menu), which is used to set the parameters for the initial and final cold resistance measurements. The front panel displays __Cold Resistance__ with the following options: 

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

Either a Voltage or a Current source can be selected to force voltage or current, respectively for measuring the cold resistance.

Moving the cursor over an option and pressing the _Wheel_ or `ENTER` selects this option. Pressing `EXIT` returns to the [Resistance Menu](#Resistance-Menu).

<a name="Level-Menu"></a>
#### Level Menu

Selecting _Level_ from the [Resistance Menu](#Resistance-Menu) displays the [Level Menu](#Level-Menu). This menu sets the level of the source modality. The selected level is forced by the TTM instrument onto (Voltage) or through (Current) the bridge-wire when measuring the bridge-wire resistance. This forcing level must be small so as not to trip or excessively heat the bridge-wire.

The display depends on the [Source Menu](#Source-Menu) option. For _Voltage_ the display is:

- Title: __0010 mV__.
- Description: _Source Voltage_.

The range is: 1 - 999 mV.

For _Current_ the display is:

- Title: __0010 mA__.
- Description: _Source Current_.

The range is: 1 - 50 mA

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

<a name="Limit-Menu"></a>
#### Limit Menu

Selecting _Limit_ from the [Resistance Menu](#Resistance-Menu) displays the [Limit Menu](#Limit-Menu). This menu sets the Limit of the sense modality. The display depends on the [Source Menu](#Source-Menu) option.

<a name="Voltage-Source-Current-Limit"></a>
##### Voltage Source Current Limit

For _Voltage_ the display is:

- Title: __050 mA__.
- Description: _Current Limit_.

The range is: 1 - 50 mA

The [Voltage Source Current Limit](#Voltage-Source-Current-Limit) is the highest current that the instrument would allow to flow through the bridge-wire when forcing voltage across the bridge-wire. This limit will be reached if the bridge-wire exhibits a low resistance at which point the measurement is said to hit [Compliance](#Compliance).

<a name="Current-Source-Voltage-Limit"></a>
##### Current Source Voltage Limit

For _Current_ the display is:

- Title: __010 mV__.
- Description: _Voltage Limit_.

The range is: 1 - 999 mV.

The [Current Source Voltage Limit](#Current-Source-Voltage-Limit) is the highest voltage that the instrument would allow over the bridge-wire when forcing current through the bridge-wire. This limit will be reached if the bridge-wire is open or exhibits a very high resistance at which point the measurement is said to hit [Compliance](#Compliance).

<a name="Compliance"></a>
##### Compliance

The measurement is said to hit [Compliance](#Compliance) if the measurement forcing modality, e.g., voltage or current, caused the measured modality, e.g, current or voltage respectively, to reach the limit set by the [Limit Menu](#Limit-Menu). At compliance, the forced modality reaches a level that is lower than the level set in the [Level Menu](#Level-Menu) thus protecting the bridge-wire from over voltage or current.

<a name="Aperture-Menu"></a>
#### Aperture Menu

Selecting _Aperture_ from the [Resistance Menu](#Resistance-Menu) displays the [Aperture Menu](#Aperture-Menu). This menu sets the Aperture, or duration, of the applied source level. The front panel displays:

- Title: __01.000 PLC__.
- Description: _Integration Period_.

The range is: 0.001 - 20.

This menu sets the duration of the measurement in power line cycles (PLC).

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

The aperture (integration period) defines the duration and, thereby the accuracy, of the cold resistance measurement. This value is defined in multiples or fractions of power line cycles (PLC). one (1) PLC equals to 1/60th of a second in the US. At 1 PLC the measurement accuracy exceeds 0.01%.

<a name="Limits-Menu"></a>
#### Limits Menu

Selecting _Limits_ from the [Resistance Menu](#Resistance-Menu) displays the [Limits Menu](#Limits-Menu). The front panel displays __Limits__ with the following options: 

| Option | Description |
|--------|-------------|
| Low | Selects the [Low Limit Menu](#Low-Limit-Menu) |
| High | Selects the [High Limit Menu](#High-Limit-Menu) |

Moving the cursor over an option and pressing the _Wheel_ or `ENTER` selects this option. Pressing `EXIT` returns to the [Resistance Menu](#Resistance-Menu).

Limits are used to determine if the measured value is within range (pass) or out of range (fail). 

The initial and final cold resistances are checked against the limits and the measurement outcome is set to __High__ or __Low__ if the values are out of range. 

If the initial resistance measurement fails, the measured value is displayed along with the outcome status (see [Status](#Status) below).

<a name="Low-Limit-Menu"></a>
##### Low Limit  Menu

Selecting _Low_ from the [Limits Menu](#Limits-Menu) displays the [Low Limit Menu](#Low-Limit-Menu). This menu sets the low limit of the measurement in Ohms. The front panel displays:

- Title: __01.920 Ω__.
- Description: _Low Limit_.

The range is: 0.1 - 10 Ω.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

<a name="High-Limit-Menu"></a>
##### High Limit  Menu

Selecting _High_ from the [Limits Menu](#Limits-Menu) displays the [High Limit Menu](#High-Limit-Menu). This menu sets the High limit of the measurement in Ohms. The front panel displays:

- Title: __02.160 Ω__.
- Description: _High Limit_.

The range is: 0.1 - 10 Ω.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

<a name="Status-Menu"></a>
#### Status Menu

Selecting _Status_ from the [Resistance Menu](#Resistance-Menu) displays the [Status Menu](#Status-Menu):

- Title: __002__.
- Description: _66=compliance 64 or over-temp 2_.

The expected range are: 2, 64, 66.

This sets bit values that toggle the over-temperature failure condition (2) or compliance failure condition (64). For example, setting the value to 2 the instrument will fail under an over-temperature condition but will not fail if the sense current or voltage reaches the limit set with the [Limit Menu](#Limit-Menu).

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

<a name="Buffer-Status"></a>
##### Buffer Status

In addition to measuring the cold resistance, the instrument reports a _Buffer Status_ byte where each bit represents a specific setting or outcome as follows:

|Bit|Name|Hex Value|Decimal Value|Description|
|----|----|----|----|
|B0|Reserved|0x01|1|Reserved for future use|
|B1|Over-temp|0x02|2|Over temperature condition|
|B2|AutoRangeMeas|0x04|4|Measure range was auto ranged|
|B3|AutoRangeSrc|0x08|8|Source range was auto ranged|
|B4|4Wire|0x10|16|4-wire (remote) sense mode was enabled|
|B5|Rel|0x20|32|Relative offset was applied to a reading|
|B6|Compliance|0x40|64|Source function was in compliance|
|B7|Filtered|0x80|128|Reading was filtered|

- Select _Status_ from the _Cold Resistance_ menu;
- The _Fail Status Compliance 64 Over-temp 2_ prompt will display showing the status bitmask that is to indicate a failed cold resistance measurement;
- Press the thumb wheel to select and then increase or decrease the value;
- Press `ENTER` to accept or `EXIT` to cancel the changes.

The [Status Menu](#Status-Menu) settings of the Thermal Transient Meter allows the operator to select what status values are interpreted as a failed measurement.

The legacy revisions of the TTM firmware prior to and including 2.3.4009 has taken either or both the over-temp and compliance status bits to indicate a failure. With the new firmware, the operator can choose to accept a measurement that hits a [Compliance](#Compliance) condition. For example, the operator may choose to apply a voltage of 100 mV but limit the current at 20 mA. When making this measurement on a 2 Ohm bridge-wire, the instrument will _hit compliance_. The measurement accuracy will still be acceptable. In the case, the operator will set the Status settings to 2 instead of the default of 66.

The measurement outcome and status are displayed along with the bridge-wire resistance and low or high indication if the initial cold resistance measurement failed. Typically a status value of 28 will display indicating that a 4-wire measurement was taken under auto range conditions.

##### Legacy support
In order to support the legacy Visual Basic software the status settings defaults to over-temp only. This way, the instrument can be set to source voltage using the [Source-Menu](#Source-Menu) while still using the legacy current source settings which would cause the instrument to hit [Compliance](#Compliance).

<a name="Thermal-Transient-Menu"></a>
### Thermal Transient Menu

Selecting _Transient_ from the [Main Menu](#Main-Menu) displays the [Thermal Transient Menu](#Thermal-Transient-Menu):

| Option | Description |
|--------|-------------|
| [Current](#Source_Current-Menu) | Selects the [Source Current Menu](#Source_Current-Menu). |
| [Voltage](#Maximum_Voltage-Menu) | Selects the [Maximum Voltage Menu](#Maximum_Voltage-Menu). |
| [Aperture](#Transient_Aperture-Menu) | Sets the duration of each measurement point of the thermal transient pulse. |
| [Points](#Transient_Points-Menu) | Sets the number of points to acquire during the thermal transient pulse. |
| [Period](#Transient_Period-Menu) | Sets the repetition period of the thermal transient pulse points. |
| [Delay](#Port_Transient_Delay-Menu) | Sets the time between the thermal transient and the final cold resistance measurement. |
| [Limits](#Transient_Limits-Menu) | Sets the limits of the maximum thermal transient response voltage. |

Use [Thermal Transient Menu](#Thermal-Transient-Menu) items to set the test parameters of the thermal transient forcing and measurement parameters.

<a name="Source_Current-Menu"></a>
#### Source Current Menu

Selecting _Current_ from the [Thermal Transient Menu](#Thermal-Transient-Menu) displays the [Source Current Menu](#Source_Current-Menu). This menu sets the current level of the thermal transient pulse:

- Title: __297 mA__.
- Description: _Source Current_.

The range is: 10 - 999 mA.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

The source current is the current that is forced by the TTM instrument through the bridge-wire to measure the bridge-wire thermal transient response. This current is pulsed through the bridge-wire for a very short duration determined by the [Period Menu](#Transient_Period-Menu) value times the [Thermal Transient Points Menu](#Transient_Points-Menu) value.

<a name="Maximum_Voltage-Menu"></a>
#### Maximum Voltage Menu

Selecting _Voltage_ from the [Thermal Transient Menu](#Thermal-Transient-Menu) displays the [Maximum Voltage Menu](#Maximum_Voltage-Menu). This menu sets the Voltage limit of the sense modality:

- Title: __0.990 V__.
- Description: _Voltage Limit_.

The range is: 10 - 999 mV.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

The maximum voltage is the limit set by the instrument for the maximum voltage that could develop on the bridge-wire during the thermal transient test. This limit will be reached if the bridge-wire is open or exhibits a very high resistance.

<a name="Transient_Aperture-Menu"></a>
#### Thermal Transient Aperture Menu

Selecting _Aperture_ from the [Thermal Transient Menu](#Thermal-Transient-Menu) displays the [Thermal Transient Aperture Menu](#Transient_Aperture-Menu). This menu sets the Aperture, or duration, of each measurement of the current pulse point:

- Title: __0.004 PLC__.
- Description: _Integration Period_.

The range is: 0.001 - 0.01.

This menu sets the duration of the measurement in power line cycles (PLC).

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

The aperture (integration period) defines the duration and accuracy of the thermal transient measurement. This value is defined in multiples or fractions of power line cycles (PLC). One (1) PLC equals to 1/60th of a second in the US. Thermal transients are measured during a very short pulse period. Therefore, a very small integration period is allowed for the thermal transient.

<a name="Transient_Points-Menu"></a>
#### Thermal Transient Points Menu

Selecting _Points_ from the [Thermal Transient Menu](#Thermal-Transient-Menu) displays the [Thermal Transient Points Menu](#Transient_Points-Menu). This menu sets the number of points of the current pulse applied during the thermal transient measurement:

- Title: __00100__.
- Description: _Data Points_.

The range is: 10 - 10000.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

During the thermal transient test a current pulse is applied to the bridge-wire. The pulse duration is determined by the [Period](#Transient_Period-Menu) times the [Points](#Transient_Points-Menu) values.

<a name="Transient_Period-Menu"></a>
#### Thermal Transient Period Menu

Selecting _Period_ from the [Thermal Transient Menu](#Thermal-Transient-Menu) displays the [Period Menu](#Transient_Period-Menu). This menu sets the duration of each point of the current pulse applied during the thermal transient measurement:

- Title: __00100 µS__.
- Description: _Sampling Interval_.

The range is: 80 - 1000 µS.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

The total duration of the thermal transient pulse equals the [Period](#Transient_Period-Menu) times the [Points](#Transient_Points-Menu) values.

<a name="Post_Transient_Delay-Menu"></a>
#### Post Thermal Transient Delay Menu

Selecting _Delay_ from the [Thermal Transient Menu](#Thermal-Transient-Menu) displays the [Delay Menu](#Post_Transient_Delay-Menu). This menu sets the delay time between the end of the thermal transient measurement and the beginning of the final resistance measurement:

- Title: __00011 mS__.
- Description: _Final Cold Resistance Delay_.

The range is: 1 - 10000 mS.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

The delay sets the time the instrument will pause between the thermal transient measurement and the final resistance measurement. This time is designed to allow the bridge-wire to cool down before making the final resistance measurement.

<a name="Transient_Limits-Menu"></a>
#### Thermal Transient Limits Menu

Selecting _Limits_ from the [Thermal Transient Menu](#Thermal-Transient-Menu) displays the [Thermal Transient Limits Menu](#Transient_Limits-Menu). The front panel displays __Limits__ with the following options: 

| Option | Description |
|--------|-------------|
| Low | Selects the [Thermal Transient Low Limit Menu](#Transient_Low_Limit-Menu) |
| High | Selects the [Thermal Transient High Limit Menu](#Transient_High_Limit-Menu) |

Moving the cursor over an option and pressing the _Wheel_ or `ENTER` selects this option. Pressing `EXIT` returns to the [Thermal Transient Menu](#Thermal-Transient-Menu).

The limits are used to determine if the thermal transient measurement is within range (pass) or out of range (fail).

<a name="Transient_Low_Limit-Menu"></a>
##### Thermal Transient Low Limit Menu

Selecting _Low_ from the [Limits Menu](#Transient_Limits-Menu) displays the [Thermal Transient Low Limit Menu](#Transient_Low_Limit-Menu). This menu sets the low limit for the measured thermal transient voltage. The front panel displays:

- Title: __005.4 mV__.
- Description: _Low Limit_.

The range is: 1 - 999 mV.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

<a name="Transient_High_Limit-Menu"></a>
##### Thermal Transient High Limit Menu

Selecting _High_ from the [Thermal Transient Limits Menu](#Transient_Limits-Menu) displays the [Thermal Transient High Limit Menu](#Transient_High_Limit-Menu). This menu sets the high limit for the measured thermal transient voltage. The front panel displays:

- Title: __076.0 mV__
- Description: _High Limit_

The range is: 1 - 999 mV.

Moving the cursor over the digits, the digit blinks and changes with the rotation of the _Navigation Wheel_ the number blinks. Pressing the _Wheel_ or `ENTER` and turning the _Wheel_ changes the digit between 0 and 9. Pressing the _Wheel_ or `ENTER` selects the new value.

<a name="Measuring_Using_the_Front_Panel"></a>
## Measuring Using the Front Panel

The instrument takes a new measurement when triggered.

Triggering form the front panel of the instrument is done as follows:

- [Start](#Starting) the instrument.
- Wait for the [Action Panel](#Action-Panel).
- Press the `TRIG` button to obtain new measurements.
- [Read](#Reading_the_Outcome) the outcome from the digital I/O.

A failed measurement is designated on the instrument panel by a blinking prefix character. For example, if Final Resistance failed, the R preceding the measured value will blink with changing intensity. Upon a failure of the initial resistance measurement the instrument displays the resistance, the high or low indication, the outcome value and status (see [status] above}.

To exit from the [Action Panel](#Action-Panel) mode, press the `RUN` key.

<a name="Measuring_Using_the_TRG_Command"></a>
## Measuring Using the `*TRG` command

The instrument takes a new measurement when triggered.

The instrument `*TRG` command can be used to trigger a measurement as follows:

- Start the instrument;
- Connect the instrument using an external interface such as GPIB or Ethernet;
- Send the `*TRG` command.
- [Read](#Reading_the_Outcome) the outcome from the digital I/O.

A failed measurement is designated on the instrument panel by a blinking prefix character. For example, if Initial Resistance failed, the R preceding the measured value will blink with changing intensity.

<a name="Measuring_Using_Digital_Input"></a>
## Measuring Using a Digital Input

Measurements can be triggered and outcomes read using the instrument [Digital I/O] port.

<a name="Digita_IO"></a>
## Digital I/O

Digital I/O Interface:

- Connector: 25-pin female D
- I/O Pins: 14 open drain I/O Bits
- Absolute Maximum Voltage: 5.25V
- Absolute Minimum Voltage: -0.25V
- Maximum Logic Low Input Voltage: 0.7V @ +850µA
- Minimum Logic High Input Voltage: 2.1V @ +570µA
- Maximum Source Current (flowing out of digital I/O bit): +960µA  
- Absolute Maximum Sink Current (flowing into digital I/O bit): -11.0mA  
- Maximum Sink Current: -5.0mA @ 0.7V

![](TriggerDiagram.png)

<a name="Triggering"></a>
## Triggering

The instrument provides full hand shake for controlling the measurement by way of the [Digital I/O](#F186A3EE9ED4B6DB25D2650896E4DA3333AE075B) port:

- Trigger a measurement by taking line 1 of the digital I/O port from low to high.
- The instrument responds by taking line 2 high. Line 3 goes low as well as the pass/fail lines 4 through 7.
- Line 3 goes high when the measurement completes at which time the measurement outcomes are available on lines 4 through 7.

<a name="Reading_the_Outcome"></a>
## Reading the Outcome

Measurement outcomes are signaled by lines 4 through 6 of the digital I/O. Values are valid once line 3 goes high:

- Line 4: Initial resistance pass (high) or fail (low);
- Line 5: Final resistance pass (high) or fail (low);
- Line 6: Thermal Transient in range (high) or out of range (low);
- Line 7: All above tests passed (high) or one of them failed (low);

<a name="Outcome_Values"></a>
### Outcome values

Upon a failure of the initial resistance measurement the instrument display the resistance, the high or low indication, the outcome value and status (see [status] above"></a>. The outcome bit values are:

|Bit|Name|Hex Value|Decimal Value|Description|
|----|----|----|----|
|B0|badStatus|0x01|1|Sampling returned bad status from the buffer| 
|B1|badTimeStamps|0x02|2|Sampling returned bad time stamps|
|B2|configFailed|0x04|4|Configuration failed|
|B3|initiationFailed|0x08|8|Pulse initiation failed|
|B4|loadFailed|0x10|16|Scripts not loaded completely|
|B5|notMeasured|0x20|32|Measurement not completed|
|B6|measurementFailed|0x40|64|Measurement failed (e.g., infinite reading due to open sense lines)|
|B7|openLeads|0x80|128|open leads|

#### Clear
Upon clear, which is issued before each measurement, the outcome is set to 0 (Okay) 

#### Legacy Support
The internal outcome value of the instrument is set to the `Lua` `nil` value upon clear. 
The outcome status bit is set if the contact check failed. 

<a name="Measurement_Readings"></a>
### Measurement Readings

Measurement readings are retrieved from the instrument after the measurements are completed.

The API driver sets triggered measurements by starting a infinite loop in the instrument.  The driver may lock the front panel. In addition, by default, the driver sets the firmware to output `OPC` upon the completion of each triggered measurement cycle. This is done by sending a command such this:

```
prepareForTrigger(true,'OPC') waitcomplete()
```

Once the `OPC` message is placed on the output buffer, the instrument turns on the message available (MAV) bit of the status register. Thus, the driver can detect the completion of measurements by monitoring the message available bit of the instrument status register:

```
<read status byte> and MAV equals MAV
```

Once detected, the driver reads the `OPC` message and then fetches the initial, final and thermal transient readings. Typically, the driver also reads the measurement outcome of each of the initial, final and thermal transient measurements as an additional indication of the measurement status.

#### Special Values

The following Special readings might be output (SCPI-Standard Commands for Programmable Instruments):

`SCPI Positive Infinity 9.9E+37` if the measured resistance current is zero and the voltage is positive.
`SCPI Negative Infinity -9.91E+37` if the measured resistance current is zero and the voltage is negative.
`SCPI Not A Number (NaN) 9.91E+37` if the the resistance contact check failed.
`SCPI Not A Number (NaN) 9.91E+34` if the the thermal transient contact check failed, which the driver multiplies by 1000.

<a name="Specifications"></a>
## Specifications

<a name="Meter-Settings"></a>
### Meter Settings

- Channel: `smua` or `smub`.
- Driver: 0 or 1.
- Leads Maximum Resistance: 10 - 999.
- Contact Checks: 1, 3, 5, or 7.

<a name="Thermal_Transient_Response"></a>
### Thermal Transient Response

- Aperture: 0.001 - 0.01 Power Line Cycles.
- Current Level: 10 - 999 mA
- Maximum Voltage: 40 V
- Transient Voltage: 1 mV - 1 V
- Pulse Width: 10-1000 ms
- Accuracy: 0.3 mV RMS
- Current Source Noise: 150 uA RMS at 100-999 mA current level.
- Post Transient Delay: 1 ms - 10 s
- Median Filter Length: 3-9
- Trace Points: 10 - 10000
- Voltage Limit: 0.01 - 9.99 V

<a name="Initial_And_Final_Resistance"></a>
### Initial And Final Resistance

- Current Level: 0.1-10 mA.
- Measurement Duration: 0.01 to 10 power line cycles.
- Accuracy: Depends on power line cycle and current level.
- Typical accuracy at 10 - 100 mA current range:
  - 1% at 0.01 Power Line Cycles.
  - 0.01% at 1 Power Line Cycle.

<a name="Cold_Resistance_Default_Settings"></a>
### Cold Resistance Default Settings

<a name="Cold_Resistance_Voltage_Source"></a>
#### Cold Resistance Voltage Source

- Voltage Level: 20 mV
- Aperture: 1 power line cycles (16.7 ms)
- Current Limit: 40 mA

<a name="Cold_Resistance_Current_Source"></a>
#### Cold Resistance Current Source

- Current Level: 20 mA
- Aperture: 1 power line cycles (16.7 ms)
- Voltage Limit: 100 mV

<a name="Thermal_Transient_Default_Settings"></a>
### Thermal Transient Default Settings

- Aperture: 0.004 power line cycles.
- Current Level: 270 mA
- Voltage Change: 0.099 V
- Pulse Width: 10 ms
- Trace Points: 100
- Median Filter Length: 3
- Voltage Limit: 0.99 V
- Delay time between thermal transient and final resistance measurement: 500 ms

<a name="Attributions"></a>
## Attributions

Last Updated 2024-10-10

&copy; 2011 by Integrated Scientific Resources, Inc.  

This information is provided "AS IS" WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.

Licensed under [The Fair End User] and [MIT] Licenses.

Unless required by applicable law or agreed to in writing, this software is provided "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

Source code is hosted on [GitHub]

[The Fair End User]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/Fair%20End%20User%20License.html
[MIT]: http://opensource.org/licenses/MIT
[GitHub]: https://www.github.com/ATECoder
[TTM Framework Guide]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/TTM%20Framework%20Guide.html
[Four-Terminal Sensing]: https://en.wikipedia.org/wiki/Four-terminal_sensing
[Lua]: https://www.lua.org
