using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3Static.ResponseHeaders
{
    public class DummyResponseHeaderType : IOpenApiResponseHeaderType
    {
        public Dictionary<string, OpenApiHeader> Headers { get; set; } = new Dictionary<string, OpenApiHeader>()
        {
            { "x-dummy-header", new OpenApiHeader() { Description = "Dummy header", Schema = new OpenApiSchema() { Type = "string" } } }
        };
    }
}
