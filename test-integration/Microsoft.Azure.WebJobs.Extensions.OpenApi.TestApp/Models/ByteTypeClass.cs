using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class ByteTypeClass
    {
        public byte ByteValue { get; set; }
        public byte[] ByteArrayValue { get; set; }
        public byte? NullableByteValue { get; set; }
    }
}
