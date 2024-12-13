# Thermal Transient Meter&trade; Instrument User's Guide

## Table of Contents

- [This Guide](#Guide)
- [Preparations](#Preparations)
  - [Wiring the Device Under Test](#Wiring)
  - [Loading the TTM Firmware](#Loading)
  - [Taking the instrument from Remote to Local Modes](#Local)
  - [Starting TTM from the Front Panel](#Starting)
  - [Exiting from TTM Front Panel Mode](#Exiting)
  - [Setup of Measurement Parameters](#Parameters)
	- [Meter](#Meter)
	  - [SMU](#SMU)
	  - [Driver](#Driver)
	  - [Leads](#Leads)
	  - [Contacts](#Contacts)
	- [Resistance](#Resistance)
	  - [Source](#Source)
	  - [Level](#Level)
	  - [Limit](#Limit)
	  - [Aperture](#Aperture)
	  - [Limits](#Limits)
		- [Low Limit](#Low_Limit)
		- [High Limit](#High_Limit)
	  - [Status](#Status)
	- [Transient](#Transient)
	  - [Source Current](#Source_Current)
	  - [Maximum Voltage](#Maximum_Voltage)
	  - [Aperture](#Transient_Aperture)
	  - [Points](#Points)
	  - [Period](#Period)
	  - [Delay](#Delay)
	  - [Limits](#Transient_Limits)
		- [Low Limit](#Transient_Low_Limit)
		- [High Limit](#Transient_High_LImit)
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
      - [old Resistance Current Source](#Cold_Resistance_Current_Source)
  - [Thermal Transient Default Settings](#Thermal_Transient_Default_Settings)
- [Attributions](#Attributions)

<a name="Guide"></a>
## Guide

This guide describes how to use the TTM instrument as a stand-along bench or production line device.

First time readers should start by reading the [TTM Framework Guide].

## Legacy Support

The TTM firmware is is compatibly with earlier versions of TTM Visual Basic drivers released before 2024 (e.g., 3.2.5367, 2.3.4077).

The firmware uses a `MeterDefaults.legacyDriver` and `MeterValues.legacyDriver` flags that is set to 1 for use with the legacy Visual Basic drivers. 

The `MeterValues.legacyDriver` is persistent and can be set from the instrument menu or by the `legacyDriverSetter()` function call.

<a name="Preparations"></a>
## Preparations

<a name="Wiring"></a>
### Wiring the Device Under Test

The device under test must be connected to the TTM Instrument using four-wires:

![](KelvinDiagram.jpg)

<a name="Loading"></a>
### Loading the TTM Firmware

Use the TTM Loader to update the TTM Firmware as described in the [TTM Loader Guide].

<a name="Remote/Local"></a>
### Taking the instrument from Remote to Local Modes

When controlling the instrument from an application program, such as the Loader or Driver Tester, the instrument goes into Remote mode. Press the _EXIT_ button on the instrument panel to go into local mode.

<a name="Starting"></a>
## Starting TTM from the Front Panel

When running TTM the instrument panel display the instrument opening screen, such as 'Therm. Trans. Meter', or the last measurement, such as 'R: 2.0016..'. Otherwise, TTM needs to be loaded and run as follows:

- From the instrument front panel press _LOAD_;
- Select _USER_ from the menu and press _Enter_;
- Select _TTM_ and press _Enter_;
- From the instrument front panel press _LOAD_;
- From the instrument front panel press _RUN_;
- The instrument opening screen, such as 'Therm. Trans. Meter', should be displayed.

<a name="Exiting"></a>
### Exiting from TTM Front Panel Mode

Click the _RUN_ button to exit from the run loop.

<a name="Parameters"></a>
## Setup of Measurement Parameters

- Select _Menu_ from the instrument panel to configure the instrument.
- Follow the prompts from the instrument menu screens.
- See [Specifications](#Specifications) for parameter ranges.

<a name="Meter"></a>
### Meter

Select _Meter_ from the _TTM Main Menu_ to alter the settings of the meter.

<a name="SMU"></a>
#### SMU

This option allows the operator to selected _SMUA_ or _SMUB_ for instruments with dual source meter units. This setting is turned of for instruments with a single source measure unit.

- Select _SMU_ from the _Meter_ menu;
- The _SMU_ prompt will display showing the current value, `smua` or `smub'.
- Press the thumb wheel to select and then set the value top `smua` or `smub`;
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Driver"></a>
#### Driver

Set the legacy driver option `1`. Set to `0` if using software that is compatible with the new features of the firmware.

- Select _Driver_ from the _Meter_ menu;
- The _Legacy Driver_ prompt will display showing the current value, such as `1` for using legacy Visual Basic drivers;
- Press the thumb wheel to select and then set the value to `1` for supporting the the legacy Visual Basic software or `0` otherwise;
- Press _Enter_ to accept or _Exit_ to cancel the changes.


<a name="Leads"></a>
#### Leads

The contact check function prevents measurements that may be in error due to excessive resistance in the force or sense leads when making remotely sensed (Kelvin) measurements. Potential sources for this resistance include poor contact at the device under test (DUT), failing relay contacts on a switching card, and wires that are too long or thin.

The contact check function also detects an open circuit that may occur when a four-point probe is misplaced or misaligned. This relationship is shown schematically in the figure below, where R~C~ is the resistance of the mechanical contact at the DUT, and R~S~ is the series resistance of cables and any series relays.

The leads contacts are checked if the maximum leads resistance (see below) is set to a non-zero positive value. Contacts are measured before taking the initial resistance measurement. The measurement is aborted and the lead resistances are displayed if the measured lead resistances exceeds the maximum limit. 

![](ContactCheckDiagram.png)

To enable and set the maximum allowed leads resistance:

- Select _Leads_ from the _Meter_ menu;
- The _Max Resistance_ prompt will display showing the maximum resistance in Ohms between 10 and 999;
- Press the thumb wheel to select and then increase or decrease the value;
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Contacts"></a>
#### Contacts

The _Contacts_ menu selects when contact checks are to be performed

|Bit|Name|Hex Value|Decimal Value|Description|
|----|----|----|----|
|B0|Pre-Initial-Resistance|0x01|1|Always 1|
|B1|Pre-Thermal-Transient|0x02|2|Check contacts before making the thermal transient measurement|
|B2|Pre-Final-Resistance|0x04|4|Check contacts before making the final cold resistance measurement|

- Select _Contacts_ from the _Meter_ menu;
- The _Contacts_ prompt will display showing the current value;
- Press the thumb wheel to select and then set the value;
- For example, set the value to `7` (1+2+4) for checking contacts before the initial, final, and thermal transient measurements;
- Press _Enter_ to accept or _Exit_ to cancel the changes.

Note that the program forces the 1 bit thus forcing a contact check before each initial resistance measurement.

<a name="Resistance"></a>
### Resistance

Select _Resistance_ from the _TTM Main Menu_ to alter the settings of the initial and final cold resistance (bridge wire resistance at low current level).

<a name="Source"></a>
#### Source

Either a Voltage or a Current source can be selected to force voltage or current, respectively for measuremnt the cold resistance.

- Select _Source_ from the _Cold Resistance_ menu;
- The _Voltage_ and _Current_ options will display;
- Rotate the thumb wheel to highlight either _Voltage_ or _Current_;
- Press _Enter_ to accept or _Exit_ to cancel the selection.

<a name="Level"></a>
#### Level

The source level is the voltage (if using a Voltage Source) or current (if using a current source) for measuring the cold resistance.

The selected level is forced by the TTM instrument onto or through  the bridge-wire to measure the bridge wire resistance. This forcing level must be small so as not to trip or ecessively heat the bridge-wire.

- Select _Level_ from the _Cold Resistance_ menu;
- The _Level_ prompt will display showing the source voltage or current that was last set;
- Press the thumb wheel to select and then increase or decrease the value;
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Limit"></a>
#### Limit

The maximum voltage is the limit set by the instrument for the maximum voltage that could develop on the bridge wire during the cold resistance test. This limit will be reach if the bridge wire is open or exhibits a very high resistance.

- Select _Voltage_ from the _Cold Resistance_ menu;
- The _Maximum Voltage_ prompt will display showing the voltage that was last set in volts.
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Aperture"></a>
#### Aperture

The aperture (integration period) defines the duration and accuracy of the cold resistance measurement. This value is defined in multiples or fractions of power line cycles (PLC). 1 PLC equals to 1/60th of a second in the US. At 1 PLC the measurement accuracy exceeds 0.01%.

- Select _Aperture_ from the _Cold Resistance_ menu;
- The _Integration Period_ prompt will display showing the number of power line cycles was last set in PLC;
- Press the thumb wheel to select and then increase or decrease the value;
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Limits"></a>
#### Limits

Limits are used to determine if the measured value is within range (pass) or out of range (fail). 

The initial and final cold resistances are checked against the limits and the measurement outcome is set to High or Low if the values are out of range. 

If the initial resistance measurement fails, the measured value is displayed along with the outcome status (see [Status](#Status) below).

- Select _Limits_ from the _Cold Resistance_ menu to alter the limits.

<a name="Low_Limit"></a>
##### Low Limit

- Select _Low_ from the _Limits_ menu;
- The _Low Limit_ prompt will display showing the last value that was set in ohms;
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="High_Limit"></a>
##### High Limit

- Select _High_ from the _Limits_ menu;
- The _High Limit_ prompt will display showing the last value that was set in ohms;
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Status"></a>
#### Status

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
- Press _Enter_ to accept or _Exit_ to cancel the changes.

The Status settings of the Thermal Transient Meter allows the operator to select what status values are interpreted as a failed measurement.

With the 4009 of the TTM firmware both over-temp and compliance were taken to indicate a failure. With the new firmware, the operator can choose accept a measurement under compliance condition. For example, the operator may choose to apply a voltage of 100 mV but limit the current at 20 mA. When making this measurement on a 2 Ohm bridge-wire, the instrument will _hit compliance_. The measurement accuracy will still be acceptable. In the case, the operator will set the Status settings to 2 instead of the default of 66.

The measurement outcome and status are displayed along with the bridge-wire resistance and low or high indication if the initial cold resistance measurement failed. Typically a status value of 28 will display indicating that a 4-wire measurement was taken under auto range conditions.

##### Legacy support
In order to support the legacy Visual Basic software the status settings defaults to over-temp only.

<a name="Transient"></a>
### Transient

Use _Transient_ menu items to set the test parameters of the thermal transient forcing and measurement parameters.

- Select _Transient_ from the _TTM Main Menu_ to alter the settings of the thermal transient response measurement of the bridge wire.

<a name="Source_Current"></a>
#### Source Current

The source current is the current that is forced by the TTM instrument through the bridge wire to measure the bridge wire thermal transient response. This current is pulsed through the bridge wire for a very short duration determined by the period times points (see below).

- Select _Current_ from the _Thermal Transient_ menu;
- The _Source Current_ prompt will display showing the source current that was last set in milli Amperes.
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Maximum_Voltage"></a>
#### Maximum Voltage

The maximum voltage is the limit set by the instrument for the maximum voltage that could develop on the bridge wire during the thermal transient test. This limit will be reached if the bridge wire is open or exhibits a very high resistance.

- Select _Voltage_ from the _Thermal Transient_ menu;
- The _Maximum Voltage_ prompt will display showing the voltage that was last set in volts.
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Transient_Aperture"></a>
#### Aperture

The aperture (integration period) defines the duration and accuracy of the thermal transient measurement. This value is defined in multiples or fractions of power line cycles (PLC). 1 PLC equals to 1/60th of a second in the US. Thermal transients are measured during a very short pulse period. Therefore, a very small integration period is allowed for the thermal transient.

- Select _Aperture_ from the _Thermal Transient_ menu;
- The _Integration Period_ prompt will display showing the number of power line cycles was last set in PLC.
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Points"></a>
#### Points

During the thermal transient test a current pulse is applied to the bridge wire. The pulse duration is determined by the period (see below) times the Points value.

- Select _Points_ from the _Thermal Transient_ menu;
- The _Data Points_ prompt will display showing the last settings.
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Period"></a>
#### Period

The duration of the thermal transient current pulse equals the period times points (see above).

- Select _Period_ from the _Thermal Transient_ menu;
- The _Sampling Interval_ prompt will display showing the last settings in micro seconds.
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Delay"></a>
#### Delay

Delay sets the time the instrument will pause between the thermal transient measurement and the final resistance measurement. This time is designed to allow the bridge wire to cool down before making the final resistance measurement.

- Select _Delay_ from the _Thermal Transient_ menu;
- The _Final Cold Resistance Delay_ prompt will display showing the last settings in mill seconds.
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Transient_Limits"></a>
#### Limits

Limits are used to determine if the thermal transient measurement is within range (pass) or out of range (fail).

- Select _Limits_ from the _Thermal Transient_ menu to alter the limits.

<a name="Transient_Low_Limit"></a>
##### Low Limit

- Select _Low_ from the _Limits_ menu;
- The _Low Limit_ prompt will display showing the last value that was set in milli volts.
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Transient_High_Limit"></a>
##### High Limit

- Select _High_ from the _Limits_ menu;
- The _High Limit_ prompt will display showing the last value that was set in milli volts.
- Press the thumb wheel to select and then increase or decrease the value.
- Press _Enter_ to accept or _Exit_ to cancel the changes.

<a name="Measuring_Using_the_Front_Panel"></a>
## Measuring Using the Front Panel

The instrument takes a new measurement when triggered.

Triggering form the front panel of the instrument is done as follows:

- Start the instrument.
- Press the _Run_ button on the instrument front panel.
- Press the _Trig_ button to obtain new measurements.

Note that the instrument scripts must be loaded and verified first using the TTM Console application. Otherwise, the scripts will not run and the front panel will display R: 0.0000 with a blinking R:.

A failed measurement is designated on the instrument panel by a blinking prefix character. For example, if Final Resistance failed, the R preceding the measured value will blink with changing intensity. Upon a failure of the initial resistance measurement the instrument display the resistance, the high or low indication, the outcome value and status (see [status] above}.

To exit from the _<TRIG MENU RUN>_ mode, press the _RUN_ key.

[Reading the Outcome]

The measurement outcomes can be read from the digital I/O.

<a name="Measuring_Using_the_TRG_Command"></a>
## Measuring Using the \*TRG command

The instrument takes a new measurement when triggered.

The instrument \*TRG command can be used to trigger a measurement as follows:

- Start the instrument;
- Connect the instrument using an external interface such as GPIB or Ethernet;
- Send the \*TRG command.

A failed measurement is designated on the instrument panel by a blinking prefix character. For example, if Initial Resistance failed, the R preceding the measured value will blink with changing intensity.

[Reading the Outcome]

The measurement outcomes can be read from the digital I/O.

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

Upon a failure of the initial resistance measurement the instrument display the resistance, the high or low indication, the outcome value and status (see [status] above}. The outcome bit values are:

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
The outcome value is set to `nil` upon clear.
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

Source code is hosted on [Bit Bucket].and [Github]

[The Fair End User]: http://www.isr.cc/licenses/Fair%20End%20User%20Use%20License.pdf
[MIT]: http://opensource.org/licenses/MIT
[Bit Bucket].: https://www.bitbucket.org/davidhary
[GitHub].: https://www.github.com/ATECoder
[TTM Framework Guide]: TTM%20Framework%20Guide.md
[TTM Loader Guide]: TTM%20Loader%20Guide.md
[TTM API Guide]: TTM%20API%20Guide.md
