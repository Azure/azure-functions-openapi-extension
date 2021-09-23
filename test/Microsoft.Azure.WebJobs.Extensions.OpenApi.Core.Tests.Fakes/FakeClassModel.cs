using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    public class FakeClassModel
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public List<FakeClassModel> Children { get; set; }
    }
}
