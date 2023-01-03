# Memory Dump

## Issue in tool download
- running dotnet cli cmd: `dotnet tool install --global dotnet-symbol`
- Error: `Unable to load the service index for source https:\\...`
    - download tool from nuget but package source is troubling, eg. "BigData" on error logs :
- goto C:\Users\kamahale\AppData\Roaming\NuGet
- open NuGet.Config
- comment the package source. for eg "BigData" and save
- rerun the command, once successful goto NuGet.Config and uncomment the line, save

## Memory dump

- create folder [customFolder] => eg. investigateMemoryDump:

### Get ProcessId, PID

- powershell => `tasklist`
- powershell => `get-process`
- taskmanager => admin mode => details

### dotnet-counters tool

- https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-counters
- in [customFolder]
- download using curl
  - `curl -L https://aka.ms/dotnet-counters/win-x64 -o dotnet-counters`

- cd dotnet-counters
- list of all dotnet processes: `.\dotnet-counters.exe ps`
- details of dotnet process: `.\dotnet-counters.exe monitor -p [processId]`
  - details of cpu/memory/threadpool/heap/queue
  - you can watch the application performance live

### procdump

- https://learn.microsoft.com/en-us/sysinternals/downloads/procdump
- in [customFolder]
- download using curl
  - `curl -L https://aka.ms/dotnet-dump/win-x64 -o dotnet-dump`

- copy symbols ie .pdb files from the application you are investigating, eg.DistributedCacheApplication.pdb
  - location: \AzureDotnetSolution\DistributedCacheApplication\bin\Debug\net6.0
- capture memory dump full and mini of the processId
  - `.\procdump.exe -mm -ma [processId]`
  - `.\procdump.exe -mm -ma 14060`
  - you will get 2 dump files mini and full
  - you can also collect dump => `.\procdump.exe collect -p 14060`, default is full

- load symbols for a dump file
  - `dotnet symbol --debugging --host-only [dumpFile]`
  - eg. `dotnet symbol --debugging --host-only .\DistributedCacheApplication.exe_230103_114035_Full.dmp`


### Open in Visual Studio 2022

- if dump is on different machine? copy that to the devbox where you have VS2022
- open dump file in VS2022
- Thread pool
  - click debug with managed only
  - Debug - window - parallel stacks => can check threads utilization
    - You can hover over the large number of threads and call stack. double click the API
- Memory leak
  - in memory leak investigation. You want to capture dump with good state vs bad state
  - 2 dumps. use `.\procdump.exe collect -p 14060` cmd. eg memoryleak1 and memoryleak2
  - Click debug managed memory
  - in heap view -> compare with baseline
- Diagnostic Analysis of dump

### Analyze using dotnet dump cmd
- memory leaks
  - https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-dump
    - install tool
    - `dotnet-dump analyze [dumpName]`
    - `eeheap -gc` => `dumpheap -stat`
    - `dumpheap -mt [ObjectNumberId]`
    - `dumpheap -mt [ObjectNumberId]`
    - `db -c 1024 [ObjectNumberId]`