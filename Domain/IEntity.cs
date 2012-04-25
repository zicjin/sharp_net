using System;

namespace zic_dotnet.Domain
{
    /// <summary>
    /// Represents that the implemented classes are domain entities.
    /// </summary>
    public interface IEntity
    {
        #region Properties
        /// <summary>
        /// Gets the ID of the entity.
        /// </summary>
        Guid ID { get; }
        #endregion
    }
}
