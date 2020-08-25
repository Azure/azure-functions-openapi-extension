using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    public class DummyArrayResponseModel
    {
        public string Id { get; set; }

        [JsonRequired]
        public string JsonRequiredValue { get; set; }
    }
}
