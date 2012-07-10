using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using zic_dotnet.Domain;
using zic_dotnet.Specifications;

namespace zic_dotnet.Repositories {

    public abstract class Repository<T> : IRepository<T> where T : class, IAggregateRoot {
        private readonly IRepositoryContext context;
        public Repository(IRepositoryContext context) {
            this.context = context;
        }

        protected abstract void DoAdd(T aggregateRoot);
        protected abstract bool DoExists(ISpecification<T> specification);
        protected abstract void DoRemove(T aggregateRoot);
        protected abstract void DoUpdate(T aggregateRoot);
        protected abstract T DoGetByKey(params object[] keyValues);

        protected abstract T DoFind(ISpecification<T> specification);
        protected abstract IEnumerable<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder);
        protected abstract IEnumerable<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        //eagerLoadingProperties
        protected abstract T DoFind(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        protected abstract IEnumerable<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        protected abstract IEnumerable<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties);
        
        #region IRepository<T> Members
        public IRepositoryContext Context {
            get { return this.context; }
        }

        public bool Exists(ISpecification<T> specification) {
            return this.DoExists(specification);
        }
        public void Add(T aggregateRoot) {
            this.DoAdd(aggregateRoot);
        }
        public void Remove(T aggregateRoot) {
            this.DoRemove(aggregateRoot);
        }
        public void Update(T aggregateRoot) {
            this.DoUpdate(aggregateRoot);
        }
        public T GetByKey(params object[] keyValues) {
            return this.DoGetByKey(keyValues);
        }
         
        //Find
        public T Find(ISpecification<T> specification) {
            return this.DoFind(specification);
        }
        public IEnumerable<T> FindAll() {
            return DoFindAll(new AnySpecification<T>(), null, SortOrder.Unspecified);
        }
        public IEnumerable<T> FindAll(ISpecification<T> specification) {
            return DoFindAll(specification, null, SortOrder.Unspecified);
        }
        public IEnumerable<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder) {
            return DoFindAll(new AnySpecification<T>(), sortPredicate, sortOrder);
        }
        public IEnumerable<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder) {
            return this.DoFindAll(specification, sortPredicate, sortOrder);
        }

        //page
        public IEnumerable<T> FindAll(int pageNumber, int pageSize) {
            return DoFindAll(new AnySpecification<T>(), null, SortOrder.Unspecified, pageNumber, pageSize);
        }
        public IEnumerable<T> FindAll(ISpecification<T> specification, int pageNumber, int pageSize) {
            return DoFindAll(specification, null, SortOrder.Unspecified, pageNumber, pageSize);
        }
        public IEnumerable<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            return DoFindAll(new AnySpecification<T>(), sortPredicate, sortOrder, pageNumber, pageSize);
        }
        public IEnumerable<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
        }

        //eagerLoadingProperties
        public T Find(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return this.DoFind(specification, eagerLoadingProperties);
        }
        public IEnumerable<T> FindAll(params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<T>(), null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        public IEnumerable<T> FindAll(int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<T>(), null, SortOrder.Unspecified, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<T> FindAll(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(specification, null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        public IEnumerable<T> FindAll(ISpecification<T> specification, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(specification, null, SortOrder.Unspecified, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<T>(), sortPredicate, sortOrder, eagerLoadingProperties);
        }
        public IEnumerable<T> FindAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<T>(), sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
        }
        public IEnumerable<T> FindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }

        //Get
        public T Get(ISpecification<T> specification) {
            T result = this.DoFind(specification);
            if (result == null)
                throw new ArgumentException("Aggregate not found.");
            return result;
        }
        public IEnumerable<T> GetAll() {
            return GetAll(new AnySpecification<T>(), null, SortOrder.Unspecified);
        }
        public IEnumerable<T> GetAll(int pageNumber, int pageSize) {
            return GetAll(new AnySpecification<T>(), null, SortOrder.Unspecified, pageNumber, pageSize);
        }
        public IEnumerable<T> GetAll(ISpecification<T> specification) {
            return GetAll(specification, null, SortOrder.Unspecified);
        }
        public IEnumerable<T> GetAll(ISpecification<T> specification, int pageNumber, int pageSize) {
            return GetAll(specification, null, SortOrder.Unspecified, pageNumber, pageSize);
        }
        public IEnumerable<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder) {
            return GetAll(new AnySpecification<T>(), sortPredicate, sortOrder);
        }
        public IEnumerable<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            return GetAll(new AnySpecification<T>(), sortPredicate, sortOrder, pageNumber, pageSize);
        }
        public IEnumerable<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder) {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder);
            if (results == null || results.Count() == 0)
                throw new ArgumentException("Aggregate not found.");
            return results;
        }
        public IEnumerable<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
            if (results == null || results.Count() == 0)
                throw new ArgumentException("Aggregate not found.");
            return results;
        }
        public T Get(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            T result = this.DoFind(specification, eagerLoadingProperties);
            if (result == null)
                throw new ArgumentException("Aggregate not found.");
            return result;
        }
        public IEnumerable<T> GetAll(params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return GetAll(new AnySpecification<T>(), null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        public IEnumerable<T> GetAll(int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return GetAll(new AnySpecification<T>(), null, SortOrder.Unspecified, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return GetAll(new AnySpecification<T>(), sortPredicate, sortOrder, eagerLoadingProperties);
        }
        public IEnumerable<T> GetAll(Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return GetAll(new AnySpecification<T>(), sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<T> GetAll(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return GetAll(specification, null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        public IEnumerable<T> GetAll(ISpecification<T> specification, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return GetAll(specification, null, SortOrder.Unspecified, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
            if (results == null || results.Count() == 0)
                throw new ArgumentException("Aggregate not found.");
            return results;
        }
        public IEnumerable<T> GetAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
            if (results == null || results.Count() == 0)
                throw new ArgumentException("Aggregate not found.");
            return results;
        }
        #endregion IRepository<T> Members
    }
}