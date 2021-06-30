using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.Azure.Functions.Worker.Http;

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
