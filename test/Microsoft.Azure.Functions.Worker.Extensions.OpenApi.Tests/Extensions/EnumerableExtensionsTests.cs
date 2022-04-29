using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssertions;

using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Tests.Extensions
{
    [TestClass]
    public class EnumerableExtensionsTests
    {

        [TestMethod]
        public void DistinctBy_Should_Return_Filtered_Result()
        {
            this.TestData.DistinctByProperty(x => x.Bar).Count().Should().Be(7);
            this.TestData.DistinctByProperty(x => x.Foo).Count().Should().Be(5);
            this.TestData.DistinctByProperty(x => x.Id).Count().Should().Be(9);
        }

        private IEnumerable<SomeClass> TestData
        {
            get
            {
                yield return new SomeClass() { Bar = 1, Foo = "Foo1", Id = 1 };
                yield return new SomeClass() { Bar = 2, Foo = "Foo1", Id = 2 };
                yield return new SomeClass() { Bar = 3, Foo = "Foo1", Id = 3 };
                yield return new SomeClass() { Bar = 4, Foo = "Foo2", Id = 4 };
                yield return new SomeClass() { Bar = 5, Foo = "Foo2", Id = 5 };
                yield return new SomeClass() { Bar = 6, Foo = "Foo3", Id = 6 };
                yield return new SomeClass() { Bar = 7, Foo = "Foo4", Id = 7 };
                yield return new SomeClass() { Bar = 7, Foo = "Foo5", Id = 8 };
                yield return new SomeClass() { Bar = 7, Foo = "Foo5", Id = 9 };
            }
        }

        private class SomeClass
        {
            public string Foo { get; set; }
            public int Bar { get; set; }
            public int Id { get; set; }
        }
    }
}
