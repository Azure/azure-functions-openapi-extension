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
            [DataRow("/post-applicationjson-int32", "post")]
            [DataRow("/post-applicationjson-int64", "post")]
            [DataRow("/post-applicationjson-uint16", "post")]
            [DataRow("/post-applicationjson-uint32", "post")]
            [DataRow("/post-applicationjson-uint64", "post")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody(string path, string operationType)
            {
                var requestBody = this._doc["paths"][path][operationType]["requestBody"];

                requestBody.Should().NotBeEmpty();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int16", "post", "text/plain")]
            [DataRow("/post-applicationjson-int32", "post", "text/plain")]
            [DataRow("/post-applicationjson-int64", "post", "text/plain")]
            [DataRow("/post-applicationjson-uint16", "post", "text/plain")]
            [DataRow("/post-applicationjson-uint32", "post", "text/plain")]
            [DataRow("/post-applicationjson-uint64", "post", "text/plain")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType(string path, string operationType, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                content[contentType].Should().NotBeNull();
            }
            
            [DataTestMethod]
            [DataRow("/post-applicationjson-int16", "post", "text/plain", "integer")]
            [DataRow("/post-applicationjson-int32", "post", "text/plain", "integer")]
            [DataRow("/post-applicationjson-int64", "post", "text/plain", "integer")]
            [DataRow("/post-applicationjson-uint16", "post", "text/plain", "integer")]
            [DataRow("/post-applicationjson-uint32", "post", "text/plain", "integer")]
            [DataRow("/post-applicationjson-uint64", "post", "text/plain", "integer")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema(string path, string operationType, string contentType, string propertyType)
            {
                var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

                var value = content[contentType]["schema"];

                value.Should().NotBeNull();
                value.Value<string>("type").Should().Be(propertyType);
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int16", "post", "200")]
            [DataRow("/post-applicationjson-int32", "post", "200")]
            [DataRow("/post-applicationjson-int64", "post", "200")]
            [DataRow("/post-applicationjson-uint16", "post", "200")]
            [DataRow("/post-applicationjson-uint32", "post", "200")]
            [DataRow("/post-applicationjson-uint64", "post", "200")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
            {
                var responses = this._doc["paths"][path][operationType]["responses"];

                responses[responseCode].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int16", "post", "200", "application/json")]
            [DataRow("/post-applicationjson-int32", "post", "200", "application/json")]
            [DataRow("/post-applicationjson-int64", "post", "200", "application/json")]
            [DataRow("/post-applicationjson-uint16", "post", "200", "application/json")]
            [DataRow("/post-applicationjson-uint32", "post", "200", "application/json")]
            [DataRow("/post-applicationjson-uint64", "post", "200", "application/json")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
            {
                var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

                content[contentType].Should().NotBeNull();
            }

            [DataTestMethod]
            [DataRow("/post-applicationjson-int16", "post", "200", "application/json", "integerObjectModel")]
            [DataRow("/post-applicationjson-int32", "post", "200", "application/json", "integerObjectModel")]
            [DataRow("/post-applicationjson-int64", "post", "200", "application/json", "integerObjectModel")]
            [DataRow("/post-applicationjson-uint16", "post", "200", "application/json", "integerObjectModel")]
            [DataRow("/post-applicationjson-uint32", "post", "200", "application/json", "integerObjectModel")]
            [DataRow("/post-applicationjson-uint64", "post", "200", "application/json", "integerObjectModel")]
            public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string reference)
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
            [DataRow("integerObjectModel",  "int16Value", "integer",  "int32",  "int32Value", "integer",  "int32",   "int64Value", "integer", "int64")]
            [DataRow("integerObjectModel", "uInt16Value", "integer",     null, "uInt32Value", "integer",     null,  "uInt64Value", "integer", "int64")]
            public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string propertyName1, string propertyType1, string propertyFormat1, string propertyName2, string propertyType2, string propertyFormat2, string propertyName3, string propertyType3, string propertyFormat3)
            {
                var properties = this._doc["components"]["schemas"][@ref]["properties"];

                var value1 = properties[propertyName1];
                var value2 = properties[propertyName2];
                var value3 = properties[propertyName3];

                value1.Should().NotBeNull();
                value1.Value<string>("type").Should().Be(propertyType1);
                value1.Value<string>("format").Should().Be(propertyFormat1);

                value2.Should().NotBeNull();
                value2.Value<string>("type").Should().Be(propertyType2);
                value2.Value<string>("format").Should().Be(propertyFormat2);

                value3.Should().NotBeNull();
                value3.Value<string>("type").Should().Be(propertyType3);
                value3.Value<string>("format").Should().Be(propertyFormat3);
            }
        }
    }
}
