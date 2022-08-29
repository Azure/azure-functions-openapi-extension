using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests.Fakes;
using Microsoft.OpenApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests
{
    [TestClass]
    public class OpenApiHttpTriggerContextTests
    {
        [TestCleanup]
        public void CleanUp()
        {
            Environment.SetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT", null);
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__Document", null);
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__UI", null);
            Environment.SetEnvironmentVariable("OpenApi__ApiKey", null);
        }

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
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_OpenApiHttpTriggerAuthorization()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var auth = ((await context.SetApplicationAssemblyAsync(location, false)) as OpenApiHttpTriggerContext)
                                      .OpenApiHttpTriggerAuthorization;

            auth.Should().NotBeNull();
            auth.GetType().Name.Should().Be("FakeOpenApiHttpTriggerAuthorization");
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
        }

        [TestMethod]
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_HttpSettings()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var settings = (await context.SetApplicationAssemblyAsync(location, false))
                                         .HttpSettings;

            settings.Should().NotBeNull();
            settings.RoutePrefix.Should().Be("api");
        }

        [TestMethod]
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_Document()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var document = (await context.SetApplicationAssemblyAsync(location, false))
                                         .Document;

            document.Should().NotBeNull();
        }

        [TestMethod]
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_SwaggerUI()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var swaggerUI = (await context.SetApplicationAssemblyAsync(location, false))
                                         .SwaggerUI;

            swaggerUI.Should().NotBeNull();
        }

        [TestMethod]
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_NamingStrategy()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var namingStrategy = (await context.SetApplicationAssemblyAsync(location, false))
                                         .NamingStrategy;

            namingStrategy.Should().NotBeNull();
            namingStrategy.Should().BeOfType<CamelCaseNamingStrategy>();
        }

        [DataTestMethod]
        [DataRow("Development", true)]
        [DataRow("Production", false)]
        public async Task Given_Type_When_Initiated_Then_It_Should_Return_IsDevelopment(string environment, bool expected)
        {
            Environment.SetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT", environment);
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var isDevelopment = (await context.SetApplicationAssemblyAsync(location, false))
                                         .IsDevelopment;

            isDevelopment.Should().Be(expected);
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

        [TestMethod]
        public void Given_Type_When_GetVisitorCollection_Invoked_Then_It_Should_Return_Result()
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetVisitorCollection();

            result.Should().NotBeNull();
            result.Visitors.Should().NotBeNull();
            result.Visitors.Should().HaveCountGreaterThan(0);
        }

        [TestMethod]
        public void Given_NoVersion_When_GetOpenApiVersionType_Invoked_Then_It_Should_Return_V2()
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetOpenApiVersionType();

            result.Should().Be(OpenApiVersionType.V2);
        }

        [TestMethod]
        public void Given_InvalidVersion_When_GetOpenApiVersionType_Invoked_Then_It_Should_Throw_Exception()
        {
            var context = new OpenApiHttpTriggerContext();

            Action action = () => context.GetOpenApiVersionType("v1");

            action.Should().Throw<InvalidOperationException>();
        }

        [DataTestMethod]
        [DataRow("v2", OpenApiVersionType.V2)]
        [DataRow("V2", OpenApiVersionType.V2)]
        [DataRow("v3", OpenApiVersionType.V3)]
        [DataRow("V3", OpenApiVersionType.V3)]
        public void Given_ValidVersion_When_GetOpenApiVersionType_Invoked_Then_It_Should_Return_V2(string version, OpenApiVersionType expected)
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetOpenApiVersionType(version);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_InvalidVersion_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Throw_Exception()
        {
            var context = new OpenApiHttpTriggerContext();

            Action action = () => context.GetOpenApiSpecVersion("v1");

            action.Should().Throw<InvalidOperationException>();
        }

        [DataTestMethod]
        [DataRow("v2", OpenApiSpecVersion.OpenApi2_0)]
        [DataRow("V2", OpenApiSpecVersion.OpenApi2_0)]
        [DataRow("v3", OpenApiSpecVersion.OpenApi3_0)]
        [DataRow("V3", OpenApiSpecVersion.OpenApi3_0)]
        public void Given_Type_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(string version, OpenApiSpecVersion expected)
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetOpenApiSpecVersion(version);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_InvalidVersion_When_GetOpenApiFormat_Invoked_Then_It_Should_Throw_Exception()
        {
            var context = new OpenApiHttpTriggerContext();

            Action action = () => context.GetOpenApiFormat("text");

            action.Should().Throw<InvalidOperationException>();
        }

        [DataTestMethod]
        [DataRow("yml", OpenApiFormat.Yaml)]
        [DataRow("YML", OpenApiFormat.Yaml)]
        [DataRow("yaml", OpenApiFormat.Yaml)]
        [DataRow("YAML", OpenApiFormat.Yaml)]
        [DataRow("json", OpenApiFormat.Json)]
        [DataRow("JSON", OpenApiFormat.Json)]
        public void Given_Type_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(string format, OpenApiFormat expected)
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetOpenApiFormat(format);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_NoAuthLevel_When_GetDocumentAuthLevel_Invoked_Then_It_Should_Return_Anonymous()
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetDocumentAuthLevel();

            result.Should().Be(OpenApiAuthLevelType.Anonymous);
        }

        [DataTestMethod]
        [DataRow("Anonymous", OpenApiAuthLevelType.Anonymous)]
        [DataRow("User", OpenApiAuthLevelType.User)]
        [DataRow("Function", OpenApiAuthLevelType.Function)]
        [DataRow("System", OpenApiAuthLevelType.System)]
        [DataRow("Admin", OpenApiAuthLevelType.Admin)]
        [DataRow("ServiceAccount", OpenApiAuthLevelType.Anonymous)]
        public void Given_AuthLevel_When_GetDocumentAuthLevel_Invoked_Then_It_Should_Return_Result(string authLevel, OpenApiAuthLevelType expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__Document", authLevel);

            var context = new OpenApiHttpTriggerContext();

            var result = context.GetDocumentAuthLevel();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_NoAuthLevel_When_GetUIAuthLevel_Invoked_Then_It_Should_Return_Anonymous()
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetUIAuthLevel();

            result.Should().Be(OpenApiAuthLevelType.Anonymous);
        }

        [DataTestMethod]
        [DataRow("Anonymous", OpenApiAuthLevelType.Anonymous)]
        [DataRow("User", OpenApiAuthLevelType.User)]
        [DataRow("Function", OpenApiAuthLevelType.Function)]
        [DataRow("System", OpenApiAuthLevelType.System)]
        [DataRow("Admin", OpenApiAuthLevelType.Admin)]
        [DataRow("ServiceAccount", OpenApiAuthLevelType.Anonymous)]
        public void Given_AuthLevel_When_GetUIAuthLevel_Invoked_Then_It_Should_Return_Result(string authLevel, OpenApiAuthLevelType expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__UI", authLevel);

            var context = new OpenApiHttpTriggerContext();

            var result = context.GetUIAuthLevel();

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_NoApiKey_When_GetSwaggerAuthKey_Invoked_Then_It_Should_Return_Empty()
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetSwaggerAuthKey();

            result.Should().BeEmpty();
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_ApiKey_When_GetSwaggerAuthKey_Invoked_Then_It_Should_Return_Result(string apiKey)
        {
            Environment.SetEnvironmentVariable("OpenApi__ApiKey", apiKey);

            var context = new OpenApiHttpTriggerContext();

            var result = context.GetSwaggerAuthKey();

            result.Should().Be(apiKey);
        }

        [TestMethod]
        public async Task Given_Type_When_GetDocumentFilterCollection_Invoked_Then_It_Should_Return_Result()
        {
            var location = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            var context = new OpenApiHttpTriggerContext();

            var result = (await context.SetApplicationAssemblyAsync(location, false))
                                      .GetDocumentFilterCollection();

            result.Should().NotBeNull();
            result.DocumentFilters.Should().NotBeNull();
            result.DocumentFilters.Should().HaveCount(0);
        }
    }
}
