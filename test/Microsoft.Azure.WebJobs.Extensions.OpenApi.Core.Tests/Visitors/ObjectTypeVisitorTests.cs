using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Visitors
{
    [TestClass]
    public class ObjectTypeVisitorTests
    {
        private VisitorCollection _visitorCollection;
        private IVisitor _visitor;
        private NamingStrategy _strategy;

        [TestInitialize]
        public void Init()
        {
            this._visitorCollection = VisitorCollection.CreateInstance();
            this._visitor = new ObjectTypeVisitor(this._visitorCollection);
            this._strategy = new CamelCaseNamingStrategy();
        }

        [DataTestMethod]
        [DataRow(typeof(FakeModel), true)]
        public void Given_Type_When_IsNavigatable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsNavigatable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeModel), true)]
        [DataRow(typeof(object), false)]
        [DataRow(typeof(int), false)]
        [DataRow(typeof(Uri), false)]
        [DataRow(typeof(IEnumerable<object>), false)]
        [DataRow(typeof(IDictionary<string, object>), false)]
        public void Given_Type_When_IsVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeModel), false)]
        [DataRow(typeof(object), false)]
        [DataRow(typeof(int), false)]
        [DataRow(typeof(Uri), false)]
        [DataRow(typeof(IEnumerable<object>), false)]
        [DataRow(typeof(IDictionary<string, object>), false)]
        public void Given_Type_When_IsParameterVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsParameterVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeModel), true)]
        [DataRow(typeof(int), false)]
        [DataRow(typeof(Uri), false)]
        [DataRow(typeof(IEnumerable<object>), false)]
        [DataRow(typeof(IDictionary<string, object>), false)]
        public void Given_Type_When_IsPayloadVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsPayloadVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeModel), "object", null, 3, 3, "fakeModel")]
        [DataRow(typeof(FakeRequiredModel), "object", null, 1, 0, "fakeRequiredModel")]
        [DataRow(typeof(FakeRecursiveModel), "object", null, 3, 2, "fakeRecursiveModel")]
        [DataRow(typeof(FakeGenericModel<List<FakeModel>>), "object", null, 0, 4, "fakeGenericModel_list_fakeModel")]
        public void Given_Type_When_Visit_Invoked_Then_It_Should_Return_Result(Type objectType, string dataType, string dataFormat, int requiredCount, int rootSchemaCount, string referenceId)
        {
            var name = "hello";
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, objectType);

            this._visitor.Visit(acceptor, type, this._strategy);

            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Type.Should().Be(dataType);
            acceptor.Schemas[name].Format.Should().Be(dataFormat);

            acceptor.Schemas[name].Required.Count.Should().Be(requiredCount);

            acceptor.RootSchemas.Count.Should().Be(rootSchemaCount);

            acceptor.RootSchemas.Keys.Any(a => a.Contains("`")).Should().BeFalse();

            acceptor.Schemas[name].Reference.Type.Should().Be(ReferenceType.Schema);
            acceptor.Schemas[name].Reference.Id.Should().Be(referenceId);
        }

        [DataTestMethod]
        [DataRow("hello", "lorem ipsum", true)]
        [DataRow("hello", "lorem ipsum", false)]
        public void Given_OpenApiPropertyAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(string name, string description, bool nullable)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeModel));
            var attribute = new OpenApiPropertyAttribute() { Description = description, Nullable = nullable };

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas[name].Nullable.Should().Be(nullable);
            acceptor.Schemas[name].Default.Should().BeNull();
            acceptor.Schemas[name].Description.Should().Be(description);
        }

        [DataTestMethod]
        [DataRow("hello", 3)]
        public void Given_ObjectModel_With_NewtonsoftJsonPropertyAttribute_When_Visit_Invoked_Then_It_Should_Set_Required_And_Nullability(string name, int requiredCount)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeModel));

            this._visitor.Visit(acceptor, type, this._strategy);

            acceptor.Schemas[name].Required.Count.Should().Be(requiredCount);
            acceptor.Schemas[name].Required.Should().Contain("fakeProperty");
            acceptor.Schemas[name].Required.Should().Contain("anotherJsonFakeProperty");
            acceptor.Schemas[name].Required.Should().Contain("fakePropertyRequiredAllowNullPropertyValue");

            acceptor.Schemas[name].Properties["anotherJsonFakeProperty"].Nullable.Should().BeFalse();
            acceptor.Schemas[name].Properties["fakePropertyNoPropertyValue"].Nullable.Should().BeTrue();
            acceptor.Schemas[name].Properties["fakePropertyRequiredAllowNullPropertyValue"].Nullable.Should().BeTrue();
            acceptor.Schemas[name].Properties["fakePropertyRequiredDisallowAllowNullPropertyValue"].Nullable.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("hello", 3)]
        public void Given_RecursiveModel_With_NewtonsoftJsonPropertyAttribute_When_Visit_Invoked_Then_It_Should_Set_Required_And_Nullability(string name, int requiredCount)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeRecursiveModel));

            this._visitor.Visit(acceptor, type, this._strategy);

            acceptor.Schemas[name].Required.Count.Should().Be(requiredCount);
            acceptor.Schemas[name].Required.Should().Contain("stringValue");
            acceptor.Schemas[name].Required.Should().Contain("fakeAlwaysRequiredProperty");
            acceptor.Schemas[name].Required.Should().Contain("fakePropertyRequiredAllowNullPropertyValue");

            acceptor.Schemas[name].Properties["fakeAlwaysRequiredProperty"].Nullable.Should().BeFalse();
            acceptor.Schemas[name].Properties["fakePropertyNoPropertyValue"].Nullable.Should().BeTrue();
            acceptor.Schemas[name].Properties["fakePropertyRequiredAllowNullPropertyValue"].Nullable.Should().BeTrue();
            acceptor.Schemas[name].Properties["fakePropertyRequiredDisallowAllowNullPropertyValue"].Nullable.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("hello", OpenApiVisibilityType.Advanced)]
        [DataRow("hello", OpenApiVisibilityType.Important)]
        [DataRow("hello", OpenApiVisibilityType.Internal)]
        public void Given_OpenApiSchemaVisibilityAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(string name, OpenApiVisibilityType visibility)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeModel));
            var attribute = new OpenApiSchemaVisibilityAttribute(visibility);

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas[name].Extensions.Should().ContainKey("x-ms-visibility");
            acceptor.Schemas[name].Extensions["x-ms-visibility"].Should().BeOfType<OpenApiString>();
            (acceptor.Schemas[name].Extensions["x-ms-visibility"] as OpenApiString).Value.Should().Be(visibility.ToDisplayName(this._strategy));
        }

        [DataTestMethod]
        [DataRow(typeof(FakeModel), "object", null, null)]
        public void Given_Type_When_ParameterVisit_Invoked_Then_It_Should_Return_Result(Type objectType, string dataType, string dataFormat, OpenApiSchema expected)
        {
            var result = this._visitor.ParameterVisit(objectType, this._strategy);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeModel), "object", null)]
        public void Given_Type_When_PayloadVisit_Invoked_Then_It_Should_Return_Result(Type objectType, string dataType, string dataFormat)
        {
            var result = this._visitor.PayloadVisit(objectType, this._strategy);

            result.Type.Should().Be(dataType);
            result.Format.Should().Be(dataFormat);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeAliasCollectionModel), typeof(FakeAliasSubModel), typeof(FakeSubModel), typeof(FakeDummyModel))]
        [DataRow(typeof(FakeAliasDictionaryModel), typeof(FakeAliasSubModel), typeof(FakeSubModel), typeof(FakeDummyModel))]
        public void Given_Alias_Type_When_Visit_Invoked_Then_It_Should_Return_All_Sub_Schemas(Type type, params Type[] schemaTypes)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var key = type.GetOpenApiReferenceId(type.IsOpenApiDictionary(), type.IsOpenApiArray(), this._strategy);

            this._visitor.Visit(acceptor, new KeyValuePair<string, Type>(key, type), this._strategy);

            acceptor.RootSchemas.Count.Should().Be(schemaTypes.Length);

            foreach (var schemaType in schemaTypes)
            {
                var subKey = schemaType.GetOpenApiReferenceId(schemaType.IsOpenApiDictionary(), schemaType.IsOpenApiArray(), this._strategy);

                acceptor.RootSchemas.Should().ContainKey(subKey);
            }
        }
    }
}
