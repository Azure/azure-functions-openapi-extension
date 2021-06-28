using FluentAssertions;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Extensions
{
    [TestClass]
    public class OpenApiHostBuilderExtensionsTests
    {
        [TestMethod]
        public void Given_HostBuilder_When_Build_Invoked_Then_It_Should_Return_Context()
        {
            var host = new HostBuilder()
                           .ConfigureOpenApi()
                           .Build();

            var context = host.Services.GetService<IOpenApiHttpTriggerContext>();

            context.Should().NotBeNull();
        }

        [TestMethod]
        public void Given_HostBuilder_When_Build_Invoked_Then_It_Should_Return_Function()
        {
            var host = new HostBuilder()
                           .ConfigureOpenApi()
                           .Build();

            var function = host.Services.GetService<IOpenApiTriggerFunction>();

            function.Should().NotBeNull();
        }
    }
}
