using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi
{
    /// <summary>
    /// This provides interfaces to <see cref="OpenApiTriggerFunction"/> class.
    /// /// </summary>
    public interface IOpenApiTriggerFunction
        : IOpenApiTriggerRenderSwaggerDocumentFunctionProvider,
          IOpenApiTriggerRenderOpenApiDocumentFunctionProvider,
          IOpenApiTriggerRenderSwaggerUIFunctionProvider,
          IOpenApiTriggerRenderOAuth2RedirectFunctionProvider
    {
    }
}
