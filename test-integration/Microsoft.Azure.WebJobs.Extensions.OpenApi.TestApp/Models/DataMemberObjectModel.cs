using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class DataMemberObjectModel
    {
        [DataMember(Name = "DataMemberValue1")]
        public string DataMemberValue1 { get; set; }

        [DataMember(Name = "DataMemberValue2")]
        public int DataMemberValue2 { get; set; }

        [DataMember(Name = "DataMemberValue3")]
        public DateTime DataMemberValue3 { get; set; }

        [DataMember(Name = "DataMemberValue4")]
        public bool DataMemberValue4 { get; set; }

        [DataMember(Name = "DataMemberValue5")]
        public double DataMemberValue5 { get; set; }

    }
}
