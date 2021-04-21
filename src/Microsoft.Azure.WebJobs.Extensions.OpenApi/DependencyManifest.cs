using System.Collections.Generic;

namespace Microsoft.Azure.WebJobs.Extensions.OpenApi
{
    /// <summary>
    /// This represents the dependency manifest entity.
    /// </summary>
    public class DependencyManifest
    {
        /// <summary>
        /// Gets or sets the runtime target that indicates the current runtime;
        /// </summary>
        public virtual RuntimeTarget RuntimeTarget { get; set; }

        /// <summary>
        /// Gets or sets the collection of runtime packages.
        /// </summary>
        public virtual Dictionary<string, Dictionary<string, RuntimePackage>> Targets { get; set; }
    }

    /// <summary>
    /// This represents the runtime target entity.
    /// </summary>
    public class RuntimeTarget
    {
        /// <summary>
        /// Gets or sets the runtime target name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the runtime target signature.
        /// </summary>
        public virtual string Signature { get; set; }
    }

    /// <summary>
    /// This represents the runtime package entity.
    /// </summary>
    public class RuntimePackage
    {
        /// <summary>
        /// Gets or sets the collection of dependent packages.
        /// </summary>
        public virtual Dictionary<string, string> Dependencies { get; set; }

        /// <summary>
        /// Gets or sets the collection of runtime libraries.
        /// </summary>
        public virtual Dictionary<string, object> Runtime { get; set; }
    }
}
