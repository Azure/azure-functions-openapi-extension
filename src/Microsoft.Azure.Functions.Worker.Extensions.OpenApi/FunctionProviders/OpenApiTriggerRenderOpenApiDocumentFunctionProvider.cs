using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionProviders
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the OpenAPI document with the <see cref="AuthorizationLevel.Anonymous"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOpenApiDocumentAnonymousFunctionProvider : IOpenApiTriggerRenderOpenApiDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOpenApiDocumentAnonymousFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOpenApiDocumentAnonymousFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOpenApiDocumentAnonymousFunctionProvider.RenderOpenApiDocument))]
        public async Task<HttpResponseData> RenderOpenApiDocument(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "openapi/{version}.{extension}")] HttpRequestData req,
            string version,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderOpenApiDocument(req, version, extension, ctx).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the OpenAPI document with the <see cref="AuthorizationLevel.User"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOpenApiDocumentUserFunctionProvider : IOpenApiTriggerRenderOpenApiDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOpenApiDocumentUserFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOpenApiDocumentUserFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOpenApiDocumentUserFunctionProvider.RenderOpenApiDocument))]
        public async Task<HttpResponseData> RenderOpenApiDocument(
            [HttpTrigger(AuthorizationLevel.User, "GET", Route = "openapi/{version}.{extension}")] HttpRequestData req,
            string version,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderOpenApiDocument(req, version, extension, ctx).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the OpenAPI document with the <see cref="AuthorizationLevel.Function"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOpenApiDocumentFunctionFunctionProvider : IOpenApiTriggerRenderOpenApiDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOpenApiDocumentFunctionFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOpenApiDocumentFunctionFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOpenApiDocumentFunctionFunctionProvider.RenderOpenApiDocument))]
        public async Task<HttpResponseData> RenderOpenApiDocument(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "openapi/{version}.{extension}")] HttpRequestData req,
            string version,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderOpenApiDocument(req, version, extension, ctx).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the OpenAPI document with the <see cref="AuthorizationLevel.System"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOpenApiDocumentSystemFunctionProvider : IOpenApiTriggerRenderOpenApiDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOpenApiDocumentSystemFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOpenApiDocumentSystemFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOpenApiDocumentSystemFunctionProvider.RenderOpenApiDocument))]
        public async Task<HttpResponseData> RenderOpenApiDocument(
            [HttpTrigger(AuthorizationLevel.System, "GET", Route = "openapi/{version}.{extension}")] HttpRequestData req,
            string version,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderOpenApiDocument(req, version, extension, ctx).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the OpenAPI document with the <see cref="AuthorizationLevel.Admin"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOpenApiDocumentAdminFunctionProvider : IOpenApiTriggerRenderOpenApiDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOpenApiDocumentAdminFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOpenApiDocumentAdminFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOpenApiDocumentAdminFunctionProvider.RenderOpenApiDocument))]
        public async Task<HttpResponseData> RenderOpenApiDocument(
            [HttpTrigger(AuthorizationLevel.Admin, "GET", Route = "openapi/{version}.{extension}")] HttpRequestData req,
            string version,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderOpenApiDocument(req, version, extension, ctx).ConfigureAwait(false);
        }
    }
}
