using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionProviders
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger UI page with the <see cref="AuthorizationLevel.Anonymous"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerUIAnonymousFunctionProvider : IOpenApiTriggerRenderSwaggerUIFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerUIAnonymousFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerUIAnonymousFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerUIAnonymousFunctionProvider.RenderSwaggerUI))]
        public async Task<HttpResponseData> RenderSwaggerUI(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "swagger/ui")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerUI(req, ctx);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger UI page with the <see cref="AuthorizationLevel.User"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerUIUserFunctionProvider : IOpenApiTriggerRenderSwaggerUIFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerUIUserFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerUIUserFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerUIUserFunctionProvider.RenderSwaggerUI))]
        public async Task<HttpResponseData> RenderSwaggerUI(
            [HttpTrigger(AuthorizationLevel.User, "GET", Route = "swagger/ui")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerUI(req, ctx);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger UI page with the <see cref="AuthorizationLevel.Function"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerUIFunctionFunctionProvider : IOpenApiTriggerRenderSwaggerUIFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerUIFunctionFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerUIFunctionFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerUIFunctionFunctionProvider.RenderSwaggerUI))]
        public async Task<HttpResponseData> RenderSwaggerUI(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger/ui")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerUI(req, ctx);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger UI page with the <see cref="AuthorizationLevel.System"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerUISystemFunctionProvider : IOpenApiTriggerRenderSwaggerUIFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerUISystemFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerUISystemFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerUISystemFunctionProvider.RenderSwaggerUI))]
        public async Task<HttpResponseData> RenderSwaggerUI(
            [HttpTrigger(AuthorizationLevel.System, "GET", Route = "swagger/ui")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerUI(req, ctx);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger UI page with the <see cref="AuthorizationLevel.Admin"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerUIAdminFunctionProvider : IOpenApiTriggerRenderSwaggerUIFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerUIAdminFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerUIAdminFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerUIAdminFunctionProvider.RenderSwaggerUI))]
        public async Task<HttpResponseData> RenderSwaggerUI(
            [HttpTrigger(AuthorizationLevel.Admin, "GET", Route = "swagger/ui")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerUI(req, ctx);
        }
    }
}
