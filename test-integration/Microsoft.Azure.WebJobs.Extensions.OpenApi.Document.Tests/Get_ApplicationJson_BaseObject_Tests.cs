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
        [DataRow("/get-applicationjson-baseobject", "get", "200", "application/json", "baseObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Not_Return_ReferenceSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("baseObjectModel", "baseObjectValue", true)]
        [DataRow("baseObjectModel", "nonObjectValue", false)]
        [DataRow("baseObjectModel", "subObjectValue", false)]
        [DataRow("baseSubObjectModel", "baseSubObjectValue", true)]
        public void Given_OpenApiDocument_And_BaseObject_Then_It_Should_Return_Expected_Type(string @ref, string propName, bool isBaseObject)
        {
            var schemas = this._doc["components"]["schemas"];

            var type = schemas?[@ref]?["properties"]?[propName]?["type"]?.Value<string>() ;

            if (isBaseObject)
            {
                type.Should().Be("object");
            }
            else
            {
                type.Should().NotBe("object");
            }
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_Null()
        {
            var @object = this._doc["components"]["schemas"]["object"];

            @object.Should().BeNull();
        }
    }
}
