namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for category of Swagger Pet Store.
    /// </summary>
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
