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
            var types = default(List<Type>);
            try
            {
                types = assembly.GetTypes().ToList();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null).ToList();
            }

            var assemblies = assembly
                             .GetReferencedAssemblies()
                             .Where(x => x.FullName.StartsWith("Microsoft.Azure.WebJobs.Extensions.OpenApi") == false &&
                                         x.FullName.StartsWith("Microsoft.Azure.Functions.Worker.Extensions.OpenApi") == false)
                             .ToList();
            foreach (var asmbly in assemblies)
            {
                try
                {
                    types.AddRange(Assembly.Load(asmbly).GetTypes());
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types.AddRange(ex.Types.Where(t => t != null));
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

            var assemblies = dlls.Select(p =>
                                  {
                                      var asmbly = default(Assembly);
                                      try
                                      {
                                          asmbly = Assembly.LoadFrom(p);
                                      }
                                      catch { }

                                      return asmbly;
                                  })
                                 .Where(p => p != null);

            var types = assemblies
                            .SelectMany(p =>
                             {
                                 var ts = default(IEnumerable<Type>);
                                 try
                                 {
                                     ts = p.GetTypes();
                                 }
                                 catch (ReflectionTypeLoadException ex)
                                 {
                                     ts = ex.Types.Where(q => q != null);
                                 }

                                 return ts.Where(q => q.GetMethods()
                                                       .Any(r => r.ExistsCustomAttribute<OpenApiOperationAttribute>())
                                           );
                             })
                            .ToArray();

            return types;
        }
    }
}
