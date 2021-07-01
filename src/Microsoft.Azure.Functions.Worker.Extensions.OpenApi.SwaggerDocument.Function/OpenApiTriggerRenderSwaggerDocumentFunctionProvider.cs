using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionProviders
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger document with the <see cref="AuthorizationLevel.Function"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerDocumentFunctionProvider : IOpenApiTriggerRenderSwaggerDocumentFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerDocumentFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerDocumentFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerDocumentFunctionProvider.RenderSwaggerDocument))]
        public async Task<HttpResponseData> RenderSwaggerDocument(
            [HttpTrigger(AuthorizationLevel.Function, "GET", Route = "swagger.{extension}")] HttpRequestData req,
            string extension,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerDocument(req, extension, ctx).ConfigureAwait(false);
         }
    }
}
