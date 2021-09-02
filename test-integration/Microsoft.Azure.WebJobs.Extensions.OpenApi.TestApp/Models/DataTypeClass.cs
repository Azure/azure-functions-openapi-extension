using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class DataTypeClass
    {
        [DataType(DataType.DateTime)]
        public DateTime DateTimeValue1 { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateTimeValue2 { get; set; }

        [DataType(DataType.Time)]
        public DateTime DateTimeValue3 { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? NullableDateTimeValue1 { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NullableDateTimeValue2 { get; set; }

        [DataType(DataType.Time)]
        public DateTime? NullableDateTimeValue3 { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset DateTimeOffsetValue1 { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset DateTimeOffsetValue2 { get; set; }

        [DataType(DataType.Time)]
        public DateTimeOffset DateTimeOffsetValue3 { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? NullableDateTimeOffsetValue1 { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset? NullableDateTimeOffsetValue2 { get; set; }

        [DataType(DataType.Time)]
        public DateTimeOffset? NullableDateTimeOffsetValue3 { get; set; }
    }
}
