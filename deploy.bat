@echo off
setlocal
:PROMPT
SET /P AREYOUSURE=Are you sure you want to publish the NuGet packages (Y/[N])?
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END
  C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe Build.msbuild /target:Deploy
:END
endlocal