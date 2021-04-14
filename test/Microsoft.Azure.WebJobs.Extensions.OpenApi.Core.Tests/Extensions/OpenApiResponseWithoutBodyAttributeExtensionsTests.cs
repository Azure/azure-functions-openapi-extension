using System;
using System.Net;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.OpenApi.Any;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class OpenApiResponseWithoutBodyAttributeExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_ToOpenApiResponse_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => OpenApiResponseWithoutBodyAttributeExtensions.ToOpenApiResponse(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("Lorem Ipsum", "Hello World", typeof(FakeResponseHeader))]
        public void Given_Properties_When_ToOpenApiResponse_Invoked_Then_It_Should_Return_Value(string summary, string description, Type headerType)
        {
            var statusCode = HttpStatusCode.OK;
            var attribute = new OpenApiResponseWithoutBodyAttribute(statusCode)
            {
                Summary = summary,
                Description = description,
                CustomHeaderType = headerType,
            };

            var result = OpenApiResponseWithoutBodyAttributeExtensions.ToOpenApiResponse(attribute);

            result.Description.Should().Be(description);
            result.Extensions.Should().ContainKey("x-ms-summary");
            (result.Extensions["x-ms-summary"] as OpenApiString).Value.Should().Be(summary);
            result.Headers.Should().ContainKey("x-fake-header");
        }
    }
}
