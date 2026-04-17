# Ivi Visa Net Service Request

## About

This project aims to demonstrates service request handling using polling or call backs for the IVI.Visa message based session implementation with the Keysight I/O suite. The project is aimed to be compatible with any instrument supporting IEE488.2 SCPI. It was tested with the Keithley 2612A source meter.

The project uses the Keysight `KtE4990_Cs_StatusSRQ.csproj` project as a template but replaces the IVI Device methods with IVI.Visa methods.

## Failure report

The service request call back occurs only once (see the .NET 10.0 output below). 

Note that the `KtE4990_Cs_StatusSRQ.csproj` tests only a single instance of the service request handling. We could not use this project for our testing at this time because out 4990 instrument was delivered to our customer.

## VISA

- [Keysight VISA.NET 21.2.207](https://www.keysight.com/us/en/software/keysight-io-suite.html)

## Required Packages
- [Ivi.Visa 8.0.2](https://www.nuget.org/packages/Ivi.Visa/)
- [KeysightTechnologies.Visa 18.6.6](https://www.nuget.org/packages/KeysightTechnologies.Visa/)

## Directions

1. Edit the command line using the instrument TcpIP resource name;
1. Turn on the instrument;
1. Run the program from the IDE or the command window.
1. Command: IviVisaNetServiceRequest tcpip0::192.168.0.150::inst0::instr 

## Output under the Keysight IO Suite 21.2.207

### .NET 10.0 output IviFoundation.Visa Version 8.0.2
```
Running under .NETCoreApp,Version=v10.0 runtime .NET 10.0.5

VISA.NET Shared Components assembly:
        Full name: Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1
        File name: Ivi.Visa.dll
        Location:  C:\my\lib\vs\vi\vi\src\keysight\IviVisaNetServiceRequest\bin\Debug\net10.0
        Version:   8.0.0.0
        Product:   8.0.7803.0
        File:      8.0.7803.0

VISA Config Manager product version 8.0.7331.0

Executing GlobalResourceManager.Find( "TCPIP?*INSTR"  )

IVI GlobalResourceManager ImplementationVersion:8.0.0.0 SpecificationVersion:7.4.0.0
Selected the VISA.NET Implementation by Keysight Technologies, Inc. version 18.6.0.0


tcpip0::192.168.0.150::inst0::instr Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6

Configuring Service Request (SRQ) on operation completion

Testing Service Request (SRQ) on OperationComplete
        Standard event status enable (ESE): 1
        Service request enable (SRE): 32

Polling status byte for *OPC

        SRQ detected on trial #1
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #2
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #3
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #4
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #5
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

Calling back asynchronous SRQ event for *OPC
        Service Request Event Handler called #1
        Event arguments:
                CustomEventType: 1073684491
                      EventType: ServiceRequest
        Initial Status Byte: 0x60
        Final Status Byte: 0x60

***     SRQ did not call back by *OPC in allotted time on trial #2
```

or, wait on events
```
Running under .NETCoreApp,Version=v10.0 runtime .NET 10.0.5

VISA.NET Shared Components assembly:
        Full name: Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1
        File name: Ivi.Visa.dll
        Location:  C:\my\lib\vs\vi\vi\src\keysight\IviVisaNetServiceRequest\bin\Debug\net10.0
        Version:   8.0.0.0
        Product:   8.0.7803.0
        File:      8.0.7803.0

VISA Config Manager product version 8.0.7331.0

Executing GlobalResourceManager.Find( "TCPIP?*INSTR"  )

IVI GlobalResourceManager ImplementationVersion:8.0.0.0 SpecificationVersion:7.4.0.0
Selected the VISA.NET Implementation by Keysight Technologies, Inc. version 18.6.0.0


TCPIP0::192.168.0.150::inst0::INSTR Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6

Configuring Service Request (SRQ) on operation completion

Testing Service Request (SRQ) on OperationComplete
        Standard event status enable (ESE): 1
        Service request enable (SRE): 32

Polling status byte for *OPC

        SRQ detected on trial #1
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #2
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #3
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #4
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #5
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

Waiting on SRQ event for *OPC

        Service Request Event #1 occurred after 1 ms
        Event arguments:
                CustomEventType: 1073684491
                      EventType: ServiceRequest

***     SRQ by *OPC timed out after 126 ms on trial #2

```

### .NET 10.0 output KeysightTechnologies.Visa Version 18.6.6
```
Running under .NETCoreApp,Version=v10.0 runtime .NET 10.0.5

VISA.NET Shared Components assembly:
        Full name: Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1
        File name: Ivi.Visa.dll
        Location:  C:\my\lib\vs\vi\vi\src\keysight\IviVisaNetServiceRequest\bin\Debug\net10.0
        Version:   8.0.0.0
        Product:   8.0.7803.0
        File:      8.0.7803.0

VISA Config Manager product version 8.0.7331.0

Executing GlobalResourceManager.Find( "TCPIP?*INSTR"  )

IVI GlobalResourceManager ImplementationVersion:8.0.0.0 SpecificationVersion:7.4.0.0
Selected the VISA.NET Implementation by Keysight Technologies, Inc. version 18.6.0.0


TCPIP0::192.168.0.150::inst0::INSTR Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6

Configuring Service Request (SRQ) on operation completion

Testing Service Request (SRQ) on OperationComplete
        Standard event status enable (ESE): 1
        Service request enable (SRE): 32

Polling status byte for *OPC

        SRQ detected on trial #1
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #2
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #3
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #4
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #5
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

Calling back asynchronous SRQ event for *OPC
        Service Request Event Handler called #1
        Event arguments:
                CustomEventType: 1073684491
                      EventType: ServiceRequest
        Initial Status Byte: 0x60
        Final Status Byte: 0x60

***     SRQ did not call back by *OPC in allotted time on trial #2
```

Waiting on events

```
Running under .NETCoreApp,Version=v10.0 runtime .NET 10.0.5


VISA.NET Shared Components assembly:
        Full name: Ivi.Visa, Version=8.0.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1
        File name: Ivi.Visa.dll
        Location:  C:\my\lib\vs\vi\vi\src\keysight\IviVisaNetServiceRequest\bin\Debug\net10.0
        Version:   8.0.0.0
        Product:   8.0.7803.0
        File:      8.0.7803.0

VISA Config Manager product version 8.0.7331.0

Executing GlobalResourceManager.Find( "TCPIP?*INSTR"  )

IVI GlobalResourceManager ImplementationVersion:8.0.0.0 SpecificationVersion:7.4.0.0
Selected the VISA.NET Implementation by Keysight Technologies, Inc. version 18.6.0.0


TCPIP0::192.168.0.150::inst0::INSTR Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6

Configuring Service Request (SRQ) on operation completion

Testing Service Request (SRQ) on OperationComplete
        Standard event status enable (ESE): 1
        Service request enable (SRE): 32

Polling status byte for *OPC

        SRQ detected on trial #1
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #2
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #3
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #4
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

        SRQ detected on trial #5
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

Waiting on SRQ event for *OPC

        Service Request Event #1 occurred after 1 ms
        Event arguments:
                CustomEventType: 1073684491
                      EventType: ServiceRequest

***     SRQ by *OPC timed out after 129 ms on trial #2
```

## Feedback

IviVisaNetServiceRequest is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi
