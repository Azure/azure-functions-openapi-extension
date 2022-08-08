using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class DefaultOpenApiConfigurationOptionsTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            Environment.SetEnvironmentVariable("OpenApi__DocVersion", null);
            Environment.SetEnvironmentVariable("OpenApi__DocTitle", null);
            Environment.SetEnvironmentVariable("OpenApi__DocDescription", null);
            Environment.SetEnvironmentVariable("OpenApi__HostNames", null);
            Environment.SetEnvironmentVariable("OpenApi__Version", null);
            Environment.SetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT", "Development");
            Environment.SetEnvironmentVariable("OpenApi__ForceHttp", null);
            Environment.SetEnvironmentVariable("OpenApi__ForceHttps", null);
        }

        [TestMethod]
        public void Given_Type_When_Instantiated_Then_Properties_Should_Return_Value()
        {
            Environment.SetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT", "Development");

            var options = new DefaultOpenApiConfigurationOptions();

            options.Info.Should().NotBeNull();
            options.Info.Version.Should().Be("1.0.0");
            options.Info.Title.Should().Be("OpenAPI Document on Azure Functions");

            options.Servers.Should().NotBeNull();
            options.Servers.Should().HaveCount(0);

            options.OpenApiVersion.Should().Be(OpenApiVersionType.V2);
            options.IncludeRequestingHostName.Should().BeTrue();
            options.ForceHttp.Should().BeFalse();
            options.ForceHttps.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow(null, "1.0.0")]
        [DataRow("", "1.0.0")]
        [DataRow("v1.0", "v1.0")]
        [DataRow("1.0", "1.0")]
        public void Given_OpenApiDocVersion_When_Instantiated_Then_Property_Should_Return_Value(string version, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocVersion", version);

            var options = new DefaultOpenApiConfigurationOptions();

            options.Info.Version.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, "OpenAPI Document on Azure Functions")]
        [DataRow("", "OpenAPI Document on Azure Functions")]
        [DataRow("hello world", "hello world")]
        public void Given_OpenApiDocTitle_When_Instantiated_Then_Property_Should_Return_Value(string title, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocTitle", title);

            var options = new DefaultOpenApiConfigurationOptions();

            options.Info.Title.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, "This is the OpenAPI Document on Azure Functions")]
        [DataRow("", "This is the OpenAPI Document on Azure Functions")]
        [DataRow("hello world", "hello world")]
        public void Given_OpenApiDocDescription_When_Instantiated_Then_Property_Should_Return_Value(string description, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocDescription", description);

            var options = new DefaultOpenApiConfigurationOptions();

            options.Info.Description.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(0, null)]
        [DataRow(0, "")]
        [DataRow(1, "https://localhost")]
        [DataRow(2, "https://localhost", "https://loremipsum")]
        [DataRow(2, "https://localhost", " https://loremipsum")]
        [DataRow(1, "https://localhost ", " ")]
        public void Given_HostNames_When_Instantiated_Then_Property_Should_Return_Value_Count(int expected, params string[] hostnames)
        {
            Environment.SetEnvironmentVariable("OpenApi__HostNames", string.Join(",", hostnames));

            var options = new DefaultOpenApiConfigurationOptions();

            options.Servers.Should().HaveCount(expected);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("https://localhost")]
        [DataRow("https://localhost", "https://loremipsum")]
        [DataRow("https://localhost", " https://loremipsum")]
        [DataRow("https://localhost ", " ")]
        public void Given_HostNames_When_Instantiated_Then_Property_Should_Return_Value(params string[] hostnames)
        {
            Environment.SetEnvironmentVariable("OpenApi__HostNames", string.Join(",", hostnames));

            var options = new DefaultOpenApiConfigurationOptions();

            var expectedHostnames = hostnames
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select((s, i) => (i, s.Trim()));

            foreach (var (index, hostname) in expectedHostnames)
            {
                options.Servers[index].Url.Should().Be(hostname);
            }
        }

        [DataTestMethod]
        [DataRow(null, OpenApiVersionType.V2)]
        [DataRow("", OpenApiVersionType.V2)]
        [DataRow("v2", OpenApiVersionType.V2)]
        [DataRow("V2", OpenApiVersionType.V2)]
        [DataRow("v3", OpenApiVersionType.V3)]
        [DataRow("V3", OpenApiVersionType.V3)]
        public void Given_OpenApiVersion_When_Instantiated_Then_Property_Should_Return_Value(string version, OpenApiVersionType expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__Version", version);

            var options = new DefaultOpenApiConfigurationOptions();

            options.OpenApiVersion.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("Development", true)]
        [DataRow("Production", false)]
        public void Given_Environment_When_Instantiated_Then_Property_Should_Return_Value(string environment, bool expected)
        {
            Environment.SetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT", environment);

            var options = new DefaultOpenApiConfigurationOptions();

            options.IncludeRequestingHostName.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, "1.0.0")]
        [DataRow("", "1.0.0")]
        [DataRow("v1.0", "v1.0")]
        [DataRow("1.0", "1.0")]
        public void Given_EnvironmentVariable_When_GetOpenApiDocVersion_Invoked_Then_It_Should_Return_Result(string version, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocVersion", version);

            var result = DefaultOpenApiConfigurationOptions.GetOpenApiDocVersion();

            result.Should().BeOfType<string>();
            (result as string).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, "OpenAPI Document on Azure Functions")]
        [DataRow("", "OpenAPI Document on Azure Functions")]
        [DataRow("hello world", "hello world")]
        public void Given_EnvironmentVariable_When_GetOpenApiDocTitle_Invoked_Then_It_Should_Return_Result(string title, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocTitle", title);

            var result = DefaultOpenApiConfigurationOptions.GetOpenApiDocTitle();

            result.Should().BeOfType<string>();
            (result as string).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, "This is the OpenAPI Document on Azure Functions")]
        [DataRow("", "This is the OpenAPI Document on Azure Functions")]
        [DataRow("hello world", "hello world")]
        public void Given_OpenApiDocDescription_When_Instantiated_Then_Property_Should_Return_Result(string description, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocDescription", description);

            var result = DefaultOpenApiConfigurationOptions.GetOpenApiDocDescription();

            result.Should().BeOfType<string>();
            (result as string).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, 0)]
        [DataRow("", 0)]
        [DataRow("https://localhost", 1)]
        [DataRow("https://localhost,https://loremipsum", 2)]
        public void Given_EnvironmentVariable_When_GetHostNames_Invoked_Then_It_Should_Return_Result(string hostnames, int expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__HostNames", hostnames);

            var result = DefaultOpenApiConfigurationOptions.GetHostNames();

            result.Should().BeOfType<List<OpenApiServer>>();
            (result as List<OpenApiServer>).Should().HaveCount(expected);
        }

        [DataTestMethod]
        [DataRow(null, OpenApiVersionType.V2)]
        [DataRow("", OpenApiVersionType.V2)]
        [DataRow("v2", OpenApiVersionType.V2)]
        [DataRow("V2", OpenApiVersionType.V2)]
        [DataRow("v3", OpenApiVersionType.V3)]
        [DataRow("V3", OpenApiVersionType.V3)]
        public void Given_EnvironmentVariable_When_GetOpenApiVersion_Invoked_Then_It_Should_Return_Result(string version, OpenApiVersionType expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__Version", version);

            var result = DefaultOpenApiConfigurationOptions.GetOpenApiVersion();

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("Development", true)]
        [DataRow("Production", false)]
        public void Given_EnvironmentVariable_When_IsFunctionsRuntimeEnvironmentDevelopment_Invoked_Then_It_Should_Return_Result(string environment, bool expected)
        {
            Environment.SetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT", environment);

            var result = DefaultOpenApiConfigurationOptions.IsFunctionsRuntimeEnvironmentDevelopment();

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("true", true)]
        [DataRow("false", false)]
        [DataRow("", false)]
        [DataRow(null, false)]
        public void Given_EnvironmentVariable_When_IsHttpForced_Invoked_Then_It_Should_Return_Result(string forceHttps, bool expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__ForceHttp", forceHttps);

            var result = DefaultOpenApiConfigurationOptions.IsHttpForced();

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("true", true)]
        [DataRow("false", false)]
        [DataRow("", false)]
        [DataRow(null, false)]
        public void Given_EnvironmentVariable_When_IsHttpsForced_Invoked_Then_It_Should_Return_Result(string forceHttps, bool expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__ForceHttps", forceHttps);

            var result = DefaultOpenApiConfigurationOptions.IsHttpsForced();

            result.Should().Be(expected);
        }

        public void Given_Type_When_Instantiated_Then_It_Should_Return_EmptyListOfDocumentFilters()
        {
            var options = new DefaultOpenApiConfigurationOptions();

            options.DocumentFilters.Should().NotBeNull();
            options.DocumentFilters.Should().BeEmpty();
        }
    }
}
