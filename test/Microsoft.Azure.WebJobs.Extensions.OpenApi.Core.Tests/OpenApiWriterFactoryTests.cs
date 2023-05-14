using FluentAssertions;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
{
    [TestClass]
    public class OpenApiWriterFactoryTests
    {
        private StringWriter _writer;

        [TestInitialize]
        public void Init()
        {
            _writer = new StringWriter();
        }

        [DataTestMethod]
        [DataRow(OpenApiFormat.Json)]
        public void CreateInstance_Should_Return_OpenApiJsonWriter(OpenApiFormat format)
        {
            var result = OpenApiWriterFactory.CreateInstance(format, _writer);

            result.Should().NotBeNull();
            result.Should().BeOfType<OpenApiJsonWriter>();
        }

        [DataTestMethod]
        [DataRow(OpenApiFormat.Yaml)]
        public void CreateInstance_Should_Return_OpenApiYamlWriter(OpenApiFormat format)
        {
            var result = OpenApiWriterFactory.CreateInstance(format, _writer);

            result.Should().NotBeNull();
            result.Should().BeOfType<OpenApiYamlWriter>();
        }
    }
}
