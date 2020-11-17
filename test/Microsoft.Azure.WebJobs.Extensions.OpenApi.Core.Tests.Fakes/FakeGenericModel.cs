namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeGenericModel<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
    }
}
