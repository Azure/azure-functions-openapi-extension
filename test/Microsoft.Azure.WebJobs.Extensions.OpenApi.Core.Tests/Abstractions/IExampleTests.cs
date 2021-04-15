using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.OpenApi.Any;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Abstractions
{
    [TestClass]
    public class IExampleTests
    {
        [TestMethod]
        public void Given_Type_When_Instantiated_Then_It_Should_Return_Result()
        {
            var example = new FakeExample();

            var result = example.Build().Examples;

            result["first"].Value.Should().BeOfType<OpenApiString>();
        }
    }
}
