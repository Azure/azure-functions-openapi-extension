using System;

using FluentAssertions;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.FunctionProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests.FunctionProviders
{
    [TestClass]
    public class OpenApiTriggerRenderSwaggerUIFunctionProviderTests
    {
        [TestMethod]
        public void Given_Null_When_Anonymous_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiTriggerRenderSwaggerUIAnonymousFunctionProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_When_User_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiTriggerRenderSwaggerUIUserFunctionProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_When_Function_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiTriggerRenderSwaggerUIFunctionFunctionProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_When_System_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiTriggerRenderSwaggerUISystemFunctionProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_When_Admin_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiTriggerRenderSwaggerUIAdminFunctionProvider(null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
