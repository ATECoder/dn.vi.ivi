# Changelog
All notable changes to these libraries will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)

## [8.1.9215] - 2025-03-25
- Add device win controls unit tests.

## [8.1.9208] - 2025-03-18
- Update K2600.Ttm.

## [8.1.9181] - 2025-02-19
- Update K2600.Ttm.

## [8.1.9175] - 2025-02-13
- Update K2600.Ttm.

## [8.1.9172] - 2025-02-10
- Update versions to 9174.

## [8.1.9171] - 2025-02-08
- Version properties:
  - Set release numbers to 103.
- Json Settings:
  - Set JSON maximum voltage change to 0.099.
- Meter:
  - Display the current version when starting the meter.
- TTM MS Test
  - Use TTM Element values to unify test methods for the IR, FR, TR and EST tests.
  - Test both voltage and current sources for the legacy driver.
  - Split framework syntax tests to smaller methods.
  - Fix bugs in unit tests.
  - Update tests of reading to match the new firmware.
- Legacy MS Test:
  - Add timeout to trigger test.

## [8.1.9161] - 2025-01-30
Device Tsp: Fix parsing nullable integer by adding exponent and period.
Legacy driver tester: Fix reading the outcome.

## [8.1.9154] - 2025-01-23
- K2600 TTM
  - Add Contact check subsystem tests.

## [8.1.9154] - 2025-01-23
- TSP
  - Fix setting of the source meter reference name.
- K2600
  - Add Contact check subsystem tests.

## [8.1.9124] - 2024-12-24
* Kelary: Add IVI Visa Compatibility Test.
* Kelary: Sign the IVI Visa Compatibility library.
* Docs: Update compatibility user guide.
* Directory Build: Point to the git hub origin repository.

## [8.1.9119] - 2024-12-19
* IVI Visa Compatibility Demo: Add command file.

## [8.1.9112] - 2024-12-12
Cloned from https://www.bitbucket.org/davidhary/dn.vi.git

## [8.1.9111] - 2024-12-11
TTMWare: 
* Access subsystem: use salt constant.
* Add TTM firmware upgrade user guides.
Loader:
* remove binary scripts and salt files.
* Increment release versions.

## [8.1.9110] - 2024-12-11 Preview 202304
Resource:
* Fix resource name caption. 
GAC Loader:
* Update IVI to 7.2.0
* Update GAC loaders and demo revision to 7.2
TTMWare Loader: 
* Add the mesa conditional compilation constant.
* Fix bug in reporting the deployed files folder.
* Analyze done. add load, save, register etc. 
* Conditionally add access function to the deploy folder.
TTM Ware:
* Add derigistration method.

## [8.1.9106] - 2024-12-06 Preview 202304
Resource:
* Session Base: Add timeout to read and throw if operation incomplete.

## [8.1.9100] - 2024-11-30 Preview 202304
* Increment version to 9100.

## [8.1.9085] - 2024-11-15 Preview 202304
* Visa Session Base:
  * Move subsystem only code to the specific reset, clear, init and preset methods.
* TTM Meter:
  * Implement preset known state.
  * Move instrument default and reading configuration from define reset known state to define preset known state.

## [8.1.9084] - 2024-11-14 Preview 202304
* Increment version to 9084.

## [8.1.9083] - 2024-11-13 Preview 202304
* Update packages.
* Upgrade Forms libraries and Test and Demo apps to .Net 9.0.
* Increment version to 9083.

## [8.1.9082] - 2024-11-12 Preview 202304
* TTM: Move legacy driver and unit tests to legacy driver projects.

## [8.1.9080] - 2024-11-09 Preview 202304
* Increment TTM Ware revision to 2.4.9080.
* TTM:
  * Add legacy driver code.
  * Add TTM unit tests: Legacy driver, driver and meter..
* TTM Firmware:
  * update change log.

## [8.1.9063] - 2024-10-24 Preview 202304
* Move resource settings from the session to the test settings.
* Report the revision of the project under test.

## [8.1.9054] - 2024-10-14 Preview 202304
* See TTM Ware project for changes
* Session: Add operation completion query method.

## [8.1.9040] - 2024-10-01 Preview 202304
* Downgrade the Kelary.IVI.Visa package to 5.11.3422.
* Add a solution for visa applications with only Kelary package dependencies.
* Add the IVI Visa Net test application from Kelary.
* Update the Rohde ID Query application to using the Kelary package.
* Add GacLoader from Kelary to support .NET 5 and above.
* Apply code analysis and test all stand along applications.
* Change gacloader to comply with earlier .NET frameworks.
* Add AutoGenerateBindingRedirects to all MSTest projects.
* Display the assembly version to see if setting the version properties in the project works. It does.
* Rohde Query: Add NET 4.7.2 framework; add tests for resource name casing: ::inst0:: must be lower case.
* Add Ivi Visa Compatibility and Demo projects to demonstrate accessiong IVI Visa fron .NET stadnard 2.0.
* Remove .Net standard 2.1 leaving leaving support for .net standard 2.0.
* Add gac loader to the foundation project.
* override constant rules in the local editor config to permit pascal case constants.
* Add developer notes.
* Apply code analysis rules.
* Session base: throw exceptions on empty query or write commands.
* Use MS Test and NET21 projects.
* Load VISA implementations when creating the foundation session factory.
* Passed all device pith tests.
* Remove setting the entry assembly.
* Add session logger.
* move TSP syntax to the Pith project.
* Move standard register code and wait completion to Session Base class.
* Move device error handling to the session base class.
* add support for saving scripts as binary on the controller node.
* remove trim when reading the salt file.

* TSP Script and 2600 Firmware major updates:
  * Script project was updated. 
  * Firmware project was updated and unit tests.

## [8.1.8944] - 2024-06-27 Preview 202304
* Interim commit.
* Update projects to .NET 8.
* Create a solution for 2600 TTM.
* Apply code analysis rules.
* Change MSTest SDK to 3.4.3.
* Use local trace loggers for tests under the MSTest namespace.

## [8.1.8535] - 2023-05-15 Preview 202304
* Use cc.isr.Json.AppSettings.ViewModels project for settings I/O.

## [8.1.8518] - 2023-04-28 Preview 202304
* Split README.MD to attribution, cloning, open-source and read me files.
* Add code of conduct, contribution and security documents.
* Increment version.

## [8.0.8189] - 2022-06-03
* Use Value Tuples to implement GetHashCode().

## [8.0.8126] - 2022-04-01
* Tests pass in project and package reference modes.

## [8.0.8123] - 2022-03-29
* Use the ?. operator, without making a copy of the delegate,
to check if a delegate is non-null and invoke it in a thread-safe way.
* Use the new Json application settings base class.
* Use logging trace log win forms.
* Initialize settings to ensure that settings get created in cases
where the application context settings files is not imported from the package.
* Pack.
* Caveat: the RequestingServiceShouldBeEnabledBySession unit test is 
failing. It works on 7510 SCPI but not TSP. Confirmed with Keithley,
which has issued a defect report.

## [8.0.8070] - 2022-02-04
* Targeting Visual Studio 2022, C## 10 and .NET 6.0.
* Update NuGet packages.
* Remove unused references. 
* Update build version.
* Display version file when updating build version.

## [8.0.7986] - 2021-11-11
* Defaults to using the global resource finder. 
* Uses .NET 4.8.

## [8.0.7909] - 2021-08-27
* Add description to settings. 

## [8.0.7878] - 2021-07-26
* Add Interactive Visa IO projects and installer. 

## [7.3.7865] - 2021-07-14
* Ported from [vs.VI]

## [1.0.4652] - 2012-09-26
* Created [vs.Visa], which evolved to [vs.VI].

&copy; 2012 Integrated Scientific Resources, Inc. All rights reserved.

[8.1.9215]: https://www.github.com/atecoder/dn.vi.ivi
[vs.VI]: https://www.github.com/atecoder/dn.vi.ivi
[vs.Visa]: https://bitbucket.org/davidhary/vs.io.visa
