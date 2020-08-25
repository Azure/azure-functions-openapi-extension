using System;
using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Resolvers;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Configuration.AppSettings.Tests.Resolvers
{
    [TestClass]
    public class ConfigurationResolverTests
    {
        [TestMethod]
        public void Given_Type_Should_Have_Methods()
        {
            var @params = new List<Type>();

            typeof(ConfigurationResolver)
                .Should().HaveMethod("Resolve", @params.ToArray())
                    .Which.Should().Return<IConfiguration>();

            typeof(ConfigurationResolver)
                .Should().HaveMethod("GetValue", new[] { typeof(string), typeof(IConfiguration) });

            typeof(ConfigurationResolver)
                .Should().HaveMethod("GetBasePath", new[] { typeof(IConfiguration) })
                    .Which.Should().Return<string>();
        }
    }
}
