using System;
using System.Collections.Generic;

using FluentAssertions;
using FluentAssertions.Common;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Tests
{
    [TestClass]
    public class AppSettingsBaseTests
    {
        [TestMethod]
        public void Given_Type_Should_Be_Abstract()
        {
            typeof(AppSettingsBase).Should().BeAbstract();
        }

        [TestMethod]
        public void Given_Type_Should_Have_Constructor()
        {
            typeof(AppSettingsBase).Should().HaveDefaultConstructor();
        }

        [TestMethod]
        public void Given_Type_Should_Have_Properties()
        {
            typeof(AppSettingsBase)
                .Should().HaveProperty<IConfiguration>("Config");
        }

        [TestMethod]
        public void Given_Type_Should_Have_Methods()
        {
            var @params = new List<Type>();

            typeof(AppSettingsBase)
                .Should().HaveMethod("GetBasePath", @params.ToArray())
                    .Which.Should().Return<string>()
                        .And.HaveAccessModifier(CSharpAccessModifier.Protected);
        }
    }
}
