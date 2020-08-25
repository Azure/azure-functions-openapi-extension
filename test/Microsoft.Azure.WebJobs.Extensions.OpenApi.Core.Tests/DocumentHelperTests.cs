using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
{
    [TestClass]
    public class DocumentHelperTests
    {
        [TestMethod]
        public void Given_Null_Constructor_Should_Throw_Exception()
        {
            Action action = () => new DocumentHelper(null, null);
            action.Should().Throw<ArgumentNullException>();

            var filter = new RouteConstraintFilter();

            action = () => new DocumentHelper(filter, null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
