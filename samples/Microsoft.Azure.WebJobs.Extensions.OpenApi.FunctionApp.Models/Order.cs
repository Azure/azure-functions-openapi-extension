using System;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for order of Swagger Pet Store.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Gets or sets the order ID.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Gets or sets the pet ID.
        /// </summary>
        public long? PetId { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// Gets or seets the date/time shipped.
        /// </summary>
        public DateTime? ShipDate { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="OrderStatus"/> value.
        /// </summary>
        public OrderStatus? Status { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the order is complete or not.
        /// </summary>
        public bool? Complete { get; set; }
    }
}
