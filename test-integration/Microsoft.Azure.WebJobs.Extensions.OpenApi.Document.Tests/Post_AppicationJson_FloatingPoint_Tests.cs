using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Document.Tests
{
    [TestClass]
    [TestCategory(Constants.TestCategory)]
    public class Post_AppicationJson_FloatingPoint_Tests
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
        [DataRow("/post-applicationjson-single", "post")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_Single(string path, string operationType)
        {
            var requestBody = this._doc["paths"][path][operationType]["requestBody"];

            requestBody.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-double", "post")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_Double(string path, string operationType)
        {
            var requestBody = this._doc["paths"][path][operationType]["requestBody"];

            requestBody.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-decimal", "post")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_Decimal(string path, string operationType)
        {
            var requestBody = this._doc["paths"][path][operationType]["requestBody"];

            requestBody.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-decimal", "post", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-single", "post", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_Single(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-double", "post", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_Double(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-decimal", "post", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_Decimal(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-single", "post", "text/plain", "number")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_Single(string path, string operationType, string contentType, string propertyType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var value = content[contentType]["schema"];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-double", "post", "text/plain", "number")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_Double(string path, string operationType, string contentType, string propertyType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var value = content[contentType]["schema"];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-decimal", "post", "text/plain", "number")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_Decimal(string path, string operationType, string contentType, string propertyType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var value = content[contentType]["schema"];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
        }


        [DataTestMethod]
        [DataRow("/post-applicationjson-single", "post", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_Single(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-double", "post", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_Double(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-decimal", "post", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_Decimal(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-single", "post", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_Single(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataRow("/post-applicationjson-double", "post", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_Double(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataRow("/post-applicationjson-decimal", "post", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_Decimal(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-single", "post", "200", "application/json", "floatingPointObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_Single (string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }
        [DataTestMethod]
        [DataRow("/post-applicationjson-double", "post", "200", "application/json", "floatingPointObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_Double(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-decimal", "post", "200", "application/json", "floatingPointObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_Decimal(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }


        [DataTestMethod]
        [DataRow("floatingPointObjectModel", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType)
        {
            var schemas = this._doc["components"]["schemas"];

            var schema = schemas[@ref];

            schema.Should().NotBeNull();
            schema.Value<string>("type").Should().Be(refType);
        }

        [DataTestMethod]
        [DataRow("floatingPointObjectModel", "singleValue", "number", "doubleValue", "number", "decimalValue", "number")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string propertyName, string propertyType, string propertyName1, string propertyType1, string propertyName2, string propertyType2)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value = properties[propertyName];
            var value1 = properties[propertyName1];
            var value2 = properties[propertyName2];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType1);

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType2);
        }

    }
}
