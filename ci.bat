@ECHO OFF

powershell Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
powershell Unblock-File .\ci\build.ps1
pushd .\ci

ECHO CI Tool

powershell .\build.ps1 -Verbosity Minimal -Command="%1"

popd

@ECHO ON