using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeAliasCollectionModel
    {
        [JsonRequired]
        public string @String { get; set; }

        public ICollection<FakeAliasSubModel> FakeAliasSubModel { get; set; }
        public ICollection<FakeSubModel> FakeSubModel { get; set; }
    }
}
