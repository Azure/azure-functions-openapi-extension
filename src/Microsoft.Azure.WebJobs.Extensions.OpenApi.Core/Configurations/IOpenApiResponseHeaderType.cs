using System.Collections.Generic;

using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This provides interfaces to classes implementing Open API response object.
    /// </summary>
    public interface IOpenApiResponseHeaderType
    {
        /// <summary>
        /// Gets or sets the collection of the <see cref="OpenApiHeader"/> instances.
        /// </summary>
        Dictionary<string, OpenApiHeader> Headers { get; set; }
    }
}
