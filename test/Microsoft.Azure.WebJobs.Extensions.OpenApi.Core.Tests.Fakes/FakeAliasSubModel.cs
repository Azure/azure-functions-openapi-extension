using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeAliasSubModel
    {
        [JsonRequired]
        public string @String { get; set; }

        public FakeSubModel FakeSubModel { get; set; }
        public FakeDummyModel FakeDummyModel { get; set; }
    }
}
