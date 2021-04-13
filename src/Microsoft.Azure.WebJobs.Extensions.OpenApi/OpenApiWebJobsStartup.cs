using Microsoft.Azure.WebJobs.Extensions.OpenApi;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.WebJobs.Script.Description;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(OpenApiWebJobsStartup))]
namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the startup entity for Open API endpoints registration
    /// </summary>
    public class OpenApiWebJobsStartup : IWebJobsStartup
    {
        private const string OpenApiSettingsKey = "OpenApi";

        /// <inheritdoc />
        public void Configure(IWebJobsBuilder builder)
        {
            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettingsKey);

            builder.Services.AddSingleton(settings);
            builder.Services.AddSingleton<IFunctionProvider, OpenApiTriggerFunctionProvider>();
        }
    }
}
