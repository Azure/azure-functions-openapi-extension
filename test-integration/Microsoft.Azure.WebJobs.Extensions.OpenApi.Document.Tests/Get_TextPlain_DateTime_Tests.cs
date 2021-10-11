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
        [DataRow("/get-textplain-datetime", "get")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_DateTime(string path, string operationType)
        {
            var requestBody = this._doc["paths"][path][operationType]["requestBody"];

            requestBody.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetime", "get", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_DateTime(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetime", "get", "text/plain", "string", "date-time")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_DateTime(string path, string operationType, string contentType, string propertyType, string propertyFormat)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var value = content[contentType]["schema"];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<string>("format").Should().Be(propertyFormat);
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
        [DataRow("/get-textplain-datetimeoffset", "get")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_DateTimeOffset(string path, string operationType)
        {
            var requestBody = this._doc["paths"][path][operationType]["requestBody"];

            requestBody.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetimeoffset", "get", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_DateTimeOffset(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-datetimeoffset", "get", "text/plain", "string", "date-time")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_DateTimeOffset(string path, string operationType, string contentType, string propertyType, string propertyFormat)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var value = content[contentType]["schema"];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<string>("format").Should().Be(propertyFormat);
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
    }
}  