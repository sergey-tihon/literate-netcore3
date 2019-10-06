@echo off
cls

dotnet tool restore
dotnet paket restore

dotnet fsi docs/generate.fsx