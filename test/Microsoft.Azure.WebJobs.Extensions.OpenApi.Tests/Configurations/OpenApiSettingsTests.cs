using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Resolvers;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests.Configurations
{
    [TestClass]
    public class OpenApiSettingsTests
    {
        [DataTestMethod]
        [DataRow("true", "false", true, false, "lorem", "Function", AuthorizationLevel.Function, "Anonymous", AuthorizationLevel.Anonymous, "http://localhost:7071", "https://contoso.com/api/")]
        [DataRow("false", "false", false, false, "ipsum", "Anonymous", AuthorizationLevel.Anonymous, "Function", AuthorizationLevel.Function, "http://contoso", "https://fabrikam.com/api/")]
        [DataRow("false", "true", false, true, "ipsum", "Anonymous", AuthorizationLevel.Anonymous, "Function", AuthorizationLevel.Function, "http://contoso", "https://fabrikam.com/api/")]
        public void Given_EnvironmentVariables_When_Instantiated_Then_It_Should_Return_Result(string hideSwaggerUI, bool expectedHideSwaggerUI,
                                                                                              string hideDocument, bool expectedHideDocument,
                                                                                              string apiKey,
                                                                                              string authLevelDoc, AuthorizationLevel expectedAuthLevelDoc,
                                                                                              string authLevelUI, AuthorizationLevel expectedAuthLevelUI,
                                                                                              string proxyUrl, string hostnames)
        {
            Environment.SetEnvironmentVariable("OpenApi__HideSwaggerUI", hideSwaggerUI);
            Environment.SetEnvironmentVariable("OpenApi__HideDocument", hideDocument);
            Environment.SetEnvironmentVariable("OpenApi__ApiKey", apiKey);
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__Document", authLevelDoc);
            Environment.SetEnvironmentVariable("OpenApi__AuthLevel__UI", authLevelUI);
            Environment.SetEnvironmentVariable("OpenApi__BackendProxyUrl", proxyUrl);
            Environment.SetEnvironmentVariable("OpenApi__HostNames", hostnames);

            var config = ConfigurationResolver.Resolve();
            var settings = config.Get<OpenApiSettings>("OpenApi");

            settings.HideSwaggerUI.Should().Be(expectedHideSwaggerUI);
            settings.HideDocument.Should().Be(expectedHideDocument);
            settings.ApiKey.Should().Be(apiKey);
            settings.AuthLevel.Document.Should().Be(expectedAuthLevelDoc);
            settings.AuthLevel.UI.Should().Be(expectedAuthLevelUI);
            settings.BackendProxyUrl.Should().Be(proxyUrl);
            settings.HostNames.Should().Be(hostnames);
        }
    }
}
