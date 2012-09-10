using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using sharp_net.Repositories;
using sharp_net.Repositories.MongoDB;
using sharp_net.Specifications;

namespace sharp_net.Repositories.MongoDB {
    public class MongoDBRepository<T> : Repository<T> where T : class, IAggregateRoot {

        private readonly IMongoDBRepositoryContext mongoDBRepositoryContext;

        public MongoDBRepository(IRepositoryContext context)
            : base(context) {
            if (context is IMongoDBRepositoryContext)
                mongoDBRepositoryContext = context as MongoDBRepositoryContext;
            else
                throw new InvalidOperationException("Invalid repository context type.");
        }

        protected override void DoAdd(T aggregateRoot) {
            mongoDBRepositoryContext.RegisterNew(aggregateRoot);
        }
        protected override bool DoExists(ISpecification<T> specification) {
            return this.DoFind(specification) != null;
        }
        protected override void DoRemove(T aggregateRoot) {
            mongoDBRepositoryContext.RegisterDeleted(aggregateRoot);
        }
        protected override void DoUpdate(T aggregateRoot) {
            mongoDBRepositoryContext.RegisterModified(aggregateRoot);
        }
        protected override T DoGetByKey(params object[] keys) {
            MongoCollection collection = mongoDBRepositoryContext.GetCollectionForType(typeof(T));
            int id = (int)keys[0];
            return collection.AsQueryable<DomainInt>().Where(p => p.Id == id).First() as T;
        }

        protected override T DoFind(ISpecification<T> specification) {
            var collection = this.mongoDBRepositoryContext.GetCollectionForType(typeof(T));
            return collection.AsQueryable<T>().Where(specification.GetExpression()).FirstOrDefault();
        }
        protected override IEnumerable<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder) {
            var collection = this.mongoDBRepositoryContext.GetCollectionForType(typeof(T));
            var query = collection.AsQueryable<T>().Where(specification.GetExpression());
            if (sortPredicate != null) {
                switch (sortOrder) {
                    case SortOrder.Ascending:
                        return query.OrderBy(sortPredicate).ToList();
                    case SortOrder.Descending:
                        return query.OrderByDescending(sortPredicate).ToList();
                    default:
                        break;
                }
            }
            return query.ToList();
        }
        protected override IEnumerable<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize) {
            var collection = this.mongoDBRepositoryContext.GetCollectionForType(typeof(T));
            var query = collection.AsQueryable<T>().Where(specification.GetExpression());
            int skip = (pageNumber - 1) * pageSize;
            int take = pageSize;
            if (sortPredicate != null) {
                switch (sortOrder) {
                    case SortOrder.Ascending:
                        return query.OrderBy(sortPredicate).Skip(skip).Take(take).ToList();
                    case SortOrder.Descending:
                        return query.OrderByDescending(sortPredicate).Skip(skip).Take(take).ToList();
                    default:
                        break;
                }
            }
            return query.Skip(skip).Take(take).ToList();
        }

        protected override T DoFind(ISpecification<T> specification, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return this.DoFind(specification);
        }
        protected override IEnumerable<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(specification, sortPredicate, sortOrder);
        }
        protected override IEnumerable<T> DoFindAll(ISpecification<T> specification, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize, params Expression<Func<T, dynamic>>[] eagerLoadingProperties) {
            return this.DoFindAll(specification, sortPredicate, sortOrder, pageNumber, pageSize);
        }

    }
}
