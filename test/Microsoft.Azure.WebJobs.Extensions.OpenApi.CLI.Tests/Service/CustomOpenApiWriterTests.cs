using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Extension;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Service.Impl;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Tests.Service
{
    [TestClass]
    public class CustomOpenApiWriterTests
    {
        [TestMethod]
        public async Task WriteOpenApiToFile()
        {
            // Arrange
            var openApiDocument = "{\"Test\": \"Test\"}";
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var openApiFormatType = OpenApiFormatType.Json;

            var service = this.SetupSut();

            // Act
            await service.WriteOpenApiToFile(
                openApiDocument,
                path,
                openApiFormatType);

            // Assert
            File.Exists($"{path}{ProjectPathExtensions.DirectorySeparator}swagger.{openApiFormatType.ToDisplayName()}").Should().BeTrue();
        }

        private CustomOpenApiWriter SetupSut()
        {
            var service = new CustomOpenApiWriter();
            return service;
        }
    }
}
