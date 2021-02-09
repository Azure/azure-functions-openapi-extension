using System.Linq;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiOperationAttributeTests
    {
        [TestMethod]
        public void Given_No_Parameters_When_Instantiated_Then_It_Should_Return_Value()
        {
            var attribute = new OpenApiOperationAttribute();

            attribute.OperationId.Should().BeNullOrWhiteSpace();
            attribute.Tags.Should().NotBeNull();
            attribute.Tags.Should().BeEmpty();
            attribute.Summary.Should().BeNullOrWhiteSpace();
            attribute.Description.Should().BeNullOrWhiteSpace();
            attribute.Visibility.Should().Be(OpenApiVisibilityType.Undefined);
            attribute.Deprecated.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("lorem ipsum", "hello", "world")]
        public void Given_Parameters_When_Instantiated_Then_It_Should_Return_Value(string opId, params string[] tags)
        {
            var attribute = new OpenApiOperationAttribute(opId, tags);

            attribute.OperationId.Should().BeEquivalentTo(opId);
            attribute.Tags.First().Should().BeEquivalentTo(tags[0]);
            attribute.Tags.Last().Should().BeEquivalentTo(tags[1]);
            attribute.Summary.Should().BeNullOrWhiteSpace();
            attribute.Description.Should().BeNullOrWhiteSpace();
            attribute.Visibility.Should().Be(OpenApiVisibilityType.Undefined);
            attribute.Deprecated.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("Lorem Ipsum", "Hello World", OpenApiVisibilityType.Important, true)]
        public void Given_Properties_When_Provided_Then_It_Should_Return_Value(string summary, string description, OpenApiVisibilityType visibility, bool deprecated)
        {
            var attribute = new OpenApiOperationAttribute("lorem ipsum", "hello", "world")
            {
                Summary = summary,
                Description = description,
                Visibility = visibility,
                Deprecated = deprecated,
            };

            attribute.Summary.Should().Be(summary);
            attribute.Description.Should().Be(description);
            attribute.Visibility.Should().Be(visibility);
            attribute.Deprecated.Should().Be(deprecated);
        }
    }
}
