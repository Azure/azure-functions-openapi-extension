using System;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class SwaggerUIExtensionsTests
    {
        [TestMethod]
        public void Given_Null_Method_Should_Throw_Exception()
        {
            Func<Task> func = async () => await SwaggerUIExtensions.RenderAsync(null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            var ui = new Mock<ISwaggerUI>();
            var task = Task.FromResult(ui.Object);

            func = async () => await SwaggerUIExtensions.RenderAsync(task, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task Given_Value_Method_Should_Return_Value()
        {
            var endpoint = "swagger/ui";
            var rendered = "hello world";

            var ui = new Mock<ISwaggerUI>();
            ui.Setup(p => p.RenderAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(rendered);

            var task = Task.FromResult(ui.Object);

            var result = await SwaggerUIExtensions.RenderAsync(task, endpoint).ConfigureAwait(false);

            result.Should().BeEquivalentTo(rendered);
        }
    }
}
