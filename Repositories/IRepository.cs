using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using zic_dotnet.Domain;
using zic_dotnet.Specifications;

namespace zic_dotnet.Repositories {

    public interface IRepository<T> where T : class, IAggregateRoot {

        IRepositoryContext Context { get; }

        void Add(T aggregateRoot);
        void Remove(T aggregateRoot);
        void Update(T aggregateRoot);
        bool Exists(ISpecification<T> specification);
        T GetById(params object[] keyValues);

        T Find(ISpecification<T> specification);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindAll(ISpecification<T> specification);
        IEnumerable<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder);
        IEnumerable<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder);

        #region page
        IEnumerable<T> FindAll(int pageNumber, int pageSize);
        IEnumerable<T> FindAll(ISpecification<T> specification, int pageNumber, int pageSize);
        IEnumerable<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        IEnumerable<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        #endregion

        #region eagerLoadingProperties
        T Find(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);

        IEnumerable<T> FindAll(params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> FindAll(int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);

        IEnumerable<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);

        IEnumerable<T> FindAll(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> FindAll(ISpecification<T> specification, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);

        IEnumerable<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        #endregion

        #region Get
        T Get(ISpecification<T> specification);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(int pageNumber, int pageSize);
        IEnumerable<T> GetAll(ISpecification<T> specification);
        IEnumerable<T> GetAll(ISpecification<T> specification, int pageNumber, int pageSize);
        IEnumerable<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder);
        IEnumerable<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        IEnumerable<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder);
        IEnumerable<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        T Get(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> GetAll(params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> GetAll(int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> GetAll(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> GetAll(ISpecification<T> specification, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        IEnumerable<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        #endregion
    }
}