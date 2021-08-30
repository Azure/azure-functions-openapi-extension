using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeAliasModel
    {
        [JsonRequired]
        public string @String { get; set; }

        public FakeAliasSubModel FakeAliasSubModel { get; set; }
        public FakeSubModel FakeSubModel { get; set; }
    }
}
