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
        [DataRow(OpenApiFormat.Yaml)]
        public void CreateInstance_Should_Return_Correct_OpenApiWriter(OpenApiFormat format)
        {
            var result = OpenApiWriterFactory.CreateInstance(format, _writer);

            result.Should().NotBeNull();
            if (format == OpenApiFormat.Json)
            {
                result.Should().BeOfType<OpenApiJsonWriter>();
            }
            else if (format == OpenApiFormat.Yaml)
            {
                result.Should().BeOfType<OpenApiYamlWriter>();
            }
        }
    }
}
