using System;

namespace sharp_net.Repositories {

    /// <summary>
    /// Represents that the implemented classes are domain events.
    /// </summary>
    public interface IDomainEvent {

        /// <summary>
        /// Gets or sets the assembly qualified name of the type of the aggregate root.
        /// </summary>
        string AssemblyQualifiedSourceType { get; set; }
        /// <summary>
        /// Gets or sets the identifier of the aggregate root.
        /// </summary>
        string SourceID { get; set; }
        /// <summary>
        /// Gets or sets the version of the domain event.
        /// </summary>
        long Version { get; set; }
        /// <summary>
        /// Gets or sets the branch on which the current domain event exists.
        /// </summary>
        long Branch { get; set; }

        DateTime Timestamp { get; set; }
    }
}