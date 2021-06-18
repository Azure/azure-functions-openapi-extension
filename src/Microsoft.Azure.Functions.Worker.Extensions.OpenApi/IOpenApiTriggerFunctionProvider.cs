using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi
{
    /// <summary>
    /// This provides interfaces to the <see cref="IOpenApiTriggerFunctionProvider"/> class.
    /// </summary>
    public interface IOpenApiTriggerFunctionProvider
    {
        /// <summary>
        /// Invokes the HTTP trigger endpoint to get OpenAPI document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        Task<HttpResponseData> RenderSwaggerDocument(HttpRequestData req, string extension, FunctionContext ctx);

        /// <summary>
        /// Invokes the HTTP trigger endpoint to get OpenAPI document.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="version">OpenAPI document spec version. This MUST be either "v2" or "v3".</param>
        /// <param name="extension">File extension representing the document format. This MUST be either "json" or "yaml".</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>OpenAPI document in a format of either JSON or YAML.</returns>
        Task<HttpResponseData> RenderOpenApiDocument(HttpRequestData req, string version, string extension, FunctionContext ctx);

        /// <summary>
        /// Invokes the HTTP trigger endpoint to render Swagger UI in HTML.
        /// </summary>
        /// <param name="req"><see cref="HttpRequestData"/> instance.</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>Swagger UI in HTML.</returns>
        Task<HttpResponseData> RenderSwaggerUI(HttpRequestData req, FunctionContext ctx);

        /// <summary>
        /// Invokes the HTTP trigger endpoint to render oauth2-redirect.html.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="ctx"><see cref="FunctionContext"/> instance.</param>
        /// <returns>oauth2-redirect.html.</returns>
        Task<HttpResponseData> RenderOAuth2Redirect(HttpRequestData req, FunctionContext ctx);
    }
}
