using System;

using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    public class DummyDictionaryResponseModel
    {
        public Guid? Id { get; set; }

        [JsonRequired]
        public string JsonRequiredValue { get; set; }
    }
}
