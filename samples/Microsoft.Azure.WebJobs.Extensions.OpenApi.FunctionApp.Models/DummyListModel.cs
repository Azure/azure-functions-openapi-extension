using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    public class DummyListModel
    {
        public List<DummyStringModel> ListValues1 { get; set; }

        public HashSet<int> ListValues2 { get; set; }

        public ISet<DummyStringModel> ListValues3 { get; set; }
    }
}
