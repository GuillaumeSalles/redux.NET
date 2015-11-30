@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=0.1.0
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

nuget restore src/Redux.sln -verbosity detailed
msbuild src/Redux.sln /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll" /p:Configuration=Release
nunit-console src/Redux.Tests/bin/Release/Redux.Tests.dll

nuget restore examples/todomvc/Redux.TodoMvc.sln -verbosity detailed
msbuild examples/todomvc/Redux.TodoMvc.sln /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll" /p:Configuration=Release

nuget pack "nuget\Redux.NET.nuspec" -NoPackageAnalysis -verbosity detailed -Version %version% -p Configuration="Release"
nuget pack "nuget\Redux.NET.DevTools.nuspec" -NoPackageAnalysis -verbosity detailed -Version %version% -p Configuration="Release"
