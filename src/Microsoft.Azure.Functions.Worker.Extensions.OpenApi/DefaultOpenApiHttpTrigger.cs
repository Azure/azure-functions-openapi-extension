using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi
{
    /// <summary>
    /// This represents HTTP trigger entity to render OpenAPI endpoints with anonymous access.
    /// </summary>
    public class DefaultOpenApiHttpTrigger
    {
        /// <summary>
        /// Defines the text/plain content type.
        /// </summary>
        protected const string ContentTypeText = "text/plain";

        /// <summary>
        /// Defines the text/html content type.
        /// </summary>
        protected const string ContentTypeHtml = "text/html";

        /// <summary>
        /// Defines the application/json content type.
        /// </summary>
        protected const string ContentTypeJson = "application/json";

        /// <summary>
        /// Defines the text/vnd.yaml content type.
        /// </summary>
        protected const string ContentTypeYaml = "text/vnd.yaml";

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOpenApiHttpTrigger"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public DefaultOpenApiHttpTrigger(IOpenApiTriggerFunction function)
        {
            this.Function = function.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Gets the <see cref="IOpenApiTriggerFunction"/> instance.
        /// </summary>
        protected IOpenApiTriggerFunction Function { get; }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get OpenAPI document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        [Function(nameof(DefaultOpenApiHttpTrigger.RenderSwaggerDocument))]
        [OpenApiIgnore]
        public virtual async Task<HttpResponseData> RenderSwaggerDocument(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "swagger.{extension}")] HttpRequestData req,
            string extension,
            FunctionContext ctx)
        {
            var response = await this.Function.RenderSwaggerDocument(req, extension, ctx).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get OpenAPI document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="version">OpenAPI document spec version. This MUST be either "v2" or "v3".</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        [Function(nameof(DefaultOpenApiHttpTrigger.RenderOpenApiDocument))]
        [OpenApiIgnore]
        public virtual async Task<HttpResponseData> RenderOpenApiDocument(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "openapi/{version}.{extension}")] HttpRequestData req,
            string version,
            string extension,
            FunctionContext ctx)
        {
            var response = await this.Function.RenderOpenApiDocument(req, version, extension, ctx).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to render Swagger UI in HTML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>Swagger UI in HTML.</returns>
        [Function(nameof(DefaultOpenApiHttpTrigger.RenderSwaggerUI))]
        [OpenApiIgnore]
        public virtual async Task<HttpResponseData> RenderSwaggerUI(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "swagger/ui")] HttpRequestData req,
            FunctionContext ctx)
        {
            var response = await this.Function.RenderSwaggerUI(req, ctx).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to render oauth2-redirect.html.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>oauth2-redirect.html.</returns>
        [Function(nameof(DefaultOpenApiHttpTrigger.RenderOAuth2Redirect))]
        [OpenApiIgnore]
        public virtual async Task<HttpResponseData> RenderOAuth2Redirect(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "oauth2-redirect.html")] HttpRequestData req,
            FunctionContext ctx)
        {
            var response = await this.Function.RenderOAuth2Redirect(req, ctx).ConfigureAwait(false);

            return response;
        }
    }
}
