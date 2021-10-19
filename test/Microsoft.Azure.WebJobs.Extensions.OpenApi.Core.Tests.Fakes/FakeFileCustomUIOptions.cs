using System.Collections.Generic;
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

        public override string CustomStylesheetPath { get; } = "dist.fake.css";

        public override string CustomJavaScriptPath { get; } = "dist.fake.js";

        public override IEnumerable<string> CustomFaviconMetaTags { get; } = new List<string>()
        {
            "<link rel=\"icon\" type=\"image/png\" href=\"dist.fake-favicon-16x16.png\" sizes=\"16x16\" />",
            "<link rel=\"icon\" type=\"image/png\" href=\"dist.fake-favicon-32x32.png\" sizes=\"32x32\" />"

        };
    }
}
