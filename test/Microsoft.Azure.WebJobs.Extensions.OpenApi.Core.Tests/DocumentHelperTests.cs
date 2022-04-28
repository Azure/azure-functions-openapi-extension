using System;
using System.Linq;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
{
    [TestClass]
    public class DocumentHelperTests
    {
        [TestMethod]
        public void Given_Null_Constructor_Should_Throw_Exception()
        {
            Action action = () => new DocumentHelper(null, null);
            action.Should().Throw<ArgumentNullException>();

            var filter = new RouteConstraintFilter();

            action = () => new DocumentHelper(filter, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void GetOpenApiSchemas_Result_Should_Contain_Schema_For_Function_Classes()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var filter = new RouteConstraintFilter();
            var acceptor = new OpenApiSchemaAcceptor();
            var documentHelper = new DocumentHelper(filter, acceptor);
            var visitorCollection = VisitorCollection.CreateInstance();

            var methods = typeof(FakeFunctions).GetMethods().ToList();

            var schemas = documentHelper.GetOpenApiSchemas(methods, namingStrategy, visitorCollection);

            schemas.Should().NotBeNull();
            schemas.Count.Should().Be(6);

            schemas.Should().ContainKey("FakeClassModel");

            schemas["FakeClassModel"].Properties.Count.Should().Be(3);
            schemas["FakeClassModel"].Type.Should().Be("object");

            schemas.Should().ContainKey("FakeOtherClassModel");

            schemas["FakeOtherClassModel"].Properties.Count.Should().Be(2);
            schemas["FakeOtherClassModel"].Type.Should().Be("object");

            schemas["FakeClassModel"].Properties.Count.Should().Be(3);
            schemas["FakeClassModel"].Type.Should().Be("object");

            schemas.Should().ContainKey("FakeListModel");

            schemas["FakeListModel"].Properties.Count.Should().Be(1);
            schemas["FakeListModel"].Type.Should().Be("object");

            schemas.Should().ContainKey("FakeStringModel");
            schemas["FakeStringModel"].Properties.Count.Should().Be(2);
            schemas["FakeStringModel"].Type.Should().Be("object");

            schemas.Should().ContainKey("FakeGenericModel_FakeClassModel");

            schemas["FakeGenericModel_FakeClassModel"].Properties.Count.Should().Be(2);
            schemas["FakeGenericModel_FakeClassModel"].Type.Should().Be("object");
            schemas["FakeGenericModel_FakeClassModel"].Properties.Should().ContainKey("Name");
            schemas["FakeGenericModel_FakeClassModel"].Properties.Should().ContainKey("Value");
            schemas["FakeGenericModel_FakeClassModel"].Properties["Value"].Properties.Should().ContainKey("Number");
            schemas["FakeGenericModel_FakeClassModel"].Properties["Value"].Properties.Should().ContainKey("Text");

            schemas.Should().ContainKey("FakeGenericModel_FakeOtherClassModel");

            schemas["FakeGenericModel_FakeOtherClassModel"].Properties.Count.Should().Be(2);
            schemas["FakeGenericModel_FakeOtherClassModel"].Type.Should().Be("object");
            schemas["FakeGenericModel_FakeOtherClassModel"].Properties.Should().ContainKey("Name");
            schemas["FakeGenericModel_FakeOtherClassModel"].Properties.Should().ContainKey("Value");
            schemas["FakeGenericModel_FakeOtherClassModel"].Properties["Value"].Properties.Should().ContainKey("FirstName");
            schemas["FakeGenericModel_FakeOtherClassModel"].Properties["Value"].Properties.Should().ContainKey("LastName");
        }

        [TestMethod]
        public void GetOpenApiSchemas_Result_Should_Contain_No_Overlapping_OpenApiReferences()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var filter = new RouteConstraintFilter();
            var acceptor = new OpenApiSchemaAcceptor();
            var documentHelper = new DocumentHelper(filter, acceptor);
            var visitorCollection = VisitorCollection.CreateInstance();

            var methods = typeof(FakeFunctionsWithOverlappingModel.OverlappingClass1).GetMethods()
                .Concat(typeof(FakeFunctionsWithOverlappingModel.OverlappingClass2).GetMethods()).ToList();

            var schemas = documentHelper.GetOpenApiSchemas(methods, namingStrategy, visitorCollection);

            schemas.Should().NotBeNull();
            schemas.Count.Should().Be(1);

            schemas.Where(x => x.Key == "FakeInternalModel").Count().Should().Be(1);

            schemas["FakeInternalModel"].Type.Should().Be("object");
        }
    }
}
