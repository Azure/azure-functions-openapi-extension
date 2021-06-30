using System;
using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions.Tests
{
    [TestClass]
    public class FunctionNameAttributeExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_GetFunctionName_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => FunctionNameAttributeExtensions.GetFunctionName(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("DoSomething", "FakeFunction")]
        public void Given_MemberInfo_When_ExistsCustomAttribute_Invoked_Then_It_Should_Return_Result(string methodName, string expected)
        {
            var method = typeof(FakeHttpTrigger).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

            var result = FunctionNameAttributeExtensions.GetFunctionName(method);

            result.Should().BeOfType<FunctionNameAttribute>();
            result.Name.Should().Be(expected);
        }
    }
}
