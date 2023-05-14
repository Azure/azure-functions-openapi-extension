using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions
{
    /// <summary>
    /// Extensions for IEnumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Return distinct elements based on property from the source collection.
        /// </summary>
        /// <param name="source"><see cref="IEnumerable{TSource}"/> instance.</param>
        /// <param name="keySelector">Func{TSource, TKey}) instance.</param>
        /// <typeparam name="TSource">Type of Source</typeparam>
        /// <typeparam name="TKey">Type of Key</typeparam>
        /// <returns>Returns an <see cref="IEnumerable{TSource}"/> that contains the distinct elements based on the specified property.</returns>
        public static IEnumerable<TSource> DistinctByProperty<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}   