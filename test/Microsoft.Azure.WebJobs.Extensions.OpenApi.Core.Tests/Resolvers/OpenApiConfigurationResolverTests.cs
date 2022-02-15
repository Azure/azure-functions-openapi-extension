using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Resolvers
{
    [TestClass]
    public class OpenApiConfigurationResolverTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(OpenApiConfigurationResolver)
                .Should().HaveMethod("Resolve", new[] { typeof(Assembly) })
                .Which.Should().Return<IOpenApiConfigurationOptions>();
        }

        [TestMethod]
        public void Given_ExecutingAssembly_When_Resolve_Invoked_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var result = OpenApiConfigurationResolver.Resolve(assembly);

            result.Should().BeOfType<DefaultOpenApiConfigurationOptions>();
        }

        [TestMethod]
        public void Given_Assembly_When_Resolve_Invoked_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetAssembly(typeof(DefaultOpenApiConfigurationOptions));

            var result = OpenApiConfigurationResolver.Resolve(assembly);

            result.Should().BeOfType<DefaultOpenApiConfigurationOptions>();
        }

        // NOTE: this abstract class is referenced via the OpenApiConfigurationResolver.Resolve method (and will be ignored)
        public abstract class TestOpenApiConfigurationOptionsBase : DefaultOpenApiConfigurationOptions
        {
        }
    }
}
