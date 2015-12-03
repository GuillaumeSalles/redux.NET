@echo Off

nuget restore src/Redux.sln -verbosity detailed
msbuild src/Redux.sln /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll" /p:Configuration=Release
nunit-console src/Redux.Tests/bin/Release/Redux.Tests.dll

nuget restore examples/todomvc/Redux.TodoMvc.sln -verbosity detailed
msbuild examples/todomvc/Redux.TodoMvc.sln /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll" /p:Configuration=Release