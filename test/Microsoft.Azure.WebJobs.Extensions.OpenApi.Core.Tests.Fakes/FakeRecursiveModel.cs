using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    /// <summary>
    /// This represents the recursive fake model entity
    /// </summary>
    public class FakeRecursiveModel
    {
        [JsonRequired]
        public string StringValue { get; set; }

        public FakeRecursiveModel RecursiveValue { get; set; }

        public FakeOtherClassModel FirstValue { get; set; }

        public FakeOtherClassModel SecondValue { get; set; }
    }
}
