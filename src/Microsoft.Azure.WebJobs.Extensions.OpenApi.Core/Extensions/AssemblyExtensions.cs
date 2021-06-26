using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="Assembly"/>.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Loads the <see cref="Assembly"/>'s <see cref="Type"/>s that can be loaded ignoring others.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        /// <returns>Returns the list of <see cref="Type"/>s that can be loaded.</returns>
        public static Type[] GetLoadableTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return exception.Types.Where(t => t != null).ToArray();
            }
        }
    }
}
