param(
    [string]
    [Parameter()]
    [ValidateSet("proposal", "package")]
    $Command = "proposal"
)

$datasync = ".\bin\Debug\net7.0\datasync.exe"

Write-Output "datasync $Command listen"

if (!(Test-Path -Path $datasync)) {
    & dotnet build
}

& $datasync $Command listen