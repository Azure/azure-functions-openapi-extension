using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;

using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to the <see cref="DocumentHelper"/> class.
    /// </summary>
    public interface IDocumentHelper
    {
        /// <summary>
        /// Gets the list of HTTP triggers.
        /// </summary>
        /// <param name="assembly">Assembly of Azure Function instance.</param>
        /// <returns>List of <see cref="MethodInfo"/> instances representing HTTP triggers.</returns>
        List<MethodInfo> GetHttpTriggerMethods(Assembly assembly);

        /// <summary>
        /// Gets the <see cref="FunctionNameAttribute"/> from the method.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="FunctionNameAttribute"/> instance.</returns>
        FunctionNameAttribute GetFunctionNameAttribute(MethodInfo element);

        /// <summary>
        /// Gets the <see cref="HttpTriggerAttribute"/> from the parameters of the method.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <returns><see cref="HttpTriggerAttribute"/> instance.</returns>
        HttpTriggerAttribute GetHttpTriggerAttribute(MethodInfo element);

        /// <summary>
        /// Gets the HTTP trigger endpoint.
        /// </summary>
        /// <param name="function"><see cref="FunctionNameAttribute"/> instance.</param>
        /// <param name="trigger"><see cref="HttpTriggerAttribute"/> instance.</param>
        /// <returns>Function HTTP endpoint.</returns>
        string GetHttpEndpoint(FunctionNameAttribute function, HttpTriggerAttribute trigger);

        /// <summary>
        /// Gets the HTTP verb.
        /// </summary>
        /// <param name="trigger"><see cref="HttpTriggerAttribute"/> instance.</param>
        /// <returns><see cref="OperationType"/> value.</returns>
        OperationType GetHttpVerb(HttpTriggerAttribute trigger);

        /// <summary>
        /// Gets the <see cref="OpenApiPathItem"/> instance.
        /// </summary>
        /// <param name="path">HTTP endpoint as a path.</param>
        /// <param name="paths"><see cref="OpenApiPaths"/> instance.</param>
        /// <returns><see cref="OpenApiPathItem"/> instance.</returns>
        OpenApiPathItem GetOpenApiPath(string path, OpenApiPaths paths);

        /// <summary>
        /// Gets the <see cref="OpenApiOperation"/> instance.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <param name="function"><see cref="FunctionNameAttribute"/> instance.</param>
        /// <param name="verb"><see cref="OperationType"/> value.</param>
        /// <returns><see cref="OpenApiOperation"/> instance.</returns>
        OpenApiOperation GetOpenApiOperation(MethodInfo element, FunctionNameAttribute function, OperationType verb);

        /// <summary>
        /// Gets the collection of <see cref="OpenApiSecurityRequirement"/> instances.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <returns>Collection of <see cref="OpenApiSecurityRequirement"/> instance.</returns>
        List<OpenApiSecurityRequirement> GetOpenApiSecurityRequirement(MethodInfo element, NamingStrategy namingStrategy = null);

        /// <summary>
        /// Gets the list of <see cref="OpenApiParameter"/> instances.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <param name="trigger"><see cref="HttpTriggerAttribute"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance to process parameters.</param>
        /// <returns>List of <see cref="OpenApiParameter"/> instance.</returns>
        List<OpenApiParameter> GetOpenApiParameters(MethodInfo element, HttpTriggerAttribute trigger, NamingStrategy namingStrategy, VisitorCollection collection);

        /// <summary>
        /// Gets the <see cref="OpenApiRequestBody"/> instance.
        /// </summary>
        /// <param name="element"><see cref="MethodInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance to process parameters.</param>
        /// <returns><see cref="OpenApiRequestBody"/> instance.</returns>
        OpenApiRequestBody GetOpenApiRequestBody(MethodInfo element, NamingStrategy namingStrategy, VisitorCollection collection);

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
        /// <returns><see cref="OpenApiResponses"/> instance.</returns>
        OpenApiResponses GetOpenApiResponses(MethodInfo element, NamingStrategy namingStrategy, VisitorCollection collection);

        /// <summary>
        /// Gets the collection of <see cref="OpenApiSchema"/> instances.
        /// </summary>
        /// <param name="elements">List of <see cref="MethodInfo"/> instance.</param>
        /// <param name="namingStrategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <param name="collection"><see cref="VisitorCollection"/> instance to add types to schema.</param>
        /// <returns>Collection of <see cref="OpenApiSchema"/> instance.</returns>
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
    }
}
