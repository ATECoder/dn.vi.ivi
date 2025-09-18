# Thermal Transient Meter&trade; Driver API Guide
# Thermal Transient Meter&trade; Driver API Guide

Application programming interface for the Thermal Transient Meter&trade;

Applicable TTM firmware version: 2.4.x
Applicable Virtual Instrument (VI) Framework version: 8.1.x

## Contents

- [Description](#Description)
- [Changes](#Changes)
  - [TTM_Library](#TTM_Library)
  - [Settings Files](#Settings_Files)
	- [Unit Tests](#Unit_Tests)
  - [Legacy Driver](#Legacy_Deriver)
  - [Meter Driver](#Meter_Driver)
    - [Code Breaking Chances](#Code_Breaking_Chances)
      - [Trace Messages](#Trace_Messages)
      - [Return Values](#Return_Values)
      - [Connect using Open Session](#Connect_using_Open_Session)
- [Using the API](#Using_the_API)
  - [Developer Source Code](#Developer_Source_Code
  - [IVI Visa](#IVI_Visa)
- [Testing the Library](#Testing_the_Library)
  - [Launch](#Launch)
  - [Connect](#Connect)
  - [Configure](#Configure)
  - [Manual Measurements](#Manual_Measurements)
  - [Triggered Measurements](#Triggered_Measurements)
    - [Stopping](#Stopping)
- [Using the ISR TTM Driver](#Using_the_ISR_TTM_Driver)
  - [Developer Source Code](#Developer_Source_Code)
    - [Compiling the developer source code](#Compiling_the_developer_source_code)
  - [Addressing the Driver](#Addressing_the_Driver)
    - [Recommended](#Recommended)
    - [Adding the source code](#Adding_the_source_code)
    - [Adding Reference to the DLL](#Adding_Reference_to_the_DLL)
  - [Instantiating the Interface](#Instantiating_the_Interface)
  - [Trace Message Handling](#Trace_Message_Handling)
  - [Connecting to the Instrument](#Connecting_to_the_Instrument)
  - [Configuring the Measurement](#Configuring_the_Measurement)
    - [The Part Element](#The_Part_Element)
    - [TTM Measurements](#TTM_Measurements)
    - [Shunt Measurement](#Shunt_Measurement)
  - [Making Measurements](#Making_Measurements)
    - [Initial Resistance](#Initial_Resistance)
    - [Thermal Transient](#Thermal_Transient)
    - [Final Resistance](#Final_Resistance)
    - [Shunt Resistance](#Shunt_Resistance)
    - [Updating Display Values](#Updating_Display_Values)
  - [Making Triggered Measurements](#Making_Triggered_Measurements)
    - [Priming](#Priming)
    - [Triggering](#Triggering)
    - [Reading](#Reading)
- [Attributions](#Attributions)

<a name="This_Guide"></a>
## This Guide

This document describes how to control the instrument remotely.

<a name="Drivers"></a>
## Drivers

The TTM Framework API consists of two drivers_

- A [Legacy Driver](#Legacy_Deriver), which requires minimal changes from the original TTM driver (rev. 2.3.x), and
- A [Meter Driver](#Meter_Deriver), which breaks compatibility with the original TTM driver (rev. 2.3.x).

For drivers are based on [.NET Framework] version 9.0 targeting the [Microsoft .NET Standard] 2.0, which means that the can be called from earlier releases of the [.NET Framework], such as 4.7.2 or 4.8.

<a name="Changes"></a>
## Changes

<a name="TTM_Library"></a>
### TTM_Library

Both the [Legacy Driver](#Legacy_Deriver) and [Meter Driver](#Meter_Deriver) use the [TTM Library Project] to access the TTM Instrument.

<a name="Settings_Files"></a>
### Settings Files

Both the [Legacy Driver](#Legacy_Deriver) and [Meter Driver](#Meter_Deriver) as well as the [TTM Library Project] use JSon settings files to define logging, timing settings, and default configuration settings for Cold Resistance and Thermal Transient configurations. Samples of these settings files are includes in the corresponding unit tests projects [Legacy Driver Unit Tests] and [Meter Driver Unit Tests].
 
<a name="Unit_Tests"></a>
#### Unit Tests

Both the [Legacy Driver](#Legacy_Deriver) and [Meter Driver](#Meter_Deriver) as well as the [TTM Library Project] come with unit tests project. In addition to providing sample JSon settings files, the corresponding unit tests projects [Legacy Driver Unit Tests] and [Meter Driver Unit Tests] are a good reference for understanding how to use the Legacy and the Meter API.

<a name="Legacy_Deriver"></a>
### Legacy Driver

The new legacy driver `cc.isr.VI.Tsp.K2600.Ttm.Legacy` implementation entails the following changes:
- Targets the [Microsoft .NET Standard] 2.0.
- Targets [IVI VISA] 8.0.2 thus requiring the installations compatible editions of the [Keysight IO Suite] (e.g., version 21.2.207).
- If is not clear at this time if [NI Visa] can be used because, starting with [Keysight IO Suite] 21.1.209, the application no longer compiles against a naked IVI Visa DLL.
- Uses a Message-Based session rather than a GPIB session to access the instrument. Thus, the driver can access a GPIB device by way of a USB-GPIB or TCP/IP-GPIB adapters.
- Uses a `cc` prefix to precede the `isr` namespace such as `cc.isr.Ttm.Device` instead of `isr.Ttm.Device`, which is commensurate with the registered `isr.cc` domain of Integrated Scientific Resources, Inc.
- Targets either the legacy TTM Firmware version 2.3.4009 or the Firmware version 2.4.9111 and above.
- Omits the Shunt features, which has not been implemented in production by users of the TTM Framework.
- Omits the Contact Check methods, which have not been implemented in production by users of the TTM Framework.
- Uses two objects to encapsulate the test configuration: Cold Resistance Config and Thermal Transient Config.
- Uses two objects encapsulate the test results: Cold Resistance and Thermal Transient.

<a name="Meter_Driver"></a>
### Meter Driver

The new meter driver `cc.isr.VI.Tsp.K2600.Ttm` implementation entails the following changes:
- Targets the [Microsoft .NET Standard] 2.0.
- Targets [IVI VISA] 8.0.2 thus requiring the installations compatible editions of the [Keysight IO Suite] (e.g., version 21.2.207).
- If is not clear at this time if [NI Visa] can be used because, starting with [Keysight IO Suite] 21.1.209, the application no longer compiles against a naked IVI Visa DLL.
- Uses a Message-Based session rather than a GPIB session to access the instrument. Thus, the driver can access a GPIB device by way of a USB-GPIB or TCP/IP-GPIB adapters.
- Uses a `cc` prefix to precede the `isr` namespace such as `cc.isr.Ttm.Device` instead of `isr.Ttm.Device`, which is commensurate with the registered `isr.cc` domain of Integrated Scientific Resources, Inc.
- Targets the new TTM Firmware version 2.4.9111 and above.
- Uses Contact Check methods that have been embedded in the new TTM Firmware.
- The meter must be reset and reconfigured when switching between shunt and thermal transient measurements.
- Uses three objects encapsulate the device under test: Cold Resistance, Thermal Transient and Shunt Resistance. These include both configuration settings and measurement results;
- Uses three objects encapsulate the thermal transient API: Meter Cold Resistance, Meter Thermal Transient and Thermal Transient Estimator. These include both the configuration and measurement commands and properties.
- Additional objects were defined to handle other subsystems such as Display and Status.
- The Source Measure, Display and Status subsystems are encapsulated in the Master Device object, which represents a Test Script Processor instrument such as the 2600 meter.

<a name="API_Changes"></a>
## API Changes

- The thermal transient elements include additional configuration settings.
- The API implements equality functions that allow to determine changes in configuration settings.
- The API includes reset and clear functions that clear or reset the execution state of the instrument to known values.
- Default execution state values are defined in the TTM Instrument namespace.
- The measure command clears the execution state.
- New API elements allow detecting changed configurations and applying only new configuration values. Note that before measuring shunt resistance after measuring thermal transient, all configuration values must be applied.

<a name="Legacy_Support"></a>
### Legacy Support

The TTM Firmware as described in the [TTM Firmware API Guide] is compatible with earlier versions of TTM Visual Basic drivers released before 2024 (e.g., 3.2.5367, 2.3.4077).

The firmware uses a persistent `MeterValues.legacyDriver` flag, which can be set from the instrument `MENU` or from [Legacy Driver](#Legacy_Deriver) API by the `legacyDriverSetter()` function call. When set to 1, the firmware is fully compatible with the legacy Visual Basic drivers. This option has been used to verify that the new firmware is compatible with the legacy drivers.

<a name="Code_Breaking_Chances"></a>
### Code Breaking Chances
 
<a name="Trace_Messages"></a>
#### Trace Messages

The driver uses new TraceMessage and TraceMessageEventArgs classes instead of the MessageEventArgs class. Specifically, the following changes are called for:

- Replace the event handles for the message event arguments with the following code (see _Console.vb_):

```
#Region " TRACE MESSAGES "

  ''' <summary> Event handler. Called by _meter for trace message available events. </summary>
  ''' <param name="sender"> The source of the event. </param>
  ''' <param name="e">    Trace message event information. </param>
  Private Sub _meter_TraceMessageAvailable(sender As Object, e As TraceMessageEventArgs) Handles _meter.TraceMessageAvailable
    Me.OnTraceMessageAvailable(e.TraceMessage)
  End Sub

  ''' <summary> Executes the trace message available action. </summary>
  ''' <param name="value"> The TraceMessage to process. </param>
  Public Overridable Sub OnTraceMessageAvailable(ByVal value As TraceMessage)
    If value IsNot Nothing Then
        My.Application.Log.WriteEntry(value.Details, value.TraceLevel)
    End If
  End Sub

  ''' <summary> Executes the trace message available action. </summary>
  ''' <param name="traceLevel"> The trace level. </param>
  ''' <param name="format">      Describes the format to use. </param>
  ''' <param name="args">     A variable-length parameters list containing arguments. </param>
  ''' <returns> The event arguments. </returns>
  Public Overridable Function OnTraceMessageAvailable(traceLevel As System.Diagnostics.TraceEventType, format As String, ParamArray args() As Object) As TraceMessage
    Dim e As New TraceMessage(traceLevel, format, args)
    Me.OnTraceMessageAvailable(New TraceMessage(traceLevel, format, args))
    Return e
  End Function

#End Region
```

<a name="Return_Values"></a>
#### Return Values

The new drivers use exceptions rather than return values to signal failures of operations. For example, the `OpenSession` command may raise the `ArgumentNullException` or `OperationFailedException` exceptions and could be handled as follows:

```
Try
  meter.TspDevice.OpenSession(resourceName)
Catch ex As ArgumentNullException
  MessageBox.Show("Connection Failed")
  Try
    meter.TspDevice.CloseSession()
  Finally
  End Try
Catch ex As OperationFailedException
  MessageBox.Show("Connection Failed")
  Try
    meter.TspDevice.CloseSession()
  Finally
  End Try
Catch ex As Exception
  MessageBox.Show("Connection Failed")
  Try
    meter.TspDevice.CloseSession()
  Finally
  End Try
End Try
```

<a name="Connect_using_Open_Session"></a>
#### Connect using the Open Session Method

See [Connecting to the Instrument](#Connecting).

<a name="Testing_the_Library"></a>
## Testing the Library

Use the [Legacy Driver Unit Tests] and [Meter Driver Unit Tests] unit test projects and the [TTM Console], which are included in the [TTM Solution], to test the library.

<a name="Using_the_API"></a>
## Using the API

<a name="Developer_Source_Code"></a>
### Developer Source Code

The open source code for the TTM Visual Studio Framework is included in the [TTM Repository]. The [TTM Firmware Private Repository] includes the closed source code for the TTM firmware. 

<a name="IVI_Visa"></a>
### IVI VISA

Adding the driver source code to your project is the preferred way because the code is less likely to conflict with dynamic link libraries that might be used by your main code. Specifically, the driver uses a particular version of the [IVI VISA], which might conflict with the VISA library installed on your computer. By adding the driver source code library to your project you will be compiling the driver to use the same VISA link library as your code. This may require to change the `Kelary.Ivi.Visa` package reference of the `cc.isr.Visa` and the `IviCompatibility` projects.

<a name="Compiling_the_developer_source_code"></a>
### Compiling the developer source code

Compile the the developer source code after updating the [IVI Visa](#IVI_VISA).

<a name="Addressing_the_Driver"></a>
### Addressing the Driver

The driver can be addressed by adding the driver source code to a Visual Studio project or by referencing the driver DLL.

<a name="Adding_the_source_code"></a>
### Adding the source code

You can add a copy of the source code to your solution. Otherwise, you could add the code from where the source code solution was installed on your computer.

Once you add the project to your solution, add a reference to the TTM
```
<ItemGroup>
  <ProjectReference Include="..\ttm\cc.isr.VI.Tsp.K2600.Ttm.csproj" />
</ItemGroup>
```

or legacy projects
```
<ItemGroup>
  <ProjectReference Include="..\Legacy\cc.isr.VI.Tsp.K2600.Ttm.Legacy.csproj" />
</ItemGroup>
```

to your project.

<a name="Adding_Reference_to_the_DLL"></a>
### Adding a Reference to the DLL

Alternatively, you could use the driver link library as is by adding a reference to the DLL:

- From the Add Reference dialog of the Visual Studio project, browse to the location of the compiled driver (see [Compiling the developer source code](#Compiling_the_developer_source_code)
- Select the driver DLL
  - `isr.VI.Tsp.K2600.Ttm.dll` or
  - `isr.VI.Tsp.K2600.Ttm.Legacy.dll`.
- This will add the `isr.VI.Tsp.K2600.Ttm` or `isr.VI.Tsp.K2600.Ttm.Legacy` reference to your project.

<a name="Instantiating_the_Interface"></a>
### Instantiating the Interface

Instantiate the ISR TTM Driver as follows:

```
Dim meter As New cc.isr.VI.Tsp.K2600.Ttm.Meter( Me.)
```

or

```
Dim legacyDevice As New cc.isr.VI.Tsp.K2600.Ttm.Legacy.LegacyDevice( Me.GetType(), ".Driver")
```

<a name="Trace_Message_Handling"></a>
### Trace Message Handling

The driver sends messages the TraceMessageEvent events (see [Trace Messages](#Trace_Messages). These events can be handled as follows:

```
#Region " TRACE MESSAGES "

  ''' <summary> Event handler. Called by _meter for trace message available events. </summary>
  ''' <param name="sender"> The source of the event. </param>
  ''' <param name="e">    Trace message event information. </param>
  Private Sub _meter_TraceMessageAvailable(sender As Object, e As TraceMessageEventArgs) Handles _meter.TraceMessageAvailable
    Me.OnTraceMessageAvailable(e.TraceMessage)
  End Sub

  ''' <summary> Executes the trace message available action. </summary>
  ''' <param name="value"> The TraceMessage to process. </param>
  Public Overridable Sub OnTraceMessageAvailable(ByVal value As TraceMessage)
    If value IsNot Nothing Then
        My.Application.Log.WriteEntry(value.Details, value.TraceLevel)
    End If
  End Sub

  ''' <summary> Executes the trace message available action. </summary>
  ''' <param name="traceLevel"> The trace level. </param>
  ''' <param name="format">      Describes the format to use. </param>
  ''' <param name="args">     A variable-length parameters list containing arguments. </param>
  ''' <returns> The event arguments. </returns>
  Public Overridable Function OnTraceMessageAvailable(traceLevel As System.Diagnostics.TraceEventType, format As String, ParamArray args() As Object) As TraceMessage
    Dim e As New TraceMessage(traceLevel, format, args)
    Me.OnTraceMessageAvailable(New TraceMessage(traceLevel, format, args))
    Return e
  End Function

#End Region
```

<a name="Connecting"></a>
### Connecting to the Instrument

Use the OpenSession method to Connect the meter:

```
Dim resourceName As String
resourceName = "TCPIP::192.168.0.150::inst0::INSTR"
Try
  Dim meter As New cc.isr.VI.Tsp.K2600.Ttm.Meter( Me.)
  meter.TspDevice.OpenSession(resourceName)
  
  Dim session As Pith.SessionBase session
  session = meter.TspDevice.Session

  session.ClearActiveState()
  session.ThrowDeviceExceptionIfError()

  session.ResetKnownState()
  session.ThrowDeviceExceptionIfError()

  session.ClearExecutionState()
  session.ThrowDeviceExceptionIfError()

  session.InitKnownState();

  meter.PresetKnownState();
  
  Dim deviceUnderTest As DeviceUnderTest
  deviceUnderTest = meter.ConfigInfo
  deviceUnderTest.ClearPartInfo()
  deviceUnderTest.ClearMeasurements()

  deviceUnderTest.PartNumber = "part number"
  deviceUnderTest.LotId = "lot id"
  deviceUnderTest.OperatorId = "operator id"
  meter.Part = deviceUnderTest
 
Catch ex As Exception
  MessageBox.Show("Connection Failed")
  Try
    meter?.TspDevice?.CloseSession()
  Finally
  End Try
End Try
```
or,
```
Dim resourceName As String
resourceName = "TCPIP::192.168.0.150::inst0::INSTR"
Try
  Dim legacyDevice As New cc.isr.VI.Tsp.K2600.Ttm.Legacy.LegacyDevice( Me.GetType(), ".Driver")
  legacyDevice.Connect( resourceName )
  legacyDevice.ResetAndClear()
  
  Dim session As Pith.SessionBase session
  session = meter.TspDevice.Session

  session.InitKnownState();

  meter.PresetKnownState();
  
  Dim deviceUnderTest As DeviceUnderTest
  deviceUnderTest = meter.ConfigInfo
  deviceUnderTest.ClearPartInfo()
  deviceUnderTest.ClearMeasurements()

  deviceUnderTest.PartNumber = "part number"
  deviceUnderTest.LotId = "lot id"
  deviceUnderTest.OperatorId = "operator id"
  meter.Part = deviceUnderTest
  
Catch ex As Exception
  MessageBox.Show("Connection Failed")
  Try
    legacyDevice?.meter?.TspDevice?.CloseSession()
  Finally
  End Try
End Try
```

This connects the meter to the device connected to the GPIB Board 0 at address 26.

<a name="Configuring_the_Measurement"></a>
### Configuring the Measurement

<a name="The_Part_Element"></a>
#### The Part Element

Configuration parameters and measurement values use both part and meter elements.

This allows separating between the part configuration and the actual meter settings. See the console class (_Console.vb)_ for more details.

The meter elements use the same class to hold their test parameters. Moreover, when setting the configuration parameters, the part elements are passed to the meter elements and are updated by the meter elements. Using binding, these updated values update the user interface with minimal coding as implemented in the new driver tester.

```
' Define a part to hold the current settings of the part or user interface.
Private WithEvents _Part As Part
_Part = meter.Part
```

<a name="TTM_Measurements"></a>
#### TTM Measurements

```
' Set the current to 10mA
_part.InitialResistance.CurrentLevel = 0.01
_part.FinalResistance.CurrentLevel = 0.01

' Set the voltage limit for measuring cold resistance to 0.1 volt.
_part.InitialResistance.VoltageLimit = 0.1
_part.FinalResistance.VoltageLimit = 0.1

' Set the low and high limits for the cold resistance.
_part.InitialResistance.HighLimit = 2.1
_part.InitialResistance.LowLimit = 1.9
_part.FinalResistance.HighLimit = 2.1
_part.FinalResistance.LowLimit = 1.9

' Set the delay time between the end of the thermal transient measurement and the onset of the final resistance measurement to 0.5 seconds.
_Part.ThermalTransient.PostThermalTransientDelay = 0.5

' Set the thermal transient current pulse level to 270mA.
_Part.ThermalTransient.CurrentLevel = 0.27

' Set the expected thermal transient voltage change during the thermal transient current pulse 0.1V
_Part.ThermalTransient.VoltageChange = 0.099

' Set the thermal transient high and low limits
_Part.ThermalTransient.HighLimit = 0.017
_Part.ThermalTransient.LowLimit = 0.006

Try
    Me.OnTraceMessageAvailable(TraceEventType.Verbose, "Configuring Thermal Transient;. ")
    If meter.IsDeviceOpen Then
        meter.ResetKnownState()
        meter.ClearExecutionState()
        meter.InitialResistance.Configure(Me._Part.InitialResistance)
        meter.FinalResistance.Configure(Me._Part.FinalResistance)
        meter.ThermalTransient.Configure(Me._Part.ThermalTransient)
    Else
        Me.OnTraceMessageAvailable(TraceEventType.Warning, "Meter not connected;. ")
    End If
Catch ex As Exception
    Me.OnTraceMessageAvailable(TraceEventType.Error, "Exception occurred configuring Thermal Transient;. Details: {0}", ex)
End Try
```

or,

```
Meter meter = legacyDevice.Meter;
Pith.SessionBase session = legacyDevice.meter.TspDevice.Session;

if ( !legacyDevice.ColdResistanceSourceOutputGetter( out SourceOutputOption initialSourceOutputOption ) )
{
	if ( !legacyDevice.ColdResistanceSourceOutputSetter( initialSourceOutputOption ) )
		throw new InvalidOperationException( "Failed setting the initial source output option." );
}

SourceOutputOption alternateOption = Ttm.MeterSubsystem.LegacyFirmware
	? SourceOutputOption.Current
	: initialSourceOutputOption == SourceOutputOption.Current
		? SourceOutputOption.Voltage
		: SourceOutputOption.Current;

float value = 0.01f;
legacyDevice.ColdResistanceConfig.CurrentLevel = value;
legacyDevice.ColdResistanceCurrentLevelSetter( value );
value = 0.02f;
legacyDevice.ColdResistanceConfig.VoltageLimit = value;
legacyDevice.ColdResistanceVoltageLimitSetter( value );

value = 2.6f;
legacyDevice.ColdResistanceConfig.HighLimit = value;
legacyDevice.ColdResistanceHighLimitSetter( value );

value = 1.8f;
legacyDevice.ColdResistanceConfig.LowLimit = value;
legacyDevice.ColdResistanceLowLimitSetter( value );

value = 0.01f;
legacyDevice.PostTransientDelayConfig = value;
legacyDevice.PostTransientDelaySetter( value );

value = 0.27f;
legacyDevice.ThermalTransientConfig.CurrentLevel = value;
legacyDevice.ThermalTransientCurrentLevelSetter( value );

value = 0.099f;
legacyDevice.ThermalTransientConfig.AllowedVoltageChange = value;
legacyDevice.ThermalTransientVoltageChangeSetter( value );

value = 0.19f;
legacyDevice.ThermalTransientConfig.HighLimit = value;
legacyDevice.ThermalTransientHighLimitSetter( value );

value = 0.006f;
legacyDevice.ThermalTransientConfig.LowLimit = value;
legacyDevice.ThermalTransientLowLimitSetter( value );
```

<a name="Shunt_Measurement"></a>
#### Shunt Measurement

As with the thermal transient measurement, the configuration parameters are set in the part element which is updated by the meter element and can be used with binding to update the user interface.

```
' Set the current to 1mA
_part.ShuntResistance.CurrentLevel = 0.001

' Set the current range to 10mA
_part.ShuntResistance.CurrentRange = 0.01

' Set the voltage limit for measuring resistance to 10 volt.
_part.ShuntResistance.VoltageLimit = 10

' Set the low and high limits for the resistance.
_part.ShuntResistance.HighLimit = 950
_part.ShuntResistance.LowLimit = 1050

Try
    Me.OnTraceMessageAvailable(TraceEventType.Verbose, "Configuring Shunt Resistance;. ")
    If meter.IsDeviceOpen Then
        meter.ResetKnownState()
        meter.ClearExecutionState()
        Me.OnTraceMessageAvailable(TraceEventType.Verbose, "Sending Shunt resistance configuration settings to the meter;. ")
        meter.ConfigureShuntResistance(Me._Part.ShuntResistance)
        Me.OnTraceMessageAvailable(TraceEventType.Verbose, "Shunt resistance measurement configured successfully;. ")
    Else
        Me.OnTraceMessageAvailable(TraceEventType.Warning, "Meter not connected;. ")
    End If
Catch ex As Exception
    Me.OnTraceMessageAvailable(TraceEventType.Error, "Exception occurred configuring shunt resistance;. Details: {0}", ex)
End Try
```

<a name="Making_Measurements"></a>
#### Making Measurements

<a name="Initial_Resistance"></a>
##### Initial Resistance

The following commands measure the initial resistance.

```
Try
    meter.TspDevice.DisplaySubsystem.ClearDisplayMeasurement()
    meter.InitialResistance.Measure()
Catch ex As Exception
    Me.OnTraceMessageAvailable(TraceEventType.Error, "Failed Measuring Initial Resistance;. Details: {0}", ex)
End Try
```

<a name="Thermal_Transient"></a>
##3## Thermal Transient

The following commands measure the thermal transient.

```
Try
    meter.ThermalTransient.Measure()
Catch ex As Exception
    Me.OnTraceMessageAvailable(TraceEventType.Error, "Failed Measuring Thermal Transient;. Details: {0}", ex)
End Try
```

<a name="Final_Resistance"></a>
##### Final Resistance

The following commands measure the final resistance.
```
Try
    meter.FinalResistance.Measure()
Catch ex As Exception
    Me.OnTraceMessageAvailable(TraceEventType.Error, "Failed Measuring Final Resistance;. Details: {0}", ex)
End Try
```

<a name="Shunt_Resistance"></a>
##### Shunt Resistance

The following commands measure the shunt resistance.

```
Try
    meter.MeasureShuntResistance(Me._ShuntResistance)
Catch ex As Exception
    Me.OnTraceMessageAvailable(TraceEventType.Error, "Failed Measuring Shunt Resistance;. Details: {0}", ex)
End Try
```

<a name="">Updating_Display_Values</a>
#### Updating Display Values

Display values could be updated by binding or by directly handling the property changed events of the elements as follows:

```
' These elements are part of the Form Designer definitions
Me._errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
Me._initialResistanceTextBox = New System.Windows.Forms.TextBox()
Me._FinalResistanceTextBox = New System.Windows.Forms.TextBox()
Me._ThermalTransientVoltageTextBox = New System.Windows.Forms.TextBox()

#Region " DISPLAY "

  ''' <summary> Displays the thermal transient. </summary>
  Private Sub setErrorProvider(ByVal textBox As TextBox, ByVal resistance As ResistanceMeasureBase)
    If (resistance.Outcome And MeasurementOutcomes.PartFailed) <> 0 Then
        Me._errorProvider.SetError(textBox, "Value out of range")
    ElseIf (resistance.Outcome And MeasurementOutcomes.MeasurementFailed) <> 0 Then
        Me._errorProvider.SetError(textBox, "Measurement failed")
    ElseIf (resistance.Outcome And MeasurementOutcomes.MeasurementNotMade) <> 0 Then
        Me._errorProvider.SetError(textBox, "Measurement not made")
    End If
  End Sub

  ''' <summary> Displays the resistance. </summary>
  Private Sub showResistance(ByVal textBox As TextBox, ByVal resistance As ResistanceMeasureBase)
    Me._initialResistanceTextBox.Text = resistance.ResistanceCaption
    Me.setErrorProvider(textBox, resistance)
  End Sub

  ''' <summary> Displays the thermal transient. </summary>
  Private Sub showThermalTransient(ByVal textBox As TextBox, ByVal resistance As ResistanceMeasureBase)
    textBox.Text = resistance.VoltageCaption
    Me.setErrorProvider(textBox, resistance)
  End Sub

#End Region

#Region " PART "

  ''' <summary> The Part. </summary>
  Private WithEvents _Part As Part

  ''' <summary> Gets or sets the part. </summary>
  ''' <value> The part. </value>
  Private Property Part As Part
    Get
        Return Me._Part
    End Get
    Set(value As Part)
        Me._Part = value
        If Me._Part IsNot Nothing Then
          Me._InitialResistance = Me._Part.InitialResistance
          Me._FinalResistance = Me._Part.FinalResistance
          Me._ShuntResistance = Me._Part.ShuntResistance
          Me._ThermalTransient = Me._Part.ThermalTransient
        End If
    End Set
  End Property

#End Region

#Region " PART: INITIAL RESISTANCE "

  ''' <summary> The Part Initial Resistance. </summary>
  Private WithEvents _InitialResistance As ColdResistance

  ''' <summary> Event handler. Called by _InitialResistance for property changed events. </summary>
  ''' <param name="sender"> The source of the event. </param>
  ''' <param name="e">    Property changed event information. </param>
  <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
  Private Sub _InitialResistance_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _InitialResistance.PropertyChanged
    Try
        If sender IsNot Nothing AndAlso
          e IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(e.PropertyName) Then
          Dim resistance As ColdResistance = CType(sender, ColdResistance)
          Select Case e.PropertyName
            Case "MeasurementAvailable"
                If resistance.MeasurementAvailable Then
                  Me.showResistance(Me._initialResistanceTextBox, resistance)
                End If
          End Select
        End If
    Catch ex As Exception
        Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling property '{0}'. Details: {1}.",
                 e.PropertyName, ex.Message)
    End Try
  End Sub

#End Region

#Region " PART: FINAL RESISTANCE "

  ''' <summary> The Part Final Resistance. </summary>
  Private WithEvents _FinalResistance As ColdResistance

  ''' <summary> Event handler. Called by _FinalResistance for property changed events. </summary>
  ''' <param name="sender"> The source of the event. </param>
  ''' <param name="e">    Property changed event information. </param>
  <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
  Private Sub _FinalResistance_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _FinalResistance.PropertyChanged
    Try
        If sender IsNot Nothing AndAlso
          e IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(e.PropertyName) Then
          Dim resistance As ColdResistance = CType(sender, ColdResistance)
          Select Case e.PropertyName
            Case "MeasurementAvailable"
                If resistance.MeasurementAvailable Then
                  Me.showResistance(Me._finalResistanceTextBox, resistance)
                End If
          End Select
        End If
    Catch ex As Exception
        Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling property '{0}'. Details: {1}.",
                 e.PropertyName, ex.Message)
    End Try
  End Sub

#End Region

#Region " PART: SHUNT RESISTANCE "

  ''' <summary> The Part Shunt Resistance. </summary>
  Private WithEvents _ShuntResistance As ShuntResistance

  ''' <summary> Event handler. Called by _ShuntResistance for property changed events. </summary>
  ''' <param name="sender"> The source of the event. </param>
  ''' <param name="e">    Property changed event information. </param>
  <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
  Private Sub _ShuntResistance_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _ShuntResistance.PropertyChanged
    Try
        If sender IsNot Nothing AndAlso
          e IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(e.PropertyName) Then
          Dim resistance As ShuntResistance = CType(sender, ShuntResistance)
          Select Case e.PropertyName
            Case "MeasurementAvailable"
                If resistance.MeasurementAvailable Then
                  Me.showResistance(Me._ShuntResistanceTextBox, resistance)
                End If
          End Select
        End If
    Catch ex As Exception
        Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling property '{0}'. Details: {1}.",
                 e.PropertyName, ex.Message)
    End Try
  End Sub

#End Region

#Region " PART: THERMAL TRANSIENT "

  Private WithEvents _ThermalTransient As ThermalTransient
  ''' <summary> Event handler. Called by _ThermalTransient for property changed events. </summary>
  ''' <param name="sender"> The source of the event. </param>
  ''' <param name="e">    Property changed event information. </param>
  <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")>
  Private Sub _ThermalTransient_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _ThermalTransient.PropertyChanged
    Try
        If sender IsNot Nothing AndAlso
          e IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(e.PropertyName) Then
          Dim transient As ThermalTransient = CType(sender, ThermalTransient)
          Select Case e.PropertyName
            Case "MeasurementAvailable"
                If transient.MeasurementAvailable Then
                  Me.showThermalTransient(Me._thermalTransientVoltageTextBox, transient)
                End If
          End Select
        End If
    Catch ex As Exception
        Debug.Assert(Not Debugger.IsAttached, "Exception handling property", "Exception handling property '{0}'. Details: {1}.",
                 e.PropertyName, ex.Message)
    End Try
  End Sub
#End Region
```

<a name="Making_Triggered_Measurements"></a>
### Making Triggered Measurements

<a name="Priming"></a>
### Priming

The following command prepares the instrument to wait for an external trigger.

```
Try
  meter.PrepareForTrigger()
  Me.OnTraceMessageAvailable(TraceEventType.Verbose, "Monitoring instrument for measurements;. ")
Catch ex As Exception
  Me.OnTraceMessageAvailable(TraceEventType.Warning, "Failed preparing instrument for waiting for trigger;. ")
End Try
```

<a name="Triggering"></a>
### Triggering

The instrument provides full hand shake for controlling the measurement by way of thedigital I/O:

- Trigger a measurement by taking line 1 of the digital I/O port from low to high.
- The instrument responds by taking line 2 high. Lines 3 go low as well as the pass/fail lines 4 through 7.
- Line 3 goes high when the measurement completes at which time the measurement readings are available on lines 4 through 7.

See the TTM Instrument User's Guide for more information on the digital I/O

<a name="Reading"></a>
### Reading

The following commands wait for measurement completion and then reads the measured data. The property changed handlers (see [Updating Display Values](#Updating_Display_Values)) will update the values.

```
Dim done as Boolean = False
Do Until done
  If meter.IsMeasurementCompleted() Then
    done = True
    Try
      meter.ReadMeasurements()
    Catch Ex as Exception
      Me.OnTraceMessageAvailable(TraceEventType.Warning, "Failed reading;. ")
    End Try
Loop
```

<a name="Attributions"></a>
### Attributions

Last Updated 2024-10-09

&copy; 2011 by Integrated Scientific Resources, Inc.  

This information is provided "AS IS" WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.

Licensed under [The Fair End User] and [MIT] Licenses.

Unless required by applicable law or agreed to in writing, this software is provided "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

Source code is hosted on [GitHub]

[The Fair End User]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/Fair%20End%20User%20License.html
[MIT]: http://opensource.org/licenses/MIT
[GitHub]: https://www.github.com/ATECoder
[.NET Framework]: https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet-framework
[Microsoft .NET Standard]: https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-1-0
[IVI VISA]: https://www.ivi.org
[Keysight IO Suite]: https://www.keysight.com/en/pd-1985909/io-libraries-suite
[NI Visa]: http://ftp.ni.com/support/softlib/visa/VISA%20Run-Time%20Engine
[TTM Framework Guide]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/TTM%20Framework%20Guide.html
[TTM Firmware API Guide]: https://github.com/ATECoder/dn.vi.ivi.git/docs/ttm/TTM%20Firmware%20API%20Guide.html
[Legacy Driver Unit Tests]: https://github.com/ATECoder/dn.vi.ivi.git/src/vi/k2600.ttm/legacy.mstest
[Meter Driver Unit Tests]: https://github.com/ATECoder/dn.vi.ivi.git/src/vi/k2600.ttm/mstest
[TTM Library Project]: https://github.com/ATECoder/dn.vi.ivi.git/src/vi/k2600.ttm/ttm
[TTM Solution]: https://github.com/ATECoder/dn.vi.ivi.git/src/cc.isr.vi.2600.ttm.sln
[TTM Console]: https://github.com/ATECoder/dn.vi.ivi.git/src/vi/k2600.ttm/console
[TTM Repository]: https://github.com/ATECoder/dn.vi.ivi.git
[TTM Firmware Private Repository]: https://github.com/ATECoder/dn.ttmware.git