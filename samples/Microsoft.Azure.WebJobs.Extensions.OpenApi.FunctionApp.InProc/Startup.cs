using AutoFixture;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc.Configurations;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc.Startup))]

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.InProc
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var fixture = new Fixture();
            builder.Services.AddSingleton(fixture);

            // Example: If you want to change the configuration during startup of your function you can use the following code:

            //builder.Services.AddSingleton<IOpenApiConfigurationOptions>((_) =>
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
        }
    }
}
