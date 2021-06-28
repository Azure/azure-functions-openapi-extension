using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionProviders
{
    /// <summary>
    /// This represents the function provider entity for OpenAPI HTTP trigger that renders the Swagger UI page with the <see cref="AuthorizationLevel.User"/> access level.
    /// </summary>
    public class OpenApiTriggerRenderSwaggerUIFunctionProvider : IOpenApiTriggerRenderSwaggerUIFunctionProvider
    {
        private readonly IOpenApiTriggerFunction _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiTriggerRenderSwaggerUIFunctionProvider"/> class.
        /// </summary>
        /// <param name="function"><see cref="IOpenApiTriggerFunction"/> instance.</param>
        public OpenApiTriggerRenderSwaggerUIFunctionProvider(IOpenApiTriggerFunction function)
        {
            this._function = function.ThrowIfNullOrDefault();
        }

        /// <inheritdoc/>
        [OpenApiIgnore]
        [Function(nameof(OpenApiTriggerRenderSwaggerUIFunctionProvider.RenderSwaggerUI))]
        public async Task<HttpResponseData> RenderSwaggerUI(
            [HttpTrigger(AuthorizationLevel.User, "GET", Route = "swagger/ui")] HttpRequestData req,
            FunctionContext ctx)
        {
            return await this._function.RenderSwaggerUI(req, ctx);
        }
    }
}
