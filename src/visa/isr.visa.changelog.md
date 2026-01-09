# ISR Visa Changelog
All notable changes to these libraries will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)

[8.0.2.9504]: https://www.github.com/atecoder/dn.vi.ivi

Current IVI Visa Compatibility: 8.0.2.9391

## [8.0.2.9504] - 2026-01-08
- GAC Loader
  - Add full report in case a visa exception occurred.

## [8.0.2.9449] - 2025-11-14
- Transition .NET 9.0 projects to .NET 10 and VS2026.

## [8.0.2.9448] - 2025-11-13
- Add .NET 10.0 targets.

## [8.0.2.9435] - 2025-10-31
- Settings
  - Override CreateScribe.
  - Use Read Settings to initialize the settings instance.

## [8.0.2.9431] - 2025-10-27
- Settings
  - Move Scribe creation to Create Instance.
  - Define section name when construction a single settings scribe.
  - Use the scribe for reading the settings.
  - document creating the instance.
  - Copy settings if debug or not existing.
  - Use existence check from the scribe class.
  - Replace the abstract read and save settings with implementations using the created scribe.

## [8.0.2.9428] - 2025-10-24
- Remove Std MSTest project from solution.
- Add Exists to settings file and remove enabled and all from settigns files and classes.
- MS Test:
  - Remove reference to the cc.isr.Std.MSTest project.
  - Replace test site settings with Location Settings subclass of the JSon location settings base.
  - Use Settings base class from the JSon Application Settings library.
  - Use Settings Container base class from the JSon Application Settings library.

## [8.0.2.9421] - 2025-10-17
- Update packages:
  - .NET Test SDK to 4.0.1.
  - Microsoft Extensions to 9.0.10.
  - XUnit from 3.1.4 to 3.1.5.
  - Fluent Assertions from 8.6.0 to 8.7.1. 
  - Microsoft.Net.Test.SDK from 17.14.1 to 18.0.0.
  - BenchmarkDotNet from 0.14.0 to 0.15.4.
- Test projects
  - Use [TestMethod( DisplayName = "...']
  - Change [ClassCleanup( ClassCleanupBehavior.EndOfClass )] to [ClassCleanup]
  - Add parallelize to the assembly attributes.
  - change Assert...( ..., format, args ); to Assert...(... string.Format( System.Globalization.CultureInfo.CurrentCulture, format, args ) );
  - Use Assert.HasCount<T> in place of Assert.AreEquals( count, [T].Length ).
- directory.build.props
  - update packages.
  - point to the current repository.

## [8.0.2.9391] - 2025-09-17
- Update to Keysight IO Suite 21.2.207 and use KeySight Technologies VISA 18.6.6 package.

## [8.0.1.9371] - 2025-08-28
- Use preview in net standard classes.
- use isr.cc as company name in the Serilog settings generator.

## [8.0.1.9356] - 2025-08-13
- Update to Keysight IO Suite 21.1.209 and use KeySight Technologies VISA 18.5.73 package.

## [7.2.9220] - 2025-03-30
- Replace tabs with spaces.
- Remove trailing spaces 
- Reference main project revision in test and demo projects.
- Replace community SDK observable object with INotify Property Change in test site settings classes.
- Update change log.
- Update revisions to 9220.

## [7.2.9215] - 2025-03-25
- Add VISA Console project.

## [7.2.9213] - 2025-03-23
- Simple Read and Write Control: Add carriage returns.
- All: Update version build tools to user current date and time. 

## [7.2.9209] - 2025-03-19

## [7.2.9112] - 2024-12-12
Cloned from https://www.bitbucket.org/davidhary/dn.vi.git

## [7.2.9110] - 2024-12-11 Preview 202304
GAC Loader:
* Update IVI to 7.2.0

## [5.11.9040] - 2024-10-01 Preview 202304
* Downgrade the Kelary.IVI.Visa package to 5.11.3422.
* Add a solution for visa applications with only Kelary package dependencies.
* Add the IVI Visa Net test application from Kelary.
* Add GacLoader from Kelary to support .NET 5 and above.
* Add AutoGenerateBindingRedirects to all MSTest projects.
* Display the assembly version to see if setting the version properties in the project works. It does.
* Remove .Net standard 2.1 leaving leaving support for .net standard 2.0.
* override constant rules in the local editor config to permit pascal case constants.
* Remove setting the entry assembly.

## [5.11.8070] - 2022-02-04
* Targeting Visual Studio 2022, C## 10 and .NET 6.0.

## [5.11.7878] - 2021-07-26
* Add Interactive Visa IO projects and installer. 

## [5.11.7865] - 2021-07-14
* Ported from [vs.VI]

## [5.11.4652] - 2012-09-26
* Created [vs.Visa], which evolved to [vs.VI].

&copy; 2012 Integrated Scientific Resources, Inc. All rights reserved.

[vs.VI]: https://www.github.com/atecoder/dn.vi.ivi
[vs.Visa]: https://bitbucket.org/davidhary/vs.io.visa

