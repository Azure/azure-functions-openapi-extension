using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class ExtensionsSettingsTests
    {
        [TestMethod]
        public void Given_Value_Property_Should_Return_Value()
        {
            var settings = new ExtensionsSettings();

            settings.Http.Should().NotBeNull();
        }
    }
}
