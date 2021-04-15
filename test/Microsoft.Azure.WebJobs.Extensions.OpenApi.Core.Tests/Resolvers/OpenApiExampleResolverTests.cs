using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.OpenApi.Any;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        [DataRow("name")]
        public void Given_Name_When_Resolve_Invoked_Then_It_Should_Return_Result(string name)
        {
            var instance = new FakeClassModel() { Number = 1, Text = "Hello World" };

            var result = OpenApiExampleResolver.Resolve(name, instance);

            result.Key.Should().Be(name);
            result.Value.Value.Should().BeOfType<OpenApiString>();
            (result.Value.Value as OpenApiString).Value.Should().Contain("{\"Number\":");
        }

        [DataTestMethod]
        [DataRow("name", "summary")]
        public void Given_Summary_When_Resolve_Invoked_Then_It_Should_Return_Result(string name, string summary)
        {
            var instance = new FakeClassModel() { Number = 1, Text = "Hello World" };

            var result = OpenApiExampleResolver.Resolve(name, summary, instance);

            result.Key.Should().Be(name);
            result.Value.Summary.Should().Be(summary);
            result.Value.Value.Should().BeOfType<OpenApiString>();
            (result.Value.Value as OpenApiString).Value.Should().Contain("{\"Number\":");
        }

        [DataTestMethod]
        [DataRow("name", "summary", "description")]
        public void Given_Description_When_Resolve_Invoked_Then_It_Should_Return_Result(string name, string summary, string description)
        {
            var instance = new FakeClassModel() { Number = 1, Text = "Hello World" };

            var result = OpenApiExampleResolver.Resolve(name, summary, description, instance);

            result.Key.Should().Be(name);
            result.Value.Summary.Should().Be(summary);
            result.Value.Description.Should().Be(description);
            result.Value.Value.Should().BeOfType<OpenApiString>();
            (result.Value.Value as OpenApiString).Value.Should().Contain("{\"Number\":");
        }
    }
}
