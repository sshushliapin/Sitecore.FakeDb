@echo off
setlocal
:PROMPT
SET /P AREYOUSURE=Are you sure you want to publish the NuGet packages (Y/[N])?
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END
  %WINDIR%/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe Build.msbuild /p:Configuration=Release /t:Deploy
:END
endlocal