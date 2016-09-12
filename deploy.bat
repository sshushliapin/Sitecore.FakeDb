@echo off
setlocal
:PROMPT
SET /P AREYOUSURE=Are you sure you want to publish the NuGet packages (Y/[N])?
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END
  %WINDIR%/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe Build.msbuild /p:Configuration=Release /t:Versions /p:NoWarn=1591 /p:TargetFrameworkVersion=v4.5.2 /p:SupportedSitecoreVersions=SC820 /p:PatchDatabase=true
  if %errorlevel% neq 0 exit /b %errorlevel%
  %WINDIR%/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe Build.msbuild /p:Configuration=Release /t:Deploy /p:NoWarn=1591
:END
endlocal