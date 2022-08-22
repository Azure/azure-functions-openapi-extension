# Microsoft.Azure.Functions.Worker.Extensions.OpenApi #

![Build and Test](https://github.com/Azure/azure-functions-openapi-extension/workflows/Build%20and%20Test/badge.svg) [![](https://img.shields.io/nuget/dt/Microsoft.Azure.Functions.Worker.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.Functions.Worker.Extensions.OpenApi/) [![](https://img.shields.io/nuget/v/Microsoft.Azure.Functions.Worker.Extensions.OpenApi.svg)](https://www.nuget.org/packages/Microsoft.Azure.Functions.Worker.Extensions.OpenApi/)

This enables Azure Functions to render OpenAPI document and Swagger UI. Unlike the other extension, [Microsoft.Azure.WebJobs.Extensions.OpenApi](./openapi-in-proc.md), this supports out-of-process model running on .NET 5 and later.

> **NOTE**: This extension supports both [OpenAPI 2.0 (aka Swagger)](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/2.0.md) and [OpenAPI 3.0.1](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md) spec.


## Acknowledgement ##

* [Swagger UI](https://github.com/swagger-api/swagger-ui) version used for this library is [v3.44.0](https://github.com/swagger-api/swagger-ui/releases/tag/v3.44.0) under the [Apache 2.0 license](https://opensource.org/licenses/Apache-2.0).


## Issues? ##

While using this library, if you find any issue, please raise a ticket on the [Issue](https://github.com/Azure/azure-functions-openapi-extension/issues) page.


## Getting Started ##

For detailed getting started document, find this [Enable OpenAPI Endpoints on Azure Functions &ndash; Out-of-Process Model](enable-open-api-endpoints-out-of-proc.md) page.


## Advanced Configuration in General ##

If you look for the advanced configurations in general, please find the document, [Advanced Configurations for OpenAPI Extension](./openapi.md)


### Injecting `OpenApiConfigurationOptions` during Startup ###

You may want to inject the `OpenApiConfigurationOptions` instance during startup, through the `Program.cs` class. Here's the example:

```csharp
namespace MyFunctionApp
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureOpenApi()
                /* ⬇️⬇️⬇️ Add this ⬇️⬇️⬇️ */
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
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
                })
                /* ⬆️⬆️⬆️ Add this ⬆️⬆️⬆️ */
                .Build();

            host.Run();
        }
    }
}
```

### Injecting `OpenApiHttpTriggerAuthorization` during Startup ###

You may want to inject the `OpenApiHttpTriggerAuthorization` instance during startup, through the `Program.cs` class. Here's the example:

```csharp
namespace MyFunctionApp
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureOpenApi()
                /* ⬇️⬇️⬇️ Add this ⬇️⬇️⬇️ */
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IOpenApiHttpTriggerAuthorization>(_ =>
                            {
                                var auth = new OpenApiHttpTriggerAuthorization(async req =>
                                {
                                    var result = default(OpenApiAuthorizationResult);

                                    var authtoken = (string)req.Headers["Authorization"];
                                    if (authtoken.IsNullOrWhiteSpace())
                                    {
                                        result = new OpenApiAuthorizationResult()
                                        {
                                            StatusCode = HttpStatusCode.Unauthorized,
                                            ContentType = "text/plain",
                                            Payload = "Unauthorized",
                                        };

                                        return await Task.FromResult(result).ConfigureAwait(false);
                                    }

                                    return await Task.FromResult(result).ConfigureAwait(false);
                                });

                                return auth;
                            });
                })
                /* ⬆️⬆️⬆️ Add this ⬆️⬆️⬆️ */
                .Build();

            host.Run();
        }
    }
}
```


