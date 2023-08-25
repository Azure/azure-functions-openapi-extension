using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class CharObjectModel
    {
        public char CharValue { get; set; }

        public Nullable<char> NullableCharValue { get; set; }

        public char? NullableCharValueNull { get; set;}

    }
}