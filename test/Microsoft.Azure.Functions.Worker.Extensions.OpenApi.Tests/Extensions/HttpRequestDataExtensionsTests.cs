using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Extensions
{
    [TestClass]
    public class HttpRequestDataExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_Headers_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => OpenApiHttpRequestDataExtensions.Headers(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_NullHeader_When_Headers_Invoked_Then_It_Should_Return_Result()
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: null);

            var result = OpenApiHttpRequestDataExtensions.Headers(req);

            result.Count.Should().Be(0);
       }

        [TestMethod]
        public void Given_NoHeader_When_Headers_Invoked_Then_It_Should_Return_Result()
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var headers = new Dictionary<string, string>();

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: headers);

            var result = OpenApiHttpRequestDataExtensions.Headers(req);

            result.Count.Should().Be(0);
       }

        [DataTestMethod]
        [DataRow("hello=world", "hello")]
        [DataRow("hello=world&lorem=ipsum", "hello", "lorem")]
        public void Given_Headers_When_Headers_Invoked_Then_It_Should_Return_Result(string headerstring, params string[] keys)
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var kvps = headerstring.Split('&').ToDictionary(p => p.Split('=').First(), p => p.Split('=').Last());
            var headers = new Dictionary<string, string>(kvps);

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: headers);

            var result = OpenApiHttpRequestDataExtensions.Headers(req);

            result.Count.Should().Be(keys.Length);
            result.Keys.Should().Contain(keys);
        }

        [TestMethod]
        public void Given_Null_When_Header_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => OpenApiHttpRequestDataExtensions.Header(null, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("hello=world", "hello", "world")]
        [DataRow("hello=world&lorem=ipsum", "hello", "world")]
        [DataRow("hello=world&lorem=ipsum", "lorem", "ipsum")]
        public void Given_Headers_When_Header_Invoked_Then_It_Should_Return_Result(string headerstring, string key, string expected)
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var kvps = headerstring.Split('&').ToDictionary(p => p.Split('=').First(), p => p.Split('=').Last());
            var headers = new Dictionary<string, string>(kvps);

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: headers);

            var result = (string) OpenApiHttpRequestDataExtensions.Header(req, key);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_NullHeader_When_Header_Invoked_Then_It_Should_Return_Result()
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: null);

            var result = (string) OpenApiHttpRequestDataExtensions.Header(req, "hello");

            result.Should().BeNull();
       }

        [TestMethod]
        public void Given_NoHeader_When_Header_Invoked_Then_It_Should_Return_Result()
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var headers = new Dictionary<string, string>();

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: headers);

            var result = (string) OpenApiHttpRequestDataExtensions.Query(req, "hello");

            result.Should().BeNull();
       }

        [TestMethod]
        public void Given_Null_When_Queries_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => OpenApiHttpRequestDataExtensions.Queries(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_NullQuerystring_When_Queries_Invoked_Then_It_Should_Return_Result()
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: null);

            var result = OpenApiHttpRequestDataExtensions.Queries(req);

            result.Count.Should().Be(0);
       }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void Given_NoQuerystring_When_Queries_Invoked_Then_It_Should_Return_Result(string querystring)
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}?{querystring}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: null);

            var result = OpenApiHttpRequestDataExtensions.Queries(req);

            result.Count.Should().Be(0);
       }

        [DataTestMethod]
        [DataRow("hello=world", "hello")]
        [DataRow("hello=world&lorem=ipsum", "hello", "lorem")]
        public void Given_Querystring_When_Queries_Invoked_Then_It_Should_Return_Result(string querystring, params string[] keys)
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}?{querystring}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: null);

            var result = OpenApiHttpRequestDataExtensions.Queries(req);

            result.Count.Should().Be(keys.Length);
            result.Keys.Should().Contain(keys);
        }

        [TestMethod]
        public void Given_Null_When_Query_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => OpenApiHttpRequestDataExtensions.Query(null, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("hello=world", "hello", "world")]
        [DataRow("hello=world&lorem=ipsum", "hello", "world")]
        [DataRow("hello=world&lorem=ipsum", "lorem", "ipsum")]
        public void Given_Querystring_When_Query_Invoked_Then_It_Should_Return_Result(string querystring, string key, string expected)
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}?{querystring}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: null);

            var result = (string) OpenApiHttpRequestDataExtensions.Query(req, key);

            result.Should().Be(expected);
        }

        [TestMethod]
        public void Given_NullQuerystring_When_Query_Invoked_Then_It_Should_Return_Result()
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: null);

            var result = (string) OpenApiHttpRequestDataExtensions.Query(req, "hello");

            result.Should().BeNull();
       }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void Given_NoQuerystring_When_Query_Invoked_Then_It_Should_Return_Result(string querystring)
        {
            var context = Substitute.For<FunctionContext>();

            var baseHost = "localhost";
            var uri = Uri.TryCreate($"http://{baseHost}?{querystring}", UriKind.Absolute, out var tried) ? tried : null;

            var req = (HttpRequestData) new FakeHttpRequestData(context, uri, headers: null);

            var result = (string) OpenApiHttpRequestDataExtensions.Query(req, "hello");

            result.Should().BeNull();
       }
    }
}
