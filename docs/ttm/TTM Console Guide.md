# # Thermal Transient Meter&trade; Console Guide

Last Updated 2013-12-30

# Table of Contents

* [Description]
* [Terminology]
* [Requirements]
	* [Microsoft .NET Framework]
	* [NI-VISA Runtime]
* [Preparations]
	* [Wiring the Device Under Test]
	* [Connecting the Instrument]
	* [Launching the ISR TTM Console]
	* [Connecting to the instrument]
		* [Selecting the instrument Resource Name]
		* [A Note on Connecting]
		* [Clearing the instrument State]
	* [Loading the Firmware]
		* [Notes]
		* [Updating the Firmware]
			* [Unload Existing Firmware]
			* [Load New Firmware]
			* [Embed to Non Volatile Memory]
			* [Testing Persistence of Firmware]
* [Testing]
	* [Configuring a TTM Test]
	* [Measuring Initial and Final Resistances and Thermal Transients]
	* [Automatically Measuring Initial and Final Resistances and Thermal Transients]
	* [Reading and Saving the Thermal Transient Trace]
* [Listing and Saving Part Measurements]
* [Alerts and Messages]
* [Program Log]
* [Attributions]

# Description

The ISR Thermal Transient Meter (TTM) Console measures Thermal Transients. The transients are measured using the ISR TTM Driver. This instrument is based on the Keithley Instruments 2600A series instruments and uses scripts embedded in the instrument to perform the measurements. Thermal Transients can be also measured using the ISR TTM Instrument as a stand-alone device.

# Terminology

TTM Firmware and Scripts

The ISR Thermal Transient Meter is a implemented by customizing an off-the-shelf Keithley Driver. This is done by embedding custom firmware--the TTM Firmware--in this Driver. The TTM firmware consists of scripts written in the programming language of the Driver.

Unloading (Deleting removing) and Uploading (Installing) Firmware

The ISR Thermal Transient Meter comes already installed with the TTM Firmware. The TTM Console is capable of updating the embedded TTM firmware. This is done by removing (unloading) the TTM firmware embedded in the instrument and uploading (installing) new TTM Firmware.

Saving and Volatile and Non-Volatile Memory

When updated, the TTM Firmware exists in the instrument volatile memory. Scripts residing in volatile memory are erased when turning off the Driver. The TTM Console is capable of saving the TTM Firmware in non-volatile memory. Scripts embedded in non-volatile memory persist in the Driver.

Startup (boot) Firmware

When starting the TTM instrument, the instrument panel displays the Thermal Transient Meter 'welcome message. In addition, the TTM Firmware is initiated to prepare the instrument for measurement. This is done by the TTM Startup or bootstrap script. The startup script is embedded in non-volatile memory when saving the TTM Firmware to non-volatile memory.  

# Requirements

The latest information about requirement can be found in the program read me file which can be downloaded from the ISR FTP site:

http://bit.ly/aJgNDP

# Microsoft .NET Framework

The TTM console requires the installation of Microsoft .NET Framework 4.0. This is a one-time installation.

The installation program can be downloaded from the following location:

http://www.microsoft.com/en-us/download/details.aspx?id=17851  
  
To download, enter the link in the address bar of your browser.

# NI-VISA Runtime

The TTM console requires the installation of VISA runtime support from National Instruments. This is a one-time installation. The VISA run time can be downloaded from the National Instrument FTP site:  

http://ftp.ni.com/support/softlib/visa/VISA%20Run-Time%20Engine

  
To download, enter the above link in the address bar of your browser.

# Preparations

# Wiring the Device Under Test

The device under test must be connected to the TTM Instrument using four-wires:

![](images/E0858D6A252214FCA9C71A26247867F77B3F3C44.PNG)

# Connecting the Instrument

The TTM Console connects to the instrument using with USB, Ethernet or GPIB (IEEE-488).

The instrument address on the GPIB bus is displayed when the instrument is started. Otherwise, the GPIB must be enabled from the instrument front panel as described in the Interface Configuration chapter of the Getting Started section of the Series 2600 Reference Manual.

# Launching the ISR TTM Console

Start the TTM Console from Start, Programs, ISR, isr.TTM.2014, Programs, Console

# Connecting to the instrument

Connect the program to the instrument by clicking the _connect_ (![](images/19221B6DFE1C51ED94C19701061E63E636D8A72C.jpg)) button from the _FIND, SELECT AND CONNECT_ selector. This assumes the instrument resource name is correctly entered (see [Selecting the instrument Resource Name](#4DE0BE2C5A206089E06808C1494635CD8B611066) below). The selector displays the default resource name or the name last used. The resource name defaults to board 0 and instrument at address 26, e.g., GPIB0::26::INSTR. Once connected, the program enables measurement if firmware is already loaded (see [Loading the Firmware](#776005067687CDCD0F9A726DDD3B35D0949348EA) below).

# Selecting the instrument Resource Name

*   Click the _search_ (![](images/33117E4ECF87EC555A90AD1203684CE4284D74EB.jpg)) button from the _FIND, SELECT AND CONNECT_ selector in the _CONNECT_ tab.
*   The selector will display the list of available VISA resources connected to the computer.
*   Select the resource name of the Keithley 2600A instrument, e.g., GPIB0::26::INSTR.

# A Note on Connecting

At times, especially when using a USB GPIB adaptor, VISA loses track of the available resources. In this case, the _FIND,_ _SELECT AND CONNECT_ selector will show no resources in the resource list. A Workaround that most often resolves this is disconnecting and then reconnecting the USB-GPIB adapter.

# Clearing the instrument State

*   The instrument state is reset to its initial known state when first connected.
*   The instrument can be reset to its know state by selecting the _clear_ button (![](images/7CF0788D23466967AB36581D11243F70F6F039EE.jpg)) from the _FIND, SELECT AND CONNECT_ selector.

# Loading the Firmware

# Notes

*   When connecting, the program queries the instrument for the current firmware revision and displays the firmware status under the _FIRMWARE_ tab.
*   The TTM firmware can be unloaded (removed from the instrument), loaded, and embedded to permanent memory from the _FIRMWARE_ tab.
*   A good practice when updating firmware is to disconnect and toggle the instrument power after deleting scripts.
*   Firmware resides in volatile memory after it is loaded using the _LOAD FIRMWARE_ button. Firmware residing in volatile memory is cleared when the instrument is turned off.
*   For using the instrument in its stand alone mode, embed the firmware into permanent (non-volatile) memory after loading.
*   Once the firmware is loaded, the program enables the measurement controls on the _Measurement_ Tab.

# Updating the Firmware

# Unload Existing Firmware

*   Connect to the instrument from the CONNECT tab;
*   Select the _FIRMWARE_ tab;
*   Click the _UNLOAD FIRMWARE_ button. This unloads the firmware from the instrument;
*   Close the TTM Console program;
*   Toggle the instrument power.

# Load New Firmware

*   Start the TTM Console program;
*   Connect to the instrument from the CONNECT tab;
*   Select the _FIRMWARE_ tab;
*   Click the _LOAD FIRMWARE_ button;
*   Allow time for the firmware to load.
*   Once loaded, the current version of the firmware will be displayed in the _INSTALLED VERSION_ text box. This version should match the version listed in the _RELEASED VERSION_ text box. At this point, the firmware resides in volatile memory and will be cleared when the instrument is turned off;
*   Also, the _EMBED FIRMWARE_ button will be enabled.

# Embed to Non Volatile Memory

*   Click _EMBED FIRMWARE_. This will embed the firmware in non-volatile memory. Firmware residing in non-volatile memory persists after the instrument power is toggled;
*   Allow time for the firmware to be embedded into non-volatile memory;
*   You can now proceed with measurements.

# Testing Persistence of Firmware

Testing Persistence of Firmware

*   Disconnect the instrument by clicking the _connect_ (![](images/19221B6DFE1C51ED94C19701061E63E636D8A72C.jpg)) button from the _FIND,_ _SELECT AND CONNECT_ selector of the _CONNECT_ tab.
*   Close the TTM Console program;
*   Toggle the instrument power;
*   Start the TTM Console program;
*   Connect the instrument from the CONNECT tab;;
*   Select the _FIRMWARE_ tab;
*   Verify that the installed version matches the released version.

# Testing

# Configuring a TTM Test

*   Select the _CONFIGURE_ tab.
*   Enter the operator and lot identification numbers in the relevant boxes;
*   Enter the part number in the _PART NUMBER_ box;
*   Select the time between the thermal transient test and final resistance in the _POST_ _TTM DELAY_ box;
*   Enter the current level and voltage limit for the cold (low-current) resistance measurements;
*   Enter the current level and the maximum expected voltage change for the thermal transient measurement;
*   C heck or uncheck the option to _CHECK CONTACTS BEFORE INITIAL RESISTANCE;_
*   Click _APPLY_ to accept the new settings and send them to the instrument;
*   To ignore changes made in the _CONFIGURE_ panel, click _RESTORE_. This cancels changes made and displays the current part settings;
*   Once applied, the new settings become the default settings, which will be displayed the next time the program is launched.

Important Note about Noise and Current Range

Thermal transient noise level depends on the source current range. The current range setting change based on the decimal point. Thus, setting the current range to 1.0A yields a significantly larger noise level than setting the current range to 0.999A.

# Measuring Initial and Final Resistances and Thermal Transients

*   Select the _MEASURE_ tab;
*   Connect a device to the instrument;
*   Set the part serial number in the _SERIAL NUMBER_ box;
*   Click _CLEAR MEASUREMENTS_ to clear data of the previous part;
*   Click _INITIAL RESISTANCE_ to measure initial resistance;
*   Notice that the measurement is displayed on the instrument panel;
*   If contact check failed, the _CONTACT CHECK FAILED_ message is displayed;
*   The measurement is also displayed in the top panel of the program window under _INITIAL R;_
*   Click _THERMAL TRANSIENT_ to measure the thermal transient (_TRANSIENT V_) and thermal resistance (_TRANSIENT R_) ;
*   Click _FINAL RESISTANCE_ to measure final resistance (_FINAL R;_)
*   Success is designated by a green emoticon;
*   A failed measurement is designated on the instrument panel by a blinking prefix. For example, if Initial Resistance failed, the R preceding the measured value will blink with changing intensity.

# Automatically Measuring Initial and Final Resistances and Thermal Transients

*   Select the _MEASURE_ tab;
*   Connect a device to the instrument;
*   Set the part serial number in the _SERIAL NUMBER_ box;
*   Click _CLEAR MEASUREMENTS_ to clear data of the previous part;
*   Check _AUTOMATICALLY ADD PART_ if you wish to add the part to the parts list automatically upon completion of all measurements on this part.
*   Click _START_ under the _MEASURE ALL_ box.
*   Measurements will proceed as indicated in the progress bar at the bottom of the program window. When ended, a new part will be added to the part list and new values displayed on the top header.
*   Click _ABORT_ to abort the measurement.

# Reading and Saving the Thermal Transient Trace

*   Make a thermal transient measurement;
*   Select the _TRACE_ tab;
*   Click _READ TRACE_ to read the trace from the instrument and display the trace in the list view;
*   Click _SAVE TRACE_ to save the list to a file.
*   Click _CLEAR TRACE_ to clear the list.

# Listing and Saving Part Measurements

*   Select the _PARTS_ tab.
*   The _ADD_ button is enabled if all measurements completed successfully.
*   Click _ADD_ to add the last measurement to the list of measurements.
*   Repeat measuring another device and adding it to the list.
*   Click _SAVE_ to save the list to a file.
*   Click _CLEAR_ to remove all measurements from the list. This starts a new sample count.

# Alerts and Messages

The _ALERTS_ tab displays error and warnings. The _MESSAGES_ tab displays information messages. Whenever alerts are added to the _ALERTS_ tab, the program displays an _ALERTS_ enunciator on the top header. The enunciator is cleared when the _ALERTS_ tab is selected.

The tab label shows the number of new messages in parentheses thus indicating the existence of new information. Selecting the tab clears the count of new messages and hides the enunciator. Both the _ALERTS_ and _MESSAGES_ tabs display the last message at the top of the list.

# Program Log

The program log messages to a log file. The log file name changes every day. The location of the log file is displayed in the _MESSAGES_ tab when the program starts.

# Attributions

Copyright 2011 by Integrated Scientific Resources, Inc.  
This information is provided "AS IS" WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.

Licensed under the [ISR Fair End User Use License](http://bit.ly/FairEndUserUseLicense), Version 1.0 (the "ISR License"). You may obtain a copy of the ISR License at Unless required by applicable law or agreed to in writing, software distributed under the ISR License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the ISR License for the specific language governing permissions and limitations under the ISR License.