# Find Resources

## Category: VISA

## Description
This application shows how to use the IVI Global ResourceManager to
find all of the available resources on their system. The example
allows selecting several filters to narrow the list. The Public
property _ResourceName_ contains the resource name selected in Resource Tree View.

## Directions

1. Running the program opens the Available _Resource List form_.
1. Select a _Filter String_, e.g., `*?`;
1. The locarted resource will be displayed in the _Available Resource Found:_ box, such as:
   1. TCP/IP
       1. TCPIP0::192:168:0:132::inst0::INSTR
       1. TCPIP0::192:168:0:144::inst0::INSTR
       1. TCPIP0::192:168:0:50::inst0::INSTR

# Note
The list of resources consists of the resources that are explicitly defined in the VISA vendor's (e.g., Keysight) resource manager. The Keysight resource manager stores the resource information are stored in a locked SQLite local database. The National Instruments resource manager stores the resource information in an INI type file under the common program data folder (e.g., `c:\program data\national instruments`). The Rohde & Schwarz resources are stored in a CSV type file under the common program data folder.

## Specifications
* Language: C#  
* Required Software: IVI-VISA  
* Required Hardware: Any message-based device

## Outputs

# .NET 9.0
```
Running under .NETCoreApp,Version=v9.0.
VISA.NET Shared Components version 7.2.0.0.
VISA Shared Components version 7.2.7619.0 detected.
Loaded assembly "Keysight VISA.NET".
Executing GlobalResourceManager.Find( "TCPIP?*INSTR"  )
IVI GlobalResourceManager ImplementationVersion:7.2.0.0 SpecificationVersion:7.2.0.0
Selected the VISA.NET Implementation by Keysight Technologies, Inc. version 18.4.0.0
tcpip0::192.168.0.150::inst0::instr Identification string:
Keithley Instruments Inc., Model 2612A, 1214466, 2.2.6
```

## Feedback

Rohde.IdnQuery is released as open source under the MIT license.
Bug reports and contributions are welcome at the [VI Repository].

[VI Repository]: https://www.github.com/atecoder/ds.vi.ivi
