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
        [DataRow("/get-applicationjson-array", "get", "200", "application/json", "microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType)
        {
            var schemas = this._doc["components"]["schemas"];

            var schema = schemas[@ref];

            schema.Should().NotBeNull();
            schema.Value<string>("type").Should().Be(refType);
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "objectValue", "array")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "booleanValue", "array")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "stringValue", "array")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "int32Value", "array")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "int64Value", "array")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "floatValue", "array")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "decimalValue", "array")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "stringObjectValue", "array")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "objectArrayValue", "array")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string refType, string propertyName, string propertyType)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value = properties[propertyName];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "listStringObjectValue", "microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ListStringObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaPropertyWithReference(string @ref, string refType, string propertyName, string reference)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value = properties[propertyName];

            value.Should().NotBeNull();
            value.Value<string>("$ref").Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "objectValue", "array", "object")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "booleanValue", "array", "boolean")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "stringValue", "array", "string")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "int32Value", "array", "integer")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "int64Value", "array", "integer")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "floatValue", "array", "number")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "decimalValue", "array", "number")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaPropertyItems(string @ref, string refType, string propertyName, string propertyType, string itemType)
        {
            var items = this._doc["components"]["schemas"][@ref]["properties"][propertyName]["items"];

            var type = items["type"];

            type.Should().NotBeNull();
            type.Value<string>().Should().Be(itemType);
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "stringObjectValue", "array", "microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.StringObjectModel")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.ArrayObjectModel", "object", "objectArrayValue", "array", "list_object")]
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
