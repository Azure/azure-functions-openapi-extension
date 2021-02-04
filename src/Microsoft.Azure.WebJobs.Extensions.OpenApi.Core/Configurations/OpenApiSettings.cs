using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the settings entity for Open API metadata.
    /// </summary>
    public sealed class OpenApiSettings : IOpenApiConfigurationOptions
    {
        /// <inheritdoc />
        public OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "1.0.0",
            Title = "Azure Functions Open API Extension",
        };
    }
}
