$NATIVE_SRC = "https://github.com/surban/PLplot/releases/download/b1/plplot_windows.zip"


$ErrorActionPreference = "Stop"
Invoke-WebRequest $NATIVE_SRC -OutFile windows.zip 
Expand-Archive windows.zip -DestinationPath "$PSScriptRoot/Native" -Force
Remove-Item -Force windows.zip
