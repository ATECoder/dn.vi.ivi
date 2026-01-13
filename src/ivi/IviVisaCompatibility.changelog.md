# IVI Visa Compatibility Changelog
All notable changes to these libraries will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)

[8.0.2.9509]: https://www.github.com/atecoder/dn.vi.ivi

Current IVI Visa Compatibility: 8.0.2.9509

## [8.0.2.9509] - 2029-01-13
- IviVisaNetSample
  - Split off the gac assembly loader from the assembly loader class.
- IviVisaCompatibility
  - Copy Gac Loader from IviVisaNetSampe.
  - Set package to the IviFoundation.Visa
- Ivi Apps
  - Update program class,

## [8.0.2.9508] - 2026-01-12
- IviVisaNetSample
  - Successfully use IviFoundation.Visa package 8.0.2 by allowing it to load it's associated Keysight assembly.

## [8.0.2.9448] - 2025-11-13
- Add .NET 10.0 targets.

## [8.0.2.9391] - 2025-09-17
- Update to Keysight IO Suite 21.2.207 and use KeySight Technologies VISA 18.6.6 package.

## [8.0.1.9371] - 2025-08-28
- Use preview in net standard classes.
- use isr.cc as company name in the Serilog settings generator.

## [8.0.1.9362] - 2025-08-19
- Remove unnecessary supression of NuGet NU1701 and RAZORSDK1006 warnings.

## [8.0.1.9356] - 2025-08-13
- Update to Keysight IO Suite 21.1.209 and use KeySight Technologies VISA 18.5.73 package.

## [7.2.9220] - 2025-03-30
- Replace tabs with spaces.s
- Remove trailing spaces 
- Update change log.
- Update revisions to 9220.

## [7.2.9213] - 2025-03-23
* Kelary: Update version build tools to user current date and time.
* Visa: Update version build tools to user current date and time. 

## [7.2.9124] - 2024-12-24
* Kelary: Add IVI Visa Compatibility Test.
* Kelary: Sign the IVI Visa Compatibility library.
* Docs: Update compatibility user guide.
* Directory Build: Point to the git hub origin repository.

## [7.2.9119] - 2024-12-19
* IVI Visa Compatibility Demo: Add command file.

## [7.2.9112] - 2024-12-12
Cloned from https://www.bitbucket.org/davidhary/dn.vi.git

## [7.2.9110] - 2024-12-11 Preview 202304
GAC Loader:
* Update IVI to 7.2.0
* Update GAC loaders and demo revision to 7.2

## [5.11.9040] - 2024-10-01 Preview 202304
* Downgrade the Kelary.IVI.Visa package to 5.11.3422.
* Add a solution for visa applications with only Kelary package dependencies.
* Add the IVI Visa Net test application from Kelary.
* Add GacLoader from Kelary to support .NET 5 and above.
* Change gacloader to comply with earlier .NET frameworks.
* Display the assembly version to see if setting the version properties in the project works. It does.
* Add Ivi Visa Compatibility and Demo projects to demonstrate accessiong IVI Visa fron .NET stadnard 2.0.
* Remove .Net standard 2.1 leaving leaving support for .net standard 2.0.
* Add gac loader to the foundation project.

&copy; 2012 Integrated Scientific Resources, Inc. All rights reserved.

[vs.VI]: https://www.github.com/atecoder/dn.vi.ivi
[vs.Visa]: https://bitbucket.org/davidhary/vs.io.visa
