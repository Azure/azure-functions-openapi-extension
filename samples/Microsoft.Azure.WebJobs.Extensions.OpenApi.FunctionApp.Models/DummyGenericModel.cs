namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    public class DummyGenericModel<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
    }
}
