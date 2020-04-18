@echo off
cd Gameiki.Patcher
dotnet msbuild "Gameiki.Patcher.csproj"
xcopy /s ".\bin\debug\Gameiki.Patcher.exe" "..\..\..\refs" /Y
cd ..\Gameiki
dotnet msbuild "Gameiki.csproj"