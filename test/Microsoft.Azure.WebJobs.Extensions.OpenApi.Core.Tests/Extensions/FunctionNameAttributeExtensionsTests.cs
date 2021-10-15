using System;
using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
{
    [TestClass]
    public class FunctionNameAttributeExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_GetFunctionName_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => new Mock<IDocumentHelper>().Object.GetFunctionNameAttribute(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("DoSomething", "FakeFunction")]
        public void Given_MemberInfo_When_ExistsCustomAttribute_Invoked_Then_It_Should_Return_Result(string methodName, string expected)
        {
            var method = typeof(FakeHttpTrigger).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

            var result = new Mock<IDocumentHelper>().Object.GetFunctionNameAttribute(method);

#if NET5_0
            result.Should().BeOfType<Functions.Worker.FunctionAttribute>();
#else
            result.Should().BeOfType<FunctionNameAttribute>();
#endif
            result.Name.Should().Be(expected);

        }
    }
}
