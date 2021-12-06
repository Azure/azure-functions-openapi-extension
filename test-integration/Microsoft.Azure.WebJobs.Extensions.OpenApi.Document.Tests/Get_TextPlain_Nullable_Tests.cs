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
    public class Get_TextPlain_Nullable_Tests
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
        [DataRow("/get-textplain-nullableboolean")]
        [DataRow("/get-textplain-nullableuint16")]
        [DataRow("/get-textplain-nullableuint32")]
        [DataRow("/get-textplain-nullableuint64")]
        [DataRow("/get-textplain-nullableint16")]
        [DataRow("/get-textplain-nullableint32")]
        [DataRow("/get-textplain-nullableint64")]
        [DataRow("/get-textplain-nullablesingle")]
        [DataRow("/get-textplain-nullabledouble")]
        [DataRow("/get-textplain-nullabledecimal")]
        [DataRow("/get-textplain-nullabledatetime")]
        [DataRow("/get-textplain-nullabledatetimeoffset")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableboolean", "get", "200")]
        [DataRow("/get-textplain-nullableuint16", "get", "200")]
        [DataRow("/get-textplain-nullableuint32", "get", "200")]
        [DataRow("/get-textplain-nullableuint64", "get", "200")]
        [DataRow("/get-textplain-nullableint16", "get", "200")]
        [DataRow("/get-textplain-nullableint32", "get", "200")]
        [DataRow("/get-textplain-nullableint64", "get", "200")]
        [DataRow("/get-textplain-nullablesingle", "get", "200")]
        [DataRow("/get-textplain-nullabledouble", "get", "200")]
        [DataRow("/get-textplain-nullabledecimal", "get", "200")]
        [DataRow("/get-textplain-nullabledatetime", "get", "200")]
        [DataRow("/get-textplain-nullabledatetimeoffset", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableboolean", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullableuint16", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullableuint32", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullableuint64", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullableint16", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullableint32", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullableint64", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullablesingle", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullabledouble", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullabledecimal", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullabledatetime", "get", "200", "text/plain")]
        [DataRow("/get-textplain-nullabledatetimeoffset", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableboolean", "get", "200", "text/plain", "boolean", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_Boolean(string path, string operationType, string responseCode, string contentType, string dataType, bool nullable)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<bool>("nullable").Should().Be(nullable);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint16", "get", "200", "text/plain", "integer", "int32", true)]
        [DataRow("/get-textplain-nullableuint32", "get", "200", "text/plain", "integer", "int32", true)]
        [DataRow("/get-textplain-nullableuint64", "get", "200", "text/plain", "integer", "int64", true)]
        [DataRow("/get-textplain-nullableint16", "get", "200", "text/plain", "integer", "int32", true)]
        [DataRow("/get-textplain-nullableint32", "get", "200", "text/plain", "integer", "int32", true)]
        [DataRow("/get-textplain-nullableint64", "get", "200", "text/plain", "integer", "int64", true)]
        [DataRow("/get-textplain-nullablesingle", "get", "200", "text/plain", "number", "float", true)]
        [DataRow("/get-textplain-nullabledouble", "get", "200", "text/plain", "number", "double", true)]
        [DataRow("/get-textplain-nullabledecimal", "get", "200", "text/plain", "number", "double", true)]
        [DataRow("/get-textplain-nullabledatetime", "get", "200", "text/plain", "string", "date-time", true)]
        [DataRow("/get-textplain-nullabledatetimeoffset", "get", "200", "text/plain", "string", "date-time", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat, bool nullable)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
            schema.Value<bool>("nullable").Should().Be(nullable);
        }
    }
}
