# Microsoft.Azure.WebJobs.Extensions.OpenApi #

![Build and Test](https://github.com/Azure/azure-functions-openapi-extension/workflows/Build%20and%20Test/badge.svg) [![](https://img.shields.io/nuget/dt/Microsoft.Azure.WebJobs.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi/) [![](https://img.shields.io/nuget/v/Microsoft.Azure.WebJobs.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi/)

This enables Azure Functions to render OpenAPI document and Swagger UI. The more details around the Swagger UI on Azure Functions can be found on this [blog post](https://devkimchi.com/2019/02/02/introducing-swagger-ui-on-azure-functions/).

> **NOTE**: This extension supports both [OpenAPI 2.0 (aka Swagger)](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md) and [OpenAPI 3.0.1](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md) spec.


## Acknowledgement ##

* [Swagger UI](https://github.com/swagger-api/swagger-ui) version used for this library is [v3.44.0](https://github.com/swagger-api/swagger-ui/releases/tag/v3.44.0) under the [Apache 2.0 license](https://opensource.org/licenses/Apache-2.0).


## Issues? ##

While using this library, if you find any issue, please raise a ticket on the [Issue](https://github.com/Azure/azure-functions-openapi-extension/issues) page.


## Getting Started ##

For detailed getting started document, find this [Enable OpenAPI Endpoints on Azure Functions (Preview)](enable-open-api-endpoints.md) page.


### Configure Authorization Level ###

As a default, all endpoints to render Swagger UI and OpenAPI documents have the authorisation level of `AuthorizationLevel.Anonymous`. However, if you want to secure those endpoints, change their authorisation level to `AuthorizationLevel.Functions` and pass the API Key through either request header or querystring parameter. This can be done through the environment variables. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

```json
{
  "Values": {
    "OpenApi__ApiKey": "",
    "OpenApi__AuthLevel__Document": "Anonymous",
    "OpenApi__AuthLevel__UI": "Anonymous"
  }
}
```

You can have granular controls to both Swagger UI and OpenAPI documents by setting the authorisation level to `Anonymous`, `User`, `Function`, `System` or `Admin`. Make sure that you MUST provide the `OpenApi__AuthKey` value, if you choose the `OpenApi__AuthLevel__Document` value other than `Anonymous`. Otherwise, it will throw an error. Both Swagger UI and OpenAPI document pages are allowed `Anonymous` access by default.


### Configure Swagger UI Visibility ###

You may want to only enable the Swagger UI page during the development time, and disable the page when publishing it to Azure. You can configure an environment variable to enable/disable the Swagger UI page. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

```json
{
  "Values": {
    "OpenApi__HideSwaggerUI": "false"
  }
}
```

If you set the `OpenApi__HideSwaggerUI` value to `true`, the Swagger UI page won't be showing up, and you will see the 404 error. The default value is `false`.
