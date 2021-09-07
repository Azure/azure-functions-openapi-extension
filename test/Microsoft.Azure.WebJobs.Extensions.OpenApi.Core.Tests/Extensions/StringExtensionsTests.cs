using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void Given_Null_StringValues_When_ToArray_Invoked_Then_It_Should_Return_Result()
        {
            var sv = default(StringValues);

            var result = StringExtensions.ToArray(sv, ",");

            result.Should().BeEquivalentTo(new string[0]);
        }

        [DataTestMethod]
        [DataRow("hello world")]
        [DataRow("hello", "world")]
        public void Given_StringValues_When_ToArray_Invoked_Then_It_Should_Return_Result(params string[] values)
        {
            var sv = new StringValues(values);

            var result = StringExtensions.ToArray(sv, ",");

            result.Should().BeEquivalentTo(values);
        }
    }
}
