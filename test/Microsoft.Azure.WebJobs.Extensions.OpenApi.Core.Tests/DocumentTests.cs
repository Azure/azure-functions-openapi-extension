using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
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

        [TestMethod]
        public async Task Given_ServerDetails_With_ConfigurationOptions_When_RenderAsync_Invoked_Then_It_Should_Return_Result()
        {
            var helper = new Mock<IDocumentHelper>();

            var scheme = "https";
            var host = "localhost";
            var routePrefix = "api";

            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);
            req.SetupGet(p => p.Host).Returns(new HostString(host));

            var options = new Mock<IOpenApiConfigurationOptions>();
            options.SetupGet(p => p.Servers).Returns(new List<OpenApiServer>() { new OpenApiServer() { Url = $"https://contoso.com/{routePrefix}" } });

            var doc = new Document(helper.Object);

            var result = await doc.InitialiseDocument()
                                  .AddServer(req.Object, routePrefix, options.Object)
                                  .RenderAsync(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);

            dynamic json = JObject.Parse(result);

            ((string)json?.host).Should().BeEquivalentTo(host);
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
