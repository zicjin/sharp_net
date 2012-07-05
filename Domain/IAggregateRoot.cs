namespace zic_dotnet.Domain {

    /// <summary>
    /// Represents that the implemented classes are aggregate roots.
    /// </summary>
    public interface IAggregateRoot<TKey> : IEntity<TKey> {
    }
}