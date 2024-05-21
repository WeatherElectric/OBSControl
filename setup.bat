@echo off

REM Check for admin privileges
NET SESSION >nul 2>&1
if %errorLevel% neq 0 (
    echo This script requires administrator privileges. Please run it as administrator.
    pause
    exit /b 1
)

REM Change the working directory so it does not create the symlinks in System32
cd /d "%~dp0"

set /p bl_path=Where is BONELAB installed? 

if not exist "%bl_path%" (
    echo Path '%bl_path%' does not exist! Do you have permissions to read it? Did you enter the path wrong?
    exit /b 1
)

set "ml_path=%bl_path%\MelonLoader"
if not exist "%ml_path%" (
    echo MelonLoader folder was not found! Have you installed MelonLoader?
    exit /b 1
)

set "mod_path=%bl_path%\Mods"
if not exist "%mod_path%" (
    echo Mods folder was not found! Have you launched MelonLoader at least once?
    exit /b 1
)

echo Linking files and folders...
if not exist ".\Links" (
    mkdir ".\Links"
)

mklink /D ".\Links\Mods" "%mod_path%"
mklink /D ".\Links\MelonLoader" "%ml_path%"
mklink /D ".\Links\Game" "%bl_path%"

echo Finding BONELAB executable...

for %%F in ("%bl_path%\BONELAB*.exe") do (
    echo Found '%%~nxF'
    mklink ".\Links\BONELAB.exe" "%%~fF"
    goto :found
)

echo No BONELAB executable found!
exit /b 1

:found
exit /b 0