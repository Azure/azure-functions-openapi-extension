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
        /// Distinct by defined property
        /// </summary>
        /// <param name="source">An instance of <see cref="IEnumerable"/> that represents the source collection.</param>
        /// <param name="keySelector">A function that defines the property to be used as the key. (Func<TSource, TKey>)</param>
        /// <typeparam name="TSource">The type of elements in the source collection.</typeparam>
        /// <typeparam name="TKey">The type of the key property.</typeparam>
        /// <returns>An <see cref="IEnumerable{TSource}"/> that contains the distinct elements based on the specified property.</returns>
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
