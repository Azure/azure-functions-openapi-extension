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
    public class Get_Path_ParameterExamples_Tests
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
        [DataRow("/get-path-parameter-examples")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationType(string path, string operationType)
        {
            var pathItem = this._doc["paths"][path];

            pathItem.Value<JToken>(operationType).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "stringParameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "int16Parameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "int32Parameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "int64Parameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "uint16Parameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "uint32Parameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "uint64Parameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "singleParameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "doubleParameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "dateTimeParameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "dateTimeOffsetParameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "booleanParameter", "path", true)]
        [DataRow("/get-path-parameter-examples", "get", "guidParameter", "path", true)]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationParameter(string path, string operationType, string name, string @in, bool required)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();

            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            parameter.Should().NotBeNull();
            parameter.Value<string>("in").Should().Be(@in);
            parameter.Value<bool>("required").Should().Be(required);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "stringParameter", "string")]
        [DataRow("/get-path-parameter-examples", "get", "int16Parameter", "integer")]
        [DataRow("/get-path-parameter-examples", "get", "int32Parameter", "integer")]
        [DataRow("/get-path-parameter-examples", "get", "int64Parameter", "integer")]
        [DataRow("/get-path-parameter-examples", "get", "uint16Parameter", "integer")]
        [DataRow("/get-path-parameter-examples", "get", "uint32Parameter", "integer")]
        [DataRow("/get-path-parameter-examples", "get", "uint64Parameter", "integer")]
        [DataRow("/get-path-parameter-examples", "get", "singleParameter", "number")]
        [DataRow("/get-path-parameter-examples", "get", "doubleParameter", "number")]
        [DataRow("/get-path-parameter-examples", "get", "dateTimeParameter", "string")]
        [DataRow("/get-path-parameter-examples", "get", "dateTimeOffsetParameter", "string")]
        [DataRow("/get-path-parameter-examples", "get", "booleanParameter", "boolean")]
        [DataRow("/get-path-parameter-examples", "get", "guidParameter", "string")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationParameterSchema(string path, string operationType, string name, string dataType)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var schema = parameter["schema"];

            schema.Value<string>("type").Should().Be(dataType);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "stringParameter", "stringValue1", "Lorem", "stringValue2", "")]
        public void Given_OpenApiDocument_StringType_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, string exampleValue1, string exampleName2, string exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<string>().Should().Be(exampleValue1);
            examples[exampleName2]["value"].Value<string>().Should().Be(exampleValue2);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "int16Parameter", "int16Value1", (short)1, "int16Value2", (short)0)]
        public void Given_OpenApiDocument_Int16Type_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, short exampleValue1, string exampleName2, short exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<short>().Should().Be(exampleValue1);
            examples[exampleName2]["value"].Value<short>().Should().Be(exampleValue2);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "int32Parameter", "int32Value1", 1, "int32Value2", 0)]
        public void Given_OpenApiDocument_Int32Type_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, int exampleValue1, string exampleName2, int exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<int>().Should().Be(exampleValue1);
            examples[exampleName2]["value"].Value<int>().Should().Be(exampleValue2);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "int64Parameter", "int64Value1", (long)1, "int64Value2", (long)0)]
        public void Given_OpenApiDocument_Int64Type_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, long exampleValue1, string exampleName2, long exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<long>().Should().Be(exampleValue1);
            examples[exampleName2]["value"].Value<long>().Should().Be(exampleValue2);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "uint16Parameter", "uint16Value1", (ushort)1, "uint16Value2", (ushort)0)]
        public void Given_OpenApiDocument_Uint16Type_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, ushort exampleValue1, string exampleName2, ushort exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<ushort>().Should().Be(exampleValue1);
            examples[exampleName2]["value"].Value<ushort>().Should().Be(exampleValue2);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "uint32Parameter", "uint32Value1", (uint)1, "uint32Value2", (uint)0)]
        public void Given_OpenApiDocument_Uint32Type_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, uint exampleValue1, string exampleName2, uint exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<uint>().Should().Be(exampleValue1);
            examples[exampleName2]["value"].Value<uint>().Should().Be(exampleValue2);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "uint64Parameter", "uint64Value1", (ulong)1, "uint64Value2", (ulong)0)]
        public void Given_OpenApiDocument_Uint64Type_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, ulong exampleValue1, string exampleName2, ulong exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<ulong>().Should().Be(exampleValue1);
            examples[exampleName2]["value"].Value<ulong>().Should().Be(exampleValue2);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "singleParameter", "singleValue1", (float)1.1, "singleValue2", (float)0.0)]
        public void Given_OpenApiDocument_SingleType_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, float exampleValue1, string exampleName2, float exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<float>().Should().Be(exampleValue1);
            examples[exampleName2]["value"].Value<float>().Should().Be(exampleValue2);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "doubleParameter", "doubleValue1", (double)1.1, "doubleValue2", (double)0.0)]
        public void Given_OpenApiDocument_DoubleType_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, double exampleValue1, string exampleName2, double exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<double>().Should().Be(exampleValue1);
            examples[exampleName2]["value"].Value<double>().Should().Be(exampleValue2);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "dateTimeParameter", "dateTimeValue1", "2021-01-01", "dateTimeValue2", "2021-01-01T12:34:56Z")]
        public void Given_OpenApiDocument_DateTimeType_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, string exampleValue1, string exampleName2, string exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<DateTime>().Should().Be(DateTime.Parse(exampleValue1));
            examples[exampleName2]["value"].Value<DateTime>().Should().Be(DateTime.Parse(exampleValue2));
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "dateTimeOffsetParameter", "dateTimeOffsetValue1", "05/01/2008", "dateTimeOffsetValue2", "11:36 PM")]
        [DataRow("/get-path-parameter-examples", "get", "dateTimeOffsetParameter", "dateTimeOffsetValue3", "05/01/2008 +1:00", "dateTimeOffsetValue4", "Thu May 01, 2008")]
        public void Given_OpenApiDocument_DateTimeOffsetType_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, string exampleValue1, string exampleName2, string exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<DateTime>().Should().Be(DateTime.Parse(exampleValue1));
            examples[exampleName2]["value"].Value<DateTime>().Should().Be(DateTime.Parse(exampleValue2));
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "booleanParameter", "booleanValue1", true, "booleanValue2", false)]
        public void Given_OpenApiDocument_BooleanType_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName1, bool exampleValue1, string exampleName2, bool exampleValue2)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName1]["value"].Value<bool>().Should().Be(exampleValue1);
            examples[exampleName2]["value"].Value<bool>().Should().Be(exampleValue2);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "guidParameter", "guidValue1", "74be27de-1e4e-49d9-b579-fe0b331d3642")]
        public void Given_OpenApiDocument_GuidType_Then_It_Should_Return_OperationParameterExamples(string path, string operationType, string name, string exampleName, string exampleValue)
        {
            var parameters = this._doc["paths"][path][operationType]["parameters"].Children();
            var parameter = parameters.SingleOrDefault(p => p["name"].Value<string>() == name);

            var examples = parameter["examples"];

            examples[exampleName]["value"].Value<string>().Should().Be(exampleValue);
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "200")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponse(string path, string operationType, string responseCode)
        {
            var responses = this._doc["paths"][path][operationType]["responses"];

            responses[responseCode].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "200", "text/plain")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentType(string path, string operationType, string responseCode, string contentType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            content[contentType].Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-path-parameter-examples", "get", "200", "text/plain", "string")]
        public void Given_OpenApiDocument_Then_It_Should_Return_OperationResponseContentTypeSchema(string path, string operationType, string responseCode, string contentType, string dataType)
        {
            var content = this._doc["paths"][path][operationType]["responses"][responseCode]["content"];

            var schema = content[contentType]["schema"];

            schema.Value<string>("type").Should().Be(dataType);
        }
    }
}
