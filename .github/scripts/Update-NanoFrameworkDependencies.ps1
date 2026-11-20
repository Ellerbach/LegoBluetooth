[CmdletBinding()]
param(
    [string]$RepositoryRoot = (Join-Path $PSScriptRoot '..\..')
)

$ErrorActionPreference = 'Stop'
$latestVersions = @{}
$projects = @(Get-ChildItem $RepositoryRoot -Filter '*.nfproj' -Recurse -File)
if ($projects.Count -eq 0) {
    throw "No nanoFramework projects found in $RepositoryRoot."
}

foreach ($project in $projects) {
    $configPath = Join-Path $project.DirectoryName 'packages.config'
    $configText = [System.IO.File]::ReadAllText($configPath)
    $projectText = [System.IO.File]::ReadAllText($project.FullName)
    [xml]$config = $configText
    $changed = $false

    foreach ($package in $config.packages.package) {
        $id = [string]$package.id
        $oldVersion = [string]$package.version
        if (-not $latestVersions.ContainsKey($id)) {
            $uri = "https://api.nuget.org/v3-flatcontainer/$($id.ToLowerInvariant())/index.json"
            $response = Invoke-RestMethod -Uri $uri
            $stableVersions = @($response.versions | Where-Object { $_ -notmatch '-' })
            if ($stableVersions.Count -eq 0) {
                throw "No stable NuGet versions found for $id."
            }
            $latestVersions[$id] = $stableVersions[-1]
        }

        $newVersion = $latestVersions[$id]
        if ([version]$newVersion -le [version]$oldVersion) {
            continue
        }

        $oldPath = "packages\$id.$oldVersion\"
        if (-not $projectText.Contains($oldPath)) {
            throw "Cannot find reference path for $id $oldVersion in $($project.FullName)."
        }
        $projectText = $projectText.Replace($oldPath, "packages\$id.$newVersion\")
        $pattern = '(<package\s+id="' + [regex]::Escape($id) + '"\s+version=")' + [regex]::Escape($oldVersion) + '"'
        $configText = [regex]::Replace($configText, $pattern, '${1}' + $newVersion + '"')
        Write-Host "$($project.BaseName): $id $oldVersion -> $newVersion"
        $changed = $true
    }

    if ($changed) {
        $encoding = New-Object System.Text.UTF8Encoding($false)
        [System.IO.File]::WriteAllText($configPath, $configText, $encoding)
        [System.IO.File]::WriteAllText($project.FullName, $projectText, $encoding)
    }
}
