using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Resolvers;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings
{
    /// <summary>
    /// This represents the base app settings entity.
    /// </summary>
    public abstract class AppSettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsBase"/> class.
        /// </summary>
        protected AppSettingsBase()
        {
            this.Config = ConfigurationResolver.Resolve();
        }

        /// <summary>
        /// Gets the <see cref="IConfiguration"/> instance.
        /// </summary>
        protected virtual IConfiguration Config { get; }

        /// <summary>
        /// Gets the base path
        /// </summary>
        /// <returns></returns>
        protected string GetBasePath()
        {
            return ConfigurationResolver.GetBasePath(this.Config);
        }
    }
}
