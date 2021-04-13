using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class RouteConstraintFilterTests
    {
        [DataTestMethod]
        [DataRow("{name}", "{name}")]
        [DataRow("{name:int}", "{name}")]
        [DataRow("{*name}", "{*name}")]
        [DataRow("{*name:int}", "{*name}")]
        public void Given_Value_When_Filter_Applied_Then_It_Should_Return_Result(string input, string expected)
        {
            var filter = new RouteConstraintFilter();

            var result = filter.Filter.Replace(input, filter.Replacement);

            result.Should().Be(expected);
        }
    }
}
