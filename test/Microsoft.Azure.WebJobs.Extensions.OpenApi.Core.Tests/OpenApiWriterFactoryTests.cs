using FluentAssertions;
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
        private StringWriter _writer;

        [TestInitialize]
        public void Init()
        {
            _writer = new StringWriter();
        }

        [DataTestMethod]
        [DataRow(OpenApiFormat.Json, typeof(OpenApiJsonWriter))]
        [DataRow(OpenApiFormat.Yaml, typeof(OpenApiYamlWriter))]
        public void Given_OpenApiFormat_When_CreateInstance_Then_Should_Return_Correct_OpenApiWriter(OpenApiFormat format, Type expected)
        {
            var result = OpenApiWriterFactory.CreateInstance(format, _writer);

            result.Should().NotBeNull();
            result.Should().BeOfType(expected);
        }
    }
}
