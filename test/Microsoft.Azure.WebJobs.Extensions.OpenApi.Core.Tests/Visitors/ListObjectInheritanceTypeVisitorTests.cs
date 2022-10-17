using System;
using System.Collections;
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
    public class ListObjectInheritanceTypeVisitorTests
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
        [DataRow(typeof(TestClassListString), true)]
        [DataRow(typeof(TestClassIListString), true)]
        [DataRow(typeof(TestClassICollectionString), true)]
        [DataRow(typeof(TestClassIEnumerableString), true)]
        [DataRow(typeof(TestClassIReadOnlyListString), true)]
        [DataRow(typeof(TestClassIReadOnlyCollectionString), true)]
        [DataRow(typeof(TestClassHashSetString), true)]
        [DataRow(typeof(TestClassISetString), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(TestClassListString), true)]
        [DataRow(typeof(TestClassIListString), true)]
        [DataRow(typeof(TestClassICollectionString), true)]
        [DataRow(typeof(TestClassIEnumerableString), true)]
        [DataRow(typeof(TestClassIReadOnlyListString), true)]
        [DataRow(typeof(TestClassIReadOnlyCollectionString), true)]
        [DataRow(typeof(TestClassHashSetString), true)]
        [DataRow(typeof(TestClassISetString), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsParameterVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsParameterVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(TestClassListString), true)]
        [DataRow(typeof(TestClassIListString), true)]
        [DataRow(typeof(TestClassICollectionString), true)]
        [DataRow(typeof(TestClassIEnumerableString), true)]
        [DataRow(typeof(TestClassIReadOnlyListString), true)]
        [DataRow(typeof(TestClassIReadOnlyCollectionString), true)]
        [DataRow(typeof(TestClassHashSetString), true)]
        [DataRow(typeof(TestClassISetString), true)]
        [DataRow(typeof(int), false)]
        public void Given_Type_When_IsPayloadVisitable_Invoked_Then_It_Should_Return_Result(Type type, bool expected)
        {
            var result = this._visitor.IsPayloadVisitable(type);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(typeof(TestClassListString), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(TestClassIListString), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(TestClassICollectionString), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(TestClassIEnumerableString), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(TestClassIReadOnlyListString), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(TestClassIReadOnlyCollectionString), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(TestClassHashSetString), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(TestClassISetString), "array", null, "string", false, "string", 0)]
        [DataRow(typeof(TestClassListFakeModel), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(TestClassIListFakeModel), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(TestClassICollectionFakeModel), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(TestClassIEnumerableFakeModel), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(TestClassIReadOnlyListFakeModel), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(TestClassIReadOnlyCollectionFakeModel), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(TestClassHashSetFakeModel), "array", null, "object", true, "fakeModel", 1)]
        [DataRow(typeof(TestClassISetFakeModel), "array", null, "object", true, "fakeModel", 1)]
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
        [DataRow(typeof(TestClassListString), 1)]
        [DataRow(typeof(TestClassIListString), 1)]
        [DataRow(typeof(TestClassICollectionString), 1)]
        [DataRow(typeof(TestClassIEnumerableString), 1)]
        [DataRow(typeof(TestClassIReadOnlyListString), 1)]
        [DataRow(typeof(TestClassIReadOnlyCollectionString), 1)]
        [DataRow(typeof(TestClassHashSetString), 1)]
        [DataRow(typeof(TestClassISetString), 1)]
        [DataRow(typeof(TestClassListFakeModel), 1)]
        [DataRow(typeof(TestClassIListFakeModel), 1)]
        [DataRow(typeof(TestClassICollectionFakeModel), 1)]
        [DataRow(typeof(TestClassIEnumerableFakeModel), 1)]
        [DataRow(typeof(TestClassIReadOnlyListFakeModel), 1)]
        [DataRow(typeof(TestClassIReadOnlyCollectionFakeModel), 1)]
        [DataRow(typeof(TestClassHashSetFakeModel), 1)]
        [DataRow(typeof(TestClassISetFakeModel), 1)]
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
        [DataRow(typeof(TestClassListString), 10)]
        [DataRow(typeof(TestClassIListString), 10)]
        [DataRow(typeof(TestClassICollectionString), 10)]
        [DataRow(typeof(TestClassIEnumerableString), 10)]
        [DataRow(typeof(TestClassIReadOnlyListString), 10)]
        [DataRow(typeof(TestClassIReadOnlyCollectionString), 10)]
        [DataRow(typeof(TestClassHashSetString), 10)]
        [DataRow(typeof(TestClassISetString), 10)]
        [DataRow(typeof(TestClassListFakeModel), 10)]
        [DataRow(typeof(TestClassIListFakeModel), 10)]
        [DataRow(typeof(TestClassICollectionFakeModel), 10)]
        [DataRow(typeof(TestClassIEnumerableFakeModel), 10)]
        [DataRow(typeof(TestClassIReadOnlyListFakeModel), 10)]
        [DataRow(typeof(TestClassIReadOnlyCollectionFakeModel), 10)]
        [DataRow(typeof(TestClassHashSetFakeModel), 10)]
        [DataRow(typeof(TestClassISetFakeModel), 10)]
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
            var type = new KeyValuePair<string, Type>(name, typeof(TestClassListString));
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
            var type = new KeyValuePair<string, Type>(name, typeof(TestClassListString));
            var attribute = new OpenApiSchemaVisibilityAttribute(visibility);

            this._visitor.Visit(acceptor, type, this._strategy, attribute);

            acceptor.Schemas[name].Extensions.Should().ContainKey("x-ms-visibility");
            acceptor.Schemas[name].Extensions["x-ms-visibility"].Should().BeOfType<OpenApiString>();
            (acceptor.Schemas[name].Extensions["x-ms-visibility"] as OpenApiString).Value.Should().Be(visibility.ToDisplayName(this._strategy));
        }

        [DataTestMethod]
        [DataRow(typeof(TestClassListString), "array", null, "string", false)]
        [DataRow(typeof(TestClassIListString), "array", null, "string", false)]
        [DataRow(typeof(TestClassICollectionString), "array", null, "string", false)]
        [DataRow(typeof(TestClassIEnumerableString), "array", null, "string", false)]
        [DataRow(typeof(TestClassIReadOnlyListString), "array", null, "string", false)]
        [DataRow(typeof(TestClassIReadOnlyCollectionString), "array", null, "string", false)]
        [DataRow(typeof(TestClassHashSetString), "array", null, "string", false)]
        [DataRow(typeof(TestClassISetString), "array", null, "string", false)]
        [DataRow(typeof(TestClassListFakeModel), "array", null, "object", true)]
        [DataRow(typeof(TestClassIListFakeModel), "array", null, "object", true)]
        [DataRow(typeof(TestClassICollectionFakeModel), "array", null, "object", true)]
        [DataRow(typeof(TestClassIEnumerableFakeModel), "array", null, "object", true)]
        [DataRow(typeof(TestClassIReadOnlyListFakeModel), "array", null, "object", true)]
        [DataRow(typeof(TestClassIReadOnlyCollectionFakeModel), "array", null, "object", true)]
        [DataRow(typeof(TestClassHashSetFakeModel), "array", null, "object", true)]
        [DataRow(typeof(TestClassISetFakeModel), "array", null, "object", true)]
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
        [DataRow(typeof(TestClassListString), "array", null, "string", false, null)]
        [DataRow(typeof(TestClassIListString), "array", null, "string", false, null)]
        [DataRow(typeof(TestClassICollectionString), "array", null, "string", false, null)]
        [DataRow(typeof(TestClassIEnumerableString), "array", null, "string", false, null)]
        [DataRow(typeof(TestClassIReadOnlyListString), "array", null, "string", false, null)]
        [DataRow(typeof(TestClassIReadOnlyCollectionString), "array", null, "string", false, null)]
        [DataRow(typeof(TestClassHashSetString), "array", null, "string", false, null)]
        [DataRow(typeof(TestClassISetString), "array", null, "string", false, null)]
        [DataRow(typeof(TestClassListFakeModel), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(TestClassIListFakeModel), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(TestClassICollectionFakeModel), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(TestClassIEnumerableFakeModel), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(TestClassIReadOnlyListFakeModel), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(TestClassIReadOnlyCollectionFakeModel), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(TestClassHashSetFakeModel), "array", null, "object", true, "fakeModel")]
        [DataRow(typeof(TestClassISetFakeModel), "array", null, "object", true, "fakeModel")]
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

        internal class TestClassListString : List<string> { }
        internal class TestClassIListString : IList<string>
        {
            public string this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public int Count => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public void Add(string item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(string item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<string> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public int IndexOf(string item)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, string item)
            {
                throw new NotImplementedException();
            }

            public bool Remove(string item)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassICollectionString : ICollection<string>
        {
            public int Count => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public void Add(string item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(string item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<string> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public bool Remove(string item)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassIEnumerableString : IEnumerable<string>
        {
            public IEnumerator<string> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassIReadOnlyListString : IReadOnlyList<string>
        {
            public string this[int index] => throw new NotImplementedException();

            public int Count => throw new NotImplementedException();

            public IEnumerator<string> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassIReadOnlyCollectionString : IReadOnlyCollection<string>
        {
            public int Count => throw new NotImplementedException();

            public IEnumerator<string> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassHashSetString : HashSet<string> { }
        internal class TestClassISetString : ISet<string>
        {
            public int Count => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public bool Add(string item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(string item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public void ExceptWith(IEnumerable<string> other)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<string> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public void IntersectWith(IEnumerable<string> other)
            {
                throw new NotImplementedException();
            }

            public bool IsProperSubsetOf(IEnumerable<string> other)
            {
                throw new NotImplementedException();
            }

            public bool IsProperSupersetOf(IEnumerable<string> other)
            {
                throw new NotImplementedException();
            }

            public bool IsSubsetOf(IEnumerable<string> other)
            {
                throw new NotImplementedException();
            }

            public bool IsSupersetOf(IEnumerable<string> other)
            {
                throw new NotImplementedException();
            }

            public bool Overlaps(IEnumerable<string> other)
            {
                throw new NotImplementedException();
            }

            public bool Remove(string item)
            {
                throw new NotImplementedException();
            }

            public bool SetEquals(IEnumerable<string> other)
            {
                throw new NotImplementedException();
            }

            public void SymmetricExceptWith(IEnumerable<string> other)
            {
                throw new NotImplementedException();
            }

            public void UnionWith(IEnumerable<string> other)
            {
                throw new NotImplementedException();
            }

            void ICollection<string>.Add(string item)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassListFakeModel : List<FakeModel> { }
        internal class TestClassIListFakeModel : IList<FakeModel>
        {
            public FakeModel this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public int Count => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public void Add(FakeModel item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(FakeModel item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(FakeModel[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<FakeModel> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public int IndexOf(FakeModel item)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, FakeModel item)
            {
                throw new NotImplementedException();
            }

            public bool Remove(FakeModel item)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassICollectionFakeModel : ICollection<FakeModel>
        {
            public int Count => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public void Add(FakeModel item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(FakeModel item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(FakeModel[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<FakeModel> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public bool Remove(FakeModel item)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassIEnumerableFakeModel : IEnumerable<FakeModel>
        {
            public IEnumerator<FakeModel> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassIReadOnlyListFakeModel : IReadOnlyList<FakeModel>
        {
            public FakeModel this[int index] => throw new NotImplementedException();

            public int Count => throw new NotImplementedException();

            public IEnumerator<FakeModel> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassIReadOnlyCollectionFakeModel : IReadOnlyCollection<FakeModel>
        {
            public int Count => throw new NotImplementedException();

            public IEnumerator<FakeModel> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
        internal class TestClassHashSetFakeModel : HashSet<FakeModel> { }
        internal class TestClassISetFakeModel : ISet<FakeModel>
        {
            public int Count => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public bool Add(FakeModel item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(FakeModel item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(FakeModel[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public void ExceptWith(IEnumerable<FakeModel> other)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<FakeModel> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public void IntersectWith(IEnumerable<FakeModel> other)
            {
                throw new NotImplementedException();
            }

            public bool IsProperSubsetOf(IEnumerable<FakeModel> other)
            {
                throw new NotImplementedException();
            }

            public bool IsProperSupersetOf(IEnumerable<FakeModel> other)
            {
                throw new NotImplementedException();
            }

            public bool IsSubsetOf(IEnumerable<FakeModel> other)
            {
                throw new NotImplementedException();
            }

            public bool IsSupersetOf(IEnumerable<FakeModel> other)
            {
                throw new NotImplementedException();
            }

            public bool Overlaps(IEnumerable<FakeModel> other)
            {
                throw new NotImplementedException();
            }

            public bool Remove(FakeModel item)
            {
                throw new NotImplementedException();
            }

            public bool SetEquals(IEnumerable<FakeModel> other)
            {
                throw new NotImplementedException();
            }

            public void SymmetricExceptWith(IEnumerable<FakeModel> other)
            {
                throw new NotImplementedException();
            }

            public void UnionWith(IEnumerable<FakeModel> other)
            {
                throw new NotImplementedException();
            }

            void ICollection<FakeModel>.Add(FakeModel item)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
