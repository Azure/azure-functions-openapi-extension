using System.Collections.Generic;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeUriCustomUIOptions : DefaultOpenApiCustomUIOptions
    {
        public FakeUriCustomUIOptions(Assembly assembly)
            : base(assembly)
        {
        }

        public override string CustomStylesheetPath { get; } =
            "https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc/dist/my-custom.css";

        public override string CustomJavaScriptPath { get; } =
            "https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static/dist/my-custom.js";

        public override IEnumerable<string> CustomFaviconMetaTags { get; } = new List<string>()
        {
            "<link rel=\"icon\" type=\"image/png\" href=\"https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/src/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core/dist/favicon-16x16.png\" sizes=\"16x16\" />",
            "<link rel=\"icon\" type=\"image/png\" href=\"https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/src/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core/dist/favicon-32x32.png\" sizes=\"32x32\" />"

        };
    }
}
