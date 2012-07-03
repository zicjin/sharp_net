using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using zic_dotnet.Domain;
using zic_dotnet.Specifications;

namespace zic_dotnet.Repositories {

    public interface IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot {

        IRepositoryContext Context { get; }

        void Add(TAggregateRoot aggregateRoot);
        void Remove(TAggregateRoot aggregateRoot);
        void Update(TAggregateRoot aggregateRoot);
        bool Exists(ISpecification<TAggregateRoot> specification);
        TAggregateRoot GetByKey(Guid key);


        TAggregateRoot Find(ISpecification<TAggregateRoot> specification);

        IEnumerable<TAggregateRoot> FindAll();
        IEnumerable<TAggregateRoot> FindAll(int pageNumber, int pageSize);

        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification);
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, int pageNumber, int pageSize);

        IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder);
        IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);

        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder);
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);


        //with eagerLoadingProperties
        TAggregateRoot Find(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);

        IEnumerable<TAggregateRoot> FindAll(params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        IEnumerable<TAggregateRoot> FindAll(int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);

        IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);

        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);

        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);

    }
}