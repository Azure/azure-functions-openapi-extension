using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the settings entity for Open API metadata.
    /// </summary>
    public class OpenApiSettings
    {
        /// <summary>
        /// Gets or sets the <see cref="OpenApiInfo"/> instance.
        /// </summary>
        public virtual OpenApiInfo Info { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="OpenApiComponents"/> instance.
        /// </summary>
        public virtual OpenApiComponents Components { get; set; }

        // public virtual OpenApiSecurityRequirement Security { get; set; }
    }
}
