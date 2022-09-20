# OpenAPI Generator from Azure Functions #

This action generates an OpenAPI document from the given Azure Functions app that is either locally running within the GitHub Actions workflow pipeline or remotely running on Azure.


## Inputs ##

All input parameters are optional, and each has its default value.

* `functionAppPath`: Path for the compiled Function app. default: `bin/Debug/net6.0`
* `requestUri`: URI to generate the OpenAPI document. default: `http://localhost:7071/api/swagger.json`
* `documentPath`: Path to store the generated OpenAPI document. default: `generated`
* `documentName`: Name of the OpenAPI document. default: `swagger.json`
* `delay`: Delay in seconds to start the Function app. default: `10`
* `isRemote`: Value indicating whether the Function app runs remotely or not. default: `false`


## Outputs ##

* `generated`: Path to the generated OpenAPI document


## Example Usage ##

```yml
steps:
- name: Checkout the repository
  uses: actions/checkout@v2

- name: Setup .NET SDK 6 LTS
  uses: actions/setup-dotnet@v1
  with:
    dotnet-version: '6.x'

- name: Restore NuGet packages
  shell: pwsh
  run: |
    dotnet restore .

- name: Build solution
  shell: pwsh
  run: |
    dotnet build . -c Debug -v minimal

- name: Generate OpenAPI document
  id: oai
  uses: azure-functions-openapi-extension/actions/build-openapi@v1
  with:
    functionAppPath: 'bin/Debug/net6.0'
    requestUri: 'http://localhost:7071/api/swagger.json'
    documentPath: 'generated'
    documentName: 'swagger.json'
    delay: '10'
    isRemote: 'false'

- name: Check generated OpenAPI document
    shell: pwsh
    run: |
      echo "Generated Document: ${{ steps.oai.outputs.generated }}"

      Get-Content -Path ${{ steps.oai.outputs.generated }}
```

