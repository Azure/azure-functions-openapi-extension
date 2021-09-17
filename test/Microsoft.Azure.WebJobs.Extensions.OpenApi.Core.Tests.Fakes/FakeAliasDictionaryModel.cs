using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeAliasDictionaryModel
    {
        [JsonRequired]
        public string @String { get; set; }

        public IDictionary<string, FakeAliasSubModel> FakeAliasSubModel { get; set; }
        public IDictionary<string, FakeSubModel> FakeSubModel { get; set; }
    }
}
