using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Configurations
{
    public class OpenApiCunsomUIOptions : DefaultOpenApiCustomUIOptions
    {
        public OpenApiCunsomUIOptions(Assembly assembly)
            : base(assembly)
        {
        }

        public override string CustomStylesheetPath { get; } =
            "https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static/dist/my-custom.css";

        public override string CustomJavaScriptPath { get; } =
            "https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static/dist/my-custom.js";

        public override IEnumerable<string> CustomFaviconMetaTags { get; } = new List<string>()
        {
            "<link rel=\"icon\" type=\"image/png\" href=\"dist.fake-favicon-16x16.png\" sizes=\"16x16\" />",
            "<link rel=\"icon\" type=\"image/png\" href=\"dist.fake-favicon-32x32.png\" sizes=\"32x32\" />"
        };
    }
}
