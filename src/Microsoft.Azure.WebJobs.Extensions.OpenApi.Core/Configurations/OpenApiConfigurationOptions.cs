using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the options entity for OpenAPI metadata configuration.
    /// </summary>
    [OpenApiConfigurationOptionsIgnore]
    public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
    {
        /// <inheritdoc />
        public virtual OpenApiInfo Info { get; set; } = new OpenApiInfo();

        /// <inheritdoc />
        public virtual List<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>();

        /// <inheritdoc />
        public virtual OpenApiVersionType OpenApiVersion { get; set; }

        /// <inheritdoc />
        public virtual bool IncludeRequestingHostName { get; set; }

        /// <inheritdoc />
        public virtual bool ForceHttp { get; set; }

        /// <inheritdoc />
        public virtual bool ForceHttps { get; set; }

        /// <inheritdoc />
        public virtual List<IDocumentFilter> DocumentFilters { get; set; } = new List<IDocumentFilter>();
    }
}
