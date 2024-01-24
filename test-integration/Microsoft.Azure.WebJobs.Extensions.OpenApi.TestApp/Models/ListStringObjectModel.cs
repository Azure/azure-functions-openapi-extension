using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models;

public class ListStringObjectModel : IEnumerable<StringObjectModel>
{
    public IEnumerator<StringObjectModel> GetEnumerator() => Enumerable.Empty<StringObjectModel>().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
