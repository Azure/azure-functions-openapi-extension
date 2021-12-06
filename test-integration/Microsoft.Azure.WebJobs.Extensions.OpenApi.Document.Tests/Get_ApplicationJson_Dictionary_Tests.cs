using System;
using System.Linq;
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
    public class Get_ApplicationJson_Dictionary_Tests
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
        [DataRow("/get-applicationjson-dictionary")]
        [DataRow("/get-applicationjson-dictionary-idictionary")]
        [DataRow("/get-applicationjson-dictionary-ireadonlydictionary")]
        [DataRow("/get-applicationjson-dictionary-keyvaluepair")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-dictionary", "get")]
        [DataRow("/get-applicationjson-dictionary-idictionary", "get")]
        [DataRow("/get-applicationjson-dictionary-ireadonlydictionary", "get")]
        [DataRow("/get-applicationjson-dictionary-keyvaluepair", "get")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationType(string path, string operationType)
        {
            var pathItem = this._doc["paths"][path];

            pathItem.Value<JToken>(operationType).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-dictionary", "get", "200")]
        [DataRow("/get-applicationjson-dictionary-idictionary", "get", "200")]
        [DataRow("/get-applicationjson-dictionary-ireadonlydictionary", "get", "200")]
        [DataRow("/get-applicationjson-dictionary-keyvaluepair", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-dictionary", "get", "200", "application/json")]
        [DataRow("/get-applicationjson-dictionary-idictionary", "get", "200", "application/json")]
        [DataRow("/get-applicationjson-dictionary-ireadonlydictionary", "get", "200", "application/json")]
        [DataRow("/get-applicationjson-dictionary-keyvaluepair", "get", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-dictionary", "get", "200", "application/json", "object")]
        [DataRow("/get-applicationjson-dictionary-idictionary", "get", "200", "application/json","object")]
        [DataRow("/get-applicationjson-dictionary-ireadonlydictionary", "get", "200", "application/json",  "object")]
        [DataRow("/get-applicationjson-dictionary-keyvaluepair", "get", "200", "application/json", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchemaType(string path, string operationType, string responseCode, string contentType, string itemType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];
            var schema = content[contentType]["schema"];

            var type = schema["type"];

            type.Value<string>().Should().Be(itemType);
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-dictionary", "get", "200", "application/json", "object", "string")]
        [DataRow("/get-applicationjson-dictionary-idictionary", "get", "200", "application/json", "object", "integer")]
        [DataRow("/get-applicationjson-dictionary-ireadonlydictionary", "get", "200", "application/json", "object", "number")]
        [DataRow("/get-applicationjson-dictionary-keyvaluepair", "get", "200", "application/json", "object", "boolean")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchemaPropertiesType(string path, string operationType, string responseCode, string contentType, string dataType, string itemType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];
            var schema = content[contentType]["schema"];

            var additionaltype = schema["additionalProperties"]["type"];

            additionaltype.Value<string>().Should().Be(itemType);
        }

    }
}