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
    public class Post_ApplicationJson_DateTime_Tests
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
        [DataRow("/post-applicationjson-datetime", "post")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_DateTime(string path, string operationType)
        {
            var requestBody = this._doc["paths"][path][operationType]["requestBody"];

            requestBody.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetime", "post", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_DateTime(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetime", "post", "text/plain", "string", "date-time")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_DateTime(string path, string operationType, string contentType, string propertyType, string propertyFormat)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var value = content[contentType]["schema"];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<string>("format").Should().Be(propertyFormat);
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetime", "post", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_DateTime(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetime", "post", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_DateTime(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetime", "post", "200", "application/json", "dateTimeObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_DateTime(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetimeoffset", "post")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_DateTimeOffset(string path, string operationType)
        {
            var requestBody = this._doc["paths"][path][operationType]["requestBody"];

            requestBody.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetimeoffset", "post", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_DateTimeOffset(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetimeoffset", "post", "text/plain", "string", "date-time")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_DateTimeOffset(string path, string operationType, string contentType, string propertyType, string propertyFormat)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var value = content[contentType]["schema"];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<string>("format").Should().Be(propertyFormat);
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetimeoffset", "post", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_DateTimeOffset(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetimeoffset", "post", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_DateTimeOffset(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-datetimeoffset", "post", "200", "application/json", "dateTimeObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_DateTimeOffset(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }


        [DataTestMethod]
        [DataRow("dateTimeObjectModel", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType)
        {
            var schemas = this._doc["components"]["schemas"];

            var schema = schemas[@ref];

            schema.Should().NotBeNull();
            schema.Value<string>("type").Should().Be(refType);
        }

        [DataTestMethod]
        [DataRow("dateTimeObjectModel", "dateTimeValue", "string", "date-time", "dateTimeOffsetValue", "string", "date-time")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string propertyName1, string propertyType1, string propertyFormat1, string propertyName2, string propertyType2, string propertyFormat2)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value1 = properties[propertyName1];
            var value2 = properties[propertyName2];

            value1.Should().NotBeNull();
            value1.Value<string>("type").Should().Be(propertyType1);
            value1.Value<string>("format").Should().Be(propertyFormat1);

            value2.Should().NotBeNull();
            value2.Value<string>("type").Should().Be(propertyType2);
            value2.Value<string>("format").Should().Be(propertyFormat2);
        }
    }
}  