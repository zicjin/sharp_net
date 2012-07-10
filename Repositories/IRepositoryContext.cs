using System;
using zic_dotnet.Domain;

namespace zic_dotnet.Repositories {

    public interface IRepositoryContext : IUnitOfWork, IDisposable {

        Guid ID { get; }

        void RegisterNew<T>(T obj) where T : class, IAggregateRoot;

        void RegisterModified<T>(T obj) where T : class, IAggregateRoot;

        void RegisterDeleted<T>(T obj) where T : class, IAggregateRoot;

        IRepository<T> GetRepository<T>() where T : class, IAggregateRoot;
    }
}