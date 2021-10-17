using System;
using System.Collections.Generic;

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
    public class ExceptionTypeVisitorTests
    {
        private VisitorCollection _visitorCollection;
        private IVisitor _visitor;
        private NamingStrategy _strategy;

        [TestInitialize]
        public void Init()
        {
            this._visitorCollection = new VisitorCollection();
            this._visitor = new ExceptionTypeVisitor(this._visitorCollection);
            this._strategy = new CamelCaseNamingStrategy();
        }


        [DataTestMethod]
        [DataRow(typeof(Exception), false)]
        public void Given_Type_When_IsNavigatable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsNavigatable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Exception), true)]
        [DataRow(typeof(StackOverflowException), true)]
        [DataRow(typeof(AggregateException), true)]
        [DataRow(typeof(ArgumentException), true)]
        [DataRow(typeof(ArgumentNullException), true)]
        [DataRow(typeof(ArgumentOutOfRangeException), true)]
        [DataRow(typeof(InvalidCastException), true)]
        [DataRow(typeof(InvalidOperationException), true)]
        [DataRow(typeof(object), false)]
        public void Given_Type_When_IsVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Exception), false)]
        [DataRow(typeof(StackOverflowException), false)]
        [DataRow(typeof(AggregateException), false)]
        [DataRow(typeof(ArgumentException), false)]
        [DataRow(typeof(ArgumentNullException), false)]
        [DataRow(typeof(ArgumentOutOfRangeException), false)]
        [DataRow(typeof(InvalidCastException), false)]
        [DataRow(typeof(InvalidOperationException), false)]
        [DataRow(typeof(object), false)]
        public void Given_Type_When_IsParameterVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsParameterVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Exception), true)]
        [DataRow(typeof(StackOverflowException), true)]
        [DataRow(typeof(AggregateException), true)]
        [DataRow(typeof(ArgumentException), true)]
        [DataRow(typeof(ArgumentNullException), true)]
        [DataRow(typeof(ArgumentOutOfRangeException), true)]
        [DataRow(typeof(InvalidCastException), true)]
        [DataRow(typeof(InvalidOperationException), true)]
        [DataRow(typeof(object), false)]
        public void Given_Type_When_IsPayloadVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsPayloadVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(Exception), "object", null, "exception")]
        [DataRow(typeof(StackOverflowException), "object", null, "stackOverflowException")]
        [DataRow(typeof(AggregateException), "object", null, "aggregateException")]
        [DataRow(typeof(ArgumentException), "object", null, "argumentException")]
        [DataRow(typeof(ArgumentNullException), "object", null, "argumentNullException")]
        [DataRow(typeof(ArgumentOutOfRangeException), "object", null, "argumentOutOfRangeException")]
        [DataRow(typeof(InvalidCastException), "object", null, "invalidCastException")]
        [DataRow(typeof(InvalidOperationException), "object", null, "invalidOperationException")]
        public void Given_Type_When_Visit_Invoked_Then_It_Should_Return_Result(Type listType, string dataType, string dataFormat, string id)
        {
            var name = "hello";
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, listType);

            this._visitor.Visit(acceptor, type, this._strategy);

            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Type.Should().Be(dataType);
            acceptor.Schemas[name].Format.Should().Be(dataFormat);
        }

        [DataTestMethod]
        [DataRow("hello", "lorem ipsum")]
        public void Given_OpenApiPropertyAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(string name, string description)
        {
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(List<string>));
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
            var type = new KeyValuePair<string, Type>(name, typeof(List<string>));
            var attribute = new OpenApiSchemaVisibilityAttribute(visibility);

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas[name].Extensions.Should().ContainKey("x-ms-visibility");
            acceptor.Schemas[name].Extensions["x-ms-visibility"].Should().BeOfType<OpenApiString>();
            (acceptor.Schemas[name].Extensions["x-ms-visibility"] as OpenApiString).Value.Should().Be(visibility.ToDisplayName(this._strategy));
        }
    }
}

