using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to the <see cref="DocumentHelper"/> class.
    /// </summary>
    public interface IDocumentHelper
    {
        (OpenApiPaths paths, List<MethodInfo> methods) GetOpenApiPathAndMethodInfos(Assembly assembly, NamingStrategy strategy, VisitorCollection collection, OpenApiVersionType version);

            /// <summary>
        /// Gets the <see cref="OpenApiPathItem"/> instance.
        /// </summary>
        /// <param name="path">HTTP endpoint as a path.</param>
        /// <param name="paths"><see cref="OpenApiPaths"/> instance.</param>
        /// <returns><see cref="OpenApiPathItem"/> instance.</returns>
        OpenApiPathItem GetOpenApiPath(string path, OpenApiPaths paths);

        /// <summary>
        /// Gets the collection of <see cref="OpenApiSecurityRequirement"/> instances.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <returns>Collection of <see cref="OpenApiSecurityRequirement"/> instance.</returns>
        List<OpenApiSecurityRequirement> GetOpenApiSecurityRequirement(MethodInfo element, NamingStrategy namingStrategy = null);

        /// <summary>
        /// Gets the <see cref="OpenApiRequestBody"/> instance.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance to process parameters.</param>
        /// <param name="version">OpenAPI spec version.</param>
        /// <returns><see cref="OpenApiRequestBody"/> instance.</returns>
        OpenApiRequestBody GetOpenApiRequestBody(MethodInfo element, NamingStrategy namingStrategy, VisitorCollection collection, OpenApiVersionType version = OpenApiVersionType.V2);

        /// <summary>
        /// Gets the <see cref="OpenApiResponses"/> instance.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <returns><see cref="OpenApiResponses"/> instance.</returns>
        [Obsolete("This method is obsolete from 2.0.0. Use GetOpenApiResponses instead", error: true)]
        OpenApiResponses GetOpenApiResponseBody(MethodInfo element, NamingStrategy namingStrategy = null);

        /// <summary>
        /// Gets the <see cref="OpenApiResponses"/> instance.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance to process parameters.</param>
        /// <param name="version">OpenAPI spec version.</param>
        /// <returns><see cref="OpenApiResponses"/> instance.</returns>
        OpenApiResponses GetOpenApiResponses(MethodInfo element, NamingStrategy namingStrategy, VisitorCollection collection, OpenApiVersionType version = OpenApiVersionType.V2);

        /// <summary>
        /// Gets the collection of <see cref="OpenApiSchema"/> instances.
        /// </summary>
        /// <param name="elements">List of <see cref="MethodInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance to add types to schema.</param>
        /// /// <returns>Collection of <see cref="OpenApiSchema"/> instance.</returns>
        Dictionary<string, OpenApiSchema> GetOpenApiSchemas(List<MethodInfo> elements, NamingStrategy namingStrategy, VisitorCollection collection);

        /// <summary>
        /// Gets the collection of <see cref="OpenApiSecurityScheme"/> instances.
        /// </summary>
        /// <returns>Collection of <see cref="OpenApiSecurityScheme"/> instance.</returns>
        [Obsolete("This method is obsolete from 3.2.0. Use GetOpenApiSecuritySchemes(List<MethodInfo> elements, NamingStrategy namingStrategy = null) instead", error: true)]
        Dictionary<string, OpenApiSecurityScheme> GetOpenApiSecuritySchemes();

        /// <summary>
        /// Gets the collection of <see cref="OpenApiSecurityScheme"/> instances.
        /// </summary>
        /// <param name="elements">List of <see cref="MethodInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <returns>Collection of <see cref="OpenApiSecurityScheme"/> instance.</returns>
        Dictionary<string, OpenApiSecurityScheme> GetOpenApiSecuritySchemes(List<MethodInfo> elements, NamingStrategy namingStrategy = null);

        /// <summary>
        /// Filters the route constraints.
        /// </summary>
        /// <param name="route">Route parameter value.</param>
        /// <returns>Returns the route value without the constraints.</returns>
        string FilterRouteConstraints(string route);
    }
}
