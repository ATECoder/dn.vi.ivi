# Thermal Transient Meter&trade; Release notes

Last update: 2026-01-16 11:15:48

## User Guides 

| File                                   | Date       | Status                                                      |
|:---------------------------------------|:-----------|:------------------------------------------------------------|
| TTM Console Guide                      | 2013-12-30 | Legacy. Awaiting release of version 8.x of the TTM Console. |
| TTM Firmware Command Line Loader Guide | 2026-01-16 | Up to date.                                                 |
| TTM Driver API Guide                   | 2024-10-09 | Work in progress.                                           |     
| TTM Driver API Upgrade Guide           | 2024-12-09 | Work in progress.                                           |     
| TTM Firmware API Guide                 | 2025-09-29 | Work in progress.                                           |     
| TTM Framework Guide                    | 2025-09-16 | Work in progress.                                           |     
| TTM Instrument Guide                   | 2025-09-12 | Up to date.                                                 |
| TTM Manual Test Guide                  | 2024-12-30 | Work in progress.                                           |     

## Releases

### 2026-01-16

| Item               | Info                                | Notes                                                         |
|:-------------------|:------------------------------------|:--------------------------------------------------------------|
| Loader             | Ttmware.Loader.8.1.9512.2.4.9410.7z |                                                               |
| Source code        | Ttmware.code.8.1.9512.2.4.9410.7z   |                                                               |
| Firmware           | 2.4.9410                            |                                                               |
| .NET               | Modern .NET 10.                     | Required                                                      |
| IVI-Visa           | 8.0.2                               | Included in the published package                             |
| Keysight I/O Suite | 21.2.207                            | Required                                                      |

The command file `00_make_cc_isr_settings_dir` must be run with administrator credential the first time this version is installed. This command creates the application settings folder under the shared `c:\ProgramData` folder.

The _TTM Firmware Command Line Loader Guide_ was updated with installation instruction of the required modern .NET and IVI Foundation VISA implementation.

### 2026-01-12

| Item               | Info                                | Notes                                                         |
|:-------------------|:------------------------------------|:--------------------------------------------------------------|
| Loader             | Ttmware.Loader.8.1.9508.2.4.9410.7z |                                                               |
| Source code        | Ttmware.code.8.1.9508.2.4.9410.7z   |                                                               |
| Firmware           | 2.4.9410                            |                                                               |
| .NET               | Modern .NET 10.                     | Required                                                      |
| IVI-Visa           | 7.2.0                               | Included in the published package                             |
| Keysight I/O Suite | 21.1.17                             | Required                                                      |

The command file `00_make_cc_isr_settings_dir` must be run with administrator credential the first time this version is installed. This command creates the application settings folder under the shared `c:\ProgramData` folder.

