using System.Linq;

using FluentAssertions;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Configurations;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests
{
    [TestClass]
    public class OpenApiWorkerStartupTests
    {
        [TestMethod]
        public void Given_Type_When_Initialised_Then_It_Should_Return_Result()
        {
            var services = new ServiceCollection();
            var builder = new FakeFunctionsWorkerApplicationBuilder(services);

            var startup = new OpenApiWorkerStartup();
            startup.Configure(builder);

            services.SingleOrDefault(p => p.ServiceType == typeof(OpenApiSettings)).Should().NotBeNull();
            services.SingleOrDefault(p => p.ServiceType == typeof(IOpenApiTriggerFunction)).Should().NotBeNull();
            services.SingleOrDefault(p => p.ServiceType == typeof(IOpenApiHttpTriggerContext)).Should().NotBeNull();
        }
    }
}
