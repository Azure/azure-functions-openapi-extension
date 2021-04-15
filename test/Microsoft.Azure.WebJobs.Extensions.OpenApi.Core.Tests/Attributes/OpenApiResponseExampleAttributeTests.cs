using System;
using System.Net;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiResponseExampleAttributeTests
    {
        [TestMethod]
        public void Given_Null_When_Instantiated_It_Should_Throw_Exception()
        {
            Action action = () => new OpenApiResponseExampleAttribute(HttpStatusCode.OK, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow(typeof(FakeExample))]
        public void Given_Parameter_When_Instantiated_It_Should_Return_Value(Type example)
        {
            var attribute = new OpenApiResponseExampleAttribute(HttpStatusCode.OK, example);

            attribute.Example.Should().Be(example);
        }
    }
}
