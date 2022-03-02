using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.DocumentFilters;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Configurations
{
    public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
    {
        public const string DocVersion = "1.0.0";
        public const string DocTitle = "OpenAPI on Azure Functions";
        public const string DocDescription = "OpenAPI document rendering on top of Azure Functions app";
        public const string TermsOfService = "https://github.com/Azure/azure-functions-openapi-extension";
        public const string ContactName = "Contoso";
        public const string ContactEmail = "azfunc-openapi@contoso.com";
        public const string ContactUrl = "https://github.com/Azure/azure-functions-openapi-extension/issues";
        public const string LicenseName = "MIT";
        public const string LicenseUrl = "http://opensource.org/licenses/MIT";
        public const OpenApiVersionType OpenApiSpecVersion = OpenApiVersionType.V3;

        public OpenApiConfigurationOptions()
        {
            this.DocumentFilters.Add(new RewriteDescriptionDocumentFilter());
        }

        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = DocVersion,
            Title = DocTitle,
            Description = DocDescription,
            TermsOfService = new Uri(TermsOfService),
            Contact = new OpenApiContact()
            {
                Name = ContactName,
                Email = ContactEmail,
                Url = new Uri(ContactUrl),
            },
            License = new OpenApiLicense()
            {
                Name = LicenseName,
                Url = new Uri(LicenseUrl),
            }
        };

        public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiSpecVersion;
    }
}
