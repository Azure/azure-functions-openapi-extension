using System;
using System.Collections.Generic;
using System.Reflection;

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
        [TestMethod]
        public void Given_Type_When_Instantiated_Then_Properties_Should_Return_Value()
        {
            var settings = new DefaultOpenApiConfigurationOptions();

            settings.Info.Should().NotBeNull();
            settings.Info.Version.Should().Be("1.0.0");
            settings.Info.Title.Should().Be("Azure Functions OpenAPI Extension");

            settings.Servers.Should().NotBeNull();
            settings.Servers.Should().HaveCount(0);

            settings.OpenApiVersion.Should().Be(OpenApiVersionType.V2);
            settings.IncludeRequestingHostName.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("Development", true)]
        [DataRow("Production", false)]
        public void Given_Environment_When_Instantiated_Then_Property_Should_Return_Value(string environment, bool expected)
        {
            Environment.SetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT", environment);

            var settings = new DefaultOpenApiConfigurationOptions();

            settings.IncludeRequestingHostName.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, 0)]
        [DataRow("", 0)]
        [DataRow("https://localhost", 1)]
        [DataRow("https://localhost,https://loremipsum", 2)]
        public void Given_EnvironmentVariable_When_GetHostNames_Invoked_Then_It_Should_Return_Result(string hostnames, int expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__HostNames", hostnames);

            var settings = new DefaultOpenApiConfigurationOptions();
            var method = typeof(DefaultOpenApiConfigurationOptions).GetMethod("GetHostNames", BindingFlags.NonPublic | BindingFlags.Static);

            var result = method.Invoke(settings, null);

            result.Should().BeOfType<List<OpenApiServer>>();
            (result as List<OpenApiServer>).Should().HaveCount(expected);
        }
    }
}
