using System;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests
{
    [TestClass]
    public class OpenApiTriggerFunctionProviderTests
    {
        [TestMethod]
        public void Given_Null_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiTriggerFunctionProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow(true, 2)]
        [DataRow(false, 4)]
        public async Task Given_HideSwaggerUI_When_GetFunctionMetadataAsync_Invoked_Then_It_Should_Return_Result(bool hideSwaggerUI, int expected)
        {
            var settings = Substitute.For<OpenApiSettings>();
            settings.HideSwaggerUI.Returns(hideSwaggerUI);

            var provider = new OpenApiTriggerFunctionProvider(settings);

            var result = await provider.GetFunctionMetadataAsync().ConfigureAwait(false);

            result.Should().HaveCount(expected);
        }

        [DataTestMethod]
        [DataRow(true, 0)]
        [DataRow(false, 4)]
        public async Task Given_HideDocument_When_GetFunctionMetadataAsync_Invoked_Then_It_Should_Return_Result(bool hideDocument, int expected)
        {
            var settings = Substitute.For<OpenApiSettings>();
            settings.HideDocument.Returns(hideDocument);

            var provider = new OpenApiTriggerFunctionProvider(settings);

            var result = await provider.GetFunctionMetadataAsync().ConfigureAwait(false);

            result.Should().HaveCount(expected);
        }

        [DataTestMethod]
        [DataRow(AuthorizationLevel.Anonymous, AuthorizationLevel.Anonymous)]
        [DataRow(AuthorizationLevel.Anonymous, AuthorizationLevel.Function)]
        [DataRow(AuthorizationLevel.Function, AuthorizationLevel.Anonymous)]
        [DataRow(AuthorizationLevel.Function, AuthorizationLevel.Function)]
        public async Task Given_AuthLevel_When_GetFunctionMetadataAsync_Invoked_Then_It_Should_Return_Result(AuthorizationLevel authLevelDoc, AuthorizationLevel authLevelUI)
        {
            var authLevelSettings = Substitute.For<OpenApiAuthLevelSettings>();
            authLevelSettings.Document.Returns(authLevelDoc);
            authLevelSettings.UI.Returns(authLevelUI);

            var settings = Substitute.For<OpenApiSettings>();
            settings.HideSwaggerUI.Returns(false);
            settings.HideDocument.Returns(false);
            settings.AuthLevel.Returns(authLevelSettings);

            var provider = new OpenApiTriggerFunctionProvider(settings);

            var result = await provider.GetFunctionMetadataAsync().ConfigureAwait(false);

            result.Single(p => p.Name == "RenderSwaggerDocument").Bindings.First().Raw.Value<int>("authLevel").Should().Be((int)authLevelDoc);
            result.Single(p => p.Name == "RenderOpenApiDocument").Bindings.First().Raw.Value<int>("authLevel").Should().Be((int)authLevelDoc);
            result.Single(p => p.Name == "RenderSwaggerUI").Bindings.First().Raw.Value<int>("authLevel").Should().Be((int)authLevelUI);
            result.Single(p => p.Name == "RenderOAuth2Redirect").Bindings.First().Raw.Value<int>("authLevel").Should().Be((int)authLevelUI);
        }
    }
}
