using System.Collections.Generic;

using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core
{
    /// <summary>
    /// This provides interfaces to classes implementing OpenAPI response object.
    /// </summary>
    public interface IOpenApiCustomResponseHeader
    {
        /// <summary>
        /// Gets or sets the collection of the <see cref="OpenApiHeader"/> instances.
        /// </summary>
        Dictionary<string, OpenApiHeader> Headers { get; set; }
    }
}
