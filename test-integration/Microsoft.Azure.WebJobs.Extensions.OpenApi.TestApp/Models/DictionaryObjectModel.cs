using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class DictionaryObjectModel<TKey, TValue>
    {
        public Dictionary<TKey, TValue> Dictionary { get; set; }

        public IDictionary<TKey, TValue> IDictionary { get; set; }

        public IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionary { get; set; }

        public KeyValuePair<TKey, TValue> KeyValuePair { get; set; }
    }
}
