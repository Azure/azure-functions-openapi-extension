using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiRequestExampleAttributeTests
    {
        [TestMethod]
        public void Given_Null_When_Instantiated_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiRequestExampleAttribute(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow(typeof(FakeExample))]
        public void Given_Parameter_When_Instantiated_It_Should_Return_Value(Type example)
        {
            var attribute = new OpenApiRequestExampleAttribute(example);

            attribute.Example.Should().Be(example);
        }
    }
}
