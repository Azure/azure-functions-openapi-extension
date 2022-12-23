using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.OpenApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class EnumExtensionsTests
    {
        [DataTestMethod]
        [DataRow(FakeStringEnum.StringValue1, "lorem")]
        [DataRow(FakeStringEnum.StringValue2, "ipsum")]
        [DataRow(FakeStringEnum.StringValue3, "dolor")]
        [DataRow(FakeStringEnum.StringValue4, "StringValue4")]
        public void Given_Enum_Method_Should_Return_Value(FakeStringEnum @enum, string expected)
        {
            var name = EnumExtensions.ToDisplayName(@enum);

            name.Should().Be(expected);
        }

        [TestMethod]
        public void Given_TypeCode_ToDataType_Should_Throw_Exception()
        {
            Action action = () => EnumExtensions.ToDataType(null);
            action.Should().Throw<InvalidOperationException>();
        }

        [DataTestMethod]
        [DataRow(typeof(short), "integer")]
        [DataRow(typeof(int), "integer")]
        [DataRow(typeof(long), "integer")]
        [DataRow(typeof(float), "number")]
        [DataRow(typeof(double), "number")]
        [DataRow(typeof(decimal), "number")]
        [DataRow(typeof(bool), "boolean")]
        [DataRow(typeof(DateTime), "string")]
        [DataRow(typeof(DateTimeOffset), "string")]
        [DataRow(typeof(Guid), "string")]
        [DataRow(typeof(TimeSpan), "string")]
        [DataRow(typeof(object), "object")]
        public void Given_TypeCode_ToDataType_Should_Return_Value(Type type, string expected)
        {
            var dataType = EnumExtensions.ToDataType(type);

            dataType.Should().Be(expected);
        }

        [TestMethod]
        public void Given_TypeCode_ToDataFormat_Should_Throw_Exception()
        {
            Action action = () => EnumExtensions.ToDataFormat(null);
            action.Should().Throw<InvalidOperationException>();
        }

        [DataTestMethod]
        [DataRow(typeof(short), "int32")]
        [DataRow(typeof(int), "int32")]
        [DataRow(typeof(long), "int64")]
        [DataRow(typeof(float), "float")]
        [DataRow(typeof(double), "double")]
        [DataRow(typeof(decimal), "double")]
        [DataRow(typeof(bool), null)]
        [DataRow(typeof(DateTime), "date-time")]
        [DataRow(typeof(DateTimeOffset), "date-time")]
        [DataRow(typeof(Guid), "uuid")]
        [DataRow(typeof(TimeSpan), "timespan")]
        [DataRow(typeof(object), null)]
        public void Given_TypeCode_ToDataFormat_Should_Return_Value(Type type, string expected)
        {
            var dataType = EnumExtensions.ToDataFormat(type);

            dataType.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(OpenApiFormat.Json, "application/json")]
        [DataRow(OpenApiFormat.Yaml, "text/vnd.yaml")]
        public void Given_OpenApiFormat_When_GetContentType_Invoked_Then_It_Should_Return_Result(OpenApiFormat format, string expected)
        {
            var result = EnumExtensions.GetContentType(format);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(OpenApiVersionType.V2, OpenApiSpecVersion.OpenApi2_0)]
        [DataRow(OpenApiVersionType.V3, OpenApiSpecVersion.OpenApi3_0)]
        public void Given_OpenApiVersionType_When_ToOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(OpenApiVersionType version, OpenApiSpecVersion expected)
        {
            var result = EnumExtensions.ToOpenApiSpecVersion(version);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(OpenApiFormatType.Json, OpenApiFormat.Json)]
        [DataRow(OpenApiFormatType.Yaml, OpenApiFormat.Yaml)]
        public void Given_OpenApiFormatType_When_ToOpenApiFormat_Invoked_Then_It_Should_Return_Result(OpenApiFormatType format, OpenApiFormat expected)
        {
            var result = EnumExtensions.ToOpenApiFormat(format);

            result.Should().Be(expected);
        }
    }
}
