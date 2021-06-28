using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionProviders.System.Tests
{
    [TestClass]
    public class OpenApiTriggerRenderFunctionProviderTests
    {
        [TestMethod]
        public void Given_Null_When_OAuth2Redirect_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiTriggerRenderOAuth2RedirectFunctionProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_When_OpenApiDocument_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiTriggerRenderOpenApiDocumentFunctionProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_When_SwaggerDocument_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiTriggerRenderSwaggerDocumentFunctionProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_When_SwaggerUI_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiTriggerRenderSwaggerUIFunctionProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
