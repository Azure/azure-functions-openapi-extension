using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
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
        [DataRow(null, 0)]
        [DataRow(typeof(FakeModel), 0)]
        [DataRow(typeof(FakeExample), 3)]
        public void Given_OpenApiRequestBodyAttribute_With_Example_When_ToOpenApiMediaType_Invoked_Then_It_Should_Return_Result(Type example, int count)
        {
            var contentType = "application/json";
            var bodyType = typeof(object);
            var attribute = new OpenApiRequestBodyAttribute(contentType, bodyType)
            {
                Example = example,
            };
            var namingStrategy = new CamelCaseNamingStrategy();

            var result = OpenApiPayloadAttributeExtensions.ToOpenApiMediaType(attribute, namingStrategy);

            result.Examples.Should().NotBeNull();
            result.Examples.Should().HaveCount(count);
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

        [DataTestMethod]
        [DataRow(null, 0, OpenApiVersionType.V2)]
        [DataRow(typeof(FakeModel), 0, OpenApiVersionType.V2)]
        [DataRow(typeof(FakeExample), 3, OpenApiVersionType.V2)]
        [DataRow(null, 0, OpenApiVersionType.V3)]
        [DataRow(typeof(FakeModel), 0, OpenApiVersionType.V3)]
        [DataRow(typeof(FakeExample), 3, OpenApiVersionType.V3)]
        public void Given_OpenApiResponseWithBodyAttribute_With_Example_When_ToOpenApiMediaType_Invoked_Then_It_Should_Return_Result(Type example, int count, OpenApiVersionType version)
        {
            var statusCode = HttpStatusCode.OK;
            var contentType = "application/json";
            var bodyType = typeof(object);
            var attribute = new OpenApiResponseWithBodyAttribute(statusCode, contentType, bodyType)
            {
                Example = example,
            };
            var namingStrategy = new CamelCaseNamingStrategy();

            var result = OpenApiPayloadAttributeExtensions.ToOpenApiMediaType(attribute, namingStrategy, version: version);

            result.Examples.Should().NotBeNull();
            result.Examples.Should().HaveCount(count);

            if (count == 0)
            {
                return;
            }

            if (version != OpenApiVersionType.V2)
            {
                result.Example.Should().BeNull();

                return;
            }

            var instance = (dynamic)Activator.CreateInstance(example);
            var examples = (IDictionary<string, OpenApiExample>)instance.Build(namingStrategy).Examples;
            var first = examples.First().Value;

            result.Example.Should().NotBeNull();
            (result.Example as OpenApiString).Value.Should().Be((first.Value as OpenApiString).Value);
        }
    }
}
