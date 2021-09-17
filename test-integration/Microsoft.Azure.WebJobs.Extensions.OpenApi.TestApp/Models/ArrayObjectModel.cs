using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class ArrayObjectModel
    {
        public List<bool> BoolValue { get; set; }
        public IList<string> StringValue { get; set; }
        public ICollection<int> Int32Value { get; set; }
        public IEnumerable<long> Int64Value { get; set; }
        public IReadOnlyList<float> FloatValue { get; set; }
        public IReadOnlyCollection<decimal> DecimalValue { get; set; }
        public HashSet<StringObjectModel> StringObjectValue { get; set; }
        public ISet<Int32ObjectModel> Int32ObjectValue { get; set; }

        public IEnumerable<StringObjectModel> StringObjectModel { get; set; }
    }
}
