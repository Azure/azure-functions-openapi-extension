using System;
using System.Net;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiResponseWithBodyAttributeTests
    {
        [TestMethod]
        public void Given_Null_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            var statusCode = HttpStatusCode.OK;

            Action action = () => new OpenApiResponseWithBodyAttribute(statusCode, null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new OpenApiResponseWithBodyAttribute(statusCode, "hello world", null);
            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK, "application/json", typeof(object))]
        public void Given_Parameters_When_Instantiated_Then_It_Should_Return_Value(HttpStatusCode statusCode, string contentType, Type bodyType)
        {
            var attribute = new OpenApiResponseWithBodyAttribute(statusCode, contentType, bodyType);

            attribute.StatusCode.Should().Be(statusCode);
            attribute.ContentType.Should().BeEquivalentTo(contentType);
            attribute.BodyType.Should().Be(bodyType);
        }

        [DataTestMethod]
        [DataRow("Lorem Ipsum", "Hello World", true, typeof(FakeResponseHeaderType))]
        [DataRow("Lorem Ipsum", "Hello World", false, typeof(FakeResponseHeaderType))]
        public void Given_Properties_When_Instantiated_Then_It_Should_Return_Value(string summary, string description, bool deprecated, Type headerType)
        {
            var statusCode = HttpStatusCode.OK;
            var contentType = "application/json";
            var bodyType = typeof(object);
            var attribute = new OpenApiResponseWithBodyAttribute(statusCode, contentType, bodyType)
            {
                Summary = summary,
                Description = description,
                Deprecated = deprecated,
                HeaderType = headerType,
            };

            attribute.Summary.Should().Be(summary);
            attribute.Description.Should().Be(description);
            attribute.Deprecated.Should().Be(deprecated);
            attribute.HeaderType.Should().Be(headerType);
        }
    }
}
