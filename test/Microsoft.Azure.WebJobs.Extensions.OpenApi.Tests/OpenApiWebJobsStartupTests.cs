using System.Linq;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests.Fakes;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Script.Description;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests
{
    [TestClass]
    public class OpenApiWebJobsStartupTests
    {
        [TestMethod]
        public void Given_Type_When_Initialised_Then_It_Should_Return_Result()
        {
            var services = new ServiceCollection();
            var builder = new FakeWebJobsBuilder(services);

            var startup = new OpenApiWebJobsStartup();
            startup.Configure(builder);

            services.SingleOrDefault(p => p.ServiceType == typeof(OpenApiSettings)).Should().NotBeNull();
            services.SingleOrDefault(p => p.ServiceType == typeof(IFunctionProvider)).Should().NotBeNull();
            services.SingleOrDefault(p => p.ServiceType == typeof(IOpenApiHttpTriggerContext)).Should().NotBeNull();
            services.SingleOrDefault(p => p.ServiceType == typeof(IExtensionConfigProvider)).Should().NotBeNull();
        }
    }
}
