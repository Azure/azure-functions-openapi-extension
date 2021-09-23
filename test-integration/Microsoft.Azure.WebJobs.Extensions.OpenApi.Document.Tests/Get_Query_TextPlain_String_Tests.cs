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
    public class Get_Query_TextPlain_String_Tests
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
        [DataRow("/get-query-textplain-string")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-query-textplain-string", "get")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationType(string path, string operationType)
        {
            var pathItem = this._doc["paths"][path];

            pathItem.Value<JToken>(operationType).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-query-textplain-string", "get", "name", "query", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationParameter(string path, string operationType, string name, string @in, bool required)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();

            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            parameter.Should().NotBeNull();
            parameter.Value<string>("in").Should().Be(@in);
            parameter.Value<bool>("required").Should().Be(required);
        }

        [DataTestMethod]
        [DataRow("/get-query-textplain-string", "get", "name", "string")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationParameterSchema(string path, string operationType, string name, string dataType)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var schema = parameter["schema"];

            schema.Value<string>("type").Should().Be(dataType);
        }

        [DataTestMethod]
        [DataRow("/get-query-textplain-string", "get", "name")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var example = parameter["examples"];

            example["intValue"].Value<int>("value").Should().Be(1);
            example["stringValue"].Value<string>("value").Should().Be("stringValue");
            example["doubleValue"].Value<double>("value").Should().Be(0.123);
            example["date-timeValue"].Value<DateTime>("value").Should().Be(DateTime.Parse("2021.01.01"));
            example["booleanValue"].Value<bool>("value").Should().Be(false);
        }

        [DataTestMethod]
        [DataRow("/get-query-textplain-string", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-query-textplain-string", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-query-textplain-string", "get", "200", "text/plain", "string")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string dataType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
        }
    }
}
