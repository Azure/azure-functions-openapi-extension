using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.OpenApi.Any;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Abstractions
{
    [TestClass]
    public class IExampleTests
    {
        [TestMethod]
        public void Given_DefaultNamingStrategy_When_Instantiated_Then_It_Should_Return_Result()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeExample();

            var result = example.Build(namingStrategy).Examples;

            result["first"].Value.Should().BeOfType<OpenApiString>();
            (result["first"].Value as OpenApiString).Value.Should().Contain("{\"Number\":");
        }

        [TestMethod]
        public void Given_CamelCaseNamingStrategy_When_Instantiated_Then_It_Should_Return_Result()
        {
            var namingStrategy = new CamelCaseNamingStrategy();
            var example = new FakeExample();

            var result = example.Build(namingStrategy).Examples;

            result["first"].Value.Should().BeOfType<OpenApiString>();
            (result["first"].Value as OpenApiString).Value.Should().Contain("{\"number\":");
        }
    }
}
