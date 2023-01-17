# Azure Functions OpenAPI Extension #

| Out-of-Proc Worker | In-Proc Worker |
| :----------------: | :------------: |
| [![](https://img.shields.io/nuget/dt/Microsoft.Azure.Functions.Worker.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.Functions.Worker.Extensions.OpenApi/) [![](https://img.shields.io/nuget/v/Microsoft.Azure.Functions.Worker.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.Functions.Worker.Extensions.OpenApi/) | [![](https://img.shields.io/nuget/dt/Microsoft.Azure.WebJobs.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi/) [![](https://img.shields.io/nuget/v/Microsoft.Azure.WebJobs.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi/) |


## Acknowledgement ##

* [Swagger UI](https://github.com/swagger-api/swagger-ui) version used for this library is [v3.44.0](https://github.com/swagger-api/swagger-ui/releases/tag/v3.44.0) under the [Apache 2.0 license](https://opensource.org/licenses/Apache-2.0).
* This extension supports the OpenAPI spec version of [v2.0](https://github.com/OAI/OpenAPI-Specification/blob/main/versions/2.0.md) and [v3.0.1](https://github.com/OAI/OpenAPI-Specification/blob/main/versions/3.0.1.md).


## Getting Started ##

* [**Enable OpenAPI documents to your Azure Functions HTTP Trigger**](docs/enable-open-api-endpoints.md): This document shows how to enable OpenAPI extension on your Azure Functions applications and render Swagger UI, and OpenAPI v2 and v3 documents on-the-fly.
  * [**Microsoft.Azure.Functions.Worker.Extensions.OpenApi**](docs/openapi-out-of-proc.md)
  * [**Microsoft.Azure.WebJobs.Extensions.OpenApi**](docs/openapi-in-proc.md)
  * [**Microsoft.Azure.WebJobs.Extensions.OpenApi.Core**](docs/openapi-core.md)
* [**Securing Azure Functions Endpoints through OpenAPI Auth**](docs/openapi-auth.md): This document shows many various scenarios to add authN features including the built-in features and OAuth2 auth flows.
* ~~[**Azure Functions v1 Support**](docs/azure-functions-v1-support.md): This document shows how to support Azure Functions v1 runtime with this OpenAPI extension.~~
* [**Integrating OpenAPI-enabled Azure Functions to Azure API Management**](docs/integrate-with-apim.md): This document shows how to integrate the Azure Functions application with [Azure API Management](https://docs.microsoft.com/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet_0000_juyoo), via this OpenAPI extension.
<!-- * [**Integrating OpenAPI-enabled Azure Functions to Power Platform**](docs/integrate-with-powerplatform.md): This document shows how to integrate the Azure Functions application with [Power Platform](https://powerplatform.microsoft.com/?WT.mc_id=dotnet_0000_juyoo), via this OpenAPI extension. -->
* [**Generic CI/CD Pipeline Support](./docs/generic-cicd-pipeline-support.md): This document shows how to generate the OpenAPI document within a CI/CD pipeline, using either PowerShell or bash shell script.


## GitHub Actions Support ##

If you are using GitHub Actions as your preferred CI/CD pipeline, you can run the GitHub Action into your workflow to automatically generate the OpenAPI document. Find more details at the [Build OpenAPI](./actions/build-openapi/) action page.


## GitHub Codespaces Support ##

If you want to run your Azure Functions app on GitHub Codespaces, you might want to accommodate `local.settings.json`. The following PowerShell script may help you for the accommodation:

```bash
# Update local.settings.json
pwsh -c "Invoke-RestMethod https://aka.ms/azfunc-openapi/add-codespaces.ps1 | Invoke-Expression"
```


## Sample Azure Function Apps with OpenAPI Document Enabled ##

Here are sample apps using the project references:

* [Function App out-of-proc worker](samples/Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc)
* [Function App in-proc worker](samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc)


~~## Azure Functions V1 Support ##~~

~~This library supports Azure Functions V3 and onwards. If you still want to get your v1 and v2 runtime app supported, find the [community contribution](https://github.com/aliencube/AzureFunctions.Extensions) or the [proxy feature](docs/azure-functions-v1-support.md).~~


## Known Issues ##

### Missing .dll Files ###

Due to the Azure Functions Runtime limitation, sometimes some of .dll files are removed while publishing the function app. In this case, try the following workaround with your function app `.csproj` file.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  ...
  <PropertyGroup>
    ...
    <_FunctionsSkipCleanOutput>true</_FunctionsSkipCleanOutput>
  </PropertyGroup>
  ...
</Project>
```

### Empty Swagger UI When Deployed through Azure Pipelines ###

* Workaround: [#306](https://github.com/Azure/azure-functions-openapi-extension/issues/306)


### Swagger UI Error When Empty Project Referenced ###

* Workaround: [#302](https://github.com/Azure/azure-functions-openapi-extension/issues/302#issuecomment-961791941)


## Issues? ##

While using this library, if you find any issue, please raise an issue on the [Issue](https://github.com/Azure/azure-functions-openapi-extension/issues) page.


## Contributing ##

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
