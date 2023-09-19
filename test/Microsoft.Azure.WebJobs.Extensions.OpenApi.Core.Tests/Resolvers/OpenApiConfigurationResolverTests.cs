using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Serialization;

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

        /// <summary>
        /// This tests are added to cover the switch statement in the Resolve method.
        /// </summary>
        [TestMethod]
        public void Given_CamelCase_When_Resolve_Invoked_Then_It_Should_Return_CamelCaseNamingStrategy()
        {
            var strategyType = OpenApiNamingStrategy.CamelCase;

            var result = OpenApiConfigurationResolver.Resolve(strategyType);

            result.Should().BeOfType<CamelCaseNamingStrategy>();
        }

        [TestMethod]
        public void Given_PascalCase_When_Resolve_Invoked_Then_It_Should_Return_DefaultNamingStrategy()
        {
            var strategyType = OpenApiNamingStrategy.PascalCase;

            var result = OpenApiConfigurationResolver.Resolve(strategyType);

            result.Should().BeOfType<DefaultNamingStrategy>();
        }

        [TestMethod]
        public void Given_SnakeCase_When_Resolve_Invoked_Then_It_Should_Return_SnakeCaseNamingStrategy()
        {
            var strategyType = OpenApiNamingStrategy.SnakeCase;

            var result = OpenApiConfigurationResolver.Resolve(strategyType);

            result.Should().BeOfType<SnakeCaseNamingStrategy>();
        }

        [TestMethod]
        public void Given_KebabCase_When_Resolve_Invoked_Then_It_Should_Return_KebabCaseNamingStrategy()
        {
            var strategyType = OpenApiNamingStrategy.KebabCase;

            var result = OpenApiConfigurationResolver.Resolve(strategyType);

            result.Should().BeOfType<KebabCaseNamingStrategy>();
        }

        [TestMethod]
        public void Given_SnakeCase_number_When_Resolve_Invoked_Then_It_Should_Return_SnakeCaseNamingStrategy()
        {
            var strategyType = (OpenApiNamingStrategy)2; 

            var result = OpenApiConfigurationResolver.Resolve(strategyType);

            result.Should().BeOfType<SnakeCaseNamingStrategy>();
        }

        [TestMethod]
        public void Given_UnexpectedValue_When_Resolve_Invoked_Then_It_Should_Return_CamelCaseNamingStrategy()
        {
            var strategyType = (OpenApiNamingStrategy)999; 

            var result = OpenApiConfigurationResolver.Resolve(strategyType);

            result.Should().BeOfType<CamelCaseNamingStrategy>();
        }

    }
}
