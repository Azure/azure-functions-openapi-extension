using System;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Document.Tests
{
    [TestClass]
    [TestCategory(Constants.TestCategory)]
    public class Get_TextPlain_DateTime_Tests
    {
        private static HttpClient http = new HttpClient();

        private JObject _doc;

        [TestInitialize]
        public async Task Init()
        {
            var json = await http.GetStringAsync(Constants.OpenApiDocEndpoint).ConfigureAwait(false);
            this._doc = JsonConvert.DeserializeObject<JObject>(json);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetime")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_DateTime(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetime", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_DateTime(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetime", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_DateTime(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetime", "get", "200", "text/plain", "string", "date-time")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_DateTime(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetimeoffset")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_DateTimeOffset(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetimeoffset", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_DateTimeOffset(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetimeoffset", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_DateTimeOffset(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetimeoffset", "get", "200", "text/plain", "string", "date-time")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_DateTimeOffset(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
        }
    }
}  