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
    public class DictionaryObjectTypeVisitorTests
    {
        private VisitorCollection _visitorCollection;
        private IVisitor _visitor;
        private NamingStrategy _strategy;

        [TestInitialize]
        public void Init()
        {
            this._visitorCollection = VisitorCollection.CreateInstance();
            this._visitor = new DictionaryObjectTypeVisitor(this._visitorCollection);
            this._strategy = new CamelCaseNamingStrategy();
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, string[]>), false)]
        [DataRow(typeof(Dictionary<string, string>), false)]
        public void Given_Type_When_IsNavigatable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsNavigatable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, string[]>), true)] 
        [DataRow(typeof(Dictionary<string, string>), true)]
        [DataRow(typeof(IDictionary<string, string>), true)]
        [DataRow(typeof(IReadOnlyDictionary<string, string>), true)]
        [DataRow(typeof(KeyValuePair<string, string>), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, string[]>), false)] 
        [DataRow(typeof(Dictionary<string, string>), false)]
        [DataRow(typeof(IDictionary<string, string>), false)]
        [DataRow(typeof(IReadOnlyDictionary<string, string>), false)]
        [DataRow(typeof(KeyValuePair<string, string>), false)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsParameterVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsParameterVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, string[]>), true)] 
        [DataRow(typeof(Dictionary<string, string>), true)]
        [DataRow(typeof(IDictionary<string, string>), true)]
        [DataRow(typeof(IReadOnlyDictionary<string, string>), true)]
        [DataRow(typeof(KeyValuePair<string, string>), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsPayloadVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsPayloadVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, string>), "object", null, "string", false, "string", 0)]
        [DataRow(typeof(IDictionary<string, string>), "object", null, "string", false, "string", 0)]
        [DataRow(typeof(IReadOnlyDictionary<string, string>), "object", null, "string", false, "string", 0)]
        [DataRow(typeof(KeyValuePair<string, string>), "object", null, "string", false, "string", 0)]
        [DataRow(typeof(Dictionary<string, FakeModel>), "object", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(Dictionary<string, string[]>), "object", null, "array", true, "list_string", 1)]
        [DataRow(typeof(IDictionary<string, FakeModel>), "object", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(IReadOnlyDictionary<string, FakeModel>), "object", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(KeyValuePair<string, FakeModel>), "object", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(KeyValuePair<string, List<FakeGenericModel<FakeModel>>>), "object", null, "array", true, "list_fakeGenericModel_fakeModel", 1)]
        public void Given_Type_When_Visit_Invoked_Then_It_Should_Return_Result(Type dictionaryType, string dataType, string dataFormat, string additionalPropertyType, bool isReferential, string referenceId, int expected)
        {
            var name = "hello";
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, dictionaryType);

            this._visitor.Visit(acceptor, type, this._strategy);

            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Type.Should().Be(dataType);
            acceptor.Schemas[name].Format.Should().Be(dataFormat);

            acceptor.Schemas[name].AdditionalProperties.Should().NotBeNull();
            acceptor.Schemas[name].AdditionalProperties.Type.Should().Be(additionalPropertyType);

            if (isReferential)
            {
                acceptor.Schemas[name].AdditionalProperties.Reference.Type.Should().Be(ReferenceType.Schema);
                acceptor.Schemas[name].AdditionalProperties.Reference.Id.Should().Be(referenceId);
            }

            acceptor.RootSchemas.Count(p => p.Key == referenceId).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("hello", "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(string name, string description)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(Dictionary<string, string>));
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
            var type = new KeyValuePair<string, Type>(name, typeof(Dictionary<string, string>));
            var attribute = new OpenApiSchemaVisibilityAttribute(visibility);

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas[name].Extensions.Should().ContainKey("x-ms-visibility");
            acceptor.Schemas[name].Extensions["x-ms-visibility"].Should().BeOfType<OpenApiString>();
            (acceptor.Schemas[name].Extensions["x-ms-visibility"] as OpenApiString).Value.Should().Be(visibility.ToDisplayName(this._strategy));
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, string>), "object", null, null)]
        [DataRow(typeof(IDictionary<string, string>), "object", null, null)]
        [DataRow(typeof(IReadOnlyDictionary<string, string>), "object", null, null)]
        [DataRow(typeof(KeyValuePair<string, string>), "object", null, null)]
        public void Given_Type_When_ParameterVisit_Invoked_Then_It_Should_Return_Result(Type dictionaryType, string dataType, string dataFormat, OpenApiSchema expected)
        {
            var result = this._visitor.ParameterVisit(dictionaryType, this._strategy);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, string>), "object", null, "string", false, null)]
        [DataRow(typeof(IDictionary<string, string>), "object", null, "string", false, null)]
        [DataRow(typeof(IReadOnlyDictionary<string, string>), "object", null, "string", false, null)]
        [DataRow(typeof(KeyValuePair<string, string>), "object", null, "string", false, null)]
        [DataRow(typeof(Dictionary<string, FakeModel>), "object", null, "object", true, "fakeModel")]
        [DataRow(typeof(IDictionary<string, FakeModel>), "object", null, "object", true, "fakeModel")]
        [DataRow(typeof(IReadOnlyDictionary<string, FakeModel>), "object", null, "object", true, "fakeModel")]
        [DataRow(typeof(KeyValuePair<string, FakeModel>), "object", null, "object", true, "fakeModel")]
        public void Given_Type_When_PayloadVisit_Invoked_Then_It_Should_Return_Result(Type dictionaryType, string dataType, string dataFormat, string additionalPropertyType, bool reference, string referenceId)
        {
            var result = this._visitor.PayloadVisit(dictionaryType, this._strategy);

            result.Type.Should().Be(dataType);
            result.Format.Should().Be(dataFormat);

            result.AdditionalProperties.Should().NotBeNull();
            result.AdditionalProperties.Type.Should().Be(additionalPropertyType);

            if (reference)
            {
                result.AdditionalProperties.Reference.Type.Should().Be(ReferenceType.Schema);
                result.AdditionalProperties.Reference.Id.Should().Be(referenceId);
            }
            else
            {
                result.AdditionalProperties.Reference.Should().BeNull();
            }
        }

        [DataTestMethod]
        [DataRow(typeof(Dictionary<string, FakeAliasCollectionModel>), typeof(FakeAliasCollectionModel), typeof(FakeAliasSubModel), typeof(FakeSubModel), typeof(FakeDummyModel))]
        [DataRow(typeof(IDictionary<string, FakeAliasCollectionModel>), typeof(FakeAliasCollectionModel), typeof(FakeAliasSubModel), typeof(FakeSubModel), typeof(FakeDummyModel))]
        [DataRow(typeof(IReadOnlyDictionary<string, FakeAliasCollectionModel>), typeof(FakeAliasCollectionModel), typeof(FakeAliasSubModel), typeof(FakeSubModel), typeof(FakeDummyModel))]
        [DataRow(typeof(KeyValuePair<string, FakeAliasCollectionModel>), typeof(FakeAliasCollectionModel), typeof(FakeAliasSubModel), typeof(FakeSubModel), typeof(FakeDummyModel))]
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
