using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Resolvers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class OpenApiSettingsTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            Environment.SetEnvironmentVariable("OpenApi__Version", null);
            Environment.SetEnvironmentVariable("OpenApi__DocVersion", null);
            Environment.SetEnvironmentVariable("OpenApi__DocTitle", null);
            Environment.SetEnvironmentVariable("OpenApi__DocDescription", null);
            Environment.SetEnvironmentVariable("OpenApi__HostNames", null);
            Environment.SetEnvironmentVariable("OpenApi__ExcludeRequestingHost", null);
            Environment.SetEnvironmentVariable("OpenApi__ForceHttps", null);
            Environment.SetEnvironmentVariable("OpenApi__ForceHttp", null);
            Environment.SetEnvironmentVariable("OpenApi__HideSwaggerUI", null);
            Environment.SetEnvironmentVariable("OpenApi__HideDocument", null);
            Environment.SetEnvironmentVariable("OpenApi__ApiKey", null);
            Environment.SetEnvironmentVariable("OpenApi__BackendProxyUrl", null);
        }

        [DataTestMethod]
        [DataRow(null, OpenApiVersionType.V2)]
        [DataRow("", OpenApiVersionType.V2)]
        [DataRow("v2", OpenApiVersionType.V2)]
        [DataRow("v3", OpenApiVersionType.V3)]
        public void Given_Version_When_Instantiated_Then_It_Should_Return_Result(string version, OpenApiVersionType expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__Version", version);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.Version.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow("1.0.0", "1.0.0")]
        public void Given_DocVersion_When_Instantiated_Then_It_Should_Return_Result(string version, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocVersion", version);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.DocVersion.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow("hello", "hello")]
        public void Given_DocTitle_When_Instantiated_Then_It_Should_Return_Result(string title, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocTitle", title);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.DocTitle.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow("world", "world")]
        public void Given_DocDescription_When_Instantiated_Then_It_Should_Return_Result(string description, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__DocDescription", description);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.DocDescription.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow("https://contoso", "https://contoso")]
        [DataRow("https://contoso, https://fabrikam", "https://contoso, https://fabrikam")]
        public void Given_HostNames_When_Instantiated_Then_It_Should_Return_Result(string hostnames, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__HostNames", hostnames);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.HostNames.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow("true", true)]
        [DataRow("True", true)]
        [DataRow("false", false)]
        [DataRow("False", false)]
        public void Given_ExcludeRequestingHost_When_Instantiated_Then_It_Should_Return_Result(string excludeRequestingHost, bool expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__ExcludeRequestingHost", excludeRequestingHost);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.ExcludeRequestingHost.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("true", true)]
        [DataRow("false", false)]
        public void Given_ForceHttps_When_Instantiated_Then_It_Should_Return_Result(string https, bool expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__ForceHttps", https);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.ForceHttps.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("true", true)]
        [DataRow("false", false)]
        public void Given_ForceHttp_When_Instantiated_Then_It_Should_Return_Result(string http, bool expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__ForceHttp", http);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.ForceHttp.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("true", true)]
        [DataRow("false", false)]
        public void Given_HideSwaggerUI_When_Instantiated_Then_It_Should_Return_Result(string hide, bool expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__HideSwaggerUI", hide);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.HideSwaggerUI.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("true", true)]
        [DataRow("false", false)]
        public void Given_HideDocument_When_Instantiated_Then_It_Should_Return_Result(string hide, bool expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__HideDocument", hide);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.HideDocument.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow("lorem", "lorem")]
        public void Given_ApiKey_When_Instantiated_Then_It_Should_Return_Result(string apiKey, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__ApiKey", apiKey);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.ApiKey.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow("lorem", "lorem")]
        public void Given_BackendProxyUrl_When_Instantiated_Then_It_Should_Return_Result(string url, string expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__BackendProxyUrl", url);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);

            settings.BackendProxyUrl.Should().Be(expected);
        }
    }
}
