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
    public class Get_ApplicationJson_BaseObject_Tests
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
        [DataRow("/get-applicationjson-baseobject", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-baseobject", "get", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-baseobject", "get", "200", "application/json", "microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Not_Return_ReferenceSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseObjectModel", "baseObjectValue")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseSubObjectModel", "baseSubObjectValue")]
        public void Given_OpenApiDocument_And_BaseObject_Then_It_Should_Return_Expected_TypeOf_Object(string @ref, string propName)
        {
            var schemas = this._doc["components"]["schemas"];

            var type = schemas?[@ref]?["properties"]?[propName]?["type"]?.Value<string>() ;

            type.Should().Be("object");
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseObjectModel", "nonObjectValue")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseObjectModel", "subObjectValue")]
        public void Given_OpenApiDocument_And_BaseObject_Then_It_Should_Not_Return_Expected_TypeOf_Object(string @ref, string propName)
        {
            var schemas = this._doc["components"]["schemas"];

            var type = schemas?[@ref]?["properties"]?[propName]?["type"]?.Value<string>() ;

            type.Should().NotBe("object");
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseObjectModel", "baseObjectList", "array")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseObjectModel", "baseObjectDictionary", "object")]
        public void Given_OpenApiDocument_And_BaseObject_Then_It_Should_Return_Expected_Type(string @ref, string propName, string listType)
        {
            var schemas = this._doc["components"]["schemas"];

            var type = schemas?[@ref]?["properties"]?[propName]?["type"]?.Value<string>();

            type.Should().Be(listType);
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseObjectModel", "baseObjectList", "items", "object")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseObjectModel", "baseObjectDictionary", "additionalProperties", "object")]
        public void Given_OpenApiDocument_And_BaseObject_Then_It_Should_Return_Expected_SubType(string @ref, string propName, string attrName, string subType)
        {
            var schemas = this._doc["components"]["schemas"];

            var property = schemas?[@ref]?["properties"]?[propName];
            var attr = property[attrName];

            attr["type"].Value<string>().Should().Be(subType);
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseObjectModel", "baseObjectList", "items")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.BaseObjectModel", "baseObjectDictionary", "additionalProperties")]
        public void Given_OpenApiDocument_And_BaseObject_Then_It_Should_Return_Null_Title(string @ref, string propName, string attr)
        {
            var schemas = this._doc["components"]["schemas"];

            var title = schemas?[@ref]?["properties"]?[propName]?[attr]?["title"]?.Value<string>();

            title.Should().BeNull();
        }
    }
}
