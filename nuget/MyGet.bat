@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=0.1.0
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

nuget\nuget.exe restore src\Redux.sln

"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" src\Redux.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false

nuget\nuget.exe pack "nuget\ReduxNet.nuspec" -NoPackageAnalysis -verbosity detailed -Version %version% -p Configuration="%config%"