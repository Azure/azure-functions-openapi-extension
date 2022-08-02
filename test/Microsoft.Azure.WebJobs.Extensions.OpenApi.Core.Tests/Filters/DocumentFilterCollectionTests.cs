using System.Linq;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Filters
{
    [TestClass]
    public class DocumentFilterCollectionTests
    {
        [TestMethod]
        public void Given_Null_When_Instantated_Then_It_Should_Return_Result()
        {
            var collection = new DocumentFilterCollection();

            collection.DocumentFilters.Should().NotBeNull();
            collection.DocumentFilters.Should().HaveCount(0);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void Given_Filters_When_Instantated_Then_It_Should_Return_Result(int count)
        {
            var filters = Enumerable.Range(0, count).Select(i => new Mock<IDocumentFilter>().Object).ToList();
            var collection = new DocumentFilterCollection(filters);

            collection.DocumentFilters.Should().NotBeNull();
            collection.DocumentFilters.Should().HaveCount(count);
        }
    }
}
