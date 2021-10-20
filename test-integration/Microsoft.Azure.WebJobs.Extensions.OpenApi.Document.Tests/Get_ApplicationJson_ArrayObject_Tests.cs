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
    public class Get_ApplicationJson_ArrayObject_Tests
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
        [DataRow("/get-applicationjson-array", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-array", "get", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-array", "get", "200", "application/json", "arrayObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("arrayObjectModel", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType)
        {
            var schemas = this._doc["components"]["schemas"];

            var schema = schemas[@ref];

            schema.Should().NotBeNull();
            schema.Value<string>("type").Should().Be(refType);
        }

        [DataTestMethod]
        [DataRow("arrayObjectModel", "object", "objectValue", "array")]
        [DataRow("arrayObjectModel", "object", "booleanValue", "array")]
        [DataRow("arrayObjectModel", "object", "stringValue", "array")]
        [DataRow("arrayObjectModel", "object", "int32Value", "array")]
        [DataRow("arrayObjectModel", "object", "int64Value", "array")]
        [DataRow("arrayObjectModel", "object", "floatValue", "array")]
        [DataRow("arrayObjectModel", "object", "decimalValue", "array")]
        [DataRow("arrayObjectModel", "object", "stringObjectValue", "array")]
        [DataRow("arrayObjectModel", "object", "objectArrayValue", "array")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string refType, string propertyName, string propertyType)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value = properties[propertyName];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
        }

        [DataTestMethod]
        [DataRow("arrayObjectModel", "object", "objectValue", "array", "object")]
        [DataRow("arrayObjectModel", "object", "booleanValue", "array", "boolean")]
        [DataRow("arrayObjectModel", "object", "stringValue", "array", "string")]
        [DataRow("arrayObjectModel", "object", "int32Value", "array", "integer")]
        [DataRow("arrayObjectModel", "object", "int64Value", "array", "integer")]
        [DataRow("arrayObjectModel", "object", "floatValue", "array", "number")]
        [DataRow("arrayObjectModel", "object", "decimalValue", "array", "number")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaPropertyItems(string @ref, string refType, string propertyName, string propertyType, string itemType)
        {
            var items = this._doc["components"]["schemas"][@ref]["properties"][propertyName]["items"];

            var type = items["type"];

            type.Should().NotBeNull();
            type.Value<string>().Should().Be(itemType);
        }

        [DataTestMethod]
        [DataRow("arrayObjectModel", "object", "stringObjectValue", "array", "stringObjectModel")]
        [DataRow("arrayObjectModel", "object", "objectArrayValue", "array", "list_object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaPropertyItemReference(string @ref, string refType, string propertyName, string propertyType, string itemRef)
        {
            var items = this._doc["components"]["schemas"][@ref]["properties"][propertyName]["items"];

            var itemReference = items["$ref"];

            itemReference.Should().NotBeNull();
            itemReference.Value<string>().Should().Be($"#/components/schemas/{itemRef}");

            this._doc["components"]["schemas"][itemRef].Should().NotBeNullOrEmpty();
        }
    }
}
