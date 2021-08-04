using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi
{
    /// <summary>
    /// This represents HTTP trigger entity to render OpenAPI endpoints with function access.
    /// </summary>
    public class OpenApiHttpTrigger : DefaultOpenApiHttpTrigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiHttpTrigger"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiHttpTrigger(IOpenApiTriggerFunction function) : base(function)
        {
        }

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get OpenAPI document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        [Function(nameof(OpenApiHttpTrigger.RenderSwaggerDocument))]
        [OpenApiIgnore]
        public new async Task<HttpResponseData> RenderSwaggerDocument(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger.{extension}")] HttpRequestData req,
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
        [Function(nameof(OpenApiHttpTrigger.RenderOpenApiDocument))]
        [OpenApiIgnore]
        public new async Task<HttpResponseData> RenderOpenApiDocument(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "openapi/{version}.{extension}")] HttpRequestData req,
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
        [Function(nameof(OpenApiHttpTrigger.RenderSwaggerUI))]
        [OpenApiIgnore]
        public new async Task<HttpResponseData> RenderSwaggerUI(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger/ui")] HttpRequestData req,
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
        [Function(nameof(OpenApiHttpTrigger.RenderOAuth2Redirect))]
        [OpenApiIgnore]
        public new async Task<HttpResponseData> RenderOAuth2Redirect(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "oauth2-redirect.html")] HttpRequestData req,
            FunctionContext ctx)
        {
            var response = await this.Function.RenderOAuth2Redirect(req, ctx).ConfigureAwait(false);

            return response;
        }
    }
}
