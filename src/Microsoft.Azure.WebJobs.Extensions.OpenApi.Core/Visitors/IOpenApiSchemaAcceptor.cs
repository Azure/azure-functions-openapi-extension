using System;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;

using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors
{
    /// <summary>
    /// This provides interfaces to the <see cref="OpenApiSchemaAcceptor"/> class.
    /// </summary>
    public interface IOpenApiSchemaAcceptor : IAcceptor
    {
        /// <summary>
        /// Gets the list of <see cref="OpenApiSchema"/> instances as key/value pair representing the root schemas.
        /// </summary>
        Dictionary<string, OpenApiSchema> RootSchemas { get; set; }

        /// <summary>
        /// Gets the list of <see cref="OpenApiSchema"/> instances as key/value pair.
        /// </summary>
        Dictionary<string, OpenApiSchema> Schemas { get; set; }

        /// <summary>
        /// Gets the list of <see cref="Type"/> objects.
        /// </summary>
        Dictionary<string, Type> Types { get; set; }

        /// <summary>
        /// Gets and sets the list of types processed with their full name if exist.
        /// </summary>
        List<string> TypesAcceptedWithFullName { get; set; }
    }
}
