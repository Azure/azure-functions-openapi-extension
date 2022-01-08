using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Filters;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to its implementing class.
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// Gets the underlying <see cref="OpenApiDocument"/> instance.
        /// </summary>
        OpenApiDocument OpenApiDocument { get; }

        /// <summary>
        /// Initializes the document instance.
        /// </summary>
        /// <returns><see cref="IDocument"/> instance.</returns>
        IDocument InitialiseDocument();

        /// <summary>
        /// Adds metadata to build OpenAPI document.
        /// </summary>
        /// <param name="info"><see cref="OpenApiInfo"/> instance.</param>
        /// <returns><see cref="IDocument"/> instance.</returns>
        IDocument AddMetadata(OpenApiInfo info);

        /// <summary>
        /// Adds server details.
        /// </summary>
        /// <param name="req"><see cref="IHttpRequestDataObject"/> instance.</param>
        /// <param name="routePrefix">Route prefix value.</param>
        /// <param name="options"><see cref="IOpenApiConfigurationOptions"/> instance.</param>
        /// <returns><see cref="IDocument"/> instance.</returns>
        IDocument AddServer(IHttpRequestDataObject req, string routePrefix, IOpenApiConfigurationOptions options = null);

        /// <summary>
        /// Adds the naming strategy.
        /// </summary>
        /// <param name="strategy"><see cref="NamingStrategy"/> instance to create the JSON schema from .NET Types.</param>
        /// <returns><see cref="IDocument"/> instance.</returns>
        IDocument AddNamingStrategy(NamingStrategy strategy);

        /// <summary>
        /// Adds the visitor collection.
        /// </summary>
        /// <param name="collection"><see cref="VisitorCollection"/> instance.</param>
        /// <returns><see cref="IDocument"/> instance.</returns>
        IDocument AddVisitors(VisitorCollection collection);

        /// <summary>
        /// Builds OpenAPI document.
        /// </summary>
        /// <param name="assemblyPath">Assembly file path.</param>
        /// <param name="version">OpenAPI spec version.</param>
        /// <returns><see cref="IDocument"/> instance.</returns>
        IDocument Build(string assemblyPath, OpenApiVersionType version = OpenApiVersionType.V2);

        /// <summary>
        /// Builds OpenAPI document.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        /// <param name="version">OpenAPI spec version.</param>
        /// <returns><see cref="IDocument"/> instance.</returns>
        IDocument Build(Assembly assembly, OpenApiVersionType version = OpenApiVersionType.V2);

        /// <summary>
        /// Applies the given <see cref="DocumentFilterCollection"/> to the <see cref="IDocument"/>.
        /// </summary>
        /// <param name="collection"><see cref="DocumentFilterCollection"/> instance.</param>
        /// <returns></returns>
        IDocument ApplyDocumentFilters(DocumentFilterCollection collection);

        /// <summary>
        /// Renders OpenAPI document.
        /// </summary>
        /// <param name="version"><see cref="OpenApiSpecVersion"/> value.</param>
        /// <param name="format"><see cref="OpenApiFormat"/> value.</param>
        /// <returns>Serialised OpenAPI document.</returns>
        Task<string> RenderAsync(OpenApiSpecVersion version, OpenApiFormat format);
    }
}
