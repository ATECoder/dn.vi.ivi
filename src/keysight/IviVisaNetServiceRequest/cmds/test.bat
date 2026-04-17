@echo off
:: Command Line: ResourceName, OptionBits
:: Option Bins:
:: 1 - Polling
:: 2 - Wait On Events
:: 4 - CallBack
..\IviVisaNetServiceRequest.exe TCPIP0::192.168.0.150::inst0::INSTR 7
pause
