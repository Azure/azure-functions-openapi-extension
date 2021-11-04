using System.Collections.Generic;
using System.Reflection;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
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
        [DataRow("http", "localhost", "api", "/api")]
        [DataRow("https", "localhost", "api/", "/api")]
        [DataRow("http", "localhost", "api/prod", "/api/prod")]
        [DataRow("https", "localhost", "api/prod/", "/api/prod")]
        public void Given_NullOptions_When_AddServer_Invoked_Then_It_Should_Return_SwaggerUIApiPrefix(string scheme, string host, string routePrefix, string expected)
        {
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);

            var hostString = new HostString(host);
            req.SetupGet(p => p.Host).Returns(hostString);

            var ui = new SwaggerUI();

            ui.AddServer(req.Object, routePrefix, null);

            var fi = ui.GetType().GetField("_swaggerUiApiPrefix", BindingFlags.Instance | BindingFlags.NonPublic);

            (fi.GetValue(ui) as string).Should().Be(expected);
        }
    }
}
