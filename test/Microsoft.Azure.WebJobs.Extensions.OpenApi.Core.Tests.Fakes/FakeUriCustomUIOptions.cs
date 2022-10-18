using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    [OpenApiCustomUIOptionsIgnore]
    public class FakeUriCustomUIOptions : DefaultOpenApiCustomUIOptions
    {
        public FakeUriCustomUIOptions(Assembly assembly)
            : base(assembly)
        {
        }

        public override string CustomStylesheetPath { get; set; } =
            "https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc/dist/my-custom.css";

        public override string CustomJavaScriptPath { get; set; } =
            "https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc/dist/my-custom.js";
    }
}
