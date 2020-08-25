using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    public class DummyRequestModel
    {
        public Guid Id { get; set; }

        public short ShortValue { get; set; }

        public int IntValue { get; set; }

        public long LongValue { get; set; }

        public float SingleValue { get; set; }

        public double DoubleValue { get; set; }

        public decimal DecimalValue { get; set; }

        public int? NullableIntValue { get; set; }

        public bool BoolValue { get; set; }

        public ShortEnum ShortEnumValue { get; set; }

        public IntEnum IntEnumValue { get; set; }

        public LongEnum LongEnumValue { get; set; }

        public StringEnum StringEnumValue { get; set; }

        public DateTime DateTimeValue { get; set; }

        public DateTimeOffset DateTimeOffsetValue { get; set; }
    }
}
