using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This provides interfaces classes implementing.
    /// </summary>
    public interface IOpenApiConfigurationOptions
    {
        /// <summary>
        /// Gets or sets the <see cref="OpenApiInfo"/> instance.
        /// </summary>
        OpenApiInfo Info { get; set; }
    }
}
