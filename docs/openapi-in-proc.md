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

For detailed getting started document, find this [Enable OpenAPI Endpoints on Azure Functions &ndash; In-Process Model](enable-open-api-endpoints-in-proc.md) page.


## Advanced Configuration in General ##

If you look for the advanced configurations in general, please find the document, [Advanced Configurations for OpenAPI Extension](./openapi.md)


### Injecting `OpenApiConfigurationOptions` during Startup ###

You may want to inject the `OpenApiConfigurationOptions` instance during startup, through the `Startup.cs` class. Here's the example:

```csharp
[assembly: FunctionsStartup(typeof(MyFunctionApp.Startup))]
namespace MyFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            /* ⬇️⬇️⬇️ Add this ⬇️⬇️⬇️ */
            builder.Services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
            {
                var options = new OpenApiConfigurationOptions()
                {
                    Info = new OpenApiInfo()
                    {
                        Version = "1.0.0",
                        Title = "Swagger Petstore",
                        Description = "This is a sample server Petstore API designed by [http://swagger.io](http://swagger.io).",
                        TermsOfService = new Uri("https://github.com/Azure/azure-functions-openapi-extension"),
                        Contact = new OpenApiContact()
                        {
                            Name = "Enquiry",
                            Email = "azfunc-openapi@microsoft.com",
                            Url = new Uri("https://github.com/Azure/azure-functions-openapi-extension/issues"),
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "MIT",
                            Url = new Uri("http://opensource.org/licenses/MIT"),
                        }
                    },
                    Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                    OpenApiVersion = OpenApiVersionType.V2,
                    IncludeRequestingHostName = true,
                    ForceHttps = false,
                    ForceHttp = false,
                };

                return options;
            });
            /* ⬆️⬆️⬆️ Add this ⬆️⬆️⬆️ */
        }
    }
}
```

### Injecting `OpenApiHttpTriggerAuthorization` during Startup ###

You may want to inject the `OpenApiHttpTriggerAuthorization` instance during startup, through the `Startup.cs` class. Here's the example:

```csharp
[assembly: FunctionsStartup(typeof(MyFunctionApp.Startup))]
namespace MyFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            /* ⬇️⬇️⬇️ Add this ⬇️⬇️⬇️ */
            builder.Services.AddSingleton<IOpenApiHttpTriggerAuthorization, MyOpenApiHttpTriggerAuthorization>();
            /* ⬆️⬆️⬆️ Add this ⬆️⬆️⬆️ */
        }
    }
}
```
