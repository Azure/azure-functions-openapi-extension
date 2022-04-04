using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
{
    [TestClass]
    public class SwaggerUITests
    {
        [DataTestMethod]
        [DataRow("http", "localhost", "api", "http://localhost/api")]
        [DataRow("https", "localhost", "api", "https://localhost/api")]
        [DataRow("http", "localhost:7071", "api", "http://localhost:7071/api")]
        [DataRow("https", "localhost:47071", "api", "https://localhost:47071/api")]
        [DataRow("http", "localhost", "", "http://localhost")]
        [DataRow("https", "localhost", "", "https://localhost")]
        [DataRow("http", "localhost:7071", "", "http://localhost:7071")]
        [DataRow("https", "localhost:47071", "", "https://localhost:47071")]
        [DataRow("http", "localhost", null, "http://localhost")]
        [DataRow("https", "localhost", null, "https://localhost")]
        [DataRow("http", "localhost:7071", null, "http://localhost:7071")]
        [DataRow("https", "localhost:47071", null, "https://localhost:47071")]
        public void Given_NullOptions_When_AddServer_Invoked_Then_It_Should_Return_BaseUrl(string scheme, string host, string routePrefix, string expected)
        {
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);

            var hostString = new HostString(host);
            req.SetupGet(p => p.Host).Returns(hostString);

            var ui = new SwaggerUI();

            ui.AddServer(req.Object, routePrefix, null);

            var fi = ui.GetType().GetField("_baseUrl", BindingFlags.Instance | BindingFlags.NonPublic);

            (fi.GetValue(ui) as string).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("http", "localhost", "api", true, true, "https://localhost/api")]
        [DataRow("https", "localhost", "api", true, true, "https://localhost/api")]
        [DataRow("http", "localhost:7071", "api", true, true, "https://localhost:7071/api")]
        [DataRow("https", "localhost:47071", "api", true, true, "https://localhost:47071/api")]
        [DataRow("http", "localhost", "", true, true, "https://localhost")]
        [DataRow("https", "localhost", "", true, true, "https://localhost")]
        [DataRow("http", "localhost:7071", "", true, true, "https://localhost:7071")]
        [DataRow("https", "localhost:47071", "", true, true, "https://localhost:47071")]
        [DataRow("http", "localhost", null, true, true, "https://localhost")]
        [DataRow("https", "localhost", null, true, true, "https://localhost")]
        [DataRow("http", "localhost:7071", null, true, true, "https://localhost:7071")]
        [DataRow("https", "localhost:47071", null, true, true, "https://localhost:47071")]
        [DataRow("http", "localhost", "api", true, false, "https://localhost/api")]
        [DataRow("https", "localhost", "api", true, false, "https://localhost/api")]
        [DataRow("http", "localhost:7071", "api", true, false, "https://localhost:7071/api")]
        [DataRow("https", "localhost:47071", "api", true, false, "https://localhost:47071/api")]
        [DataRow("http", "localhost", "", true, false, "https://localhost")]
        [DataRow("https", "localhost", "", true, false, "https://localhost")]
        [DataRow("http", "localhost:7071", "", true, false, "https://localhost:7071")]
        [DataRow("https", "localhost:47071", "", true, false, "https://localhost:47071")]
        [DataRow("http", "localhost", null, true, false, "https://localhost")]
        [DataRow("https", "localhost", null, true, false, "https://localhost")]
        [DataRow("http", "localhost:7071", null, true, false, "https://localhost:7071")]
        [DataRow("https", "localhost:47071", null, true, false, "https://localhost:47071")]
        [DataRow("http", "localhost", "api", false, true, "http://localhost/api")]
        [DataRow("https", "localhost", "api", false, true, "http://localhost/api")]
        [DataRow("http", "localhost:7071", "api", false, true, "http://localhost:7071/api")]
        [DataRow("https", "localhost:47071", "api", false, true, "http://localhost:47071/api")]
        [DataRow("http", "localhost", "", false, true, "http://localhost")]
        [DataRow("https", "localhost", "", false, true, "http://localhost")]
        [DataRow("http", "localhost:7071", "", false, true, "http://localhost:7071")]
        [DataRow("https", "localhost:47071", "", false, true, "http://localhost:47071")]
        [DataRow("http", "localhost", null, false, true, "http://localhost")]
        [DataRow("https", "localhost", null, false, true, "http://localhost")]
        [DataRow("http", "localhost:7071", null, false, true, "http://localhost:7071")]
        [DataRow("https", "localhost:47071", null, false, true, "http://localhost:47071")]
        [DataRow("http", "localhost", "api", false, false, "http://localhost/api")]
        [DataRow("https", "localhost", "api", false, false, "https://localhost/api")]
        [DataRow("http", "localhost:7071", "api", false, false, "http://localhost:7071/api")]
        [DataRow("https", "localhost:47071", "api", false, false, "https://localhost:47071/api")]
        [DataRow("http", "localhost", "", false, false, "http://localhost")]
        [DataRow("https", "localhost", "", false, false, "https://localhost")]
        [DataRow("http", "localhost:7071", "", false, false, "http://localhost:7071")]
        [DataRow("https", "localhost:47071", "", false, false, "https://localhost:47071")]
        [DataRow("http", "localhost", null, false, false, "http://localhost")]
        [DataRow("https", "localhost", null, false, false, "https://localhost")]
        [DataRow("http", "localhost:7071", null, false, false, "http://localhost:7071")]
        [DataRow("https", "localhost:47071", null, false, false, "https://localhost:47071")]
        public void Given_Options_When_AddServer_Invoked_Then_It_Should_Return_BaseUrl(string scheme, string host, string routePrefix, bool forceHttps, bool forceHttp, string expected)
        {
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);

            var hostString = new HostString(host);
            req.SetupGet(p => p.Host).Returns(hostString);

            var options = new Mock<IOpenApiConfigurationOptions>();
            options.SetupGet(p => p.ForceHttps).Returns(forceHttps);
            options.SetupGet(p => p.ForceHttp).Returns(forceHttp);
            options.SetupGet(p => p.Servers).Returns(new List<OpenApiServer>());

            var ui = new SwaggerUI();

            ui.AddServer(req.Object, routePrefix, options.Object);

            var fi = ui.GetType().GetField("_baseUrl", BindingFlags.Instance | BindingFlags.NonPublic);

            (fi.GetValue(ui) as string).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("http", "localhost", "api", false, "/api")]
        [DataRow("https", "localhost", "api/", false, "/api")]
        [DataRow("http", "localhost", "api/prod", false, "/api/prod")]
        [DataRow("https", "localhost", "api/prod/", false, "/api/prod")]
        [DataRow("http", "localhost", null, false, "")]
        [DataRow("https", "localhost", null, false, "")]
        [DataRow("http", "localhost", "api", true, "/api")]
        [DataRow("https", "localhost", "api/", true, "/api")]
        [DataRow("http", "localhost", "api/prod", true, "/api/prod")]
        [DataRow("https", "localhost", "api/prod/", true, "/api/prod")]
        [DataRow("http", "localhost", null, true, "")]
        [DataRow("https", "localhost", null, true, "")]
        public void Given_NullOptions_When_AddServer_Invoked_Then_It_Should_Return_SwaggerUIApiPrefix(string scheme, string host, string routePrefix, bool optionsSet, string expected)
        {
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);

            var hostString = new HostString(host);
            req.SetupGet(p => p.Host).Returns(hostString);

            var ui = new SwaggerUI();
            var options = new Mock<IOpenApiConfigurationOptions>();
            options.SetupGet(p => p.Servers).Returns(new List<OpenApiServer>());
            ui.AddServer(req.Object, routePrefix, optionsSet ? options.Object: null);

            var fi = ui.GetType().GetField("_swaggerUiApiPrefix", BindingFlags.Instance | BindingFlags.NonPublic);

            (fi.GetValue(ui) as string).Should().Be(expected);
        }


        [DataTestMethod]
        [DataRow(OpenApiAuthLevelType.Anonymous, null, false, false)]
        [DataRow(OpenApiAuthLevelType.Anonymous, "", false, false)]
        [DataRow(OpenApiAuthLevelType.Anonymous, "configKey", false, false)]
        [DataRow(OpenApiAuthLevelType.User, null, true, false)]
        [DataRow(OpenApiAuthLevelType.User, "", true, false)]
        [DataRow(OpenApiAuthLevelType.User, "configKey", false, true)]
        [DataRow(OpenApiAuthLevelType.Function, null, true, false)]
        [DataRow(OpenApiAuthLevelType.Function, "", true, false)]
        [DataRow(OpenApiAuthLevelType.Function, "configKey", false, true)]
        [DataRow(OpenApiAuthLevelType.System, null, true, false)]
        [DataRow(OpenApiAuthLevelType.System, "", true, false)]
        [DataRow(OpenApiAuthLevelType.System, "configKey", false, true)]
        [DataRow(OpenApiAuthLevelType.Admin, null, true, false)]
        [DataRow(OpenApiAuthLevelType.Admin, "", true, false)]
        [DataRow(OpenApiAuthLevelType.Admin, "configKey", false, true)]
        public void Given_Options_When_IsAuthKeyRequired_Invoked_Then_It_Should_Return_Result(
            OpenApiAuthLevelType configuredAuthLevel, string configKey, bool throwsException, bool expected)
        {
            var ui = new SwaggerUI();
            var method = ui.GetType()
                .GetMethod("IsAuthKeyRequired", BindingFlags.Instance | BindingFlags.NonPublic);

            Func<bool> action = () => (bool)method.Invoke(ui, new object[] { configuredAuthLevel, configKey });

            if (throwsException)
            {
                action.Should().Throw<TargetInvocationException>().And.InnerException.Should()
                    .BeOfType<InvalidOperationException>();
            }
            else
            {
                action.Invoke().Should().Be(expected);
            }
        }


        [DataTestMethod]
        [DataRow(null, OpenApiAuthLevelType.Anonymous, null, null)]
        [DataRow(null, OpenApiAuthLevelType.Anonymous, "configKey", null)]
        [DataRow(null, OpenApiAuthLevelType.Anonymous, "", null)]
        [DataRow("queryKey", OpenApiAuthLevelType.Anonymous, "configKey", "queryKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.Anonymous, null, "queryKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.Anonymous, "", "queryKey")]
        [DataRow("", OpenApiAuthLevelType.Anonymous, null, "")]
        [DataRow("", OpenApiAuthLevelType.Anonymous, "configKey", "")]
        [DataRow("", OpenApiAuthLevelType.Anonymous, "", "")]
        [DataRow(null, OpenApiAuthLevelType.User, null, null, true)]
        [DataRow(null, OpenApiAuthLevelType.User, null, "", true)]
        [DataRow(null, OpenApiAuthLevelType.User, "configKey", "configKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.User, "configKey", "configKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.User, null, "queryKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.User, "", "queryKey")]
        [DataRow("", OpenApiAuthLevelType.User, "configKey", "configKey")]
        [DataRow("", OpenApiAuthLevelType.User, null, "",true)]
        [DataRow("", OpenApiAuthLevelType.User, "", "",true)]
        [DataRow(null, OpenApiAuthLevelType.Function, null, null, true)]
        [DataRow(null, OpenApiAuthLevelType.Function, null, "", true)]
        [DataRow(null, OpenApiAuthLevelType.Function, "configKey", "configKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.Function, "configKey", "configKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.Function, null, "queryKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.Function, "", "queryKey")]
        [DataRow("", OpenApiAuthLevelType.Function, "configKey", "configKey")]
        [DataRow("", OpenApiAuthLevelType.Function, null, "",true)]
        [DataRow("", OpenApiAuthLevelType.Function, "", "",true)]
        [DataRow(null, OpenApiAuthLevelType.System, null, null, true)]
        [DataRow(null, OpenApiAuthLevelType.System, null, "", true)]
        [DataRow(null, OpenApiAuthLevelType.System, "configKey", "configKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.System, "configKey", "configKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.System, null, "queryKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.System, "", "queryKey")]
        [DataRow("", OpenApiAuthLevelType.System, "configKey", "configKey")]
        [DataRow("", OpenApiAuthLevelType.System, null, "",true)]
        [DataRow("", OpenApiAuthLevelType.System, "", "",true)]
        [DataRow(null, OpenApiAuthLevelType.Admin, null, null, true)]
        [DataRow(null, OpenApiAuthLevelType.Admin, null, "", true)]
        [DataRow(null, OpenApiAuthLevelType.Admin, "configKey", "configKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.Admin, "configKey", "configKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.Admin, null, "queryKey")]
        [DataRow("queryKey", OpenApiAuthLevelType.Admin, "", "queryKey")]
        [DataRow("", OpenApiAuthLevelType.Admin, "configKey", "configKey")]
        [DataRow("", OpenApiAuthLevelType.Admin, null, "",true)]
        [DataRow("", OpenApiAuthLevelType.Admin, "", "",true)]
        public async Task Given_AuthKey_Options_When_RenderAsync_Invoked_Then_It_Should_Be_Used_As_Request_Key(
            string queryKey, OpenApiAuthLevelType configuredAuthLevel, string configKey, string expectedRequestKey,
            bool throwsException = false)
        {
            var endpoint = "swagger/ui";
            var baseUrl = "https://localhost:7071";
            var ui = new SwaggerUI();
            ui.AddMetadata(new OpenApiInfo());
            var uiType = ui.GetType();

            //Generate Request Object with query key
            var queryDict = new Dictionary<string, StringValues>();
            if (queryKey != null)
            {
                queryDict["code"] = queryKey;
            }
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Query).Returns(new QueryCollection(queryDict));
            uiType.GetField("_req", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ui, req.Object);

            //Set BaseUrl
            uiType.GetField("_baseUrl", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ui, baseUrl);

            //Set html string just to contain url placeholder
            var swaggerUrlPlaceholder =
                uiType.GetField("SwaggerUrlPlaceholder", BindingFlags.Static | BindingFlags.NonPublic)
                    .GetRawConstantValue() as string;
            uiType.GetField("_indexHtml", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(ui, swaggerUrlPlaceholder);


            Func<Task<string>> action = async () => await ui.RenderAsync(endpoint, configuredAuthLevel, configKey);

            if (throwsException)
            {
                await action.Should().ThrowAsync<InvalidOperationException>();
            }
            else
            {
                var result = await action();
                if (expectedRequestKey != null)
                {
                    result.Should().Contain($"code={expectedRequestKey}");
                }
                else
                {
                    result.Should().NotContain($"code=");
                }
            }
        }
    }
}
