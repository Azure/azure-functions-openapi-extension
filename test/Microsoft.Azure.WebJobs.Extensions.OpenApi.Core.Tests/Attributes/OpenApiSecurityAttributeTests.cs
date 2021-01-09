using System;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiSecurityAttributeTests
    {
        [TestMethod]
        public void Given_NullValue_Constructor_Should_Throw_Exception()
        {
            Action action = () => new OpenApiSecurityAttribute(null, SecuritySchemeType.ApiKey);
            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("authKey", SecuritySchemeType.ApiKey)]
        public void Given_Value_Property_Should_Return_Value(string schemeName, SecuritySchemeType schemeType)
        {
            var attribute = new OpenApiSecurityAttribute(schemeName, schemeType);

            attribute.SchemeName.Should().Be(schemeName);
            attribute.SchemeType.Should().Be(schemeType);
        }
    }
}
