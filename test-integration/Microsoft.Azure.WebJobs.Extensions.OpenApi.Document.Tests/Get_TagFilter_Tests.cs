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
    public class Get_Query_TagFilter_Tests
    {
        private static HttpClient http = new HttpClient();

        private JObject _doc;

        [TestInitialize]
        public async Task Init()
        {
            var json = await http.GetStringAsync(Constants.OpenApiDocEndpoint + "?tag=tagFilter").ConfigureAwait(false);
            this._doc = JsonConvert.DeserializeObject<JObject>(json);
        }

        [DataTestMethod]
        [DataRow("/get-textplain-string")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Null(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().BeNull();
        }

        [DataTestMethod]
        [DataRow("/get-tagfilter")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Path(string path)
        {
            var paths = this._doc["paths"];

            paths.Value<JToken>(path).Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow("/get-tagfilter", "get", "tagFilter")]
        public void Given_OpenApiDocument_Then_It_Should_Return_Tag(string path, string operationType, string tag)
        {
            var tags = this._doc["paths"][path][operationType]["tags"].Children();

            var tagItem = tags.SingleOrDefault(p => p.Value<string>() == tag);

            tagItem.Should().NotBeNull();
        }
    }
}
