using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces classes implementing for OpenAPI metadata..
    /// </summary>
    public interface IOpenApiConfigurationOptions
    {
        /// <summary>
        /// Gets or sets the <see cref="OpenApiInfo"/> instance.
        /// </summary>
        OpenApiInfo Info { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="OpenApiServer"/> instances.
        /// </summary>
        List<OpenApiServer> Servers { get; set; }

        /// <summary>
        /// Gets or sets the OpenAPI spec version.
        /// </summary>
        OpenApiVersionType OpenApiVersion { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to exclude the requesting host or not.
        /// </summary>
        bool ExcludeRequestingHost { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to force the HTTP protocol or not.
        /// </summary>
        bool ForceHttp { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether to force the HTTPS protocol or not.
        /// </summary>
        bool ForceHttps { get; set; }
        
        /// <summary>
        /// Gets or sets the value indicating whether to use the FullName or not.
        /// </summary>
        bool UseFullName { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="IDocumentFilter"/> instances.
        /// </summary>
        List<IDocumentFilter> DocumentFilters { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IOpenApiHttpTriggerAuthorization"/> instance for Swagger endpoints.
        /// </summary>
        IOpenApiHttpTriggerAuthorization Security { get; set; }

        /// <summary>
        /// Gets or sets the value indicating OpenApiNamingStrategy.
        /// </summary>
        OpenApiNamingStrategy OpenApiNamingStrategy { get; set; } 
    }
}
