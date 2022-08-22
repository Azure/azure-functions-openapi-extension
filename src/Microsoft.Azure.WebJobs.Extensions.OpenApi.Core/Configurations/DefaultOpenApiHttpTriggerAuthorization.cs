using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the entity for the default authorisation options for the HTTP trigger endpoints used for Swagger UI and OpenAPI document.
    /// </summary>
    [OpenApiHttpTriggerAuthorizationIgnore]
    public class DefaultOpenApiHttpTriggerAuthorization : IOpenApiHttpTriggerAuthorization
    {
        /// <inheritdoc />
        public virtual async Task<OpenApiAuthorizationResult> AuthorizeAsync(IHttpRequestDataObject req)
        {
            var result = default(OpenApiAuthorizationResult);

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
