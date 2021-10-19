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
    public class Get_TextPlain_Integer_Tests
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
        [DataRow("/get-textplain-int16")]
        [DataRow("/get-textplain-int32")]
        [DataRow("/get-textplain-int64")]
        [DataRow("/get-textplain-uint16")]
        [DataRow("/get-textplain-uint32")]
        [DataRow("/get-textplain-uint64")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-int16", "get", "int16value", "integer", "int32","path")]
        [DataRow("/get-textplain-int32", "get", "int32value", "integer", "int32", "path")]
        [DataRow("/get-textplain-int64", "get", "int64value", "integer", "int64", "path")]
        [DataRow("/get-textplain-uint16", "get", "uint16value", "integer", "int32", "path")]
        [DataRow("/get-textplain-uint32", "get", "uint32value", "integer", "int32", "path")]
        [DataRow("/get-textplain-uint64", "get", "uint64value", "integer", "int64","path")]
        [DataRow("/get-textplain-int16", "get", "int16value", "integer", "int32","query")]
        [DataRow("/get-textplain-int32", "get", "int32value", "integer", "int32", "query")]
        [DataRow("/get-textplain-int64", "get", "int64value", "integer", "int64", "query")]
        [DataRow("/get-textplain-uint16", "get", "uint16value", "integer", "int32", "query")]
        [DataRow("/get-textplain-uint32", "get", "uint32value", "integer", "int32", "query")]
        [DataRow("/get-textplain-uint64", "get", "uint64value", "integer", "int64","query")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationParameterSchema(string path, string operationType, string name, string dataType, string dataFormat, string @in)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name && p["in"].Value<string>() == @in);

            var schema = parameter["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-int16", "get", "200")]
        [DataRow("/get-textplain-int32", "get", "200")]
        [DataRow("/get-textplain-int64", "get", "200")]
        [DataRow("/get-textplain-uint16", "get", "200")]
        [DataRow("/get-textplain-uint32", "get", "200")]
        [DataRow("/get-textplain-uint64", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-int16", "get", "200", "text/plain")]
        [DataRow("/get-textplain-int32", "get", "200", "text/plain")]
        [DataRow("/get-textplain-int64", "get", "200", "text/plain")]
        [DataRow("/get-textplain-uint16", "get", "200", "text/plain")]
        [DataRow("/get-textplain-uint32", "get", "200", "text/plain")]
        [DataRow("/get-textplain-uint64", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }


        [DataTestMethod]
        [DataRow("/get-textplain-int16", "get", "200", "text/plain", "integer", "int32")]
        [DataRow("/get-textplain-int32", "get", "200", "text/plain", "integer", "int32")]
        [DataRow("/get-textplain-int64", "get", "200", "text/plain", "integer", "int64")]
        [DataRow("/get-textplain-uint16", "get", "200", "text/plain", "integer", "int32")]
        [DataRow("/get-textplain-uint32", "get", "200", "text/plain", "integer", "int32")]
        [DataRow("/get-textplain-uint64", "get", "200", "text/plain", "integer", "int64")]

        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
        }


        [DataTestMethod]
        [DataRow("/get-textplain-int16", "get", "int16value", "path", true)]
        [DataRow("/get-textplain-int32", "get", "int32value", "path", true)]
        [DataRow("/get-textplain-int64", "get", "int64value", "path", true)]
        [DataRow("/get-textplain-uint16", "get", "uint16value", "path", true)]
        [DataRow("/get-textplain-uint32", "get", "uint32value", "path", true)]
        [DataRow("/get-textplain-uint64", "get", "uint64value", "path", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationParameter(string path, string operationType, string name, string @in, bool required)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();

            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name && p["in"].Value<string>() == @in);

            parameter.Should().NotBeNull();
            parameter.Value<string>("in").Should().Be(@in);
            parameter.Value<bool>("required").Should().Be(required);
        }
    }
}
