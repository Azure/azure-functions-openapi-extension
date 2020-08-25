using System;
using System.IO;

using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="OpenApiDocument"/>.
    /// </summary>
    public static class OpenApiDocumentExtensions
    {
        /// <summary>
        /// Serialises the <see cref="OpenApiDocument"/> based on the spec version and format.
        /// </summary>
        /// <param name="document"><see cref="OpenApiDocument"/> instance.</param>
        /// <param name="writer"><see cref="TextWriter"/> instance.</param>
        /// <param name="version"><see cref="OpenApiSpecVersion"/> value.</param>
        /// <param name="format"><see cref="OpenApiFormat"/> value.</param>
        public static void Serialise(this OpenApiDocument document, TextWriter writer, OpenApiSpecVersion version, OpenApiFormat format)
        {
            document.ThrowIfNullOrDefault();
            writer.ThrowIfNullOrDefault();

            var oaw = OpenApiWriterFactory.CreateInstance(format, writer);
            switch (version)
            {
                case OpenApiSpecVersion.OpenApi2_0:
                    document.SerializeAsV2(oaw);
                    oaw.Flush();
                    return;

                case OpenApiSpecVersion.OpenApi3_0:
                    document.SerializeAsV3(oaw);
                    oaw.Flush();
                    return;

                default:
                    throw new InvalidOperationException("Invalid Open API version");
            }
        }
    }
}
