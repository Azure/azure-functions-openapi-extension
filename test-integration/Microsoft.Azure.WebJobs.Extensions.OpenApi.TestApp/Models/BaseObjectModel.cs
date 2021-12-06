using System.Collections.Generic;

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
    }
}
