using System;
using zic_dotnet.Domain;

namespace zic_dotnet.Repositories {

    public interface IRepositoryContext<TKey> : IUnitOfWork, IDisposable {

        Guid ID { get; }

        void RegisterNew<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot<TKey>;

        void RegisterModified<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot<TKey>;

        void RegisterDeleted<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot<TKey>;

        IRepository<TAggregateRoot, TKey> GetRepository<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot<TKey>;
    }
}