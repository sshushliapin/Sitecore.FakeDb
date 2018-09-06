@echo off
setlocal
:PROMPT
SET /P AREYOUSURE=Are you sure you want to publish the NuGet packages (Y/[N])?
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END
  "%PROGRAMFILES(X86)%\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe" Build.msbuild /p:Configuration=Release /t:Deploy /p:NoWarn=1591 /p:RunCodeAnalysis=true;CodeAnalysisRuleSet=..\..\Sitecore.FakeDb.ruleset /fileLogger
:END
endlocal