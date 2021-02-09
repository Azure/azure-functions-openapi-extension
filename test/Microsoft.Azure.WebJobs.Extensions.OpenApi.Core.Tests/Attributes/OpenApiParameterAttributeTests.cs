using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiParameterAttributeTests
    {
        [TestMethod]
        public void Given_Null_When_Instantiated_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiParameterAttribute(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("Hello World")]
        public void Given_Parameter_When_Instantiated_It_Should_Return_Value(string name)
        {
            var attribute = new OpenApiParameterAttribute(name);

            attribute.Name.Should().BeEquivalentTo(name);
            attribute.Summary.Should().BeNullOrWhiteSpace();
            attribute.Description.Should().BeNullOrWhiteSpace();
            attribute.Type.Should().Be<string>();
            attribute.In.Should().Be(ParameterLocation.Path);
            attribute.CollectionDelimiter.Should().Be(OpenApiParameterCollectionDelimiterType.Comma);
            attribute.Explode.Should().BeFalse();
            attribute.Required.Should().BeFalse();
            attribute.Visibility.Should().Be(OpenApiVisibilityType.Undefined);
            attribute.Deprecated.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("Hello World", "Lorem Ipsum", typeof(FakeModel), ParameterLocation.Header, OpenApiParameterCollectionDelimiterType.Pipe, true, true, OpenApiVisibilityType.Important, true)]
        public void Given_Properties_When_Instantiated_It_Should_Return_Value(
            string summary, string description, Type type, ParameterLocation @in,
            OpenApiParameterCollectionDelimiterType delimiter, bool explode, bool required, OpenApiVisibilityType visibility, bool deprecated)
        {
            var attribute = new OpenApiParameterAttribute("Name")
            {
                Summary = summary,
                Description = description,
                Type = type,
                In = @in,
                CollectionDelimiter = delimiter,
                Explode = explode,
                Required = required,
                Visibility = visibility,
                Deprecated = deprecated,
            };

            attribute.Summary.Should().Be(summary);
            attribute.Description.Should().Be(description);
            attribute.Type.Should().Be(type);
            attribute.In.Should().Be(@in);
            attribute.CollectionDelimiter.Should().Be(delimiter);
            attribute.Explode.Should().Be(explode);
            attribute.Required.Should().Be(required);
            attribute.Visibility.Should().Be(visibility);
            attribute.Deprecated.Should().Be(deprecated);
        }
    }
}
