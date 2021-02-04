using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class OpenApiSettingsTests
    {
        [TestMethod]
        public void Given_Value_Property_Should_Return_Value()
        {
            var settings = new OpenApiSettings();

            settings.Info.Should().NotBeNull();
            settings.Info.Version.Should().Be("1.0.0");
            settings.Info.Title.Should().Be("Azure Functions Open API Extension");
        }
    }
}
