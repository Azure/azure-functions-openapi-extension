using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class BaseSubObjectModel
    {
        public object BaseSubObjectValue { get; set; }
    }

    public class BaseObjectModel
    {
        public object BaseObjectValue { get; set; }
        public int NonObjectValue { get; set; }
        public BaseSubObjectModel SubObjectValue { get; set; }
        public List<object> BaseObjectList { get; set; }
        public Dictionary<string, object> BaseObjectDictionary { get; set; }
        public SimpleEnumType SimpleEnumValue1 { get; set; }
        public SimpleEnumType SimpleEnumValue2 { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SimpleEnumType
    {
        Simple,
        TitleCase,
        Values
    }
    
  
}
