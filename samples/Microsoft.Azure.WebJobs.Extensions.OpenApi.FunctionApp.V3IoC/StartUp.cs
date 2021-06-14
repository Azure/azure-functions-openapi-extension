using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Services;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3IoC.Configurations;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3IoC.StartUp))]
namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V3IoC
{
    /// <summary>
    /// This represents the entity to be invoked during the runtime startup.
    /// </summary>
    public class StartUp : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<AppSettings>();
            builder.Services.AddTransient<IDummyHttpService, DummyHttpService>();
        }
    }
}
