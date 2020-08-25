using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class DisplayAttributeTests
    {
        [TestMethod]
        public void Given_Null_Constructor_Should_Throw_Exception()
        {
            Action action = () => new DisplayAttribute(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Value_Property_Should_Return_Value()
        {
            var name = "Hello World";
            var attribute = new DisplayAttribute(name);

            attribute.Name.Should().BeEquivalentTo(name);
        }
    }
}
