using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.OpenApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests
{
    [TestClass]
    public class OpenApiHttpTriggerContextTests
    {
        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Return_ApplicationAssembly()
        {
            var context = new OpenApiHttpTriggerContext();

            var assembly = context.ApplicationAssembly;

            assembly.DefinedTypes.Should().Contain(typeof(IOpenApiHttpTriggerContext).GetTypeInfo());
            assembly.DefinedTypes.Should().Contain(typeof(OpenApiHttpTriggerContext).GetTypeInfo());
            assembly.DefinedTypes.Should().Contain(typeof(OpenApiHttpTrigger).GetTypeInfo());
            assembly.DefinedTypes.Should().NotContain(typeof(ISwaggerUI).GetTypeInfo());
        }

        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Return_PackageAssembly()
        {
            var context = new OpenApiHttpTriggerContext();

            var assembly = context.PackageAssembly;

            assembly.DefinedTypes.Should().Contain(typeof(ISwaggerUI).GetTypeInfo());
            assembly.DefinedTypes.Should().NotContain(typeof(IOpenApiHttpTriggerContext).GetTypeInfo());
        }

        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Return_OpenApiConfigurationOptions()
        {
            var context = new OpenApiHttpTriggerContext();

            var options = context.OpenApiConfigurationOptions;

            options.Info.Version.Should().Be("1.0.0");
            options.Servers.Count.Should().Be(0);
        }

        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Return_OpenApiCustomUIOptions()
        {
            var context = new OpenApiHttpTriggerContext();

            var options = context.OpenApiCustomUIOptions;

            options.CustomStylesheetPath.Should().Be("dist.custom.css");
            options.CustomJavaScriptPath.Should().Be("dist.custom.js");
        }

        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Return_HttpSettings()
        {
            var context = new OpenApiHttpTriggerContext();

            var settings = context.HttpSettings;

            settings.RoutePrefix.Should().Be("api");

        }

        [DataTestMethod]
        [DataRow("v2", OpenApiSpecVersion.OpenApi2_0)]
        [DataRow("v3", OpenApiSpecVersion.OpenApi3_0)]
        public void Given_Type_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(string version, OpenApiSpecVersion expected)
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetOpenApiSpecVersion(version);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("yaml", OpenApiFormat.Yaml)]
        [DataRow("json", OpenApiFormat.Json)]
        public void Given_Type_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(string format, OpenApiFormat expected)
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetOpenApiFormat(format);

            result.Should().Be(expected);
        }
    }
}
