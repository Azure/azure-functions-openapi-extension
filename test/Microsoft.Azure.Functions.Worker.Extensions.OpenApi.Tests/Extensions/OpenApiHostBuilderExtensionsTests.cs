using System;

using FluentAssertions;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Extensions
{
    [TestClass]
    public class OpenApiHostBuilderExtensionsTests
    {
        [DataTestMethod]
        [DataRow("Anonymous", "Anonymous")]
        [DataRow("Anonymous", "User")]
        [DataRow("Anonymous", "Function")]
        [DataRow("Anonymous", "System")]
        [DataRow("Anonymous", "Admin")]
        [DataRow("User", "Anonymous")]
        [DataRow("User", "User")]
        [DataRow("User", "Function")]
        [DataRow("User", "System")]
        [DataRow("User", "Admin")]
        [DataRow("Function", "Anonymous")]
        [DataRow("Function", "User")]
        [DataRow("Function", "Function")]
        [DataRow("Function", "System")]
        [DataRow("Function", "Admin")]
        [DataRow("System", "Anonymous")]
        [DataRow("System", "User")]
        [DataRow("System", "Function")]
        [DataRow("System", "System")]
        [DataRow("System", "Admin")]
        [DataRow("Admin", "Anonymous")]
        [DataRow("Admin", "User")]
        [DataRow("Admin", "Function")]
        [DataRow("Admin", "System")]
        [DataRow("Admin", "Admin")]
        public void Given_HideSwaggerUI_Of_False_When_ConfigureOpenApi_Invoked_Then_It_Should_Result_Result(string authLevelDoc, string authLevelUI)
        {
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__Document", authLevelDoc);
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__UI", authLevelUI);
            Environment.SetEnvironmentVariable("OpenApi__HideSwaggerUI", "false");

            var host = new HostBuilder()
                           .ConfigureOpenApi()
                           .Build();

            var swaggerDoc = host.Services.GetService<IOpenApiTriggerRenderSwaggerDocumentFunctionProvider>();

            swaggerDoc.Should().NotBeNull();
            swaggerDoc.GetType().Name.Should().Be($"OpenApiTriggerRenderSwaggerDocument{authLevelDoc}FunctionProvider");

            var openApiDoc = host.Services.GetService<IOpenApiTriggerRenderOpenApiDocumentFunctionProvider>();

            openApiDoc.Should().NotBeNull();
            openApiDoc.GetType().Name.Should().Be($"OpenApiTriggerRenderOpenApiDocument{authLevelDoc}FunctionProvider");

            var oauth2Redirect = host.Services.GetService<IOpenApiTriggerRenderOAuth2RedirectFunctionProvider>();

            oauth2Redirect.Should().NotBeNull();
            oauth2Redirect.GetType().Name.Should().Be($"OpenApiTriggerRenderOAuth2Redirect{authLevelUI}FunctionProvider");

            var swaggerUI = host.Services.GetService<IOpenApiTriggerRenderSwaggerUIFunctionProvider>();

            swaggerUI.Should().NotBeNull();
            swaggerUI.GetType().Name.Should().Be($"OpenApiTriggerRenderSwaggerUI{authLevelUI}FunctionProvider");
        }

        [DataTestMethod]
        [DataRow("Anonymous", "Anonymous")]
        [DataRow("Anonymous", "User")]
        [DataRow("Anonymous", "Function")]
        [DataRow("Anonymous", "System")]
        [DataRow("Anonymous", "Admin")]
        [DataRow("User", "Anonymous")]
        [DataRow("User", "User")]
        [DataRow("User", "Function")]
        [DataRow("User", "System")]
        [DataRow("User", "Admin")]
        [DataRow("Function", "Anonymous")]
        [DataRow("Function", "User")]
        [DataRow("Function", "Function")]
        [DataRow("Function", "System")]
        [DataRow("Function", "Admin")]
        [DataRow("System", "Anonymous")]
        [DataRow("System", "User")]
        [DataRow("System", "Function")]
        [DataRow("System", "System")]
        [DataRow("System", "Admin")]
        [DataRow("Admin", "Anonymous")]
        [DataRow("Admin", "User")]
        [DataRow("Admin", "Function")]
        [DataRow("Admin", "System")]
        [DataRow("Admin", "Admin")]
        public void Given_HideSwaggerUI_Of_True_When_ConfigureOpenApi_Invoked_Then_It_Should_Result_Result(string authLevelDoc, string authLevelUI)
        {
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__Document", authLevelDoc);
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__UI", authLevelUI);
            Environment.SetEnvironmentVariable("OpenApi__HideSwaggerUI", "true");

            var host = new HostBuilder()
                           .ConfigureOpenApi()
                           .Build();

            var swaggerDoc = host.Services.GetService<IOpenApiTriggerRenderSwaggerDocumentFunctionProvider>();

            swaggerDoc.Should().NotBeNull();
            swaggerDoc.GetType().Name.Should().Be($"OpenApiTriggerRenderSwaggerDocument{authLevelDoc}FunctionProvider");

            var openApiDoc = host.Services.GetService<IOpenApiTriggerRenderOpenApiDocumentFunctionProvider>();

            openApiDoc.Should().NotBeNull();
            openApiDoc.GetType().Name.Should().Be($"OpenApiTriggerRenderOpenApiDocument{authLevelDoc}FunctionProvider");

            var oauth2Redirect = host.Services.GetService<IOpenApiTriggerRenderOAuth2RedirectFunctionProvider>();

            oauth2Redirect.Should().NotBeNull();
            oauth2Redirect.GetType().Name.Should().Be($"OpenApiTriggerRenderOAuth2Redirect{authLevelUI}FunctionProvider");

            var swaggerUI = host.Services.GetService<IOpenApiTriggerRenderSwaggerUIFunctionProvider>();

            swaggerUI.Should().BeNull();
        }
    }
}
