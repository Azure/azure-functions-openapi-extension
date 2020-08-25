using System.Net;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiResponseWithoutBodyAttributeTests
    {
        [TestMethod]
        public void Given_Value_Property_Should_Return_Value()
        {
            var statusCode = HttpStatusCode.OK;
            var attribute = new OpenApiResponseWithoutBodyAttribute(statusCode);

            attribute.StatusCode.Should().Be(statusCode);
            attribute.Summary.Should().BeNullOrWhiteSpace();
            attribute.Description.Should().BeNullOrWhiteSpace();
        }
    }
}
