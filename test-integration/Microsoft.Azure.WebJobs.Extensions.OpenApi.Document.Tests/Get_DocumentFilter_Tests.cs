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
    public class Get_DocumentFilter_Tests
    {
        private static HttpClient http = new HttpClient();

        private JObject _doc;

        [TestInitialize]
        public async Task Init()
        {
            var json = await http.GetStringAsync(Constants.OpenApiDocEndpoint).ConfigureAwait(false);
            this._doc = JsonConvert.DeserializeObject<JObject>(json);
        }

        [TestMethod]
        public void Given_Rewritten_OpenApiDocument_Then_It_Should_Have_Modified_Description()
        {
            var modifiedDescription = this._doc["paths"]["/get-documentfilter"]["get"]["responses"]["200"].Value<string>("description");

            modifiedDescription.Should().Be("The OK response rewritten");
        }
    }
}
