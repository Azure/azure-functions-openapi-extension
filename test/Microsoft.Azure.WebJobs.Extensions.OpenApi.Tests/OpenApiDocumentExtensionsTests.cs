using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Newtonsoft.Json.Linq;
using OpenApiSettings = Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.OpenApiSettings;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests
{
    [TestClass]
    public class OpenApiDocumentExtensionsTests
    {
        [TestMethod]
        public async Task ExtensionTest()
        {
            var settings = new Mock<OpenApiSettings>();
            var ctx = new Mock<OpenApiHttpTriggerContext> { CallBase = true };
            ctx.Setup(p => p.SetApplicationAssemblyAsync(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(ctx.Object);
            ctx.Setup(p => p.AuthorizeAsync(It.IsAny<IHttpRequestDataObject>())).ReturnsAsync((OpenApiAuthorizationResult)null);
            ctx.SetupGet(p => p.OpenApiConfigurationOptions).Returns(new DefaultOpenApiConfigurationOptions());
            ctx.SetupGet(p => p.ApplicationAssembly).Returns(Assembly.GetExecutingAssembly);

            var provider = new OpenApiTriggerFunctionProvider(settings.Object, ctx.Object, new[] { new FakeDocumentExtension() });
            var httpContext = new DefaultHttpContext();
            var executionContext = new ExecutionContext();

            var response = await OpenApiTriggerFunctionProvider.RenderSwaggerDocument(httpContext.Request, "json", executionContext, NullLogger.Instance) as ContentResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.ContentType.Should().BeEquivalentTo("application/json");

            // Deserialize response
            var obj = JObject.Parse(response.Content);
            var mockTrigger = obj.SelectToken("paths./MockExtension");
            mockTrigger.Should().NotBeNull();
            mockTrigger.SelectToken("description")?.Value<string>().Should().BeEquivalentTo("Mock Path");
        }
    }
}
