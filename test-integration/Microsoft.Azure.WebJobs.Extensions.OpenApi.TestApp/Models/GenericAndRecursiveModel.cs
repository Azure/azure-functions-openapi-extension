using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.TestApp.Models
{
    public class GenericAndRecursiveModel
    {
        public List<GenericAndRecursiveModel> Children  { get; set; }

        public Dictionary<string, GenericAndRecursiveModel> Children2 { get; set; }

    }
}
