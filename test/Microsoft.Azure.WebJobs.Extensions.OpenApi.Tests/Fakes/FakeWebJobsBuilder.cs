using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests.Fakes
{
    public class FakeWebJobsBuilder : IWebJobsBuilder
    {
        public FakeWebJobsBuilder(IServiceCollection services)
        {
            this.Services = services.ThrowIfNullOrDefault();
        }

        public IServiceCollection Services { get; }
    }
}
