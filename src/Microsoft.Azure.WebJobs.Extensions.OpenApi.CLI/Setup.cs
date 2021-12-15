using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Service.Impl;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Service.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI
{
    public static class Setup
    {
        public static void ConfigureInternalServices(this IServiceCollection services)
        {
            services.AddSingleton<ICustomApiMockCreator, CustomApiMockCreator>();
            services.AddSingleton<ICustomOpenApiCreator, CustomOpenApiCreator>();
            services.AddSingleton<ICustomOpenApiWriter, CustomOpenApiWriter>();
        }
    }
}