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
    public class Get_ApplicationJson_GenericAndRecursiveObject_Tests
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
        [DataRow("/get-applicationjson-genericandrecursive", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-genericandrecursive", "get", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-genericandrecursive", "get", "200", "application/json", "genericAndRecursiveObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("genericAndRecursiveObjectModel", "object", "listValue", "array")]
        [DataRow("genericAndRecursiveObjectModel", "object", "dictionaryValue", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_PropertyType(string @ref, string refType, string propertyName, string propertyType)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];
            var value = properties[propertyName];

            var type = value["type"];

            type.Should().NotBeNull();
            type.Value<string>().Should().Be(propertyType);
        }

        [DataTestMethod]
        [DataRow("genericAndRecursiveObjectModel", "object", "listValue", "items", "genericAndRecursiveObjectModel")]
        [DataRow("genericAndRecursiveObjectModel", "object", "dictionaryValue", "additionalProperties", "genericAndRecursiveObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType, string propertyName, string propertyType, string itemReference)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];
            var value = properties[propertyName];

            var itemRef = value[propertyType]["$ref"];

            itemRef.Should().NotBeNull();
            itemRef.Value<string>().Should().Be($"#/components/schemas/{itemReference}");
        }
    }
}
