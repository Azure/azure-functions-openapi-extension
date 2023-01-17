# Generic CI/CD Pipeline Support #

You can run either PowerShell script or bash shell script to generate the OpenAPI document within your own CI/CD pipeline including Azure DevOps or GitHub Actions.


## PowerShell ##

Run the PowerShell script remotely:

```powershell
& $([Scriptblock]::Create($(Invoke-RestMethod https://aka.ms/azfunc-openapi/generate-openapi.ps1))) `
    -FunctionAppPath    <function app directory> `
    -BaseUri            <function app base URI> `
    -Endpoint           <endpoint for OpenAPI document> `
    -OutputPath         <output directory for generated OpenAPI document> `
    -OutputFilename     <OpenAPI document name> `
    -Delay              <delay in second between run function app and document generation> `
    -UseWindows         <switch indicating whether to use Windows OS or not>
```

Alternatively, you can manually download the PowerShell script and include it to your codebase: [Get-OpenApiDocument.ps1](./actions/Get-OpenApiDocument.ps1)

```powershell
./actions/Get-OpenApiDocument.ps1 `
    -FunctionAppPath    <function app directory> `
    -BaseUri            <function app base URI> `
    -Endpoint           <endpoint for OpenAPI document> `
    -OutputPath         <output directory for generated OpenAPI document> `
    -OutputFilename     <OpenAPI document name> `
    -Delay              <delay in second between run function app and document generation> `
    -UseWindows         <switch indicating whether to use Windows OS or not>
```

For more details, run `Get-OpenApiDocument.ps1 -Help`


## Bash Shell ##

Run the bash shell script remotely:

```bash
curl -fsSL https://aka.ms/azfunc-openapi/generate-openapi.sh \
    | bash -s -- \
        -p|--functionapp-path   <function app directory> \
        -u|--base-uri           <function app base URI> \
        -e|--endpoint           <endpoint for OpenAPI document> \
        -o|--output-path        <output directory for generated OpenAPI document> \
        -f|--output-filename    <OpenAPI document name> \
        -d|--delay              <delay in second between run function app and document generation>
```

Alternatively, you can manually download the PowerShell script and include it to your codebase: [get-openapi-document.sh](./actions/get-openapi-document.sh)

```bash
./actions/get-openapi-document.sh \
    -p|--functionapp-path   <function app directory> \
    -u|--base-uri           <function app base URI> \
    -e|--endpoint           <endpoint for OpenAPI document> \
    -o|--output-path        <output directory for generated OpenAPI document> \
    -f|--output-filename    <OpenAPI document name> \
    -d|--delay              <delay in second between run function app and document generation>
```

For more details, run `get-openapi-document.sh --help`
