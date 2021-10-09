using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class DictionaryObjectModel
    {
        public Dictionary<string, bool> BooleanValue { get; set; }
        public IDictionary<string, string> StringValue { get; set; }
        public IReadOnlyDictionary<string, float> FloatValue { get; set; }
        public KeyValuePair<string, IntegerObjectModel> IntegerObjectValue { get; set; }
        public IDictionary<string, StringObjectModel> StringObjectValue { get; set; }
        public Dictionary<int, int[]> Int32ArrayValue { get; set; }
        public Dictionary<string, string[]> StringArrayValue { get; set; }
    }
}
