using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net.Repositories {
    /// <summary>
    /// Represents the base class for all domain events.
    /// </summary>
    [Serializable]
    public abstract class DomainEvent : IDomainEvent {

        public DomainEvent() { }

        /// <summary>
        /// Returns the hash code for current domain event.
        /// </summary>
        public override int GetHashCode() {
            return RepoUtils.GetHashCode(this.SourceID.GetHashCode(),
                this.AssemblyQualifiedSourceType.GetHashCode(),
                this.Branch.GetHashCode(),
                this.Timestamp.GetHashCode(),
                this.Version.GetHashCode());
        }
        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value indicating whether this instance is equal to a specified
        /// entity.
        /// </summary>
        public override bool Equals(object obj) {
            if (obj == null)
                return false;
            DomainEvent domainEvent = obj as DomainEvent;
            if ((object)domainEvent == null)
                return false;
            return this.Equals((IAggregateRoot)domainEvent);
        }

        /// <summary>
        /// Gets or sets the identifier of the aggregate root.
        /// </summary>
        public virtual string SourceID { get; set; }
        /// <summary>
        /// Gets or sets the assembly qualified name of the type of the aggregate root.
        /// </summary>
        public virtual string AssemblyQualifiedSourceType { get; set; }
        /// <summary>
        /// Gets or sets the version of the domain event.
        /// </summary>
        public virtual long Version { get; set; }
        /// <summary>
        /// Gets or sets the branch on which the current version of domain event exists.
        /// </summary>
        public virtual long Branch { get; set; }
        /// <summary>
        /// Gets or sets the assembly qualified type name of the event.
        /// </summary>
        public virtual string AssemblyQualifiedEventType { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which the event was produced.
        /// </summary>
        /// <remarks>The format of this date/time value could be various between different
        /// systems. Apworks recommend system designer or architect uses the standard
        /// UTC date/time format.</remarks>
        public virtual DateTime Timestamp { get; set; }

    }
}
