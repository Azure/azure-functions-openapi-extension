using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models.Examples;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    [OpenApiExample(typeof(DummyStringModelExample))]
    public class DummyStringModel
    {
        public string StringValue { get; set; }

        public Uri UriValue { get; set; }
    }
}
