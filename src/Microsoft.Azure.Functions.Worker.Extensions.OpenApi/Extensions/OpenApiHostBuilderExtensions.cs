using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Functions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions
{
    /// <summary>
    /// This represents the extensions entity to configure OpenAPI capability to Azure Functions out-of-process worker.
    /// </summary>
    public static class OpenApiHostBuilderExtensions
    {
        /// <summary>
        /// Configures to use OpenAPI features.
        /// </summary>
        /// <param name="hostBuilder"><see cref="IHostBuilder"/> instance.</param>
        /// <returns>Returns <see cref="IHostBuilder"/> instance.</returns>
        public static IHostBuilder ConfigureOpenApi(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();
                services.AddSingleton<IOpenApiTriggerFunction, OpenApiTriggerFunction>();
                // services.AddSingleton<DefaultOpenApiHttpTrigger, DefaultOpenApiHttpTrigger>();
            });

            return hostBuilder;
        }
    }
}
