using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to the <see cref="SwaggerUI"/> class.
    /// </summary>
    public interface ISwaggerUI
    {
        /// <summary>
        /// Adds metadata to build OpenAPI document.
        /// </summary>
        /// <param name="info"><see cref="OpenApiInfo"/> instance.</param>
        /// <returns><see cref="ISwaggerUI"/> instance.</returns>
        ISwaggerUI AddMetadata(OpenApiInfo info);

        /// <summary>
        /// Adds server details.
        /// </summary>
        /// <param name="req"><see cref="IHttpRequestDataObject"/> instance.</param>
        /// <param name="routePrefix">Route prefix value.</param>
        /// <param name="options"><see cref="IOpenApiConfigurationOptions"/> instance.</param>
        /// <returns><see cref="ISwaggerUI"/> instance.</returns>
        ISwaggerUI AddServer(IHttpRequestDataObject req, string routePrefix, IOpenApiConfigurationOptions options = null);

        /// <summary>
        /// Builds Swagger UI document.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        /// <param name="options"><see cref="IOpenApiCustomUIOptions"/> instance.</param>
        /// <returns><see cref="ISwaggerUI"/> instance.</returns>
        Task<ISwaggerUI> BuildAsync(Assembly assembly, IOpenApiCustomUIOptions options = null);

        /// <summary>
        /// Builds OAuth2 Redirect document.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        /// <returns><see cref="ISwaggerUI"/> instance.</returns>
        Task<ISwaggerUI> BuildOAuth2RedirectAsync(Assembly assembly);

        /// <summary>
        /// Renders OpenAPI UI in HTML.
        /// </summary>
        /// <param name="endpoint">The endpoint of the Swagger document.</param>
        /// <param name="authLevel">The authorisation level of the Swagger document.</param>
        /// <param name="authKey">API key of the HTTP endpoint to render OpenAPI document.</param>
        /// <returns>OpenAPI UI in HTML.</returns>
        Task<string> RenderAsync(string endpoint, OpenApiAuthLevelType authLevel = OpenApiAuthLevelType.Anonymous, string authKey = null);

        /// <summary>
        /// Renders OAuth Redirect in HTML.
        /// </summary>
        /// <param name="endpoint">The endpoint of the OAuth2 Redirect page.</param>
        /// <param name="authLevel">The authorisation level of the Swagger document.</param>
        /// <param name="authKey">API key of the HTTP endpoint to render OpenAPI document.</param>
        /// <returns>OAuth Redirect in HTML.</returns>
        Task<string> RenderOAuth2RedirectAsync(string endpoint, OpenApiAuthLevelType authLevel = OpenApiAuthLevelType.Anonymous, string authKey = null);
    }
}
