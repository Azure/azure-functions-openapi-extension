using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class HttpRequestDataObjectExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_GetScheme_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => HttpRequestDataObjectExtensions.GetScheme(null, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("http", "http")]
        [DataRow("https", "https")]
        public void Given_NullOptions_When_GetScheme_Invoked_Then_It_Should_Return_Result(string scheme, string expected)
        {
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);

            var result = HttpRequestDataObjectExtensions.GetScheme(req.Object, null);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("http", true, true, "https")]
        [DataRow("http", true, false, "https")]
        [DataRow("http", false, true, "http")]
        [DataRow("http", false, false, "http")]
        [DataRow("https", true, true, "https")]
        [DataRow("https", true, false, "https")]
        [DataRow("https", false, true, "http")]
        [DataRow("https", false, false, "https")]
        public void Given_Options_When_GetScheme_Invoked_Then_It_Should_Return_Result(string scheme, bool forceHttps, bool forceHttp, string expected)
        {
            var req = new Mock<IHttpRequestDataObject>();
            req.SetupGet(p => p.Scheme).Returns(scheme);

            var options = new Mock<IOpenApiConfigurationOptions>();
            options.SetupGet(p => p.ForceHttps).Returns(forceHttps);
            options.SetupGet(p => p.ForceHttp).Returns(forceHttp);

            var result = HttpRequestDataObjectExtensions.GetScheme(req.Object, options.Object);

            result.Should().Be(expected);
        }
    }
}
