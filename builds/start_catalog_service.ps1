$servicePath = "$PSScriptRoot\..\src\Services\NT.CatalogService.Api"

sl $servicePath
# & "C:\Program Files\dotnet\dotnet.exe" restore
# & "C:\Program Files\dotnet\dotnet.exe" build
& "C:\Program Files\dotnet\dotnet.exe" run

$HOST.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown") | OUT-NULL
$HOST.UI.RawUI.Flushinputbuffer()