using System;
using System.Net;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiResponseWithBodyAttributeTests
    {
        [TestMethod]
        public void Given_Null_Constructor_Should_Throw_Exception()
        {
            var statusCode = HttpStatusCode.OK;

            Action action = () => new OpenApiResponseWithBodyAttribute(statusCode, null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new OpenApiResponseWithBodyAttribute(statusCode, "hello world", null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Value_Property_Should_Return_Value()
        {
            var statusCode = HttpStatusCode.OK;
            var contentType = "Hello World";
            var bodyType = typeof(object);
            var attribute = new OpenApiResponseWithBodyAttribute(statusCode, contentType, bodyType);

            attribute.StatusCode.Should().Be(statusCode);
            attribute.ContentType.Should().BeEquivalentTo(contentType);
            attribute.BodyType.Should().Be(bodyType);
            attribute.Summary.Should().BeNullOrWhiteSpace();
            attribute.Description.Should().BeNullOrWhiteSpace();
        }
    }
}
