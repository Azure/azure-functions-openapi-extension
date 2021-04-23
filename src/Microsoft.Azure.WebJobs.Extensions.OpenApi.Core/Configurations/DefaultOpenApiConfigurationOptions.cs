using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the options entity for OpenAPI metadata configuration.
    /// </summary>
    public class DefaultOpenApiConfigurationOptions : IOpenApiConfigurationOptions
    {
        /// <inheritdoc />
        public virtual OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "1.0.0",
            Title = "Azure Functions OpenAPI Extension",
        };

        /// <inheritdoc />
        public virtual List<OpenApiServer> Servers { get; set; } = GetHostNames();

        /// <inheritdoc />
        public virtual OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V2;

        /// <inheritdoc />
        public virtual bool IncludeRequestingHostName { get; set; } = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development";

        /// <summary>
        /// Gets the list of hostnames.
        /// </summary>
        /// <returns>Returns the list of hostnames.</returns>
        protected static List<OpenApiServer> GetHostNames()
        {
            var servers = new List<OpenApiServer>();
            var collection = Environment.GetEnvironmentVariable("OpenApi__HostNames");
            if (collection.IsNullOrWhiteSpace())
            {
                return servers;
            }

            var hostnames = collection.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(p => new OpenApiServer() { Url = p });

            servers.AddRange(hostnames);

            return servers;
        }
    }
}
