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
        /// <param name="includeReferenced">Set to true to load <see cref="Type"/>s from referenced assemblies too</param>
        /// <returns>Returns the list of <see cref="Type"/>s that can be loaded.</returns>
        public static Type[] GetLoadableTypes(this Assembly assembly, bool includeReferenced = false)
        {
            try
            {
                return includeReferenced
                    ? assembly.GetTypes()
                        .Union(assembly
                            .GetReferencedAssemblies()
                            .SelectMany(x => Assembly.Load(x).GetTypes()))
                        .Distinct()
                        .ToArray()
                    : assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return exception.Types.Where(t => t != null).ToArray();
            }
        }
    }
}
