### Gets the OpenAPI document from the locally running Functions app.
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $FunctionAppPath = "bin/Debug/net6.0",

    [string]
    [Parameter(Mandatory=$false)]
    $BaseUri = "http://localhost:7071/api/",

    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("swagger.json", "openapi/v2.json", "openapi/v3.json", "swagger.yaml", "openapi/v2.yaml", "openapi/v3.yaml")]
    $Endpoint = "swagger.json",

    [string]
    [Parameter(Mandatory=$false)]
    $OutputPath = "generated",

    [string]
    [Parameter(Mandatory=$false)]
    $OutputFilename = "swagger.json",

    [int]
    [Parameter(Mandatory=$false)]
    $Delay = 30,

    [switch]
    [Parameter(Mandatory=$false)]
    $UseWindows,

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This downloads the OpenAPI document from the locally running Azure Functions app.
    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            [-FunctionAppPath <function app directory>] ``
            [-BaseUri <function app base URI>] ``
            [-Endpoint <endpoint for OpenAPI document>] ``
            [-OutputPath <output directory for generated OpenAPI document>] ``
            [-OutputFilename <OpenAPI document name>] ``
            [-Delay <delay in second between run function app and document generation>] ``
            [-UseWindows] ``
            [-Help]
    Options:
        -FunctionAppPath    Function app path. It can be the project directory or compiled app directory.
                            Default: 'bin/Debug/net6.0'
        -BaseUri            Function app base URI.
                            Default: 'http://localhost:7071/api/'
        -Endpoint           OpenAPI document endpoint.
                            Default: 'swagger.json'
        -OutputPath         Output directory to store the generated OpenAPI document.
                            Default: 'generated'
        -OutputFilename     Output filename for the generated OpenAPI document.
                            Default: 'swagger.json'
        -Delay              Delay in second between the function app run and document generation.
                            Default: 30
        -UseWindows         Switch that indicates using Windows OS.
        -Help               Show this message.
"

    Exit 0
}

# Show usage
$needHelp = $Help -eq $true
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}

# Check the function app execution path
$func = $(Get-Command func).Source
if ($UseWindows -eq $true) {
    $func = $func.Replace(".ps1", ".cmd")
}

pushd $FunctionAppPath

# Run the function app in the background
Start-Process -NoNewWindow "$func" @("start","--verbose","false")
Start-Sleep -s $Delay

$requestUri = "$($BaseUri.TrimEnd('/'))/$($Endpoint.TrimStart('/'))"
$filepath = "$($OutputPath.TrimEnd('/'))/$($OutputFilename.TrimStart('/'))"

if ($(Test-Path -Path "$($OutputPath.TrimEnd('/'))" -PathType Container) -eq $false) {
    New-Item -Path "$($OutputPath.TrimEnd('/'))" -ItemType Directory
}

# Download the OpenAPI document
Invoke-RestMethod -Method Get -Uri $requestUri | ConvertTo-Json -Depth 100 | Out-File -FilePath $filepath -Force

# Stop the function app
$process = $(get-Process -Name func)
if ($process -ne $null) {
    Stop-Process -Id $process.Id
}

popd
