using FluentAssertions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes;
using Microsoft.OpenApi.Any;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Serialization;
using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Abstractions
{
    [TestClass]
    public class OpenApiExampleTests
    {
        [TestMethod]
        [DataRow("stringValue1", "Lorem")]
        [DataRow("stringValue2", "")]
        public void Given_StringType_When_Instantiated_Then_It_Should_Return_Result(string exampleName, string exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeStringParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiString>();
            (result[exampleName].Value as OpenApiString).Value.Should().Be(exampleValue);
        }
        [TestMethod]
        [DataRow("int16Value1", (short)1)]
        [DataRow("int16Value2", (short)0)]
        public void Given_Int16Type_When_Instantiated_Then_It_Should_Return_Result(string exampleName, short exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeInt16ParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiInteger>();
            (result[exampleName].Value as OpenApiInteger).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("int32Value1", 1)]
        [DataRow("int32Value2", 0)]
        public void Given_Int32Type_When_Instantiated_Then_It_Should_Return_Result(string exampleName, int exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeInt32ParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiInteger>();
            (result[exampleName].Value as OpenApiInteger).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("int64Value1", (long)1)]
        [DataRow("int64Value2", (long)0)]
        public void Given_Int64Type_When_Instantiated_Then_It_Should_Return_Result(string exampleName, long exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeInt64ParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiLong>();
            (result[exampleName].Value as OpenApiLong).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("uint16Value1", (ushort)1)]
        [DataRow("uint16Value2", (ushort)0)]
        public void Given_Uint16Type_When_Instantiated_Then_It_Should_Return_Result(string exampleName, ushort exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeUint16ParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiDouble>();
            (result[exampleName].Value as OpenApiDouble).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("uint32Value1", (uint)1)]
        [DataRow("uint32Value2", (uint)0)]
        public void Given_Uint32Type_When_Instantiated_Then_It_Should_Return_Result(string exampleName, uint exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeUint32ParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiDouble>();
            (result[exampleName].Value as OpenApiDouble).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("uint64Value1", (ulong)1)]
        [DataRow("uint64Value2", (ulong)0)]
        public void Given_Uint64Type_When_Instantiated_Then_It_Should_Return_Result(string exampleName, ulong exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeUint64ParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiDouble>();
            (result[exampleName].Value as OpenApiDouble).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("singleValue1", (float)1.1)]
        [DataRow("singleValue2", (float)0.0)]
        public void Given_SingleType_When_Instantiated_Then_It_Should_Return_Result(string exampleName, float exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeSingleParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiFloat>();
            (result[exampleName].Value as OpenApiFloat).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("doubleValue1", 1.1)]
        [DataRow("doubleValue2", 0.0)]
        public void Given_DoubleType_When_Instantiated_Then_It_Should_Return_Result(string exampleName, double exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeDoubleParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiDouble>();
            (result[exampleName].Value as OpenApiDouble).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("booleanValue1", true)]
        [DataRow("booleanValue2", false)]
        public void Given_BooleanType_When_Instantiated_Then_It_Should_Return_Result(string exampleName, bool exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeBooleanParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiBoolean>();
            (result[exampleName].Value as OpenApiBoolean).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("dateTimeValue1", "2021-01-01")]
        [DataRow("dateTimeValue2", "2021-01-01T12:34:56Z")]
        public void Given_DateTimeType_When_Instantiated_Then_It_Should_Return_Result(string exampleName, string exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeDateTimeParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiDateTime>();
            (result[exampleName].Value as OpenApiDateTime).Value.Should().Be(DateTime.Parse(exampleValue));
        }

        [TestMethod]
        [DataRow("dateTimeOffsetValue1", "05/01/2008")]
        [DataRow("dateTimeOffsetValue2", "11:36 PM")]
        [DataRow("dateTimeOffsetValue3", "05/01/2008 +1:00")]
        [DataRow("dateTimeOffsetValue4", "Thu May 01, 2008")]
        public void Given_DateTimeOffsetType_When_Instantiated_Then_It_Should_Return_Result(string exampleName, string exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeDateTimeOffsetParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiDateTime>();
            (result[exampleName].Value as OpenApiDateTime).Value.Should().Be(DateTimeOffset.Parse(exampleValue));
        }

        [TestMethod]
        [DataRow("timeSpanValue1","06:12:14")]
        [DataRow("timeSpanValue2", "6.12:14:45")]
        public void Given_TimeSpanType_When_Instantiated_Then_It_Should_Return_Result(string exampleName, string exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeTimeSpanParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiString>();
            (result[exampleName].Value as OpenApiString).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("guidValue1", "74be27de-1e4e-49d9-b579-fe0b331d3642")]
        public void Given_GuidType_When_Instantiated_Then_It_Should_Return_Result(string exampleName, string exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeGuidParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiString>();
            (result[exampleName].Value as OpenApiString).Value.Should().Be(exampleValue);
        }

        [TestMethod]
        [DataRow("byteArrayValue1", "rBgS8A==")]
        [DataRow("byteArrayValue2", "/zIR")]
        public void Given_ByteArrayType_When_Instantiated_Then_It_Should_Return_Result(string exampleName, string exampleValue)
        {
            var namingStrategy = new DefaultNamingStrategy();
            var example = new FakeByteArrayParameterExample();

            var result = example.Build(namingStrategy).Examples;

            result[exampleName].Value.Should().BeOfType<OpenApiString>();
            (result[exampleName].Value as OpenApiString).Value.Should().Be(exampleValue);
        }
    }
}
