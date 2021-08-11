# Microsoft.Azure.WebJobs.Extensions.OpenApi #

![Build and Test](https://github.com/Azure/azure-functions-openapi-extension/workflows/Build%20and%20Test/badge.svg) [![](https://img.shields.io/nuget/dt/Microsoft.Azure.WebJobs.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi/) [![](https://img.shields.io/nuget/v/Microsoft.Azure.WebJobs.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi/)

This enables Azure Functions to render OpenAPI document and Swagger UI. The more details around the Swagger UI on Azure Functions can be found on this [blog post](https://techcommunity.microsoft.com/t5/apps-on-azure/create-and-publish-openapi-enabled-azure-functions-with-visual/ba-p/2381067?WT.mc_id=dotnet-0000-juyoo).

> [!NOTE]
> This extension supports both [OpenAPI 2.0 (aka Swagger)](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md) and [OpenAPI 3.0.1](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md) spec.


## Looking for Isolated Worker Support? ##

Please find this document, [Microsoft.Azure.Functions.Worker.Extensions.OpenApi](./openapi-out-of-proc.md).


## Acknowledgement ##

* [Swagger UI](https://github.com/swagger-api/swagger-ui) version used for this library is [v3.44.0](https://github.com/swagger-api/swagger-ui/releases/tag/v3.44.0) under the [Apache 2.0 license](https://opensource.org/licenses/Apache-2.0).


## Issues? ##

While using this library, if you find any issue, please raise a ticket on the [Issue](https://github.com/Azure/azure-functions-openapi-extension/issues) page.


## Getting Started ##

For detailed getting started document, find this [Enable OpenAPI Endpoints on Azure Functions (Preview) &ndash; In-Process Model](enable-open-api-endpoints-in-proc.md) page.


## Configuration ##

For the extension's advanced configuration, it expects the following config keys.


### Configure Authorization Level ###

As a default, all endpoints to render Swagger UI and OpenAPI documents have the authorisation level of `AuthorizationLevel.Anonymous`. However, if you want to secure those endpoints, change their authorisation level to `AuthorizationLevel.Function` and pass the API Key through either request header or querystring parameter. This can be done through the environment variables. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

```json
{
  "Values": {
    "OpenApi__ApiKey": "",
    "OpenApi__AuthLevel__Document": "Anonymous",
    "OpenApi__AuthLevel__UI": "Anonymous"
  }
}
```

You can have granular controls to both Swagger UI and OpenAPI documents by setting the authorisation level to `Anonymous`, `User`, `Function`, `System` or `Admin`. Make sure that you MUST provide the `OpenApi__AuthKey` value, if you choose the `OpenApi__AuthLevel__Document` value other than `Anonymous`. Otherwise, it will throw an error.

> [!NOTE]
> Both Swagger UI and OpenAPI document pages are allowed `Anonymous` access by default.


### Configure Swagger UI Visibility ###

You may want to only enable the Swagger UI page during the development time, and disable the page when publishing it to Azure. You can configure an environment variable to enable/disable the Swagger UI page. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

```json
{
  "Values": {
    "OpenApi__HideSwaggerUI": "false"
  }
}
```

If you set the `OpenApi__HideSwaggerUI` value to `true`, the Swagger UI page won't be showing up, and you will see the 404 error.

> [!NOTE]
> The default value for `OpenApi__HideSwaggerUI` is `false`.


### Configure OpenAPI Information ###

As a default, the OpenAPI document automatically generated provides a minimum set of information like:

* OpenAPI Version: `2.0`
* OpenAPI Document Title: `OpenAPI Document on Azure Functions`
* OpenAPI Document Version: `1.0.0`

You may want to provide consumers with more details by implementing the `IOpenApiConfigurationOptions` interface or inheriting the `DefaultOpenApiConfigurationOptions` class. On the other hand, you can use the following environment variables to avoid the app from being recompiled and redeployed. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

```json
{
  "Values": {
    "OpenApi__Version": "v2",
    "OpenApi__DocTitle": "Azure Functions OpenAPI Extension",
    "OpenApi__DocVersion": "1.0.0"
  }
}
```


### Configure Custom Base URLs ###

There's a chance that you want to expose the UI and OpenAPI document through [Azure API Management](https://docs.microsoft.com/azure/api-management/api-management-key-concepts?WT.mc_id=github-0000-juyoo) or load balancing services like [Azure Front Door](https://docs.microsoft.com/azure/frontdoor/front-door-overview?WT.mc_id=github-0000-juyoo). You can configure an environment variable to add them. Here's the sample `local.settings.json` file. The other values are omitted for brevity.

```json
{
  "Values": {
    "OpenApi__HostNames": "https://contoso.com/api/,https://fabrikam.com/api/"
  }
}
```

> **NOTE**: This multiple hostnames support feature only works with OpenAPI 3.x, not OpenAPI 2.x.
