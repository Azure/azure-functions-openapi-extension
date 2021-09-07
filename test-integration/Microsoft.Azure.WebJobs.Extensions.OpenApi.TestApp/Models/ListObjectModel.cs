using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class ListObjectModel<T>
    {
        public List<T> List { get; set; }

        public IList<T> IList { get; set; }

        public ICollection<T> ICollection { get; set; }

        public IEnumerable<T> IEnumerable { get; set; }

        public IReadOnlyList<T> IReadOnlyList { get; set; }

        public IReadOnlyCollection<T> IReadOnlyCollection { get; set; }
    }
}
