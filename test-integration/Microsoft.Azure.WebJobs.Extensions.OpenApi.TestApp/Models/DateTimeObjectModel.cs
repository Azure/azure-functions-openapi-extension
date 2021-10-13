using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class DateTimeObjectModel
    {
        public DateTime DateTimeValue { get; set; }
      
        public DateTimeOffset DateTimeOffsetValue { get; set; }
    }
}
