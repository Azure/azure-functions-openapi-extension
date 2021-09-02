using System;
using System.Linq;
using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
{
    [TestClass]
    public class DocumentHelperTests
    {
        [TestMethod]
        public void Given_Null_Constructor_Should_Throw_Exception()
        {
            Action action = () => new TestDocumentHelper(null, null);
            action.Should().Throw<ArgumentNullException>();

            var filter = new RouteConstraintFilter();

            action = () => new TestDocumentHelper(filter, null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void GetOpenApiSchemas_Result_Should_Contain_Schema_For_Function_Classes()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var filter = new RouteConstraintFilter();
            var acceptor = new OpenApiSchemaAcceptor();
            var documentHelper = new TestDocumentHelper(filter, acceptor);
            var visitorCollection = VisitorCollection.CreateInstance();

            var methods = typeof(FakeFunctions).GetMethods().ToList();

            var schemas = documentHelper.GetOpenApiSchemas(methods, namingStrategy, visitorCollection);

            schemas.Should().NotBeNull();
            schemas.Count.Should().Be(6);

            schemas.Should().ContainKey("FakeClassModel");

            schemas["FakeClassModel"].Properties.Count.Should().Be(2);
            schemas["FakeClassModel"].Type.Should().Be("object");

            schemas.Should().ContainKey("FakeOtherClassModel");

            schemas["FakeOtherClassModel"].Properties.Count.Should().Be(2);
            schemas["FakeOtherClassModel"].Type.Should().Be("object");

            schemas["FakeClassModel"].Properties.Count.Should().Be(2);
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
        public void Given_AdditionalOpenApiParameters_Configured_returns_ApiParameter()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var filter = new RouteConstraintFilter();
            var acceptor = new OpenApiSchemaAcceptor();
            var documentHelper = new TestDocumentHelper(filter, acceptor);
            var visitorCollection = VisitorCollection.CreateInstance();

            var method = typeof(FakeFunctions).GetMethod(nameof(FakeFunctions.MethodNoAttributes));
            var httpTriggerAttr = new HttpTriggerAttribute(new[] { "get" })
            {
                Route = "test/route",
            };

            var additionalParam = new OpenApiParameterAttribute("NoAttr");

            var apiObj = new Mock<IAdditionalOpenApiParameter>();
            apiObj.Setup(x => x.OpenApiParameters(It.IsAny<MethodInfo>())).Returns(new[] { additionalParam });
            var options = new Mock<IOpenApiConfigurationOptions>();
            options.Setup(x => x.AdditionalOpenApiParameters).Returns(apiObj.Object);

            var apiParameters = documentHelper.GetOpenApiParameters(method, httpTriggerAttr, namingStrategy, visitorCollection, options.Object);

            apiParameters.Should().HaveCount(1);
            apiParameters.First().Name.Should().Be(additionalParam.Name);
        }

        [TestMethod]
        public void Given_AdditionalOpenApiRequestBody_Configured_returns_BodyParameter()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var filter = new RouteConstraintFilter();
            var acceptor = new OpenApiSchemaAcceptor();
            var documentHelper = new TestDocumentHelper(filter, acceptor);
            var visitorCollection = VisitorCollection.CreateInstance();

            var method = typeof(FakeFunctions).GetMethod(nameof(FakeFunctions.MethodNoAttributes));

            var bodyAttr = new OpenApiRequestBodyAttribute("application/json", typeof(string));

            var apiObj = new Mock<IAdditionalOpenApiRequestBody>();
            apiObj.Setup(x => x.OpenApiRequestBody(It.IsAny<MethodInfo>())).Returns(new[] { bodyAttr });
            var options = new Mock<IOpenApiConfigurationOptions>();
            options.Setup(x => x.AdditionalOpenApiRequestBody).Returns(apiObj.Object);

            var requestBody = documentHelper.GetOpenApiRequestBody(method, namingStrategy, visitorCollection, Core.Enums.OpenApiVersionType.V3, options.Object);

            requestBody.Should().NotBeNull();
        }

        [TestMethod]
        public void Given_AdditionalOpenApiResponse_Configured_returns_ResponseParameter()
        {
            var namingStrategy = new DefaultNamingStrategy();
            var filter = new RouteConstraintFilter();
            var acceptor = new OpenApiSchemaAcceptor();
            var documentHelper = new TestDocumentHelper(filter, acceptor);
            var visitorCollection = VisitorCollection.CreateInstance();

            var method = typeof(FakeFunctions).GetMethod(nameof(FakeFunctions.MethodNoAttributes));

            var BodyResponse = new OpenApiResponseWithBodyAttribute(System.Net.HttpStatusCode.OK, "BodyResponse", typeof(string));
            var NoBodyResponse = new OpenApiResponseWithoutBodyAttribute(System.Net.HttpStatusCode.Created);

            var apiObj = new Mock<IAdditionalOpenApiResponse>();
            apiObj.Setup(x => x.OpenApiResponseWithBody(It.IsAny<MethodInfo>())).Returns(new[] { BodyResponse });
            apiObj.Setup(x => x.OpenApiResponseWithoutBody(It.IsAny<MethodInfo>())).Returns(new[] { NoBodyResponse });
            var options = new Mock<IOpenApiConfigurationOptions>();
            options.Setup(x => x.AdditionalOpenApiResponse).Returns(apiObj.Object);

            var responses = documentHelper.GetOpenApiResponses(method, namingStrategy, visitorCollection, Core.Enums.OpenApiVersionType.V3, options.Object);

            responses.Should().HaveCount(2);
        }
    }

    /// <summary>
    /// Document helper for Azure Functions
    /// </summary>
    internal class TestDocumentHelper : DocumentHelper<FunctionNameAttribute>
    {
        /// <summary
        /// Initializes a new instance of the <see cref="TestDocumentHelper"/> class.
        /// </summary>
        public TestDocumentHelper(RouteConstraintFilter filter, IOpenApiSchemaAcceptor acceptor)
            : base(filter, acceptor, f => f.Name)
        {
        }
    }
}
