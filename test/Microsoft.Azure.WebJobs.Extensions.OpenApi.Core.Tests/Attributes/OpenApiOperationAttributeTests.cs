using System.Linq;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiOperationAttributeTests
    {
        [TestMethod]
        public void Given_Null_Properties_Should_Return_Value()
        {
            var visibility = OpenApiVisibilityType.Undefined;
            var attribute = new OpenApiOperationAttribute();

            attribute.OperationId.Should().BeNullOrWhiteSpace();
            attribute.Tags.Should().NotBeNull();
            attribute.Tags.Should().BeEmpty();
            attribute.Summary.Should().BeNullOrWhiteSpace();
            attribute.Description.Should().BeNullOrWhiteSpace();
            attribute.Visibility.Should().Be(visibility);
        }

        [TestMethod]
        public void Given_Value_Properties_Should_Return_Value()
        {
            var opId = "lorem ipsum";
            var tag1 = "hello";
            var tag2 = "world";
            var visibility = OpenApiVisibilityType.Undefined;
            var attribute = new OpenApiOperationAttribute(opId, tag1, tag2);

            attribute.OperationId.Should().BeEquivalentTo(opId);
            attribute.Tags.First().Should().BeEquivalentTo(tag1);
            attribute.Tags.Last().Should().BeEquivalentTo(tag2);
            attribute.Summary.Should().BeNullOrWhiteSpace();
            attribute.Description.Should().BeNullOrWhiteSpace();
            attribute.Visibility.Should().Be(visibility);
        }
    }
}
