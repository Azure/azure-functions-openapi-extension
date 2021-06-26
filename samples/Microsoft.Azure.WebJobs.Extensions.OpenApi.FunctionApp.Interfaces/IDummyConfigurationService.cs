using Microsoft.Extensions.Configuration;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Interfaces
{
    /// <summary>
    /// This provides an interface for type in entry assembly that will be unloadable using reflection.
    /// </summary>
    public interface IDummyConfigurationService
    {
        IConfiguration Get();
    }
}
