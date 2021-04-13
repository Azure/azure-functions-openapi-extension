using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class DefaultOpenApiConfigurationOptionsTests
    {
        [TestMethod]
        public void Given_Type_When_Instantiated_Then_Properties_Should_Return_Value()
        {
            var settings = new DefaultOpenApiConfigurationOptions();

            settings.Info.Should().NotBeNull();
            settings.Info.Version.Should().Be("1.0.0");
            settings.Info.Title.Should().Be("Azure Functions OpenAPI Extension");

            settings.Servers.Should().NotBeNull();
            settings.Servers.Should().HaveCount(0);
        }
    }
}
