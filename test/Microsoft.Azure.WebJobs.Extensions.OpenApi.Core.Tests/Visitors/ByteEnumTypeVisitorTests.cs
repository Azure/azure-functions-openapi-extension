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
    public class ByteEnumTypeVisitorTests
    {
        private VisitorCollection _visitorCollection;
        private IVisitor _visitor;
        private NamingStrategy _strategy;

        [TestInitialize]
        public void Init()
        {
            this._visitorCollection = new VisitorCollection();
            this._visitor = new ByteEnumTypeVisitor(this._visitorCollection);
            this._strategy = new CamelCaseNamingStrategy();
        }

        [DataTestMethod]
        [DataRow(typeof(FakeByteEnum), false)]
        public void Given_Type_When_IsNavigatable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsNavigatable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeByteEnum), true)]
        [DataRow(typeof(byte), false)]
        public void Given_Type_When_IsVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeByteEnum), true)]
        [DataRow(typeof(byte), false)]
        public void Given_Type_When_IsParameterVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsParameterVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(FakeByteEnum), true)]
        [DataRow(typeof(byte), false)]
        public void Given_Type_When_IsPayloadVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsPayloadVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("integer", "int32", typeof(FakeByteEnum))]
        public void Given_Type_When_Visit_Invoked_Then_It_Should_Return_Result(string dataType, string dataFormat, Type enumType)
        {
            var name = "hello";
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeByteEnum));
            var enums = enumType.ToOpenApiByteCollection();

            this._visitor.Visit(acceptor, type, this._strategy);

            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Type.Should().Be(dataType);
            acceptor.Schemas[name].Format.Should().Be(dataFormat);

            for (var i = 0; i < acceptor.Schemas[name].Enum.Count; i++)
            {
                var @enum = acceptor.Schemas[name].Enum[i];
                @enum.Should().BeOfType<OpenApiInteger>();
                (@enum as OpenApiInteger).Value.Should().Be((enums[i] as OpenApiInteger).Value);
            }

            (acceptor.Schemas[name].Default as OpenApiInteger).Value.Should().Be((enums.First() as OpenApiInteger).Value);
        }

        [DataTestMethod]
        [DataRow("hello", "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(string name, string description)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeByteEnum));
            var attribute = new OpenApiPropertyAttribute() { Description = description };

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas[name].Nullable.Should().Be(false);
            acceptor.Schemas[name].Default.Should().BeNull();
            acceptor.Schemas[name].Description.Should().Be(description);
        }

        [DataTestMethod]
        [DataRow("hello", true, "lorem ipsum")]
        [DataRow("hello", false, "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_With_Default_When_Visit_Invoked_Then_It_Should_Return_Result(string name, bool nullable, string description)
        {
            var @default = (byte)FakeByteEnum.ByteValue1;
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeByteEnum));
            var attribute = new OpenApiPropertyAttribute() { Nullable = nullable, Default = @default, Description = description };

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas[name].Nullable.Should().Be(nullable);
            acceptor.Schemas[name].Default.Should().NotBeNull();
            (acceptor.Schemas[name].Default as OpenApiInteger).Value.Should().Be((int)@default);
            acceptor.Schemas[name].Description.Should().Be(description);
        }

        [DataTestMethod]
        [DataRow("hello", true, "lorem ipsum")]
        [DataRow("hello", false, "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_Without_Default_When_Visit_Invoked_Then_It_Should_Return_Result(string name, bool nullable, string description)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(FakeByteEnum));
            var attribute = new OpenApiPropertyAttribute() { Nullable = nullable, Description = description };

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

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
            var type = new KeyValuePair<string, Type>(name, typeof(FakeByteEnum));
            var attribute = new OpenApiSchemaVisibilityAttribute(visibility);

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas[name].Extensions.Should().ContainKey("x-ms-visibility");
            acceptor.Schemas[name].Extensions["x-ms-visibility"].Should().BeOfType<OpenApiString>();
            (acceptor.Schemas[name].Extensions["x-ms-visibility"] as OpenApiString).Value.Should().Be(visibility.ToDisplayName(this._strategy));
        }

        [DataTestMethod]
        [DataRow("integer", "int32", typeof(FakeByteEnum))]
        public void Given_Type_When_ParameterVisit_Invoked_Then_It_Should_Return_Result(string dataType, string dataFormat, Type enumType)
        {
            var enums = enumType.ToOpenApiByteCollection();

            var result = this._visitor.ParameterVisit(typeof(FakeByteEnum), this._strategy);

            result.Type.Should().Be(dataType);
            result.Format.Should().Be(dataFormat);

            for (var i = 0; i < result.Enum.Count; i++)
            {
                var @enum = result.Enum[i];
                @enum.Should().BeOfType<OpenApiInteger>();
                (@enum as OpenApiInteger).Value.Should().Be((enums[i] as OpenApiInteger).Value);
            }

            (result.Default as OpenApiInteger).Value.Should().Be((enums.First() as OpenApiInteger).Value);
        }

        [DataTestMethod]
        [DataRow("integer", "int32", typeof(FakeByteEnum))]
        public void Given_Type_When_PayloadVisit_Invoked_Then_It_Should_Return_Result(string dataType, string dataFormat, Type enumType)
        {
            var enums = enumType.ToOpenApiByteCollection();

            var result = this._visitor.PayloadVisit(typeof(FakeByteEnum), this._strategy);

            result.Type.Should().Be(dataType);
            result.Format.Should().Be(dataFormat);

            for (var i = 0; i < result.Enum.Count; i++)
            {
                var @enum = result.Enum[i];
                @enum.Should().BeOfType<OpenApiInteger>();
                (@enum as OpenApiInteger).Value.Should().Be((enums[i] as OpenApiInteger).Value);
            }

            (result.Default as OpenApiInteger).Value.Should().Be((enums.First() as OpenApiInteger).Value);
        }
    }
}
