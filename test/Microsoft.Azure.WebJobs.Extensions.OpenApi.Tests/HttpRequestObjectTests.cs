using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests
{
    [TestClass]
    public class HttpRequestObjectTests
    {
        [TestMethod]
        public void Given_Null_Parameter_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new HttpRequestObject(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("http", "localhost", 80, "hello", "world", "lorem ipsum")]
        [DataRow("http", "localhost", 7071, "lorem", "ipsum", "hello world")]
        [DataRow("https", "localhost", 443, "hello", "world", "lorem ipsum")]
        [DataRow("https", "localhost", 47071, "lorem", "ipsum", "hello world")]
        public void Given_Parameter_When_Instantiated_Then_It_Should_Return_Result(string scheme, string hostname, int port, string key, string value, string payload)
        {
            var req = new Mock<HttpRequest>();
            req.SetupGet(p => p.Scheme).Returns(scheme);

            var ports = new[] { 80, 443 };
            var baseHost = $"{hostname}{(ports.Contains(port) ? string.Empty : $":{port}")}";
            var hoststring = new HostString(baseHost);
            req.SetupGet(p => p.Host).Returns(hoststring);

            var dict = new Dictionary<string, StringValues>() { { key, new StringValues(value) } };
            var query = new QueryCollection(dict);
            req.SetupGet(p => p.Query).Returns(query);

            var bytes = Encoding.UTF8.GetBytes(payload);
            var body = new MemoryStream(bytes);
            req.SetupGet(p => p.Body).Returns(body);

            var result = new HttpRequestObject(req.Object);

            result.Scheme.Should().Be(scheme);
            result.Host.Value.Should().Be(baseHost);
            result.Query.Should().ContainKey(key);
            ((string) result.Query[key]).Should().Be(value);
            (new StreamReader(result.Body)).ReadToEnd().Should().Be(payload);

            body.Dispose();
        }
    }
}
