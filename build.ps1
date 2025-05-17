$ErrorActionPreference = "Stop"

dotnet clean

dotnet publish -c Release -r win-x64 --self-contained true `
  /p:PublishSingleFile=true `
  /p:PublishReadyToRun=true `
  /p:TargetFramework="net8.0-windows" `
  /p:Platform="x64" `
  /p:PublishDir="..\resources\"

Write-Host "buildCompleted!"
