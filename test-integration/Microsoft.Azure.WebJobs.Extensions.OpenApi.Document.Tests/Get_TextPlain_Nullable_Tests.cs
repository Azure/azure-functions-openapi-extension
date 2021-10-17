using System;
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
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_DateTime(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableboolean", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_DateTime(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableboolean", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_DateTime(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableboolean", "get", "200", "text/plain", "boolean", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_DateTime(string path, string operationType, string responseCode, string contentType, string dataType, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint16")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableUInt16(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint16", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableUInt16(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint16", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableUInt16(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint16", "get", "200", "text/plain", "integer", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableUInt16(string path, string operationType, string responseCode, string contentType, string dataType, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint32")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableUInt32(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint32", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableUInt32(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint32", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableUInt32(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint32", "get", "200", "text/plain", "integer", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableUInt32(string path, string operationType, string responseCode, string contentType, string dataType, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }


        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint64")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableUInt64(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint64", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableUInt64(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint64", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableUInt64(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableuint64", "get", "200", "text/plain", "integer", "int64", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableUInt64(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }


        [DataTestMethod]
        [DataRow("/get-textplain-nullableint16")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableInt16(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint16", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableInt16(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint16", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableInt16(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint16", "get", "200", "text/plain", "integer", "int32", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableInt16(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint32")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableInt32(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint32", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableInt32(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint32", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableInt32(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint32", "get", "200", "text/plain", "integer", "int32", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableInt32(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint64")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableInt64(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint64", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableInt64(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint64", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableInt64(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullableint64", "get", "200", "text/plain", "integer", "int64", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableInt64(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullablesingle")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableSingle(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullablesingle", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableSingle(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullablesingle", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableSingle(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullablesingle", "get", "200", "text/plain", "number", "float", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableSingle(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledouble")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableDouble(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledouble", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableDouble(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledouble", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableDouble(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledouble", "get", "200", "text/plain", "number", "double", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableDouble(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledecimal")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableDecimal(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledecimal", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableDecimal(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledecimal", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableDecimal(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledecimal", "get", "200", "text/plain", "number", "double", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableDecimal(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledatetime")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableDateTime(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledatetime", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableDateTime(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledatetime", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableDateTime(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledatetime", "get", "200", "text/plain", "string", "date-time", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableDateTime(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledatetimeoffset")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path_NullableDateTimeOffset(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledatetimeoffset", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse_NullableDateTimeOffset(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledatetimeoffset", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType_NullableDateTimeOffset(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-textplain-nullabledatetimeoffset", "get", "200", "text/plain", "string", "date-time", "True")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema_NullableDateTimeOffset(string path, string operationType, string responseCode, string contentType, string dataType, string dataFormat, string isNullableType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
            schema.Value<string>("format").Should().Be(dataFormat);
            schema.Value<string>("nullable").Should().Be(isNullableType);
        }
    }
}
