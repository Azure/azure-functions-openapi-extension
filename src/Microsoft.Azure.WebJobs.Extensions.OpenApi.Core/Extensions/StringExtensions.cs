using System;

using Microsoft.Extensions.Primitives;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension class for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks whether the string value is either <c>null</c> or white space.
        /// </summary>
        /// <param name="value"><see cref="string"/> value to check.</param>
        /// <returns><c>true</c>, if the string value is either <c>null</c> or white space; otherwise returns <c>false</c>.</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the given value is <c>null</c> or white space.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>The original value, if the value is NOT <c>null</c>; otherwise throws an <see cref="ArgumentNullException"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/></exception>
        public static string ThrowIfNullOrWhiteSpace(this string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value;
        }

        /// <summary>
        /// Converts the <see cref="StringValues"/> value to array of string.
        /// </summary>
        /// <param name="value"><see cref="StringValues"/> value.</param>
        /// <param name="delimiter">Delimiter to split values.</param>
        /// <returns>Returns the array of string.</returns>
        public static string[] ToArray(this StringValues value, string delimiter = ",")
        {
            if (value.IsNullOrDefault())
            {
                return new string[0];
            }

            var values = value.ToString();
            if (values.IsNullOrWhiteSpace())
            {
                return new string[0];
            }

            return values.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
