# Microsoft.Azure.WebJobs.Extensions.OpenApi.Extensions.Configuration.AppSettings #

![Build and Test](https://github.com/Azure/azure-functions-openapi-extension/workflows/Build%20and%20Test/badge.svg) [![](https://img.shields.io/nuget/dt/Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings/) [![](https://img.shields.io/nuget/v/Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.svg)](https://www.nuget.org/packages/Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings/)

This is a base app settings environment variables deserialised. This MUST be inherited for use.

```csharp
public abstract class OpenApiAppSettingsBase : AppSettingsBase
{
    public OpenApiAppSettingsBase()
        : base()
    {
        ...
    }
    ...
}
```
