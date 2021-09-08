using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class ListObjectTypeVisitorTests
    {
        private VisitorCollection _visitorCollection;
        private IVisitor _visitor;
        private NamingStrategy _strategy;

        [TestInitialize]
        public void Init()
        {
            this._visitorCollection = VisitorCollection.CreateInstance();
            this._visitor = new ListObjectTypeVisitor(this._visitorCollection);
            this._strategy = new CamelCaseNamingStrategy();
        }

        [DataTestMethod]
        [DataRow(typeof(List<string>), false)]
        public void Given_Type_When_IsNavigatable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsNavigatable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(List<string>), true)]
        [DataRow(typeof(IList<string>), true)]
        [DataRow(typeof(ICollection<string>), true)]
        [DataRow(typeof(IEnumerable<string>), true)]
        [DataRow(typeof(IReadOnlyList<string>), true)]
        [DataRow(typeof(IReadOnlyCollection<string>), true)]
        [DataRow(typeof(HashSet<string>), true)]
        [DataRow(typeof(ISet<string>), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(List<string>), true)]
        [DataRow(typeof(IList<string>), true)]
        [DataRow(typeof(ICollection<string>), true)]
        [DataRow(typeof(IEnumerable<string>), true)]
        [DataRow(typeof(IReadOnlyList<string>), true)]
        [DataRow(typeof(IReadOnlyCollection<string>), true)]
        [DataRow(typeof(HashSet<string>), true)]
        [DataRow(typeof(ISet<string>), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsParameterVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsParameterVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(List<string>), true)]
        [DataRow(typeof(IList<string>), true)]
        [DataRow(typeof(ICollection<string>), true)]
        [DataRow(typeof(IEnumerable<string>), true)]
        [DataRow(typeof(IReadOnlyList<string>), true)]
        [DataRow(typeof(IReadOnlyCollection<string>), true)]
        [DataRow(typeof(HashSet<string>), true)]
        [DataRow(typeof(ISet<string>), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsPayloadVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsPayloadVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(List<string>), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(IList<string>), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(ICollection<string>), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(IEnumerable<string>), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(IReadOnlyList<string>), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(IReadOnlyCollection<string>), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(HashSet<string>), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(ISet<string>), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(List<FakeModel>), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(IList<FakeModel>), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(ICollection<FakeModel>), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(IEnumerable<FakeModel>), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(IReadOnlyList<FakeModel>), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(IReadOnlyCollection<FakeModel>), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(HashSet<FakeModel>), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(ISet<FakeModel>), "array", null, "object", true, "fakeModel", 1)]
        public void Given_Type_When_Visit_Invoked_Then_It_Should_Return_Result(Type listType, string dataType, string dataFormat, string itemType, bool isReferential, string referenceId, int expected)
        {
            var name = "hello";
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, listType);

            this._visitor.Visit(acceptor, type, this._strategy);

            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Type.Should().Be(dataType);
            acceptor.Schemas[name].Format.Should().Be(dataFormat);

            acceptor.Schemas[name].Items.Should().NotBeNull();
            acceptor.Schemas[name].Items.Type.Should().Be(itemType);

            if (isReferential)
            {
                acceptor.Schemas[name].Items.Reference.Type.Should().Be(ReferenceType.Schema);
                acceptor.Schemas[name].Items.Reference.Id.Should().Be(referenceId);
            }

            acceptor.RootSchemas.Count(p => p.Key == referenceId).Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(List<string>), 1)]
        [DataRow(typeof(IList<string>), 1)]
        [DataRow(typeof(ICollection<string>), 1)]
        [DataRow(typeof(IEnumerable<string>), 1)]
        [DataRow(typeof(IReadOnlyList<string>), 1)]
        [DataRow(typeof(IReadOnlyCollection<string>), 1)]
        [DataRow(typeof(HashSet<string>), 1)]
        [DataRow(typeof(ISet<string>), 1)]
        [DataRow(typeof(List<FakeModel>), 1)]
        [DataRow(typeof(IList<FakeModel>), 1)]
        [DataRow(typeof(ICollection<FakeModel>), 1)]
        [DataRow(typeof(IEnumerable<FakeModel>), 1)]
        [DataRow(typeof(IReadOnlyList<FakeModel>), 1)]
        [DataRow(typeof(IReadOnlyCollection<FakeModel>), 1)]
        [DataRow(typeof(HashSet<FakeModel>), 1)]
        [DataRow(typeof(ISet<FakeModel>), 1)]
        public void Given_MinLengthAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(Type listType, int length)
        {
            var name = "hello";
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, listType);
            var attribute = new MinLengthAttribute(length);

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Type.Should().Be("array");
            acceptor.Schemas[name].MinItems.Should().Be(length);
        }

        [DataTestMethod]
        [DataRow(typeof(List<string>), 10)]
        [DataRow(typeof(IList<string>), 10)]
        [DataRow(typeof(ICollection<string>), 10)]
        [DataRow(typeof(IEnumerable<string>), 10)]
        [DataRow(typeof(IReadOnlyList<string>), 10)]
        [DataRow(typeof(IReadOnlyCollection<string>), 10)]
        [DataRow(typeof(HashSet<string>), 10)]
        [DataRow(typeof(ISet<string>), 10)]
        [DataRow(typeof(List<FakeModel>), 10)]
        [DataRow(typeof(IList<FakeModel>), 10)]
        [DataRow(typeof(ICollection<FakeModel>), 10)]
        [DataRow(typeof(IEnumerable<FakeModel>), 10)]
        [DataRow(typeof(IReadOnlyList<FakeModel>), 10)]
        [DataRow(typeof(IReadOnlyCollection<FakeModel>), 10)]
        [DataRow(typeof(HashSet<FakeModel>), 10)]
        [DataRow(typeof(ISet<FakeModel>), 10)]
        public void Given_MaxLengthAttribute_When_Visit_Invoked_Then_It_Should_Return_Result(Type listType, int length)
        {
            var name = "hello";
            var acceptor = new OpenApiSchemaAcceptor();
            var type = new KeyValuePair<string, Type>(name, listType);
            var attribute = new MaxLengthAttribute(length);

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas.Should().ContainKey(name);
            acceptor.Schemas[name].Type.Should().Be("array");
            acceptor.Schemas[name].MaxItems.Should().Be(length);
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

        [DataTestMethod]
        [DataRow(typeof(List<string>), "array", null, "string", false)]
        [DataRow(typeof(IList<string>), "array", null, "string", false)]
        [DataRow(typeof(ICollection<string>), "array", null, "string", false)]
        [DataRow(typeof(IEnumerable<string>), "array", null, "string", false)]
        [DataRow(typeof(IReadOnlyList<string>), "array", null, "string", false)]
        [DataRow(typeof(IReadOnlyCollection<string>), "array", null, "string", false)]
        [DataRow(typeof(HashSet<string>), "array", null, "string", false)]
        [DataRow(typeof(ISet<string>), "array", null, "string", false)]
        [DataRow(typeof(List<FakeModel>), "array", null, "object", true)]
        [DataRow(typeof(IList<FakeModel>), "array", null, "object", true)]
        [DataRow(typeof(ICollection<FakeModel>), "array", null, "object", true)]
        [DataRow(typeof(IEnumerable<FakeModel>), "array", null, "object", true)]
        [DataRow(typeof(IReadOnlyList<FakeModel>), "array", null, "object", true)]
        [DataRow(typeof(IReadOnlyCollection<FakeModel>), "array", null, "object", true)]
        [DataRow(typeof(HashSet<FakeModel>), "array", null, "object", true)]
        [DataRow(typeof(ISet<FakeModel>), "array", null, "object", true)]
        public void Given_Type_When_ParameterVisit_Invoked_Then_It_Should_Return_Result(Type listType, string dataType, string dataFormat, string itemType, bool isItemToBeNull)
        {
            var result = this._visitor.ParameterVisit(listType, this._strategy);

            result.Type.Should().Be(dataType);
            result.Format.Should().Be(dataFormat);

            if (!isItemToBeNull)
            {
                result.Items.Should().NotBeNull();
                result.Items.Type.Should().Be(itemType);
            }
        }

        [DataTestMethod]
        [DataRow(typeof(List<string>), "array", null, "string", false, null)]
        [DataRow(typeof(IList<string>), "array", null, "string", false, null)]
        [DataRow(typeof(ICollection<string>), "array", null, "string", false, null)]
        [DataRow(typeof(IEnumerable<string>), "array", null, "string", false, null)]
        [DataRow(typeof(IReadOnlyList<string>), "array", null, "string", false, null)]
        [DataRow(typeof(IReadOnlyCollection<string>), "array", null, "string", false, null)]
        [DataRow(typeof(HashSet<string>), "array", null, "string", false, null)]
        [DataRow(typeof(ISet<string>), "array", null, "string", false, null)]
        [DataRow(typeof(List<FakeModel>), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(IList<FakeModel>), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(ICollection<FakeModel>), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(IEnumerable<FakeModel>), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(IReadOnlyList<FakeModel>), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(IReadOnlyCollection<FakeModel>), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(HashSet<FakeModel>), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(ISet<FakeModel>), "array", null, "object", true, "fakeModel")]
        public void Given_Type_When_PayloadVisit_Invoked_Then_It_Should_Return_Result(Type listType, string dataType, string dataFormat, string itemType, bool reference, string referenceId)
        {
            var result = this._visitor.PayloadVisit(listType, this._strategy);

            result.Type.Should().Be(dataType);
            result.Format.Should().Be(dataFormat);

            result.Items.Should().NotBeNull();
            result.Items.Type.Should().Be(itemType);

            if (reference)
            {
                result.Items.Reference.Type.Should().Be(ReferenceType.Schema);
                result.Items.Reference.Id.Should().Be(referenceId);
            }
            else
            {
                result.Items.Reference.Should().BeNull();
            }
        }
    }
}
