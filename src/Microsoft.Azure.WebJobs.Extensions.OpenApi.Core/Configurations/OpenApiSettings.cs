using System;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the settings entity for OpenAPI metadata.
    /// </summary>
    [Obsolete("This class is obsolete from 0.5.0-preview. Use OpenApiConfigurationOptions instead", error: true)]
    public sealed class OpenApiSettings : IOpenApiConfigurationOptions
    {
        /// <inheritdoc />
        public OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "1.0.0",
            Title = "Azure Functions OpenAPI Extension",
        };

        /// <inheritdoc />
        public List<OpenApiServer> Servers { get; set; } = new List<OpenApiServer>();

        /// <inheritdoc />
        public OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V2;

        /// <inheritdoc />
        public bool IncludeRequestingHostName { get; set; }

        /// <inheritdoc />
        public bool ForceHttp { get; set; }

        /// <inheritdoc />
        public bool ForceHttps { get; set; }

        /// <inheritdoc />
        public List<IDocumentFilter> DocumentFilters { get; set; } = new List<IDocumentFilter>();
    }
}
