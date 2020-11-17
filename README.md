# Azure Functions Open API Extension #

## Acknowledgement ##

* [Swagger UI](https://github.com/swagger-api/swagger-ui) version used for this library is [3.20.5](https://github.com/swagger-api/swagger-ui/releases/tag/v3.20.5) under the [Apache 2.0 license](https://opensource.org/licenses/Apache-2.0).


## Getting Started ##

* [**Enable Open API documents to your Azure Functions HTTP Trigger (Preview)**](docs/enable-open-api-endpoints.md): This document shows how to enable Open API extension on your Azure Functions applications and render Swagger UI, and Open API v2 and v3 documents on-the-fly. It's currently in preview.
* [**Azure Functions v1 Support**](docs/azure-functions-v1-support.md): This document shows how to support Azure Functions v1 runtime with this Open API extension.
* [**Integrating Open API-enabled Azure Functions to Azure API Management**](docs/integrate-with-apim.md): This document shows how to integrate the Azure Functions application with [Azure API Management](https://docs.microsoft.com/azure/api-management/api-management-key-concepts?WT.mc_id=dotnet_0000_juyoo), via this Open API extension.
<!-- * [**Integrating Open API-enabled Azure Functions to Power Platform**](docs/integrate-with-powerplatform.md): This document shows how to integrate the Azure Functions application with [Power Platform](https://powerplatform.microsoft.com/?WT.mc_id=dotnet_0000_juyoo), via this Open API extension. -->


## Sample Azure Function Apps with Open API Metadata Enabled ##

* [Function App v2 static](samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V2Static)
* [Function App v2 IoC](samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V2IoC)
* [Function App v3 static](samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static)
* [Function App v3 IoC](samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3IoC)


## Azure Functions V1 Support ##

This library supports Azure Functions V2 and onwards. If you still want to get your v1 app supported, find the [community contribution](https://github.com/aliencube/AzureFunctions.Extensions).


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
