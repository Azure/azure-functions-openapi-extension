namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    public class DummyRecursiveResponseModel
    {
        public string Id { get; set; }

        public DummyRecursiveResponseModel RecursiveValue { get; set; }
    }
}
