using System;
using sharp_net.Domain;

namespace sharp_net.Repositories {

    public interface IRepositoryContext : IUnitOfWork, IDisposable {

        Guid ID { get; }

        void RegisterNew<T>(T obj) where T : class, IAggregateRoot;

        void RegisterModified<T>(T obj) where T : class, IAggregateRoot;

        void RegisterDeleted<T>(T obj) where T : class, IAggregateRoot;

        IRepository<T> GetRepository<T>() where T : class, IAggregateRoot;
    }
}