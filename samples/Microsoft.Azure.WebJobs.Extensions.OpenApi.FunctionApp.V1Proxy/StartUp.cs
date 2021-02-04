using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V1Proxy.StartUp))]
namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V1Proxy
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
