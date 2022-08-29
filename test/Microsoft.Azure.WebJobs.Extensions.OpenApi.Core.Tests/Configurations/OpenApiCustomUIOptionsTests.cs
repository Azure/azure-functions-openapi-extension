using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class OpenApiCustomUIOptionsTests
    {
        [TestMethod]
        public async Task Given_Type_When_Instantiated_Then_Properties_Should_Return_Result()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var options = new OpenApiCustomUIOptions(assembly);

            options.Assembly.Should().NotBeNull();
            options.GetStylesheet.Should().BeNull();
            options.GetJavaScript.Should().BeNull();
            options.CustomStylesheetPath.Should().BeNull();
            options.CustomJavaScriptPath.Should().BeNull();
            (await options.GetStylesheetAsync().ConfigureAwait(false)).Should().BeEmpty();
            (await options.GetJavaScriptAsync().ConfigureAwait(false)).Should().BeEmpty();
        }

        [DataTestMethod]
        [DataRow("helloworld.css")]
        public async Task Given_Deligate_Through_Constructor_When_GetStylesheetAsync_Invoked_Then_It_Should_Return_Result(string css)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var options = new OpenApiCustomUIOptions(assembly, funcCSS: () =>
            {
                return Task.FromResult(css);
            });

            var result = await options.GetStylesheetAsync().ConfigureAwait(false);

            result.Should().Be(css);
        }

        [DataTestMethod]
        [DataRow("helloworld.js")]
        public async Task Given_Deligate_Through_Constructor_When_GetJavaScriptAsync_Invoked_Then_It_Should_Return_Result(string js)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var options = new OpenApiCustomUIOptions(assembly, funcJS: () =>
            {
                return Task.FromResult(js);
            });

            var result = await options.GetJavaScriptAsync().ConfigureAwait(false);

            result.Should().Be(js);
        }

        [DataTestMethod]
        [DataRow("helloworld.css", "helloworld.css")]
        public async Task Given_Deligate_Through_Property_When_GetStylesheetAsync_Invoked_Then_It_Should_Return_Result(string css, string expected)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var options = new OpenApiCustomUIOptions(assembly)
            {
                GetStylesheet = () =>
                {
                    return Task.FromResult(css);
                }
            };

            var result = await options.GetStylesheetAsync().ConfigureAwait(false);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("helloworld.js", "helloworld.js")]
        public async Task Given_Deligate_Through_Property_When_GetJavaScriptAsync_Invoked_Then_It_Should_Return_Result(string js, string expected)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var options = new OpenApiCustomUIOptions(assembly)
            {
                GetJavaScript = () =>
                {
                    return Task.FromResult(js);
                }
            };

            var result = await options.GetJavaScriptAsync().ConfigureAwait(false);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("helloworld.css", 0)]
        public async Task Given_Invalid_CSSPath_When_GetStylesheetAsync_Invoked_Then_It_Should_Return_Result(string css, int expected)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var options = new OpenApiCustomUIOptions(assembly)
            {
                CustomStylesheetPath = css
            };

            var result = await options.GetStylesheetAsync().ConfigureAwait(false);

            result.Should().BeEmpty();
            result.Length.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("dist.fake.css", 0)]
        public async Task Given_Valid_CSSPath_When_GetStylesheetAsync_Invoked_Then_It_Should_Return_Result(string css, int expected)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var options = new OpenApiCustomUIOptions(assembly)
            {
                CustomStylesheetPath = css
            };

            var result = await options.GetStylesheetAsync().ConfigureAwait(false);

            result.Should().NotBeEmpty();
            result.Length.Should().BeGreaterThan(expected);
        }

        [DataTestMethod]
        [DataRow("helloworld.js", 0)]
        public async Task Given_Invalid_JSPath_When_GetJavaScriptAsync_Invoked_Then_It_Should_Return_Result(string js, int expected)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var options = new OpenApiCustomUIOptions(assembly)
            {
                CustomJavaScriptPath = js
            };

            var result = await options.GetJavaScriptAsync().ConfigureAwait(false);

            result.Should().BeEmpty();
            result.Length.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("dist.fake.js", 0)]
        public async Task Given_Valid_JSPath_When_GetJavaScriptAsync_Invoked_Then_It_Should_Return_Result(string js, int expected)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var options = new OpenApiCustomUIOptions(assembly)
            {
                CustomJavaScriptPath = js
            };

            var result = await options.GetJavaScriptAsync().ConfigureAwait(false);

            result.Should().NotBeEmpty();
            result.Length.Should().BeGreaterThan(expected);
        }
    }
}
