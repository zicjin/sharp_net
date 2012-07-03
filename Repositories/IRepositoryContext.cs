using System;
using zic_dotnet.Domain;

namespace zic_dotnet.Repositories {

    public interface IRepositoryContext : IUnitOfWork, IDisposable {

        Guid ID { get; }

        void RegisterNew<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot;

        void RegisterModified<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot;

        void RegisterDeleted<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot;

        IRepository<TAggregateRoot> GetRepository<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot;
    }
}