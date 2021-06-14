using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models.Examples;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for category of Swagger Pet Store.
    /// </summary>
    [OpenApiExample(typeof(CategoryExample))]
    public class Category
    {
        /// <summary>
        /// Gets or sets the category ID.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
