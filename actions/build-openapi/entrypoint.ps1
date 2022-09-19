Param(
    [string]
    [Parameter(Mandatory=$false)]
    $FunctionAppPath = "bin/Debug/net6.0",

    [string]
    [Parameter(Mandatory=$false)]
    $RequestUri = "http://localhost:7071/api/swagger.json",

    [string]
    [Parameter(Mandatory=$false)]
    $DocumentPath = "generated",

    [string]
    [Parameter(Mandatory=$false)]
    $DocumentName = "swagger.json",

    [string]
    [Parameter(Mandatory=$false)]
    $Delay = "30",

    [string]
    [Parameter(Mandatory=$false)]
    $IsRemote = "false"
)

$artifactPath = "$($env:GITHUB_WORKSPACE.TrimEnd('/'))/$($FunctionAppPath.Trim('/'))"
$generatedPath = "$($env:GITHUB_WORKSPACE.TrimEnd('/'))/$($DocumentPath.Trim('/'))"
$generatedFile = "$($DocumentPath.Trim('/'))/$($DocumentName)"
$outputPath = "$($env:GITHUB_WORKSPACE.TrimEnd('/'))/$generatedFile"

$remote = [boolean]$IsRemote
if ($remote -eq $false) {
    pushd $artifactPath

    Start-Process -NoNewWindow func @("start","--verbose","false")

    if (($Delay -eq "") -or ($Delay -eq $null)) {
        $Delay = "30"
    }

    $sleep = [int]$Delay

    if ($sleep -gt 0) {
        Start-Sleep -Seconds $sleep
    }

    popd
}

mkdir $generatedPath

Invoke-RestMethod -Method Get -uri $requestUri | ConvertTo-Json -Depth 100 -Compress | Out-File $outputPath -Encoding utf8 -Force

Write-Output "::set-output name=generated::$generatedFile"

Remove-Variable -Name artifactPath
Remove-Variable -Name generatedPath
Remove-Variable -Name generatedFile
Remove-Variable -Name outputPath
Remove-Variable -Name remote
Remove-Variable -Name sleep
