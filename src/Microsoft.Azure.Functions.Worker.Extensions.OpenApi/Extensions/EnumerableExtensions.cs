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
        /// Gets a collection of distinct elements based on the specified property
        /// </summary>
        /// <param name="source"><see cref="IEnumerable{TSource}"/> instance.</param>
        /// <param name="keySelector"><see cref="Func{TSource, TKey}"/> instance.</param>
        /// <typeparam name="TSource">Type of <see cref="IEnumerable{T}" />.</typeparam>
        /// <typeparam name="TKey">Type of <see cref="Func{T1, T2}" />.</typeparam>
        /// <returns>Returns the <see cref="IEnumerable{TSource}"/> that contains the distinct elements based on the specified property.</returns>
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