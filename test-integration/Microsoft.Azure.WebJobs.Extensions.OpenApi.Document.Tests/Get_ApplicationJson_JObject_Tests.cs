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
    public class Get_ApplicationJson_JObject_Tests
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
        [DataRow("/get-applicationjson-jobject", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-jobject", "get", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-applicationjson-jobject", "get", "200", "application/json", "microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.JObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.JObjectModel", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType)
        {
            var schemas = this._doc["components"]["schemas"];

            var schema = schemas[@ref];

            schema.Should().NotBeNull();
            schema.Value<string>("type").Should().Be(refType);
        }

        [DataTestMethod]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.JObjectModel", "jObjectValue", "object")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.JObjectModel", "jTokenValue", "object")]
        [DataRow("microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models.JObjectModel", "jArrayValue", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string propertyName, string propertyType)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value = properties[propertyName];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
        }

        [DataTestMethod]
        [DataRow("jObject")]
        [DataRow("jToken")]
        [DataRow("jArray")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Null(string @ref)
        {
            var @object = this._doc["components"]["schemas"][@ref];

            @object.Should().BeNull();
        }
    }
}
