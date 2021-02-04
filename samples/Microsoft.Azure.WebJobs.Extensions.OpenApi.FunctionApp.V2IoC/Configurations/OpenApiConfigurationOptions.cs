using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V2IoC.Configurations
{
    public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
    {
        public OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "2.0.0",
            Title = "Open API Sample on Azure Functions (IoC)",
            Description = "A sample API that runs on Azure Functions (IoC) 3.x using Open API specification.",
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
}
