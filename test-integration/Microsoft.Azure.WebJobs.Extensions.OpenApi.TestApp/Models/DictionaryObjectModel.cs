using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class DictionaryObjectModel
    {
        public IDictionary<string, StringObjectModel> StringObjectModel { get; set; }
    }
}
