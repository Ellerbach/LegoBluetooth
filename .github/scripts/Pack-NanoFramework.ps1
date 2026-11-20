[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$Version,
    [string]$OutputDirectory = (Join-Path $PSScriptRoot '..\..\artifacts\packages')
)

$ErrorActionPreference = 'Stop'
$projectDirectory = (Resolve-Path (Join-Path $PSScriptRoot '..\..\LegoBluetooth')).Path
[xml]$spec = Get-Content (Join-Path $projectDirectory 'LegoBluetooth.nuspec') -Raw
[xml]$packages = Get-Content (Join-Path $projectDirectory 'packages.config') -Raw

# Read the restored dependency versions rather than duplicating them in the nuspec.
foreach ($package in $packages.packages.package) {
    $dependency = $spec.CreateElement('dependency')
    $dependency.SetAttribute('id', $package.id)
    $dependency.SetAttribute('version', $package.version)
    $spec.SelectSingleNode('/package/metadata/dependencies').AppendChild($dependency) | Out-Null
}

New-Item $OutputDirectory -ItemType Directory -Force | Out-Null
$OutputDirectory = (Resolve-Path $OutputDirectory).Path
$specPath = Join-Path $OutputDirectory 'nanoFramework.LegoBluetooth.nuspec'
try {
    $spec.Save($specPath)
    & nuget pack $specPath -BasePath $projectDirectory -Version $Version -OutputDirectory $OutputDirectory -NonInteractive
    if ($LASTEXITCODE -ne 0) {
        throw "NuGet pack failed with exit code $LASTEXITCODE."
    }
}
finally {
    if (Test-Path $specPath) {
        Remove-Item $specPath
    }
}
