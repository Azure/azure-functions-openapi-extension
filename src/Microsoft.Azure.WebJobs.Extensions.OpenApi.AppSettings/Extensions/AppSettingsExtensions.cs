using System.Runtime.InteropServices;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions
{
    /// <summary>
    /// This represents the entity to determine operating system.
    /// </summary>
    public static class OperationSystem
    {
        /// <summary>
        /// Gets the value indicating whether the OS is Windows or not.
        /// </summary>
        /// <returns><c>True</c>, if the OS is Windows; otherwise returns <c>False</c>.</returns>
        public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// Gets the value indicating whether the OS is Mac or not.
        /// </summary>
        /// <returns><c>True</c>, if the OS is Mac; otherwise returns <c>False</c>.</returns>
        public static bool IsMacOs() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// Gets the value indicating whether the OS is Linux or not.
        /// </summary>
        /// <returns><c>True</c>, if the OS is Linux; otherwise returns <c>False</c>.</returns>
        public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
}