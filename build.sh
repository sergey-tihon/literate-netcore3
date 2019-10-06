#!/bin/bash

dotnet tool restore
dotnet paket restore

#COREHOST_TRACE=1 
#export COREHOST_TRACE
#COREHOST_TRACEFILE=./log.txt
#export COREHOST_TRACEFILE

dotnet fsi docs/generate.fsx