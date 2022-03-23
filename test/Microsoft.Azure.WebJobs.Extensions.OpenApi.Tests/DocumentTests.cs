using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Filters;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests
{
    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        public void Given_Null_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new Document((IDocumentHelper)null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_That_When_InitialiseDocument_Invoked_Then_It_Should_Return_Result()
        {
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var result = doc.InitialiseDocument();

            result.Should().NotBeNull();
            doc.OpenApiDocument.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Info_When_AddMetadata_Invoked_Then_It_Should_Return_Result(string title)
        {
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var info = new OpenApiInfo() { Title = title };

            var result = doc.InitialiseDocument()
                            .AddMetadata(info);

            result.OpenApiDocument.Info.Should().NotBeNull();
            result.OpenApiDocument.Info.Title.Should().Be(title);
        }

        [DataTestMethod]
        [DataRow("http", "localhost", 80, null)]
        [DataRow("http", "localhost", 80, "")]
        [DataRow("http", "localhost", 80, "api")]
        [DataRow("http", "localhost", 7071, null)]
        [DataRow("http", "localhost", 7071, "")]
        [DataRow("http", "localhost", 7071, "api")]
        [DataRow("https", "localhost", 443, null)]
        [DataRow("https", "localhost", 443, "")]
        [DataRow("https", "localhost", 443, "api")]
        [DataRow("https", "localhost", 47071, null)]
        [DataRow("https", "localhost", 47071, "")]
        [DataRow("https", "localhost", 47071, "api")]
        public void Given_NoOptions_When_AddServer_Invoked_Then_It_Should_Return_Result(string scheme, string host, int port, string routePrefix)
        {
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var ports = new[] { 80, 443 };
            var baseHost = $"{host}{(ports.Contains(port) ? string.Empty : $":{port}")}";
            var url = $"{scheme}://{baseHost}/{routePrefix}".TrimEnd('/');
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);
            req.SetupGet(p => p.Host).Returns(new HostString(baseHost));

            var result = doc.InitialiseDocument()
                            .AddServer(req.Object, routePrefix);

            result.OpenApiDocument.Servers.Should().HaveCount(1);
            result.OpenApiDocument.Servers.First().Url.Should().Be(url);
        }

        [DataTestMethod]
        [DataRow("http", "localhost", 80, null)]
        [DataRow("http", "localhost", 80, "")]
        [DataRow("http", "localhost", 80, "api")]
        [DataRow("http", "localhost", 7071, null)]
        [DataRow("http", "localhost", 7071, "")]
        [DataRow("http", "localhost", 7071, "api")]
        [DataRow("https", "localhost", 443, null)]
        [DataRow("https", "localhost", 443, "")]
        [DataRow("https", "localhost", 443, "api")]
        [DataRow("https", "localhost", 47071, null)]
        [DataRow("https", "localhost", 47071, "")]
        [DataRow("https", "localhost", 47071, "api")]
        public void Given_EmptyOptions_When_AddServer_Invoked_Then_It_Should_Return_Result(string scheme, string host, int port, string routePrefix)
        {
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var ports = new[] { 80, 443 };
            var baseHost = $"{host}{(ports.Contains(port) ? string.Empty : $":{port}")}";
            var url = $"{scheme}://{baseHost}/{routePrefix}".TrimEnd('/');
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);
            req.SetupGet(p => p.Host).Returns(new HostString(baseHost));

            var options = new Mock<IOpenApiConfigurationOptions>();
            options.SetupGet(p => p.Servers).Returns(new List<OpenApiServer>());

            var result = doc.InitialiseDocument()
                            .AddServer(req.Object, routePrefix, options.Object);

            result.OpenApiDocument.Servers.Should().HaveCount(1);
            result.OpenApiDocument.Servers.First().Url.Should().Be(url);
        }

        [DataTestMethod]
        [DataRow("http", "localhost", 80, null, "helloworld")]
        [DataRow("http", "localhost", 80, "", "helloworld")]
        [DataRow("http", "localhost", 80, "api", "helloworld")]
        [DataRow("http", "localhost", 7071, null, "helloworld")]
        [DataRow("http", "localhost", 7071, "", "helloworld")]
        [DataRow("http", "localhost", 7071, "api", "helloworld")]
        [DataRow("https", "localhost", 443, null, "helloworld")]
        [DataRow("https", "localhost", 443, "", "helloworld")]
        [DataRow("https", "localhost", 443, "api", "helloworld")]
        [DataRow("https", "localhost", 47071, null, "helloworld")]
        [DataRow("https", "localhost", 47071, "", "helloworld")]
        [DataRow("https", "localhost", 47071, "api", "helloworld")]
        public void Given_ExtraServers_And_Include_When_AddServer_Invoked_Then_It_Should_Return_Result(string scheme, string host, int port, string routePrefix, string server)
        {
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var ports = new[] { 80, 443 };
            var baseHost = $"{host}{(ports.Contains(port) ? string.Empty : $":{port}")}";
            var url = $"{scheme}://{baseHost}/{routePrefix}".TrimEnd('/');
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);
            req.SetupGet(p => p.Host).Returns(new HostString(baseHost));

            var servers = new List<OpenApiServer>() { new OpenApiServer() { Url = server } };
            var options = new Mock<IOpenApiConfigurationOptions>();
            options.SetupGet(p => p.Servers).Returns(servers);
            options.SetupGet(p => p.IncludeRequestingHostName).Returns(true);

            var result = doc.InitialiseDocument()
                            .AddServer(req.Object, routePrefix, options.Object);

            result.OpenApiDocument.Servers.Should().HaveCount(2);
            result.OpenApiDocument.Servers.First().Url.Should().Be(url);
            result.OpenApiDocument.Servers.Last().Url.Should().Be(server);
        }

        [DataTestMethod]
        [DataRow("http", "localhost", 80, null, "helloworld")]
        [DataRow("http", "localhost", 80, "", "helloworld")]
        [DataRow("http", "localhost", 80, "api", "helloworld")]
        [DataRow("http", "localhost", 7071, null, "helloworld")]
        [DataRow("http", "localhost", 7071, "", "helloworld")]
        [DataRow("http", "localhost", 7071, "api", "helloworld")]
        [DataRow("https", "localhost", 443, null, "helloworld")]
        [DataRow("https", "localhost", 443, "", "helloworld")]
        [DataRow("https", "localhost", 443, "api", "helloworld")]
        [DataRow("https", "localhost", 47071, null, "helloworld")]
        [DataRow("https", "localhost", 47071, "", "helloworld")]
        [DataRow("https", "localhost", 47071, "api", "helloworld")]
        public void Given_ExtraServers_And_Exclude_When_AddServer_Invoked_Then_It_Should_Return_Result(string scheme, string host, int port, string routePrefix, string server)
        {
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var ports = new[] { 80, 443 };
            var baseHost = $"{host}{(ports.Contains(port) ? string.Empty : $":{port}")}";
            var url = $"{scheme}://{baseHost}/{routePrefix}".TrimEnd('/');
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);
            req.SetupGet(p => p.Host).Returns(new HostString(baseHost));

            var servers = new List<OpenApiServer>() { new OpenApiServer() { Url = server } };
            var options = new Mock<IOpenApiConfigurationOptions>();
            options.SetupGet(p => p.Servers).Returns(servers);
            options.SetupGet(p => p.IncludeRequestingHostName).Returns(false);

            var result = doc.InitialiseDocument()
                            .AddServer(req.Object, routePrefix, options.Object);

            result.OpenApiDocument.Servers.Should().HaveCount(1);
            result.OpenApiDocument.Servers.First().Url.Should().Be(server);
        }

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
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);

            var hostString = new HostString(host);
            req.SetupGet(p => p.Host).Returns(hostString);

            var result = doc.InitialiseDocument()
                            .AddServer(req.Object, routePrefix, null);

            result.OpenApiDocument.Servers.Should().HaveCount(1);
            result.OpenApiDocument.Servers.First().Url.Should().Be(expected);
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
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);

            var hostString = new HostString(host);
            req.SetupGet(p => p.Host).Returns(hostString);

            var options = new Mock<IOpenApiConfigurationOptions>();
            options.SetupGet(p => p.ForceHttps).Returns(forceHttps);
            options.SetupGet(p => p.ForceHttp).Returns(forceHttp);
            options.SetupGet(p => p.Servers).Returns(new List<OpenApiServer>());

            var result = doc.InitialiseDocument()
                            .AddServer(req.Object, routePrefix, options.Object);

            result.OpenApiDocument.Servers.Should().HaveCount(1);
            result.OpenApiDocument.Servers.First().Url.Should().Be(expected);
        }

        [TestMethod]
        public void Given_Null_When_AddNamingStrategy_Invoked_Then_It_Should_Throw_Exception()
        {
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            Action action = () => doc.AddNamingStrategy(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_That_When_AddNamingStrategy_Invoked_Then_It_Should_Return_Result()
        {
            var field = typeof(Document).GetField("_strategy", BindingFlags.Instance | BindingFlags.NonPublic);
            var strategy = new DefaultNamingStrategy();
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var result = doc.AddNamingStrategy(strategy);

            field.GetValue(result).Should().NotBeNull();
            field.GetValue(result).Should().BeOfType<DefaultNamingStrategy>();
        }

        [TestMethod]
        public void Given_Null_When_AddVisitors_Invoked_Then_It_Should_Throw_Exception()
        {
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            Action action = () => doc.AddVisitors(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_That_When_AddVisitors_Invoked_Then_It_Should_Return_Result()
        {
            var field = typeof(Document).GetField("_collection", BindingFlags.Instance | BindingFlags.NonPublic);
            var collection = new VisitorCollection();
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var result = doc.AddVisitors(collection);

            field.GetValue(result).Should().NotBeNull();
            field.GetValue(result).Should().BeOfType<VisitorCollection>();
        }

        [TestMethod]
        public void Given_Null_When_ApplyDocumentFilters_Invoked_Then_It_Should_Throw_Exception()
        {
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            Action action = () => doc.ApplyDocumentFilters(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_That_When_ApplyDocumentFilters_Invoked_Then_It_Should_Invoke_Each_Filter()
        {
            var documentFilter = new Mock<IDocumentFilter>();
            var collection = new DocumentFilterCollection(new List<IDocumentFilter> { documentFilter.Object });
            var openApiDocument = new OpenApiDocument();
            var doc = new Document(openApiDocument);

            var req = new Mock<IHttpRequestDataObject>();

            doc.AddServer(req.Object, "");
            doc.ApplyDocumentFilters(collection);

            documentFilter.Verify(x => x.Apply(req.Object, openApiDocument), Times.Once());
        }

        [TestMethod]
        public async Task Given_VersionAndFormat_When_RenderAsync_Invoked_Then_It_Should_Return_Result()
        {
            var helper = new Mock<IDocumentHelper>();
            var doc = new Document(helper.Object);

            var result = await doc.InitialiseDocument()
                                  .RenderAsync(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);

            dynamic json = JObject.Parse(result);

            ((string)json?.swagger).Should().BeEquivalentTo("2.0");
        }

        [TestMethod]
        public async Task Given_Metadata_When_RenderAsync_Invoked_Then_It_Should_Return_Result()
        {
            var helper = new Mock<IDocumentHelper>();

            var title = "hello world";
            var info = new OpenApiInfo() { Title = title };

            var doc = new Document(helper.Object);

            var result = await doc.InitialiseDocument()
                                  .AddMetadata(info)
                                  .RenderAsync(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);

            dynamic json = JObject.Parse(result);

            ((string)json?.info?.title).Should().BeEquivalentTo(title);
        }

        [TestMethod]
        public async Task Given_ServerDetails_When_RenderAsync_Invoked_Then_It_Should_Return_Result()
        {
            var helper = new Mock<IDocumentHelper>();

            var scheme = "https";
            var host = "localhost";
            var routePrefix = "api";

            var url = $"{scheme}://{host}";
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);
            req.SetupGet(p => p.Host).Returns(new HostString(host));

            var doc = new Document(helper.Object);

            var result = await doc.InitialiseDocument()
                                  .AddServer(req.Object, routePrefix)
                                  .RenderAsync(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);

            dynamic json = JObject.Parse(result);

            ((string)json?.host).Should().BeEquivalentTo(host);
            ((string)json?.basePath).Should().BeEquivalentTo($"/{routePrefix}");
            ((string)json?.schemes[0]).Should().BeEquivalentTo(scheme);
        }

        [DataTestMethod]
        [DataRow(true, "localhost", "localhost")]
        [DataRow(false, "fabrikam.com", "contoso.com")]
        public async Task Given_ServerDetails_With_ConfigurationOptions_When_RenderAsync_Invoked_Then_It_Should_Return_Result(bool includeRequestingHostName, string host, string expected)
        {
            var helper = new Mock<IDocumentHelper>();

            var scheme = "https";
            var routePrefix = "api";

            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);
            req.SetupGet(p => p.Host).Returns(new HostString(host));

            var options = new Mock<IOpenApiConfigurationOptions>();
            options.SetupGet(p => p.IncludeRequestingHostName).Returns(includeRequestingHostName);
            options.SetupGet(p => p.Servers).Returns(new List<OpenApiServer>() { new OpenApiServer() { Url = $"{scheme}://contoso.com/{routePrefix}" } });

            var doc = new Document(helper.Object);

            var result = await doc.InitialiseDocument()
                                  .AddServer(req.Object, routePrefix, options.Object)
                                  .RenderAsync(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);

            dynamic json = JObject.Parse(result);

            ((string)json?.host).Should().BeEquivalentTo(expected);
            ((string)json?.basePath).Should().BeEquivalentTo($"/{routePrefix}");
            ((string)json?.schemes[0]).Should().BeEquivalentTo(scheme);
        }

        [TestMethod]
        public async Task Given_ServerDetails_WithNullRoutePrefix_When_RenderAsync_Invoked_Then_It_Should_Return_Result()
        {
            var helper = new Mock<IDocumentHelper>();

            var scheme = "https";
            var host = "localhost";
            string routePrefix = null;

            var url = $"{scheme}://{host}";
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);
            req.SetupGet(p => p.Host).Returns(new HostString(host));

            var doc = new Document(helper.Object);

            var result = await doc.InitialiseDocument()
                                  .AddServer(req.Object, routePrefix)
                                  .RenderAsync(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);

            dynamic json = JObject.Parse(result);

            ((string)json?.host).Should().BeEquivalentTo(host);
            ((string)json?.basePath).Should().BeEquivalentTo(null);
            ((string)json?.schemes[0]).Should().BeEquivalentTo(scheme);
        }

        [TestMethod]
        public async Task Given_ServerDetails_WithEmptyRoutePrefix_When_RenderAsync_Invoked_Then_It_Should_Return_Result()
        {
            var helper = new Mock<IDocumentHelper>();

            var scheme = "https";
            var host = "localhost";
            var routePrefix = string.Empty;

            var url = $"{scheme}://{host}";
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);
            req.SetupGet(p => p.Host).Returns(new HostString(host));

            var doc = new Document(helper.Object);

            var result = await doc.InitialiseDocument()
                                  .AddServer(req.Object, routePrefix)
                                  .RenderAsync(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);

            dynamic json = JObject.Parse(result);

            ((string)json?.host).Should().BeEquivalentTo(host);
            ((string)json?.basePath).Should().BeEquivalentTo(null);
            ((string)json?.schemes[0]).Should().BeEquivalentTo(scheme);
        }
    }
}
