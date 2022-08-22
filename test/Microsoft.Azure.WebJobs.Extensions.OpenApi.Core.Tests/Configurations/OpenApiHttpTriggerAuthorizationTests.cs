using System.Net;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class OpenApiHttpTriggerAuthorizationTests
    {
        [TestMethod]
        public async Task Given_NoDeligate_When_AuthorizeAsync_Invoked_Then_It_Should_Return_Null()
        {
            var req = new Mock<IHttpRequestDataObject>();
            var auth = new OpenApiHttpTriggerAuthorization();

            var result = await auth.AuthorizeAsync(req.Object).ConfigureAwait(false);

            result.Should().BeNull();
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.Unauthorized, "text/plain", "Unauthorized")]
        public async Task Given_Deligate_Through_Constructor_When_AuthorizeAsync_Invoked_Then_It_Should_Return_Result(HttpStatusCode statusCode, string contentType, string payload)
        {
            var req = new Mock<IHttpRequestDataObject>();
            var auth = new OpenApiHttpTriggerAuthorization(req =>
            {
                var result = new OpenApiAuthorizationResult()
                {
                    StatusCode = statusCode,
                    ContentType = contentType,
                    Payload = payload,
                };

                return Task.FromResult(result);
            });

            var result = await auth.AuthorizeAsync(req.Object).ConfigureAwait(false);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(statusCode);
            result.ContentType.Should().Be(contentType);
            result.Payload.Should().Be(payload);
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.Unauthorized, "text/plain", "Unauthorized")]
        public async Task Given_Deligate_Through_Property_When_AuthorizeAsync_Invoked_Then_It_Should_Return_Result(HttpStatusCode statusCode, string contentType, string payload)
        {
            var req = new Mock<IHttpRequestDataObject>();
            var auth = new OpenApiHttpTriggerAuthorization
            {
                Authorization = req =>
                {
                    var result = new OpenApiAuthorizationResult()
                    {
                        StatusCode = statusCode,
                        ContentType = contentType,
                        Payload = payload,
                    };

                    return Task.FromResult(result);
                }
            };

            var result = await auth.AuthorizeAsync(req.Object).ConfigureAwait(false);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(statusCode);
            result.ContentType.Should().Be(contentType);
            result.Payload.Should().Be(payload);
        }
    }
}
