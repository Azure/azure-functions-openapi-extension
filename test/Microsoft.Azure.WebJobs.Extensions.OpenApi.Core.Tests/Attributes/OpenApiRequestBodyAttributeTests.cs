using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiRequestBodyAttributeTests
    {
        [TestMethod]
        public void Given_Null_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiRequestBodyAttribute(null, null, "GET");
            action.Should().Throw<ArgumentNullException>();

            action = () => new OpenApiRequestBodyAttribute("hello world", null, "GET");
            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("Hello World", typeof(object))]
        public void Given_Parameters_When_Instantiated_It_Should_Return_Value(string contentType, Type bodyType)
        {
            var attribute = new OpenApiRequestBodyAttribute(contentType, bodyType, "GET");

            attribute.ContentType.Should().BeEquivalentTo(contentType);
            attribute.BodyType.Should().Be(bodyType);
        }

        [DataTestMethod]
        [DataRow("Lorem Ipsum", true, true, typeof(FakeExample))]
        [DataRow("Lorem Ipsum", true, false, typeof(FakeExample))]
        [DataRow("Lorem Ipsum", false, true, typeof(FakeExample))]
        [DataRow("Lorem Ipsum", false, false, typeof(FakeExample))]
        public void Given_Properties_When_Instantiated_It_Should_Return_Value(string description, bool required, bool deprecated, Type example)
        {
            var contentType = "Hello World";
            var bodyType = typeof(object);
            var attribute = new OpenApiRequestBodyAttribute(contentType, bodyType, "GET")
            {
                Description = description,
                Required = required,
                Deprecated = deprecated,
                Example = example
            };

            attribute.Description.Should().Be(description);
            attribute.Required.Should().Be(required);
            attribute.Deprecated.Should().Be(deprecated);
            attribute.Example.Should().Be(example);
        }
    }
}
