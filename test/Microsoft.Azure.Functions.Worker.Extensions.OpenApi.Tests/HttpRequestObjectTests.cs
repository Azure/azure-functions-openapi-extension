using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
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
        [DataRow("http", "localhost", 80, "hello", "world", "dolor", "lorem ipsum")]
        [DataRow("http", "localhost", 7071, "lorem", "ipsum", "sit", "hello world")]
        [DataRow("https", "localhost", 443, "hello", "world", "amet", "lorem ipsum")]
        [DataRow("https", "localhost", 47071, "lorem", "ipsum", "consectetur", "hello world")]
        public void Given_Parameter_When_Instantiated_Then_It_Should_Return_Result(string scheme, string hostname, int port, string key, string value, string authType, string payload)
        {
            var context = new Mock<FunctionContext>();

            var ports = new[] { 80, 443 };
            var baseHost = $"{hostname}{(ports.Contains(port) ? string.Empty : $":{port}")}";
            var uri = Uri.TryCreate($"{scheme}://{baseHost}?{key}={value}", UriKind.Absolute, out var tried) ? tried : null;

            var headers = new Dictionary<string, string>() { { key, value } };

            var identities = new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(
                    authenticationType: authType,
                    nameType: ClaimsIdentity.DefaultNameClaimType,
                    roleType: ClaimsIdentity.DefaultRoleClaimType)
            };

            var bytes = Encoding.UTF8.GetBytes(payload);
            var body = new MemoryStream(bytes);

            var req = (HttpRequestData)new FakeHttpRequestData(context.Object, uri, headers, identities, body);

            var result = new HttpRequestObject(req);

            result.Scheme.Should().Be(scheme);
            result.Host.Value.Should().Be(baseHost);
            result.Headers.Should().ContainKey(key);
            result.Query.Should().ContainKey(key);
            ((string)result.Query[key]).Should().Be(value);
            result.Identities.Where(p => p.AuthenticationType == authType).Should().HaveCount(1);
            (new StreamReader(result.Body)).ReadToEnd().Should().Be(payload);

            body.Dispose();
        }
    }
}
