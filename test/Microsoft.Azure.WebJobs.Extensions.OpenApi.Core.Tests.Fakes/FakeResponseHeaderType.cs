using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeResponseHeaderType : IOpenApiResponseHeaderType
    {
        /// <inheritdoc/>
        public Dictionary<string, OpenApiHeader> Headers { get; set; } = new Dictionary<string, OpenApiHeader>()
        {
            { "x-fake-header", new OpenApiHeader() { Description = "Fake header", Schema = new OpenApiSchema() { Type = "string" } } }
        };
    }
}
