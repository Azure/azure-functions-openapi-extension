using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static.Configurations
{
    public class OpenApiCustomUIOptions : DefaultOpenApiCustomUIOptions
    {
        public OpenApiCustomUIOptions(Assembly assembly)
            : base(assembly)
        {
        }

        public override string CustomStylesheetPath { get; set; } = "dist.custom.css";

        public override string CustomJavaScriptPath { get; set; } = "dist.custom.js";

        public override Task<string> GetStylesheetAsync(string filepath = "dist.custom.css")
        {
            return base.GetStylesheetAsync(filepath);
        }

        public override Task<string> GetJavaScriptAsync(string filepath = "dist.custom.js")
        {
            return base.GetJavaScriptAsync(filepath);
        }
    }
}
