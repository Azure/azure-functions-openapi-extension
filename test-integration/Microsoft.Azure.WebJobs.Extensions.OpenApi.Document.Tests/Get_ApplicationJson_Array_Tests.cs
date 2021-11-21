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
    public class Get_ApplicationJson_Array_Tests
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
        [DataRow("/get-applicationjson-string-array")]
        [DataRow("/get-applicationjson-int-array")]
        [DataRow("/get-applicationjson-bool-array")]
        [DataRow("/get-applicationjson-int-list")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-string-array", "get")]
        [DataRow("/get-applicationjson-int-array", "get")]
        [DataRow("/get-applicationjson-bool-array", "get")]
        [DataRow("/get-applicationjson-int-list", "get")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationType(string path, string operationType)
        {
            var pathItem = this._doc["paths"][path];

            pathItem.Value<JToken>(operationType).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-string-array", "get", "200")]
        [DataRow("/get-applicationjson-int-array", "get", "200")]
        [DataRow("/get-applicationjson-bool-array", "get", "200")]
        [DataRow("/get-applicationjson-int-list", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-string-array", "get", "200", "application/json")]
        [DataRow("/get-applicationjson-int-array", "get", "200", "application/json")]
        [DataRow("/get-applicationjson-bool-array", "get", "200", "application/json")]
        [DataRow("/get-applicationjson-int-list", "get", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-string-array", "get", "200", "application/json", "array")]
        [DataRow("/get-applicationjson-int-array", "get", "200", "application/json", "array")]
        [DataRow("/get-applicationjson-bool-array", "get", "200", "application/json", "array")]
        [DataRow("/get-applicationjson-int-list", "get", "200", "application/json", "array")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string dataType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-int-array", "get", "200", "application/json", "array", "integer", "int32")]
        [DataRow("/get-applicationjson-int-list", "get", "200", "application/json", "array", "integer", "int32")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeItemTypeFormat(string path, string operationType, string responseCode, string contentType, string dataType, string itemType, string itemFormat)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var items = content[contentType]["schema"]["items"];

            items.Value<string>("type").Should().Be(itemType);
            items.Value<string>("format").Should().Be(itemFormat);
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-string-array", "get", "200", "application/json", "array", "string")]
        [DataRow("/get-applicationjson-bool-array", "get", "200", "application/json", "array", "boolean")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeItemType(string path, string operationType, string responseCode, string contentType, string dataType, string itemType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var items = content[contentType]["schema"]["items"];

            items.Value<string>("type").Should().Be(itemType);
        }
    }
}
