using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionProviders
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger document with the <see cref="AuthorizationLevel.Anonymous"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerDocumentAnonymousFunctionProvider : IOpenApiTriggerRenderSwaggerDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerDocumentAnonymousFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerDocumentAnonymousFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerDocumentAnonymousFunctionProvider.RenderSwaggerDocument))]
        public async Task<HttpResponseData> RenderSwaggerDocument(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "swagger.{extension}")] HttpRequestData req,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerDocument(req, extension, ctx).ConfigureAwait(false);
         }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger document with the <see cref="AuthorizationLevel.User"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerDocumentUserFunctionProvider : IOpenApiTriggerRenderSwaggerDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerDocumentUserFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerDocumentUserFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerDocumentUserFunctionProvider.RenderSwaggerDocument))]
        public async Task<HttpResponseData> RenderSwaggerDocument(
            [HttpTrigger(AuthorizationLevel.User, "GET", Route = "swagger.{extension}")] HttpRequestData req,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerDocument(req, extension, ctx).ConfigureAwait(false);
         }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger document with the <see cref="AuthorizationLevel.Function"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerDocumentFunctionFunctionProvider : IOpenApiTriggerRenderSwaggerDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerDocumentFunctionFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerDocumentFunctionFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerDocumentFunctionFunctionProvider.RenderSwaggerDocument))]
        public async Task<HttpResponseData> RenderSwaggerDocument(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger.{extension}")] HttpRequestData req,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerDocument(req, extension, ctx).ConfigureAwait(false);
         }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger document with the <see cref="AuthorizationLevel.System"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerDocumentSystemFunctionProvider : IOpenApiTriggerRenderSwaggerDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerDocumentSystemFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerDocumentSystemFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerDocumentSystemFunctionProvider.RenderSwaggerDocument))]
        public async Task<HttpResponseData> RenderSwaggerDocument(
            [HttpTrigger(AuthorizationLevel.System, "GET", Route = "swagger.{extension}")] HttpRequestData req,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerDocument(req, extension, ctx).ConfigureAwait(false);
         }
    }

    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger document with the <see cref="AuthorizationLevel.Admin"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerDocumentAdminFunctionProvider : IOpenApiTriggerRenderSwaggerDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerDocumentAdminFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerDocumentAdminFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerDocumentAdminFunctionProvider.RenderSwaggerDocument))]
        public async Task<HttpResponseData> RenderSwaggerDocument(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "swagger.{extension}")] HttpRequestData req,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerDocument(req, extension, ctx).ConfigureAwait(false);
         }
    }
}
