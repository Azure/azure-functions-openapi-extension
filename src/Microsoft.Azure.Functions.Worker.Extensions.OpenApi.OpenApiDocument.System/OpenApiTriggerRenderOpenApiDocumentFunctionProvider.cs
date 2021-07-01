using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionProviders
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the OpenAPI document with the <see cref="AuthorizationLevel.System"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderOpenApiDocumentFunctionProvider : IOpenApiTriggerRenderOpenApiDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderOpenApiDocumentFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderOpenApiDocumentFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderOpenApiDocumentFunctionProvider.RenderOpenApiDocument))]
        public async Task<HttpResponseData> RenderOpenApiDocument(
            [HttpTrigger(AuthorizationLevel.System, "GET", Route = "openapi/{version}.{extension}")] HttpRequestData req,
            string version,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderOpenApiDocument(req, version, extension, ctx).ConfigureAwait(false);
        }
    }
}
