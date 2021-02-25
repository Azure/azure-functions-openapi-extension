using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="SwaggerUI"/>.
    /// </summary>
    public static class SwaggerUIExtensions
    {
        /// <summary>
        /// Renders the Open API UI in HTML.
        /// </summary>
        /// <param name="ui"><see cref="ISwaggerUI"/> instance.</param>
        /// <param name="endpoint">The endpoint of the Swagger document.</param>
        /// <param name="authKey">API key of the HTTP endpoint to render Open API document.</param>
        /// <returns>The Open API UI in HTML.</returns>
        public static async Task<string> RenderAsync(this Task<ISwaggerUI> ui, string endpoint, string authKey = null)
        {
            var instance = await ui.ThrowIfNullOrDefault().ConfigureAwait(false);
            endpoint.ThrowIfNullOrWhiteSpace();

            return await instance.RenderAsync(endpoint, authKey).ConfigureAwait(false);
        }

        /// <summary>
        /// Renders the OAuth2 Redirect page in HTML.
        /// </summary>
        /// <param name="ui"><see cref="ISwaggerUI"/> instance.</param>
        /// <param name="endpoint">The endpoint of the OAuth2 Redirect page.</param>
        /// <param name="authKey">API key of the HTTP endpoint to render Open API document.</param>
        /// <returns>The OAuth2 Redirect page in HTML.</returns>
        public static async Task<string> RenderOAuth2RedirectAsync(this Task<ISwaggerUI> ui, string endpoint, string authKey = null)
        {
            var instance = await ui.ThrowIfNullOrDefault().ConfigureAwait(false);
            endpoint.ThrowIfNullOrWhiteSpace();

            return await instance.RenderOAuth2RedirectAsync(endpoint, authKey).ConfigureAwait(false);
        }
    }
}
