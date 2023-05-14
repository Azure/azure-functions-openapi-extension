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
        [DataRow((short)0, (short)0)]
        [DataRow((short)32767, (short)32767)]
        [DataRow((short)(-32768), (short)(-32768))]
        public void Given_Int16_When_Instantiated_It_Should_be_Int16(short input, short expectedValue)
        {
            // Arrange
            var expected = new OpenApiInteger(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);

        }

        [DataTestMethod]
        [DataRow(0, 0)]
        [DataRow(32767, 32767)]
        [DataRow((-32768), (-32768))]
        public void Given_Int32_When_Instantiated_It_Should_be_Int32(int input, int expectedValue)
        {
             // Arrange
            var expected = new OpenApiInteger(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((long)0, (long)0)]
        [DataRow((long)32767, (long)32767)]
        [DataRow((long)(-32768), (long)(-32768))]
        public void Given_Int64_When_Instantiated_It_Should_be_Int64(long input, long expectedValue)
        {
             // Arrange
            var expected = new OpenApiLong(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((ushort)0, (ushort)0)]
        [DataRow((ushort)32767, (ushort)32767)]
        [DataRow((ushort)1000, (ushort)1000)]
        public void Given_UInt16_When_Instantiated_It_Should_be_UInt16(ushort input, ushort expectedValue)
        {
             // Arrange
            var expected = new OpenApiDouble(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((uint)0, (uint)0)]
        [DataRow((uint)32767, (uint)32767)]
        [DataRow((uint)10000000, (uint)10000000)]
        public void Given_UInt32_When_Instantiated_It_Should_be_UInt32(uint input, uint expectedValue)
        {
             // Arrange
            var expected = new OpenApiDouble(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((ulong)0, (ulong)0)]
        [DataRow((ulong)32767, (ulong)32767)]
        [DataRow((ulong)1000000000, (ulong)1000000000)]
        public void Given_UInt64_When_Instantiated_It_Should_be_UInt64(ulong input, ulong expectedValue)
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
        [DataRow((double)0, (double)0)]
        [DataRow((double)32761231237, (double)32761231237)]
        [DataRow((double)1012.1200, (double)1012.1200)]
        public void Given_Double_When_Instantiated_It_Should_be_Double(double input, double expectedValue)
        {
              // Arrange
            var expected = new OpenApiDouble(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((bool)true, (bool)true)]
        [DataRow((bool)false, (bool)false)]
        public void Given_Boolean_When_Instantiated_It_Should_be_Boolean(bool input, bool expectedValue)
        {
             // Arrange
            var expected = new OpenApiBoolean(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow((string)"test", (string)"test")]
        [DataRow((string)"abcde", (string)"abcde")]
        public void Given_String_When_Instantiated_It_Should_be_String(string input, string expectedValue)
        {
             // Arrange
            var expected = new OpenApiString(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow("2022-01-01T00:00:00.000Z")]
        [DataRow("2023-05-13T12:34:56.789Z")]
        public void Given_DateTime_When_Instantiated_It_Should_be_DateTime(string input)
        {
            // Arrange
            DateTime input_DataTime = DateTime.Parse(input);
            var expectedValue = new OpenApiDateTime(input_DataTime);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input_DataTime, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expectedValue);
        }

        [DataTestMethod]
        [DataRow("2023-05-13T10:00:00+09:00")]
        [DataRow("2022-12-31T23:59:59-08:00")]
        public void Given_DateTimeOffset_When_Instantiated_It_Should_be_OpenApiDateTimeOffset(string input)
        {
            // Arrange
            var inputDateTimeOffset = DateTimeOffset.Parse(input);
            var expected = new OpenApiDateTime(inputDateTimeOffset);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(inputDateTimeOffset, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow("c9da6455-213d-42c9-9a79-3f9149a57833")]
        [DataRow("3f3e3c3b-9f0f-4b5e-bb32-7f5e5c7d7c5c")]
        [DataRow("7e9e2b6f-156d-4d6c-95db-3132a4bfb4e4")]
        public void Given_ObjectGuid_When_Instantiated_It_Should_be_ObjectGuid(string input)
        {
        // Arrange
        var expected = new OpenApiString(input);
        // Act
        var result = OpenApiExampleFactory.CreateInstance(Guid.Parse(input), new JsonSerializerSettings());

        // Assert
        result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow(new byte[] { }, "")]
        [DataRow(new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100 }, "SGVsbG8gV29ybGQ=")]
        public void Given_ObjectByte_When_Instantiated_It_Should_be_ObjectByte(byte[] input, string expectedValue)
        {
            // Arrange
            var expected = new OpenApiString(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }


        [DataTestMethod]
        [DataRow(1, "1")]
        [DataRow(3.14, "3.14")]
        [DataRow(true, "true")]
        [DataRow(false, "false")]
        public void Given_Object_When_Instantiated_It_Should_be_Object(object input, string expectedValue)
        {
            // Arrange
            var expected = new OpenApiString(expectedValue);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [DataTestMethod]
        [DataRow('t')]
        public void Given_default_When_Instantiated_It_Should_throw_InvalidOperationException(char input)
        {
        // Act & Assert
        Action action = () => OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());
        action.Should().Throw<InvalidOperationException>();
        }
    }
}
