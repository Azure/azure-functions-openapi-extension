using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using FluentAssertions;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Tests.Extensions
{
    [TestClass]
    public class OpenApiSchemaExtensionsTests
    {
        [DataTestMethod]
        [DataRow("array", null, false, false, false)]
        [DataRow("object", "uri", false, false, false)]
        [DataRow("object", null, true, false, false)]
        [DataRow("object", null, false, true, false)]
        [DataRow("object", null, false, false, true)]
        public void Given_Instance_When_IsOpenApiSchemaObject_Invoked_Then_It_Should_Return_Result(string type, string format, bool items, bool additionalProperties, bool expected)
        {
            var schema = new OpenApiSchema()
            {
                Type = type,
                Format = format,
                Items = items ? new OpenApiSchema() : null,
                AdditionalProperties = additionalProperties ? new OpenApiSchema() : null,
            };

            var result = OpenApiSchemaExtensions.IsOpenApiSchemaObject(schema);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("object", null, false, false, false)]
        [DataRow("array", "uri", false, false, false)]
        [DataRow("array", null, true, false, true)]
        [DataRow("array", null, false, true, false)]
        [DataRow("array", null, false, false, false)]
        public void Given_Instance_When_IsOpenApiSchemaArray_Invoked_Then_It_Should_Return_Result(string type, string format, bool items, bool additionalProperties, bool expected)
        {
            var schema = new OpenApiSchema()
            {
                Type = type,
                Format = format,
                Items = items ? new OpenApiSchema() : null,
                AdditionalProperties = additionalProperties ? new OpenApiSchema() : null,
            };

            var result = OpenApiSchemaExtensions.IsOpenApiSchemaArray(schema);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("array", null, false, false, false)]
        [DataRow("object", "uri", false, false, false)]
        [DataRow("object", null, true, false, false)]
        [DataRow("object", null, false, true, true)]
        [DataRow("object", null, false, false, false)]
        public void Given_Instance_When_IsOpenApiSchemaDictionary_Invoked_Then_It_Should_Return_Result(string type, string format, bool items, bool additionalProperties, bool expected)
        {
            var schema = new OpenApiSchema()
            {
                Type = type,
                Format = format,
                Items = items ? new OpenApiSchema() : null,
                AdditionalProperties = additionalProperties ? new OpenApiSchema() : null,
            };

            var result = OpenApiSchemaExtensions.IsOpenApiSchemaDictionary(schema);

            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(DataType.DateTime, "date-time")]
        [DataRow(DataType.Date, "date")]
        [DataRow(DataType.Time, "time")]
        [DataRow(DataType.Duration, "duration")]
        [DataRow(DataType.PhoneNumber, "tel")]
        [DataRow(DataType.Currency, "currency")]
        [DataRow(DataType.Text, "string")]
        [DataRow(DataType.Html, "html")]
        [DataRow(DataType.MultilineText, "multiline")]
        [DataRow(DataType.EmailAddress, "email")]
        [DataRow(DataType.Password, "password")]
        [DataRow(DataType.Url, "uri")]
        [DataRow(DataType.ImageUrl, "uri")]
        [DataRow(DataType.CreditCard, "credit-card")]
        [DataRow(DataType.PostalCode, "postal-code")]
        public void Given_MinLengthAttribute_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result(DataType dataType, string expected)
        {
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new DataTypeAttribute(dataType)
            };

            schema.ApplyValidationAttributes(attributes);

            schema.Format.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(10)]
        public void Given_MinLengthAttribute_With_StringType_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result(int length)
        {
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new MinLengthAttribute(length)
            };

            schema.ApplyValidationAttributes(attributes);

            schema.MinLength.Should().Be(length);
        }

        [DataTestMethod]
        [DataRow(10)]
        public void Given_MinLengthAttribute_With_ArrayType_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result(int length)
        {
            var schema = new OpenApiSchema
            {
                Type = "array",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new MinLengthAttribute(length)
            };

            schema.ApplyValidationAttributes(attributes);

            schema.MinItems.Should().Be(length);
        }

        [DataTestMethod]
        [DataRow(15)]
        public void Given_MaxLengthAttribute_With_StringType_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result(int length)
        {
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new MaxLengthAttribute(length)
            };

            schema.ApplyValidationAttributes(attributes);

            schema.MaxLength.Should().Be(length);
        }

        [DataTestMethod]
        [DataRow(15)]
        public void Given_MaxLengthAttribute_With_ArrayType_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result(int length)
        {
            var schema = new OpenApiSchema
            {
                Type = "array",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new MaxLengthAttribute(length)
            };

            schema.ApplyValidationAttributes(attributes);

            schema.MaxItems.Should().Be(length);
        }

        [DataTestMethod]
        [DataRow(10, 15)]
        public void Given_RangeAttribute_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result(int min, int max)
        {
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new RangeAttribute(min, max)
            };

            schema.ApplyValidationAttributes(attributes);

            schema.Minimum.Should().Be(min);
            schema.Maximum.Should().Be(max);
        }

        [DataTestMethod]
        [DataRow("helloworld")]
        public void Given_RegularExpressionAttribute_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result(string pattern)
        {
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new RegularExpressionAttribute(pattern)
            };

            schema.ApplyValidationAttributes(attributes);

            schema.Pattern.Should().Be(pattern);
        }

        [TestMethod]
        public void Given_StringLengthAttribute_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result()
        {
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new StringLengthAttribute(15)
                {
                    MinimumLength = 10
                },
            };

            schema.ApplyValidationAttributes(attributes);

            schema.MinLength.Should().Be(10);
            schema.MaxLength.Should().Be(15);
        }

        [TestMethod]
        public void Given_RequiredAttribute_WithStringType_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result()
        {
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new RequiredAttribute()
                {
                    AllowEmptyStrings = false
                },
            };

            schema.ApplyValidationAttributes(attributes);

            schema.MinLength.Should().Be(1);
        }

        [TestMethod]
        public void Given_RequiredAttribute_WithMinLength_WithStringType_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result()
        {
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = null,
                Items = null,
                MinLength = 5,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new RequiredAttribute()
                {
                    AllowEmptyStrings = false
                },
            };

            schema.ApplyValidationAttributes(attributes);

            schema.MinLength.Should().Be(5);
        }

        [TestMethod]
        public void Given_RequiredAttributeAllowEmptyStrings_WithStringType_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result()
        {
            var schema = new OpenApiSchema
            {
                Type = "string",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new RequiredAttribute()
                {
                    AllowEmptyStrings = true
                },
            };

            schema.ApplyValidationAttributes(attributes);

            schema.MinLength.Should().BeNull();
        }

        [TestMethod]
        public void Given_RequiredAttribute_NoStringType_When_ApplyValidationAttributes_Invoked_Then_It_Should_Return_Result()
        {
            var schema = new OpenApiSchema
            {
                Type = "object",
                Format = null,
                Items = null,
            };

            var attributes = new List<ValidationAttribute>()
            {
                new RequiredAttribute()
                {
                    AllowEmptyStrings = false
                },
            };

            schema.ApplyValidationAttributes(attributes);

            schema.MinLength.Should().BeNull();
        }
    }
}
