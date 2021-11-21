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
    public class Get_Deprecated_Tests
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
        [DataRow("/get-textplain-deprecated-true")]
        [DataRow("/get-textplain-deprecated-false")]
        [DataRow("/get-textplain-deprecated-null")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-deprecated-true", "get")]
        [DataRow("/get-textplain-deprecated-false", "get")]
        [DataRow("/get-textplain-deprecated-null", "get")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType)
        {
            var pathItem = this._doc["paths"][path];

            pathItem.Value<JToken>(operationType).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-deprecated-true", "get", "200")]
        [DataRow("/get-textplain-deprecated-false", "get", "200")]
        [DataRow("/get-textplain-deprecated-null", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-deprecated-true", "get", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_Deprecated(string path, string operationType, bool deprecated)
        {
            var isDeprecated = this._doc["paths"][path][operationType]["deprecated"];

            isDeprecated.Value<bool>().Should().Be(deprecated);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-deprecated-false", "get")]
        [DataRow("/get-textplain-deprecated-null", "get")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Deprecated_False(string path, string operationType)
        {
            var isDeprecated = this._doc["paths"][path][operationType]["deprecated"];

            isDeprecated.Should().BeNull();
        }
        
        [DataTestMethod]
        [DataRow("/get-textplain-deprecated-true", "get", "200", "text/plain")]
        [DataRow("/get-textplain-deprecated-false", "get", "200", "text/plain")]
        [DataRow("/get-textplain-deprecated-null", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-deprecated-true", "get", "200", "text/plain", "string")]
        [DataRow("/get-textplain-deprecated-false", "get", "200", "text/plain", "string")]
        [DataRow("/get-textplain-deprecated-null", "get", "200", "text/plain", "string")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string dataType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
        }

        
    }
}
