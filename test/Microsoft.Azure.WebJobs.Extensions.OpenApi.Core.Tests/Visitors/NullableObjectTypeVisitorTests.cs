using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors;
using Microsoft.OpenApi.Any;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Visitors
{
    [TestClass]
    public class NullableObjectTypeVisitorTests
    {
        private VisitorCollection _visitorCollection;
        private IVisitor _visitor;
        private NamingStrategy _strategy;
        private OpenApiNamespaceType _namespaceType;

        [TestInitialize]
        public void Init()
        {
            this._visitorCollection = VisitorCollection.CreateInstance();
            this._visitor = new NullableObjectTypeVisitor(this._visitorCollection);
            this._strategy = new CamelCaseNamingStrategy();
            this._namespaceType = OpenApiNamespaceType.ShortName;
        }

        [DataTestMethod]
        [DataRow(typeof(DateTime?), false)]
        [DataRow(typeof(string), false)]
        public void Given_Type_When_IsNavigatable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsNavigatable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(DateTime?), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(DateTime?), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsParameterVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsParameterVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(DateTime?), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsPayloadVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsPayloadVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(DataType.DateTime, "string", "hello", "date-time")]
        [DataRow(DataType.Date, "string", "hello", "date")]
        public void Given_DataTypeAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(DataType objectType, string dataType, string name, string expected)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(DateTime?));
            var attribute = new DataTypeAttribute(objectType);

            this._visitor.Visit(acceptor, type, this._strategy, this._namespaceType, attribute);

            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Type.Should().Be(dataType);
            acceptor.Schemas[name].Format.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(DateTime?), "string", "date-time", true)]
        [DataRow(typeof(int?), "integer", "int32", true)]
        public void Given_Type_When_Visit_Invoked_Then_It_Should_Return_Result(Type objectType, string dataType, string dataFormat, bool schemaNullable)
        {
            var name = "hello";
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, objectType);

            this._visitor.Visit(acceptor, type, this._strategy, this._namespaceType);

            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Type.Should().Be(dataType);
            acceptor.Schemas[name].Format.Should().Be(dataFormat);
            acceptor.Schemas[name].Nullable.Should().Be(schemaNullable);
        }

        [DataTestMethod]
        [DataRow("hello", "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(string name, string description)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(DateTime?));
            var attribute = new OpenApiPropertyAttribute() { Description = description };

            this._visitor.Visit(acceptor, type, this._strategy, this._namespaceType, attribute);

            acceptor.Schemas[name].Nullable.Should().Be(false);
            acceptor.Schemas[name].Default.Should().BeNull();
            acceptor.Schemas[name].Description.Should().Be(description);
        }

        [DataTestMethod]
        [DataRow(typeof(DateTime?), true, "hello", "lorem ipsum")]
        [DataRow(typeof(int?), true, "hello", "lorem ipsum")]
        [DataRow(typeof(DateTime?), false, "hello", "lorem ipsum")]
        [DataRow(typeof(int?), false, "hello", "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(Type objectType, bool nullable, string name, string description)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, objectType);
            var attribute = new OpenApiPropertyAttribute() { Nullable = nullable, Description = description };

            this._visitor.Visit(acceptor, type, this._strategy, this._namespaceType, attribute);

            acceptor.Schemas[name].Nullable.Should().Be(nullable);
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
            var type = new KeyValuePair<string, Type>(name, typeof(DateTime?));
            var attribute = new OpenApiSchemaVisibilityAttribute(visibility);

            this._visitor.Visit(acceptor, type, this._strategy, this._namespaceType, attribute);

            acceptor.Schemas[name].Extensions.Should().ContainKey("x-ms-visibility");
            acceptor.Schemas[name].Extensions["x-ms-visibility"].Should().BeOfType<OpenApiString>();
            (acceptor.Schemas[name].Extensions["x-ms-visibility"] as OpenApiString).Value.Should().Be(visibility.ToDisplayName(this._strategy));
        }

        [DataTestMethod]
        [DataRow(typeof(DateTime?), "string", "date-time", true)]
        [DataRow(typeof(int?), "integer", "int32", true)]
        public void Given_Type_When_ParameterVisit_Invoked_Then_It_Should_Return_Result(Type objectType, string dataType, string dataFormat, bool schemaNullable)
        {
            var result = this._visitor.ParameterVisit(objectType, this._strategy, this._namespaceType);

            result.Type.Should().Be(dataType);
            result.Format.Should().Be(dataFormat);
            result.Nullable.Should().Be(schemaNullable);
        }

        [DataTestMethod]
        [DataRow(typeof(DateTime?), "string", "date-time", true)]
        [DataRow(typeof(int?), "integer", "int32", true)]
        public void Given_Type_When_PayloadVisit_Invoked_Then_It_Should_Return_Result(Type objectType, string dataType, string dataFormat, bool schemaNullable)
        {
            var result = this._visitor.PayloadVisit(objectType, this._strategy, this._namespaceType);

            result.Type.Should().Be(dataType);
            result.Format.Should().Be(dataFormat);
            result.Nullable.Should().Be(schemaNullable);
        }
    }
}
