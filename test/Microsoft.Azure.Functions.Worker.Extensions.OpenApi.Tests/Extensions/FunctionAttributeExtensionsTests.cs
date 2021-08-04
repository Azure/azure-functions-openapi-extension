using System;
using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Extensions
{
    [TestClass]
    public class FunctionAttributeExtensionsTests
    {
        [TestMethod]
        public void Given_Null_When_GetFunctionName_Invoked_Then_It_Should_Throw_Exception()
        {
            Action action = () => FunctionAttributeExtensions.GetFunctionName(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("DoSomething", "FakeFunction")]
        public void Given_MemberInfo_When_ExistsCustomAttribute_Invoked_Then_It_Should_Return_Result(string methodName, string expected)
        {
            var method = typeof(FakeHttpTrigger).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

            var result = FunctionAttributeExtensions.GetFunctionName(method);

            result.Should().BeOfType<FunctionAttribute>();
            result.Name.Should().Be(expected);
        }
    }
}
