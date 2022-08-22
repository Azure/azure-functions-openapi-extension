using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    [OpenApiConfigurationOptionsIgnore]
    public abstract class FakeOpenApiConfigurationOptionsBase : DefaultOpenApiConfigurationOptions
    {
    }
}
