# IRECKONU assignment

Build a web app with the features described below.
Backend:
*   C# .NET Core web API
*   Import the file via web API, expect the file to be very large in production
*   Transform data into a logical model
*	Data-storage (all of these, so Database and JSON file
*   Database (MS SQL or MongoDB)
* 	JSON file on the disk
Data: File: https://goo.gl/tJWo1f
 
# How to use

## Prerequisites
* .NET Core SDK 2.2.107
* PowerShell

## Before runing scripts
* Run cmd 
* Navigate to the app root folder
* ...

## Run without Visual Stidio
Next command will build application and run it with JSON storage.
```
     .\build.ps1 -Target Run_JsonStorage
```
Next command will build application and run it with MS-SQL storage.
```
     .\build.ps1 -Target Run_SqlStorage
```
Next command will build unit tests and run them.
```
     .\build.ps1 -Target RunUnitTests
```

# What was used
## Tech Stack:
* ASP.NTET Core
* EF
* MS SQL Server
* Cake
* Automapper
* NUnit
* NSubstitute
* Swagger

## Principles:
* SOLID
* Dependency Injection
* Repository pattern
