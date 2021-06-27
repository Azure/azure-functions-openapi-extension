using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionProviders
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the oauth2-redirect.html page with the <see cref="AuthorizationLevel.Anonymous"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOAuth2RedirectAnonymousFunctionProvider : IOpenApiTriggerRenderOAuth2RedirectFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOAuth2RedirectAnonymousFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOAuth2RedirectAnonymousFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOAuth2RedirectAnonymousFunctionProvider.RenderOAuth2Redirect))]
        public async Task<HttpResponseData> RenderOAuth2Redirect(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "oauth2-redirect.html")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderOAuth2Redirect(req, ctx).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the oauth2-redirect.html page with the <see cref="AuthorizationLevel.User"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOAuth2RedirectUserFunctionProvider : IOpenApiTriggerRenderOAuth2RedirectFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOAuth2RedirectUserFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOAuth2RedirectUserFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOAuth2RedirectUserFunctionProvider.RenderOAuth2Redirect))]
        public async Task<HttpResponseData> RenderOAuth2Redirect(
            [HttpTrigger(AuthorizationLevel.User, "GET", Route = "oauth2-redirect.html")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderOAuth2Redirect(req, ctx).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the oauth2-redirect.html page with the <see cref="AuthorizationLevel.Function"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOAuth2RedirectFunctionFunctionProvider : IOpenApiTriggerRenderOAuth2RedirectFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOAuth2RedirectFunctionFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOAuth2RedirectFunctionFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOAuth2RedirectFunctionFunctionProvider.RenderOAuth2Redirect))]
        public async Task<HttpResponseData> RenderOAuth2Redirect(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "oauth2-redirect.html")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderOAuth2Redirect(req, ctx).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the oauth2-redirect.html page with the <see cref="AuthorizationLevel.System"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOAuth2RedirectSystemFunctionProvider : IOpenApiTriggerRenderOAuth2RedirectFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOAuth2RedirectSystemFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOAuth2RedirectSystemFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOAuth2RedirectSystemFunctionProvider.RenderOAuth2Redirect))]
        public async Task<HttpResponseData> RenderOAuth2Redirect(
            [HttpTrigger(AuthorizationLevel.System, "GET", Route = "oauth2-redirect.html")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderOAuth2Redirect(req, ctx).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the oauth2-redirect.html page with the <see cref="AuthorizationLevel.Admin"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOAuth2RedirectAdminFunctionProvider : IOpenApiTriggerRenderOAuth2RedirectFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOAuth2RedirectAdminFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOAuth2RedirectAdminFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOAuth2RedirectAdminFunctionProvider.RenderOAuth2Redirect))]
        public async Task<HttpResponseData> RenderOAuth2Redirect(
            [HttpTrigger(AuthorizationLevel.Admin, "GET", Route = "oauth2-redirect.html")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderOAuth2Redirect(req, ctx).ConfigureAwait(false);
        }
    }
}
