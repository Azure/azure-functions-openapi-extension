using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Extension;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Service.Impl;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Tests.Service
{
    [TestClass]
    public class CustomOpenApiCreatorTests
    {
        [TestMethod]
        public async Task CreateOpenApiDocument()
        {
            // Arrange
            var apiBaseUrl = "http://test.function.com/";
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var compiledDllPath = $"{path}/Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionApp.V3Net5.dll";
            var hostJsonPath = $"{path}/host.json";
            var httpSettings = hostJsonPath.SetHostSettings();
            var openApiInfo = compiledDllPath.SetOpenApiInfo();
            var openApiVersionType = OpenApiVersionType.V3;
            var openApiFormatType = OpenApiFormatType.Json;

            var service = this.SetupSut();

            // Act
            var result = await service.CreateOpenApiDocument(
                apiBaseUrl,
                compiledDllPath,
                httpSettings.RoutePrefix,
                openApiInfo,
                openApiVersionType,
                openApiFormatType);

            // Assert
            result.Should().NotBeNull();
        }

        private CustomOpenApiCreator SetupSut()
        {
            var service = new CustomOpenApiCreator();
            return service;
        }
    }
}
