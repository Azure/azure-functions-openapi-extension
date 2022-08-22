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
    public class OpenApiHttpTriggerAuthorizationResolverTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(OpenApiHttpTriggerAuthorizationResolver)
                .Should().HaveMethod("Resolve", new[] { typeof(Assembly) })
                .Which.Should().Return<IOpenApiHttpTriggerAuthorization>();
        }

        [TestMethod]
        public void Given_ExecutingAssembly_When_Resolve_Invoked_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var result = OpenApiHttpTriggerAuthorizationResolver.Resolve(assembly);

            result.Should().BeOfType<DefaultOpenApiHttpTriggerAuthorization>();
        }

        [TestMethod]
        public void Given_Assembly_When_Resolve_Invoked_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetAssembly(typeof(DefaultOpenApiHttpTriggerAuthorization));

            var result = OpenApiHttpTriggerAuthorizationResolver.Resolve(assembly);

            result.Should().BeOfType<DefaultOpenApiHttpTriggerAuthorization>();
        }
    }
}
