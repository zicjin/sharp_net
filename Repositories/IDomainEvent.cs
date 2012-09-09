using System;

namespace sharp_net.Repositories {

    /// <summary>
    /// Represents that the implemented classes are domain events.
    /// </summary>
    public interface IDomainEvent {

        /// <summary>
        /// Gets the ID of the domain event.
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// Gets the date and time on which the domain event was created.
        /// </summary>
        DateTime TimeStamp { get; }
    }
}