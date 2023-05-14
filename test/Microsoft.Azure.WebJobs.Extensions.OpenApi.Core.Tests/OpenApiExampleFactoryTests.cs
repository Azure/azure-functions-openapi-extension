using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.OpenApi.Any;
using FluentAssertions;

using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
{
    [TestClass]
    public class OpenApiExampleFactoryTests
    {
        [DataTestMethod]
        [DataRow((Int16)0, (Int16)0)]
        [DataRow((Int16)32767, (Int16)32767)]
        [DataRow((Int16)(-32768), (Int16)(-32768))]
        public void Given_Int16_When_Instantiated_It_Should_be_Int16(Int16 input, Int16 expectedValue)
        {
            // Arrange
            var expected = new OpenApiInteger(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);

        }

        [DataTestMethod]
        [DataRow((Int32)0, (Int32)0)]
        [DataRow((Int32)32767, (Int32)32767)]
        [DataRow((Int32)(-32768), (Int32)(-32768))]
        public void Given_Int32_When_Instantiated_It_Should_be_Int32(Int32 input, Int32 expectedValue)
        {
             // Arrange
            var expected = new OpenApiInteger(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((Int64)0, (Int64)0)]
        [DataRow((Int64)32767, (Int64)32767)]
        [DataRow((Int64)(-32768), (Int64)(-32768))]
        public void Given_Int64_When_Instantiated_It_Should_be_Int64(Int64 input, Int64 expectedValue)
        {
             // Arrange
            var expected = new OpenApiLong(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((UInt16)0, (UInt16)0)]
        [DataRow((UInt16)32767, (UInt16)32767)]
        [DataRow((UInt16)1000, (UInt16)1000)]
        public void Given_UInt16_When_Instantiated_It_Should_be_UInt16(UInt16 input, UInt16 expectedValue)
        {
             // Arrange
            var expected = new OpenApiDouble(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((UInt32)0, (UInt32)0)]
        [DataRow((UInt32)32767, (UInt32)32767)]
        [DataRow((UInt32)10000000, (UInt32)10000000)]
        public void Given_UInt32_When_Instantiated_It_Should_be_UInt32(UInt32 input, UInt32 expectedValue)
        {
             // Arrange
            var expected = new OpenApiDouble(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((UInt64)0, (UInt64)0)]
        [DataRow((UInt64)32767, (UInt64)32767)]
        [DataRow((UInt64)1000000000, (UInt64)1000000000)]
        public void Given_UInt64_When_Instantiated_It_Should_be_UInt64(UInt64 input, UInt64 expectedValue)
        {
             // Arrange
            var expected = new OpenApiDouble(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }


        [DataTestMethod]
        [DataRow((float)0, (float)0)]
        [DataRow((float)32767, (float)32767)]
        [DataRow((float)10.00, (float)10.00)]
        public void Given_Single_When_Instantiated_It_Should_be_Single(float input, float expectedValue)
        {
             // Arrange
            var expected = new OpenApiFloat(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((Double)0, (Double)0)]
        [DataRow((Double)32761231237, (Double)32761231237)]
        [DataRow((Double)1012.1200, (Double)1012.1200)]
        public void Given_Double_When_Instantiated_It_Should_be_Double(Double input, Double expectedValue)
        {
              // Arrange
            var expected = new OpenApiDouble(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((Boolean)true, (Boolean)true)]
        [DataRow((Boolean)false, (Boolean)false)]
        public void Given_Boolean_When_Instantiated_It_Should_be_Boolean(Boolean input, Boolean expectedValue)
        {
             // Arrange
            var expected = new OpenApiBoolean(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((String)"test", (String)"test")]
        [DataRow((String)"abcde", (String)"abcde")]
        public void Given_String_When_Instantiated_It_Should_be_String(String input, String expectedValue)
        {
             // Arrange
            var expected = new OpenApiString(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        // [DataTestMethod]
        // [DataRow(0UL, "0001-01-01T00:00:00Z")]
        // [DataRow(637552816000000000UL, "2021-09-01T00:00:00Z")]
        // public void Given_DateTime_When_Instantiated_It_Should_be_DateTime(ulong input, string expectedValue)
        // {
        //     // Arrange
        //     var expected = new OpenApiDateTime(DateTimeOffset.Parse(expectedValue));

        //     // Act
        //     var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

        //     // Assert
        //     result.Should().BeEquivalentTo(expected);
        // }

        // [DataTestMethod]
        // [DataRow("2023-05-13T10:00:00+09:00", "2023-05-13T10:00:00+09:00")]
        // [DataRow("2022-12-31T23:59:59-08:00", "2022-12-31T23:59:59-08:00")]
        // public void Given_DateTimeOffset_When_Instantiated_It_Should_be_OpenApiDateTimeOffset(string input, string expectedValue)
        // {
        //     // Arrange
        //     var inputDateTimeOffset = DateTimeOffset.Parse(input);
        //     var expectedDateTimeOffset = DateTimeOffset.Parse(expectedValue);
        //     var expected = new OpenApiDateTime(expectedDateTimeOffset);

        //     // Act
        //     var result = OpenApiExampleFactory.CreateInstance(inputDateTimeOffset, new JsonSerializerSettings());

        //     // Assert
        //     result.Should().BeEquivalentTo(expected);
        // }

        // [DataTestMethod]
        // [DataRow("c9da6455-213d-42c9-9a79-3f9149a57833")]
        // [DataRow("3f3e3c3b-9f0f-4b5e-bb32-7f5e5c7d7c5c")]
        // [DataRow("7e9e2b6f-156d-4d6c-95db-3132a4bfb4e4")]
        // public void Given_ObjectGuid_When_Instantiated_It_Should_be_ObjectGuid(string input)
        // {
        // // Arrange
        // var expected = new OpenApiString(input);
        // // Act
        // var result = OpenApiExampleFactory.CreateInstance(Guid.Parse(input), new JsonSerializerSettings());

        // // Assert
        // result.Should().BeEquivalentTo(expected);
        // }

        // [DataTestMethod]
        // [DataRow(new byte[] { }, "")]
        // [DataRow(new byte[] { 1, 2, 3 }, "AQID")]
        // [DataRow(new byte[] { 255, 128, 0 }, "/4A=")]
        // public void Given_ObjectByte_When_Instantiated_It_Should_be_ObjectByte(byte[] input, string expectedValue)
        // {
        //     // Arrange
        //     var expected = new OpenApiString(expectedValue);

        //     // Act
        //     var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

        //     // Assert
        //     result.Should().BeEquivalentTo(expected);
        // }


        // [DataTestMethod]
        // [DataRow("{\"Name\":\"Alice\",\"Age\":30}", "{\"Name\":\"Alice\",\"Age\":30}")]
        // [DataRow("{\"Name\":\"Bob\",\"Age\":25}", "{\"Name\":\"Bob\",\"Age\":25}")]
        // public void Given_Object_When_Instantiated_It_Should_be_Object(string input, string expectedValue)
        // {
        //     // Arrange
        //     var expected = new OpenApiString(expectedValue);

        //     // Act
        //     var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

        //     // Assert
        //     result.Should().BeEquivalentTo(expected);
        // }

        // [DataTestMethod]
        // [DataRow("")]
        // [DataRow(null)]
        // public void Given_default_When_Instantiated_It_Should_throw_InvalidOperationException(string input)
        // {
        // // Act & Assert
        // Action action = () => OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());
        // action.Should().Throw<InvalidOperationException>();
        // }
    }
}
