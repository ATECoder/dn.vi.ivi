# VI Core libraries

Virtual Instruments (VI) libraries using the [IVI Foundation] VISA.

* [Supported .NET Frameworks](#Supported-Dot-Net-Frameworks)
* [Runtime Pre-Requisites](#Runtime-Pre-Requisites)
* Project README files:
  * [cc.isr.VI.Device](/src/device/device/readme.md) 
  * [cc.isr.VI.Tsp](/src/device/tsp/readme.md) 
  * [cc.isr.VI.Device.Tsp.Script](/src/device/tsp.script/readme.md) 
  * [cc.isr.VI.Device.Tsp2](/src/device/tsp2/readme.md) 
  * [cc.isr.VI.Foundation](/src/resource/foundation/readme.md) 
  * [cc.isr.VI.Pith](/src/resource/pith/readme.md) 
  * [cc.isr.VI.DeviceWinControls](/src/ui/device.win.controls/readme.md) 
  * [cc.isr.VI.SubsystemsWinControls](/src/ui/subsystems.win.controls/readme.md) 
  * [cc.isr.VI.WinControls](/src/ui/win.controls/readme.md) 
  * [cc.isr.VI.Ats600](/src/vi/ats600/ats600/readme.md) 
  * [cc.isr.VI.Clt10](/src/vi/clt10/clt10/readme.md) 
  * [cc.isr.VI.E4990](/src/vi/e4990/e4990/readme.md) 
  * [cc.isr.VI.EG2001](/src/vi/eg2001/eg2001/readme.md) 
  * [cc.isr.VI.K2000](/src/vi/k2000/k2000/readme.md) 
  * [cc.isr.VI.K2400](/src/vi/k2400/k2400/readme.md) 
  * [cc.isr.VI.K2400.Hipot](/src/vi/k2400.hipot/k2400.hipot/readme.md) 
  * [cc.isr.VI.K2450](/src/vi/k2450/k2450/readme.md) 
  * [cc.isr.VI.K2600](/src/vi/k2600/k2600/readme.md) 
  * [cc.isr.VI.K2600.Ttm](/src/vi/k2600.ttm/k2600.ttm/readme.md) 
  * [cc.isr.VI.K2700](/src/vi/k2700/k2700/readme.md) 
  * [cc.isr.VI.K3458](/src/vi/k3458/k3458/readme.md) 
  * [cc.isr.VI.K34980](/src/vi/k34980/k34980/readme.md) 
  * [cc.isr.VI.K3706](/src/vi/k3706/k3706/readme.md) 
  * [cc.isr.VI.K7000](/src/vi/k7000/k7000/readme.md) 
  * [cc.isr.VI.k7500](/src/vi/k7500/k7500/readme.md) 
  * [cc.isr.VI.K5700](/src/vi/k5700/k5700/readme.md) 
  * [cc.isr.VI.T1700](/src/vi/t1700/t1700/readme.md) 
* [Attributions](Attributions.md)
* [Change Log](./CHANGELOG.md)
* [Cloning](Cloning.md)
* [Code of Conduct](code_of_conduct.md)
* [Contributing](contributing.md)
* [Legal Notices](#legal-notices)
* [License](LICENSE)
* [Open Source](Open-Source.md)
* [Repository Owner](#Repository-Owner)
* [Security](security.md)

<a name="Repository-Owner"></a>
# Repository Owner
[ATE Coder]

<a name="Authors"></a>
## Authors
* [ATE Coder]  

<a name="legal-notices"></a>
## Legal Notices

Integrated Scientific Resources, Inc., and any contributors grant you a license to the documentation and other content in this repository under the [Creative Commons Attribution 4.0 International Public License] and grant you a license to any code in the repository under the [MIT License].

Integrated Scientific Resources, Inc., and/or other Integrated Scientific Resources, Inc., products and services referenced in the documentation may be either trademarks or registered trademarks of Integrated Scientific Resources, Inc., in the United States and/or other countries. The licenses for this project do not grant you rights to use any Integrated Scientific Resources, Inc., names, logos, or trademarks.

Integrated Scientific Resources, Inc., and any contributors reserve all other rights, whether under their respective copyrights, patents, or trademarks, whether by implication, estoppel or otherwise.

<a name="Supported-Dot-Net-Frameworks"></a>
## Supported .NET Frameworks

The following [Microsoft .NET Framework] are supported:
* .NET Standard 2.0;
* .Net Standard 2.1;
* .NET Framework 4.72;
* .NET Framework 4.8;
* .NET 5.0 and above.

<a name="Runtime-Pre-Requisites"></a>
## Runtime Pre-Requisites

### IVI Visa
[IVI Foundation] 5.11 and above is required for accessing devices.
The IVI VISA implementation can be obtained from either one of the following vendors: 
* Compiled using VISA Shared Components version: 5.12.0

### Keysight I/O Suite
The [Keysight I/O Suite] version 18.1 and above is recommended.
* Compiled using I/O Library Suite revision: 18.1.26209.5 released 2020-10-15.

### NI VISA 
* [NI VISA] revision 19.0 and above can be used.
* Tested with [NI VISA] 20.0 and 21.0.

[Creative Commons Attribution 4.0 International Public License]: https://github.com/ATECoder/dn.vi.ivi/blob/main/license
[MIT License]: https://github.com/ATECoder/dn.vi.ivi/blob/main/license-code
[ATE Coder]: https://www.IntegratedScientificResources.com
[dn.core]: https://www.bitbucket.org/davidhary/dn.core

[IVI Foundation]: https://www.ivifoundation.org
[Keysight I/O Suite]: https://www.keysight.com/en/pd-1985909/io-libraries-suite
[NI VISA]: https://www.ni.com/en-us/support/downloads/drivers/download.ni-visa.html#346210
[Test Script Builder]: https://www.tek.com/keithley-test-script-builder
[Microsoft .NET Framework]: https://dotnet.microsoft.com/download

