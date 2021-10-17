using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class ExceptionObjectModel
    {
        public Exception ExceptionValue { get; set; }
        public InvalidOperationException InvalidOperationExceptionValue { get; set; }
        public StackOverflowException StackOverflowExceptionValue { get; set; }
    }
}
