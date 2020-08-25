using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiRequestBodyAttributeTests
    {
        [TestMethod]
        public void Given_Null_Constructor_Should_Throw_Exception()
        {
            Action action = () => new OpenApiRequestBodyAttribute(null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new OpenApiRequestBodyAttribute("hello world", null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Value_Property_Should_Return_Value()
        {
            var contentType = "Hello World";
            var bodyType = typeof(object);
            var attribute = new OpenApiRequestBodyAttribute(contentType, bodyType);

            attribute.ContentType.Should().BeEquivalentTo(contentType);
            attribute.BodyType.Should().Be(bodyType);
            attribute.Description.Should().BeNullOrWhiteSpace();
            attribute.Required.Should().Be(false);
        }
    }
}
