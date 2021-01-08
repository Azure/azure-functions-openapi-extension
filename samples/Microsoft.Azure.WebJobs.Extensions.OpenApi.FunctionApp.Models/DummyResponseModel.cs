using System.Collections.Generic;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    public class DummyResponseModel
    {
        public DummySubResponseModel SubObjectValue { get; set; }

        public DummyRecursiveResponseModel RecursiveObjectValue { get; set; }

        public IDictionary<string, string> DictionaryStringValue { get; set; }

        public Dictionary<string, int> DictionaryIntValue { get; set; }

        public Dictionary<string, DummyDictionaryResponseModel> DictionaryObjectValue { get; set; }

        public IList<string> ListStringValue { get; set; }

        public List<int> ListIntValue { get; set; }

        public List<DummyArrayResponseModel> ListObjectValue { get; set; }

        public DummyArrayResponseModel[] ArrayObjectValue { get; set; }

        public JObject JObjectValue { get; set; }

        public JToken JTokenValue { get; set; }

        [JsonProperty("CapitalisedJsonPropertyValue")]
        public string JsonPropertyValue { get; set; }

        [JsonIgnore]
        public string JsonIgnoreValue { get; set; }

        [JsonProperty("CapitalisedJsonPropertyRequiredValue", Required = Required.DisallowNull)]
        public string JsonPropertyRequiredValue { get; set; } = "hello world";

        [JsonRequired]
        public string JsonRequiredValue { get; set; } = "lorem ipsum";

        [OpenApiSchemaVisibility(OpenApiVisibilityType.Advanced)]
        public string OpenApiSchemaVisibilityValue { get; set; }

        public DummySubResponseModel SubResponse1 { get; set; }

        public DummySubResponseModel SubResponse2 { get; set; }

        public DummyGenericModel<DummyModel> DummyGenericDummyModel { get; set; }
    }
}
