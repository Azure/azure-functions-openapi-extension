using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class ExceptionObjectModel
    {
        public Exception Exception { get; set; }
        public StackOverflowException StackOverflowException { get; set; }
    }
}
