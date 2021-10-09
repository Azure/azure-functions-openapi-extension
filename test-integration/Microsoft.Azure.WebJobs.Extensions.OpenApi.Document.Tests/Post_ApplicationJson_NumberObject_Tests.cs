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
    public class Post_ApplicationJson_Number_Tests
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
        [DataRow("/post-applicationjson-single", "post")]
        [DataRow("/post-applicationjson-double", "post")]
        [DataRow("/post-applicationjson-decimal", "post")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBody(string path, string operationType)
        {
            var requestBody = this._doc["paths"][path][operationType]["requestBody"];

            requestBody.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-single", "post", "text/plain")]
        [DataRow("/post-applicationjson-double", "post", "text/plain")]
        [DataRow("/post-applicationjson-decimal", "post", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentType(string path, string operationType, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-single", "post", "text/plain", "number", "float")]
        [DataRow("/post-applicationjson-double", "post", "text/plain", "number", "double")]
        [DataRow("/post-applicationjson-decimal", "post", "text/plain", "number", "double")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationRequestBodyContentTypeSchema(string path, string operationType, string contentType, string propertyType, string propertyFormat)
        {
            var content = this._doc["paths"][path][operationType]["requestBody"]["content"];

            var value = content[contentType]["schema"];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<string>("format").Should().Be(propertyFormat);
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-single", "post", "200")]
        [DataRow("/post-applicationjson-double", "post", "200")]
        [DataRow("/post-applicationjson-decimal", "post", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-single", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-double", "post", "200", "application/json")]
        [DataRow("/post-applicationjson-decimal", "post", "200", "application/json")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/post-applicationjson-single", "post", "200", "application/json", "numberObjectModel")]
        [DataRow("/post-applicationjson-double", "post", "200", "application/json", "numberObjectModel")]
        [DataRow("/post-applicationjson-decimal", "post", "200", "application/json", "numberObjectModel")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string reference)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var @ref = content[contentType]["schema"]["$ref"];

            @ref.Value<string>().Should().Be($"#/components/schemas/{reference}");
        }

        [DataTestMethod]
        [DataRow("numberObjectModel", "object")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchema(string @ref, string refType)
        {
            var schemas = this._doc["components"]["schemas"];

            var schema = schemas[@ref];

            schema.Should().NotBeNull();
            schema.Value<string>("type").Should().Be(refType);
        }

        [DataTestMethod]
        [DataRow("numberObjectModel", "singleValue", "number", "float")]
        [DataRow("numberObjectModel", "doubleValue", "number", "double")]
        [DataRow("numberObjectModel", "decimalValue", "number", "double")]
        public void Given_OpenApiDocument_Then_It_Should_Return_ComponentSchemaProperty(string @ref, string propertyName, string propertyType, string propertyFormat)
        {
            var properties = this._doc["components"]["schemas"][@ref]["properties"];

            var value = properties[propertyName];

            value.Should().NotBeNull();
            value.Value<string>("type").Should().Be(propertyType);
            value.Value<string>("format").Should().Be(propertyFormat);
        }
    }
}
