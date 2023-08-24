using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class ByteObjectModel
    {
        public byte ByteValue { get; set; }

        public Nullable<byte> NullableByteValue { get; set; }

        public byte? NullableByteValueNull { get; set; }
    }
}