using System;
using System.IO;
using System.Linq;
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
        [DataTestMethod]
        [DataRow(typeof(OpenApiHttpTriggerContextTests))]
        public void Given_Type_When_Initiated_Then_It_Should_Return_ApplicationAssemblyWithGivenType(Type type)
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var assembly = context.SetApplicationAssembly(location, false)
                                  .ApplicationAssembly;

            var ti = type.GetTypeInfo();

            assembly.DefinedTypes.Select(p => p.FullName).Should().Contain(ti.FullName);
        }

        [DataTestMethod]
        [DataRow(typeof(IOpenApiHttpTriggerContext))]
        [DataRow(typeof(OpenApiHttpTriggerContext))]
        [DataRow(typeof(OpenApiTriggerFunctionProvider))]
        [DataRow(typeof(ISwaggerUI))]
        public void Given_Type_When_Initiated_Then_It_Should_NotReturn_ApplicationAssemblyWithGivenType(Type type)
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var assembly = context.SetApplicationAssembly(location, false)
                                  .ApplicationAssembly;

            var ti = type.GetTypeInfo();

            assembly.DefinedTypes.Select(p => p.FullName).Should().NotContain(ti.FullName);
        }

        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Return_PackageAssembly()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var assembly = context.SetApplicationAssembly(location, false)
                                  .PackageAssembly;

            assembly.DefinedTypes.Should().Contain(typeof(ISwaggerUI).GetTypeInfo());
            assembly.DefinedTypes.Should().NotContain(typeof(IOpenApiHttpTriggerContext).GetTypeInfo());
        }

        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Return_OpenApiConfigurationOptions()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var options = context.SetApplicationAssembly(location, false)
                                 .OpenApiConfigurationOptions;

            options.Info.Version.Should().Be("1.0.0");
            options.Servers.Count.Should().Be(0);
        }

        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Return_OpenApiCustomUIOptions()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var options = context.SetApplicationAssembly(location, false)
                                 .OpenApiCustomUIOptions;

            options.CustomStylesheetPath.Should().Be("dist.custom.css");
            options.CustomJavaScriptPath.Should().Be("dist.custom.js");
        }

        [TestMethod]
        public void Given_Type_When_Initiated_Then_It_Should_Return_HttpSettings()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var settings = context.SetApplicationAssembly(location, false)
                                  .HttpSettings;

            settings.RoutePrefix.Should().Be("api");

        }

        [DataTestMethod]
        [DataRow("v2", OpenApiSpecVersion.OpenApi2_0)]
        [DataRow("v3", OpenApiSpecVersion.OpenApi3_0)]
        public void Given_Type_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(string version, OpenApiSpecVersion expected)
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var result = context.SetApplicationAssembly(location, false)
                                .GetOpenApiSpecVersion(version);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("yaml", OpenApiFormat.Yaml)]
        [DataRow("json", OpenApiFormat.Json)]
        public void Given_Type_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(string format, OpenApiFormat expected)
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var result = context.SetApplicationAssembly(location, false)
                                .GetOpenApiFormat(format);

            result.Should().Be(expected);
        }
    }
}
