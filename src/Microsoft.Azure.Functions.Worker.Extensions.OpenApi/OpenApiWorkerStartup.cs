using Microsoft.Azure.Functions.Worker.Core;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

[assembly: WorkerExtensionStartup(typeof(OpenApiWorkerStartup))]

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi
{
    public class OpenApiWorkerStartup : WorkerExtensionStartup
    {
        public override void Configure(IFunctionsWorkerApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();
            applicationBuilder.Services.AddSingleton<IOpenApiTriggerFunction, OpenApiTriggerFunction>();
            //applicationBuilder.Services.AddSingleton<DefaultOpenApiHttpTrigger, DefaultOpenApiHttpTrigger>();
        }
    }
}
