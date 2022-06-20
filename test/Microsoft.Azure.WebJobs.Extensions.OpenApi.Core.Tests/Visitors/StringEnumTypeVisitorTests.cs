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
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Visitors
{
    [TestClass]
    public class StringEnumTypeVisitorTests
    {
        private VisitorCollection _visitorCollection;
        private IVisitor _visitor;
        private NamingStrategy _strategy;

        [TestInitialize]
        public void Init()
        {
            this._visitorCollection = new VisitorCollection();
            this._visitor = new StringEnumTypeVisitor(this._visitorCollection);
            this._strategy = new CamelCaseNamingStrategy();
        }

        [DataTestMethod]
        [DataRow(typeof(FakeStringEnum), false)]
        public void Given_Type_When_IsNavigatable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsNavigatable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeStringEnum), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeStringEnum), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsParameterVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsParameterVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeStringEnum), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsPayloadVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsPayloadVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("string", null, typeof(FakeStringEnum))]
        public void Given_Type_When_Visit_Invoked_Then_It_Should_Return_Result(string dataType, string dataFormat, Type enumType)
        {
            var name = "hello";
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeStringEnum));
            var enums = enumType.ToOpenApiStringCollection();

            this._visitor.Visit(acceptor, type, this._strategy);
            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Reference.Should().NotBeNull();
            var rootKey = acceptor.Schemas[name].Reference.Id;
            acceptor.RootSchemas.Should().ContainKey(rootKey);
            var rootSchema = acceptor.RootSchemas[rootKey];
            rootSchema.Type.Should().Be(dataType);
            rootSchema.Format.Should().Be(dataFormat);

            for (var i = 0; i < acceptor.Schemas[name].Enum.Count; i++)
            {
                var @enum = rootSchema.Enum[i];
                @enum.Should().BeOfType<OpenApiString>();
                (@enum as OpenApiString).Value.Should().Be((enums[i] as OpenApiString).Value);
            }

            (rootSchema.Default as OpenApiString).Value.Should().Be((enums.First() as OpenApiString).Value);
        }

        [DataTestMethod]
        [DataRow("hello", "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(string name, string description)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeStringEnum));
            var enums = type.Value.ToOpenApiStringCollection();
            var attribute = new OpenApiPropertyAttribute() { Description = description };

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas[name].Reference.Should().NotBeNull();
            var rootSchema = acceptor.RootSchemas[acceptor.Schemas[name].Reference.Id];
            rootSchema.Nullable.Should().Be(false);
            rootSchema.Description.Should().Be(description);
            (rootSchema.Default as OpenApiString).Value.Should().Be(FakeStringEnum.StringValue1.ToDisplayName());
        }

        [DataTestMethod]
        [DataRow("hello", true, "lorem ipsum")]
        [DataRow("hello", false, "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_With_Default_When_Visit_Invoked_Then_It_Should_Return_Result(string name, bool nullable, string description)
        {
            var @default = FakeStringEnum.StringValue2;
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeStringEnum));
            var attribute = new OpenApiPropertyAttribute() { Nullable = nullable, Default = @default, Description = description };

            this._visitor.Visit(acceptor, type, this._strategy, attribute);
            acceptor.Schemas[name].Reference.Should().NotBeNull();
            var rootSchema = acceptor.RootSchemas[acceptor.Schemas[name].Reference.Id];
            // Only the OpenApiProperty description attribute defined on this first usage of this enum type is used.
            // Other OpenApiProperty attributes that are usage-dependant, such as Nullablility or Default, are currently ignored
            // until allOf is implemented
            // (issue https://github.com/Azure/azure-functions-openapi-extension/issues/200#issuecomment-1157298955 )
            rootSchema.Nullable.Should().Be(false);
            (rootSchema.Default as OpenApiString).Value.Should().Be(default(FakeStringEnum).ToDisplayName());
            rootSchema.Description.Should().Be(description);
        }

        [DataTestMethod]
        [DataRow("hello", true, "lorem ipsum")]
        [DataRow("hello", false, "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_Without_Default_When_Visit_Invoked_Then_It_Should_Return_Result(string name, bool nullable, string description)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeLongEnum));
            var attribute = new OpenApiPropertyAttribute() { Nullable = nullable, Description = description };

            this._visitor.Visit(acceptor, type, this._strategy, attribute);
            acceptor.Schemas[name].Reference.Should().NotBeNull();
            var rootSchema = acceptor.RootSchemas[acceptor.Schemas[name].Reference.Id];
            // Only the OpenApiProperty description attribute defined on this first usage of this enum type is used.
            // Other OpenApiProperty attributes that are usage-dependant, such as Nullablility or Default, are currently ignored
            // until allOf is implemented
            // (issue https://github.com/Azure/azure-functions-openapi-extension/issues/200#issuecomment-1157298955 )
            rootSchema.Nullable.Should().Be(false);
            (rootSchema.Default as OpenApiString).Value.Should().Be(default(FakeLongEnum).ToDisplayName());
            rootSchema.Description.Should().Be(description);
        }

        [DataTestMethod]
        [DataRow("hello", OpenApiVisibilityType.Advanced)]
        [DataRow("hello", OpenApiVisibilityType.Important)]
        [DataRow("hello", OpenApiVisibilityType.Internal)]
        public void Given_OpenApiSchemaVisibilityAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(string name, OpenApiVisibilityType visibility)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeStringEnum));
            var attribute = new OpenApiSchemaVisibilityAttribute(visibility);

            this._visitor.Visit(acceptor, type, this._strategy, attribute);
            acceptor.Schemas[name].Reference.Should().NotBeNull();
            var rootSchema = acceptor.RootSchemas[acceptor.Schemas[name].Reference.Id];

            rootSchema.Extensions.Should().ContainKey("x-ms-visibility");
            rootSchema.Extensions["x-ms-visibility"].Should().BeOfType<OpenApiString>();
            (rootSchema.Extensions["x-ms-visibility"] as OpenApiString).Value.Should().Be(visibility.ToDisplayName(this._strategy));
        }

        [DataTestMethod]
        [DataRow("string", null, typeof(FakeStringEnum))]
        public void Given_Type_When_ParameterVisit_Invoked_Then_It_Should_Return_Result(string dataType, string dataFormat, Type enumType)
        {
            var enums = enumType.ToOpenApiStringCollection(this._strategy);

            var result = this._visitor.ParameterVisit(typeof(FakeStringEnum), this._strategy);

            result.Type.Should().Be(dataType);
            result.Format.Should().Be(dataFormat);

            for (var i = 0; i < result.Enum.Count; i++)
            {
                var @enum = result.Enum[i];
                @enum.Should().BeOfType<OpenApiString>();
                (@enum as OpenApiString).Value.Should().Be((enums[i] as OpenApiString).Value);
            }

            (result.Default as OpenApiString).Value.Should().Be((enums.First() as OpenApiString).Value);
        }

        [DataTestMethod]
        [DataRow("string", null, typeof(FakeStringEnum))]
        public void Given_Type_When_PayloadVisit_Invoked_Then_It_Should_Return_Result(string dataType, string dataFormat, Type enumType)
        {
            var enums = enumType.ToOpenApiStringCollection(this._strategy);

            var result = this._visitor.PayloadVisit(typeof(FakeStringEnum), this._strategy);

            result.Type.Should().Be(dataType);
            result.Format.Should().Be(dataFormat);

            for (var i = 0; i < result.Enum.Count; i++)
            {
                var @enum = result.Enum[i];
                @enum.Should().BeOfType<OpenApiString>();
                (@enum as OpenApiString).Value.Should().Be((enums[i] as OpenApiString).Value);
            }

            (result.Default as OpenApiString).Value.Should().Be((enums.First() as OpenApiString).Value);
        }
    }
}
