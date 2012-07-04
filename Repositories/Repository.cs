using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using zic_dotnet.Domain;
using zic_dotnet.Specifications;

namespace zic_dotnet.Repositories {

    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot {
        private readonly IRepositoryContext context;
        public Repository(IRepositoryContext context) {
            this.context = context;
        }

        protected abstract void DoAdd(TAggregateRoot aggregateRoot);
        protected abstract bool DoExists(ISpecification<TAggregateRoot> specification);
        protected abstract void DoRemove(TAggregateRoot aggregateRoot);
        protected abstract void DoUpdate(TAggregateRoot aggregateRoot);
        protected abstract TAggregateRoot DoGetByKey(Guid key);

        protected abstract TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification);
        protected abstract IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder);
        protected abstract IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);
        //eagerLoadingProperties
        protected abstract TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        protected abstract IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        protected abstract IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);
        
        #region IRepository<TAggregateRoot> Members
        public IRepositoryContext Context {
            get { return this.context; }
        }

        public bool Exists(ISpecification<TAggregateRoot> specification) {
            return this.DoExists(specification);
        }
        public void Add(TAggregateRoot aggregateRoot) {
            this.DoAdd(aggregateRoot);
        }
        public void Remove(TAggregateRoot aggregateRoot) {
            this.DoRemove(aggregateRoot);
        }
        public void Update(TAggregateRoot aggregateRoot) {
            this.DoUpdate(aggregateRoot);
        }
        public TAggregateRoot GetByKey(Guid key) {
            return this.DoGetByKey(key);
        }
         
        //Find
        public TAggregateRoot Find(ISpecification<TAggregateRoot> specification) {
            return this.DoFind(specification);
        }
        public IEnumerable<TAggregateRoot> FindAll() {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification) {
            return DoFindAll(specification, null, SortOrder.Unspecified);
        }
        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) {
            return this.DoFindAll(specification, sortPredicate, sortOrder);
        }

        //page
        public IEnumerable<TAggregateRoot> FindAll(int pageNumber, int pageSize) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified, pageNumber, pageSize);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, int pageNumber, int pageSize) {
            return DoFindAll(specification, null, SortOrder.Unspecified, pageNumber, pageSize);
        }
        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, pageNumber, pageSize);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
        }

        //eagerLoadingProperties
        public TAggregateRoot Find(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFind(specification, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(specification, null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(specification, null, SortOrder.Unspecified, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }

        //Get
        public TAggregateRoot Get(ISpecification<TAggregateRoot> specification) {
            TAggregateRoot result = this.DoFind(specification);
            if (result == null)
                throw new ArgumentException("Aggregate not found.");
            return result;
        }
        public IEnumerable<TAggregateRoot> GetAll() {
            return GetAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified);
        }
        public IEnumerable<TAggregateRoot> GetAll(int pageNumber, int pageSize) {
            return GetAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified, pageNumber, pageSize);
        }
        public IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification) {
            return GetAll(specification, null, SortOrder.Unspecified);
        }
        public IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, int pageNumber, int pageSize) {
            return GetAll(specification, null, SortOrder.Unspecified, pageNumber, pageSize);
        }
        public IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) {
            return GetAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder);
        }
        public IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            return GetAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, pageNumber, pageSize);
        }
        public IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder);
            if (results == null || results.Count() == 0)
                throw new ArgumentException("Aggregate not found.");
            return results;
        }
        public IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
            if (results == null || results.Count() == 0)
                throw new ArgumentException("Aggregate not found.");
            return results;
        }
        public TAggregateRoot Get(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            TAggregateRoot result = this.DoFind(specification, eagerLoadingProperties);
            if (result == null)
                throw new ArgumentException("Aggregate not found.");
            return result;
        }
        public IEnumerable<TAggregateRoot> GetAll(params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return GetAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> GetAll(int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return GetAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return GetAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> GetAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return GetAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return GetAll(specification, null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return GetAll(specification, null, SortOrder.Unspecified, pageNumber, pageSize, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
            if (results == null || results.Count() == 0)
                throw new ArgumentException("Aggregate not found.");
            return results;
        }
        public IEnumerable<TAggregateRoot> GetAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            var results = this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
            if (results == null || results.Count() == 0)
                throw new ArgumentException("Aggregate not found.");
            return results;
        }
        #endregion IRepository<TAggregateRoot> Members
    }
}