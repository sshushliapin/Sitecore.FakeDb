@echo off
setlocal
:PROMPT
SET /P AREYOUSURE=Are you sure you want to publish the NuGet packages (Y/[N])?
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END
  "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" Build.msbuild /p:Configuration=Release /t:Deploy /p:NoWarn=1591
:END
endlocal