// using System;
// using System.Reflection;

// using FluentAssertions;

// using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
// using Microsoft.VisualStudio.TestTools.UnitTesting;

// using MethodInfoExtensions = Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions.MethodInfoExtensions;

// namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Extensions.Tests
// {
//     [TestClass]
//     public class MethodInfoExtensionsTests
//     {
//         [TestMethod]
//         public void Given_Null_When_GetFunctionName_Invoked_Then_It_Should_Throw_Exception()
//         {
//             Action action = () => MethodInfoExtensions.GetFunctionName(null);

//             action.Should().Throw<ArgumentNullException>();
//         }

//         [DataTestMethod]
//         [DataRow("DoSomething", "FakeFunction")]
//         public void Given_MemberInfo_When_ExistsCustomAttribute_Invoked_Then_It_Should_Return_Result(string methodName, string expected)
//         {
//             var method = typeof(FakeHttpTrigger).GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

//             var result = MethodInfoExtensions.GetFunctionName(method);

//             result.Should().BeOfType<FunctionNameAttribute>();
//             result.Name.Should().Be(expected);
//         }

//         [TestMethod]
//         public void Given_Null_When_GetHttpTrigger_Invoked_Then_It_Should_Throw_Exception()
//         {
//             Action action = () => MethodInfoExtensions.GetHttpTrigger(null);

//             action.Should().Throw<ArgumentNullException>();
//         }
//     }
// }
