using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests.Extensions
{
    [TestClass]
    public class OpenApiSchemaExtensionsTests
    {
        [DataTestMethod]
        [DataRow("array", null, false, false, false)]
        [DataRow("object", "uri", false, false, false)]
        [DataRow("object", null, true, false, false)]
        [DataRow("object", null, false, true, false)]
        [DataRow("object", null, false, false, true)]
        public void Given_Instance_When_IsOpenApiSchemaObject_Invoked_Then_It_Should_Return_Result(string type, string format, bool items, bool additionalProperties, bool expected)
        {
            var schema = new OpenApiSchema()
            {
                Type = type,
                Format = format,
                Items = items ? new OpenApiSchema() : null,
                AdditionalProperties = additionalProperties ? new OpenApiSchema() : null,
            };

            var result = OpenApiSchemaExtensions.IsOpenApiSchemaObject(schema);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("object", null, false, false, false)]
        [DataRow("array", "uri", false, false, false)]
        [DataRow("array", null, true, false, true)]
        [DataRow("array", null, false, true, false)]
        [DataRow("array", null, false, false, false)]
        public void Given_Instance_When_IsOpenApiSchemaArray_Invoked_Then_It_Should_Return_Result(string type, string format, bool items, bool additionalProperties, bool expected)
        {
            var schema = new OpenApiSchema()
            {
                Type = type,
                Format = format,
                Items = items ? new OpenApiSchema() : null,
                AdditionalProperties = additionalProperties ? new OpenApiSchema() : null,
            };

            var result = OpenApiSchemaExtensions.IsOpenApiSchemaArray(schema);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("array", null, false, false, false)]
        [DataRow("object", "uri", false, false, false)]
        [DataRow("object", null, true, false, false)]
        [DataRow("object", null, false, true, true)]
        [DataRow("object", null, false, false, false)]
        public void Given_Instance_When_IsOpenApiSchemaDictionary_Invoked_Then_It_Should_Return_Result(string type, string format, bool items, bool additionalProperties, bool expected)
        {
            var schema = new OpenApiSchema()
            {
                Type = type,
                Format = format,
                Items = items ? new OpenApiSchema() : null,
                AdditionalProperties = additionalProperties ? new OpenApiSchema() : null,
            };

            var result = OpenApiSchemaExtensions.IsOpenApiSchemaDictionary(schema);

            result.Should().Be(expected);
        }
    }
}
