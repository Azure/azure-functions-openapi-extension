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
    public class Get_Query_TextPlain_DataType_Tests
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
        [DataRow("/get-query-textplain-datatype", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-query-textplain-datatype", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-query-textplain-datatype", "get", "200", "text/plain", "dataTypeClass")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }
        [DataTestMethod]
        [DataRow("dataTypeClass", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType)
        {
            var schemas = this._doc["components"]["schemas"];

            var schema = schemas[@ref];

            schema.Should().NotBeNull();
            schema.Value<string>("type").Should().Be(refType);
        }

        [DataTestMethod]
        [DataRow("dataTypeClass", "dateTimeValue1", "string", "date-time", false)]
        [DataRow("dataTypeClass", "dateTimeValue2", "string", "date", false)]
        [DataRow("dataTypeClass", "dateTimeValue3", "string", "time", false)]
        [DataRow("dataTypeClass", "nullableDateTimeValue1", "string", "date-time", true)]
        [DataRow("dataTypeClass", "nullableDateTimeValue2", "string", "date", true)]
        [DataRow("dataTypeClass", "nullableDateTimeValue3", "string", "time", true)]
        [DataRow("dataTypeClass", "dateTimeOffsetValue1", "string", "date-time", false)]
        [DataRow("dataTypeClass", "dateTimeOffsetValue2", "string", "date", false)]
        [DataRow("dataTypeClass", "dateTimeOffsetValue3", "string", "time", false)]
        [DataRow("dataTypeClass", "nullableDateTimeOffsetValue1", "string", "date-time", true)]
        [DataRow("dataTypeClass", "nullableDateTimeOffsetValue2", "string", "date", true)]
        [DataRow("dataTypeClass", "nullableDateTimeOffsetValue3", "string", "time", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string propertyName, string propertyType, string propertyFormat, bool propertyNullable)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value = properties[propertyName];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<string>("format").Should().Be(propertyFormat);
            value.Value<bool>("nullable").Should().Be(propertyNullable);
        }
    }
}
