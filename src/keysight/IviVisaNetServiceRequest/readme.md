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

### .NET 10.0 output
```
Running under .NETCoreApp,Version=v10.0 runtime .NET 10.0.3

VISA.NET Shared Components version 8.0.0.0.
VISA Shared Components version 8.0.7331.0 detected.

Executing GlobalResourceManager.Find( "TCPIP?*INSTR"  )

IVI GlobalResourceManager ImplementationVersion:8.0.0.0 SpecificationVersion:7.4.0.0
Selected the VISA.NET Implementation by Keysight Technologies, Inc. version 18.6.0.0


tcpip0::192.168.0.150::inst0::instr Identification string:

Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6

Testing Service Request (SRQ) on OperationComplete...

Testing polling status byte for SRQ trial #1
        SRQ detected trial #1
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

Testing polling status byte for SRQ trial #2
        SRQ detected trial #2
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

Testing polling status byte for SRQ trial #3
        SRQ detected trial #3
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

Testing polling status byte for SRQ trial #4
        SRQ detected trial #4
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

Testing polling status byte for SRQ trial #5
        SRQ detected trial #5
                SerialPoll(): EventStatusRegister, RequestingService
                EventStatusRegister: 0x01

Testing asynchronous SRQ event callback
        Service Request Event Handler called #1
        Event arguments:
                CustomEventType: 1073684491
                      EventType: ServiceRequest
        Initial Status Byte: 0x60
        Final Status Byte: 0x60

***     SRQ did not call back by *OPC in allotted time trial #2
```

## Feedback

IviVisaNetServiceRequest is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi
