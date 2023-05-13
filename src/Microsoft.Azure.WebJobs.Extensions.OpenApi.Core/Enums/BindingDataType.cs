namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the binding data type.
    /// </summary>
    public enum BindingDataType
    {
        /// <summary>
        /// Identifies <c>Undefined</c>.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Identifies <c>String</c>.
        /// </summary>
        String = 1,

        /// <summary>
        /// Identifies <c>Binary</c>.
        /// </summary>
        Binary = 2,

        /// <summary>
        /// Identifies <c>Stream</c>.
        /// </summary>
        Stream = 3,
    }
}
