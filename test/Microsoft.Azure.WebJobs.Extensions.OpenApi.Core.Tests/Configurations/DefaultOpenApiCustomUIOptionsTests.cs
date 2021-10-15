using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
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
            options.CustomFaviconMetaTags.Should().BeEquivalentTo(new List<string>()
            {
                "<link rel=\"icon\" type=\"image/png\" href=\"dist.favicon-32x32.png\" sizes=\"32x32\" />",
                "<link rel=\"icon\" type=\"image/png\" href=\"dist.favicon-16x16.png\" sizes=\"16x16\" />"
            });
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

        [DataTestMethod]
        [DataRow("*data:image/png;base64*", "dist.favicon-32x32.png", "dist.favicon-16x16.png")]
        public async Task Given_Type_When_GetFaviconAsync_Invoked_Then_It_Should_Return_ResultAsync(string expected, string defaultFavicon_32, string defaultFavicon_16)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var options = new DefaultOpenApiCustomUIOptions(assembly);

            var result = await options.GetFaviconMetaTagsAsync().ConfigureAwait(false);

            (result as List<string>)[0].Should().Match(expected);
            (result as List<string>)[0].Should().NotMatch(defaultFavicon_32);
            (result as List<string>)[1].Should().Match(expected);
            (result as List<string>)[1].Should().NotMatch(defaultFavicon_16);
        }

        [TestMethod]
        public async Task Given_File_When_GetStylesheetAsync_Invoked_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var options = new FakeFileCustomUIOptions(assembly);

            var result = await options.GetStylesheetAsync().ConfigureAwait(false);

            result.Should().NotBeEmpty();
        }

        [TestMethod]
        public async Task Given_File_When_GetJavaScriptAsync_Invoked_Then_It_Should_Return_Result()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var options = new FakeFileCustomUIOptions(assembly);

            var result = await options.GetJavaScriptAsync().ConfigureAwait(false);

            result.Should().NotBeEmpty();
        }

        [TestMethod]
        public async Task Given_Url_When_GetStylesheetAsync_Invoked_Then_It_Should_Return_Result()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var options = new FakeUriCustomUIOptions(assembly);

            var result = await options.GetStylesheetAsync().ConfigureAwait(false);

            result.Should().NotBeEmpty();
        }

        [TestMethod]
        public async Task Given_Url_When_GetJavaScriptAsync_Invoked_Then_It_Should_Return_Result()
        {

            var assembly = Assembly.GetExecutingAssembly();
            var options = new FakeUriCustomUIOptions(assembly);

            var result = await options.GetJavaScriptAsync().ConfigureAwait(false);

            result.Should().NotBeEmpty();
        }

        [DataTestMethod]
        [DataRow("<link rel=\"icon\" type=\"image/png\" href=\"https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/src/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core/dist/favicon-16x16.png\" sizes=\"16x16\" />"
               , "<link rel=\"icon\" type=\"image/png\" href=\"https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/src/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core/dist/favicon-32x32.png\" sizes=\"32x32\" />")]
        public async Task Given_Url_When_GetFaviconAsync_Invoked_Then_It_Should_Return_Result(string expectedFavicon_16, string expectedFavicon_32)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var options = new FakeUriCustomUIOptions(assembly);

            var result = await options.GetFaviconMetaTagsAsync().ConfigureAwait(false);

            (result as List<string>)[0].Should().Be(expectedFavicon_16);
            (result as List<string>)[1].Should().Be(expectedFavicon_32);
        }
    }
}
