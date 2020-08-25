using System.Collections.Generic;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
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
    }
}
