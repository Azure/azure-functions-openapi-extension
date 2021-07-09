using System;
using System.Collections.Generic;
using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
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
            var options = new DefaultOpenApiConfigurationOptions();

            options.Info.Should().NotBeNull();
            options.Info.Version.Should().Be("1.0.0");
            options.Info.Title.Should().Be(typeof(DefaultOpenApiConfigurationOptions).GetTypeInfo().Assembly.GetName().Name);

            options.Servers.Should().NotBeNull();
            options.Servers.Should().HaveCount(0);

            options.OpenApiVersion.Should().Be(OpenApiVersionType.V2);
            options.IncludeRequestingHostName.Should().BeFalse();
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
        [DataRow(null)]
        [DataRow("")]
        [DataRow("hello world")]
        public void Given_OpenApiDocTitle_When_Instantiated_Then_Property_Should_Return_Value(string title)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocTitle", title);

            var expected = title.IsNullOrWhiteSpace()
                               ? typeof(DefaultOpenApiConfigurationOptions).GetTypeInfo().Assembly.GetName().Name
                               : title;

            var options = new DefaultOpenApiConfigurationOptions();

            options.Info.Title.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, 0)]
        [DataRow("", 0)]
        [DataRow("https://localhost", 1)]
        [DataRow("https://localhost,https://loremipsum", 2)]
        public void Given_HostNames_When_Instantiated_Then_Property_Should_Return_Value(string hostnames, int expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__HostNames", hostnames);

            var options = new DefaultOpenApiConfigurationOptions();

            options.Servers.Should().HaveCount(expected);
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

            var options = new DefaultOpenApiConfigurationOptions();
            var method = typeof(DefaultOpenApiConfigurationOptions).GetMethod("GetOpenApiDocVersion", BindingFlags.NonPublic | BindingFlags.Static);

            var result = method.Invoke(options, null);

            result.Should().BeOfType<string>();
            (result as string).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("hello world")]
        public void Given_EnvironmentVariable_When_GetOpenApiDocTitle_Invoked_Then_It_Should_Return_Result(string title)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocTitle", title);

            var expected = title.IsNullOrWhiteSpace()
                               ? typeof(DefaultOpenApiConfigurationOptions).GetTypeInfo().Assembly.GetName().Name
                               : title;

            var options = new DefaultOpenApiConfigurationOptions();
            var method = typeof(DefaultOpenApiConfigurationOptions).GetMethod("GetOpenApiDocTitle", BindingFlags.NonPublic | BindingFlags.Static);

            var result = method.Invoke(options, new[] { (Type)null });

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

            var options = new DefaultOpenApiConfigurationOptions();
            var method = typeof(DefaultOpenApiConfigurationOptions).GetMethod("GetHostNames", BindingFlags.NonPublic | BindingFlags.Static);

            var result = method.Invoke(options, null);

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
        public void Given_EnvironmentVariable_When_GetHostNames_Invoked_Then_It_Should_Return_Result(string version, OpenApiVersionType expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__Version", version);

            var options = new DefaultOpenApiConfigurationOptions();
            var method = typeof(DefaultOpenApiConfigurationOptions).GetMethod("GetOpenApiVersion", BindingFlags.NonPublic | BindingFlags.Static);

            var result = method.Invoke(options, null);

            result.Should().BeOfType<OpenApiVersionType>();
            ((OpenApiVersionType)result).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("Development", true)]
        [DataRow("Production", false)]
        public void Given_EnvironmentVariable_When_GetHostNames_Invoked_Then_It_Should_Return_Result(string environment, bool expected)
        {
            Environment.SetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT", environment);

            var options = new DefaultOpenApiConfigurationOptions();
            var method = typeof(DefaultOpenApiConfigurationOptions).GetMethod("IsFunctionsRuntimeEnvironmentDevelopment", BindingFlags.NonPublic | BindingFlags.Static);

            var result = method.Invoke(options, null);

            result.Should().BeOfType<bool>();
            ((bool)result).Should().Be(expected);
        }
    }
}
