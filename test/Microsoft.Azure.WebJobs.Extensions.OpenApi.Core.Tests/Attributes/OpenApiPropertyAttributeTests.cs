using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Attributes
{
    [TestClass]
    public class OpenApiPropertyAttributeTests
    {
        [DataTestMethod]
        [DataRow(true, 1, "hello world")]
        public void Given_Value_Property_Should_Return_Value(bool nullable, object @default, string description)
        {
            var attribute = new OpenApiPropertyAttribute()
            {
                Nullable  = nullable,
                Default = @default,
                Description = description
            };

            attribute.Nullable.Should().Be(nullable);
            attribute.Default.Should().Be(@default);
            attribute.Description.Should().Be(description);
        }
    }
}
