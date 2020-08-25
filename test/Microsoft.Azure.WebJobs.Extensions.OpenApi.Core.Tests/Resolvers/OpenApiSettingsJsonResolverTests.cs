using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Resolvers
{
    [TestClass]
    public class OpenApiSettingsJsonResolverTests
    {
        [TestMethod]
        public void Given_Parameters_When_Resolve_Invoked_Then_It_Should_Return_Result()
        {
            var section = new Mock<IConfigurationSection>();
            section.SetupGet(p => p.Value).Returns(string.Empty);

            var env = new Mock<IConfiguration>();
            env.Setup(p => p.GetSection(It.IsAny<string>())).Returns(section.Object);

            var result = OpenApiSettingsJsonResolver.Resolve(env.Object);

            result.Should().NotBeNull()
                           .And.BeAssignableTo<IConfiguration>();
        }
    }
}
