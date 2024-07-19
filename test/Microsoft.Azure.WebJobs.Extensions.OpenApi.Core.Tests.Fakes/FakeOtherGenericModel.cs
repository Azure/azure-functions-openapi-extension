namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeOtherGenericModel<T1, T2>
    {
        public string Name { get; set; }
        public T1 FirstValue { get; set; }
        public T2 SecondValue { get; set; }
    }
}
