using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    class DateTypeObject
    {
        public DateTime NonNullbleDateTimeType { get; set; }
        public DateTime? NullbleDateTimeType { get; set; }
        [DataType(DataType.Date)]
        public DateTime NonNullbleDateType { get; set; }
        [DataType(DataType.Date)]
        public DateTime? NullbleDateType { get; set; }
    }
}
