### This add the GitHub Codespaces capability to your local.settings.json
Param(
    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "
    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) [-Help]

    Options:
        -Help:      Show this message.

"

    Exit 0
}

# Show usage
if ($Help -eq $true) {
    Show-Usage
    Exit 0
}

function Get-CodespaceUrl {
    param (
        [string] $CodespaceName
    )

    $codespaceUrl = "https://$($CodespaceName)-7071.githubpreview.dev/api"

    return $codespaceUrl
}

# Get .csproj files
$csprojs = Get-ChildItem -Path $env:CODESPACE_VSCODE_FOLDER -Filter *.csproj -Recurse

# Get local.settings.json files
$localSettingsJsons = @()
$csprojs | ForEach-Object {
    $isInProcFunctionApp = $(Get-Content -Path $_.FullName -Raw) -like '*Microsoft.NET.Sdk.Functions*'
    $isOutOfProcFunctionApp = $(Get-Content -Path $_.FullName -Raw) -like '*Microsoft.Azure.Functions.Worker.Sdk*'
    $isFunctionApp = ($isInProcFunctionApp -eq $true) -or ($isOutOfProcFunctionApp -eq $true)
    if ($isFunctionApp -eq $false) {
        return
    }

    if ($(Test-Path -Path "$($_.Directory.FullName)/local.settings.json") -eq $true) {
        $localSettingsJsons += Get-ChildItem -Path $_.Directory.FullName -Filter local.settings.json

        return
    }

    if ($(Test-Path -Path "$($_.Directory.FullName)/local.settings.sample.json") -eq $true) {
        Copy-Item -Path "$($_.Directory.FullName)/local.settings.sample.json" -Destination "$($_.Directory.FullName)/local.settings.json"
        $localSettingsJsons += Get-ChildItem -Path $_.Directory.FullName -Filter local.settings.json

        return
    }

    @{ IsEncrypted = $false; Values = @{ AzureWebJobsStorage = "UseDevelopmentStorage=true"; FUNCTIONS_WORKER_RUNTIME = "dotnet"; } } | `
        ConvertTo-Json -Depth 10 | `
        Out-File -FilePath "$($_.Directory.FullName)/local.settings.json" -Force
    $localSettingsJsons += Get-ChildItem -Path $_.Directory.FullName -Filter local.settings.json
}

# Update local.settings.json files
$localSettingsJsons | ForEach-Object {
    # Get the app settings details.
    $appSettings = Get-Content -Path $_.FullName | ConvertFrom-Json

    # Add OpenApi__ForceHttps to local.settings.json.
    if ($appSettings.Values.OpenApi__ForceHttps -eq $null) {
        $appSettings.Values | Add-Member -NotePropertyName OpenApi__ForceHttps -NotePropertyValue "true"
    } else {
        $appSettings.Values.OpenApi__ForceHttps = "true"
    }

    # Add OpenApi__RunOnCodespaces to local.settings.json.
    if ($appSettings.Values.OpenApi__RunOnCodespaces -eq $null) {
        $appSettings.Values | Add-Member -NotePropertyName OpenApi__RunOnCodespaces -NotePropertyValue "true"
    } else {
        $appSettings.Values.OpenApi__RunOnCodespaces = "true"
    }

    # Add OpenApi__HostNames to local.settings.json.
    $url = Get-CodespaceUrl -CodespaceName $env:CODESPACE_NAME
    if ($appSettings.Values.OpenApi__HostNames -eq $null) {
        $appSettings.Values | Add-Member -NotePropertyName OpenApi__HostNames -NotePropertyValue $url
    } else {
        $hostNames = $appSettings.Values.OpenApi__HostNames -split ","
        $hostNames = [System.Collections.ArrayList]$hostNames

        if ($url -ne $null) {
            $hostNames.Remove($url)
        }
        $hostNames.Insert(0, $url)

        $appSettings.Values.OpenApi__HostNames = $($hostNames -join ",").Trim(',')
    }

    # Overwrite the existing local.settings.json
    $appSettings | ConvertTo-Json -Depth 100 | Out-File -Path $_.FullName -Force
}
