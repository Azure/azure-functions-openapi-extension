using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.OpenApi.Any;

using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests
{
    [TestClass]
    public class OpenApiExampleFactoryTests
    {
        [TestMethod]
        public void Given_Int16_When_Instantiated_It_Should_be_Int16()
        {
             // Arrange
            var input = (Int16)32767;
            var expected = new OpenApiInteger(32767);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiInteger)result).Value);
        }

        [TestMethod]
        public void Given_Int32_When_Instantiated_It_Should_be_Int32()
        {
             // Arrange
            var input = (Int32)2147483647;
            var expected = new OpenApiInteger(2147483647);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiInteger)result).Value);
        }

        [TestMethod]
        public void Given_Int64_When_Instantiated_It_Should_be_Int64()
        {
             // Arrange
            var input = (Int64)9223372036854775807;
            var expected = new OpenApiLong(9223372036854775807);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiLong)result).Value);
        }

        [TestMethod]
        public void Given_UInt16_When_Instantiated_It_Should_be_UInt16()
        {
             // Arrange
            var input = (UInt16)65535;
            var expected = new OpenApiDouble(65535);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiDouble)result).Value);
        }

        [TestMethod]
        public void Given_UInt32_When_Instantiated_It_Should_be_UInt32()
        {
             // Arrange
            var input = (UInt32)4294967295;
            var expected = new OpenApiDouble(4294967295);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiDouble)result).Value);
        }

        [TestMethod]
        public void Given_UInt64_When_Instantiated_It_Should_be_UInt64()
        {
             // Arrange
            var input = (UInt64) 18446744073709551615;
            var expected = new OpenApiDouble(18446744073709551615);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiDouble)result).Value);
        }


        [TestMethod]
        public void Given_Single_When_Instantiated_It_Should_be_Single()
        {
             // Arrange
            var input = (float)3.40282347f;
            var expected = new OpenApiFloat(3.40282347f);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiFloat)result).Value);
        }

        [TestMethod]
        public void Given_Double_When_Instantiated_It_Should_be_Double()
        {
             // Arrange
            var input = (Double)3.40282347;
            var expected = new OpenApiDouble(3.40282347);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiDouble)result).Value);
        }

        [TestMethod]
        public void Given_Boolean_When_Instantiated_It_Should_be_Boolean()
        {
             // Arrange
            var input = (Boolean)false;
            var expected = new OpenApiBoolean(false);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiBoolean)result).Value);
        }
        [TestMethod]
        public void Given_String_When_Instantiated_It_Should_be_String()
        {
             // Arrange
            var input = (String)"Test";
            var expected = new OpenApiString("Test");

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiString)result).Value);
        }
        [TestMethod]
        public void Given_DateTime_When_Instantiated_It_Should_be_DateTime()
        {
            // Arrange
            var input = new DateTime(2022, 5, 13, 12, 0, 0, DateTimeKind.Utc);
            var expected = new OpenApiDateTime(input);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiDateTime)result).Value);
        }

        [TestMethod]
        public void Given_ObjectDataTimeOffset_When_Instantiated_It_Should_be_ObjectDataTimeOffset()
        {
            // Arrange
            var input = DateTimeOffset.Now;
            var expected = new OpenApiDateTime(input);

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiDateTime)result).Value);
        }
        [TestMethod]
        public void Given_ObjectGuid_When_Instantiated_It_Should_be_ObjectGuid()
        {
            // Arrange
            var input = Guid.NewGuid();
            var expected = new OpenApiString(input.ToString());

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiString)result).Value);
        }

        [TestMethod]
        public void Given_ObjectByte_When_Instantiated_It_Should_be_ObjectByte()
        {
            // Arrange
            var input = new byte[] { 1, 2, 3 };
            var expected = new OpenApiString(Convert.ToBase64String(input));

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiString)result).Value);
}

        [TestMethod]
        public void Given_Object_When_Instantiated_It_Should_be_Object()
        {
            // Arrange
            var input = new { name = "test", age = 20 };
            var expected = new OpenApiString(JsonConvert.SerializeObject(input));

            // Act
            var result = OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings());

            // Assert
            Assert.AreEqual(expected.GetType(), result.GetType());
            Assert.AreEqual(expected.Value, ((OpenApiString)result).Value);
        }
        [TestMethod]
        public void Given_default_When_Instantiated_It_Should_throw_InvalidOperationException()
        {
            // Arrange
            var input = new StringBuilder();

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => OpenApiExampleFactory.CreateInstance(input, new JsonSerializerSettings()));
        }
    }
}
