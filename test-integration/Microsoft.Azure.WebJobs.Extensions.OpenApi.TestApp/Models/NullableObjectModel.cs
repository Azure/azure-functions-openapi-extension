using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class NullableObjectModel
    {
        public bool? BooleanValue { get; set; }
        public ushort? UInt16Value { get; set; }
        public uint? UInt32Value { get; set;}
        public ulong? UInt64Value { get; set; }
        public short? Int16Value { get; set; }
        public int? Int32Value { get; set; }
        public long? Int64Value { get; set; }
        public float? SingleValue { get; set; }
        public double? DoubleValue { get; set; }
        public decimal? DecimalValue { get; set; }
        public DateTime? DateTimeValue { get; set;}
        public DateTimeOffset? DateTimeOffsetValue { get; set; }
    }
}
