set PATH=%PATH%;C:\Program Files (x86)\MSBuild\12.0\Bin

msbuild /t:rebuild /p:configuration=debug

del /q D:\ServiceRoot\EnqueueLogService

xcopy /y bin\Debug D:\ServiceRoot\EnqueueLogService

pause
