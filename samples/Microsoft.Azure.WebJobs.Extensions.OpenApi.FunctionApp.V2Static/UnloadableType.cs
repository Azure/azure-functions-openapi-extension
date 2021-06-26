using System.Reflection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.V2Static
{
    /// <summary>
    /// This type is unloadable using reflection. <see cref="Assembly.GetTypes"/> will throw the <see cref="ReflectionTypeLoadException"/> exception.
    /// </summary>
    public class UnloadableType : IDummyConfigurationService
    {
        public IConfiguration Get()
        {
            return null;
        }
    }
}
