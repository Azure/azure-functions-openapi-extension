using System;
using System.IO;
using System.Linq;
using System.Text;

using FluentAssertions;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests
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
            var context = new Mock<FunctionContext>();

            var ports = new[] { 80, 443 };
            var baseHost = $"{hostname}{(ports.Contains(port) ? string.Empty : $":{port}")}";
            var uri = Uri.TryCreate($"{scheme}://{baseHost}?{key}={value}", UriKind.Absolute, out var tried) ? tried : null;
            var bytes = Encoding.UTF8.GetBytes(payload);
            var body = new MemoryStream(bytes);

            var req = (HttpRequestData) new FakeHttpRequestData(context.Object, uri, body);

            var result = new HttpRequestObject(req);

            result.Scheme.Should().Be(scheme);
            result.Host.Value.Should().Be(baseHost);
            result.Query.Should().ContainKey(key);
            ((string) result.Query[key]).Should().Be(value);
            (new StreamReader(result.Body)).ReadToEnd().Should().Be(payload);

            body.Dispose();
        }
    }
}
