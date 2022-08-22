using System;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the entity for the authorisation options for the HTTP trigger endpoints used for Swagger UI and OpenAPI document.
    /// </summary>
    [OpenApiHttpTriggerAuthorizationIgnore]
    public class OpenApiHttpTriggerAuthorization : IOpenApiHttpTriggerAuthorization
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiHttpTriggerAuthorization"/> class.
        /// </summary>
        public OpenApiHttpTriggerAuthorization()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiHttpTriggerAuthorization"/> class.
        /// </summary>
        /// <param name="func">The delegation instance for the custom authorisation logic.</param>
        public OpenApiHttpTriggerAuthorization(Func<IHttpRequestDataObject, Task<OpenApiAuthorizationResult>> func = null)
        {
            this.Authorization = func;
        }

        /// <summary>
        /// Gets or sets the delegation instance for the custom authorisation logic.
        /// </summary>
        public Func<IHttpRequestDataObject, Task<OpenApiAuthorizationResult>> Authorization { get; set; }

        /// <inheritdoc />
        public virtual async Task<OpenApiAuthorizationResult> AuthorizeAsync(IHttpRequestDataObject req)
        {
            if (this.Authorization.IsNullOrDefault())
            {
                this.Authorization = new Func<IHttpRequestDataObject, Task<OpenApiAuthorizationResult>>(async _ =>
                {
                    var result = default(OpenApiAuthorizationResult);

                    return await Task.FromResult(result).ConfigureAwait(false);
                });
            }

            return await this.Authorization.Invoke(req).ConfigureAwait(false);
        }
    }
}
