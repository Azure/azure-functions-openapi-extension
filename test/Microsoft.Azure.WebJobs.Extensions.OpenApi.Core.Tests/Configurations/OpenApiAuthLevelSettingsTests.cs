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
    public class OpenApiAuthLevelSettingsTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__Document", null);
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__UI", null);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow("Anonymous", OpenApiAuthLevelType.Anonymous)]
        [DataRow("anonymous", OpenApiAuthLevelType.Anonymous)]
        [DataRow("Function", OpenApiAuthLevelType.Function)]
        [DataRow("function", OpenApiAuthLevelType.Function)]
        [DataRow("User", OpenApiAuthLevelType.User)]
        [DataRow("user", OpenApiAuthLevelType.User)]
        [DataRow("Admin", OpenApiAuthLevelType.Admin)]
        [DataRow("admin", OpenApiAuthLevelType.Admin)]
        [DataRow("System", OpenApiAuthLevelType.System)]
        [DataRow("system", OpenApiAuthLevelType.System)]
        public void Given_AuthLevelDoc_When_Instantiated_Then_It_Should_Return_Result(string authLevel, OpenApiAuthLevelType? expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__Document", authLevel);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);
            var authlevel = settings.AuthLevel;

            authlevel.Document.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow("Anonymous", OpenApiAuthLevelType.Anonymous)]
        [DataRow("anonymous", OpenApiAuthLevelType.Anonymous)]
        [DataRow("Function", OpenApiAuthLevelType.Function)]
        [DataRow("function", OpenApiAuthLevelType.Function)]
        [DataRow("User", OpenApiAuthLevelType.User)]
        [DataRow("user", OpenApiAuthLevelType.User)]
        [DataRow("Admin", OpenApiAuthLevelType.Admin)]
        [DataRow("admin", OpenApiAuthLevelType.Admin)]
        [DataRow("System", OpenApiAuthLevelType.System)]
        [DataRow("system", OpenApiAuthLevelType.System)]
        public void Given_AuthLevelUI_When_Instantiated_Then_It_Should_Return_Result(string authLevel, OpenApiAuthLevelType? expected)
        {
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__UI", authLevel);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>(OpenApiSettings.Name);
            var authlevel = settings.AuthLevel;

            authlevel.UI.Should().Be(expected);
        }
    }
}
