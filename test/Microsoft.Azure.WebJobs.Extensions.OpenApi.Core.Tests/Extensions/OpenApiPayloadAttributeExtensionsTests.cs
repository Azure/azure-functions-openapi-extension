using System;
using System.Collections.Generic;
using System.Net;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class OpenApiPayloadAttributeExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_ToOpenApiMediaType_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => OpenApiPayloadAttributeExtensions.ToOpenApiMediaType((OpenApiPayloadAttribute)null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow(typeof(string), "string")]
        [DataRow(typeof(FakeModel), "object")]
        public void Given_OpenApiRequestBodyAttribute_When_ToOpenApiMediaType_Invoked_Then_It_Should_Return_Result(Type bodyType, string expected)
        {
            var contentType = "application/json";
            var attribute = new OpenApiRequestBodyAttribute(contentType, bodyType)
            {
                Required = true,
                Description = "Dummy request model"
            };
            var namingStrategy = new CamelCaseNamingStrategy();

            var result = OpenApiPayloadAttributeExtensions.ToOpenApiMediaType(attribute, namingStrategy);

            result.Schema.Type.Should().Be(expected);
            result.Schema.Deprecated.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Given_OpenApiRequestBodyAttribute_With_Deprecated_When_ToOpenApiMediaType_Invoked_Then_It_Should_Return_Result(bool deprecated)
        {
            var contentType = "application/json";
            var bodyType = typeof(object);
            var attribute = new OpenApiRequestBodyAttribute(contentType, bodyType)
            {
                Required = true,
                Description = "Dummy request model",
                Deprecated = deprecated,
            };
            var namingStrategy = new CamelCaseNamingStrategy();

            var result = OpenApiPayloadAttributeExtensions.ToOpenApiMediaType(attribute, namingStrategy);

            result.Schema.Deprecated.Should().Be(deprecated);
        }

        [DataTestMethod]
        [DataRow(typeof(string), "string", false, false, null)]
        [DataRow(typeof(FakeModel), "object", false, false, null)]
        [DataRow(typeof(List<string>), "array", true, false, "string")]
        [DataRow(typeof(Dictionary<string, int?>), "object", false, true, "integer")]
        public void Given_OpenApiResponseWithBodyAttribute_When_ToOpenApiMediaType_Invoked_Then_It_Should_Return_Result(Type bodyType, string expected, bool items, bool additionalProperties, string underlyingType)
        {
            var statusCode = HttpStatusCode.OK;
            var contentType = "application/json";
            var attribute = new OpenApiResponseWithBodyAttribute(statusCode, contentType, bodyType);
            var namingStrategy = new CamelCaseNamingStrategy();

            var result = OpenApiPayloadAttributeExtensions.ToOpenApiMediaType(attribute, namingStrategy);

            result.Schema.Type.Should().Be(expected);
            result.Schema.Deprecated.Should().BeFalse();
            if (items)
            {
                result.Schema.Items.Should().NotBeNull();
                result.Schema.Items.Type.Should().Be(underlyingType);
            }
            else
            {
                result.Schema.Items.Should().BeNull();
            }

            if (additionalProperties)
            {
                result.Schema.AdditionalProperties.Should().NotBeNull();
                result.Schema.AdditionalProperties.Type.Should().Be(underlyingType);
            }
            else
            {
                result.Schema.AdditionalProperties.Should().BeNull();
            }
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Given_OpenApiResponseWithBodyAttribute_With_Deprecated_When_ToOpenApiMediaType_Invoked_Then_It_Should_Return_Result(bool deprecated)
        {
            var statusCode = HttpStatusCode.OK;
            var contentType = "application/json";
            var bodyType = typeof(object);
            var attribute = new OpenApiResponseWithBodyAttribute(statusCode, contentType, bodyType)
            {
                Deprecated = deprecated,
            };
            var namingStrategy = new CamelCaseNamingStrategy();

            var result = OpenApiPayloadAttributeExtensions.ToOpenApiMediaType(attribute, namingStrategy);

            result.Schema.Deprecated.Should().Be(deprecated);
        }
    }
}
