using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class ArrayObjectModel
    {
        public List<object> ObjectValue { get; set; }
        public IList<bool> BooleanValue { get; set; }
        public ICollection<string> StringValue { get; set; }
        public IEnumerable<int> Int32Value { get; set; }
        public IReadOnlyList<long> Int64Value { get; set; }
        public IReadOnlyCollection<float> FloatValue { get; set; }
        public HashSet<decimal> DecimalValue { get; set; }
        public ISet<StringObjectModel> StringObjectValue { get; set; }

        public List<object[]> ObjectArrayValue { get; set; }
    }
}
