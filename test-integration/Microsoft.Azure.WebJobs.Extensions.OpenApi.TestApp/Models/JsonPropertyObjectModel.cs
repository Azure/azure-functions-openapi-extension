using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class JsonPropertyObjectModel
    {
        [DataMember(Name = "memBer1")]
        public string DataMemberValue1 { get; set; }

        [DataMember(Name = "MEMBER2")]
        public int DataMemberValue2 { get; set; }

        [DataMember(Name = "MeMBer3")]
        public DateTime DataMemberValue3 { get; set; }

        [DataMember(Name = "member4")]
        public bool DataMemberValue4 { get; set; }

        [DataMember(Name = "MembeR5")]
        public double DataMemberValue5 { get; set; }

    }
}
