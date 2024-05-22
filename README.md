# OBSControl
OBS integration for BONELAB.

Allows you to control OBS from within BONELAB.

# THIS WILL NEVER WORK ON QUEST!

# OBS MUST BE OPEN BEFORE YOU OPEN BONELAB!

## Installation
1. Download the latest release from the [releases page](https://github.com/WeatherElectric/OBSControl/releases) or [Thunderstore](https://thunderstore.io/c/bonelab/p/SoulWithMae/OBSControl/)
2. Extract the zip file
3. Place the "Mods" folder in your BONELAB directory
4. Place the "UserLibs" folder in your BONELAB directory

## Usage
### OBS Setup
1. Open OBS
2. Go to `Tools` -> `WebSockets Server Settings`
3. Check `Enable WebSockets Server`
4. Check that the port is `4455`, if it is not, remember the number it is for later
5. Click "Show Connect Info"
6. Copy the password
7. Hit apply and close the window
### MelonLoader Setup
1. Run the game once so the preferences can generate
2. Open the preferences file
3. Replace `REPLACEME` in the password field with the password you copied from OBS
4. Make sure that the URL ends with the port number that OBS shows
5. Save the file
### In-Game Usage
* Double tap the menu button to save a replay. The hand it listens for can be configured.
* BoneMenu has most of the controls.