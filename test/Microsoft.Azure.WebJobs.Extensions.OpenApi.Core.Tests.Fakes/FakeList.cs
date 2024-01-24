using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeList : IEnumerable<FakeModel>
    {
        public IEnumerator<FakeModel> GetEnumerator() => Enumerable.Empty<FakeModel>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
