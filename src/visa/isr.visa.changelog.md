# ISR Visa Changelog
All notable changes to these libraries will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)

Current overall binaries revision: 7.2.9159

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

[7.2.9213]: https://www.github.com/atecoder/dn.vi.ivi
[vs.VI]: https://www.github.com/atecoder/dn.vi.ivi
[vs.Visa]: https://bitbucket.org/davidhary/vs.io.visa
