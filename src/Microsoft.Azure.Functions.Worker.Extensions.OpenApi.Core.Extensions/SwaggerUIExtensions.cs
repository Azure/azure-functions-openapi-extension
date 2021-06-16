using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="SwaggerUI"/>.
    /// </summary>
    public static class SwaggerUIExtensions
    {
        /// <summary>
        /// Renders the OpenAPI UI in HTML.
        /// </summary>
        /// <param name="ui"><see cref="ISwaggerUI"/> instance.</param>
        /// <param name="endpoint">The endpoint of the Swagger document.</param>
        /// <param name="authLevel">The authorisation level of the Swagger document.</param>
        /// <param name="authKey">API key of the HTTP endpoint to render OpenAPI document.</param>
        /// <returns>The OpenAPI UI in HTML.</returns>
        public static async Task<string> RenderAsync(this Task<ISwaggerUI> ui, string endpoint, OpenApiAuthLevelType authLevel = OpenApiAuthLevelType.Anonymous, string authKey = null)
        {
            var instance = await ui.ThrowIfNullOrDefault().ConfigureAwait(false);
            endpoint.ThrowIfNullOrWhiteSpace();

            return await instance.RenderAsync(endpoint, authLevel, authKey).ConfigureAwait(false);
        }

        /// <summary>
        /// Renders the OAuth2 Redirect page in HTML.
        /// </summary>
        /// <param name="ui"><see cref="ISwaggerUI"/> instance.</param>
        /// <param name="endpoint">The endpoint of the OAuth2 Redirect page.</param>
        /// <param name="authLevel">The authorisation level of the Swagger document.</param>
        /// <param name="authKey">API key of the HTTP endpoint to render OpenAPI document.</param>
        /// <returns>The OAuth2 Redirect page in HTML.</returns>
        public static async Task<string> RenderOAuth2RedirectAsync(this Task<ISwaggerUI> ui, string endpoint, OpenApiAuthLevelType authLevel = OpenApiAuthLevelType.Anonymous, string authKey = null)
        {
            var instance = await ui.ThrowIfNullOrDefault().ConfigureAwait(false);
            endpoint.ThrowIfNullOrWhiteSpace();

            return await instance.RenderOAuth2RedirectAsync(endpoint, authLevel, authKey).ConfigureAwait(false);
        }
    }
}
