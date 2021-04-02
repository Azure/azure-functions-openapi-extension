using System.Collections.Generic;

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
    }
}
