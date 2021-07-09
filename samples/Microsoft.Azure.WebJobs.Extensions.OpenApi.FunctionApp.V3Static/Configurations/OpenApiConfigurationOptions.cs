using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static.Configurations
{
    public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
    {
        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = GetOpenApiDocVersion(),
            Title = GetOpenApiDocTitle(typeof(OpenApiConfigurationOptions)),
            Description = "A sample API that runs on Azure Functions (STATIC) 3.x using OpenAPI specification.",
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

        public override OpenApiVersionType OpenApiVersion { get; set; } = GetOpenApiVersion();
    }
}
