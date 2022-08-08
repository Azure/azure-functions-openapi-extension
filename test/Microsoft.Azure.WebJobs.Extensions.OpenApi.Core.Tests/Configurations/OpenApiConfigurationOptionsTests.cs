using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class OpenApiConfigurationOptionsTests
    {
        [TestMethod]
        public void Given_Type_When_Instantiated_Then_Properties_Should_Return_Result()
        {
            var options = new OpenApiConfigurationOptions();

            options.Info.Should().NotBeNull();
            options.Info.Version.Should().BeNull();
            options.Info.Title.Should().BeNull();
            options.Info.Description.Should().BeNull();

            options.Servers.Should().NotBeNull();
            options.Servers.Should().HaveCount(0);

            options.OpenApiVersion.Should().Be(OpenApiVersionType.V2);
            options.IncludeRequestingHostName.Should().BeFalse();
            options.ForceHttp.Should().BeFalse();
            options.ForceHttps.Should().BeFalse();

            options.DocumentFilters.Should().NotBeNull();
            options.DocumentFilters.Should().HaveCount(0);
        }
    }
}
