using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Serialization;

using PropertyInfoExtensions = Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions.PropertyInfoExtensions;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Extensions
{
    [TestClass]
    public class PropertyInfoExtensionsTests
    {
        [TestMethod]
        public void Given_Property_When_GetJsonPropertyName_Invoked_Then_It_Should_Return_PropertyName()
        {
            var name = "FakeProperty";
            var jsonPropertyName = "FakeProperty";
            var property = typeof(FakeModel).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            var namingStrategy = new DefaultNamingStrategy();

            var result = PropertyInfoExtensions.GetJsonPropertyName(property, namingStrategy);

            result.Should().Be(jsonPropertyName);
        }

        [TestMethod]
        public void Given_Property_When_GetJsonPropertyName_Invoked_Then_It_Should_Return_JsonPropertyName()
        {
            var name = "FakeProperty2";
            var jsonPropertyName = "anotherJsonFakeProperty";
            var property = typeof(FakeModel).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            var namingStrategy = new DefaultNamingStrategy();

            var result = PropertyInfoExtensions.GetJsonPropertyName(property, namingStrategy);

            result.Should().Be(jsonPropertyName);
        }

        [TestMethod]
        public void Given_Property_When_GetJsonPropertyName_Invoked_Then_It_Should_Return_DataMemberName()
        {
            var name = "FakeProperty3";
            var dataMemberPropertyName = "anotherDataMemberFakeProperty";

            var property = typeof(FakeModel).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            var namingStrategy = new DefaultNamingStrategy();

            var result = PropertyInfoExtensions.GetJsonPropertyName(property, namingStrategy);

            result.Should().Be(dataMemberPropertyName);
        }

        [TestMethod]
        public void Given_BothProperties_When_GetJsonPropertyName_Invoked_Then_It_Should_Return_JsonPropertyName()
        {
            var name = "FakeProperty4";
            var jsonPropertyName = "jsonFakeProperty";
            var dataMemberPropertyName = "dataMemberFakeProperty";

            var property = typeof(FakeModel).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            var namingStrategy = new DefaultNamingStrategy();

            var result = PropertyInfoExtensions.GetJsonPropertyName(property, namingStrategy);

            result.Should().Be(jsonPropertyName);
            result.Should().NotBe(dataMemberPropertyName);
        }

        [TestMethod]
        public void Given_Property_When_GetJsonPropertyName_IsEmpty_Then_It_Should_Return_ElementName()
        {
            var name = "FakePropertyNoPropertyValue";
            var property = typeof(FakeModel).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            var namingStrategy = new DefaultNamingStrategy();

            var result = PropertyInfoExtensions.GetJsonPropertyName(property, namingStrategy);

            result.Should().NotBeNullOrEmpty();
            result.Should().Be(name);
        }

        [TestMethod]
        public void Given_Property_When_DefaultJsonProperyAnnotation_Invoked_Then_It_Should_Return_ElementName()
        {
            var name = "FakePropertyNoAnnotation";
            var property = typeof(FakeModel).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            var namingStrategy = new DefaultNamingStrategy();

            var result = PropertyInfoExtensions.GetJsonPropertyName(property, namingStrategy);

            result.Should().NotBeNullOrEmpty();
            result.Should().Be(name);
        }

        [TestMethod]
        public void Given_Property_When_GetJsonPropertyName_IsEmpty_WithCamelCaseNaming_Then_It_Should_Return_ElementName()
        {
            var name = "FakePropertyNoPropertyValue";
            var camelCaseName = "fakePropertyNoPropertyValue";
            var property = typeof(FakeModel).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            var namingStrategy = new CamelCaseNamingStrategy();

            var result = PropertyInfoExtensions.GetJsonPropertyName(property, namingStrategy);

            result.Should().NotBeNullOrEmpty();
            result.Should().Be(camelCaseName);
        }

        [TestMethod]
        public void Given_Property_When_DefaultJsonProperyAnnotation_Invoked_WithCamelCaseNaming_Then_It_Should_Return_ElementName()
        {
            var name = "FakePropertyNoAnnotation";
            var camelCaseName = "fakePropertyNoAnnotation";
            var property = typeof(FakeModel).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            var namingStrategy = new CamelCaseNamingStrategy();

            var result = PropertyInfoExtensions.GetJsonPropertyName(property, namingStrategy);

            result.Should().NotBeNullOrEmpty();
            result.Should().Be(camelCaseName);
        }
    }
}
