using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Resolvers
{
    [TestClass]
    public class OpenApiCustomUIResolverTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(OpenApiCustomUIResolver)
                .Should().HaveMethod("Resolve", new[] { typeof(Assembly) })
                .Which.Should().Return<IOpenApiCustomUIOptions>();
        }

        [TestMethod]
        public void Given_ExecutingAssembly_When_Resolve_Invoked_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var result = OpenApiCustomUIResolver.Resolve(assembly);

            result.Should().BeOfType<DefaultOpenApiCustomUIOptions>();
        }

        [TestMethod]
        public void Given_Assembly_When_Resolve_Invoked_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetAssembly(typeof(DefaultOpenApiCustomUIOptions));

            var result = OpenApiCustomUIResolver.Resolve(assembly);

            result.Should().BeOfType<DefaultOpenApiCustomUIOptions>();
        }

        [TestMethod]
        public void Given_An_Assembly_With_An_Abstract_Base_Configuration_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetAssembly(typeof(FakeFileCustomUIOptions));

            var result = OpenApiCustomUIResolver.Resolve(assembly);

            result.Should().BeOfType<FakeFileCustomUIOptions>();
        }
    }
}
