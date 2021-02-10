using System.Collections.Generic;

using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the collection of response headers embedded in the Open API response object.
    /// </summary>
    public abstract class OpenApiResponseHeaders
    {
        /// <summary>
        /// Gets or sets the collection of the <see cref="OpenApiHeader"/> instances.
        /// </summary>
        public abstract Dictionary<string, OpenApiHeader> Headers { get; set; }
    }
}
