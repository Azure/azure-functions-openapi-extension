using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiPropertyDescriptionAttributeTests
    {
        [DataTestMethod]
        [DataRow("hello world")]
        public void Given_Value_Property_Should_Return_Value(string description)
        {
            var attribute = new OpenApiPropertyDescriptionAttribute(description);

            attribute.Description.Should().Be(description);
        }
    }
}
