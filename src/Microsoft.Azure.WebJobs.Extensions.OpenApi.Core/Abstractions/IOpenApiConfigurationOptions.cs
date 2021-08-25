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
        /// Gets or sets the value indicating whether to include the requesting hostname or not.
        /// </summary>
        bool IncludeRequestingHostName { get; set; }

        /// <summary>
        /// Gets or sets a class that can be used to add additional parameters to the generated open api
        /// </summary>
        IAdditionalOpenApiParameter AdditionalParameters { get; set; }

        /// <summary>
        /// Gets or sets a class that can be used to add a request body if the default document helper body parameter is null
        /// </summary>
        IAdditionalOpenApiRequestBody AdditionalOpenApiRequestBody { get; set; }

        /// <summary>
        /// Gets or sets a class that can be used to add a request body if the default document helper response parameter is null
        /// </summary>
        IAdditionalOpenApiResponse AdditionalOpenApiResponse { get; set; }
    }
}
