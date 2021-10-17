using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
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
        [DataTestMethod]
        [DataRow(typeof(string), false)]
        [DataRow(typeof(int), false)]
        [DataRow(typeof(double), false)]
        [DataRow(typeof(bool), false)]
        [DataRow(typeof(Array), true)]
        [DataRow(typeof(string[]), true)]
        [DataRow(typeof(List<string>), true)]
        [DataRow(typeof(IList<string>), true)]
        [DataRow(typeof(ICollection<string>), true)]
        [DataRow(typeof(IEnumerable<string>), true)]
        [DataRow(typeof(IReadOnlyList<string>), true)]
        [DataRow(typeof(IReadOnlyCollection<string>), true)]
        [DataRow(typeof(HashSet<string>), true)]
        [DataRow(typeof(ISet<string>), true)]
        [DataRow(typeof(Dictionary<string, string>), false)]
        [DataRow(typeof(IDictionary<string, string>), false)]
        [DataRow(typeof(IReadOnlyDictionary<string, string>), false)]
        [DataRow(typeof(KeyValuePair<string, string>), false)]
        public void Given_ArrayTypes_When_IsOpenApiArray_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = TypeExtensions.IsOpenApiArray(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(string), false)]
        [DataRow(typeof(int), false)]
        [DataRow(typeof(double), false)]
        [DataRow(typeof(bool), false)]
        [DataRow(typeof(Array), false)]
        [DataRow(typeof(string[]), false)]
        [DataRow(typeof(List<string>), false)]
        [DataRow(typeof(IList<string>), false)]
        [DataRow(typeof(ICollection<string>), false)]
        [DataRow(typeof(IEnumerable<string>), false)]
        [DataRow(typeof(IReadOnlyList<string>), false)]
        [DataRow(typeof(IReadOnlyCollection<string>), false)]
        [DataRow(typeof(HashSet<string>), false)]
        [DataRow(typeof(ISet<string>), false)]
        [DataRow(typeof(Dictionary<string, string>), true)]
        [DataRow(typeof(IDictionary<string, string>), true)]
        [DataRow(typeof(IReadOnlyDictionary<string, string>), true)]
        [DataRow(typeof(KeyValuePair<string, string>), true)]
        public void Given_DictionaryTypes_When_IsOpenApiDictionary_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = TypeExtensions.IsOpenApiDictionary(type);

            result.Should().Be(expected);
        }

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

        [DataTestMethod]
        [DataRow(typeof(Exception), true)]
        [DataRow(typeof(NullReferenceException), true)]
        [DataRow(typeof(StackOverflowException), true)]
        [DataRow(typeof(AggregateException), true)]
        [DataRow(typeof(ArgumentException), true)]
        [DataRow(typeof(object), false)]
        public void Given_Type_When_IsExceptionType_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = TypeExtensions.IsOpenApiException(type);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_Null_When_HasInterface_Invoked_Then_It_Should_Return_False()
        {
            var result = TypeExtensions.HasInterface<IOpenApiCustomResponseHeader>(null);
            result.Should().BeFalse();

            result = TypeExtensions.HasInterface(null, "IOpenApiCustomResponseHeader");
            result.Should().BeFalse();
        }

        [TestMethod]
        public void Given_Invalid_TypeReference_When_HasInterface_Invoked_Then_It_Should_Return_Result()
        {
            var result = TypeExtensions.HasInterface<IOpenApiCustomResponseHeader>(typeof(FakeModel));

            result.Should().BeFalse();
        }

        [TestMethod]
        public void Given_Valid_TypeReference_When_HasInterface_Invoked_Then_It_Should_Return_Result()
        {
            var result = TypeExtensions.HasInterface<IOpenApiCustomResponseHeader>(typeof(FakeResponseHeader));

            result.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow(typeof(FakeModel), false)]
        [DataRow(typeof(FakeResponseHeader), true)]
        public void Given_Type_When_HasInterface_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = TypeExtensions.HasInterface(type, "IOpenApiCustomResponseHeader");

            result.Should().Be(expected);
        }
    }
}
