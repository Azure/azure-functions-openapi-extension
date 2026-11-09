using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
{
    [TestClass]
    public class SwaggerUIExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_RenderAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            Func<Task> func = async () => await SwaggerUIExtensions.RenderAsync(null, null).ConfigureAwait(false);
            func.Should().ThrowAsync<ArgumentNullException>();

            var ui = Substitute.For<ISwaggerUI>();
            var task = Task.FromResult(ui);

            func = async () => await SwaggerUIExtensions.RenderAsync(task, null).ConfigureAwait(false);
            func.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task Given_Value_When_RenderAsync_Invoked_Then_It_Should_Return_Value()
        {
            var endpoint = "swagger/ui";
            var rendered = "hello world";

            var ui = Substitute.For<ISwaggerUI>();
            ui.RenderAsync(Arg.Any<string>(), Arg.Any<OpenApiAuthLevelType>(), Arg.Any<string>()).Returns(Task.FromResult(rendered));

            var task = Task.FromResult(ui);

            var result = await SwaggerUIExtensions.RenderAsync(task, endpoint).ConfigureAwait(false);

            result.Should().BeEquivalentTo(rendered);
        }

        [TestMethod]
        public void Given_Null_When_RenderOAuth2RedirectAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            Func<Task> func = async () => await SwaggerUIExtensions.RenderOAuth2RedirectAsync(null, null).ConfigureAwait(false);
            func.Should().ThrowAsync<ArgumentNullException>();

            var ui = Substitute.For<ISwaggerUI>();
            var task = Task.FromResult(ui);

            func = async () => await SwaggerUIExtensions.RenderOAuth2RedirectAsync(task, null).ConfigureAwait(false);
            func.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task Given_Value_When_RenderOAuth2RedirectAsync_Invoked_Then_It_Should_Return_Value()
        {
            var endpoint = "oauth2-redirect.html";
            var rendered = "hello world";

            var ui = Substitute.For<ISwaggerUI>();
            ui.RenderOAuth2RedirectAsync(Arg.Any<string>(), Arg.Any<OpenApiAuthLevelType>(), Arg.Any<string>()).Returns(Task.FromResult(rendered));

            var task = Task.FromResult(ui);

            var result = await SwaggerUIExtensions.RenderOAuth2RedirectAsync(task, endpoint).ConfigureAwait(false);

            result.Should().BeEquivalentTo(rendered);
        }
    }
}
