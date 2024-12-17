# ISR Thermal Transient Meter<sup>&trade;</sup> Driver API Changes

* [Description](#Description)
* [Namespaces and Construction](#Namespaces_Construction)
* [Legacy Device Methods](#name=Legacy_Device_Methods)
* [Legacy Device Properties](#Legacy_Device_Properties)

## Description[](#){name=Description}

The ISR Thermal Transient Meter framework includes a new set of classes to replace the legacy `isr.TTM` device and the associated configuration classes. 

This document includes a dictionary for replacing the legacy device driver calls with the new Meter class.e capabilities.

## Namespaces and Construction[](#){name=Namespaces_Construction}

* Name: `Meter`

|type|Legacy Name|New Name|Description|
|----|----|----|----|
|using / Imports | isr.Ttm.Driver | cc.isr.VI.Tsp.K2600.Ttm.Meter | Imports all the types from a single namespace | 
|construction | Dim ttm As New Device | Dim ttm As New Meter( <Setting Assembly Member Type> ) | Constructs an instance of the device class |

The `Setting Assembly Member Type` tells the `Meter` class where to look for the `JSon` settings file. For instance, if constructing the `Meter` from a Console class, this argument becomes `this.GetType()` in C#.

## Legacy Device Methods[](#){name=Legacy_Device_Methods}

|Name | Associated Object | Name | Description|
|----|----|----|----|
| AbortMeasurement | Meter | AbortMeasurement | Aborts the measurements by issuing a trigger if awaiting a trigger | 
| ColdResistanceVoltageApertureSetter | Meter.InitialResistance | WriteCurrentLevel | Sets the aperture for measuring the initial resistance | 
|                                     | Meter.FinalResistance | WriteCurrentLevel | Sets the aperture for measuring the final resistance | 
| ColdResistanceVoltageHighLimitSetter | Meter.InitialResistance | WriteHighLimit | Sets the high limit for measuring the initial resistance | 
|                                      | Meter.FinalResistance | WriteHighLimit | Sets the high limit for measuring the final resistance | 
| ColdResistanceVoltageLowLimitSetter | Meter.InitialResistance | WriteLowLimit | Sets the low limit for measuring the initial resistance | 
|                                     | Meter.FinalResistance | WriteLowLimit | Sets the low limit for measuring the final resistance | 
| ColdResistanceCurrentLevelSetter | Meter.InitialResistance | WriteCurrentLevel | Sets the current level for measuring the initial resistance | 
|                                  | Meter.FinalResistance | WriteCurrentLevel | Sets the current level for measuring the final resistance | 
| ColdResistanceVoltageLimitSetter | Meter.InitialResistance | WriteVoltageLimit | Sets the voltage limit for measuring the initial resistance | 
|                                  | Meter.FinalResistance | WriteVoltageLimit | Sets the voltage limit for measuring the final resistance | 
| Connect                  | Meter.TspDevice | TryOpenSession | Opens a VISA session | 
| FlushRead                | N/A | N/A | Clears the instrument output buffer | 
| IsMeasurementCompleted   | Meter.TspDevice.Session | IsMessageAvailableBitSet | Reads the status byte and returns true if the message available bit is set. | 
| Measure                  | TBD | TBD | Takes initial resistance thermal transient and final resistance measurements. |
| MeasureFinalResistance   | Meter.FinalResistance | TBD | Measures the final resistance | 
| MeasureInitialResistance | Meter.InitialResistance | TBD | Measures the initial resistance | 
| MeasureShuntResistance   | Meter.ShuntResistance | TBD | Measures the shunt resistance | 
| MeasureThermalTransientResistance | Meter.ThermalTransient | TBD | Measures the thermal transient | 
| PrepareForTrigger        | Meter | PrepareForTrigger | Tells the firmware to wait for a trigger| 
| ReadMeasurements         | TBD | TBD | Reads initial resistance, final resistance and thermal transient measurements. | 
| ClearDisplayMeasurement  | cc.isr.VI.Tsp.K2600.DisplaySubsystem | ClearDisplayMeasurement | Clears the display if not in measurement mode and set measurement mode |
| DisplayCharacter | cc.isr.VI.Tsp.K2600.DisplaySubsystem | DisplayCharacter | Displays the character |
| DisplayLine      | cc.isr.VI.Tsp.K2600.DisplaySubsystem | DisplayLine | Displays message on the display |
| RestoreDisplay   | cc.isr.VI.Tsp.K2600.DisplaySubsystem | RestoreDisplay | Restores the instrument display to its main mode |
| ConfigureColdResistance   | TBD | TBD | Configures the Thermal Transient Meter for making Cold Resistance measurements |
| ConfigureShuntResistance  | TBD | TBD | Configures the Thermal Transient Meter for making Shunt Resistance measurements|
| ConfigureThermalTransient | TBD | TBD |Configures the Thermal Transient Meter for making Thermal Transient  measurements |
| IsContactChecks    | cc.isr.VI.Tsp.K2600.ContactSubsystem or Firmware 2.4.x | TBD | Determines whether contact resistances are below the specified threshold. |
| TriggerMeasurement | cc.isr.VI.Tsp.K2600.TriggerSubsystem | TBD | Sends a signal to trigger a measurement. |

## Legacy Device Properties[](#){name=Legacy_Device_Properties}

|Name|Associated Object|Name|Description|
|----|----|----|----|
| IsConnected| Meter | IsSessionOpen | Returns true if a VISA session is open|
| ContactResistances | cc.isr.VI.Tsp.K2600.ContactSubsystem or Firmware 2.4.x | TBD | Gets the contact resistances |
| ColdResistanceConfig | Meter.ConfigInfo | ColdResistance | Defines the attributes of the cold resistance measurements |
| FinalResistance      | Meter.ConfigInfo | FinalResistance | Defines the attributes of the final resistance measurements |
| FirmwareReleasedVersion | TBD | TBD | Holds the firmware version |
| Identity          | TBD | TBD | Holds the instrument identity string |
| InitialResistance | Meter.ConfigInfo | InitialResistance | Defines the attributes of the initial resistance measurements |
| PostTransientDelayConfig | Meter.ConfigInfo | MeterMain | Defines global meter attributes |
| PostTransientDelay       | Meter | MeterMain | Defines global meter attributes |
| ShuntResistanceConfig | Meter.ConfigInfo | ShuntResistance | Defines the attributes of the shunt resistance measurements |
| ShuntResistance       | Meter | ShuntResistance | Defines the attributes of the shunt resistance measurements |
| Synchronizer           | TBD | TBD | Gets or sets the ISynchronizeInvoke object for marshaling synchronization events. |
| ThermalTransientConfig | Meter.ConfigInfo | ThermalTransient | Defines the attributes of the thermal transient measurements |
| ThermalTransient       | Meter | ThermalTransient | Defines the attributes of the thermal transient measurements |

## New Device Methods[](#){name=New_Device_Methods}

|Name|Associated Object|Name|Description|
|----|----|----|----|
| Set Source Meter Name | Meter.MeterMain | WriteSmuName | Sets the name and select the source measure unit for all measurements | 
| Get Source Meter Name | Meter.MeterMain | QuerySmuName | Gets the name of the source measure unit that is used for all measurements | 
| Set Contact Threshold | Meter.MeterMain | WriteContactLimit | Sets the threshold for detecting an open leads contact | 
| Get Contact Threshold | Meter.MeterMain | QueryContactLimit | Gets the threshold for detecting an open leads contact | 

## New Device Properties[](#){name=New_Device_Properties}

|Name|Associated Object|Name|Description|
|----|----|----|----|
| Low Contact Resistance | Meter.InitialResistance | LowLeadsResistance | The contact resistance of the low leads as measured before taking the initial resistance reading. |
|                        | Meter.FinalResistance | LowLeadsResistance | The contact resistance of the low leads as measured before taking the final resistance reading. |
|                        | Meter.ThermalTransient | LowLeadsResistance | The contact resistance of the low leads as measured before taking the final resistance reading. |
| High Contact Resistance | Meter.InitialResistance | HighLeadsResistance | The contact resistance of the high leads as measured before taking the initial resistance reading. |




