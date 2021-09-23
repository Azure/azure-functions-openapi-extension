using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.OpenApi.Any;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Serialization;
using System;

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

        [TestMethod]
        public void Given_IntType_When_Instantiated_Then_It_Should_Return_Result()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeExample();

            var result = example.Build(namingStrategy).Examples;

            result["intValue"].Value.Should().BeOfType<OpenApiInteger>();
            (result["intValue"].Value as OpenApiInteger).Value.Should().Be(1);
        }

        [TestMethod]
        public void Given_StringType_When_Instantiated_Then_It_Should_Return_Result()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeExample();

            var result = example.Build(namingStrategy).Examples;

            result["stringValue"].Value.Should().BeOfType<OpenApiString>();
            (result["stringValue"].Value as OpenApiString).Value.Should().Be("stringValue");
        }

        [TestMethod]
        public void Given_doubleType_When_Instantiated_Then_It_Should_Return_Result()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeExample();

            var result = example.Build(namingStrategy).Examples;

            result["doubleValue"].Value.Should().BeOfType<OpenApiDouble>();
            (result["doubleValue"].Value as OpenApiDouble).Value.Should().Be(0.123);
        }

        [TestMethod]
        public void Given_datetimeType_When_Instantiated_Then_It_Should_Return_Result()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeExample();

            var result = example.Build(namingStrategy).Examples;

            result["date-timeValue"].Value.Should().BeOfType<OpenApiDateTime>();
            (result["date-timeValue"].Value as OpenApiDateTime).Value.Should().Be(Convert.ToDateTime("2021.01.01"));
        }

        [TestMethod]
        public void Given_booleanType_When_Instantiated_Then_It_Should_Return_Result()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeExample();

            var result = example.Build(namingStrategy).Examples;

            result["booleanValue"].Value.Should().BeOfType<OpenApiBoolean>();
            (result["booleanValue"].Value as OpenApiBoolean).Value.Should().Be(false);
        }
    }
}
