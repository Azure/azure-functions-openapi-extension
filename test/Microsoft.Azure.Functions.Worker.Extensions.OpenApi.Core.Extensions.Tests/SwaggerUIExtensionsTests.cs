using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Core.Extensions.Tests
{
    [TestClass]
    public class SwaggerUIExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_RenderAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            Func<Task> func = async () => await SwaggerUIExtensions.RenderAsync(null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            var ui = new Mock<ISwaggerUI>();
            var task = Task.FromResult(ui.Object);

            func = async () => await SwaggerUIExtensions.RenderAsync(task, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task Given_Value_When_RenderAsync_Invoked_Then_It_Should_Return_Value()
        {
            var endpoint = "swagger/ui";
            var rendered = "hello world";

            var ui = new Mock<ISwaggerUI>();
            ui.Setup(p => p.RenderAsync(It.IsAny<string>(), It.IsAny<OpenApiAuthLevelType>(), It.IsAny<string>())).ReturnsAsync(rendered);

            var task = Task.FromResult(ui.Object);

            var result = await SwaggerUIExtensions.RenderAsync(task, endpoint).ConfigureAwait(false);

            result.Should().BeEquivalentTo(rendered);
        }

        [TestMethod]
        public void Given_Null_When_RenderOAuth2RedirectAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            Func<Task> func = async () => await SwaggerUIExtensions.RenderOAuth2RedirectAsync(null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            var ui = new Mock<ISwaggerUI>();
            var task = Task.FromResult(ui.Object);

            func = async () => await SwaggerUIExtensions.RenderOAuth2RedirectAsync(task, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task Given_Value_When_RenderOAuth2RedirectAsync_Invoked_Then_It_Should_Return_Value()
        {
            var endpoint = "oauth2-redirect.html";
            var rendered = "hello world";

            var ui = new Mock<ISwaggerUI>();
            ui.Setup(p => p.RenderOAuth2RedirectAsync(It.IsAny<string>(), It.IsAny<OpenApiAuthLevelType>(), It.IsAny<string>())).ReturnsAsync(rendered);

            var task = Task.FromResult(ui.Object);

            var result = await SwaggerUIExtensions.RenderOAuth2RedirectAsync(task, endpoint).ConfigureAwait(false);

            result.Should().BeEquivalentTo(rendered);
        }
    }
}
