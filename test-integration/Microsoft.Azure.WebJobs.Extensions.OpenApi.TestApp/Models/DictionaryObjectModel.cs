using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class DictionaryObjectModel
    {
        public Dictionary<string, object> ObjectValue { get; set; }
        public IDictionary<string, bool> BooleanValue { get; set; }
        public IReadOnlyDictionary<string, string> StringValue { get; set; }
        public KeyValuePair<string, int> Int32Value { get; set; }

        public Dictionary<string, ObjectObjectModel> ObjectObjectValue { get; set; }
        public IDictionary<string, BooleanObjectModel> BooleanObjectValue { get; set; }
        public IReadOnlyDictionary<string, StringObjectModel> StringObjectValue { get; set; }
        public KeyValuePair<string, IntegerObjectModel> IntegerObjectValue { get; set; }

        public Dictionary<int, object[]> ObjectArrayValue { get; set; }
        public IDictionary<int, bool[]> BooleanArrayValue { get; set; }
        public IReadOnlyDictionary<string, string[]> StringArrayValue { get; set; }
        public KeyValuePair<int, int[]> Int32ArrayValue { get; set; }
    }
}
