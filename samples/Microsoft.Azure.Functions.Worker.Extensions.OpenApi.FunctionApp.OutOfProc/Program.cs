using AutoFixture;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.OutOfProc
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureOpenApi()
                .ConfigureServices(services => {
                    services.AddSingleton<Fixture>();

                    // Example: If you want to change the configuration during startup of your function you can use the following code:

                    //services.AddSingleton<IOpenApiConfigurationOptions>(x =>
                    //{
                    //    return new OpenApiConfigurationOptions()
                    //    {
                    //        Info = new Microsoft.OpenApi.Models.OpenApiInfo
                    //        {
                    //            Title = "A dynamic title generated at runtime",
                    //            Description = "Dynamic Open API information at runtime"
                    //        }
                    //    };
                    //});
                })
                .Build();

            host.Run();
        }
    }
}
