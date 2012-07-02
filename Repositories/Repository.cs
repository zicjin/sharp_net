using System;
using System.Collections.Generic;
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

        protected virtual IEnumerable<TAggregateRoot> DoFindAll() {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified);
        }
        protected virtual IEnumerable<TAggregateRoot> DoFindAll(int pageNumber, int pageSize) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified, pageNumber, pageSize);
        }

        protected virtual IEnumerable<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder);
        }
        protected virtual IEnumerable<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, pageNumber, pageSize);
        }

        protected virtual IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification) {
            return DoFindAll(specification, null, SortOrder.Unspecified);
        }
        protected virtual IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, int pageNumber, int pageSize) {
            return DoFindAll(specification, null, SortOrder.Unspecified, pageNumber, pageSize);
        }

        protected abstract IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder);
        protected abstract IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);

        //with eagerLoadingProperties
        protected abstract TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties);

        protected virtual IEnumerable<TAggregateRoot> DoFindAll(params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        protected virtual IEnumerable<TAggregateRoot> DoFindAll(int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), null, SortOrder.Unspecified, pageNumber, pageSize, eagerLoadingProperties);
        }

        protected virtual IEnumerable<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, eagerLoadingProperties);
        }
        protected virtual IEnumerable<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(new AnySpecification<TAggregateRoot>(), sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }

        protected virtual IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(specification, null, SortOrder.Unspecified, eagerLoadingProperties);
        }
        protected virtual IEnumerable<TAggregateRoot> DoFindAll(ISpecification<TAggregateRoot> specification, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return DoFindAll(specification, null, SortOrder.Unspecified, pageNumber, pageSize, eagerLoadingProperties);
        }

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

        public TAggregateRoot Find(ISpecification<TAggregateRoot> specification) {
            return this.DoFind(specification);
        }
        
        public IEnumerable<TAggregateRoot> FindAll() {
            return this.DoFindAll();
        }
        public IEnumerable<TAggregateRoot> FindAll(int pageNumber, int pageSize) {
            return this.DoFindAll(pageNumber, pageSize);
        }
        
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification) {
            return this.DoFindAll(specification);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, int pageNumber, int pageSize) {
            return this.DoFindAll(specification, pageNumber, pageSize);
        }

        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) {
            return this.DoFindAll(sortPredicate, sortOrder);
        }
        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            return this.DoFindAll(sortPredicate, sortOrder, pageNumber, pageSize);
        }

        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder) {
            return this.DoFindAll(specification, sortPredicate, sortOrder);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
        }

        public TAggregateRoot Find(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFind(specification, eagerLoadingProperties);
        }
        
        public IEnumerable<TAggregateRoot> FindAll(params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(pageNumber, pageSize, eagerLoadingProperties);
        }

        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(specification, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(specification, pageNumber, pageSize, eagerLoadingProperties);
        }

        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(sortPredicate, sortOrder, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(specification, sortPredicate, sortOrder, eagerLoadingProperties);
        }
        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<TAggregateRoot, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize, eagerLoadingProperties);
        }
        
        #endregion IRepository<TAggregateRoot> Members
    }
}