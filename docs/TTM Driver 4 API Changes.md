# ISR Thermal Transient Meter<sup>&trade;</sup> Changes from Drivers version 3 and 4

* [Description](#Description)
* [Namespaces and Construction](#Namespaces_Construction)
* [Legacy Device Methods](#name=Legacy_Device_Methods)
* [Legacy Device Properties](#Legacy_Device_Properties)
* [Deprecated Methods](#){name=Deprecated_Methods}
* [Deprecated Properties](#){name=Deprecated_Properties}

The updated ISR Thermal Transient Meter framework includes a `LegacyDevice` class, which is compatible with the legacy `isr.TTM.Driver.Device` class. The `LegacyDevice` class implements the legacy methods and properties that have been implemented in existing applications of the TTM Framework. This determination is based on source code provided to ISR. 

This document delineates the properties and methods of the 3.2.5367 and 2.3.4077 releases as they are implemented in the `LegacyDevice`. Also included are the methods and properties of the updated framework that implement the properties and methods that were omitted in the `LegacyDevice` cl. 

## Namespaces and Construction[](#){name=Namespaces_Construction}

* Name: `LegacyDevice`

|type|Legacy Name|New Name|Description|
|----|----|----|----|
|using / Imports | isr.Ttm.Driver | cc.isr.VI.Tsp.K2600.Ttm.Legacy | Imports all the types from a single namespace | 
|construction | Dim ttm As New isr.TTM.Device | Dim ttm As New LegacyDevice( <Setting Assembly Member Type> ) | Constructs an instance of the legacy device class |

The `Setting Assembly Member Type` tells the `Meter` class where to look for the `JSon` settings file. For instance, if constructing the `Meter` from a Console class, this argument becomes `this.GetType()` in C#.

## Legacy Device Methods[](#){name=Legacy_Device_Methods}

|Name | Description |
|----|----|----|----|
| AbortMeasurement | Aborts the measurements by issuing a trigger if awaiting a trigger | 
| Connect | Opens a VISA session| 
| ColdResistanceVoltageApertureSetter | Sets the apperture for measuring the initial resistance | 
| ColdResistanceVoltageHighLimitSetter | Sets the high limit for measuring the initial resistance | 
| ColdResistanceVoltageLowLimitSetter | Sets the low limit for measuring the initial resistance | 
| ColdResistanceCurrentLevelSetter | Sets the current level for measuring the initial resistance | 
| ColdResistanceVoltageLimitSetter | Sets the voltage limit for measuring the initial resistance | 
| FlushRead | Clears the instrument output buffer | 
| IsMeasurementCompleted | Reads the status byte and returns true if the message available bit is set. | 
| Measure | Takes initial resistance thermal transient and final resistance measurements. |
| MeasureFinalResistance | Measures the final resistance | 
| MeasureInitialResistance | Measures the initial resistance | 
| MeasureShuntResistance | Measures the shunt resistance | 
| MeasureThermalTransientResistance | Measures the thermal transient | 
| PrepareForTrigger | Tells the firmware to wait for a trigger| 
| ReadMeasurements | Reads initial resistance, final resistance and thermal transient measurements. | 
| IsMeasurementCompleted | Returns true if the trigger cycle completed. | 

## Legacy Device Properties[](#){name=Legacy_Device_Properties}

|Name | Description |
|----|----|----|----|
| ColdResistanceConfig | Defines the attributes of the cold resistance measurements |
| FinalResistance | Defines the attributes of the final resistance measurements |
| InitialResistance | Defines the attributes of the initial resistance measurements |
| IsConnected| Is a VISA session is open|
| PostTransientDelayConfiain | Defines global meter attributes |
| PostTransientDelay | meter attributes |
| ShuntResistance | Contains the attributes of the shunt resistance measurements |
| ShuntResistanceConfig | Defines the attributes of the shunt resistance measurements |
| Synchronizer | Invoke object for marshaling synchronization events. |
| ThermalTransient | Contains the attributes of the thermal transient measurements |
| ThermalTransientConfig | Defines the attributes of the thermal transient measurements |

## Deprecated Classes[](#){name=Deprecated_Classes}

Listed below are the classes and interfaces that are not longer implemented in the new legacy device along with compatible classes from the updated TTM framework.

|Legacy Entity| Compatible Entity | Description |
|----|----|----|----|
| IShuntReistanceConfig | ShuntResitanceBase | Defines the shunt resistance configuration. |
| ShuntReistanceConfig | ShuntReistance | Defines the shunt resistance configuration. |
| IShuntReistance | ShuntResitanceBase | Measures the shunt resistance. |
| ShuntReistance | K2600Device.MeasureResistanceSubsystem | Measures the shunt resistance. |

## Deprecated Methods[](#){name=Deprecated_Methods}

Listed below are the methods that are not longer implemented in the new legacy device along with compatible methods from the updated TTM framework.

|Legacy Method |Compatible Class| Compatible Method |Description|
|----|----|----|----|
| ClearDisplayMeasurement | cc.isr.VI.Tsp.K2600.DisplaySubsystem | ClearDisplayMeasurement | Clears the display if not in measurement mode and set measurement mode |
| ConfigureColdResistance | TBD | TBD | Configures the Thermal Transient Meter for making Cold Resistance measurements |
| ConfigureShuntResistance | TBD | TBD | Configures the Thermal Transient Meter for making Shunt Resistance measurements|
| ConfigureThermalTransient | TBD | TBD |Configures the Thermal Transient Meter for making Thermal Transient  measurements |
| DisplayCharacter | cc.isr.VI.Tsp.K2600.DisplaySubsystem | DisplayCharacter | Displays the character |
| DisplayLine | cc.isr.VI.Tsp.K2600.DisplaySubsystem | DisplayLine | Displays message on the display |
| FindInstrumentResource | TBD | TBD | Find Instrument VISA resource names |
| HasCompleteData | Meter | HasCompleteData | Returns true if all measurements completed. |
| IsContactChecks | cc.isr.VI.Tsp.K2600.ContactSubsystem or Firmware 2.4.x | TBD | Determines whether contact resistances are below the specified threshold. |
| MergeOutcomes   | Meter | MergeOutcome | Merges the Measurement Outcomes of the initial, final, and thermal transient measurements. |
| Measure | Meter | Measure | Measures the initial resistance, thermal transient and final resistance. |
| MeasureFinalResistance   | Meter | MeasureFinalResistance | Measures the final resistance. |
| MeasureInitialResistance | Meter | MeasureInitialResistance | Measures the initial resistance. |
| MeasureShuntResistance   | Meter | MeasureShuntResistance | Measures the shunt resistance. |
| MeasureThermalTransient  | Meter | MeasureThermalTransient | Measures the thermal transient. |
| ParseOutcome       | Meter | ParseOutcome | Parses the firmware outcome and firmware status to a Measurement Outcome value. |
| RestoreDisplay     | cc.isr.VI.Tsp.K2600.DisplaySubsystem | RestoreDisplay | Restores the instrument display to its main mode |
| TriggerMeasurement | cc.isr.VI.Tsp.K2600.TriggerSubsystem | TBD | Sends a signal to trigger a measurement. |

## Deprecated Properties[](#){name=Deprecated_Properties}

Listed below are the properties that are not longer implemented in the new legacy device along with compatible properties from the updated TTM framework.

|Name|New Object|Name|Description|
|----|----|----|----|
| ContactResistances | cc.isr.VI.Tsp.K2600.ContactSubsystem or Firmware 2.4.x | TBD | Gets the contact resistances |
| FirmwareReleasedVersion | TBD | TBD | Holds the firmware version |
| Identity | TBD | TBD | Holds the instrument identity string |
| Outcome | Meter | MeasurementOutcome  | The merged outcome of the measurements. |

### Cold Resistance[](#){name=Cold_Resistance}

#### Added Properties[](#){name=Cold_Resistance_Added_Properties}

|Name|Description|
|----|----|
| OutcomeReading | A reading of the firmware outcome. Could be 'nil' |
| StatusReading  | A reading of the firmware status. Could be 'nil' |
| OkayReading  | A reading of the firmware `isOkay` method. Could be 'nil' |
| EntityName | Defined the TTM Firmware entity name, e.g., `_G.ttm.ir` that corresponds to the API entity, e.g., `InitialResistance`. |

#### Changed Methods[](#){name=Cold_Resistance_Changed_Methods}

|Name | Description|
|----|----|
| ParseReading | No longer parses the firmware outcome or determines pass/fail outcomes. |

#### Changed Properties[](#){name=Cold_Resistance_Changed_Properties}

|Name | Description|
|----|----|
| Resistance | Changed to nullable float. Set to value only if a reading is not empty or nil indicating that a measurement was made. |

#### Deprecated Methods[](#){name=Cold_Resistance_Deprecated_Properties}

|Name | Description|
|----|----|
| TBD | |

#### Deprecated Properties[](#){name=Cold_Resistance_Deprecated_Properties}

|Name|Description|
|----|----|
| DisplayFormat | The format string for constructing the resistance caption. |
| Outcome   | Deprecated. Added Outcome Reading, which corresponds the the Firmware Outcome. |
| ResistanceCaption | The formatted resistance reading. |
| Timestamp | Not used. |

### Thermal Transient [](#){name=Thermal_Transient}

#### Added Properties[](#){name=Thermal_Transient_Added_Properties}

|Name|Description|
|----|----|
| OutcomeReading | A reading of the firmware outcome. Could be 'nil' |
| StatusReading  | A reading of the firmware status. Could be 'nil' |
| OkayReading  | A reading of the firmware `isOkay` method. Could be 'nil' |
| EntityName | Defined the TTM Firmware entity name, e.g., `_G.ttm.tr` that corresponds to the API entity, e.g., `ThermalTransient`. |

#### Changed Methods[](#){name=Thermal_Transient_Changed_Methods}

|Name|Description|
|----|----|
| ParseReading | No longer parses the firmware outcome or determines pass/fail outcomes. |

#### Changed Properties[](#){name=Thermal_Transient_Changed_Properties}

|Name|Description|
|----|----|
| Voltage  | Changed to nullable float. Set to value only if a reading is not empty or nil indicating that a measurement was made. |

#### Deprecated Methods[](#){name=Thermal_Transient_Deprecated_Properties}

|Name|Description|
|----|----|
| ReadThermalTransientTrace | Reads the thermal transient trace from the instrument. |

#### Deprecated Properties[](#){name=Thermal_Transient_Deprecated_Properties}

|Name|Description|
|----|----|
| DisplayFormat | The format string for constructing the voltage change  caption. |
| Outcome       | Deprecated. Added Outcome Reading, which corresponds the the Firmware Outcome. |
| ResistanceDisplayFormat | The format string for constructing the resistance caption. |
| ResistanceCaption       | The formatted resistance reading. |
| Timestamp      | Not used. |
| VoltageCaption | The formatted voltage reading. |
| LastTrace      | The last trace as a string. |
| LastTimeSeries | The last trace as a collection of [time,voltage] points. |

### Shunt Resistance[](#){name=Shunt_Resistance}

#### Added Properties[](#){name=Shunt_Resistance_Added_Properties}

|Name|Description|
|----|----|
| TBD | |

#### Changed Methods[](#){name=Shunt_Resistance_Changed_Methods}

|Name|Description|
|----|----|
| ParseReading | No longer parses the firmware outcome or determines pass/fail outcomes. |

#### Changed Properties[](#){name=Shunt_Resistance_Changed_Properties}

|Name|Description|
|----|----|
| Resistance | Changed to nullable float. Set to value only if a reading is not empty indicating that a measurement was made. |

#### Deprecated Methods[](#){name=Shunt_Resistance_Deprecated_Properties}

|Name|Description|
|----|----|
| TBD | |

#### Deprecated Properties[](#){name=Shunt_Resistance_Deprecated_Properties}

|Name|Description|
|----|----|
| Outcome   | Deprecated. Added Outcome Reading, which corresponds the the Firmware Outcome. |
| Timestamp | Not used. |

