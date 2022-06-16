using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Document.Tests.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Document.Tests.OpenApiInfo
{    
    [TestClass]
    [TestCategory(Constants.TestCategory)]
    public class OpenApiInfoTests
    {
        private static HttpClient http = new HttpClient();

        private string _json;
        private OpenApiDocument _doc;

        [TestInitialize]
        public async Task Init()
        {
            this._json = await http.GetStringAsync(Constants.OpenApiDocEndpoint).ConfigureAwait(false);            
            this._doc = JsonConvert.DeserializeObject<OpenApiDocument>(this._json, new OpenApiAnyConverter());        
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_OpenApiSpecVersion()
        {
            var doc = (dynamic)JsonConvert.DeserializeObject<object>(this._json);
            var specVersion = (string) doc.openapi;

            specVersion.Should().Be(OpenApiInfoConfigs.OpenApiSpecVersion);
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_Version()
        {
            this._doc.Info.Version.Should().Be(OpenApiInfoConfigs.DocVersion);
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_Title()
        {
            this._doc.Info.Title.Should().Be(OpenApiInfoConfigs.DocTitle);
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_Description()
        {
            this._doc.Info.Description.Should().Be(OpenApiInfoConfigs.DocDescription);
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_TermsOfService()
        {
            this._doc.Info.TermsOfService.AbsoluteUri.Should().Be(OpenApiInfoConfigs.TermsOfService);
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_ContactName()
        {
            var contact = this._doc.Info.Contact;

            contact.Name.Should().Be(OpenApiInfoConfigs.ContactName);
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_ContactEmail()
        {
            var contact = this._doc.Info.Contact;

            contact.Email.Should().Be(OpenApiInfoConfigs.ContactEmail);
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_ContactUrl()
        {
            var contact = this._doc.Info.Contact;

            contact.Url.AbsoluteUri.Should().Be(OpenApiInfoConfigs.ContactUrl);
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_LicenseName()
        {
            var license = this._doc.Info.License;

            license.Name.Should().Be(OpenApiInfoConfigs.LicenseName);
        }

        [TestMethod]
        public void Given_OpenApiDocument_Then_It_Should_Return_LicenseUrl()
        {
            var license = this._doc.Info.License;

            license.Url.AbsoluteUri.Should().Be(OpenApiInfoConfigs.LicenseUrl);
        }
    }
}
