using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions
{
    /// <summary>
    /// This provides interfaces to HTTP trigger authorisation for OpenAPI endpoints.
    /// </summary>
    public interface IOpenApiHttpTriggerAuthorization
    {
        /// <summary>
        /// Authorizes the endpoint.
        /// </summary>
        /// <param name="req"><see cref="IHttpRequestDataObject"/> instance.</param>
        /// <returns>Returns <see cref="OpenApiAuthorizationResult"/> instance.</returns>
        Task<OpenApiAuthorizationResult> AuthorizeAsync(IHttpRequestDataObject req);
    }
}
