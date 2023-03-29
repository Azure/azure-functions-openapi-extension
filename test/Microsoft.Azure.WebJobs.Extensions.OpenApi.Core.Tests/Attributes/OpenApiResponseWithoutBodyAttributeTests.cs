using System;
using System.Net;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiResponseWithoutBodyAttributeTests
    {
        [TestMethod]
        public void Given_Parameters_When_Instantiated_Then_It_Should_Return_Value()
        {
            var statusCode = HttpStatusCode.OK;
            var attribute = new OpenApiResponseWithoutBodyAttribute(statusCode, "GET");

            attribute.StatusCode.Should().Be(statusCode);
            attribute.Summary.Should().BeNullOrWhiteSpace();
            attribute.Description.Should().BeNullOrWhiteSpace();
        }

        [DataTestMethod]
        [DataRow("Hello World", "Lorem Ipsum", typeof(FakeResponseHeader))]
        public void Given_Properties_When_Instantiated_Then_It_Should_Return_Value(string summary, string description, Type headerType)
        {
            var statusCode = HttpStatusCode.OK;
            var attribute = new OpenApiResponseWithoutBodyAttribute(statusCode, "GET")
            {
                Summary = summary,
                Description = description,
                CustomHeaderType = headerType,
            };

            attribute.Summary.Should().Be(summary);
            attribute.Description.Should().Be(description);
            attribute.CustomHeaderType.Should().Be(headerType);
        }
    }
}
