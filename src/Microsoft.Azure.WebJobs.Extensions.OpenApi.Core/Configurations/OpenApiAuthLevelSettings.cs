using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the environment variable settings entity for OpenAPI document auth level.
    /// </summary>
    public class OpenApiAuthLevelSettings
    {
        /// <summary>
        /// Gets or sets the <see cref="OpenApiAuthLevelType"/> value for OpenAPI document rendering endpoints.
        /// </summary>
        public virtual OpenApiAuthLevelType? Document { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="OpenApiAuthLevelType"/> value for Swagger UI page rendering endpoints.
        /// </summary>
        public virtual OpenApiAuthLevelType? UI { get; set; }
    }
}
