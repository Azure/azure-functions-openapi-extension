using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

using FluentAssertions;

using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiParameterAttributeTests
    {
        [TestMethod]
        public void Given_Null_Constructor_Should_Throw_Exception()
        {
            Action action = () => new OpenApiParameterAttribute(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Value_Property_Should_Return_Value()
        {
            var name = "Hello World";
            var attribute = new OpenApiParameterAttribute(name);

            attribute.Name.Should().BeEquivalentTo(name);
            attribute.Summary.Should().BeNullOrWhiteSpace();
            attribute.Description.Should().BeNullOrWhiteSpace();
            attribute.Type.Should().Be<string>();
            attribute.In.Should().Be(ParameterLocation.Path);
            attribute.CollectionDelimiter.Should().Be(OpenApiParameterCollectionDelimiterType.Comma);
            attribute.Explode.Should().Be(false);
            attribute.Required.Should().Be(false);
            attribute.Visibility.Should().Be(OpenApiVisibilityType.Undefined);
        }
    }
}
