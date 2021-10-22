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
    public class Get_ApplicationJson_DictionaryObject_Tests
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
        [DataRow("/get-applicationjson-dictionary", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-dictionary", "get", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-dictionary", "get", "200", "application/json", "dictionaryObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("dictionaryObjectModel", "object", "objectValue", "object", "object")]
        [DataRow("dictionaryObjectModel", "object", "booleanValue", "object", "boolean")]
        [DataRow("dictionaryObjectModel", "object", "stringValue", "object", "string")]
        [DataRow("dictionaryObjectModel", "object", "int32Value", "object", "integer")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType, string propertyName, string propertyType, string itemType)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];
            var value = properties[propertyName];

            var type = value["additionalProperties"]["type"];

            type.Should().NotBeNull();
            type.Value<string>().Should().Be(itemType);
        }

        [DataTestMethod]
        [DataRow("dictionaryObjectModel", "objectObjectValue", "object", "objectObjectModel")]
        [DataRow("dictionaryObjectModel", "booleanObjectValue", "object", "booleanObjectModel")]
        [DataRow("dictionaryObjectModel", "stringObjectValue", "object", "stringObjectModel")]
        [DataRow("dictionaryObjectModel", "integerObjectValue", "object", "integerObjectModel")]
        [DataRow("dictionaryObjectModel", "objectArrayValue", "object", "list_object")]
        [DataRow("dictionaryObjectModel", "booleanArrayValue", "object", "list_boolean")]
        [DataRow("dictionaryObjectModel", "stringArrayValue", "object", "list_string")]
        [DataRow("dictionaryObjectModel", "int32ArrayValue", "object", "list_int32")]

        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string propertyName, string propertyType, string itemRef)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];
            var value = properties[propertyName];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value["additionalProperties"].Should().NotBeNullOrEmpty();
            value["additionalProperties"].Value<string>("$ref").Should().Be($"#/components/schemas/{itemRef}");
            this._doc["components"]["schemas"][itemRef].Should().NotBeNullOrEmpty();
        }

        [DataTestMethod]
        [DataRow("list_object", "array")]
        [DataRow("list_boolean", "array")]
        [DataRow("list_string", "array")]
        [DataRow("list_int32", "array")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentReferenceSchema(string @ref, string refType)
        {
            var schemas = this._doc["components"]["schemas"];
            var schema = schemas[@ref]["type"];

            schema.Should().NotBeNull();
            schema.Value<string>().Should().Be(refType);
            schema.Value<string>().Should().Be(refType);
        }

        [DataTestMethod]
        [DataRow("list_object", "array", "object")]
        [DataRow("list_boolean", "array", "bool")]
        [DataRow("list_string", "array", "string")]
        [DataRow("list_int32", "array", "integer")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentReferenceSchema(string @ref, string refType, string itemType)
        {
            var items = this._doc["components"]["schemas"][@ref];
            var type = items["items"]["type"];

            type.Should().NotBeNull();
            type.Value<string>().Should().Be(itemType);
        }
    }
}
