using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Resolvers
{
    [TestClass]
    public class OpenApiInfoResolverTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(OpenApiInfoResolver)
                .Should().HaveMethod("Resolve", new[] { typeof(Assembly) })
                .Which.Should().Return<OpenApiInfo>();
        }
    }
}
