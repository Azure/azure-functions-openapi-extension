using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests.Fakes;
using Microsoft.OpenApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests
{
    [TestClass]
    public class OpenApiHttpTriggerContextTests
    {
        [DataTestMethod]
        [DataRow(typeof(OpenApiHttpTriggerContextTests))]
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_ApplicationAssemblyWithGivenType(Type type)
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var assembly = (await context.SetApplicationAssemblyAsync(location, false))
                                         .ApplicationAssembly;

            var ti = type.GetTypeInfo();

            assembly.DefinedTypes.Select(p => p.FullName).Should().Contain(ti.FullName);
        }

        [TestMethod]
        public async Task Given_Type_With_Referenced_Project_When_Initiated_Then_It_Should_Return_ApplicationAssemblyOfRootAssembly()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var assembly = (await context.SetApplicationAssemblyAsync(location, false))
                                         .ApplicationAssembly;

            assembly.FullName.Should().Be(typeof(OpenApiHttpTriggerContextTests).Assembly.FullName);
        }

        [DataTestMethod]
        [DataRow(typeof(IOpenApiHttpTriggerContext))]
        [DataRow(typeof(OpenApiHttpTriggerContext))]
        [DataRow(typeof(OpenApiTriggerFunctionProvider))]
        [DataRow(typeof(ISwaggerUI))]
        public async Task Given_Type_When_Initiated_Then_It_Should_NotReturn_ApplicationAssemblyWithGivenType(Type type)
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var assembly = (await context.SetApplicationAssemblyAsync(location, false))
                                         .ApplicationAssembly;

            var ti = type.GetTypeInfo();

            assembly.DefinedTypes.Select(p => p.FullName).Should().NotContain(ti.FullName);
        }

        [TestMethod]
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_PackageAssembly()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var assembly = (await context.SetApplicationAssemblyAsync(location, false))
                                         .PackageAssembly;

            assembly.DefinedTypes.Should().Contain(typeof(ISwaggerUI).GetTypeInfo());
        }

        [TestMethod]
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_OpenApiConfigurationOptions()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var options = (await context.SetApplicationAssemblyAsync(location, false))
                                        .OpenApiConfigurationOptions;

            options.Info.Version.Should().Be("1.0.0");
            options.Servers.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_OpenApiCustomUIOptions()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var options = (await context.SetApplicationAssemblyAsync(location, false))
                                        .OpenApiCustomUIOptions;

            options.CustomStylesheetPath.Should().Be("dist.custom.css");
            options.CustomJavaScriptPath.Should().Be("dist.custom.js");
            options.CustomFaviconMetaTags.SingleOrDefault(p => p.Contains("dist.favicon-16x16.png")).Should().NotBeNull();
            options.CustomFaviconMetaTags.SingleOrDefault(p => p.Contains("dist.favicon-32x32.png")).Should().NotBeNull();
        }

        [TestMethod]
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_HttpSettings()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var settings = (await context.SetApplicationAssemblyAsync(location, false))
                                         .HttpSettings;

            settings.RoutePrefix.Should().Be("api");
        }

        [TestMethod]
        public async Task Given_Authorization_When_AuthorizeAsync_Invoked_Then_It_Should_Return_Result()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var req = new Mock<IHttpRequestDataObject>();
            var context = new OpenApiHttpTriggerContext();

            var result = await context.SetApplicationAssemblyAsync(location, false)
                                      .AuthorizeAsync(req.Object);

            result.StatusCode.Should().Be(FakeOpenApiHttpTriggerAuthorization.StatusCode);
            result.ContentType.Should().Be(FakeOpenApiHttpTriggerAuthorization.ContentType);
            result.Payload.Should().Be(FakeOpenApiHttpTriggerAuthorization.Payload);
        }

        [DataTestMethod]
        [DataRow("v2", OpenApiSpecVersion.OpenApi2_0)]
        [DataRow("v3", OpenApiSpecVersion.OpenApi3_0)]
        public async Task Given_Type_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(string version, OpenApiSpecVersion expected)
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var result = (await context.SetApplicationAssemblyAsync(location, false))
                                       .GetOpenApiSpecVersion(version);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("yaml", OpenApiFormat.Yaml)]
        [DataRow("json", OpenApiFormat.Json)]
        public async Task Given_Type_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(string format, OpenApiFormat expected)
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var result = (await context.SetApplicationAssemblyAsync(location, false))
                                       .GetOpenApiFormat(format);

            result.Should().Be(expected);
        }
    }
}
