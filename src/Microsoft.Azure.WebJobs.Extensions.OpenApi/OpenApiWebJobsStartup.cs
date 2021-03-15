using Microsoft.Azure.WebJobs.Extensions.OpenApi;
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
        /// <inheritdoc />
        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddSingleton<IFunctionProvider, OpenApiTriggerFunctionProvider>();
        }
    }
}
