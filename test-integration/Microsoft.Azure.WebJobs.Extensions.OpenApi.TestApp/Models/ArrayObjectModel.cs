using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class ArrayObjectModel
    {
        public List<bool> BooleanValue { get; set; }
        public IList<string> StringValue { get; set; }
        public ICollection<int> Int32Value { get; set; }
        public IEnumerable<long> Int64Value { get; set; }
        public IReadOnlyList<float> FloatValue { get; set; }
        public IReadOnlyCollection<decimal> DecimalValue { get; set; }
        public HashSet<StringObjectModel> StringObjectValue { get; set; }
        public ISet<IntegerObjectModel> IntegerObjectValue { get; set; }
        public IEnumerable<NumberObjectModel> NumberObjectValue { get; set; }
    }
}
