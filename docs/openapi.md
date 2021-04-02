# Microsoft.Azure.WebJobs.Extensions.OpenApi #

![Build and Test](https://github.com/Azure/azure-functions-openapi-extension/workflows/Build%20and%20Test/badge.svg) [![](https://img.shields.io/nuget/dt/Microsoft.Azure.WebJobs.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi/) [![](https://img.shields.io/nuget/v/Microsoft.Azure.WebJobs.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi/)

This enables Azure Functions to render OpenAPI document and Swagger UI. The more details around the Swagger UI on Azure Functions can be found on this [blog post](https://devkimchi.com/2019/02/02/introducing-swagger-ui-on-azure-functions/).

> **NOTE**: This extension supports both [OpenAPI 2.0 (aka Swagger)](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md) and [OpenAPI 3.0.1](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md) spec.


## Acknowledgement ##

* [Swagger UI](https://github.com/swagger-api/swagger-ui) version used for this library is [v3.44.0](https://github.com/swagger-api/swagger-ui/releases/tag/v3.44.0) under the [Apache 2.0 license](https://opensource.org/licenses/Apache-2.0).


## Issues? ##

While using this library, if you find any issue, please raise a ticket on the [Issue](https://github.com/Azure/azure-functions-openapi-extension/issues) page.


## Getting Started ##

### Install NuGet Package ###

In order for your Azure Functions app to enable OpenAPI capability, download the following NuGet package into your Azure Functions project.

```bash
dotnet add <PROJECT> package Microsoft.Azure.WebJobs.Extensions.OpenApi
```


### Change Authorization Level ###

As a default, all endpoints to render Swagger UI and OpenAPI documents have the authorisation level of `AuthorizationLevel.Anonymous`.


```csharp
[FunctionName(nameof(OpenApiHttpTrigger.RenderSwaggerDocument))]
[OpenApiIgnore]
public static async Task<IActionResult> RenderSwaggerDocument(
    [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "swagger.{extension}")] HttpRequest req,
    string extension,
    ILogger log)
{
    ...
}

[FunctionName(nameof(OpenApiHttpTrigger.RenderOpenApiDocument))]
[OpenApiIgnore]
public static async Task<IActionResult> RenderOpenApiDocument(
    [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "openapi/{version}.{extension}")] HttpRequest req,
    string version,
    string extension,
    ILogger log)
{
    ...
}

[FunctionName(nameof(OpenApiHttpTrigger.RenderSwaggerUI))]
[OpenApiIgnore]
public static async Task<IActionResult> RenderSwaggerUI(
    [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "swagger/ui")] HttpRequest req,
    ILogger log)
{
    ...
}
```

However, if you want to secure those endpoints, change their authorisation level to `AuthorizationLevel.Functions` and pass the API Key through either request header or querystring parameter.

> **NOTE**: To change this authorisation level, you MUST install the `Microsoft.Azure.WebJobs.Extensions.OpenApi.Core` package, instead of `Microsoft.Azure.WebJobs.Extensions.OpenApi`, and copy those three files from the source codes to your application:
> 
> * `templates/OpenApiEndpoints/IOpenApiHttpTriggerContext.cs`
> * `templates/OpenApiEndpoints/OpenApiHttpTrigger.cs`
> * `templates/OpenApiEndpoints/OpenApiHttpTriggerContext.cs`

```csharp
[FunctionName(nameof(OpenApiHttpTrigger.RenderSwaggerDocument))]
[OpenApiIgnore]
public static async Task<IActionResult> RenderSwaggerDocument(
    [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger.{extension}")] HttpRequest req,
    string extension,
    ILogger log)
{
    ...
}

[FunctionName(nameof(OpenApiHttpTrigger.RenderOpenApiDocument))]
[OpenApiIgnore]
public static async Task<IActionResult> RenderOpenApiDocument(
    [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "openapi/{version}.{extension}")] HttpRequest req,
    string version,
    string extension,
    ILogger log)
{
    ...
}

[FunctionName(nameof(OpenApiHttpTrigger.RenderSwaggerUI))]
[OpenApiIgnore]
public static async Task<IActionResult> RenderSwaggerUI(
    [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger/ui")] HttpRequest req,
    ILogger log)
{
    ...
}
```


### Configure App Settings Key ###

This key is only required if:

* The Function app is deployed to Azure, and
* The OpenAPI related endpoints has the `AuthorizationLevel` value other than `Anonymous`.

If the above conditions are met, add the following key to your `local.settings.json` or App Settings blade on Azure.

* `OpenApi__ApiKey`: either the host key value or the master key value.

> **NOTE**: It is NOT required if your OpenAPI related endpoints are set to the authorisation level of `Anonymous`.


## OpenAPI Metadata Configuration ##

To generate an OpenAPI document, [OpenApiInfo object](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md#infoObject) needs to be defined. ***It's totally optional***, but if you want, you can implement the `IOpenApiConfigurationOptions` interface within your Azure Functions project to provide OpenAPI metadata like below:

```csharp
public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
{
    public OpenApiInfo Info { get; set; } = new OpenApiInfo()
    {
        Version = "1.0.0",
        Title = "OpenAPI Document on Azure Functions",
        Description = "HTTP APIs that run on Azure Functions using OpenAPI specification.",
        TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
        Contact = new OpenApiContact()
        {
            Name = "Contoso",
            Email = "azfunc-openapi@contoso.com",
            Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues"),
        },
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("http://opensource.org/licenses/MIT"),
        }
    };
}
```
