using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionProviders
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the oauth2-redirect.html page with the <see cref="AuthorizationLevel.Admin"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOAuth2RedirectFunctionProvider : IOpenApiTriggerRenderOAuth2RedirectFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOAuth2RedirectFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOAuth2RedirectFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOAuth2RedirectFunctionProvider.RenderOAuth2Redirect))]
        public async Task<HttpResponseData> RenderOAuth2Redirect(
            [HttpTrigger(AuthorizationLevel.Admin, "GET", Route = "oauth2-redirect.html")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderOAuth2Redirect(req, ctx).ConfigureAwait(false);
        }
    }
}
