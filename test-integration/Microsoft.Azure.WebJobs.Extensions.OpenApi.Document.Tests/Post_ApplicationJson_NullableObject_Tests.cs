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
    public class Post_ApplicationJson_Nullable_Tests
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
        [DataRow("/post-applicationjson-nullableboolean", "post")]
        [DataRow("/post-applicationjson-nullableuint16", "post")]
        [DataRow("/post-applicationjson-nullableuint32", "post")]
        [DataRow("/post-applicationjson-nullableuint64", "post")]
        [DataRow("/post-applicationjson-nullableint16", "post")]
        [DataRow("/post-applicationjson-nullableint32", "post")]
        [DataRow("/post-applicationjson-nullableint64", "post")]
        [DataRow("/post-applicationjson-nullablesingle", "post")]
        [DataRow("/post-applicationjson-nullabledouble", "post")]
        [DataRow("/post-applicationjson-nullabledecimal", "post")]
        [DataRow("/post-applicationjson-nullabledatetime", "post")]
        [DataRow("/post-applicationjson-nullabledatetimeoffset", "post")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody(string path, string operationType)
        {
            var requestBody = this._doc["paths"][path][operationType]["requestBody"];

            requestBody.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-nullableboolean", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullableuint16", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullableuint32", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullableuint64", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullableint16", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullableint32", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullableint64", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullablesingle", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullabledouble", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullabledecimal", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullabledatetime", "post", "text/plain")]
        [DataRow("/post-applicationjson-nullabledatetimeoffset", "post", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-nullableboolean", "post", "text/plain", "boolean", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema_Boolean(string path, string operationType, string contentType, string propertyType, bool nullable)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var value = content[contentType]["schema"];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<bool>("nullable").Should().Be(nullable);
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-nullableuint16", "post", "text/plain", "integer", "int32",true)]
        [DataRow("/post-applicationjson-nullableuint32", "post", "text/plain", "integer", "int32",true)]
        [DataRow("/post-applicationjson-nullableuint64", "post", "text/plain", "integer", "int64", true)]
        [DataRow("/post-applicationjson-nullableint16", "post", "text/plain", "integer", "int32", true)]
        [DataRow("/post-applicationjson-nullableint32", "post", "text/plain", "integer", "int32", true)]
        [DataRow("/post-applicationjson-nullableint64", "post", "text/plain", "integer", "int64", true)]
        [DataRow("/post-applicationjson-nullablesingle", "post", "text/plain", "number", "float", true)]
        [DataRow("/post-applicationjson-nullabledouble", "post", "text/plain", "number", "double", true)]
        [DataRow("/post-applicationjson-nullabledecimal", "post", "text/plain", "number", "double", true)]
        [DataRow("/post-applicationjson-nullabledatetime", "post", "text/plain", "string", "date-time", true)]
        [DataRow("/post-applicationjson-nullabledatetimeoffset", "post", "text/plain", "string", "date-time", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema(string path, string operationType, string contentType, string propertyType, string propertyFormat, bool nullable)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var value = content[contentType]["schema"];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<string>("format").Should().Be(propertyFormat);
            value.Value<bool>("nullable").Should().Be(nullable);
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-nullableboolean", "post", "200")]
        [DataRow("/post-applicationjson-nullableuint16", "post", "200")]
        [DataRow("/post-applicationjson-nullableuint32", "post", "200")]
        [DataRow("/post-applicationjson-nullableuint64", "post", "200")]
        [DataRow("/post-applicationjson-nullableint16", "post", "200")]
        [DataRow("/post-applicationjson-nullableint32", "post", "200")]
        [DataRow("/post-applicationjson-nullableint64", "post", "200")]
        [DataRow("/post-applicationjson-nullablesingle", "post", "200")]
        [DataRow("/post-applicationjson-nullabledouble", "post", "200")]
        [DataRow("/post-applicationjson-nullabledecimal", "post", "200")]
        [DataRow("/post-applicationjson-nullabledatetime", "post", "200")]
        [DataRow("/post-applicationjson-nullabledatetimeoffset", "post", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-nullableboolean", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullableuint16", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullableuint32", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullableuint64", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullableint16", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullableint32", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullableint64", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullablesingle", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullabledouble", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullabledecimal", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullabledatetime", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-nullabledatetimeoffset", "post", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-nullableboolean", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullableuint16", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullableuint32", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullableuint64", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullableint16", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullableint32", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullableint64", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullablesingle", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullabledouble", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullabledecimal", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullabledatetime", "post", "200", "application/json", "nullableObjectModel")]
        [DataRow("/post-applicationjson-nullabledatetimeoffset", "post", "200", "application/json", "nullableObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("nullableObjectModel", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType)
        {
            var schemas = this._doc["components"]["schemas"];

            var schema = schemas[@ref];

            schema.Should().NotBeNull();
            schema.Value<string>("type").Should().Be(refType);
        }

        [DataTestMethod]
        [DataRow("nullableObjectModel", "booleanValue", "boolean", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty_Boolean(string @ref, string propertyName, string propertyType, bool nullable)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value = properties[propertyName];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<bool>("nullable").Should().Be(nullable);
        }

        [DataTestMethod]
        [DataRow("nullableObjectModel", "uInt16Value", "integer", "int32", true)]
        [DataRow("nullableObjectModel", "uInt32Value", "integer", "int32", true)]
        [DataRow("nullableObjectModel", "uInt64Value", "integer", "int64", true)]
        [DataRow("nullableObjectModel", "int16Value", "integer", "int32",  true)]
        [DataRow("nullableObjectModel", "int32Value", "integer", "int32",  true)]
        [DataRow("nullableObjectModel", "int64Value", "integer", "int64",  true)]
        [DataRow("nullableObjectModel", "singleValue", "number", "float",  true)]
        [DataRow("nullableObjectModel", "doubleValue", "number", "double", true)]
        [DataRow("nullableObjectModel", "decimalValue", "number", "double", true)]
        [DataRow("nullableObjectModel", "dateTimeValue", "string", "date-time", true)]
        [DataRow("nullableObjectModel", "dateTimeOffsetValue", "string", "date-time", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string propertyName, string propertyType, string propertyFormat, bool nullable)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value = properties[propertyName];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<string>("format").Should().Be(propertyFormat);
            value.Value<bool>("nullable").Should().Be(nullable);
        }
    }
}
