namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums
{
    /// <summary>
    /// This specifies the binding direction type
    /// </summary>
    public enum BindingDirectionType
    {
        /// <summary>
        /// Identifies <c>In</c>.
        /// </summary>
        In = 0,

        /// <summary>
        /// Identifies <c>Out</c>.
        /// </summary>
        Out = 1,

        /// <summary>
        /// Identifies <c>In/Out</c>.
        /// </summary>
        InOut = 2,
    }
}
