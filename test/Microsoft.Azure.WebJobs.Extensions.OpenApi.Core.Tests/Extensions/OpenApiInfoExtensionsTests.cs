using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using FluentAssertions;

using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class OpenApiInfoExtensionsTests
    {
        [TestMethod]
        public void Given_Null_Parameter_When_IsValid_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => OpenApiInfoExtensions.IsValid(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Parameter_When_IsValid_Invoked_Then_It_Should_Return_True()
        {
            var info = new OpenApiInfo() { Version = "lorem ipsum", Title = "hello world" };

            var result = OpenApiInfoExtensions.IsValid(info);

            result.Should().BeTrue();
        }

        [TestMethod]
        public void Given_No_Title_When_IsValid_Invoked_Then_It_Should_Return_False()
        {
            var info = new OpenApiInfo() { Version = "lorem ipsum", Title = null };

            var result = OpenApiInfoExtensions.IsValid(info);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void Given_No_Version_When_IsValid_Invoked_Then_It_Should_Return_False()
        {
            var info = new OpenApiInfo() { Version = null, Title = "hello world" };

            var result = OpenApiInfoExtensions.IsValid(info);

            result.Should().BeFalse();
        }
    }
}
