using System;
using System.Collections.Generic;
using zic_dotnet.Domain;

namespace zic_dotnet.Repositories {

    public abstract class RepositoryContext : Disposable, IRepositoryContext {

        private readonly Guid id = Guid.NewGuid();
        [ThreadStatic]
        private bool committed = true;
        public IRepository<T> GetRepository<T>() where T : class, IAggregateRoot {
            return IocLocator.Instance.GetImple<IRepository<T>>(new { context = this });
        }
        protected void ClearRegistrations() {
            this.newCollection.Clear();
            this.modifiedCollection.Clear();
            this.deletedCollection.Clear();
        }
        
        [ThreadStatic]
        private readonly Dictionary<Guid, object> newCollection = new Dictionary<Guid, object>();
        [ThreadStatic]
        private readonly Dictionary<Guid, object> modifiedCollection = new Dictionary<Guid, object>();
        [ThreadStatic]
        private readonly Dictionary<Guid, object> deletedCollection = new Dictionary<Guid, object>();
        protected IEnumerable<KeyValuePair<Guid, object>> NewCollection {
            get { return newCollection; }
        }
        protected IEnumerable<KeyValuePair<Guid, object>> ModifiedCollection {
            get { return modifiedCollection; }
        }
        protected IEnumerable<KeyValuePair<Guid, object>> DeletedCollection {
            get { return deletedCollection; }
        }

        #region IRepositoryContext Members

        public Guid ID {
            get { return id; }
        }

        public virtual void RegisterNew<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot {
            if (obj.ID.Equals(Guid.Empty))
                throw new ArgumentException("The ID of the object is empty.", "obj");
            if (modifiedCollection.ContainsKey(obj.ID))
                throw new InvalidOperationException("The object cannot be registered as a new object since it was marked as modified.");
            if (newCollection.ContainsKey(obj.ID))
                throw new InvalidOperationException("The object has already been registered as a new object.");
            newCollection.Add(obj.ID, obj);
            committed = false;
        }

        public virtual void RegisterModified<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot {
            if (obj.ID.Equals(Guid.Empty))
                throw new ArgumentException("The ID of the object is empty.", "obj");
            if (deletedCollection.ContainsKey(obj.ID))
                throw new InvalidOperationException("The object cannot be registered as a modified object since it was marked as deleted.");
            if (!modifiedCollection.ContainsKey(obj.ID) && !newCollection.ContainsKey(obj.ID))
                modifiedCollection.Add(obj.ID, obj);
            committed = false;
        }

        public virtual void RegisterDeleted<TAggregateRoot>(TAggregateRoot obj) where TAggregateRoot : class, IAggregateRoot {
            if (obj.ID.Equals(Guid.Empty))
                throw new ArgumentException("The ID of the object is empty.", "obj");
            if (newCollection.ContainsKey(obj.ID)) {
                if (newCollection.Remove(obj.ID))
                    return;
            }
            bool removedFromModified = modifiedCollection.Remove(obj.ID);
            bool addedToDeleted = false;
            if (!deletedCollection.ContainsKey(obj.ID)) {
                deletedCollection.Add(obj.ID, obj);
                addedToDeleted = true;
            }
            committed = !(removedFromModified || addedToDeleted);
        }

        #endregion IRepositoryContext Members

        #region IUnitOfWork Members

        public bool Committed {
            get { return committed; }
            protected set { committed = value; }
        }

        public abstract void Commit();

        public abstract void Rollback();

        #endregion IUnitOfWork Members

    }
}