using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for generic.
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Checks whether the given instance is <c>null</c> or default.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="instance">Instance to check.</param>
        /// <returns><c>true</c>, if the original instance is <c>null</c> or empty; otherwise returns <c>false</c>.</returns>
        public static bool IsNullOrDefault<T>(this T instance)
        {
            return instance == null || instance.Equals(default(T));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the given instance is <c>null</c> or default.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="instance">Instance to check.</param>
        /// <returns>The original instance, if the instance is NOT <c>null</c>; otherwise throws an <see cref="ArgumentNullException"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/></exception>
        public static T ThrowIfNullOrDefault<T>(this T instance)
        {
            if (instance.IsNullOrDefault())
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return instance;
        }

        /// <summary>
        /// Checks whether the given list of items is empty.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="items">List of instances.</param>
        /// <returns><c>true</c>, if the given list of items is empty; otherwise returns <c>false</c>.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            items.ThrowIfNullOrDefault();

            return !items.Any();
        }

        /// <summary>
        /// Adds range of collection to source.
        /// </summary>
        /// <typeparam name="TKey">Type of the key.</typeparam>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="source"><see cref="Dictionary{TKey, TValue}"/> instance to be added as a source.</param>
        /// <param name="collection"><see cref="Dictionary{TKey, TValue}"/> instance to add.</param>
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> collection)
        {
            source.ThrowIfNullOrDefault();

            if (collection.IsNullOrDefault())
            {
                return;
            }

            foreach (var item in collection)
            {
                if (source.ContainsKey(item.Key))
                {
                    continue;
                }

                source.Add(item.Key, item.Value);
            }
        }
    }
}
