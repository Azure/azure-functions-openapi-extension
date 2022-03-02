using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="Assembly"/>.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Loads the <see cref="Assembly"/>'s <see cref="Type"/>s that can be loaded ignoring others (includes referenced assemblies).
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        /// <returns>Returns the list of <see cref="Type"/>s that can be loaded.</returns>
        public static Type[] GetLoadableTypes(this Assembly assembly)
        {
            List<Type> types;
            try
            {
                types = assembly.GetTypes().ToList();
            }
            catch (ReflectionTypeLoadException exception)
            {
                types = exception.Types.Where(t => t != null).ToList();
            }
            var assemblies = assembly
                    .GetReferencedAssemblies()
                    .Where(x =>
                        !x.FullName.StartsWith("Microsoft.Azure.WebJobs.Extensions.OpenApi") &&
                        !x.FullName.StartsWith("Microsoft.Azure.Functions.Worker.Extensions.OpenApi"))
                    .ToList();
            foreach (var a in assemblies)
            {
                try
                {
                    types.AddRange(Assembly.Load(a).GetTypes());
                }
                catch (ReflectionTypeLoadException exception)
                {
                    types.AddRange(exception.Types.Where(t => t != null));
                }
            }
            return types.Distinct().ToArray();
        }

        /// <summary>
        /// Loads the list of <see cref="Type"/>s from other assemblies containing function endpoints.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        /// <returns>Returns the list of <see cref="Type"/>s that can be loaded.</returns>
        public static Type[] GetTypesFromReferencedFunctionApps(this Assembly assembly)
        {
            var directory = Path.GetDirectoryName(assembly.Location);
            var dlls = Directory.GetFiles(directory, "*.dll");
            var types = dlls.Select(p => Assembly.LoadFile(p))
                            .SelectMany(p => p.GetTypes()
                                              .Where(q => q.GetMethods()
                                                           .Any(r => r.ExistsCustomAttribute<OpenApiOperationAttribute>())
                                               )
                             ).ToArray();

            return types;
        }
    }
}
