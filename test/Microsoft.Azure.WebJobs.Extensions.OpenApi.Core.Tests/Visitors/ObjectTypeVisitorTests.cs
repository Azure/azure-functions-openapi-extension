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
        [DataRow(typeof(FakeModel), "object", null, 2, 3, "fakeModel")]
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
        [DataRow("hello", "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(string name, string description)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeModel));
            var attribute = new OpenApiPropertyAttribute() { Description = description };

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas[name].Nullable.Should().Be(false);
            acceptor.Schemas[name].Default.Should().BeNull();
            acceptor.Schemas[name].Description.Should().Be(description);
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

        [TestMethod]
        public void Given_Alias_Type_When_Visit_Invoked_Then_It_Should_Return_All_Sub_Schemas()
        {
            var originSchemaKey = "fakeAliasModel";
            var visitType = typeof(FakeAliasModel);
            var acceptor = new OpenApiSchemaAcceptor();

            this._visitor.Visit(acceptor, new KeyValuePair<string, Type>(originSchemaKey, visitType), this._strategy);

            acceptor.Schemas.Count.Should().Be(1);
            acceptor.Schemas.Should().ContainKey("fakeAliasModel");

            acceptor.Schemas["fakeAliasModel"].Type.Should().Be("object");
            acceptor.Schemas["fakeAliasModel"].Properties.Count.Should().Be(3);
            acceptor.Schemas["fakeAliasModel"].Type.Should().Be("object");
            acceptor.Schemas["fakeAliasModel"].Reference.Type.Should().Be(ReferenceType.Schema);
            acceptor.Schemas["fakeAliasModel"].Reference.Id.Should().Be("fakeAliasModel");

            acceptor.RootSchemas.Count.Should().Be(3);

            acceptor.RootSchemas.Should().ContainKey("fakeSubModel");
            acceptor.RootSchemas["fakeSubModel"].Type.Should().Be("object");
            acceptor.RootSchemas["fakeSubModel"].Properties.Count.Should().Be(1);
            acceptor.RootSchemas["fakeSubModel"].Reference.Type.Should().Be(ReferenceType.Schema);
            acceptor.RootSchemas["fakeSubModel"].Reference.Id.Should().Be("fakeSubModel");

            acceptor.RootSchemas.Should().ContainKey("fakeAliasSubModel");
            acceptor.RootSchemas["fakeAliasSubModel"].Type.Should().Be("object");
            acceptor.RootSchemas["fakeAliasSubModel"].Properties.Count.Should().Be(3);
            acceptor.RootSchemas["fakeAliasSubModel"].Reference.Type.Should().Be(ReferenceType.Schema);
            acceptor.RootSchemas["fakeAliasSubModel"].Reference.Id.Should().Be("fakeAliasSubModel");

            acceptor.RootSchemas.Should().ContainKey("fakeDummyModel");
            acceptor.RootSchemas["fakeDummyModel"].Type.Should().Be("object");
            acceptor.RootSchemas["fakeDummyModel"].Properties.Count.Should().Be(0);
        }
    }
}
