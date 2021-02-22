using System.Collections.Generic;

using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the options entity for Open API metadata configuration.
    /// </summary>
    public class DefaultOpenApiConfigurationOptions : IOpenApiConfigurationOptions
    {
        /// <inheritdoc />
        public virtual OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "1.0.0",
            Title = "Azure Functions Open API Extension",
        };

        /// <inheritdoc />
        public virtual List<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>();
    }
}
