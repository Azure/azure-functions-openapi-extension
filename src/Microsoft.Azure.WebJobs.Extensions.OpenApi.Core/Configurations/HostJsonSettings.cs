namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations
{
    /// <summary>
    /// This represents the settings entity for "host.json".
    /// </summary>
    public class HostJsonSettings
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="HttpSettings"/> instance.
        /// </summary>
        public virtual HttpSettings Http { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ExtensionsSettings"/> instance.
        /// </summary>
        public virtual ExtensionsSettings Extensions { get; set; }
    }
}
