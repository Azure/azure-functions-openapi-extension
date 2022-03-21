using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiPropertyAttributeTests
    {
        [DataTestMethod]
        [DataRow(true, 1, "hello world", false)]
        public void Given_Value_Property_Should_Return_Value(bool nullable, object @default, string description, bool deprecated)
        {
            var attribute = new OpenApiPropertyAttribute()
            {
                Nullable  = nullable,
                Default = @default,
                Description = description,
                Deprecated = deprecated
            };

            attribute.Nullable.Should().Be(nullable);
            attribute.Default.Should().Be(@default);
            attribute.Description.Should().Be(description);
            attribute.Deprecated.Should().Be(deprecated);
        }
    }
}
