# GTA V Realism Script Suite

This is a RPH (Rage Plugin Hook) Script I had designed for the GTAV Realism Project. The main creators of that project have since gone inactive. I am no longer interested in maintaining the script suite in their absence and have decided to publicly release it.

## Description

This is the Grand Theft Auto 5 Realism Plugin Suite intended for use with RAGE Plugin Hook (available at https://www.lcpdfr.com/downloads/gta5mods/g17media/7792-lspd-first-response/ ) and the "Realism" mod (not currently available). 
This plugin suite aims to add features to GTA 5 to increase the sense of realism and introduce a whole new challenge.

### Authors Notes
This plugin suite is being released AS-IS and is no longer maintained. If you would like to contribute you may open your own fork of the code. 
If you would like to release this script on another GTAV website we ask that you acknowledge Genesis and the GTAV Realism Team in your credits and make substantial changes to the existing code.


## Getting Started

### Dependencies

* Visual Studio 2022
* Rage Plugin Hook
* Rage Native UI
* Newtonsoft JSON

### Installing for GTAV

* Step 1: Install Grand Theft Auto 5 
* Step 2: Download LSPDFR Manual Installation with Rage Plugin Hook (at https://www.lcpdfr.com/downloads/gta5mods/g17media/7792-lspd-first-response/)
* Step 3: Extract the Manual Installation to your Grand Theft Auto 5 folder. 
  * Step 3a (Optional): Delete the LSPDFR Folder. Delete “GTAV\Plugins\LSPD First Response.dll”  (if you wish to keep LSPDFR installed and only run Realism or LSPDFR Plugin you can open Rage Plugin Hook while holding the SHIFT key, with this option screen open you can enable or disable specific RPH Plugins)
* Step 4: Start Grand Theft Auto 5 with Rage Plugin Hook.
* Step 5: Close Grand Theft Auto 5.
* Step 6: Move the "Realism.dll" and "Realism" folder to your plugins folder. (Located in your Grand Theft Auto 5 install. If you do not see a Plugins folder, please repeat the previous steps.)
* Step 7: Move "Newtonsoft.Json.dll" and "RageNativeUI.dll" into the root of your Grand Theft Auto 5 install directory. (Depending on your Operating System settings the ".dll" portion of the file name may be hidden. That is not a problem.)



### Executing program

Start GTAV with Rage Plugin Hook. Realism Script Suite should load with the game.

### Compiling

* Use Visual Studio 2022
* Reference the current versions of:
  * Rage Plugin Hook
  * Rage Native UI
  * Newtonsoft Json
* Compile

## Authors

Contributors names and contact info

Genesis (Genesis1753 @ Discord)


## Version History


V1.03.04: 
* Engine Controller Tweaks
* Fixed Amanda being assumed as a cop in the shoplifting Side Mission

V1.03.02: 
* Made Police Blips Toggle-able
* Added notification when config is corrupt and replaced

V1.03.00: 
* Engine Controller Added

V1.02.20: 
* More Garage bug fixes

V1.02.18: 
* Fixed null garage crash

V1.02.17: 
* Added keybind for menu key
* Fixed Keybinds not saving

V1.02.16: 
* Fixed attachement system sniper scope selection

V1.02.15: 
* Fixed ragdoll bug when entering vehicles

V1.02.14: 
* Fixed dog crash

V1.02.12: 
* Original Release
