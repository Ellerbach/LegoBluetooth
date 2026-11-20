$ErrorActionPreference = 'Stop'
$fixture = Join-Path ([System.IO.Path]::GetTempPath()) ([guid]::NewGuid().ToString())
$updater = Join-Path $PSScriptRoot 'Update-NanoFrameworkDependencies.ps1'

function Invoke-RestMethod {
    param([string]$Uri)
    if ($Uri -ne 'https://api.nuget.org/v3-flatcontainer/test.package/index.json') {
        throw "Unexpected request: $Uri"
    }
    return @{ versions = @('1.0.0', '1.2.0', '2.0.0-preview.1') }
}

try {
    New-Item $fixture -ItemType Directory | Out-Null
    $configPath = Join-Path $fixture 'packages.config'
    $projectPath = Join-Path $fixture 'Test.nfproj'
    $config = '<packages><package id="Test.Package" version="1.0.0" targetFramework="netnano1.0" /></packages>'
    $project = '<Project><HintPath>..\packages\Test.Package.1.0.0\lib\Test.dll</HintPath><HintPath>..\packages\Test.Package.1.0.0\lib\Test.exe</HintPath></Project>'
    [System.IO.File]::WriteAllText($configPath, $config)
    [System.IO.File]::WriteAllText($projectPath, $project)

    & $updater -RepositoryRoot $fixture
    [xml]$updatedConfig = Get-Content $configPath -Raw
    $updatedProject = [System.IO.File]::ReadAllText($projectPath)
    if ($updatedConfig.packages.package.version -ne '1.2.0' -or
        $updatedProject -ne $project.Replace('1.0.0', '1.2.0')) {
        throw 'Stable update did not synchronize the manifest and all reference paths.'
    }

    $configAfterUpdate = [System.IO.File]::ReadAllText($configPath)
    & $updater -RepositoryRoot $fixture
    if ([System.IO.File]::ReadAllText($configPath) -ne $configAfterUpdate -or
        [System.IO.File]::ReadAllText($projectPath) -ne $updatedProject) {
        throw 'Running the updater twice changed already updated files.'
    }

    [System.IO.File]::WriteAllText($configPath, $config.Replace('1.0.0', '1.3.0'))
    [System.IO.File]::WriteAllText($projectPath, $project.Replace('1.0.0', '1.3.0'))
    & $updater -RepositoryRoot $fixture
    if ([System.IO.File]::ReadAllText($configPath) -notmatch 'version="1.3.0"') {
        throw 'The updater downgraded a package.'
    }

    [System.IO.File]::WriteAllText($configPath, $config)
    [System.IO.File]::WriteAllText($projectPath, '<Project />')
    $failed = $false
    try {
        & $updater -RepositoryRoot $fixture
    }
    catch {
        if ($_.Exception.Message -notlike 'Cannot find reference path*') { throw }
        $failed = $true
    }
    if (-not $failed -or [System.IO.File]::ReadAllText($configPath) -ne $config) {
        throw 'A missing reference path must fail without modifying the manifest.'
    }

    Write-Host 'Dependency updater tests passed.'
}
finally {
    Remove-Item $fixture -Recurse -Force
}
