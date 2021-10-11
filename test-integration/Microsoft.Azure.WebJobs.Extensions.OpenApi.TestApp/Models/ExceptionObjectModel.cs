using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class ExceptionObjectModel
    {
        public AggregateException AggregateException { get; set; }
        public StackOverflowException StackOverflowException { get; set; }
    }
}
