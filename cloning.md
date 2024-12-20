# Cloning

* [Source Code](#Source-Code)
* [Repositories](#Repositories)
* [Packages](#Packages)
* [Facilitated By](#Facilitated-By)

<a name="Source-Code"></a>
## Source Code
Clone the repository along with its requisite repositories to their respective relative path.

### Repositories
The repositories listed in [external repositories] are required:
* [Enums Libraries] - .NET Standard libraries
* [Json Libraries] - .NET Standard libraries
* [Logging Libraries] - .NET Standard libraries
* [Std Libraries] - .NET Standard libraries
* [Tracing Libraries] - .NET Standard libraries
* [Win Libraries] - .NET Windows libraries
* [Win Controls Libraries] - .NET Windows Controls libraries
* [Win Forms Libraries] - .NET Windows Forms libraries
* [Units Amounts] - Units and amounts Libraries
* [IDE Repository] - IDE support files.
* [WiX Repository] - WiX Installer files.
* [VI Libraries] - VI Core Libraries.
* [VISA Libraries] - VISA  Libraries (private).

```
git clone git@bitbucket.org:davidhary/dn.enums.git
git clone git@bitbucket.org:davidhary/dn.json.git
git clone git@bitbucket.org:davidhary/dn.logging.git
git clone git@bitbucket.org:davidhary/dn.std.git
git clone git@bitbucket.org:davidhary/dn.tracing.git
git clone git@bitbucket.org:davidhary/dn.win.git
git clone git@bitbucket.org:davidhary/dn.win.controls.git
git clone git@bitbucket.org:davidhary/dn.win.forms.git
git clone git@bitbucket.org:davidhary/dn.automata.git
git clone git@bitbucket.org:davidhary/cc.isr.UnitsAmounts
git clone git@bitbucket.org:davidhary/vs.ide.git
git clone git@bitbucket.org:davidhary/vs.wix.git
git clone https://github.com/atecoder/dn.vi.ivi.git
git clone git@bitbucket.org:davidhary/dn.visa.git
```

Clone the repositories into the following folders (parents of the .git folder):
```
%dnlib%\core\enums
%dnlib%\core\json
%dnlib%\core\logging
%dnlib%\core\std
%dnlib%\core\tracing
%dnlib%\core\units.amounts
%dnlib%\core\win
%dnlib%\core\win.controls
%dnlib%\core\win.forms
%vslib%\core\ide
%vslib%\core\wix
%dnlib%\vi\vi
%dnlib%\vi\visa
```
where %dnlib% and %vslib% are  the root folders of the .NET libraries, e.g., %my%\lib\vs 
and %my%\libraries\vs, respectively, and %my% is the root folder of the .NET solutions

## Global Configuration Files
ISR libraries use a global editor configuration file and a global test run settings file. 
These files can be found in the [IDE Repository].

Restoring Editor Configuration:
```
xcopy /Y %my%\.editorconfig %my%\.editorconfig.bak
xcopy /Y %vslib%\core\ide\code\.editorconfig %my%\.editorconfig
```

Restoring Run Settings:
```
xcopy /Y %userprofile%\.runsettings %userprofile%\.runsettings.bak
xcopy /Y %vslib%\core\ide\code\.runsettings %userprofile%\.runsettings
```
where %userprofile% is the root user folder.

## Packages
Presently, packages are consumed from a _source feed_ residing in a local folder, e.g., _%my%\nuget\packages_. 
The packages are 'packed', using the _Pack_ command from each packable project,
into the _%my%\nuget_ folder as specified in the project file and then
added to the source feed. Alternatively, the packages can be downloaded from the 
private [MEGA packages folder].

<a name="Facilitated-By"></a>
## Facilitated By
* [Visual Studio]
* [Jarte RTF Editor]
* [Wix Toolset]
* [Atomineer Code Documentation]
* [EW Software Spell Checker]
* [Code Converter]
* [Funduc Search and Replace]
* [IVI Foundation] - IVI Foundation VISA
* [Keysight I/O Suite] - I/O Libraris
* [Test Script Builder] - Test Script Builder


[MEGA packages folder]: https://mega.nz/folder/KEcVxC5a#GYnmvMcwP4yI4tsocD31Pg
[Enums Libraries]: https://bitbucket.org/davidhary/dn.enums
[Json Libraries]: https://bitbucket.org/davidhary/dn.json
[Logging Libraries]: https://bitbucket.org/davidhary/dn.logging
[Std Libraries]: https://bitbucket.org/davidhary/dn.std
[Tracing Libraries]: https://bitbucket.org/davidhary/dn.tracing
[Win Libraries]: https://bitbucket.org/davidhary/dn.win
[Win Controls Libraries]: https://bitbucket.org/davidhary/dn.win.controls
[Win Forms Libraries]: https://bitbucket.org/davidhary/dn.win.forms
[Units Amounts]: https://www.github.com/atrcoder/units-amounts
[VI Libraries]: https://www.github.com/atecoder/dn.vi.ivi
[VISA Libraries]: https://www.bitbucket.org/davidhary/dn.visa
[TSP Support Framework]: https://www.github.com/atecoder/tsp.1.core
[TSP TTM Framework]: https://www.github.com/atecoder/tsp.1.ttm
[TSP Support Framework]: https://www.github.com/atecoder/tsp.1.core

[IVI Foundation]: https://www.ivifoundation.org
[IVI Foundation]: http://www.ivifoundation.org
[Keysight I/O Suite]: https://www.keysight.com/en/pd-1985909/io-libraries-suite
[NI VISA]: https://www.ni.com/en-us/support/downloads/drivers/download.ni-visa.html#346210
[Test Script Builder]: https://www.tek.com/keithley-test-script-builder
[Microsoft .NET Framework]: https://dotnet.microsoft.com/download

[external repositories]: ExternalReposCommits.csv
[IDE Repository]: https://www.bitbucket.org/davidhary/vs.ide
[WiX Repository]: https://www.bitbucket.org/davidhary/vs.wix
