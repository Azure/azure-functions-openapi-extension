using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class JsonPropertyObjectModel
    {
        [JsonProperty("Member1")]
        public string DataMemberValue1 { get; set; }

        [DataMember(Name = "MEMBER2")]
        public int DataMemberValue2 { get; set; }

        [DataMember(Name = "MeMBer3")]
        [JsonProperty("mEmbER3")]
        public DateTime DataMemberValue3 { get; set; }
    }
}
