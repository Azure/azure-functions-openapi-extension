using Microsoft.OpenApi;
using Microsoft.OpenApi.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
{
    [TestClass]
    public class OpenApiWriterFactoryTests
    {
        [TestMethod]
        public void CreateInstance_Should_Return_Json_Writer()
        {
            // Arrange
            OpenApiFormat format = OpenApiFormat.Json;
            StringWriter writer = new StringWriter();

            // Act
            IOpenApiWriter result = OpenApiWriterFactory.CreateInstance(format, writer);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OpenApiJsonWriter));
        }

        [TestMethod]
        public void CreateInstance_Should_Return_Yaml_Writer()
        {
            // Arrange
            OpenApiFormat format = OpenApiFormat.Yaml;
            StringWriter writer = new StringWriter();

            // Act
            IOpenApiWriter result = OpenApiWriterFactory.CreateInstance(format, writer);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OpenApiYamlWriter));
        }
    }
}
