using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        [DataRow("dist.favicon-32x32.png")]
        [DataRow("dist.favicon-16x16.png")]
        public async Task Given_Type_When_GetFaviconAsync_Invoked_Then_It_Should_Return_ResultAsync(string defaultFavicon)
        {
            var assembly = Assembly.GetAssembly(typeof(DefaultOpenApiCustomUIOptions));
            var options = new DefaultOpenApiCustomUIOptions(assembly);

            var result = await options.GetFaviconMetaTagsAsync().ConfigureAwait(false);
            var expected = default(string);

            using (var stream = assembly.GetManifestResourceStream($"{typeof(SwaggerUI).Namespace}.{defaultFavicon}"))
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
                expected = Convert.ToBase64String(memoryStream.ToArray());
            }

            result.SingleOrDefault(p => p.Contains(expected)).Should().NotBeNull();
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
        [DataRow("<link rel=\"icon\" type=\"image/png\" href=\"https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/src/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core/dist/favicon-16x16.png\" sizes=\"16x16\" />")]
        [DataRow("<link rel=\"icon\" type=\"image/png\" href=\"https://raw.githubusercontent.com/Azure/azure-functions-openapi-extension/main/src/Microsoft.Azure.WebJobs.Extensions.OpenApi.Core/dist/favicon-32x32.png\" sizes=\"32x32\" />")]
        public async Task Given_Url_When_GetFaviconAsync_Invoked_Then_It_Should_Return_Result(string expectedFavicon)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var options = new FakeUriCustomUIOptions(assembly);

            var result = await options.GetFaviconMetaTagsAsync().ConfigureAwait(false);

            result.SingleOrDefault(p => p.Contains(expectedFavicon)).Should().NotBeNull();
        }
    }
}
