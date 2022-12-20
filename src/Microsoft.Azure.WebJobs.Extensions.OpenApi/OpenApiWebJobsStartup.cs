using Microsoft.Azure.WebJobs.Extensions.OpenApi;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Script.Description;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(OpenApiWebJobsStartup))]

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the startup entity for OpenAPI endpoints registration
    /// </summary>
    public class OpenApiWebJobsStartup : IWebJobsStartup
    {
        /// <inheritdoc />
        public void Configure(IWebJobsBuilder builder)
        {
            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            builder.Services.AddSingleton(settings);
            builder.Services.AddSingleton<IFunctionProvider, OpenApiTriggerFunctionProvider>();
            builder.Services.AddSingleton<IOpenApiHttpTriggerContext, OpenApiHttpTriggerContext>();

            builder.AddExtension<OpenApiHttpTriggerContextBinding>();
        }
    }
}
