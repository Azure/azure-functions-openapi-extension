using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.CLI.Tests
{
    [TestClass]
    public class ProjectInfoTests
    {
        [TestMethod]
        public void Given_Null_Parameters_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            Action action = () => new ProjectInfo(null, null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new ProjectInfo("abc", null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new ProjectInfo("abc", "abc", null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
