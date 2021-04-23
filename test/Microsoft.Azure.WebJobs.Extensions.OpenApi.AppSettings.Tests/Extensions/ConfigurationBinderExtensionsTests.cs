using System.IO;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Tests.Fakes;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions.Tests
{
    /// <summary>
    /// This represents the test entity for the <see cref="ConfigurationBinderExtensions"/> class.
    /// </summary>
    [TestClass]
    public class ConfigurationBinderExtensionsTests
    {
        [TestMethod]
        public void Given_Config_Get_Should_Return_Instance()
        {
            var config = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("fakeconfig.json")
                             .Build();

            var settings = config.Get<FakeProductSettings>("productSettings");

            settings.Should().NotBeNull();
            settings.Name.Should().Be("lorem ipsum");
        }
    }
}
