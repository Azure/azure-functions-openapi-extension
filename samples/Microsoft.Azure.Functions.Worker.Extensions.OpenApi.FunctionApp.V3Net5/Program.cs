using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Resolvers;
using Microsoft.Azure.WebJobs.Script.Description;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5
{
    public class Program
    {
        private const string OpenApiSettingsKey = "OpenApi";

        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureServices(services =>
                 {
                     var config = ConfigurationResolver.Resolve();
                     var settings = config.Get<OpenApiSettings>(OpenApiSettingsKey);

                     services.AddSingleton(settings);
                     services.AddSingleton<IFunctionProvider, OpenApiTriggerFunctionProvider>();
                 })
                .Build();

            host.Run();
        }
    }
}
