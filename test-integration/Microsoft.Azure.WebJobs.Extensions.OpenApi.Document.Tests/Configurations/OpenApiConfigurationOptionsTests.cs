using System;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Document.Tests.OpenApiInfo
{
    [TestClass]
    [TestCategory(Constants.TestCategory)]
    public class OpenApiConfigurationOptionsTests
    {
        private static HttpClient http = new HttpClient();

        private string _json;
        private OpenApiDocument _doc;

        [TestInitialize]
        public async Task Init()
        {
            this._json = await http.GetStringAsync(Constants.OpenApiDocEndpoint).ConfigureAwait(false);
            this._doc = JsonConvert.DeserializeObject<OpenApiDocument>(this._json);
        }

        [TestMethod]
        public void Given_OpenApiDocument_When_ForceHttps_Given_Then_It_Should_Return_Https()
        {
            var servers = this._doc.Servers;

            servers[0].Url.Should().Be("https://localhost:7071/api");
        }
    }
}
