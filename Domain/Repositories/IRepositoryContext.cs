using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zic_dotnet.Domain.Repositories
{
    /// <summary>
    /// Represents that the implemented classes are repository contexts.
    /// </summary>
    public interface IRepositoryContext : IUnitOfWork, IDisposable
    {
        Guid ID { get; }
        
        void RegisterNew<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot;
        
        void RegisterModified<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot;
        
        void RegisterDeleted<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot;
    }
}
