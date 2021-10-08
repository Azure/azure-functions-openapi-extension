using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class FloatingPointObjectModel
    {
        public float SingleValue { get; set; } // System.SIngle
        public double DoubleValue { get; set; } // System.Double
        public decimal DecimalValue { get; set; } // System.Decimal
    }
}
