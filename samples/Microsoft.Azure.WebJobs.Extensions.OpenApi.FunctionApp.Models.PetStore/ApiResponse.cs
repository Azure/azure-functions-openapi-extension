namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models.PetStore
{
    /// <summary>
    /// This represents the model entity for API response of Swagger Pet Store.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}
