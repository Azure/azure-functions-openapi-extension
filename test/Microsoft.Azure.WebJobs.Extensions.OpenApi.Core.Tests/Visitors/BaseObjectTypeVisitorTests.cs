using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    public class BaseObjectTypeVisitorTests
    {
        private VisitorCollection _visitorCollection;
        private IVisitor _visitor;
        private NamingStrategy _strategy;

        [TestInitialize]
        public void Init()
        {
            this._visitorCollection = new VisitorCollection();
            this._visitor = new BaseObjectTypeVisitor(this._visitorCollection);
            this._strategy = new CamelCaseNamingStrategy();
        }

        [DataTestMethod]
        [DataRow(typeof(object), true)]
        [DataRow(typeof(FakeModel), false)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(object), true)]
        [DataRow(typeof(FakeModel), false)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsParameterVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsParameterVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(object), true)]
        [DataRow(typeof(FakeModel), false)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsPayloadVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsPayloadVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("object")]
        public void Given_Type_When_Visit_Invoked_Then_It_Should_Return_Result(string dataType)
        {
            var name = "object";
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, typeof(object));

            this._visitor.Visit(acceptor, type, this._strategy);

            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Type.Should().Be(dataType);
        }

    }
}
