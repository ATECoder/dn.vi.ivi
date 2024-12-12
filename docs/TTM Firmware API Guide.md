# Thermal Transient Meter&trade; (TTM) Firmware API Guide

The guide contains the Firmware level application programming interface for the Thermal Transient Meter&trade;

## Contents

- [Description](#Description)
- [Attributions](#Attributions)

<a name="Description"></a>
## Description

The TTM firmware is based on the Test Script Processing (TSP) firmware incorporated in the [Keithley] 2600 instruments. TSP is based on the [Lua] programming language.

The TTM firmware consists of a main [isr_ttm] script, a support [isr_support] script and a startup [isr_ttm_boot] script. These scripts are loaded and saved in the instrument using a loader program described in the [TTM Loader Guide], which is part of the TTM Framework described in the [TTM Framework Guide],

### Status

This document includes a partial description of the TTM firmware and is work in progress. 


<a name="isr_support"></a>
## ISR Support

The TTM firmware is based on a set of [TSP] and [Lua] script files that make the [isr_support] script.

<a name="isr_ttm"></a>
## ISR TTM

The [isr_ttm] script is the core script of the TTM framework for taking the Thermal Transient set of measurements. 

The [isr_ttm] scripts consists of three Lua classes, [Cold Resistance], [Trace] and [Estimator]. These classes are organized as a [Meter] to implement a sequence of measurements orchestrated by a [sequencer] and set up by a [user interface]. The entire system 

### The TTM Namespace

The Thermal Transient Meter namespace is set to `isr.meters.thermalTransient`, The [startup](#isr_ttm_boot) script aliases the name space as `ttm`.

```
local ttm = _G.isr.meters.thermalTransient
```

In addition, the [sequencer](#sequencer) script initializes the following instances of the TTM classes:

* `ttm.ir` - the initial resistance instance of the [cold resistance] class.
* `ttm.fr` - the final resistance instance of the [cold resistance] class.
* `ttm.tr` - the thermal transient trace instance of the [trace] class.
* `ttm.est` - the thermal transient trace estimator instance of the [estimator] class.

```
  --- <summary> Global instance of the initial resistance with its default settings. </summary>
  ir = coldResistance:new()

  --- <summary> Global instance of the final resistance with its default settings. </summary>
  fr = coldResistance:new()

  --- <summary> Global instance of the thermal transient trace with its default settings. </summary>
  tr = trace:new()

  --- <summary> Global instance of the thermal transient estimator with its default settings. </summary>
  est = estimator:new()
```

<a name="Cold_Resistance"></a>
### Cold Resistance

The [Cold Resistance] class controls the measurement of the initial and final resistances. The two instances of the [Cold Resistance] class are accessible as `ttm.ir` and `ttm.fr` corresponding to the initial and final resistances. The `isr.meters.thermalTransient` namespace is assigned to `ttm` in the [startup](#isr_ttm_boot) script:

#### Cold Resistance Methods

Note: the `x` in the method and property names listed below stands for `i` (initial) or `f` final resistance instance of the [cold resistance] class. 
:
##### `ttm.xr:init()`

* Initializes this class setting all properties to their cached values. This method executed when the class is instantiated..
* Returns: true.

##### `ttm.xr:reset()`

* Resets all properties to their default values.
* Returns: true.

##### `ttm.xr:clear()`

* Clears measurement and calculated values.
* Returns: true.

The following values are initialized by the `ttm.ir:Clear()` and `ttm.fr:Clear()` methods, which are called from `ttm.clearMeasurements()`, prior to each measurement sequence.

* `ttm.xr.low` <sup>[1](#N1)</sup> (`nil`) - true if the measured value is lower than the set [cold resistance low limit].
* `ttm.xr.high` <sup>[1](#N1)</sup> (`nil`) - true if the measured value is higher than the set [cold resistance high limit].
* `ttm.xr.pass` (`nil`) - true if the the measured value is withing the [[cold resistance high limit],[cold resistance high limit]] range.
* `ttm.xr.current` (`nil`, Amperes) - the measured current as read from the current buffer.
* `ttm.xr.voltage` (`nil`, Volts) - the measured voltage as read from the voltage buffer.
* `ttm.xr.resistance` (`nil`, Ohms) - the resistance calculated by dividing the measured voltage by the measured current.
* `ttm.xr.status` (`nil`) - the [Buffer Status](#Buffer_Status)
* `ttm.xr.outcome` (`nil`) - the [Outcome](#Outcome_Values) of the [Cold Resistance] measurement.
* `ttm.xr.highContact` <sup>[1](#N1)</sup> (`nil`) - the contact resistance of the failed high leads or 0 if contact check passed.
* `ttm.xr.lowContact` <sup>[1](#N1)</sup> (`nil`) - the contact resistance of the failed low leads or 0 if contact check passed.
* `ttm.xr.contactsOkay` <sup>[1](#N1)</sup> (`nil`) - the outcome of the contact check.

<a name="Trace"></a>
### Trace

The [Trace] class controls the measurement of the thermal transient. The instance of the [Trace] class is accessible as `ttm.tr`.

#### Cold Resistance Methods

Note: the `x` in the method and property names listed below stands for `i` (initial) or `f` final resistance instance of the [cold resistance] class.

##### `ttm.tr:init()`

* Initializes this class setting all properties to their cached values. This method executed when the class is instantiated..
* Returns: true.

##### `ttm.tr:reset()`

* Resets all properties to their default values.
* Returns: true.

##### `ttm.tr:clear()`

* Clears measurement and calculated values.
* Returns: true.

The following values are initialized by the `ttm.tr:Clear()` method, which is called prior to each measurement sequence.

* `ttm.tr.low` <sup>[1](#N1)</sup> (`nil`) - true if the measured value is lower than the set [voltage change low limit].
* `ttm.tr.high` <sup>[1](#N1)</sup> (`nil`) - true if the measured value is higher than the set [voltage change high limit].
* `ttm.tr.pass` (`nil`) - true if the the measured value is withing the [[voltage change high limit],[voltage change high limit]] range.
* `ttm.tr.voltageChange` (`nil`, Volts) - the voltage changed estimated from the measured thermal transient trace.
* `ttm.tr.outcome` (`nil`) - the [Outcome](#Outcome_Values) of the [Voltage Change] measurement.
* `ttm.xr.highContact` <sup>[1](#N1)</sup> (`nil`) - the contact resistance of the failed high leads or 0 if contact check passed.
* `ttm.xr.lowContact` <sup>[1](#N1)</sup> (`nil`) - the contact resistance of the failed low leads or 0 if contact check passed.
* `ttm.xr.contactsOkay` <sup>[1](#N1)</sup> (`nil`) - the outcome of the contact check.
* `ttm.xr.status` (`nil` <sup>[1](#N1)</sup>) - the [Buffer Status](#Buffer_Status) derived from all status values of the voltage or current buffers.

<a name="Estimator"></a>
### Estimator

The [Estimator] class estimates the parameters of the Thermal Transient. The instance of the [Estimator] is accessible as `ttm.est`. 

##### `ttm.est:init(alpha)`

* Initializes this class setting all properties to their cached values. This method executed when the class is instantiated..
* `alpha` - Specifies the temperature coefficient of resistivity in <sup>o</sup>C<sup>-1</sup>.
* Returns: true.

##### `ttm.est:reset()`<sup>[1](#N1)</sup>

* Resets all properties to their default values.
* Returns: true.

##### `ttm.est:clear()`

* Clears measurement and calculated values.
* Returns: true.

The following values are initialized by the `ttm.est:Clear()`, which is called prior to each measurement sequence.

* `ttm.est.thermalCoefficient` (`0.0005`) - estimated initial voltage.
* `ttm.est.initialVoltage` (`nil`) - estimated initial voltage.
* `ttm.est.finalVoltage` (`nil`) - estimated final voltage.
* `ttm.est.voltageChange` (`nil`) - estimated voltage change.
* `ttm.est.temperatureChange` (`nil`) - net relative voltage change (change minus cold resistance voltage divided by the initial voltage) divided by temperature coefficient.
* `ttm.est.thermalConductance` (`nil`) - dissipated power divided by the temperature change.
* `ttm.est.thermalTimeConstant` (`nil`) - estimated from the thermal transient trace and the half voltage change.
* `ttm.est.thermalCapacitance` (`nil`) - thermal conductance times thermal time constant.
* `ttm.est.outcome` (`nil`) - the outcome of the estimate. Set to 0 (Okay) when estimating the voltage change.

<a name="Meter"></a>
### Meter

<a name="Sequencer"></a>
### Sequencer

<a name="User interface"></a>
### User Interface

<a name="Measurement"></a>
### Measurement

The thermal transient measurement sequence is as follows:

#### (1) Initial Contact check

A contact check measurement is initiated upon receiving the measurement trigger. The [contactsOkay] value is set to true if the contact check passed. Otherwise, the value is set to false and the [highContact] and [lowContact] resistances of the high and low leads are measured and set.

##### Initial Contact check passed

The measurement sequence proceeds to measure the initial cold resistance.

##### Initial Contact check failed

The initial cold resistance outcome is set to [Failed Contacts].

###### Initial Contact check failed - Legacy option

* The outcome [bad status] bit is set.
* The initial resistance is set to [NaN] = 9.91e+37.

#### (2) Initial Cold Resistance

A constant current or a constant voltage is applied and the resistance is estimated based on the ratio of voltage to current.

The outcome is set per the following criteria:

If both voltage and current are positive, the resistance is estimated and the resistance [low], [high], and [pass] outcomes are determined.

Otherwise, the resistance is set to [NaN] and the outcome bit [measurement failed] is set.  The [Bad Status] bit is set if the legacy option is turned on.

#### (3) (optional) Pre-Trace Contact check

A contact check measurement is initiated if the initial resistance measurement passed. The [contactsOkay] value is set to true if the contact check passed. Otherwise, the value is set to false and the [highContact] and [lowContact] resistances of the high and low leads are measured and set.

##### Pre-Trace Contact check passed

The measurement sequence proceeds to acquire the thermal transient trace.

##### Pre-Trace Contact check failed

The [Trace] outcome is set to [Failed Contacts].

###### Pre-Trace Contact check failed - Legacy option

* The outcome [bad status] bit is set.
* The voltage change is set to 0.001 * [NaN] = 9.91e+34.

#### (4) Thermal Transient Trace

A constant current pulse is applied for a brief duration and the current and voltage values are measured at a high rate for a specified point count.

The trace thus acquired is used by the estimator to estimate the voltage change relative to the equivalent voltage estimated across an initial resistance value.

The outcome is set per the following criteria:

- if 
- Otherwise, the transient voltage is set to 0.001 * [NaN] and the outcome bit [measurement failed] is set.  The [Bad Status] bit is set if the legacy option is turned on.

#### (5) (optional) Final Contact check

A contact check measurement is initiated if the thermal transient trace measurement passed. The [contactsOkay] value is set to true if the contact check passed. Otherwise, the value is set to false and the [highContact] and [lowContact] resistances of the high and low leads are measured and set.

##### Final Contact check passed

The measurement sequence proceeds to measure the final resistance.

##### Final Contact check failed

The [Trace] outcome is set to [Failed Contacts].

###### Final Contact check failed - Legacy option

The final cold resistance outcome is set to [Failed Contacts].

###### Final Contact check failed - Legacy option

* The outcome [bad status] bit is set.
* The final resistance is set to [NaN] = 9.91e+37.

#### (6) Final Cold Resistance

A constant current or a constant voltage is applied and the resistance is estimated based on the ratio of voltage to current.

The outcome is set per the following criteria:

If both voltage and current are positive, the resistance is estimated and the resistance [low], [high], and [pass] outcomes are determined.

Otherwise, the resistance is set to [NaN] and the outcome bit [measurement failed] is set.  The [Bad Status] bit is set if the legacy option is turned on.

<a name="isr_ttm_boot"></a>
## ISR TTM BOOT

The [isr_ttm_boot] script starts when the instrument is powered up. This script runs the framework scripts, which are then ready to accept trigger or user interface commands.


<a name="Meter"></a>
## Meter

The [Meter] script holds the Meter-level settings and enumerations of the TTM framework/

<a name="Outcome_Values"></a>
### Outcome values

An outcome property is used by the [Cold Resistance], [Trace] and [Estimator] classes to denote any measurement failures. 

The bit values of the Cold Resistance outcome are:

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

<a name="Buffer_Status"></a>
### Buffer Status

Upon reading the resistance, the cold resistance class fetches a _Buffer Status_ byte where each bit represents a specific setting or outcome as follows:

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



<a name="Notes"></a>
## Notes

<a name="N1"></a>
### N1
Firmware version 2.4 and up.


<a name="Attributions"></a>
## Attributions

Last Updated 2024-10-30

&copy; 2011 by Integrated Scientific Resources, Inc.  

This information is provided "AS IS" WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.

Licensed under [Creative Commons Attribution 4.0 International Public License] and [MIT] Licenses.

Unless required by applicable law or agreed to in writing, this software is provided "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

Source code is hosted on [Bit Bucket].

[Creative Commons Attribution 4.0 International Public License]: https://bitbucket.org/davidhary//dn.vi.git/license
[MIT]: https://bitbucket.org/davidhary//dn.vi.git/license-code
[TTM Driver 4 API Guide]: https://bitbucket.org/davidhary//dn.vi.git/src/vi/k2600.ttmware/docs/TTM%20Driver%204%20API%20Guide.md
[TTM Firmware Guide]: https://bitbucket.org/davidhary//dn.vi.git/src/vi/k2600.ttmware/docs/TTM%20Firmware%20Guide.md
[TTM Framework Guide]: https://bitbucket.org/davidhary//dn.vi.git/src/vi/k2600.ttmware/docs/TTM%20Framework%20Guide.md
[TTM Instrument Guide]: https://bitbucket.org/davidhary//dn.vi.git/src/vi/k2600.ttmware/docs/TTM%20Instrument%20Guide.md
[TTM Leaflet]: https://bitbucket.org/davidhary//dn.vi.git/src/vi/k2600.ttmware/docs/TTM%20Leaflet.md
[TTM Loader Guide]: https://bitbucket.org/davidhary//dn.vi.git/src/vi/k2600.ttmware/docs/TTM%20Loader%20Guide.md
[TTM Manual Test Guide]: https://bitbucket.org/davidhary//dn.vi.git/src/vi/k2600.ttmware/docs/TTM%20Manual%20Test%20Guide.md
[The Fair End User]: http://www.isr.cc/licenses/Fair%20End%20User%20Use%20License.pdf
[Bit Bucket]: https://bitbucket.org/davidhary
[Lua](https://www.lua.org)
[Keithley]: (https://www.keithley.com)
