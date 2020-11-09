using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Services;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionAppV1Proxy.StartUp))]
namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionAppV1Proxy
{
    /// <summary>
    /// This represents the entity to be invoked during the runtime startup.
    /// </summary>
    public class StartUp : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
        }
    }
}
