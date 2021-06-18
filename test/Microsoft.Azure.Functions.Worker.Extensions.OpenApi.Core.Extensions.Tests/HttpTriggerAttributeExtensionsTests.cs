using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Core.Extensions.Tests
{
    [TestClass]
    public class MethodInfoExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_GetHttpTrigger_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => HttpTriggerAttributeExtensions.GetHttpTrigger(null);

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
