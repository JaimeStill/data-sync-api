$datasync = ".\bin\Debug\net7.0\datasync.exe"

Write-Output "datasync proposal listen"

if (!(Test-Path -Path $datasync)) {
    & dotnet build
}

& $datasync proposal listen