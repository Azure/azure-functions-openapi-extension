using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Document.Tests
{
    public class Post_ApplicationJson_IntegerObject_Tests
    {
        [TestClass]
        [TestCategory(Constants.TestCategory)]
        public class Post_ApplicationJson_int_Tests
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
            [DataRow("/post-applicationjson-int16", "post")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_Int16(string path, string operationType)
            {
                var requestBody = this._doc["paths"][path][operationType]["requestBody"];

                requestBody.Should().NotBeEmpty();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int32", "post")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_int32(string path, string operationType)
            {
                var requestBody = this._doc["paths"][path][operationType]["requestBody"];

                requestBody.Should().NotBeEmpty();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int64", "post")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_Int64(string path, string operationType)
            {
                var requestBody = this._doc["paths"][path][operationType]["requestBody"];

                requestBody.Should().NotBeEmpty();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint16", "post")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_UInt16(string path, string operationType)
            {
                var requestBody = this._doc["paths"][path][operationType]["requestBody"];

                requestBody.Should().NotBeNull();

            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint32", "post")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_UInt32(string path, string operationType)
            {
                var requestBody = this._doc["paths"][path][operationType]["requestBody"];

                requestBody.Should().NotBeNull();

            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint64", "post")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody_UInt64(string path, string operationType)
            {
                var requestBody = this._doc["paths"][path][operationType]["requestBody"];

                requestBody.Should().NotBeNull();

            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int16", "post", "text/plain")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_Int16(string path, string operationType, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int32", "post", "text/plain")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_Int32(string path, string operationType, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int64", "post", "text/plain")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_Int64(string path, string operationType, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint16", "post", "text/plain")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_UInt16(string path, string operationType, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint32", "post", "text/plain")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_UInt32(string path, string operationType, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint64", "post", "text/plain")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType_UInt64(string path, string operationType, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int16", "post", "text/plain", "integer")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_Int16(string path, string operationType, string contentType, string propertyType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                var value = content[contentType]["schema"];

                value.Should().NotBeNull();
                value.Value<string>("type").Should().Be(propertyType);
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int32", "post", "text/plain", "integer")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_Int32(string path, string operationType, string contentType, string propertyType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                var value = content[contentType]["schema"];

                value.Should().NotBeNull();
                value.Value<string>("type").Should().Be(propertyType);
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int64", "post", "text/plain", "integer")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_Int64(string path, string operationType, string contentType, string propertyType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                var value = content[contentType]["schema"];

                value.Should().NotBeNull();
                value.Value<string>("type").Should().Be(propertyType);
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint16", "post", "text/plain", "integer")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_UInt16(string path, string operationType, string contentType, string propertyType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                var value = content[contentType]["schema"];

                value.Should().NotBeNull();
                value.Value<string>("type").Should().Be(propertyType);
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint32", "post", "text/plain", "integer")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_UInt32(string path, string operationType, string contentType, string propertyType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                var value = content[contentType]["schema"];

                value.Should().NotBeNull();
                value.Value<string>("type").Should().Be(propertyType);
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint64", "post", "text/plain", "integer")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_UInt64(string path, string operationType, string contentType, string propertyType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                var value = content[contentType]["schema"];

                value.Should().NotBeNull();
                value.Value<string>("type").Should().Be(propertyType);
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int16", "post", "200")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_Int16(string path, string operationType, string responseCode)
            {
                var responses = this._doc["paths"][path][operationType]["responses"];

                responses[responseCode].Should().NotBeNull();
            }


            [DataTestMethod]
            [DataRow("/post-applicationjson-int32", "post", "200")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_Int32(string path, string operationType, string responseCode)
            {
                var responses = this._doc["paths"][path][operationType]["responses"];

                responses[responseCode].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int64", "post", "200")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_Int64(string path, string operationType, string responseCode)
            {
                var responses = this._doc["paths"][path][operationType]["responses"];

                responses[responseCode].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint16", "post", "200")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_UInt16(string path, string operationType, string responseCode)
            {
                var responses = this._doc["paths"][path][operationType]["responses"];

                responses[responseCode].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint32", "post", "200")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_UInt32(string path, string operationType, string responseCode)
            {
                var responses = this._doc["paths"][path][operationType]["responses"];

                responses[responseCode].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint64", "post", "200")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_UInt64(string path, string operationType, string responseCode)
            {
                var responses = this._doc["paths"][path][operationType]["responses"];

                responses[responseCode].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int16", "post", "200", "application/json")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_Int16(string path, string operationType, string responseCode, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int32", "post", "200", "application/json")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_Int32(string path, string operationType, string responseCode, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int64", "post", "200", "application/json")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_Int64(string path, string operationType, string responseCode, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint16", "post", "200", "application/json")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_UInt16(string path, string operationType, string responseCode, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint32", "post", "200", "application/json")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_UInt32(string path, string operationType, string responseCode, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint64", "post", "200", "application/json")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_UInt64(string path, string operationType, string responseCode, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int16", "post", "200", "application/json", "integerObjectModel")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_Int16(string path, string operationType, string responseCode, string contentType, string reference)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                var @ref = content[contentType]["schema"]["$ref"];

                @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int32", "post", "200", "application/json", "integerObjectModel")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_Int32(string path, string operationType, string responseCode, string contentType, string reference)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                var @ref = content[contentType]["schema"]["$ref"];

                @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int64", "post", "200", "application/json", "integerObjectModel")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_Int64(string path, string operationType, string responseCode, string contentType, string reference)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                var @ref = content[contentType]["schema"]["$ref"];

                @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint16", "post", "200", "application/json", "integerObjectModel")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_UInt16(string path, string operationType, string responseCode, string contentType, string reference)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                var @ref = content[contentType]["schema"]["$ref"];

                @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint32", "post", "200", "application/json", "integerObjectModel")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_UInt32(string path, string operationType, string responseCode, string contentType, string reference)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                var @ref = content[contentType]["schema"]["$ref"];

                @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-uint64", "post", "200", "application/json", "integerObjectModel")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_UInt64(string path, string operationType, string responseCode, string contentType, string reference)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                var @ref = content[contentType]["schema"]["$ref"];

                @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
            }


            [DataTestMethod]
            [DataRow("integerObjectModel", "object")]
            public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType)
            {
                var schemas = this._doc["components"]["schemas"];

                var schema = schemas[@ref];

                schema.Should().NotBeNull();
                schema.Value<string>("type").Should().Be(refType);
            }

            [DataTestMethod]
            [DataRow("integerObjectModel", "int16Value", "integer", "int32Value", "integer", "int64Value", "integer", "uInt16Value", "integer", "uInt32Value", "integer", "uInt64Value", "integer")]
            public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string propertyName1, string propertyType1, string propertyName2, string propertyType2, string propertyName3, string propertyType3, string propertyName4, string propertyType4, string propertyName5, string propertyType5, string propertyName6, string propertyType6)
            {
                var properties = this._doc["components"]["schemas"][@ref]["properties"];

                var value1 = properties[propertyName1];
                var value2 = properties[propertyName2];
                var value3 = properties[propertyName3];
                var value4 = properties[propertyName4];
                var value5 = properties[propertyName5];
                var value6 = properties[propertyName6];

                value1.Should().NotBeNull();
                value1.Value<string>("type").Should().Be(propertyType1);

                value2.Should().NotBeNull();
                value2.Value<string>("type").Should().Be(propertyType2);

                value3.Should().NotBeNull();
                value3.Value<string>("type").Should().Be(propertyType3);

                value4.Should().NotBeNull();
                value4.Value<string>("type").Should().Be(propertyType4);

                value5.Should().NotBeNull();
                value5.Value<string>("type").Should().Be(propertyType5);

                value6.Should().NotBeNull();
                value6.Value<string>("type").Should().Be(propertyType6);

            }
        }
    }
}
