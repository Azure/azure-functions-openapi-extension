using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="IOpenApiHttpTriggerContext"/>.
    /// </summary>
    public static class OpenApiHttpTriggerContextExtensions
    {
        /// <summary>
        /// Authorizes the endpoint.
        /// </summary>
        /// <param name="context"><see cref="IOpenApiHttpTriggerContext"/> instance.</param>
        /// <param name="req"><see cref="IHttpRequestDataObject"/> instance.</param>
        /// <returns>Returns <see cref="OpenApiAuthorizationResult"/> instance.</returns>
        public static async Task<OpenApiAuthorizationResult> AuthorizeAsync(this Task<IOpenApiHttpTriggerContext> context, IHttpRequestDataObject req)
        {
            req.ThrowIfNullOrDefault();

            var instance = await context.ThrowIfNullOrDefault().ConfigureAwait(false);

            return await instance.AuthorizeAsync(req).ConfigureAwait(false);
        }
    }
}
