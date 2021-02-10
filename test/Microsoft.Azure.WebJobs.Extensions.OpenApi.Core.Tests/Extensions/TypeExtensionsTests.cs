using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using TypeExtensions = Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions.TypeExtensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
        public void Given_IList_Should_Return_True() =>
            typeof(IList<string>).IsOpenApiArray().Should().BeTrue();

        [TestMethod]
        public void Given_List_Should_Return_True() =>
            typeof(List<string>).IsOpenApiArray().Should().BeTrue();

        [TestMethod]
        public void Given_Array_Method_Should_Return_True() =>
            typeof(string[]).IsOpenApiArray().Should().BeTrue();

        [TestMethod]
        public void Given_Object_That_Extends_List_Should_Return_False() =>
            typeof(JObject).IsOpenApiArray().Should().BeFalse();

        [TestMethod]
        public void Given_String_Method_Should_Return_False() =>
            typeof(string).IsOpenApiArray().Should().BeFalse();

        [TestMethod]
        public void Given_DefaultNamingStrategy_When_GetOpenApiTypeName_Invoked_Then_It_Should_Return_Result()
        {
            var type = typeof(int);
            var strategy = new DefaultNamingStrategy();

            var result = TypeExtensions.GetOpenApiTypeName(type, strategy);

            result.Should().Be("Int32");
        }

        [TestMethod]
        public void Given_CamelCaseNamingStrategy_When_GetOpenApiTypeName_Invoked_Then_It_Should_Return_Result()
        {
            var type = typeof(int);
            var strategy = new CamelCaseNamingStrategy();

            var result = TypeExtensions.GetOpenApiTypeName(type, strategy);

            result.Should().Be("int32");
        }

        [DataTestMethod]
        [DataRow(typeof(int))]
        [DataRow(typeof(string))]
        [DataRow(typeof(FakeModel))]
        public void Given_NonGenericType_When_GetUnderlyingType_Invoked_Then_It_Should_Return_Null(Type type)
        {
            var result = TypeExtensions.GetUnderlyingType(type);

            result.Should().BeNull();
        }

        [DataTestMethod]
        [DataRow(typeof(int?), typeof(int))]
        [DataRow(typeof(bool?), typeof(bool))]
        [DataRow(typeof(DateTime?), typeof(DateTime))]
        public void Given_NullableType_When_GetUnderlyingType_Invoked_Then_It_Should_Return_Result(Type type, Type expected)
        {
            var result = TypeExtensions.GetUnderlyingType(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(List<int>), typeof(int))]
        [DataRow(typeof(List<bool>), typeof(bool))]
        [DataRow(typeof(List<FakeModel>), typeof(FakeModel))]
        public void Given_ListType_When_GetUnderlyingType_Invoked_Then_It_Should_Return_Result(Type type, Type expected)
        {
            var result = TypeExtensions.GetUnderlyingType(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(List<int?>), typeof(int))]
        [DataRow(typeof(List<bool?>), typeof(bool))]
        [DataRow(typeof(List<DateTime?>), typeof(DateTime))]
        public void Given_NullableListType_When_GetUnderlyingType_Invoked_Then_It_Should_Return_Result(Type type, Type expected)
        {
            var result = TypeExtensions.GetUnderlyingType(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, int>), typeof(int))]
        [DataRow(typeof(Dictionary<string, bool>), typeof(bool))]
        [DataRow(typeof(Dictionary<string, FakeModel>), typeof(FakeModel))]
        public void Given_DictionaryType_When_GetUnderlyingType_Invoked_Then_It_Should_Return_Result(Type type, Type expected)
        {
            var result = TypeExtensions.GetUnderlyingType(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, int?>), typeof(int))]
        [DataRow(typeof(Dictionary<string, bool?>), typeof(bool))]
        [DataRow(typeof(Dictionary<string, DateTime?>), typeof(DateTime))]
        public void Given_NullableDictionaryType_When_GetUnderlyingType_Invoked_Then_It_Should_Return_Result(Type type, Type expected)
        {
            var result = TypeExtensions.GetUnderlyingType(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(int), false)]
        [DataRow(typeof(int?), false)]
        [DataRow(typeof(List<int>), true)]
        [DataRow(typeof(FakeModel), true)]
        [DataRow(typeof(JObject), false)]
        public void Given_Type_When_IsReferentialType_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = TypeExtensions.IsReferentialType(type);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_Null_When_HasInterface_Invoked_Then_It_Should_Return_False()
        {
            var result = TypeExtensions.HasInterface<IOpenApiResponseHeaderType>(null);
            result.Should().BeFalse();

            result = TypeExtensions.HasInterface(null, "IOpenApiResponseHeaderType");
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Given_Invalid_TypeReference_When_HasInterface_Invoked_Then_It_Should_Return_Result()
        {
            var result = TypeExtensions.HasInterface<IOpenApiResponseHeaderType>(typeof(FakeModel));

            result.Should().BeFalse();
        }

        [TestMethod]
        public void Given_Valid_TypeReference_When_HasInterface_Invoked_Then_It_Should_Return_Result()
        {
            var result = TypeExtensions.HasInterface<IOpenApiResponseHeaderType>(typeof(FakeResponseHeaderType));

            result.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow(typeof(FakeModel), false)]
        [DataRow(typeof(FakeResponseHeaderType), true)]
        public void Given_Type_When_HasInterface_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = TypeExtensions.HasInterface(type, "IOpenApiResponseHeaderType");

            result.Should().Be(expected);
        }
    }
}
