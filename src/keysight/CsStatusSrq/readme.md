# `CsStatusSrq` Project notes

This project is based on a `KtE4990_Cs_StatusSRQ` source code shared by Keysight on 4/20/26.

## Main Issues

The source code and information shared with Keysight, is, unfortunately, only of very limited value as follows:

## The Keysight project was based on IVI Visa 5.6 release in 2015.
```
    <Reference Include="Ivi.Visa, Version=5.6.0.0, Culture=neutral, PublicKeyToken=a128c98f1d7717c1, processorArchitecture=MSIL">
      <SpecificVersion>False</SpecificVersion>
      <HintPath>..\..\..\..\..\Program Files\IVI Foundation\VISA\Microsoft.NET\Framework64\v2.0.50727\VISA.NET Shared Components 5.6.0\Ivi.Visa.dll</HintPath>
    </Reference>
```

The problem I reported expresses itself with Ivi.Visa 8.0.2. Therefore, stating that code that worked with IVI Visa 5.6 is of little import on the issue I reported.

## The session construction code specified no resource name.
```
var vi = GlobalResourceManager.Open("TCPIP::ip or hostname::INSTR") as IMessageBasedSession;
```

The main method included no resource name. Was this an INST resource or a HISLIP.

## No output was provided

Seeing that the application engineering did not include the program output and given that the project references an IVI Visa of way back when (we are at 8.0.2 versus 5.6) gives me little confidence that the current Keysight VISA implementation works.

## I still getting a timeout exception on the second service request call

I modified the source code to target .NET 10 (see attached). 

Here is the output I am getting:

```
IDN: Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
ESE: 1
SRE: 32
Awaiting SRQ #1
Status byte: &h70, IDN: Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
IDN: Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
ESE: 1
SRE: 32
Awaiting SRQ #2
Exception while waiting for SRQ #2:
Ivi.Visa.IOTimeoutException: The IO operation timed out.
 ---> Ivi.Visa.NativeVisaException: VISA status code = ERROR_TMO (0xBFFF0015 hex, -1073807339 decimal)
VI_ERROR_TMO: A timeout occurred
   --- End of inner exception stack trace ---
   at Keysight.Visa.NativeWrapper.CheckNativeVisaStatus(UInt32 session, Int32 status)
   at Keysight.Visa.NativeWrapper.WaitOnEvent(UInt32 vi, EventType eventType, Int32 timeoutMilliseconds, EventQueueStatus& queueStatus)
   at Keysight.Visa.VisaSession.Ivi.Visa.INativeVisaSession.WaitOnEvent(Int32 eventType, Int32 timeoutMilliseconds, EventQueueStatus& eventQueueStatus)
   at Keysight.Visa.VisaSession.WaitOnEvent(EventType eventType, Int32 timeoutMilliseconds)
   at Program.<Main>$(String[] args) in C:\my\lib\vs\vi\vi\src\keysight\CsStatusSrq\Program.cs:line 91
```

# The project also has a few aesthetic if minor flaws:

## It targets .NET 4.61

```
    <TargetFrameworkVersion>v4.6.1</TargetFrameworkVersion>
```

I believe that, presently, Microsoft recommends targeting to .NET 4.8 or .NET 6.0 and above. The projects I shared with Keysight targeted .NET 10.

## The Keep Alive attributes has not been fixed

```
//((INativeVisaSession)vi).SetAttributeBoolean(NativeVisaAttribute.TcpKeepAlive, true);
// Set TCP Keep Alive
//if (vi.ResourceName.StartsWith("TCPIP"))
//{
//    ((INativeVisaSession)vi).SetAttributeBoolean(NativeVisaAttribute.TcpKeepAlive, true);
//}
```

