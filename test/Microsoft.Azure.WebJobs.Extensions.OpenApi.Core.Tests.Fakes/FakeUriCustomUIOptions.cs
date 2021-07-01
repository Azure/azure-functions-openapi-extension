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
            "https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static/dist/my-custom.css";

        public override string CustomJavaScriptPath { get; } =
            "https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/samples/Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static/dist/my-custom.js";
    }
}
