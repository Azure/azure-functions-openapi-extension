using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.OpenApi.Any;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Resolvers
{
    [TestClass]
    public class OpenApiExampleResolverTests
    {
        [TestMethod]
        public void Given_Null_When_Resolve_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => OpenApiExampleResolver.Resolve(null, (object)null);
            action.Should().Throw<ArgumentNullException>();

            action = () => OpenApiExampleResolver.Resolve("name", (object)null);
            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("name", typeof(DefaultNamingStrategy))]
        [DataRow("name", typeof(CamelCaseNamingStrategy))]
        public void Given_Name_When_Resolve_Invoked_Then_It_Should_Return_Result(string name, Type strategy)
        {
            var namingStrategy = (NamingStrategy)Activator.CreateInstance(strategy);
            var instance = new FakeClassModel() { Number = 1, Text = "Hello World" };

            var result = OpenApiExampleResolver.Resolve(name, instance, namingStrategy);

            result.Key.Should().Be(name);
            result.Value.Value.Should().BeOfType<OpenApiString>();
            if (strategy == typeof(DefaultNamingStrategy))
            {
                (result.Value.Value as OpenApiString).Value.Should().Contain("{\"Number\":");
            }
            if (strategy == typeof(CamelCaseNamingStrategy))
            {
                (result.Value.Value as OpenApiString).Value.Should().Contain("{\"number\":");
            }
        }

        [DataTestMethod]
        [DataRow("name", "summary", typeof(DefaultNamingStrategy))]
        [DataRow("name", "summary", typeof(CamelCaseNamingStrategy))]
        public void Given_Summary_When_Resolve_Invoked_Then_It_Should_Return_Result(string name, string summary, Type strategy)
        {
            var namingStrategy = (NamingStrategy)Activator.CreateInstance(strategy);
            var instance = new FakeClassModel() { Number = 1, Text = "Hello World" };

            var result = OpenApiExampleResolver.Resolve(name, summary, instance, namingStrategy);

            result.Key.Should().Be(name);
            result.Value.Summary.Should().Be(summary);
            result.Value.Value.Should().BeOfType<OpenApiString>();
            if (strategy == typeof(DefaultNamingStrategy))
            {
                (result.Value.Value as OpenApiString).Value.Should().Contain("{\"Number\":");
            }
            if (strategy == typeof(CamelCaseNamingStrategy))
            {
                (result.Value.Value as OpenApiString).Value.Should().Contain("{\"number\":");
            }
        }

        [DataTestMethod]
        [DataRow("name", "summary", "description", typeof(DefaultNamingStrategy))]
        [DataRow("name", "summary", "description", typeof(CamelCaseNamingStrategy))]
        public void Given_Description_When_Resolve_Invoked_Then_It_Should_Return_Result(string name, string summary, string description, Type strategy)
        {
            var namingStrategy = (NamingStrategy)Activator.CreateInstance(strategy);
            var instance = new FakeClassModel() { Number = 1, Text = "Hello World" };

            var result = OpenApiExampleResolver.Resolve(name, summary, description, instance, namingStrategy);

            result.Key.Should().Be(name);
            result.Value.Summary.Should().Be(summary);
            result.Value.Description.Should().Be(description);
            result.Value.Value.Should().BeOfType<OpenApiString>();
            if (strategy == typeof(DefaultNamingStrategy))
            {
                (result.Value.Value as OpenApiString).Value.Should().Contain("{\"Number\":");
            }
            if (strategy == typeof(CamelCaseNamingStrategy))
            {
                (result.Value.Value as OpenApiString).Value.Should().Contain("{\"number\":");
            }
        }
    }
}
