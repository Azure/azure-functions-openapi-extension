using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

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
            var req = Substitute.For<IHttpRequestDataObject>();
            req.Scheme.Returns(scheme);

            var result = HttpRequestDataObjectExtensions.GetScheme(req, null);

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
            var req = Substitute.For<IHttpRequestDataObject>();
            req.Scheme.Returns(scheme);

            var options = Substitute.For<IOpenApiConfigurationOptions>();
            options.ForceHttps.Returns(forceHttps);
            options.ForceHttp.Returns(forceHttp);

            var result = HttpRequestDataObjectExtensions.GetScheme(req, options);

            result.Should().Be(expected);
        }
    }
}
