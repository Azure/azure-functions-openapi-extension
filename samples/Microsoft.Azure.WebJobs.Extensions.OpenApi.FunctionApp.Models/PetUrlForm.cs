using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for pet of Swagger Pet Store, through application/x-www-form-urlencoded.
    /// </summary>
    public class PetUrlForm
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [OpenApiProperty(Description = "Updated name of the pet")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PetStatus"/> value.
        /// </summary>
        [OpenApiProperty(Description = "Updated status of the pet")]
        public PetStatus? Status { get; set; }
    }
}
