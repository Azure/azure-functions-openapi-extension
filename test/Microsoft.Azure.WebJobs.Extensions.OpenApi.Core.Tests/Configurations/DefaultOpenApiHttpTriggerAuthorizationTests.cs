using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Configurations
{
    [TestClass]
    public class DefaultOpenApiHttpTriggerAuthorizationTests
    {
        [TestMethod]
        public async Task Given_Type_When_Instantiated_Then_Properties_Should_Return_Null()
        {
            var req = Substitute.For<IHttpRequestDataObject>();
            var auth = new DefaultOpenApiHttpTriggerAuthorization();

            var result = await auth.AuthorizeAsync(req).ConfigureAwait(false);

            result.Should().BeNull();
        }
    }
}
