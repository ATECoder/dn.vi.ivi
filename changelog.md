# Changelog
Notable changes to the `cc.isr.vi` solution are documented in this file using the 
[Keep a Changelog] style. The dates specified are in coordinated universal time (UTC).

[8.1.9296]: https://www.github.com/atecoder/dn.vi.ivi

## [8.1.9296] - 2025-06-14
- TTM Console:
  - Specify System Diagnostics when referencing the debugger.
- Pith Session Base:
  - Add method: Is Command Exist and True.
- Device TSP Session Base Extensions:
  - Byte code: Throw exception if the byte code query command does not exist.
- Device TSP Script Session Base Extensions:
  - Is Binary Script Queries: Use Device Tsp Is Byte Code queries.

## [8.1.9288] - 2025-06-06
- Visa Console and Demo:
  - Set Serilog setting to copy to the output folder.
  - Add 'Exists' property to the settings Json file.
  - Initialize the application settings upon creating the settings instance when a debugger is attached as when running under the IDE.

## [8.1.9286] - 2025-06-04
- Access subsystem base: 
  - Rename Is Certified to Is Registered and Certified.
  - Rename Loaded to Is Certified and add a details argument.

## [8.1.9285] - 2025-06-03
- Access Subsystem Base:
  - Add registration and certification methods.
  
## [8.1.9279] - 2025-05-28
- VI.Device Access Subsystem Base:
  - Consolidate the registration code.
  - Add IsCertified.
- Device Tsp Session Base Extensions:
  - Add method to query the controller node serial number.
- Script Info Collection:
  - Add methods to define the deploy file name using a model and major version.

## [8.1.9275] - 2025-05-24
- Device Tsp Script
  - Script Info: Add IsEncrypted property.

## [8.1.9272] - 2025-05-21
- Device Tsp Script Session Base Extension
  - Delete saved scripts: 
    - display deleting even if no scripts found.
	- skip and do not count nil and non ISR scripts.
	- return the number of removed scripts.

## [8.1.9271] - 2025-05-20
Pith:
  - Tsp.Syntax.Script: Update the getter command for the saved scripts to end with a script name rather than a comma.

## [8.1.9270] - 2025-05-19
- K2600 TTM:
  - Add legacy version getter to the meter and support syntax.
  - Set the legacy firmware boolean based onteh legacy vs. latest version getters.

## [8.1.9268] - 2025-05-17
- Device Tsp Script 
  - Move Assembly Resources Scribe interface and base class to the Std framework project.

## [8.1.9267] - 2025-05-16
- Device Tsp Script Session Base Extensions:
  - Script info: Add latest version.
  - Update script info and firmware status report format.
  - Condition toggling auto run function on the existence of the script.
  - Add the Lua 'isInLoaded' function if not found.
- Firmware Info:
  - Compare actual version to the latest version instead of the released version.
  - Parse firmware version status based on the actual and latest firmware versions.

## [8.1.9266] - 2025-05-15
- Device Tsp Script
  - Script Info Collection: 
    - Add setter for the device information.
    - Add a method to return the script names.

## [8.1.9265] - 2025-05-14
- Device Tsp Script Session Base Method
  - Add a helper methods to build and load a script.
  - Delete Object: Fix the code to correctly report the existence of the object.
  - Script info: 
    - Remove initialize and dynamic properties. It must be set specifically.

## [8.1.9264] - 2025-05-13
- Lexemes: 
  - Add TSP Lexeme for IPairs.
- Device Tests Asserts:
  - Add Generic status subsystem asserts.
- Device TSP Script:
  - Resource Manager Base: 
    - Add Resource File exists method.
	- Handle an empty script framework ID for common resources to multiple frameworks.
  - Session Base Extensions.
    - Add Delete Saved Scripts.
	- Add toggling auto run when deleting scripts.

## [8.1.9262] - 2025-05-11
- Pith Tsp Syntax
  - Move menu commands from script to display.
- Device Tsp Session Base Exception
  - Move menu commands from script to display.
  - Remove read script status methods.
  - Move display commands from script to tsp session base extensions.
  - Move script read for the method_imports file and use FileReadAllText.

## [8.1.9261] - 2025-05-10
- Device TSP Session Base Extensions
  - Add nil extensions.
- Device TSP Script Session Base Extensions
  - Replace nil script with nil object methods.
- Device Tsp Firmware namespace
  - Add Firmware Info.
- Device Tsp Script namespace
  - Add script resource manager base class.
  - Add methods to the script resource manager interface to allow implementation of the base class by inheritance.
- Pith:
  - Add lexemes TSP and LUA classes.
  - Add copy script format format syntax.

## [8.1.9260] - 2025-05-09
- Add Script Resource Manager Interface.
- Scrip[t Info:
  - Add IsSupport and IsPrimary properties.
- Session Base Extensions
  - Deploy: Add query of script version.
  - Read State: run the script if the version getter is not found.
  - Export: Add export saved scripts.

## [8.1.9258] - 2025-05-07
- Device Tsp Script Namespace
  - ScriptInfo
    - Initialize string values to empty.
  - Deploy:
    - add reading status of scripts and saved scripts.

## [8.1.9257] - 2025-05-06
- Write all text using default encoding.
- Device Tsp Script Namespace.
  - Move line ending extensions and validations to the standard framework library.
  - Write all text: use default encoding.
  - Move copy file to the framework library.

## [8.1.9256] - 2025-05-05
- Device Tsp Script Namespace
  - Set all writes to System.Text.Encoding.UTF8 )
  - Add fetch script raw.
  - Add line ending to the fetch method.
  - Remove unnecessary line ending conversions.
  - Add line ending extensions.
  - Move unused code tot he line ending extensions. 
  - Rename Binary to Byte Code.

## [8.1.9255] - 2025-05-04
- Device Tsp Script Namespace
  - Export: add reader writer validations.
  - String Compressor: Fix selecting the contents in case of extra text around the prefix and suffix.
  - export:
    - use writer write line to convert from Linux to Windows instead of using the convert method.
	- remove the end of line conversion.
  - Remove obsoleted reader methods.
  - Ensure script source and compressed source end with a single line termination for validations.
  - Script Compression:
    - Add validation.
	- Add string comparison using string readers. 
## [8.1.9253] - 2025-05-02
- Device tsp Script
  - Add copy script between files to work around the read only attribute.
  - Initialize script info collection properties when reading the embedded scripts.

## [8.1.9251] - 2025-05-01
- Device tsp Script
  - Fix reading the script state: set the actual version.
  - Script Info Collection: Add enumeration method such as All Save and Any Loaded.

## [8.1.9251] - 2025-04-30
- Serilog Settings
  - Set log level to warning.
- Gac Loader: 
  - Remove console output.
- Device Tsp Script Namespace
  - Add destination folder argument to the trim and compress method to facilitate testing.
- Tests:
  - replace with $"{methodFullName} initializing" );
  - Remove Console.WriteLine( $"@{methodFullName}" );
  - Replace OutputMember method with Console Output Member Message.
  - Add assertion over orphan messages with member method reference.
  - Add throw if device error with member method reference.
  - Display Gac loader implementation on initializing tests.
  - Display the GAC Loader only if loaded.

## [8.1.9250] - 2025-04-29
- Fix test method headline to remove capitalization and punctuation.
- Include Json files as 'Content Include' in the project files.
- VI Device Tsp Namespace
  - Session Base Extensions Save: 
    - Remove throw if already saved argument.
	- Check if saved after saving.
- UI Device WIn Controls MSTest
  - Set serialization options of the Scriber to include CSV color serialization.
- MS Test
  - replace trace write line with console write line.

## [8.1.9249] - 2025-04-28
- Device Tsp Script:
  - Add node auto run toggle methods.
  - Add node save method.
- Firmware Manager:
  - fix saving script with auto run.

## [8.1.9248] - 2025-04-27
- Device Tsp Scrip Namespace
  - Add query firmware version methods.
  - Remove nullable from session base argument.

## [8.1.9247] - 2025-04-26
- Device Tsp Script Namespace
  - Remove the ISCriptInfo interface.
  - Rename script info base to script info.
  - Add a copy constructor.
  - Add ScriptStatuses Enum: None (not known), Loaded, Activated, Saved, Older, Current, Newer
  - Add methods for deleting a collection of script info items and a collection of such items.
  - Add methods for running a collection of script info items and a collection of such items.
  - Add methods for reading the status of a collection of script info items and a collection of such items.
  - Add serial number and model number to the script info collection.
- K2600 TTM Tests
  - Use DeviceTs.Script namespace auto-run methods.
 
## [8.1.9246] - 2025-04-25
- K2600.TTM
  - Use the new TTM version query commands.
- K2600.TTM.Tests
  - legacy tests passed.
  - firmware tests passed.
  - Add optional argument for validating model.
- project files:
  - set None in include statement for documentation.
  - set Content in include statement for settings and resources.

## [8.1.9245] - 2025-04-24
- Pith.SessionBase
  - Output garbage collection message tot he debug screen only.
- Device Tsp Script Namespace Session Base Extensions:
  - Save Script: Add option to skip if already saved.
  - Display Line: add a generic method that sets the cursor and pads the text.

## [8.1.9244] - 2025-04-23
- Link Subsystem: Replace the local node serial number query with  cc.isr.VI.Syntax.Tsp.LocalNode.SerialNumberFormattedQueryCommand.
- Device Tsp Script Namespace
  - Add auto run settings methods.
  - Set script to auto run based on the script info settings.
  - Build deploy filename when building scripts using the script info.
  - Update version to 9244. 

## [8.1.9243] - 2025-04-22
- Device Tsp Script namespace
  - Move Session Base Extensions ImportSaveToNvm to a deploy file. 
  - Fix code deleting load menu item.
  - Add queries from the LOADED table.
  - Add methods to build all script info scripts.
- Session Base:
  - Document the need to override the write line command for writing byte code.
- Pith Syntax Tsp Display: Prepend display with _G.
- Update versions to 8.1.9243.

## [8.1.9242] - 2025-04-21
- K2600 Tests
  - Add Tsp Session Debug Script Tests.
- Device Tsp Script Namespace
  - Obsolete all reader methods because the stream reader fails interpreting escape sequences causing it fail to read the Keithley byte code to the end of file.
- SessionBase:
  - Add method to write non-compound line to handle lines with escape sequences that might be interpreted as end of line. 
- SessionBase:
  - restore the trim to not add extra space.
  - Tsp Script Parse:
    - trim trailing comments.
- Device TSP Script Namespace
  - Add writing lines code to the loader to ignore the script building commands.
- K2600 Script Tests: all ran.

## [8.1.9239] - 2025-04-18
- Device Tsp Script namespace:
  - Add a Load and Save to NVM using a reader source.
  - Trim and Compress: Compresses to the trimmed file path appended with a 'c'.

## [8.1.9238] - 2025-04-17
- Device Tsp Script
  - Script Info Base: fix building the deploy file title.
  - Add a method to fetch and compress script to file.
  - Update build tests to export only the deploy file
  - Script Info Base
    - Add method to build the deploy file name using model and version arguments so tests can be conducted without having the version info base instance.
  - Tsp Script Parser: Append a single space to each trimmed code line. This fixed the failure of the certify binary script to load.
- K2600 Tests: Add a trim and compress test.  
## [8.1.9237] - 2025-04-16
- Device Tsp Namespace
  - Add namespace to the Enums file.
  - Session Base Extensions
    - Add reader import method to be used for importing an embedded resource.
	- Document the requirement that a reader import must be decompressed first is importing a string reader.
- K2600 Tests
  - All TSP test passed.

## [8.1.9236] - 2025-04-15
- Device Tsp 
  - session extensions
    - Add load and run option to the load command.
    - Add new line to compressed prefix and suffix.
    - Add test of file compression.
    - Load Script: reader: check for compression if a stream reader.
  - script compressor
    - rewind the reader after checking for compression.
  - Add Script Info interface and base class.
- Tsp Script Project
  - Move firmware manager embedded resources methods to the embedded resource class.
- K2600 MS Test;
  - add compression.

## [8.1.9235] - 2025-04-14
- Device Tsp:
  - Add end of line conversion to methods exporting script to files.
  - Add is loaded, is activate and is binary methods.
- Device Tsp Script:
  - Remove binary scripts script.
  - Move compression to the framework library and the generic device TSP string extensions.
  - Add script compressor.
    - Add query if a script is compressed.
  - Add script trim methods.
  - Add compression methods.
- K2600 Tests
  - update file names.
  - Update export methods to accommodate the EOL conversions.
  - Add Tsp Session Script Tests.
  - load as binary: remove duplicate loads. 
  - All tests passed.
- Lua Syntax:
  - Update the wait complete description.

## [8.1.9233] - 2025-04-12
- Resource Pith TSP Script Syntax: 
  - Move find and catalog formats from Lua.
- Resource Pith TSP Node Syntax: 
  - Add catalog command format.
  - add find saved script command.
- Device Tsp Session Base Extensions
  - Use TSP Syntax script constants.
  - Add find and list saved script for nodes.
  - Add end of line conversion to methods exporting script to files.
- K2600 Tests
  - update file names.
  - Update export methods to accommodate the EOL conversions.

## [8.1.9232] - 2025-04-11
- Pith Lua Syntax:
  - Rename ParseLuaChunkLine to ParseLuaChunkLine;
- Pith TSP Syntax
  - Add code parsing.
- Tsp Script Session Base Extensions
  - Add IsSaveScript, Nil Script , Remove from User Scripts, and Remove from Save Scripts.
- 2600 MS Tests: Script tests:
  - select before loading.
  - remove saved before saving.
  - test saving and running a converted binary script.

## [8.1.9231] - 2025-04-10
cc.isr.VI.Device.Tsp project :
- Add session extension methods for scripts.
- Throw exceptions if device error was detected by the Session Base instance. 
K2600 Tests:
 - Test Tsp session extension script methods.
 
## [8.1.9229] - 2025-04-08
- Tsp.Script.ResourceManager
  - Tighten code reading a resource file and an embedded resource file.
  - Add methods to copy or create text file for testing the copy or creation of linked resources.
  - Use FirmwareScriptBase.ScriptFileExtension and FirmwareScriptBase.ScriptBinaryFileExtension.
- implement changes to work around the discontinues support for loading binary scripts.
- obsolete converting scripts to binary.
- compress scripts upon saving to file.
- default to compressing the deployed file.

## [8.1.9228] - 2025-04-07
- Add script file name builder.
- Add resource manager class for loading the embedded resource.
- Split session firmware manager to embedded and anonymous files.
- vi.tsp.script:
  - Add binary scripts for testing.
  - Update embedded resources.
- Firmware Script Base
  - Add Build, trimmed and deployed script file name.
  - Build these from the title and the file format.
- Firmware Manager
  - Add code for loading embedded or file resources.

## [8.1.9226] - 2025-04-05
- Update settings when reading a specified if script exists.
- Resource TSP Syntax: Add Load and End Script commands and load string command.
- Script: 
  - Add binary scripts as an embedded resource.
  - Add methods for reading the binary script embedded resource and convert script to binary for  firmware 3.x.
  - Add method to build the load script command.
  - Use '\27LuaP\0\4\4\4\' to determine if a script source is in binary format.
  - Use string.IsNullOrEmpty() to check if a delimiter is empty. 

## [8.1.9222] - 2025-04-01
- Default to not overriding existing files when initializing the settings.
  - Tests: Override settings files when initializing the settings.
    - Remove duplicate reading for settings.
- Replace type of with this.GetType()
- Update revisions to 9222.

## [8.1.9220] - 2025-03-30
- Replace tabs with spaces.
- Remove trailing spaces 
- Reference main project revision in test and demo projects.
- Replace community SDK observable object with INotify Property Change in test site settings classes.
- Update change log.
- Update revisions to 9220.

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

[Keep a Changelog]: https://keepachangelog.com/en/1.0.0/
[vs.VI]: https://www.github.com/atecoder/dn.vi.ivi
[vs.Visa]: https://bitbucket.org/davidhary/vs.io.visa
