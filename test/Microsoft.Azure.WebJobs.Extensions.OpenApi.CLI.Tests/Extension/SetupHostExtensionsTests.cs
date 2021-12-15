using System.IO;
using System.Reflection;
using FluentAssertions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Tests.Extension
{
    [TestClass]
    public class SetupHostExtensionsTests
    {
        [TestMethod]
        public void HttpSettings()
        {
            // Arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var hostJsonPath = $"{path}/host.json";

            // Act
            var result = hostJsonPath.SetHostSettings();

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void SetOpenApiInfo()
        {
            // Arrange
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var compiledDllPath = $"{path}/Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5.dll";

            // Act
            var result = compiledDllPath.SetOpenApiInfo();

            // Assert
            result.Should().NotBeNull();
        }
    }
}
