using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class DictionaryObjectModel
    {
        public Dictionary<string, bool> BoolValue { get; set; }
        public IDictionary<string, string> StringValue { get; set; }
        public IReadOnlyDictionary<string, float> FloatValue { get; set; }
        public KeyValuePair<string, Int32ObjectModel> Int32ObjectValue { get; set; }

        public IDictionary<string, StringObjectModel> StringObjectModel { get; set; }

        public Dictionary<int, int[]> IntArrayValue { get; set; }
        public Dictionary<string, string[]> StringArrayValue { get; set; }
    }
}
