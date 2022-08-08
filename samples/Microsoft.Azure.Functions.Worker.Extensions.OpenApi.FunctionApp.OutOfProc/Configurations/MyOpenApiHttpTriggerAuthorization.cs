using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc.Configurations
{
    public class MyOpenApiHttpTriggerAuthorization : DefaultOpenApiHttpTriggerAuthorization
    {
        public override async Task<OpenApiAuthorizationResult> AuthorizeAsync(IHttpRequestDataObject req)
        {
            var result = default(OpenApiAuthorizationResult);

            // Put your custom logic here!

            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
