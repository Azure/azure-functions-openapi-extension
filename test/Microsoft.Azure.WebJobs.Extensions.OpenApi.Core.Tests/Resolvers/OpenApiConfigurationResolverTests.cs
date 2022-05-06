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

        [TestMethod]
        public void Given_An_Assembly_With_An_Abstract_Base_Configuration_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetAssembly(typeof(FakeOpenApiConfigurationOptions));

            var result = OpenApiConfigurationResolver.Resolve(assembly);

            result.Should().BeOfType<FakeOpenApiConfigurationOptions>();
            result.GetType().BaseType.Should().Be(typeof(FakeOpenApiConfigurationOptionsBase)); // This verifies the abstract type was considered in the resolution
        }

        [TestMethod]
        public void Given_ConfigurationOptions_Is_Set_Then_It_Should_Return_Result()
        {
            var updatedOpenApiConfig = new DefaultOpenApiConfigurationOptions
            {
                Info = new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "A test title"
                }
            };

            OpenApiConfigurationResolver.ConfigurationOptions = updatedOpenApiConfig;

            var assembly = Assembly.GetAssembly(typeof(DefaultOpenApiConfigurationOptions));

            var result = OpenApiConfigurationResolver.Resolve(assembly);

            result.Should().BeOfType<DefaultOpenApiConfigurationOptions>();
            result.Info.Title.Should().Be("A test title");
        }
    }
}
