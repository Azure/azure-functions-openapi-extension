using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static.Configurations
{
    public class OpenApiCustomUIOptions : DefaultOpenApiCustomUIOptions
    {
        public OpenApiCustomUIOptions(Assembly assembly)
            : base(assembly)
        {
        }

        public override string CustomStylesheetPath { get; } = "dist.my-custom.css";

        public override string CustomJavaScriptPath { get; } = "dist.my-custom.js";
    }
}
