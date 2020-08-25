using FluentAssertions;

using Microsoft.OpenApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests
{
    [TestClass]
    public class OpenApiHttpTriggerContextTests
    {
        [DataTestMethod]
        [DataRow("v2", OpenApiSpecVersion.OpenApi2_0)]
        [DataRow("v3", OpenApiSpecVersion.OpenApi3_0)]
        public void Given_Type_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(string version, OpenApiSpecVersion expected)
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetOpenApiSpecVersion(version);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("yaml", OpenApiFormat.Yaml)]
        [DataRow("json", OpenApiFormat.Json)]
        public void Given_Type_When_GetOpenApiSpecVersion_Invoked_Then_It_Should_Return_Result(string format, OpenApiFormat expected)
        {
            var context = new OpenApiHttpTriggerContext();

            var result = context.GetOpenApiFormat(format);

            result.Should().Be(expected);
        }
    }
}
