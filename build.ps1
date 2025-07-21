dotnet publish -c Release -r win-x64 --self-contained true`
  /p:PublishSingleFile=true `
  /p:PublishReadyToRun=true `
  /p:IncludeAllContentForSelfExtract=true `
  /p:PublishDir="..\resources\"

Write-Host "buildCompleted!"
