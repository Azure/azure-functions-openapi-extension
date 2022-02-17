using System.ComponentModel.DataAnnotations;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Tests.Fakes
{
    /// <summary>
    /// This represents the fake required model entity.
    /// </summary>
    public class FakeRequiredModel
    {
        public string FakeProperty { get; set; }

        [Required]
        public string FakeRequiredProperty { get; set; }
    }
}
