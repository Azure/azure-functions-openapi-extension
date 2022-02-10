using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models.PetStore
{
    /// <summary>
    /// This represents the model entity for pet of Swagger Pet Store, through multipart/form-data.
    /// </summary>
    public class PetFormData
    {
        /// <summary>
        /// Gets or sets the additional metadata to pass to server.
        /// </summary>
        [OpenApiProperty(Description = "Additional data to pass to server")]
        public string AdditionalMetadata { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="byte[]"/> value representing file to upload.
        /// </summary>
        [OpenApiProperty(Description = "File to upload")]
        public byte[] File { get; set; }
    }
}
