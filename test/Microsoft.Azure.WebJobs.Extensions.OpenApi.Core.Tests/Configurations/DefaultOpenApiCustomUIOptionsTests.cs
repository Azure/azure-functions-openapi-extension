using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class DefaultOpenApiCustomUIOptionsTests
    {
        [TestMethod]
        public void Given_Type_When_Instantiated_Then_Properties_Should_Return_Result()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var options = new DefaultOpenApiCustomUIOptions(assembly);

            options.CustomStylesheetPath.Should().Be("dist.custom.css");
            options.CustomJavaScriptPath.Should().Be("dist.custom.js");
        }

        [TestMethod]
        public async Task Given_Type_When_GetStylesheetAsync_Invoked_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var options = new DefaultOpenApiCustomUIOptions(assembly);

            var result = await options.GetStylesheetAsync().ConfigureAwait(false);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public async Task Given_Type_When_GetJavaScriptAsync_Invoked_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var options = new DefaultOpenApiCustomUIOptions(assembly);

            var result = await options.GetJavaScriptAsync().ConfigureAwait(false);

            result.Should().BeEmpty();
        }
    }
}
