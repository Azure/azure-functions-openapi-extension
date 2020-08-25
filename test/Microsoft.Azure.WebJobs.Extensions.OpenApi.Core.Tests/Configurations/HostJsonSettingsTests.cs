using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class HostJsonSettingsTests
    {
        [TestMethod]
        public void Given_Value_Property_Should_Return_Value()
        {
            var settings = new HostJsonSettings();

            settings.Version.Should().BeNullOrWhiteSpace();
            settings.Http.Should().BeNull();
            settings.Extensions.Should().BeNull();
        }
    }
}
