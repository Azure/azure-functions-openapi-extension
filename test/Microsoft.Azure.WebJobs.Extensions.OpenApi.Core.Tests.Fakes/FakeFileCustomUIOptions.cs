using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeFileCustomUIOptions : DefaultOpenApiCustomUIOptions
    {
        public FakeFileCustomUIOptions(Assembly assembly)
            : base(assembly)
        {
        }

        public override string CustomStylesheetPath { get; set; } = "dist.fake.css";

        public override string CustomJavaScriptPath { get; set; } = "dist.fake.js";
    }
}
