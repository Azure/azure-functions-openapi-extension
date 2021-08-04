using System;
using System.Linq;

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
        [DataRow("http", "localhost", 80)]
        [DataRow("http", "localhost", 7071)]
        [DataRow("https", "localhost", 443)]
        [DataRow("https", "localhost", 47071)]
        public void Given_Parameter_When_Instantiated_Then_It_Should_Return_Result(string scheme, string hostname, int port)
        {
            var context = new Mock<FunctionContext>();

            var ports = new[] { 80, 443 };
            var baseHost = $"{hostname}{(ports.Contains(port) ? string.Empty : $":{port}")}";
            var uri = Uri.TryCreate($"{scheme}://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context.Object, uri);

            var result = new HttpRequestObject(req);

            result.Scheme.Should().Be(scheme);
            result.Host.Value.Should().Be(baseHost);
        }
    }
}
