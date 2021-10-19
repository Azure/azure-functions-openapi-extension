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
    public class Get_TextPlain_Guid_Tests
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
        [DataRow("/get-textplain-guid", "get", "guid_path", "path", true)]
        [DataRow("/get-textplain-guid", "get", "guid_query", "query", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationParameter(string path, string operationType, string name, string @in, bool required)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"];
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            parameter.Should().NotBeNull();
            parameter.Value<string>("in").Should().Be(@in);
            parameter.Value<bool>("required").Should().Be(required);
        }

        [DataRow("/get-textplain-guid", "get", "guid_path", "string", "uuid")]
        [DataRow("/get-textplain-guid", "get", "guid_query", "string", "uuid")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationParameterSchema(string path, string operationType, string name, string dataType, string dataFormat)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var schema = parameter["schema"];
            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-guid", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-guid", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-guid", "get", "200", "text/plain", "string", "uuid")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];
            
            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
        }
    }
}
